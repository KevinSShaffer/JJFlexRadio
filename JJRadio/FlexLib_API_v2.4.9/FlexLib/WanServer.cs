// ****************************************************************************
///*!	\file WanServer.cs
// *	\brief Communicates with and parses messages from the WAN Server
// *
// *	\copyright	Copyright 2012-2017 FlexRadio Systems.  All Rights Reserved.
// *				Unauthorized use, duplication or distribution of this software is
// *				strictly prohibited by law.
// *
// *	\date 2017-04-20
// *	\author Abed Haque AB5ED
// */
// ****************************************************************************

using Flex.UiWpfFramework.Mvvm;
using Flex.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Flex.Smoothlake.FlexLib
{
    public class WanServer : ObservableObject
    {
        public WanServer()
        {
            HostName = "smartlink.flexradio.com";
            HostPort = "443";
        }

        public string HostName { get; set; }
        public string HostPort { get; set; }        

        private SslClient _sslClient;

        private string _sslClientPublicIp;
        public string SslClientPublicIp
        {
            get { return _sslClientPublicIp; }
            set
            {
                _sslClientPublicIp = value;
                RaisePropertyChanged("SslClientPublicIp");
            }
        }
        private bool _isConnected = false;
        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                RaisePropertyChanged("IsConnected");
            }
        }

        public void Connect()
        {
            if (_sslClient == null)
            {
                _sslClient = new SslClient(HostName, HostPort, src_port: 0, start_ping_thread: true, validate_cert: true);
                if (!_sslClient.IsConnected)
                {
                    _sslClient = null;
                    return; // to be handled by the reconnect
                }

                _sslClient.MessageReceivedReady += _sslClient_MessageReceivedReady;
                _sslClient.Disconnected += _sslClient_Disconnected;
                _sslClient.StartReceiving();                    
                IsConnected = true;
            }           
        }

        public void Disconnect()
        {
            if (_sslClient == null)
                return;

            _sslClient.MessageReceivedReady -= _sslClient_MessageReceivedReady;
            _sslClient.Disconnected -= _sslClient_Disconnected;
            _sslClient.Disconnect();
            _sslClient = null;
            IsConnected = false;
        }

        private void _sslClient_Disconnected(object sender, EventArgs e)
        {
            Disconnect();      
        }

        private void _sslClient_MessageReceivedReady(string msg)
        {
            //WriteToBox("[TCP RX]: " + msg);
            //Debug.WriteLine("[TCP RX]: " + msg);
            try
            {
                ParseMessage(msg);
            }
            catch (Exception)
            {
                
            }
        }

        private void ParseMessage(string msg)
        {
            // radio list name=<> callsign=<> serial=<>|more radios|..."
            if (msg.StartsWith("radio list "))
            {
                ParseRadioListMessage(msg);
            }

            else if (msg.StartsWith("radio connect_ready"))
            {
                ParseRadioConnectReadyMessage(msg);
            }

            else if (msg.StartsWith("application info"))
            {
                ParseApplicationInfo(msg);
            }

            else if (msg.StartsWith("application registration_invalid"))
            {
                ParseRegistrationInvalid(msg);
            }

            else if (msg.StartsWith("application user_settings"))
            {
                ParseUserSettings(msg);
            }
            else if ( msg.StartsWith("radio test_connection"))
            {
                ParseTestConnectionResults(msg);
            }
            else
            {
                Debug.WriteLine("****************************************************************************************************************");
                Debug.WriteLine("Received unknown message:" + msg);
            }
        }

        private void ParseTestConnectionResults(string msg)
        {
            var words = msg.Split(' ');

            Dictionary<string, string> keyValuePairs = words.Skip(2)
                                                            .Select(value => value.Split('='))
                                                            .ToDictionary(pair => pair[0], pair => pair[1]);

            WanTestConnectionResults results = new WanTestConnectionResults();

            results.radio_serial = "";
            results.upnp_tcp_port_working = false;
            results.upnp_udp_port_working = false;
            results.forward_tcp_port_working = false;
            results.forward_udp_port_working = false;
            results.nat_supports_hole_punch = false;

            results.radio_serial = keyValuePairs["serial"];

            bool.TryParse(keyValuePairs["upnp_tcp_port_working"], out results.upnp_tcp_port_working);
            bool.TryParse(keyValuePairs["upnp_udp_port_working"], out results.upnp_udp_port_working);
            bool.TryParse(keyValuePairs["forward_tcp_port_working"], out results.forward_tcp_port_working);
            bool.TryParse(keyValuePairs["forward_udp_port_working"], out results.forward_udp_port_working);
            bool.TryParse(keyValuePairs["nat_supports_hole_punch"], out results.nat_supports_hole_punch);

            OnTestConnectionResultsReceived(results);
        }

        private void ParseRadioListMessage(string msg)
        {
            var radioMessages = msg.Substring("radio list ".Length).Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            List<Radio> wanRadioList = new List<Radio>();

            foreach (var message in radioMessages)
            {
                var words = message.Split(' ');
                Dictionary<string, string> keyValuePairs;
                keyValuePairs = words.Select(value => value.Split('=')).ToDictionary(pair => pair[0], pair => pair[1]);

                string nickname = keyValuePairs["radio_name"];
                string callsign = keyValuePairs["callsign"];
                string serial = keyValuePairs["serial"];
                string version = keyValuePairs["version"];
                string radioModel = keyValuePairs["model"];
                string status = keyValuePairs["status"];
                string inUseIp = keyValuePairs["inUseIp"];
                string inUseHost = keyValuePairs["inUseHost"];

                string lastSeen = keyValuePairs["last_seen"];
                string publicIpAddress = keyValuePairs["public_ip"];
                bool upnpSupported = keyValuePairs["upnp_supported"] == "1";

                int publicTlsPort = -1;
                int publicUdpPort = -1;
                int publicUpnpTlsPort = -1;
                int publicUpnpUdpPort = -1;

                int.TryParse(keyValuePairs["public_tls_port"], out publicTlsPort);
                int.TryParse(keyValuePairs["public_udp_port"], out publicUdpPort);

                string maxLicensedVersion = "";
                string radioLicenseId = "";

                if ( ! keyValuePairs.TryGetValue("max_licensed_version", out maxLicensedVersion) || maxLicensedVersion == "" )
                {
                    /* Default to V1 */
                    maxLicensedVersion = "v1";
                }

                bool requiresAdditionalLicense = true;
                string requiresAdditionalLicenseStr;
                if (keyValuePairs.TryGetValue("requires_additional_license", out requiresAdditionalLicenseStr))
                {
                    uint temp;
                    bool parse_success = uint.TryParse(requiresAdditionalLicenseStr, out temp);
                    if (!parse_success || temp > 1)
                    {
                        Debug.WriteLine("FlexLib::WanServer::ParseRadioListMessage: Invalid value (requires_additional_license=" + requiresAdditionalLicenseStr + ")");
                    }
                    else
                    {
                        requiresAdditionalLicense = Convert.ToBoolean(temp);
                    }
                }

                if ( ! keyValuePairs.TryGetValue("radio_license_id", out radioLicenseId) || radioLicenseId == "")
                {
                    /* Default to empty */
                    radioLicenseId = "";
                }

                if (upnpSupported)
                {
                    int.TryParse(keyValuePairs["public_upnp_tls_port"], out publicUpnpTlsPort);
                    int.TryParse(keyValuePairs["public_upnp_udp_port"], out publicUpnpUdpPort);
                }

                int publicTlsPortToUse = -1;
                int publicUdpPortToUse = -1;
                bool isPortForwardOn = false;

                // favor using the manually defined forwarded ports if they are defined
                if (publicTlsPort != -1 && publicUdpPort != -1)
                {
                    publicTlsPortToUse = publicTlsPort;
                    publicUdpPortToUse = publicUdpPort;
                    isPortForwardOn = true;
                }
                else if (upnpSupported)
                {
                    publicTlsPortToUse = publicUpnpTlsPort;
                    publicUdpPortToUse = publicUpnpUdpPort;
                    isPortForwardOn = false;
                }

                bool requiresHolePunch = false;

                if ( !upnpSupported && !isPortForwardOn )
                {
                    /* This will require extra negotiation that chooses
                     * a port for both sides to try
                     */
                     //TODO: We also need to check the NAT for preserve_ports coming from radio here
                     // if the NAT DOES NOT preserve ports then we can't do hole punch
                    requiresHolePunch = true;
                }

                // Get reference to radio from flexlib
                // Add radioviewmodel, 
                // Fire Radio added event..., radio chooser will catch it
                ulong versionUlong = 0;
                FlexVersion.TryParse(version, out versionUlong);

                Radio radio = new Radio(isWan: true)
                {
                    Nickname = nickname,
                    Callsign = callsign,
                    Serial = serial,
                    Model = radioModel,
                    Status = status,
                    InUseIP = inUseIp,
                    InUseHost = inUseHost,
                    PublicTlsPort = publicTlsPortToUse,
                    PublicUdpPort = publicUdpPortToUse,
                    IsPortForwardOn = isPortForwardOn,
                    Version = versionUlong,
                    RequiresHolePunch = requiresHolePunch,
                    NegotiatedHolePunchPort = -1, // This is invalid until negotiated
                    MaxLicensedVersion = maxLicensedVersion,
                    RequiresAdditionalLicense = requiresAdditionalLicense,
                    RadioLicenseId = radioLicenseId,
                    LowBandwidthConnect = false // This is defaulted unless the connect is specified later 
                };
                 

                IPAddress ip;
                bool b = IPAddress.TryParse(publicIpAddress, out ip);
                if (!b)
                {
                    Debug.WriteLine("FlexLib::WanServer::ParseRadioListMessage: Invalid IPAddress");
                }
                else
                {
                    radio.IP = ip;
                }

                wanRadioList.Add(radio);
            }

            OnWanRadioListReceived(wanRadioList);
        }
        
        private void ParseRadioConnectReadyMessage(string msg)
        {
            var words = msg.Split(' ');

            Dictionary<string, string> keyValuePairs = words.Skip(2)
                                                            .Select(value => value.Split('='))
                                                            .ToDictionary(pair => pair[0], pair => pair[1]);

            string handle = keyValuePairs["handle"];
            string serial = keyValuePairs["serial"];

            OnWanRadioConnectReady(handle, serial);
        }

        private void ParseApplicationInfo(string msg)
        {
            var words = msg.Split(' ');

            Dictionary<string, string> keyValuePairs = words.Skip(2)
                                                            .Select(value => value.Split('='))
                                                            .ToDictionary(pair => pair[0], pair => pair[1]);

            SslClientPublicIp = keyValuePairs["public_ip"];
        }

        private void ParseUserSettings(string msg)
        {
            // application user_settings callsign=<> ...
            var words = msg.Split(' ');

            Dictionary<string, string> keyValuePairs = words.Skip(2)
                                                            .Select(value => value.Split('='))
                                                            .ToDictionary(pair => pair[0], pair => pair[1]);

            string callsign = keyValuePairs["callsign"];
            string firstName = keyValuePairs["first_name"];
            string lastName = keyValuePairs["last_name"];

            WanUserSettings newUserSettings = new WanUserSettings()
            {
                Callsign = callsign,
                FirstName = firstName,
                LastName = lastName
            };

            UserSettings = newUserSettings;
        }
        
        private void ParseRegistrationInvalid(string msg)
        {
            // If the registration was invlid (due to invalid token), then ask the user to
            // reenter credentials

            OnWanApplicationRegistrationInvalid();
        }

        private WanUserSettings _userSettings;

        public WanUserSettings UserSettings
        {
            get { return _userSettings; }
            private set
            {
                _userSettings = value;
                RaisePropertyChanged("UserSettings");
            }
        }

        private int _radioPublicUdpPort;

        public int RadioPublicUdpPort
        {
            get { return _radioPublicUdpPort; }
            set { _radioPublicUdpPort = value; }
        }

        private string _radioPublicIpAddress;

        public string RadioPublicIpAddress
        {
            get { return _radioPublicIpAddress; }
            set { _radioPublicIpAddress = value; }
        }

        public void SendRegisterApplicationMessageToServer(string appName, string platform, string token)
        {
            if ( _sslClient == null || _sslClient.IsConnected == false )
            {
                Debug.WriteLine("SendRegisterApplicationMessageToServer(): Not connected");
                return;
            }

            string command = "application register name=" + appName + " platform=" + platform + " token=" + token;
            if (_sslClient != null) _sslClient.Write(command);
        }

        public void SendConnectMessageToRadio(string radioSerial, int HolePunchPort = 0)
        {
            if (_sslClient == null || _sslClient.IsConnected == false)
            {
                Debug.WriteLine("SendConnectMessageToRadio: Not connected");
                return;
            }

            string command = "application connect serial=" + radioSerial + " hole_punch_port=" + HolePunchPort;

            if (_sslClient != null) _sslClient.Write(command);
        }

        public void SendDisconnectUsersMessageToServer(string radioSerial)
        {
            if (_sslClient == null || _sslClient.IsConnected == false)
            {
                Debug.WriteLine("SendDisconnectUsersMessageToServer(): Not connected");
                return;
            }

            string command = "application disconnect_users serial=" + radioSerial;
            if (_sslClient != null) _sslClient.Write(command);
        }

        public void SendRefreshLicenseMessageToServer(string radioSerial)
        {
            // Not yet implemented
        }

        public void SendSetUserInfoToServer(string token, WanUserSettings userSettings)
        {
            if (_sslClient == null || _sslClient.IsConnected == false)
            {
                Debug.WriteLine("SendSetUserInfoToServer(): Not connected");
                return;
            }

            string command = "application set_user_info owner_token=" + token + 
                                                        " callsign=" + userSettings.Callsign +
                                                        " first_name=" + userSettings.FirstName +
                                                        " last_name=" + userSettings.LastName;

            if (_sslClient != null) _sslClient.Write(command);
        }

        public delegate void WanRadioConnectReadyEventHandler(string wan_connectionhandle, string serial);
        public event WanRadioConnectReadyEventHandler WanRadioConnectReady;

        private void OnWanRadioConnectReady(string wan_connectionhandle, string serial)
        {
            if (WanRadioConnectReady == null) return;
            WanRadioConnectReady(wan_connectionhandle, serial);
        }

        public delegate void WanApplicationRegistrationInvalidEventHandler();
        public event WanApplicationRegistrationInvalidEventHandler WanApplicationRegistrationInvalid;

        private void OnWanApplicationRegistrationInvalid()
        {
            if (WanApplicationRegistrationInvalid == null) return;
            WanApplicationRegistrationInvalid();
        }

        public delegate void WanRadioRadioListRecievedEventHandler(List<Radio> radios);

        public static event WanRadioRadioListRecievedEventHandler WanRadioRadioListRecieved;

        public static void OnWanRadioListReceived(List<Radio> radios)
        {
            if (WanRadioRadioListRecieved == null) return;
            WanRadioRadioListRecieved(radios);
        }

        public delegate void TestConnectionResultsReceivedEventHandler(WanTestConnectionResults results);
        public event TestConnectionResultsReceivedEventHandler TestConnectionResultsReceived;
        public void OnTestConnectionResultsReceived(WanTestConnectionResults results)
        {
            if (TestConnectionResultsReceived == null) return;
            TestConnectionResultsReceived(results);
        }

        public void SendTestConnection(string serial)
        {
            if (_sslClient == null || _sslClient.IsConnected == false)
            {
                Debug.WriteLine("SendTestConnection(): Not connected");
                return;
            }

            string command = "application test_connection serial=" + serial;

            if (_sslClient != null) _sslClient.Write(command);


        }
    }
}
