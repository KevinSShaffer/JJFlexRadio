// "Profile" is defined with Conditional Compilation in properties.
//#define feedback // for testing mic input
//#define opusToFile
//#define opusInputToFile
//#define TwoSlices
//#define NoATU
#define CWMonitor
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Flex.Smoothlake.FlexLib;
using Flex.Smoothlake.Vita;
using HamBands;
using JJPortaudio;
using JJTrace;

namespace Radios
{
    /// <summary>
    /// Flex superclass
    /// </summary>
    public class FlexBase : IDisposable
    {
        private const string statusHdr = "Status";
        private const string importedMsg = "Import complete";
        private const string importFailMsg = "import didn't complete";
        private const string noRXAnt = "no RX antenna";
        private const string noSlice = "didn't get a slice";
        private const string noStation = "Station name not set";

        /// <summary>
        /// Data describing a rig.
        /// </summary>
        public class RigData
        {
            public string Name;
            public string ModelName;
            public string Serial;
            public bool Remote { get; internal set; }
            internal RigData() { }
        }
        public delegate void RadioFoundDel(object sender, RigData r);
        /// <summary>
        /// Radio found event, local or remote.
        /// </summary>
        public static event RadioFoundDel RadioFound;
        /// <summary>
        /// Raise RadioFound.
        /// </summary>
        /// <param name="sender">sending object, or null.</param>
        /// <param name="r">Radio object</param>
        internal static void RaiseRadioFound(object sender, RigData r)
        {
            if (RadioFound != null)
            {
                Tracing.TraceLine("RaiseRadioFound:" + r.Serial, TraceLevel.Info);
                RadioFound(sender, r);
            }
        }

        private List<Radio> myRadioList = new List<Radio>();
        private void radioAddedHandler(Radio r)
        {
            Tracing.TraceLine("radioAddedHandler:" + r.Serial, TraceLevel.Info);
            myRadioList.Add(r);
            RigData rd = new RigData();
            rd.Name = r.Nickname;
            rd.ModelName = r.Model;
            rd.Serial = r.Serial;
            rd.Remote = r.IsWan;
            RaiseRadioFound(null, rd);
        }
        internal static bool _apiInit;
        internal void apiInit(bool force = false)
        {
            Tracing.TraceLine("apiInit:" + force.ToString(), TraceLevel.Info);
            if (force)
            {
                // Always initialize.
                if (_apiInit)
                {
                    API.CloseSession();
                    // Force init.
                    _apiInit = false;
                }
            }
            // Won't init if !force and already inited.
            if (!_apiInit)
            {
                API.RadioAdded -= radioAddedHandler;
                API.RadioAdded += radioAddedHandler;
                API.Init();
                _apiInit = true;
            }
        }

        private Radio findRadioInAPI(string serial)
        {
            foreach (Radio r in myRadioList)
            {
                if (r.Serial == serial) return r;
            }
            return null;
        }

        /// <summary>
        /// Provide a list of local radios through the RadioFound event.
        /// </summary>
        public void LocalRadios()
        {
            Tracing.TraceLine("LocalRadios", TraceLevel.Info);
            apiInit(true);
        }

        /// <summary>
        /// Provide a list of remote radios through the RadioFound event.
        /// </summary>
        public void RemoteRadios()
        {
            Tracing.TraceLine("RemoteRadios", TraceLevel.Info);
            apiInit(); // don't force the init.
            bool stat = setupRemote();
            Tracing.TraceLine("RemoteRadios setupRemote:" + stat.ToString(), TraceLevel.Info);
        }

        internal Radio theRadio;
        private Thread mainThread;
        /// <summary>
        /// Connect to the specified radio.
        /// </summary>
        /// <param name="serial">serial#</param>
        /// <param name="lowBW">true if low bandwidth connect</param>
        public bool Connect(string serial, bool lowBW)
        {
            Tracing.TraceLine("Connect:" + serial, TraceLevel.Info);
            bool rv;

            theRadio = findRadioInAPI(serial);
            if (theRadio == null)
            {
                Tracing.TraceLine("Connect didn't find radio", TraceLevel.Error);
                return false;
            }

            // add the handlers.
            theRadio.PropertyChanged += new PropertyChangedEventHandler(radioPropertyChangedHandler);
            theRadio.MessageReceived += new Radio.MessageReceivedEventHandler(messageReceivedHandler);
            theRadio.GUIClientAdded += new Radio.GUIClientAddedEventHandler(guiClientAdded);
            theRadio.GUIClientUpdated += new Radio.GUIClientUpdatedEventHandler(guiClientUpdated);
            theRadio.GUIClientRemoved += new Radio.GUIClientRemovedEventHandler(guiClientRemoved);
            theRadio.SliceAdded += new Radio.SliceAddedEventHandler(sliceAdded);
            theRadio.SliceRemoved += new Radio.SliceRemovedEventHandler(sliceRemoved);
            theRadio.PanadapterAdded += new Radio.PanadapterAddedEventHandler(panadapterAdded);
            theRadio.PanadapterRemoved += new Radio.PanadapterRemovedEventHandler(panAdapterRemoved);
            theRadio.WaterfallRemoved += new Radio.WaterfallRemovedEventHandler(waterfallRemoved);
            theRadio.TNFAdded += new Radio.TNFAddedEventHandler(tnfAdded);
            theRadio.TNFRemoved += new Radio.TNFRemovedEventHandler(tnfRemoved);
            theRadio.IsTNFSubscribed = true; // v2.0.19
            theRadio.TNFEnabled = true;
            theRadio.ForwardPowerDataReady += new Radio.MeterDataReadyEventHandler(forwardPowerData);
            theRadio.SWRDataReady += new Radio.MeterDataReadyEventHandler(sWRData);
            theRadio.MicDataReady += new Radio.MeterDataReadyEventHandler(micData);
            theRadio.MicPeakDataReady += new Radio.MeterDataReadyEventHandler(micPeakData);
            theRadio.CompPeakDataReady += new Radio.MeterDataReadyEventHandler(compPeakData);
            theRadio.PATempDataReady += new Radio.MeterDataReadyEventHandler(PATempDataHandler);
            theRadio.VoltsDataReady += new Radio.MeterDataReadyEventHandler(VoltsDataHandler);
            theRadio.HWAlcDataReady += new Radio.MeterDataReadyEventHandler(hwALCData);
            theRadio.TxBandSettingsAdded += new Radio.TxBandSettingsAddedEventHandler(txBandSettingsHandler);
            theRadio.DAXRXAudioStreamAdded += new Radio.DAXRXAudioStreamAddedEventHandler(audioStreamAddedHandler);
            theRadio.DAXTXAudioStreamAdded += new Radio.DAXTXAudioStreamAddedEventHandler(txAudioStreamAddedHandler);
            theRadio.RXRemoteAudioStreamAdded += new Radio.RXRemoteAudioStreamAddedEventHandler(opusOutputStreamAddedHandler);
            theRadio.TXRemoteAudioStreamAdded += new Radio.TXRemoteAudioStreamAddedEventHandler(opusInputStreamAddedHandler);

            theRadio.LowBandwidthConnect = lowBW;

            if (theRadio.IsWan) sendRemoteConnect(theRadio);
            //theRadio.RequiresHolePunch = false;
            rv = theRadio.Connect();
            if (rv)
            {
                Tracing.TraceLine("Connect worked:" + theRadio.Serial, TraceLevel.Info);
            }
            else
            {
                Tracing.TraceLine("Connect failed", TraceLevel.Error);
            }
            return rv;
        }

        /// <summary>
        /// Start radio activity
        /// </summary>
        public void Start()
        {
            mainThread = new Thread(mainThreadProc);
            mainThread.Name = "mainThread";
            mainThread.Start();
            Thread.Sleep(0);
        }

        internal bool Disconnecting = false;
        /// <summary>
        /// Disconnect from the connected radio.
        /// Also disconnects from the wan if appropriate.
        /// </summary>
        public void Disconnect()
        {
            Tracing.TraceLine("Disconnect:" + (string)((theRadio == null) ? "null" : theRadio.Serial), TraceLevel.Info);
            if (theRadio == null) return;

            try
            {
                Disconnecting = true;
                if ((mainThread != null) && mainThread.IsAlive)
                {
                    // Stop the main thread.
                    Tracing.TraceLine("Disconnect:stopping main thread", TraceLevel.Info);
                    stopMainThread = true;
                    if (await(() => { return !mainThread.IsAlive; }, 30000))
                    {
                        Tracing.TraceLine("flex close:main thread stopped", TraceLevel.Info);
                    }
                    else Tracing.TraceLine("flex close:main thread didn't stop", TraceLevel.Error);
                }
            }
            catch (Exception ex)
            {
                Tracing.TraceLine("Dispose:mainThread:" + ex.Message, TraceLevel.Error);
            }
            mainThread = null;

            if (theRadio.Connected)
            {
                bool remote = theRadio.IsWan;
                stopRemoteAudioThread();
                theRadio.Disconnect();
                if (remote)
                {
                    wan.Disconnect();
                    wan = null;
                }
                theRadio = null;
            }
        }

        /// <summary>
        /// True if connected.
        /// </summary>
        public bool IsConnected
        {
            get { return (theRadio == null) ? false : theRadio.Connected; }
        }

        /// <summary>
        /// Reboot the radio
        /// </summary>
        /// <param name="disconnect">true to disconnect first</param>
        public void Reboot(bool disconnect = false)
        {
            Tracing.TraceLine("Reboot:" + disconnect.ToString(), TraceLevel.Info);
            if (theRadio != null)
            {
                Radio r = theRadio;
                if (disconnect & IsConnected) Disconnect();
                r.RebootRadio();
            }
        }

        /// <summary>
        /// Clear the web cache.
        /// </summary>
        public void ClearWebCache()
        {
            Tracing.TraceLine("ClearWebCache", TraceLevel.Info);
            WebBrowserHelper.ClearCache();
        }

        // WAN routines.
        #region WAN
        private List<Radio> radios;
        private bool wanListReceived;
        private void wanRadioListReceivedHandler(List<Radio> lst)
        {
            Tracing.TraceLine("wanRadioListReceivedHandler:" + lst.Count, TraceLevel.Info);
            radios = lst;
            wanListReceived = true;
            foreach (Radio r in lst)
            {
                Radio oldRadio = findRadioInAPI(r.Serial);
                if (oldRadio == null) API.OnRadioAddedEventHandler(r);
                else
                {
                    UpdateRadioDiscoveryFields(r, oldRadio);
                }
            }
        }
        private void UpdateRadioDiscoveryFields(Radio newRadio, Radio oldRadio)
        {
            if (oldRadio.Nickname != newRadio.Nickname)
                oldRadio.Nickname = newRadio.Nickname;
            if (oldRadio.Callsign != newRadio.Callsign)
                oldRadio.Callsign = newRadio.Callsign;
            if (oldRadio.Status != newRadio.Status)
                oldRadio.Status = newRadio.Status;
            if (oldRadio.GuiClientIPs != newRadio.GuiClientIPs)
                oldRadio.GuiClientIPs = newRadio.GuiClientIPs;
            if (oldRadio.GuiClientHosts != newRadio.GuiClientHosts)
                oldRadio.GuiClientHosts = newRadio.GuiClientHosts;
            if (oldRadio.PublicTlsPort != newRadio.PublicTlsPort)
                oldRadio.PublicTlsPort = newRadio.PublicTlsPort;
            if (oldRadio.PublicUdpPort != newRadio.PublicUdpPort)
                oldRadio.PublicUdpPort = newRadio.PublicUdpPort;
            if (oldRadio.IsPortForwardOn != newRadio.IsPortForwardOn)
                oldRadio.IsPortForwardOn = newRadio.IsPortForwardOn;
            if (oldRadio.Version != newRadio.Version)
                oldRadio.Version = newRadio.Version;
            if (oldRadio.RequiresHolePunch != newRadio.RequiresHolePunch)
                oldRadio.RequiresHolePunch = newRadio.RequiresHolePunch;
            if (oldRadio.NegotiatedHolePunchPort != newRadio.NegotiatedHolePunchPort)
                oldRadio.NegotiatedHolePunchPort = newRadio.NegotiatedHolePunchPort;
            if (oldRadio.MaxLicensedVersion != newRadio.MaxLicensedVersion)
                oldRadio.MaxLicensedVersion = newRadio.MaxLicensedVersion;
            if (oldRadio.RequiresAdditionalLicense != newRadio.RequiresAdditionalLicense)
                oldRadio.RequiresAdditionalLicense = newRadio.RequiresAdditionalLicense;
            if (oldRadio.RadioLicenseId != newRadio.RadioLicenseId)
                oldRadio.RadioLicenseId = newRadio.RadioLicenseId;
            if (oldRadio.LowBandwidthConnect != newRadio.LowBandwidthConnect)
                oldRadio.LowBandwidthConnect = newRadio.LowBandwidthConnect;
            oldRadio.UpdateGuiClientsList(newGuiClients: newRadio.GuiClients);
        }

        private WanServer wan;
        private string wanConnectionHandle;
        private bool WanRadioConnectReadyReceived = false;
        private void WanRadioConnectReadyHandler(string handle, string serial)
        {
            Tracing.TraceLine("WanRadioConnectReadyHandler:" + handle + ' ' + serial);
            wanConnectionHandle = handle;
            WanRadioConnectReadyReceived = true;
        }

        private string[] tokens;
        private bool setupRemote()
        {
            bool rv = false;

            // Bringup auth form.  Must be in an sta thread.
            tokens = null;
            Thread authThread = new Thread(authFormProc);
            authThread.Name = "authThread";
            authThread.SetApartmentState(ApartmentState.STA);
            authThread.Start();
            while (authThread.IsAlive) { Thread.Sleep(100); }
            if ((tokens == null) || (tokens.Length == 0))
            {
                Tracing.TraceLine("setup Remote: no tokens returned from form", TraceLevel.Error);
                goto setupRemoteDone;
            }

            // Get the jwt.
            string jwt = null;
            foreach (string keyVal in tokens)
            {
                string[] vals = keyVal.Split(new char[] { '=' });
                if (vals[0] == "id_token")
                {
                    jwt = vals[1];
                    break;
                }
            }
            if (jwt == null)
            {
                Tracing.TraceLine("setupRemote: no jwt", TraceLevel.Error);
                goto setupRemoteDone;
            }

            try
            {
                if (wan != null)
                {
                    Tracing.TraceLine("setupRemote:wan was setup, disconnecting", TraceLevel.Info);
                    wan.Disconnect();
                    Thread.Sleep(1000);
                }

                wan = new WanServer();
                wan.WanRadioConnectReady += new WanServer.WanRadioConnectReadyEventHandler(WanRadioConnectReadyHandler);

                wan.Connect();
                if (!wan.IsConnected)
                {
                    Tracing.TraceLine("setupRemote: not connected!", TraceLevel.Error);
                    goto setupRemoteDone;
                }

                Tracing.TraceLine("setupRemote: SendRegisterApplicationMessageToServer: " + API.ProgramName + ' ' + "Win10" + ' ' + jwt, TraceLevel.Info);
                WanServer.WanRadioRadioListRecieved += new WanServer.WanRadioRadioListRecievedEventHandler(wanRadioListReceivedHandler);
                wanListReceived = false;
                wan.SendRegisterApplicationMessageToServer(API.ProgramName, "Win10", jwt);
                if (!await(() => { return wanListReceived; }, 5000))
                {
                    Tracing.TraceLine("setupRemote: no radios found.", TraceLevel.Error);
                    goto setupRemoteDone;
                }

                if (radios.Count == 0)
                {
                    Tracing.TraceLine("setupRemote: no radios found", TraceLevel.Error);
                    goto setupRemoteDone;
                }

                rv = true;
            }
            catch (Exception ex)
            {
                Tracing.TraceLine("setupRemote: exception in setupThreadProc: " + ex.Message, TraceLevel.Error);
            }

            setupRemoteDone:
            return rv;
        }

        private void authFormProc()
        {
            AuthForm form = new AuthForm();
            ((Form)form).ShowDialog();
            tokens = form.Tokens;
            form.Dispose();
        }

        private void sendRemoteConnect(Radio r)
        {
            Tracing.TraceLine("sendRemoteConnect: " + r.Serial, TraceLevel.Info);
            WanRadioConnectReadyReceived = false;
            // WanRadioConnectReadyHandler already added.
            wan.SendConnectMessageToRadio(r.Serial, 0);
            if (!await(() => { return WanRadioConnectReadyReceived; }, 5000))
            {
                Tracing.TraceLine("sendRemoteConnect:Radio not ready for connect.", TraceLevel.Error);
                return;
            }
            r.WANConnectionHandle = wanConnectionHandle;
        }
        #endregion

        internal delegate bool awaitExp();
        /// <summary>
        /// Await the specified condition.
        /// </summary>
        /// <param name="exp">function that returns the condition</param>
        /// <param name="ms">milliseconds to wait.</param>
        /// <param name="interval">optional interval to check</param>
        /// <returns>true if condition met.</returns>
        internal static bool await(awaitExp exp, int ms, int interval)
        {
            int sanity = ms / interval;
            bool rv = false;
            while (sanity-- > 0)
            {
                rv = exp();
                if (rv) break;
                Thread.Sleep(interval);
            }
            return rv;
        }
        internal static bool await(awaitExp exp, int ms)
        {
            return await(exp, ms, 25);
        }

        // Implement Dispose().
        #region dispose
        private bool disposed = false;
        private Component component = new Component();
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Tracing.TraceLine("Dispose:" + disposing.ToString(), TraceLevel.Info);
            if (!disposed)
            {
                if (disposing)
                {
                    component.Dispose();
                }

                if (theRadio != null)
                {
                    Disconnect();
                }

                if (wan != null)
                {
                    wan.Disconnect();
                    wan = null;
                }

                if (_apiInit)
                {
                    _apiInit = false;
                    API.CloseSession();
                }

                if (RigFields != null)
                {
                    // The caller should have removed the user control from their form.
                    ((Flex6300Filters)RigFields.RigControl).Close(); // Remove int handlers
                    RigFields.Close();
                    RigFields = null;
                }

                disposed = true;
            }
        }

        ~FlexBase()
        {
            Dispose(false);
        }
        #endregion

        /// <summary>
        /// Off/On values for use by the rigs
        /// </summary>
        public enum OffOnValues
        {
            off,
            on
        }

        /// <summary>
        /// return the toggle of the OffOnValue
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Toggled OffOnValue</returns>
        public OffOnValues ToggleOffOn(OffOnValues value)
        {
            return (value == OffOnValues.on) ? OffOnValues.off : OffOnValues.on;
        }

        internal delegate void FreqChangeDel(object o);
        /// <summary>
        /// Called when RX frequency changes.
        /// Also called to copy a panadapter/waterfall.
        /// </summary>
        internal FreqChangeDel RXFreqChange = null;

        internal delegate void UpdateConfiguredTNFsDel(TNF tnf);
        internal UpdateConfiguredTNFsDel UpdateConfiguredTNFs = null;

        private bool globalProfileLoaded;
        private bool txProfileLoaded;
        private ATUTuneStatus originalATUStatus = ATUTuneStatus.None;
        private bool oldATUEnable = false; // false is the default, see Flex6300.
        private void radioPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            Tracing.TraceLine("propertyChanged:Radio:" + e.PropertyName, TraceLevel.Verbose);
            Radio r = (Radio)sender;
            switch (e.PropertyName)
            {
                case "ActiveSlice":
                    {
                        Slice s = r.ActiveSlice;
                        if ((s != null) && myClient(s.ClientHandle))
                        {
                            Tracing.TraceLine("ActiveSlice:mine " + SliceToVFO(s), TraceLevel.Info);
                            myActiveSlice = s;
                            _RXFrequency = LibFreqtoLong(s.Freq);
                            _RXMode = s.DemodMode;
                            _RXVFO = SliceToVFO(s);
                            s.Panadapter.GetRFGainInfo();
                        }
                        else
                        {
                            Tracing.TraceLine("ActiveSlice:none", TraceLevel.Error);
                        }
                    }
                    break;
                case "ATUEnabled":
                    {
                        Tracing.TraceLine("ATUEnabled:" + theRadio.ATUEnabled.ToString(), TraceLevel.Info);
                        if (oldATUEnable == r.ATUEnabled) return;
                        oldATUEnable = r.ATUEnabled;
                        bool wasEnabled = MyCaps.HasCap(RigCaps.Caps.ATGet);
#if !NoATU
                        if (r.ATUEnabled)
                        {
                            // indicate ATU capable.
                            MyCaps.getCaps = MyCaps.SetCap(MyCaps.getCaps, RigCaps.Caps.ATGet);
                            MyCaps.setCaps = MyCaps.SetCap(MyCaps.setCaps, RigCaps.Caps.ATSet);
                            MyCaps.getCaps = MyCaps.SetCap(MyCaps.getCaps, RigCaps.Caps.ATMems);
                            // Turn off the tuner if was bypassed.
                            // Note the bypass status might happen later.
                            if ((originalATUStatus == ATUTuneStatus.Bypass) |
                                (originalATUStatus == ATUTuneStatus.ManualBypass))
                            {
                                setFlexTunerTypeNotAuto();
                            }
                            else _FlexTunerType = FlexTunerTypes.auto;
                        }
                        else
#endif
                        {
                            // not atu capable.
                            MyCaps.getCaps = MyCaps.ResetCap(MyCaps.getCaps, RigCaps.Caps.ATGet);
                            MyCaps.setCaps = MyCaps.ResetCap(MyCaps.setCaps, RigCaps.Caps.ATSet);
                            MyCaps.getCaps = MyCaps.ResetCap(MyCaps.getCaps, RigCaps.Caps.ATMems);
                            setFlexTunerTypeNotAuto();
                        }
                        if (wasEnabled != MyCaps.HasCap(RigCaps.Caps.ATGet))
                        {
                            // enabled status changed.
                            raiseCapsChange(new CapsChangeArg(MyCaps));
                        }
                    }
                    break;
#if !NoATU
                case "ATUTuneStatus":
                    {
                        Tracing.TraceLine("ATUTuneStatus:" + theRadio.ATUTuneStatus.ToString(), TraceLevel.Info);
                        // set original status
                        if (originalATUStatus == ATUTuneStatus.None) originalATUStatus = r.ATUTuneStatus;
                        RaiseFlexAntTuneStartStop(new FlexAntTunerArg
                            (_FlexTunerType, r.ATUTuneStatus, SWR));
                        switch (theRadio.ATUTuneStatus)
                        {
                            case ATUTuneStatus.NotStarted:
                                // turn off tuning.
                                FlexTunerOn = false;
                                break;
                            case ATUTuneStatus.Aborted:
                                // turn off tuning.
                                FlexTunerOn = false;
                                break;
                            case ATUTuneStatus.InProgress:
                                // nothing to do here.
                                break;
                            case ATUTuneStatus.Bypass:
                                // stop tuning if tuning
                                if (_FlexTunerOn) FlexTunerOn = false;
                                // Turn off autoTune.
                                setFlexTunerTypeNotAuto();
                                break;
                            case ATUTuneStatus.ManualBypass:
                                // Nothing to do
                                break;
                            case ATUTuneStatus.Successful:
                            case ATUTuneStatus.OK:
                                FlexTunerOn = false;
                                break;
                            case ATUTuneStatus.Fail:
                                FlexTunerOn = false;
                                // bypass the tuner
                                theRadio.ATUTuneBypass(); // will get manualBypass status
                                                          // Turn autotune off
                                setFlexTunerTypeNotAuto();
                                break;
                            case ATUTuneStatus.FailBypass:
                                // nothing to do
                                break;
                        }
                    }
                    break;
#endif
                case "Connected":
                    {
                        Tracing.TraceLine("Connected:" + r.Connected.ToString(), TraceLevel.Error);
                    }
                    break;
                case "CWBreakIn":
                    Tracing.TraceLine("CWBreakIn:" + r.CWBreakIn.ToString(), TraceLevel.Info);
                    break;
                case "CWIambic":
                    {
                        Tracing.TraceLine("CWIambic:" + r.CWIambic.ToString(), TraceLevel.Info);
                        if (r.CWIambic)
                        {
                            _Keyer = (r.CWIambicModeA) ? IambicValues.iambicA : IambicValues.iambicB;
                        }
                        else _Keyer = IambicValues.off;
                    }
                    break;
                case "CWIambicModeA":
                    {
                        Tracing.TraceLine("CWIambicModeA:" + r.CWIambicModeA.ToString(), TraceLevel.Info);
                        if (r.CWIambic & r.CWIambicModeA) _Keyer = IambicValues.iambicA;
                    }
                    break;
                case "CWIambicModeB":
                    {
                        Tracing.TraceLine("CWIambicModeB:" + r.CWIambicModeB.ToString(), TraceLevel.Info);
                        if (r.CWIambic & r.CWIambicModeB) _Keyer = IambicValues.iambicB;
                    }
                    break;
                case "CWPitch":
                    {
                        Tracing.TraceLine("CWPitch:" + r.CWPitch, TraceLevel.Info);
                        if (useCWMon) CWMon.Frequency = (uint)r.CWPitch;
                    }
                    break;
                case "CWSpeed":
                    {
                        Tracing.TraceLine("CWSpeed:" + r.CWSpeed, TraceLevel.Info);
                        if (useCWMon) CWMon.Speed = (uint)r.CWSpeed;
                    }
                    break;
                case "CWSwapPaddles":
                    Tracing.TraceLine("CWSwapPaddles:" + r.CWSwapPaddles, TraceLevel.Info);
                    _CWReverse = r.CWSwapPaddles;
                    break;
                case "DatabaseImportComplete":
                    {
                        Tracing.TraceLine("DatabaseImportComplete:" + r.DatabaseImportComplete.ToString(), TraceLevel.Info);
                        if (r.DatabaseImportComplete)
                        {
                            q.Enqueue((FunctionDel)(() => { GetProfileInfo(true); }));
                        }
                    }
                    break;
                case "DAXOn":
                    {
                        Tracing.TraceLine("DAXOn:" + r.DAXOn.ToString(), TraceLevel.Info);
                        if ((_DAXOn == OffOnValues.on) & !r.DAXOn)
                        {
                            // We want it left on.
                            r.DAXOn = true;
                        }
                    }
                    break;
                case "InterlockState":
                    {
                        Tracing.TraceLine("InterlockState:" + r.InterlockState.ToString(), TraceLevel.Info);
                    }
                    break;
                case "Mox":
                    {
                        Tracing.TraceLine("Mox:" + r.Mox.ToString(), TraceLevel.Info);
                        bool oldTransmit = _Transmit;
                        _Transmit = r.Mox;
                        if (_Transmit != oldTransmit)
                        {
                            raiseTransmitChange(_Transmit);
                        }
                    }
                    break;
                case "PanadaptersRemaining":
                    {
                        Tracing.TraceLine("PanadaptersRemaining:" + r.PanadaptersRemaining, TraceLevel.Info);
                        // First one will be the total slices.
                        if (initialFreeSlices == -1)
                        {
                            initialFreeSlices = r.PanadaptersRemaining;
                        }
                    }
                    break;
                case "PersistenceLoaded":
                    Tracing.TraceLine("PersistenceLoaded:" + r.PersistenceLoaded.ToString(), TraceLevel.Info);
                    break;
#if Profile
                case "ProfileGlobalList":
                    {
                        globalProfileLoaded = (r.ProfileGlobalList.Contains(CurrentProfile));
                        string line = "";
                        foreach (string str in r.ProfileGlobalList)
                        {
                            line += str + " ";
                        }
                        Tracing.TraceLine("ProfileGlobalList:" + line, TraceLevel.Info);
                    }
                    break;
                case "ProfileGlobalSelection":
                    {
                        Tracing.TraceLine("ProfileGlobalSelection:" + r.ProfileGlobalSelection.ToString(), TraceLevel.Info);
                    }
                    break;
                case "ProfileTXList":
                    {
                        txProfileLoaded = (r.ProfileGlobalList.Contains(CurrentProfile));
                        string line = "";
                        foreach (string str in r.ProfileTXList)
                        {
                            line += str + " ";
                        }
                        Tracing.TraceLine("ProfileTXList:" + line, TraceLevel.Info);
                    }
                    break;
                case "ProfileTXSelection":
                    Tracing.TraceLine("ProfileTXSelection:" + r.ProfileTXSelection, TraceLevel.Info);
                    break;
#endif
                case "PTTSource":
                    Tracing.TraceLine("PTTSource:" + r.PTTSource.ToString(), TraceLevel.Info);
                    break;
                case "RFPower":
                    Tracing.TraceLine("RFPower:" + theRadio.RFPower, TraceLevel.Info);
                    _XmitPower = theRadio.RFPower;
                    break;
                case "SimpleVOXEnable":
                    Tracing.TraceLine("SimpleVOXEnable:" + r.SimpleVOXEnable.ToString(), TraceLevel.Info);
                    break;
                case "Status":
                    string status = theRadio.Status;
                    Tracing.TraceLine("radio status:" + status, TraceLevel.Info);
                    break;
                case "TransmitSlice":
                    {
                        Slice s = r.TransmitSlice;
                        if (s == null)
                        {
                            Tracing.TraceLine("TransmitSlice:null", TraceLevel.Info);
                            return;
                        }
                        if (myClient(s.ClientHandle))
                        {
                            Tracing.TraceLine("TransmitSlice:mine " + SliceToVFO(s), TraceLevel.Info);
                            //if (CanTransmit)
                            //_TXVFO = (TheGuiClient.IsLocalPtt) ? SliceToVFO(s) : noVFO;
                        }
                        else
                        {
                            Tracing.TraceLine("TransmitSlice:not mine", TraceLevel.Info);
                            //_TXVFO = noVFO;
                        }
                    }
                    break;
                case "TunePower":
                    Tracing.TraceLine("TunePower:" + r.TunePower, TraceLevel.Info);
                    _TunePower = r.TunePower;
                    break;
                case "TX1Enabled":
                    Tracing.TraceLine("TX1Enabled:" + r.TX1Enabled.ToString(), TraceLevel.Info);
                    // keep it enabled.
                    if (!r.TX1Enabled) q.Enqueue((FunctionDel)(() => { theRadio.TX1Enabled = true; }));
                    break;
                case "TXCWMonitorGain":
                    {
                        Tracing.TraceLine("TXCWMonitorGain:" + theRadio.TXCWMonitorGain, TraceLevel.Info);
#if CWMonitor
                        if (useCWMon) CWMon.Volume = theRadio.TXCWMonitorGain;
#endif
                    }
                    break;
                case "TXTune":
                    {
                        Tracing.TraceLine("TXTune:" + r.TXTune.ToString(), TraceLevel.Info);
                        if (r.TXTune)
                        {
                            // Report status if starting up.
                            ATUTuneStatus stat = ATUTuneStatus.InProgress;
                            RaiseFlexAntTuneStartStop(new FlexAntTunerArg
                                (FlexTunerType, stat, _SWR));
                        }
                    }
                    break;
            }
        }

        internal Slice myActiveSlice = null;
        private void slicePropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            Slice s = (Slice)sender;
            if (myClient(s.ClientHandle))
            {
                Tracing.TraceLine("propertyChanged:Slice:mine " + e.PropertyName, TraceLevel.Verbose);
                switch (e.PropertyName)
                {
                    case "Active":
                        {
                            Tracing.TraceLine("Active:slice " + s.Index.ToString() + " " + s.Active.ToString(), TraceLevel.Info);
                            if (s.Active)
                            {
                                myActiveSlice = s;
                                _RXFrequency = LibFreqtoLong(s.Freq);
                                _RXMode = s.DemodMode;
                                _RXVFO = SliceToVFO(s);
                                if (RXFreqChange != null) RXFreqChange(s);
#if CWMonitor
                                if (useCWMon && (s == VFOToSlice(TXVFO)) && (s.DemodMode == "CW"))
                                {
                                    CWMonStart(); // ok if already started.
                                }
#endif
                            }
                        }
                        break;
                    case "DemodMode":
                        {
                            Tracing.TraceLine("DemodMode:slice " + s.Index.ToString() + " " + s.DemodMode.ToString(), TraceLevel.Info);
                            if (s.Active) _RXMode = s.DemodMode;
                            if (s.IsTransmitSlice) _TXMode = s.DemodMode;
                            if (s.Active && (RXFreqChange != null)) RXFreqChange(s);
#if CWMonitor
                            try
                            {
                                if (useCWMon && (s == VFOToSlice(TXVFO)))
                                {
                                    if (s.DemodMode == "CW") CWMonStart();
                                    else CWMonStop();
                                }
                            }
                            catch { }
#endif
                        }
                        break;
                    case "Freq":
                        {
                            Tracing.TraceLine("Freq:slice " + s.Index.ToString() + " " + s.Freq.ToString(), TraceLevel.Verbose);
                            if (s.Active)
                            {
                                _RXFrequency = LibFreqtoLong(s.Freq);
                                if (RXFreqChange != null) RXFreqChange(s);
                            }
                            if (s.IsTransmitSlice) _TXFrequency = LibFreqtoLong(s.Freq);
                        }
                        break;
                    case "IsTransmitSlice":
                        {
                            Tracing.TraceLine("IsTransmit:slice " + s.Index.ToString() + " " + s.IsTransmitSlice.ToString(), TraceLevel.Info);
                            int vfo = SliceToVFO(s);
                            if (s.IsTransmitSlice)
                            {
                                if (CanTransmit)
                                {
                                    _TXVFO = vfo;
                                    _TXFrequency = LibFreqtoLong(s.Freq);
                                    _TXMode = s.DemodMode;
                                }
                            }
                        }
                        break;
                    case "Mute":
                        {
                            Tracing.TraceLine("slicePropertyChangedHandler:Mute slice:" + s.Index + ' ' + s.Mute.ToString(), TraceLevel.Info);
                            if (audioChannels != null)
                            {
                                audioChannelData chan = findAudioChannelBySlice(s);
                                if (s.Mute) stopAudioChannel(chan);
                                else startAudioChannel(chan);
                            }
                        }
                        break;
                    case "NBLevel":
                        {
                            Tracing.TraceLine("slice NBLevel:" + s.NBLevel.ToString(), TraceLevel.Info);
                            //s.Panadapter.NBLevel = s.NBLevel;
                        }
                        break;
                    case "NBOn":
                        {
                            Tracing.TraceLine("slice NBOn:" + s.NBOn.ToString(), TraceLevel.Info);
                            //s.Panadapter.NBOn = s.NBOn;
                        }
                        break;
                    case "RITOn":
                        {
                            Tracing.TraceLine("RITOn:" + s.RITOn.ToString(), TraceLevel.Info);
                            lock (_RIT)
                            {
                                _RIT.Active = s.RITOn;
                            }
                        }
                        break;
                    case "RITFreq":
                        {
                            Tracing.TraceLine("RITFreq:" + s.RITFreq.ToString(), TraceLevel.Info);
                            lock (_RIT)
                            {
                                _RIT.Value = s.RITFreq;
                            }
                        }
                        break;
                    case "XITOn":
                        {
                            Tracing.TraceLine("XITOn:" + s.XITOn.ToString(), TraceLevel.Info);
                            lock (_XIT)
                            {
                                _XIT.Active = s.XITOn;
                            }
                        }
                        break;
                    case "XITFreq":
                        {
                            Tracing.TraceLine("XITFreq:" + s.XITFreq.ToString(), TraceLevel.Info);
                            lock (_XIT)
                            {
                                _XIT.Value = s.XITFreq;
                            }
                        }
                        break;
#if zero
                    case "TXAntenna":
                        Tracing.TraceLine("TXAntenna:" + s.TXAnt, TraceLevel.Info);
                        // We always set the TXAnt for both slices, so we'll come through twice.
                        break;
#endif
                }
            }
            else
            {
                Tracing.TraceLine("propertyChanged:Slice:not mine " + e.PropertyName, TraceLevel.Info);
            }
        }

        private void panadapterPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            Panadapter p = (Panadapter)sender;
            if (myClient(p.ClientHandle))
            {
                Tracing.TraceLine("panPropertyChanged:mine " + e.PropertyName, TraceLevel.Verbose);
                switch (e.PropertyName)
                {
                    case "Bandwidth":
                        Tracing.TraceLine("Bandwidth:" + p.Bandwidth.ToString(), TraceLevel.Verbose);
                        break;
                    case "CenterFreq":
                        Tracing.TraceLine("CenterFreq:" + p.CenterFreq.ToString(), TraceLevel.Verbose);
                        break;
                    case "FPS":
                        Tracing.TraceLine("FPS:" + p.FPS.ToString(), TraceLevel.Info);
                        break;
                    case "HighDbm":
                        Tracing.TraceLine("HighDBM:" + p.HighDbm.ToString(), TraceLevel.Verbose);
                        break;
                    case "LowDbm":
                        Tracing.TraceLine("LowDbm:" + p.LowDbm.ToString(), TraceLevel.Verbose);
                        break;
                    case "Preamp":
                        Tracing.TraceLine("Preamp:" + p.Preamp, TraceLevel.Info);
                        break;
                    case "RFGain":
                        Tracing.TraceLine("panadapter RFGain:" + p.RFGain.ToString(), TraceLevel.Verbose);
                        //if (p == activePan) _PreAmp = (p.RFGain == PreAmpMax) ? OffOnValues.on : OffOnValues.off;
                        break;
                    case "RFGainLow":
                        Tracing.TraceLine("RFGainLow:" + p.RFGainLow, TraceLevel.Info);
                        RFGainMin = p.RFGainLow;
                        break;
                    case "RFGainHigh":
                        Tracing.TraceLine("RFGainHigh:" + p.RFGainHigh, TraceLevel.Info);
                        RFGainMax = p.RFGainHigh;
                        break;
                    case "RFGainStep":
                        Tracing.TraceLine("RFGainStep:" + p.RFGainStep, TraceLevel.Info);
                        RFGainIncrement = p.RFGainStep;
                        break;
                    case "RFGainMarkers":
                        {
                            string str = "";
                            foreach (int i in p.RFGainMarkers)
                            {
                                str += i.ToString() + ' ';
                            }
                            Tracing.TraceLine("RFGainMarkers:" + str, TraceLevel.Info);
                        }
                        break;
                }
            }
        }

        private void waterfallPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            Waterfall w = (Waterfall)sender;
            if (myClient(w.ClientHandle))
            {
                Tracing.TraceLine("waterfallPropertyChanged:mine " + e.PropertyName, TraceLevel.Verbose);
                switch (e.PropertyName)
                {
                    case "FallLineDurationMs":
                        Tracing.TraceLine("FallLineDurationMs:" + w.FallLineDurationMs.ToString(), TraceLevel.Info);
                        break;
                }
            }
        }

        private void tnfPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            // See FlexTNF.cs.
            TNF tnf = (TNF)sender;
            Tracing.TraceLine("propertyChanged:TNF:" + e.PropertyName, TraceLevel.Verbose);
            if (UpdateConfiguredTNFs != null) UpdateConfiguredTNFs(tnf);
        }

        private void daxRXAudioStreamPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            Tracing.TraceLine("propertyChanged:DAXRXAudioStream:" + e.PropertyName, TraceLevel.Info);
        }

        private void messageReceivedHandler(MessageSeverity severity, string message)
        {
            Tracing.TraceLine("message severity:" + severity.ToString() + " " + message, TraceLevel.Error);
        }

        private const uint noClient = 0xffffffff;
        private uint clientHandle = noClient;
        private void guiClientAdded(GUIClient client)
        {
            if (client == null) return;
            
            if (client.IsThisClient)
            {
                clientHandle = client.ClientHandle;
                lock (theRadio.GuiClientsLockObj)
                {
                    PrimaryStation = (theRadio.GuiClients.Count == 1);
                }
                //CanTransmit = PrimaryStation;
                CanTransmit = true;

                if (client.Station != Callouts.StationName)
                {
                    theRadio.SetClientStationName(Callouts.StationName);
                }
            }

            Tracing.TraceLine("guiClientAdded:" +
                "id:" + client.ClientID +
                " my client:" + client.IsThisClient.ToString() +
                " handle:" + client.ClientHandle +
                " program:" + client.Program +
                " station:" + client.Station +
                " is local PTT:" + client.IsLocalPtt.ToString() +
                " is available:" + client.IsAvailable.ToString() +
                " primary:" + PrimaryStation.ToString() +
                " CanTransmit:" + CanTransmit.ToString(), TraceLevel.Info);
        }

        private bool myClient(uint handle)
        {
            return ((clientHandle == handle)) ? true : false;
        }

        internal GUIClient TheGuiClient
        {
            get
            {
                GUIClient rv = theRadio.FindGUIClientByClientHandle(clientHandle);
                return rv;
            }
        }

        private void guiClientUpdated(GUIClient client)
        {
            if (client == null) return;

            if (myClient(client.ClientHandle))
            {
                if (!client.IsLocalPtt)
                {
                    Tracing.TraceLine("guiClientUpdated:no local ptt", TraceLevel.Info);
                    //TXVFO = noVFO;
                }
            }

            Tracing.TraceLine("guiClientUpdated:" +
                "id:" + client.ClientID +
                " my client:" + client.IsThisClient.ToString() +
                " handle:" + client.ClientHandle +
                " program:" + client.Program +
                " station:" + client.Station +
                " is local PTT:" + client.IsLocalPtt.ToString() +
                " is available:" + client.IsAvailable.ToString() +
                " primary:" + PrimaryStation.ToString() +
                " CanTransmit:" + CanTransmit.ToString(), TraceLevel.Info);
        }

        private void guiClientRemoved(GUIClient client)
        {
            if (client == null) return;

            if (myClient(client.ClientHandle))
            {
                Tracing.TraceLine("guiClientRemoved:my client", TraceLevel.Info);
                //myClient = null;
            }

            Tracing.TraceLine("guiClientRemoved:" +
                "id:" + client.ClientID +
                " my client:" + client.IsThisClient.ToString() +
                " handle:" + client.ClientHandle +
                " program:" + client.Program +
                " station:" + client.Station +
                " is local PTT:" + client.IsLocalPtt.ToString() +
                " is available:" + client.IsAvailable.ToString() +
                " primary:" + PrimaryStation.ToString() +
                " CanTransmit:" + CanTransmit.ToString(), TraceLevel.Info);
        }

        private bool mySliceAdded;
        private void sliceAdded(Slice slc)
        {
            if (myClient(slc.ClientHandle))
            {
                mySliceAdded = true;
                slc.PropertyChanged += new PropertyChangedEventHandler(slicePropertyChangedHandler);
                slc.MeterAdded += new Slice.MeterAddedEventHandler(meterAdded);
                sMeter_t sMeter = new sMeter_t(this, slc);
                slc.SMeterDataReady += sMeter.sMeterData;
                int ct;
                lock (mySlices)
                {
                    mySlices.Add(slc);
                    ct = mySlices.Count;
                }
                Tracing.TraceLine("sliceAdded:mine " + ct.ToString() + ':' + slc.ToString(), TraceLevel.Info);
                if (slc.IsTransmitSlice)
                {
                    Tracing.TraceLine("sliceAdded:IsTransmitSlice", TraceLevel.Info);
                    _TXVFO = SliceToVFO(slc);
                }
                if (slc.Active)
                {
                    Tracing.TraceLine("sliceAdded:activeSlice", TraceLevel.Info);
                    _RXVFO = SliceToVFO(slc);
                }
            }
            else Tracing.TraceLine("sliceAdded:not mine " + slc.ToString(), TraceLevel.Info);
        }

        private void sliceRemoved(Slice slc)
        {
            if (myClient(slc.ClientHandle))
            {
                int ct;
                lock (mySlices)
                {
                    mySlices.Remove(slc);
                    ct = mySlices.Count;
                }
                Tracing.TraceLine("sliceRemoved:mine, new count:" + ct.ToString() + ':' + slc.ToString(), TraceLevel.Info);
                // Note: The user can't remove the active or transmit slices.
            }
            else Tracing.TraceLine("sliceRemoved:not mine" + slc.ToString(), TraceLevel.Info);
        }

        internal delegate void PanSetupDel();
        /// <summary>
        /// Provided by the user control to setup the pan adapter.
        /// </summary>
        internal PanSetupDel PanSetup;
        internal Panadapter Panadapter
        {
            get
            {
                Panadapter rv = null;
                if (theRadio.ActiveSlice != null) rv = theRadio.ActiveSlice.Panadapter;
                return rv;
            }
        }
        internal List<Waterfall> waterfallList;
        internal Waterfall Waterfall
        {
            get
            {
                return GetPanadaptersWaterfall(Panadapter);
            }
        }
        internal Waterfall GetPanadaptersWaterfall(Panadapter pan)
        {
            Waterfall rv = null;
            if ((pan != null) && (waterfallList != null))
            {
                lock (waterfallList)
                {
                    foreach (Waterfall w in waterfallList)
                    {
                        if (w.StreamID == pan.ChildWaterfallStreamID)
                        {
                            rv = w;
                            break;
                        }
                    }
                }
            }
            return rv;
        }
        private List<Panadapter> myPanAdapters = new List<Panadapter>();
        private void panadapterAdded(Panadapter pan, Waterfall fall)
        {
            if (myClient(pan.ClientHandle))
            {
                if (waterfallList == null) waterfallList = new List<Waterfall>();
                lock (waterfallList)
                {
                    waterfallList.Add(fall);
                }
                pan.Width = 5000;
                pan.PropertyChanged += new PropertyChangedEventHandler(panadapterPropertyChangedHandler);
                int ct;
                lock (myPanAdapters)
                {
                    myPanAdapters.Add(pan);
                    ct = myPanAdapters.Count;
                }
                Tracing.TraceLine("panadapterAdded:mine " + ct.ToString() + ':' + pan.ToString(), TraceLevel.Info);
            }
            else Tracing.TraceLine("panadapterAdded:not mine " + pan.ToString(), TraceLevel.Info);
        }

        internal int MyNumPanadapters
        {
            get
            {
                lock (myPanAdapters)
                {
                    return myPanAdapters.Count;
                }
            }
        }

        private void panAdapterRemoved(Panadapter pan)
        {
            if (myClient(pan.ClientHandle))
            {
                int ct;
                lock (myPanAdapters)
                {
                    myPanAdapters.Remove(pan);
                    ct = myPanAdapters.Count;
                }
                Tracing.TraceLine("panadapterRemoved:new count:" + ct.ToString() + ':' + pan.ToString(), TraceLevel.Info);
            }
            else Tracing.TraceLine("panadapterRemoved:not mine", TraceLevel.Info);
        }
        private void waterfallRemoved(Waterfall fall)
        {
            Tracing.TraceLine("waterfallRemoved", TraceLevel.Info);
            lock (waterfallList)
            {
                if ((waterfallList != null) && waterfallList.Contains(fall))
                {
                    waterfallList.Remove(fall);
                }
            }
        }

        internal List<TNF> TNFs
        {
            get { return theRadio.TNFList; }
        }
        private void tnfAdded(TNF tnf)
        {
            Tracing.TraceLine("tnfAdded:" + tnf.ToString(), TraceLevel.Info);
            tnf.PropertyChanged += new PropertyChangedEventHandler(tnfPropertyChangedHandler);
            // No need to call the update, since created TNFs are not permanent.
        }
        private void tnfRemoved(TNF tnf)
        {
            Tracing.TraceLine("tnfRemove:" + tnf.ID.ToString(), TraceLevel.Info);
            // Don't call UpdateConfiguredTNFs here.
            //if ((UpdateConfiguredTNFs != null) & !Closing) UpdateConfiguredTNFs(tnf, true);
        }

        protected int DBmToPower(float dbm)
        {
            return (int)((Math.Pow(10d, (double)(dbm / 10)) / 1000) + 0.5);
        }
        protected float _PowerDBM;
        private void forwardPowerData(float data)
        {
            Tracing.TraceLine("forwardPower:" + data.ToString(), TraceLevel.Verbose);
            if (_PowerDBM != data)
            {
                _PowerDBM = data;
            }
        }

        protected float _SWR;
        internal float SWR { get { return _SWR; } }

        private void sWRData(float data)
        {
            Tracing.TraceLine("SWRData:" + data.ToString(), TraceLevel.Verbose);
            _SWR = data;
        }

        private string SWRText()
        {
            return _SWR.ToString("f1");
        }

        private void micData(float data)
        {
            Tracing.TraceLine("micData:" + data.ToString(), TraceLevel.Verbose);
        }

        internal float _MicPeakData;
        private void micPeakData(float data)
        {
            Tracing.TraceLine("micPeakData:" + data.ToString(), TraceLevel.Verbose);
            _MicPeakData = data;
        }

        private void compPeakData(float data)
        {
            Tracing.TraceLine("compPeakData:" + data.ToString(), TraceLevel.Verbose);
        }

        private void hwALCData(float data)
        {
            Tracing.TraceLine("hwALCData:" + data.ToString(), TraceLevel.Verbose);
        }

        private void txBandSettingsHandler(TxBandSettings settings)
        {
            Tracing.TraceLine("txBandSettingsHandler:" + settings.BandName, TraceLevel.Info);
        }

        private float _PATempData;
        private void PATempDataHandler(float data)
        {
            Tracing.TraceLine("PATempDataHandler:" + data.ToString(), TraceLevel.Verbose);
            _PATempData = data;
        }

        /// <summary>
        /// PA temperature in DGC.
        /// </summary>
        internal float PATemp { get { return _PATempData; } }

        private float _VoltsData;
        private void VoltsDataHandler(float data)
        {
            Tracing.TraceLine("VoltsDataHandler:" + data.ToString(), TraceLevel.Verbose);
            _VoltsData = data;
        }

        /// <summary>
        /// Voltage
        /// </summary>
        internal float Volts { get { return _VoltsData; } }

        private void meterAdded(Slice slc, Meter m)
        {
            Tracing.TraceLine("meterAdded:slice " + slc.Index.ToString() + ' ' + m.ToString(), TraceLevel.Info);
        }

        private void meterRemoved(Slice slc, Meter m)
        {
            Tracing.TraceLine("meterRemoved:" + m.ToString(), TraceLevel.Info);
        }

        private class sMeter_t
        {
            private Slice s;
            private FlexBase parent;
            public void sMeterData(float data)
            {
                // Only report for the active slice.
                if (s.Active)
                {
                    Tracing.TraceLine("sMeterData:" + s.Index + ' ' + data.ToString(), TraceLevel.Verbose);
                    parent._SMeter = (int)data;
                }
            }

            internal sMeter_t(FlexBase p, Slice slc)
            {
                parent = p;
                s = slc;
            }
        }

        private bool _Transmit;
        /// <summary>
        /// True if transmitting
        /// </summary>
        public bool Transmit
        {
            get
            {
                return _Transmit;
            }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.Mox = value; }));
            }
        }

        /// <summary>
        /// True if rig is on the WAN.
        /// </summary>
        public bool RemoteRig
        {
            get { return theRadio.IsWan; }
        }

        private int firstCharID = -1;
        private StringBuilder sentChars = new StringBuilder();
        private void charSentHandler(int id)
        {
            Tracing.TraceLine("charSent:" + id, TraceLevel.Info);
            if (firstCharID == -1) firstCharID = id;
            int currentCharID = id - firstCharID;
#if CWMonitor
            if (useCWMon && CWMon.Started)
            {
                //CWMon.Send(sentChars[currentCharID]);
                //if (currentCharID < (sentChars.Length - 1)) CWMon.Send(sentChars[currentCharID + 1]);
            }
#endif
        }

        /// <summary>
        /// Calibrated S-Meter/power
        /// </summary>
        /// <remarks>Smeter and forward power are in DBM.</remarks>
        private int _SMeter;
        public int SMeter
        {
            get
            {
                if (Transmit)
                {
                    // Show forward power = exp(10, (dbm/10)) / 1000
                    return (int)((Math.Pow(10d, (double)(_PowerDBM / 10)) / 1000) + 0.5);
                }
                else
                {
                    int val = _SMeter + 127 - 3; // puts s0 at 0.
                    if (val < 0) val = 0;
                    int s = val / 6; // S-unit
                    // Perhaps indicate over S9.
                    val = (s <= 9) ? s : val - (9 * 6) + 9;
                    return val;
                }
            }
        }

        // a VFO is really a slice index.
        internal Slice VFOToSlice(int vfo)
        {
            if (!ValidVFO(vfo))
            {
                Tracing.TraceLine("VFOToSlice error:" + vfo, TraceLevel.Error);
                return null;
            }

            return mySlices[vfo];
        }

        internal int SliceToVFO(Slice s)
        {
            lock (mySlices)
            {
                for (int i = 0; i < mySlices.Count; i++)
                {
                    if (s.Index == mySlices[i].Index) return i;
                }
            }

            Tracing.TraceLine("SliceToVFO:Error", TraceLevel.Error);
            return noVFO;
        }

        /// <summary>
        /// The next VFO, wraps around.
        /// </summary>
        /// <param name="v">current VFO</param>
        public int NextVFO(int v)
        {
            return (v + 1) % MyNumSlices;
        }

        /// <summary>
        /// The previous VFO, wraps around.
        /// </summary>
        /// <param name="v">current VFO</param>
        public int PriorVFO(int v)
        {
            return (v > 0) ? v - 1 : MyNumSlices - 1;
        }

        #region Rig properties
        private const int noVFO = -1;
        /// <summary>
        /// true if the VFO value is good.
        /// </summary>
        /// <param name="vfo">the VFO</param>
        public bool ValidVFO(int vfo)
        {
            return ((vfo >= 0) & (vfo < MyNumSlices));
        }

        // Note that VFOToSlice(RXVFO) is the ActiveSlice.
        internal int _RXVFO = noVFO;
        public int RXVFO
        {
            get { return _RXVFO; }
            set
            {
                if (_RXVFO != value)
                {
                    _RXVFO = value;
                    if (ValidVFO(_RXVFO))
                    {
                        q.Enqueue((FunctionDel)(() => { VFOToSlice(_RXVFO).Active = true; }));
                    }
                    // else we don't reset it.
                }
            }
        }

        internal int _TXVFO = noVFO;
        public int TXVFO
        {
            get { return _TXVFO; }
            set
            {
                if (_TXVFO != value)
                {
                    _TXVFO = value;
                    if (ValidVFO(_TXVFO))
                    {
                        q.Enqueue((FunctionDel)(() => { mySlices[_TXVFO].IsTransmitSlice = true; }));
                    }
                }
            }
        }

        /// <summary>
        /// Get/set the current VFO in use.
        /// </summary>
        public int CurVFO
        {
            get
            {
                return (Transmit) ? TXVFO : RXVFO;
            }
            set
            {
                if (Transmit)
                {
                    TXVFO = value;
                }
                else
                {
                    RXVFO = value;
                }
            }
        }

        /// <summary>
        /// Get the VFO's (slice's) audio
        /// </summary>
        /// <param name="v">VFO or slice</param>
        /// <returns>true if on</returns>
        public bool GetVFOAudio(int v)
        {
            return !VFOToSlice(v).Mute;
        }

        /// <summary>
        /// Turn audio on/off.
        /// </summary>
        /// <param name="v">VFO or slice id</param>
        /// <param name="on">true for on</param>
        public void SetVFOAudio(int v, bool on)
        {
            Tracing.TraceLine("SetVFOAudio:" + v + ' ' + on.ToString(), TraceLevel.Info);
            if ((v < 0) | (v >= MyNumSlices)) { return; }
            q.Enqueue((FunctionDel)(() =>
            {
                VFOToSlice(v).Mute = !on;
            }));
        }

        /// <summary>
        /// get the audio pan value
        /// </summary>
        /// <param name="v">VFO or slice</param>
        public int GetVFOPan(int v)
        {
            return VFOToSlice(v).AudioPan;
        }

        public const int MinPan = 0;
        public const int MaxPan = 100;
        public const int PanIncrement = 10;
        /// <summary>
        /// Adjust the slice audio panning
        /// </summary>
        /// <param name="v">VFO or slice</param>
        /// <param name="pan">pan value</param>
        public void SetVFOPan(int v, int pan)
        {
            Tracing.TraceLine("SetVFOPan:" + v + ' ' + pan, TraceLevel.Info);
            if ((v < 0) | (v >= MyNumSlices)) { return; }
            q.Enqueue((FunctionDel)(() =>
            {
                VFOToSlice(v).AudioPan = pan;
            }));
        }

        /// <summary>
        /// get the audio gain value
        /// </summary>
        /// <param name="v">VFO or slice</param>
        public int GetVFOGain(int v)
        {
            return VFOToSlice(v).AudioGain;
        }

        public const int MinGain = 0;
        public const int MaxGain = 100;
        public const int GainIncrement = 10;
        /// <summary>
        /// Adjust the slice audio gain
        /// </summary>
        /// <param name="v">VFO or slice</param>
        /// <param name="gain">gain value</param>
        public void SetVFOGain(int v, int gain)
        {
            Tracing.TraceLine("SetVFOGain:" + v + ' ' + gain, TraceLevel.Info);
            if ((v < 0) | (v >= MyNumSlices)) { return; }
            q.Enqueue((FunctionDel)(() =>
            {
                VFOToSlice(v).AudioGain = gain;
            }));
        }

        public void CopyVFO(int inv, int outv)
        {
            if (Transmit)
            {
                Tracing.TraceLine("CopyVFO:can't be transmitting", TraceLevel.Error);
                return;
            }
            if ((!ValidVFO(inv) | !ValidVFO(outv)) |
                (inv == outv))
            {
                Tracing.TraceLine("CopyVFO:bad VFO:" + inv.ToString() + " " + outv.ToString(), TraceLevel.Error);
                return;
            }

            Tracing.TraceLine("CopyVFO:" + inv.ToString() + " " + outv.ToString(), TraceLevel.Info);
            Slice inSlice = VFOToSlice(inv);
            Slice outSlice = VFOToSlice(outv);
            q.Enqueue((FunctionDel)(() => { outSlice.Freq = inSlice.Freq; }));
            q.Enqueue((FunctionDel)(() => { outSlice.DemodMode = inSlice.DemodMode; }));
            q.Enqueue((FunctionDel)(() => { outSlice.AutoPan = inSlice.AutoPan; }));
            q.Enqueue((FunctionDel)(() => { outSlice.RTTYMark = inSlice.RTTYMark; }));
            q.Enqueue((FunctionDel)(() => { outSlice.RTTYShift = inSlice.RTTYShift; }));
            q.Enqueue((FunctionDel)(() => { outSlice.DIGLOffset = inSlice.DIGLOffset; }));
            q.Enqueue((FunctionDel)(() => { outSlice.DIGUOffset = inSlice.DIGUOffset; }));
            q.Enqueue((FunctionDel)(() => { outSlice.FilterHigh = inSlice.FilterHigh; }));
            q.Enqueue((FunctionDel)(() => { outSlice.FilterLow = inSlice.FilterLow; }));
            q.Enqueue((FunctionDel)(() => { outSlice.ANFOn = inSlice.ANFOn; }));
            q.Enqueue((FunctionDel)(() => { outSlice.APFOn = inSlice.APFOn; }));
            q.Enqueue((FunctionDel)(() => { outSlice.ANFLevel = inSlice.ANFLevel; }));
            q.Enqueue((FunctionDel)(() => { outSlice.APFLevel = inSlice.APFLevel; }));
            q.Enqueue((FunctionDel)(() => { outSlice.WNBOn = inSlice.WNBOn; }));
            q.Enqueue((FunctionDel)(() => { outSlice.WNBLevel = inSlice.WNBLevel; }));
            q.Enqueue((FunctionDel)(() => { outSlice.NBOn = inSlice.NBOn; }));
            q.Enqueue((FunctionDel)(() => { outSlice.NBLevel = inSlice.NBLevel; }));
            q.Enqueue((FunctionDel)(() => { outSlice.NROn = inSlice.NROn; }));
            q.Enqueue((FunctionDel)(() => { outSlice.NRLevel = inSlice.NRLevel; }));
            q.Enqueue((FunctionDel)(() => { outSlice.AGCMode = inSlice.AGCMode; }));
            q.Enqueue((FunctionDel)(() => { outSlice.AGCOffLevel = inSlice.AGCOffLevel; }));
            q.Enqueue((FunctionDel)(() => { outSlice.AGCThreshold = inSlice.AGCThreshold; }));
            q.Enqueue((FunctionDel)(() => { outSlice.FMDeviation = inSlice.FMDeviation; }));
            q.Enqueue((FunctionDel)(() => { outSlice.FMRepeaterOffsetFreq = inSlice.FMRepeaterOffsetFreq; }));
            q.Enqueue((FunctionDel)(() => { outSlice.FMToneValue = inSlice.FMToneValue; }));
            q.Enqueue((FunctionDel)(() => { outSlice.FMTX1750 = inSlice.FMTX1750; }));
            q.Enqueue((FunctionDel)(() => { outSlice.RepeaterOffsetDirection = inSlice.RepeaterOffsetDirection; }));

            List<Slice> sList = new List<Slice>();
            sList.Add(inSlice);
            sList.Add(outSlice);
            if (RXFreqChange != null) RXFreqChange(sList);
        }

        internal double LongFreqToLibFreq(ulong u)
        {
            return (double)u / 1000000d;
        }

        internal ulong LibFreqtoLong(double f)
        {
            return (ulong)(f * 1000000d);
        }

        private ulong _RXFrequency;
        public ulong RXFrequency
        {
            get
            {
                return _RXFrequency;
            }
            set
            {
                double freq = LongFreqToLibFreq(value);
                q.Enqueue((FunctionDel)(() => { VFOToSlice(RXVFO).Freq = freq; }));
            }
        }

        private ulong _TXFrequency;
        public ulong TXFrequency
        {
            get
            {
                return _TXFrequency;
            }
            set
            {
                double freq = LongFreqToLibFreq(value);
                q.Enqueue((FunctionDel)(() => { VFOToSlice(TXVFO).Freq = freq; }));
            }
        }

        /// <summary>
        /// current frequency
        /// </summary>
        public ulong Frequency
        {
            get { return (Transmit) ? TXFrequency : RXFrequency; }
            set
            {
                // Don't set if transmitting.
                if (Transmit)
                {
                    Tracing.TraceLine("Frequency:can't set it now",TraceLevel.Error);
                    return;
                }
                RXFrequency = value;
            }
        }

        /// <summary>
        /// showing XMIT frequency when split.
        /// </summary>
        public bool ShowingXmitFrequency
        {
            get; set;
        }

        /// <summary>
        /// Set frequency according to ShowingXmitFrequency.
        /// </summary>
        public ulong VirtualRXFrequency
        {
            get
            {
                return (ShowingXmitFrequency) ? TXFrequency : RXFrequency;
            }
            set
            {
                if (Transmit) return; // disallow set
                if (ShowingXmitFrequency) TXFrequency = value;
                else RXFrequency = value;
            }
        }

        private string _RXMode = "";
        /// <summary>
        /// RX mode
        /// </summary>
        public string RXMode
        {
            get
            {
                return _RXMode;
            }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.DemodMode = value; }));
            }
        }

        private string _TXMode = "";
        /// <summary>
        /// TX mode
        /// </summary>
        public string TXMode
        {
            get
            {
                return _TXMode;
            }
            set
            {
                q.Enqueue((FunctionDel)(() => { VFOToSlice(TXVFO).DemodMode = value; }));
            }
        }

        /// <summary>
        /// current mode
        /// </summary>
        public string Mode
        {
            get { return (string)((Transmit) ? TXMode : RXMode); }
            set
            {
                // Can't set during transmit.
                if (Transmit) return;
                RXMode = value;
            }
        }

        internal int FilterLow
        {
            get
            {
                return theRadio.ActiveSlice.FilterLow;
            }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.FilterLow = value; }));
            }
        }

        internal int FilterHigh
        {
            get
            {
                return theRadio.ActiveSlice.FilterHigh;
            }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.FilterHigh = value; }));
            }
        }

        // TXAntenna must be set first.
        public bool RXAntenna
        {
            get
            {
                return (theRadio.ActiveSlice.RXAnt != VFOToSlice(RXVFO).TXAnt);
            }
            set
            {
#if zero
                // Use the other antenna, 0 or 1, if true.
                int ant = (value) ? (TXAntenna + 1) % 2 : TXAntenna;
                foreach (Slice s in mySlices)
                {
                    q.Enqueue((FunctionDel)(() => { s.RXAnt = theRadio.RXAntList[ant]; }));
                }
#endif
            }
        }

        /// <summary>
        /// Set both the TX and RX antenna values.
        /// </summary>
        public int TXAntenna
        {
            get
            {
                int rv = -1; // Invalid if not found.
                int max = Math.Min(theRadio.RXAntList.Length - 1, 1);
                for (int id = 0; id <= max; id++)
                {
                    if (theRadio.ActiveSlice.TXAnt == theRadio.RXAntList[id])
                    {
                        rv = id;
                        break;
                    }
                }
                return rv;
            }
            set
            {
                Tracing.TraceLine("TXAntenna:" + value.ToString(), TraceLevel.Info);
                if (value < theRadio.RXAntList.Length)
                {
                    lock (mySlices)
                    {
                        foreach (Slice s in mySlices)
                        {
                            q.Enqueue((FunctionDel)(() => { s.TXAnt = theRadio.RXAntList[value]; }));
                            q.Enqueue((FunctionDel)(() => { s.RXAnt = theRadio.RXAntList[value]; }));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Type of the Flex tuner in use
        /// </summary>
        public enum FlexTunerTypes
        {
            none,
            manual,
            auto,
        }

        private FlexTunerTypes _FlexTunerType;
        public FlexTunerTypes FlexTunerType
        {
            get { return _FlexTunerType; }
            set
            {
                // Set by the user only.
                Tracing.TraceLine("FlexTunerType:" + value.ToString() + ' ' +
                    _FlexTunerType.ToString() + ' ' + _FlexTunerOn.ToString(), TraceLevel.Info);
                if (value == _FlexTunerType) return;
                // Can't change while tuning.
                if (!_FlexTunerOn)
                {
                    if (value == FlexTunerTypes.auto) _FlexTunerType = value;
                    else
                    {
                        setFlexTunerTypeNotAuto();
                        // We were in autoTune mode.  Need to bypass.
                        theRadio.ATUTuneBypass();
                    }
                }
            }
        }

        protected void setFlexTunerTypeNotAuto()
        {
            _FlexTunerType = (MyCaps.HasCap(RigCaps.Caps.ManualATGet)) ?
                FlexTunerTypes.manual : FlexTunerTypes.none;
            Tracing.TraceLine("setFlexTunerTypeNotAuto:" + _FlexTunerType.ToString(), TraceLevel.Info);
        }

        private bool _FlexTunerOn;
        public bool FlexTunerOn
        {
            get { return _FlexTunerOn; }
            set
            {
                // set internally or by the user.
                Tracing.TraceLine("FlexTunerOn:" + value.ToString() + ' ' +
                    _FlexTunerOn.ToString() + ' ' + _FlexTunerType.ToString(), TraceLevel.Info);
                if (value == _FlexTunerOn) return;
                switch (_FlexTunerType)
                {
                    case FlexTunerTypes.manual:
                        {
                            // Raise status if turning off so we can report the SWR.
                            if (!value)
                            {
                                float highSWR;
                                // Look for minimum SWR.
                                do
                                {
                                    highSWR = _SWR;
                                    Thread.Sleep(100);
                                } while (highSWR > _SWR);
                                // Report status.
                                ATUTuneStatus stat = ATUTuneStatus.OK;
                                RaiseFlexAntTuneStartStop(new FlexAntTunerArg
                                    (FlexTunerType, stat, highSWR));
                            }
                            q.Enqueue((FunctionDel)(() => { theRadio.TXTune = value; }));
                        }
                        break;
                    case FlexTunerTypes.auto:
                        {
                            // Normally tuning stops automatically when finished.
                            q.Enqueue((FunctionDel)(() => { Transmit = value; }));
                            if (value)
                            {
                                q.Enqueue((FunctionDel)(() => { theRadio.ATUTuneStart(); }));
                            }
                        }
                        break;
                }
                _FlexTunerOn = value;
            }
        }

        public void AntennaTunerMemories()
        {
            Form theForm = new FlexATUMemories(this);
            theForm.ShowDialog();
            theForm.Dispose();
        }

        public bool FlexTunerUsingMemoryNow
        {
            get
            {
                return ((_FlexTunerType == FlexTunerTypes.auto) &
                    (theRadio.ATUTuneStatus != ATUTuneStatus.Bypass) &
                    theRadio.ATUMemoriesEnabled & theRadio.ATUUsingMemory);
            }
        }

        internal const int AudioGainMinValue = 0;
        internal const int AudioGainMaxValue = 100;
        public int AudioGain
        {
            get
            {
                //return base.AudioGain;
                return theRadio.ActiveSlice.AudioGain;
            }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.AudioGain = value; }));
            }
        }

        internal const int AudioPanMinValue = 0;
        internal const int AudioPanMaxValue = 100;
        public int AudioPan
        {
            get
            {
                return theRadio.ActiveSlice.AudioPan;
            }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.AudioPan = value; }));
            }
        }

        internal const int LineoutGainMinValue = 0;
        internal const int LineoutGainMaxValue = 100;
        public int LineoutGain
        {
            get
            {
                //return base.LineoutGain;
                return theRadio.LineoutGain;
            }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.LineoutGain = value; }));
            }
        }

        internal const int HeadphoneGainMinValue = 0;
        internal const int HeadphoneGainMaxValue = 100;
        public int HeadphoneGain
        {
            get
            {
                return theRadio.HeadphoneGain;
            }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.HeadphoneGain = value; }));
            }
        }

        public OffOnValues Vox
        {
            get
            {
                bool val = false;
                if (ValidVFO(TXVFO))
                {
                    if (VFOToSlice(TXVFO).DemodMode == "CW") val = theRadio.CWBreakIn;
                    else val = theRadio.SimpleVOXEnable;
                }
                return (val) ? OffOnValues.on : OffOnValues.off;
            }
            set
            {
                Slice s = VFOToSlice(TXVFO); // also tests the VFO
                bool val = (value == OffOnValues.on) ? true : false;
                if (s != null)
                {
                    if (s.DemodMode == "CW")
                    {
                        q.Enqueue((FunctionDel)(() => { theRadio.CWBreakIn = val; }));
                    }
                    else
                    {
                        q.Enqueue((FunctionDel)(() => { theRadio.SimpleVOXEnable = val; }));
                    }
                }
            }
        }

        public OffOnValues NoiseBlanker
        {
            get
            {
                return (theRadio.ActiveSlice.NBOn) ? OffOnValues.on : OffOnValues.off;
            }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.NBOn = (value == OffOnValues.on) ? true : false; }));
            }
        }

        // The values are the same for the wide band NB.
        internal const int NoiseBlankerValueMin = 0;
        internal const int NoiseBlankerValueMax = 100;
        internal const int NoiseBlankerValueIncrement = 5;
        internal int NoiseBlankerLevel
        {
            get { return theRadio.ActiveSlice.NBLevel; }
            set { q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.NBLevel = value; })); }
        }

        internal OffOnValues WidebandNoiseBlanker
        {
            get
            {
                return (theRadio.ActiveSlice.WNBOn) ? OffOnValues.on : OffOnValues.off;
            }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.WNBOn = (value == OffOnValues.on) ? true : false; }));
            }
        }

        internal int WidebandNoiseBlankerLevel
        {
            get { return theRadio.ActiveSlice.WNBLevel; }
            set { q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.WNBLevel = value; })); }
        }

        public OffOnValues NoiseReduction
        {
            get
            {
                return (theRadio.ActiveSlice.NROn) ? OffOnValues.on : OffOnValues.off;
            }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.NROn = (value == OffOnValues.on) ? true : false; }));
            }
        }

        internal const int NoiseReductionValueMin = 0;
        internal const int NoiseReductionValueMax = 100;
        internal const int NoiseReductionValueIncrement = 5;
        internal int NoiseReductionLevel
        {
            get { return theRadio.ActiveSlice.NRLevel; }
            set { q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.NRLevel = value; })); }
        }

        /// <summary>
        /// AGC mode
        /// </summary>
        /// <remarks>Different from AllRadios</remarks>
        internal AGCMode AGCSpeed
        {
            get { return theRadio.ActiveSlice.AGCMode; }
            set { q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.AGCMode = value; })); }
        }

        internal const int AGCThresholdMin = 0;
        internal const int AGCThresholdMax = 100;
        internal const int AGCThresholdIncrement = 5;
        internal int AGCThreshold
        {
            get { return theRadio.ActiveSlice.AGCThreshold; }
            set { q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.AGCThreshold = value; })); }
        }

        /// <summary>
        /// data type for RIT/XIT.
        /// </summary>
        public class RITData
        {
            public bool Active;
            public int Value; // may be negative
            public RITData()
            {
                Active = false;
                Value = 0;
            }
            public RITData(RITData r)
            {
                Active = r.Active;
                Value = r.Value;
            }
        }
        private RITData _RIT = new RITData();
        public RITData RIT
        {
            get
            {
                lock (_RIT)
                {
                    return _RIT;
                }
            }
            set
            {
                // _RIT set in PropertyChangedHandler
                lock (_RIT)
                {
                    q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.RITOn = value.Active; }));
                    q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.RITFreq = value.Value; }));
                }
            }
        }

        private RITData _XIT = new RITData();
        public RITData XIT
        {
            get
            {
                lock (_XIT)
                {
                    return _XIT; ;
                }
            }
            set
            {
                // _XIT set in PropertyChangedHandler
                lock (_XIT)
                {
                    q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.XITFreq = value.Value; }));
                    q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.XITOn = value.Active; }));
                }
            }
        }

        internal const int BreakinDelayMin = 0;
        internal const int BreakinDelayMax = 2000;
        internal const int BreakinDelayIncrement = 50;
        internal int BreakinDelay
        {
            get { return theRadio.CWDelay; }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.CWDelay = value; }));
                q.Enqueue((FunctionDel)(() => { cwx.Delay = value; }));
            }
        }

        internal const int SidetonePitchMin = 100;
        internal const int SidetonePitchMax = 6000;
        internal const int SidetonePitchIncrement = 50;
        internal int SidetonePitch
        {
            get { return theRadio.CWPitch; }
            set { q.Enqueue((FunctionDel)(() => { theRadio.CWPitch = value; })); }
        }

        internal const int SidetoneGainMin = 0;
        internal const int SidetoneGainMax = 100;
        internal const int SidetoneGainIncrement = 5;
        internal int SidetoneGain
        {
            get { return theRadio.TXCWMonitorGain; }
            set
            {
#if CWMonitor
                if (useCWMon) q.Enqueue((FunctionDel)(() => { CWMon.Volume = value; }));
#endif
                q.Enqueue((FunctionDel)(() => { theRadio.TXCWMonitorGain = value; }));
            }
        }

        internal enum IambicValues
        {
            off,
            iambicA,
            iambicB
        }

        private IambicValues _Keyer;
        internal IambicValues Keyer
        {
            get { return _Keyer; }
            set
            {
                // Set keyer on/off
                q.Enqueue((FunctionDel)(() => { theRadio.CWIambic = (value == IambicValues.off) ? false : true; }));
                if (value != IambicValues.off)
                {
                    // Set iambic mode.
                    q.Enqueue((FunctionDel)(() => { theRadio.CWIambicModeA = (value == IambicValues.iambicA) ? true : false; }));
                    q.Enqueue((FunctionDel)(() => { theRadio.CWIambicModeB = (value == IambicValues.iambicB) ? true : false; }));
                }
            }
        }

        private bool _CWReverse;
        internal bool CWReverse
        {
            get { return _CWReverse; }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.CWSwapPaddles = value; }));
            }
        }

        internal const int KeyerSpeedMin = 5;
        internal const int KeyerSpeedMax = 100;
        internal const int KeyerSpeedIncrement = 1;
        internal int KeyerSpeed
        {
            get { return theRadio.CWSpeed; }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.CWSpeed = value; }));
                q.Enqueue((FunctionDel)(() => { cwx.Speed = value; }));
            }
        }

        internal OffOnValues CWL
        {
            get { return (theRadio.CWL_Enabled) ? OffOnValues.on : OffOnValues.off; }
            set
            {
                bool val = (value == OffOnValues.on) ? true : false;
                q.Enqueue((FunctionDel)(() => { theRadio.CWL_Enabled = val; }));
            }
        }

        internal const int MonitorPanMin = 0;
        internal const int MonitorPanMax = 100;
        internal const int MonitorPanIncrement = 5;
        internal int MonitorPan
        {
            get { return theRadio.TXCWMonitorPan; }
            set { q.Enqueue((FunctionDel)(() => { theRadio.TXCWMonitorPan = value; })); }
        }

        internal const int MicGainMin = 0;
        internal const int MicGainMax = 100;
        internal const int MicGainIncrement = 1;
        internal int MicGain
        {
            get { return theRadio.MicLevel; }
            set { q.Enqueue((FunctionDel)(() => { theRadio.MicLevel = value; })); }
        }

        internal OffOnValues ProcessorOn
        {
            get { return (theRadio.SpeechProcessorEnable) ? OffOnValues.on : OffOnValues.off; }
            set { q.Enqueue((FunctionDel)(() => { theRadio.SpeechProcessorEnable = (value == OffOnValues.on) ? true : false; })); }
        }

        internal enum ProcessorSettings
        {
            NOR = 0,
            DX,
            DXX,
        }
        internal ProcessorSettings ProcessorSetting
        {
            get { return (ProcessorSettings)theRadio.SpeechProcessorLevel; }
            set { q.Enqueue((FunctionDel)(() => { theRadio.SpeechProcessorLevel = (uint)value; })); }
        }

        internal OffOnValues Compander
        {
            get { return (theRadio.CompanderOn) ? OffOnValues.on : OffOnValues.off; }
            set
            {
                bool val = (value == OffOnValues.on) ? true : false;
                q.Enqueue((FunctionDel)(() => { theRadio.CompanderOn = val; }));
            }
        }

        internal const int CompanderLevelMin = 0;
        internal const int CompanderLevelMax = 100;
        internal const int CompanderLevelIncrement = 5;
        internal int CompanderLevel
        {
            get { return theRadio.CompanderLevel; }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.CompanderLevel = value; }));
            }
        }

        internal int TXFilterLowMin = 0;
        internal int TXFilterLowMax { get { return (TXFilterHigh - 50); } }
        internal int TXFilterLowIncrement = 50;
        internal int TXFilterLow
        {
            get { return (theRadio != null) ? theRadio.TXFilterLow : 0; }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.TXFilterLow = value; }));
            }
        }

        internal int TXFilterHighMin { get { return (TXFilterLow + 50); } }
        internal int TXFilterHighMax = 10000;
        internal int TXFilterHighIncrement = 50;
        internal int TXFilterHigh
        {
            get { return (theRadio != null) ? theRadio.TXFilterHigh : 0; }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.TXFilterHigh = value; }));
            }
        }

        internal OffOnValues MicBoost
        {
            get { return (theRadio.MicBoost) ? OffOnValues.on : OffOnValues.off; }
            set
            {
                bool val = (value == OffOnValues.on) ? true : false;
                q.Enqueue((FunctionDel)(() => { theRadio.MicBoost = val; }));
            }
        }

        internal OffOnValues MicBias
        {
            get { return (theRadio.MicBias) ? OffOnValues.on : OffOnValues.off; }
            set
            {
                bool val = (value == OffOnValues.on) ? true : false;
                q.Enqueue((FunctionDel)(() => { theRadio.MicBias = val; }));
            }
        }

        internal OffOnValues Monitor
        {
            get { return (theRadio.TXMonitor) ? OffOnValues.on : OffOnValues.off; }
            set
            {
                bool val = (value == OffOnValues.on) ? true : false;
                q.Enqueue((FunctionDel)(() => { theRadio.TXMonitor = val; }));
            }
        }

        internal const int SBMonitorLevelMin = 0;
        internal const int SBMonitorLevelMax = 100;
        internal const int SBMonitorLevelIncrement = 5;
        internal int SBMonitorLevel
        {
            get { return theRadio.TXSBMonitorGain; }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.TXSBMonitorGain = value; }));
            }
        }

        internal const int SBMonitorPanMin = 0;
        internal const int SBMonitorPanMax = 100;
        internal const int SBMonitorPanIncrement = 5;
        internal int SBMonitorPan
        {
            get { return theRadio.TXSBMonitorPan; }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.TXSBMonitorPan = value; }));
            }
        }

        // transmit power
        internal const int XmitPowerMin = 0;
        internal const int XmitPowerMax = 100;
        internal const int XmitPowerIncrement = 5;
        private int _XmitPower;
        public int XmitPower
        {
            get
            {
                return _XmitPower;
            }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.RFPower = value; }));
            }
        }

        // Tuning power
        internal const int TunePowerMin = 0;
        internal const int TunePowerMax = 100;
        internal const int TunePowerIncrement = 1;
        private int _TunePower;
        public int TunePower
        {
            get
            {
                return _TunePower;
            }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.TunePower = value; }));
            }
        }

        // Vox delay is in MS, with 50 MS per step, see FlexLib.Radio.cs
        internal const int VoxDelayMin = 0;
        internal const int VoxDelayMax = 2000;
        internal const int VoxDelayIncrement = 100;
        internal const int VoxDelayMS = 50;
        internal int VoxDelay
        {
            get { return theRadio.SimpleVOXDelay * VoxDelayMS; }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.SimpleVOXDelay = value / VoxDelayMS; }));
            }
        }

        internal const int VoxGainMin = 0;
        internal const int VoxGainMax = 100;
        internal const int VoxGainIncrement = 5;
        internal int VoxGain
        {
            get { return theRadio.SimpleVOXLevel; }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.SimpleVOXLevel = value; }));
            }
        }

        internal OffOnValues ANF
        {
            get { return (theRadio.ActiveSlice.ANFOn) ? OffOnValues.on : OffOnValues.off; }
            set
            {
                bool val = (value == OffOnValues.on) ? true : false;
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.ANFOn = val; }));
            }
        }

        internal const int AutoNotchLevelMin = 0;
        internal const int AutoNotchLevelMax = 100;
        internal const int AutoNotchLevelIncrement = 10;
        internal int AutoNotchLevel
        {
            get { return theRadio.ActiveSlice.ANFLevel; }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.ANFLevel = value; }));
            }
        }

        internal OffOnValues APF
        {
            get { return (theRadio.ActiveSlice.APFOn) ? OffOnValues.on : OffOnValues.off; }
            //get { return (theRadio.APFMode) ? OffOnValues.on : OffOnValues.off; }
            set
            {
                bool val = (value == OffOnValues.on) ? true : false;
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.APFOn = val; }));
                //q.Enqueue((FunctionDel)(() => { theRadio.APFMode = val; }));
            }
        }

        internal const int AutoPeakLevelMin = 0;
        internal const int AutoPeakLevelMax = 100;
        internal const int AutoPeakLevelIncrement = 10;
        internal int AutoPeakLevel
        {
            get { return theRadio.ActiveSlice.APFLevel; }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.APFLevel = value; }));
            }
        }

        private Panadapter activePan
        {
            get { return (theRadio.ActiveSlice != null) ? theRadio.ActiveSlice.Panadapter : null; }
        }

        internal int RFGainMin = -10;
        internal int RFGainMax = 30;
        internal int RFGainIncrement = 10;
        internal int RFGain
        {
            get { return (activePan != null) ? activePan.RFGain : 0; }
            set
            {
                if (activePan != null)
                {
                    q.Enqueue((FunctionDel)(() => { activePan.RFGain = value; }));
                }
            }
        }

#if zero
        private const int PreAmpMin = 0;
        private const int PreAmpMax = 20;
        private OffOnValues _PreAmp;
        internal OffOnValues PreAmp
        {
            get { return _PreAmp; }
            set
            {
                // _PreAmp changed by interrupt.
                activePan.RFGain = (value == OffOnValues.on) ? PreAmpMax : PreAmpMin;
            }
        }

        internal const int PanRFMin = 0;
        internal const int PanRFMax = 20;
        internal const int PanRFIncrement = 20;
        internal int PanRF
        {
            get { return theRadio.ActiveSlice.Panadapter.RFGain; }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.Panadapter.RFGain = value; }));
            }
        }
#endif

        internal const int AutoPeakQMin = 0;
        internal const int AutoPeakQMax = 33;
        internal const int AutoPeakQIncrement = 1;
        internal int AutoPeakQ
        {
            get { return (int)theRadio.APFQFactor; }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.APFQFactor = (double)value; }));
            }
        }

        internal bool TNF
        {
            get { return theRadio.TNFEnabled; }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.TNFEnabled = value; }));
            }
        }

        internal OffOnValues Squelch
        {
            get { return (theRadio.ActiveSlice.SquelchOn) ? OffOnValues.on : OffOnValues.off; }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.SquelchOn = (value == OffOnValues.on) ? true : false; }));
            }
        }

        internal const int SquelchLevelMin = 0;
        internal const int SquelchLevelMax = 100;
        internal const int SquelchLevelIncrement = 5;
        internal int SquelchLevel
        {
            get { return theRadio.ActiveSlice.SquelchLevel; }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.SquelchLevel = value; }));
            }
        }

        /// <summary>
        /// Offset direction values
        /// </summary>
        public enum OffsetDirections : byte
        {
            off, minus, plus, allTypes
        }

        internal OffsetDirections FlexOffsetDirectionToOffsetDirection(FMTXOffsetDirection dir)
        {
            OffsetDirections rv = OffsetDirections.off;
            switch (dir)
            {
                case FMTXOffsetDirection.Down: rv = OffsetDirections.minus; break;
                case FMTXOffsetDirection.Up: rv = OffsetDirections.plus; break;
            }
            return rv;
        }
        internal FMTXOffsetDirection OffsetDirectionToFlexOffsetDirection(OffsetDirections dir)
        {
            FMTXOffsetDirection rv = FMTXOffsetDirection.Simplex;
            switch (dir)
            {
                case OffsetDirections.minus: rv = FMTXOffsetDirection.Down; break;
                case OffsetDirections.plus: rv = FMTXOffsetDirection.Up; break;
            }
            return rv;
        }
        public OffsetDirections OffsetDirection
        {
            get
            {
                OffsetDirections rv = OffsetDirections.off;
                if (theRadio.ActiveSlice != null)
                {
                    rv = FlexOffsetDirectionToOffsetDirection(theRadio.ActiveSlice.RepeaterOffsetDirection);
                }
                return rv;
            }
            set
            {
                if (theRadio.ActiveSlice != null)
                {
                    FMTXOffsetDirection val = OffsetDirectionToFlexOffsetDirection(value);
                    q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.RepeaterOffsetDirection = val; }));
                }
            }
        }

        internal OffOnValues FMEmphasis
        {
            get { return (theRadio.ActiveSlice.DFMPreDeEmphasis) ? OffOnValues.on : OffOnValues.off; }
            set
            {
                bool val = (value == OffOnValues.on) ? true : false;
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.DFMPreDeEmphasis = val; }));
            }
        }

        // Note the Flex frequency is in MhZ, and ours in KHZ.
        internal const int offsetMin = 50;
        internal const int offsetMax = 2000;
        internal const int offsetIncrement = 50;
        public int OffsetFrequency
        {
            get { return (int)(theRadio.ActiveSlice.FMRepeaterOffsetFreq * 1e3); }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.FMRepeaterOffsetFreq = (double)value / 1e3; }));
            }
        }

        // Valid FM tone modes for this rig, see FMToneMode in Memory.cs in FlexLib.
        /// <summary>
        /// Tone/CTSS value type
        /// </summary>
        public class ToneCTCSSValue
        {
            internal char value;
            private string name;
            internal ToneCTCSSValue(char val, string nam)
            {
                value = val;
                name = nam;
            }
            internal ToneCTCSSValue(char c)
            {
                value = c;
                name = "";
            }
            public static bool operator ==(ToneCTCSSValue val1, ToneCTCSSValue val2)
            {
                if (((object)val1 == null) && ((object)val2 == null)) return true;
                if (((object)val1 == null) || ((object)val2 == null)) return false;
                return (val1.value == val2.value);
            }
            public static bool operator !=(ToneCTCSSValue val1, ToneCTCSSValue val2)
            {
                if (((object)val1 == null) && ((object)val2 == null)) return false;
                if (((object)val1 == null) || ((object)val2 == null)) return true;
                return (val1.value != val2.value);
            }
            public override bool Equals(object obj)
            {
                bool rv;
                try { rv = (value == ((ToneCTCSSValue)obj).value); }
                catch (Exception ex)
                {
                    Tracing.TraceLine("ToneCTCSSValue exception:" + ex.Message, TraceLevel.Error);
                    rv = false;
                }
                return rv;
            }
            public override int GetHashCode()
            {
                return (int)value;
            }
            public override string ToString()
            {
                return name;
            }
        }

        /// <summary>
        /// FM Tone modes
        /// </summary>
        public ToneCTCSSValue[] FMToneModes;

        internal static ToneCTCSSValue[] myFMToneModes =
        {
            new ToneCTCSSValue('0', "Off"),
            new ToneCTCSSValue('1', "CTCSS"),
        };
        internal ToneCTCSSValue ToneModeToToneCTCSS(FMToneMode mode)
        {
            return myFMToneModes[(int)mode];
        }
        internal FMToneMode ToneCTCSSToToneMode(ToneCTCSSValue val)
        {
            return (FMToneMode)(val.value - 0x30);
        }
        public ToneCTCSSValue ToneCTCSS
        {
            get { return ToneModeToToneCTCSS(theRadio.ActiveSlice.ToneMode); }
            set
            {
                FMToneMode val = ToneCTCSSToToneMode(value);
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.ToneMode = val; }));
            }
        }

        internal float ToneValueToFloat(string val)
        {
            float rv = 0;
            System.Single.TryParse(val, out rv);
            return rv;
        }
        internal string FloatToToneValue(float val)
        {
            return val.ToString("F1");
        }
        public float ToneFrequency
        {
            get
            {
                return ToneValueToFloat(theRadio.ActiveSlice.FMToneValue);
            }
            set
            {
                string val = FloatToToneValue(value);
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.FMToneValue = val; }));
            }
        }

        internal OffOnValues FM1750
        {
            get { return (theRadio.ActiveSlice.FMTX1750) ? OffOnValues.on : OffOnValues.off; }
            set
            {
                bool val = (value == OffOnValues.on) ? true : false;
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.FMTX1750 = val; }));
            }
        }

        internal OffOnValues Binaural
        {
            get { return (theRadio.BinauralRX) ? OffOnValues.on : OffOnValues.off; }
            set
            {
                bool val = (value == OffOnValues.on) ? true : false;
                q.Enqueue((FunctionDel)(() => { theRadio.BinauralRX = val; }));
            }
        }

        internal OffOnValues Play
        {
            get { return (theRadio.ActiveSlice.PlayOn) ? OffOnValues.on : OffOnValues.off; }
            set
            {
                bool val = (value == OffOnValues.on) ? true : false;
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.PlayOn = val; }));
            }
        }

        internal OffOnValues Record
        {
            get { return (theRadio.ActiveSlice.RecordOn) ? OffOnValues.on : OffOnValues.off; }
            set
            {
                bool val = (value == OffOnValues.on) ? true : false;
                q.Enqueue((FunctionDel)(() => { theRadio.ActiveSlice.RecordOn = val; }));
            }
        }

        private OffOnValues _DAXOn;
        /// <summary>
        /// DAX audio, on or off.
        /// </summary>
        public OffOnValues DAXOn
        {
            get { return _DAXOn; }
            set
            {
                _DAXOn = value;
                bool val = (value == OffOnValues.on) ? true : false;
                q.Enqueue((FunctionDel)(() => { theRadio.DAXOn = val; }));
            }
        }

        internal bool CanPlay { get { return theRadio.ActiveSlice.PlayEnabled; } }

        internal const int AMCarrierLevelMin = 0;
        internal const int AMCarrierLevelMax = 100;
        internal const int AMCarrierLevelIncrement = 5;
        internal int AMCarrierLevel
        {
            get { return theRadio.AMCarrierLevel; }
            set
            {
                q.Enqueue((FunctionDel)(() => { theRadio.AMCarrierLevel = value; }));
            }
        }

        internal void setNextValue1()
        {
            if (theRadio.ActiveSlice.DemodMode == "CW")
            {
                theRadio.ActiveSlice.APFOn = !theRadio.ActiveSlice.APFOn;
            }
            else
            {
                theRadio.ActiveSlice.NROn = !theRadio.ActiveSlice.NROn;
            }
        }

        /// <summary>
        /// Rig's info (list of strings)
        /// </summary>
        public List<string> RigInfo
        {
            get
            {
                List<string> rv = new List<string>();
                rv.Add("Model:" + theRadio.Model);
                rv.Add("Version:" +
                    ((theRadio.Version & 0x00ff000000000000) / 0x0001000000000000).ToString() + '.' +
                    ((theRadio.Version & 0x0000ff0000000000) / 0x0000010000000000).ToString() + '.' +
                    ((theRadio.Version & 0x000000ff00000000) / 0x0000000100000000).ToString()
                    //((theRadio.Version & 0x00000000ffffffff)).ToString();
                    );
                rv.Add("Serial:" + theRadio.Serial);
                rv.Add("Call:" + theRadio.Callsign);
                rv.Add("Nickname:" + theRadio.Nickname);
                rv.Add("IP:" + theRadio.IP.ToString());
                return rv;
            }
        }

        /// <summary>
        /// List of connected stations
        /// </summary>
        public List<string> Stations
        {
            get
            {
                List<string> rv = new List<string>();
                rv.AddRange(theRadio.GuiClientStations.Split(new char[] { ',' }));
                return rv;
            }
        }

        public virtual void TestRoutine()
        {
            Tracing.TraceLine("TestRoutine", TraceLevel.Info);
            MessageBox.Show("slices:" + TotalNumSlices.ToString() + ' ' + OtherNumSlices.ToString(), "test", MessageBoxButtons.OK);

            string msg = "vfo/sliceID:";
            for(int i=0; i < MyNumSlices; i++)
            {
                msg += i.ToString() + '/' + VFOToSlice(i).Index.ToString() + ' ';
            }
            MessageBox.Show(msg, "test", MessageBoxButtons.OK);
        }
        #endregion

        // multi-user region
        #region multiUser
        private int initialFreeSlices = -1;
        private int _TotalNumSlices;
        /// <summary>
        /// Total panadapters and slices on the radio.
        /// </summary>
        public int TotalNumSlices
        {
            get { return _TotalNumSlices; }
        }

        internal List<Slice> mySlices = new List<Slice>();

        /// <summary>
        /// number of Panadapters and slices for this radio instance.
        /// </summary>
        public int MyNumSlices
        {
            get {
                lock (mySlices)
                {
                    return mySlices.Count;
                }
            }
        }

        /// <summary>
        /// number of slices used by others.
        /// </summary>
        public int OtherNumSlices
        {
            get
            {
                return theRadio.SliceList.Count - MyNumSlices;
            }
        }

        public enum SliceStates
        {
            none,
            mine,
            others,
            available
        }

        public SliceStates SliceState(int id)
        {
            SliceStates rv = SliceStates.none;
            if (id < MyNumSlices) rv = SliceStates.mine;
            else if (id - MyNumSlices < OtherNumSlices) rv = SliceStates.others;
            else rv = SliceStates.available;
            return rv;
        }

        /// <summary>
        /// Add a pan and slice.
        /// </summary>
        public bool NewSlice()
        {
            Tracing.TraceLine("NewSlice:", TraceLevel.Info);
            if (MyNumSlices == TotalNumSlices) return false;

            int myRXVFO = RXVFO;
            int myTXVFO = TXVFO;
            mySliceAdded = false; // need to know when slice added.
            q.Enqueue((FunctionDel)(() =>
            {
                theRadio.RequestPanafall();
                if (await(() =>
                {
                    // await both slice and panadapter.
                    return mySliceAdded & (MyNumPanadapters == MyNumSlices);
                }, 3000))
                {
                    // restore VFOs.
                    RXVFO = myRXVFO;
                    if (CanTransmit) TXVFO = myTXVFO;
                }
                else
                {
                    Tracing.TraceLine("NewSlice:counts don't match", TraceLevel.Error);
                }
            }));
            return true;
        }

        /// <summary>
        /// Remove a pan and slice.
        /// </summary>
        /// <param name="id">slice index</param>
        /// <returns>true if id valid</returns>
        public bool RemoveSlice(int id)
        {
            if ((id < 0) | (id > MyNumSlices)) return false;
            // Can't remove the active or transmit VFO.
            if ((id == RXVFO) | (CanTransmit & (id == TXVFO))) return false;

            Slice slc;
            Panadapter pan;
            lock (mySlices)
            {
                slc = mySlices[id];
            }
            pan = slc.Panadapter;

            q.Enqueue((FunctionDel)(() => { slc.Close(); ; }));
            q.Enqueue((FunctionDel)(() => { pan.Close(); }));
            return true;
        }

        /// <summary>
        /// true if can transmit.
        /// </summary>
        /*public bool CanTransmit
        {
            get
            {
                bool rv = (theRadio.GuiClientStations == myClient.Station);
                lock (theRadio.GuiClientsLockObj)
                {
                    rv = (theRadio.GuiClients[0].ClientHandle == myClient.ClientHandle);
                }
                return rv;
            }
        }*/
        public bool CanTransmit { get; internal set; }

        /// <summary>
        /// True if the primary station.
        /// </summary>
        public bool PrimaryStation { get; internal set; }

        public delegate void NoSliceErrorDel(object sender, string msg);
        /// <summary>
        /// No slice allocated to this instance.
        /// </summary>
        public event NoSliceErrorDel NoSliceError;
        private void raiseNoSliceError(string msg)
        {
            Tracing.TraceLine("raiseNoSliceError", TraceLevel.Error);
            if (NoSliceError != null)
            {
                NoSliceError(this, msg);
            }
        }
#endregion

        // region remote audio
#region RemoteAudio
        // Note that here input and output refer to input and output from the rig.
        private JJPortaudio.Devices audioSystem;
        private JJPortaudio.Devices.Device remoteInputDevice, remoteOutputDevice;
        private const uint DAXSampleRate = 24000;
        private const uint opusSampleRate = 48000;

        class audioChannelData
        {
            public string Name;
            private object radioStream; // the radio's stream
            // OpusStream for output
            public RXRemoteAudioStream OpusChannel
            {
                get { return (RXRemoteAudioStream)radioStream; }
                set { radioStream = value; }
            }
            // opus input
            public TXRemoteAudioStream TXOpusChannel
            {
                get { return (TXRemoteAudioStream)radioStream; }
                set { radioStream = value; }
            }
            public bool IsOpus;
            public DAXRXAudioStream DaxChannel
            {
                get { return (DAXRXAudioStream)radioStream; }
                set { radioStream = value; }
            }
            // Input stream is for DAX.
            public DAXTXAudioStream InputStream
            {
                get { return (DAXTXAudioStream)radioStream; }
                set { radioStream = value; }
            }
            public bool IsInput;
            public JJAudioStream PortAudioStream;
            public bool Started;
            public bool JustStarted; // used by opus to ignore initial data after start
            public uint DaxID { get { return DaxChannel.StreamID; } }
            public Slice Slice { get; set; }
            // audioChannel for Opus output
            public audioChannelData(RXRemoteAudioStream stream, string name)
            {
                stream.IsCompressed = true;
                OpusChannel = stream;
                Name = name;
                IsOpus = true;
                IsInput = false;
            }
            // audioChannel for Opus input
            public audioChannelData(TXRemoteAudioStream stream, string name)
            {
                TXOpusChannel = stream;
                Name = name;
                IsOpus = true;
                IsInput = true;
            }
            public audioChannelData(DAXRXAudioStream stream, string name)
            {
                DaxChannel = stream;
                Name = name;
                IsOpus = false;
                IsInput = false;
            }
            public audioChannelData(DAXTXAudioStream stream, string name)
            {
                InputStream = stream;
                Name = name;
                IsOpus = false;
                IsInput = true;
            }
        }
        private List<audioChannelData> audioChannels = null; // applies to DAX output channels
        private audioChannelData inputChannel; // for DAX input
        private audioChannelData opusOutputChannel;
        private audioChannelData opusInputChannel;
#if CWMonitor
        private Morse CWMon = null;
        private bool useCWMon { get { return (CWMon != null); } }
#endif

        private audioChannelData findAudioChannelByStream(RXAudioStream stream)
        {
            foreach (audioChannelData chan in audioChannels)
            {
                if (stream.StreamID == chan.DaxID) return chan;
            }
            return null;
        }

        private audioChannelData findAudioChannelBySlice(Slice s)
        {
            foreach (audioChannelData chan in audioChannels)
            {
                if (s.DAXChannel == chan.Slice.DAXChannel) return chan;
            }
            return null;
        }

        private void audioStreamAddedHandler(DAXRXAudioStream stream)
        {
            Tracing.TraceLine("audioStreamAddedHandler:" + stream.DAXChannel, TraceLevel.Info);
            string name = "JJRadio.daxchan" + stream.DAXChannel.ToString();
            audioChannelData chan = new audioChannelData(stream, name);
            chan.DaxChannel.DataReady += new RXAudioStream.DataReadyEventHandler(audioDataHandler);
            chan.DaxChannel.PropertyChanged += new PropertyChangedEventHandler(daxRXAudioStreamPropertyChangedHandler);
            audioChannels.Add(chan);
        }

        private void txAudioStreamAddedHandler(DAXTXAudioStream stream)
        {
            Tracing.TraceLine("txAudioStreamAddedHandler", TraceLevel.Info);
            if (inputChannel == null)
            {
                inputChannel = new audioChannelData(stream, "DAXInput");
            }
            else
            {
                inputChannel.InputStream = stream;
            }
        }

        // for Opus output
        private void opusOutputStreamAddedHandler(RXRemoteAudioStream stream)
        {
            Tracing.TraceLine("opusOutputStreamAddedHandler", TraceLevel.Info);
            string name = "JJRadio.OpusOutputChan";
            opusOutputChannel = new audioChannelData(stream, name);
        }

        private void opusInputStreamAddedHandler(TXRemoteAudioStream stream)
        {
            Tracing.TraceLine("opusInputStreamAddedHandler", TraceLevel.Info);
            string name = "JJRadio.OpusInputChan";
            opusInputChannel = new audioChannelData(stream, name);
        }

        // For DAX output data.
        private void audioDataHandler(RXAudioStream stream, float[] data)
        {
            if (!theRadio.DAXOn) return;
            audioChannelData chan = findAudioChannelByStream(stream);
            if (chan == null)
            {
                Tracing.TraceLine("audioDataHandler:channel not found", TraceLevel.Error);
                return;
            }
            if (chan.Slice.Mute) return;
            lock (chan)
            {
                if (chan.Started)
                {
                    int vfo = SliceToVFO(chan.Slice);
                    float panFactor = GetVFOPan(vfo);
                    int leftRight;
                    if (panFactor < 50)
                    {
                        panFactor /= 50;
                        leftRight = 1; // decrease the right chan
                    }
                    else
                    {
                        panFactor = (100 - panFactor) / 50;
                        leftRight = 0; // decrease the left
                    }
                    for(int i = 0; i < data.Length; i += 2)
                    {
                        data[i + leftRight] *= panFactor;
                    }
                    chan.PortAudioStream.Write(data);
                }
            }
        }

        private Thread remoteAudioThread;
        private bool stopRemoteAudio;

        // Save the LineoutGain here.
        private int savedLineOut = -1;
        private bool _LANAudio;
        public bool LANAudio
        {
            get { return _LANAudio; }
            set
            {
                Tracing.TraceLine("LANAudio:" + value.ToString(), TraceLevel.Info);
                if (_LANAudio != value)
                {
                    if (value)
                    {
                        savedLineOut = LineoutGain;
                        LineoutGain = 0; // turn off main audio.
                        startRemoteAudioThread();
                    }
                    else
                    {
                        if (savedLineOut != -1) LineoutGain = savedLineOut;
                        stopRemoteAudioThread();
                    }
                }
                _LANAudio = value;
            }
        }

        private void startRemoteAudioThread()
        {
            Tracing.TraceLine("startRemoteAudioThread", TraceLevel.Info);
            stopRemoteAudio = false;
            remoteAudioThread = new Thread(remoteAudioProc);
            remoteAudioThread.Name = "RemoteAudio";
            remoteAudioThread.Start();
        }

        private void stopRemoteAudioThread()
        {
            if (remoteAudioThread != null)
            {
                Tracing.TraceLine("stopRemoteAudioThread", TraceLevel.Info);
                stopRemoteAudio = true;
                remoteAudioThread.Join();
                remoteAudioThread = null;
            }
        }

        private void remoteAudioProc()
        {
            Tracing.TraceLine("remoteAudioProc is WAN=" + theRadio.IsWan.ToString(), TraceLevel.Info);
            audioChannels = null;
            inputChannel = null;
            opusOutputChannel = null;
            opusInputChannel = null;
#if CWMonitor
            CWMon = null;
#endif

            // input is from pc.
            string oldMicInput = theRadio.MicInput;
            theRadio.MicInput = "PC";
            if (!await(() =>
            {
                return theRadio.RemoteTxOn;
            }, 1000))
            {
                Tracing.TraceLine("remoteAudioProc:remote tx should be on", TraceLevel.Error);
            }

            audioSystem = new JJPortaudio.Devices(Callouts.AudioDevicesFile);
            // Get the configured devices.
            if (!audioSystem.Setup())
            {
                Tracing.TraceLine("audio setup failed", TraceLevel.Error);
                goto remoteDone;
            }
            remoteInputDevice =
                audioSystem.GetConfiguredDevice(JJPortaudio.Devices.DeviceTypes.input, true);
            if (remoteInputDevice == null)
            {
                Tracing.TraceLine("remoteInputDevice setup error", TraceLevel.Error);
                goto remoteDone;
            }
            remoteOutputDevice =
                audioSystem.GetConfiguredDevice(JJPortaudio.Devices.DeviceTypes.output, true);
            if (remoteOutputDevice == null)
            {
                Tracing.TraceLine("remoteOutputDevice setup error", TraceLevel.Error);
                goto remoteDone;
            }

            // Start the audio subsystem.
            JJPortaudio.Audio.Initialize(remoteInputDevice, remoteOutputDevice);

            // Setup audio channels
            if (theRadio.IsWan)
            {
                theRadio.RequestRXRemoteAudioStream(true); // see opusOutputStreamAddedHandler
                if (!await(() => { return (opusOutputChannel != null); }, 1000))
                {
                    Tracing.TraceLine("remoteAudioProc: opus output channel not added.", TraceLevel.Error);
                    goto remoteDone;
                }
                opusOutputChannel.PortAudioStream = new JJAudioStream();
                opusOutputChannel.PortAudioStream.OpenOpus(Devices.DeviceTypes.output, opusSampleRate);
                Tracing.TraceLine("remoteAudioProc:opusOutputChannel:" + opusOutputChannel.Name + " setup", TraceLevel.Info);

                startOpusOutputChannel();
            }
            else
            {
                // Setup the receive audio.
                audioChannels = new List<audioChannelData>();
                for (int i = 0; i < TotalNumSlices; i++)
                {
                    int daxChan = i + 1;
                    theRadio.RequestDAXRXAudioStream(daxChan); // see audioStreamAddedHandler.
                    if (!await(() => { return (audioChannels.Count == i + 1); }, 1000))
                    {
                        Tracing.TraceLine("remoteAudioProc: audio channel not added: " + i, TraceLevel.Error);
                        goto remoteDone;
                    }
                    theRadio.SliceList[i].DAXChannel = daxChan;
                    audioChannelData chan = audioChannels[i];
                    chan.Slice = theRadio.SliceList[i];
                    chan.PortAudioStream = new JJAudioStream();
                    chan.PortAudioStream.OpenAudio(Devices.DeviceTypes.output, DAXSampleRate);
                    if (myClient(chan.Slice.ClientHandle))
                    {
                        // Start if not muted.
                        if (!chan.Slice.Mute)
                        {
                            startAudioChannel(chan);
                        }
                        else
                        {
                            stopAudioChannel(chan);
                        }
                    }
                    Tracing.TraceLine("remoteAudioProc:AudioChannel:" + chan.Name + " setup", TraceLevel.Info);
                }
                DAXOn = OffOnValues.on;
            }


            // Setup the transmit audio, but don't start the I/O.
            if (theRadio.IsWan)
            {
                theRadio.RequestRemoteAudioTXStream(); // see txOpusStreamAddedHandler
                if (await(() => { return (opusInputChannel != null); }, 1000))
                {
                    opusInputChannel.PortAudioStream = new JJAudioStream();
                    opusInputChannel.PortAudioStream.OpenOpus(Devices.DeviceTypes.input, opusSampleRate, sendOpusInput);
                    Tracing.TraceLine("Opus Input Channel setup", TraceLevel.Info);
                }
                else
                {
                    Tracing.TraceLine("remoteAudioProc: didn't get RemoteAudioTXStream from radio", TraceLevel.Error);
                }
            }
            else
            {
                inputChannel = null;
                theRadio.RequestDAXTXAudioStream(); // see txAudioStreamAddedHandler
                if (await(() => { return (inputChannel != null); }, 1000))
                {
                    inputChannel.PortAudioStream = new JJAudioStream();
                    inputChannel.PortAudioStream.OpenAudio(Devices.DeviceTypes.input, DAXSampleRate, sendAudioInput);
                    Tracing.TraceLine("Local Input Channel setup", TraceLevel.Info);
                }
                else
                {
                    Tracing.TraceLine("remoteAudioProc: didn't get TXAudio stream from radio", TraceLevel.Error);
                }
            }

#if CWMonitor
            // Also need a cw monitor
            CWMonInit();
#endif

            // Main audio loop.
            // Note that we must pole for opus output.
            while (!stopRemoteAudio)
            {
                if (theRadio.IsWan)
                {
                    if (VFOToSlice(TXVFO).DemodMode != "CW")
                    {
                        if (Transmit)
                        {
                            startOpusInputChannel(); // only starts it once
                        }
                        else
                        {
                            stopOpusInputChannel(); // only stops it once.
                        }
                    }

                    // opus receive polling.
                    bool gotPackets = false;
                    // get opus data, even during transmit (for QSK).
                    try
                    {
                        lock (opusOutputChannel)
                        {
                            RXAudioStream stream = opusOutputChannel.OpusChannel;
                            lock (stream.OpusRXListLockObj)
                            {
                                int lastID = stream._opusRXList.Count - 1;
                                // See if have recent packets to process.
                                gotPackets = (!opusOutputChannel.JustStarted && (lastID != -1) &&
                                    (stream.LastOpusTimestampConsumed < stream._opusRXList.Keys[lastID]));
                                if (gotPackets)
                                {
                                    // new packets were added.
                                    // sendPacket is set when need to send packets.
                                    bool sendPacket = !stream._opusRXList.Keys.Contains(stream.LastOpusTimestampConsumed);
                                    // Note that if sendPacket is true, we may have missed packets.
                                    if (sendPacket)
                                    {
                                        Tracing.TraceLine("possible missed packets", TraceLevel.Verbose);
                                    }
                                    foreach (KeyValuePair<double, VitaOpusDataPacket> kvp in stream._opusRXList)
                                    {
                                        if (sendPacket)
                                        {
                                            opusOutputChannel.PortAudioStream.WriteOpus(kvp.Value.payload);
#if opusToFile
                                            writeOpus(kvp.Value.payload);
#endif
                                        }
                                        // See if need to send subsequent packets.
                                        sendPacket = (sendPacket || (kvp.Key == stream.LastOpusTimestampConsumed));
                                        // send all packets following LastOpusTimestampConsumed.
                                    }
                                    stream.LastOpusTimestampConsumed = stream._opusRXList.Keys[lastID];
                                }
                                else
                                {
                                    // ignore packets when just started.
                                    if (opusOutputChannel.JustStarted & (lastID != -1))
                                    {
                                        stream.LastOpusTimestampConsumed = stream._opusRXList.Keys[lastID];
                                        opusOutputChannel.JustStarted = false;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Tracing.TraceLine("remoteAudioProc exception:" + ex.Message, TraceLevel.Info);
                    }
                    if (!gotPackets & !Transmit)
                    {
                        // receiving and didn't send any audio out, wait.
                        Thread.Sleep(1);
                    }
                    else Thread.Yield();
                }
                else
                {
                    if (VFOToSlice(TXVFO).DemodMode != "CW")
                    {
                        // using DAX input, no polling here.
                        if (Transmit)
                        {
                            startLocalInputChannel(); // only starts once.
                        }
                        else
                        {
                            // receiving
                            stopLocalInputChannel();
                        }
                    }
                    Thread.Sleep(10); // see audioDataHandler
                }
            }

            Tracing.TraceLine("stopping remote audio", TraceLevel.Info);

            remoteDone:
#if opusToFile
            closeOpus();
#endif
#if opusInputToFile
#endif
#if CWMonitor
            if (useCWMon) CWMonDone();
#endif

            if (opusOutputChannel != null)
            {
                stopOpusOutputChannel();
                if (opusOutputChannel.OpusChannel != null)
                {
                    opusOutputChannel.OpusChannel.Close();
                }
                if (opusOutputChannel.PortAudioStream != null)
                {
                    opusOutputChannel.PortAudioStream.Close();
                }
            }

            if (opusInputChannel != null)
            {
                stopOpusInputChannel();
                if (opusInputChannel.TXOpusChannel != null)
                {
                    opusInputChannel.TXOpusChannel.Close();
                }
                if (opusInputChannel.PortAudioStream != null)
                {
                    opusInputChannel.PortAudioStream.Close();
                }
            }

            // tear down DAX audio output.
            DAXOn = OffOnValues.off;
            if (audioChannels != null)
            {
                for (int i = 0; i < audioChannels.Count; i++)
                {
                    audioChannelData chan = audioChannels[i];
                    stopAudioChannel(chan);
                    if (chan.PortAudioStream != null)
                    {
                        chan.PortAudioStream.Close();
                    }
                    if (chan.DaxChannel != null)
                    {
                        chan.DaxChannel.Close();
                    }
                }
            }

            if (inputChannel != null)
            {
                stopLocalInputChannel();
                if (inputChannel.InputStream != null)
                {
                    inputChannel.InputStream.Close();
                }
                if (inputChannel.PortAudioStream != null)
                {
                    inputChannel.PortAudioStream.Close();
                }
            }
            Audio.Terminate();
            audioChannels = null;
            inputChannel = null;
            opusOutputChannel = null;
            opusInputChannel = null;
#if CWMonitor
            CWMon = null;
#endif
            // Restore mic input.
            theRadio.MicInput = oldMicInput;

            Tracing.TraceLine("remoteAudioProc exiting", TraceLevel.Info);
        }

        // we need to pass 128 sterio data items to the radio.
        private const int dataNeeded = 128 * 2;
        private float[] outBuf = new float[dataNeeded];
        private int outputPosition = 0;

        // Note:  Called from the audio callback.
        private void sendAudioInput(float[] inBuf)
        {
#if feedback
            audioDataHandler(audioChannels[theRadio.ActiveSlice.Index].DaxChannel, inBuf);
#else
            if (inBuf.Length == 0) return; // happens when finished.

            // initially dataAvailable > dataNeeded.
            int inputPosition = 0;
            // Fill in and write the output buffer if needed.
            if (outputPosition > 0)
            {
                inputPosition = dataNeeded - outputPosition; // initial copy length
                Array.ConstrainedCopy(inBuf, 0, outBuf, outputPosition, inputPosition);
                inputChannel.InputStream.AddTXData(outBuf);
                outputPosition = 0;
            }

            int dataAvailable = inBuf.Length - inputPosition;
            // Copy and write whole input buffers.
            while (dataAvailable >= dataNeeded)
            {
                dataAvailable -= dataNeeded;
                Array.ConstrainedCopy(inBuf, inputPosition, outBuf, outputPosition, dataNeeded);
                inputPosition += dataNeeded;
                inputChannel.InputStream.AddTXData(outBuf);
            }

            // Copy any remaining input to output buffer.
            if (dataAvailable > 0)
            {
                Array.ConstrainedCopy(inBuf, inputPosition, outBuf, 0, dataAvailable);
                outputPosition = dataAvailable;
            }
#endif
        }

        // Note:  Called from the audio callback.
        private void sendOpusInput(byte[] data)
        {
#if opusInputToFile
            opusIn_writeOpus(data);
#endif
            if (data.Length > 0)
            {
                opusInputChannel.TXOpusChannel.AddTXData(data);
            }
            else { }
        }

#if opusToFile
        private const string fName = @"c:\users\jjs\documents\tmp\opusOut.dat";
        private Stream fStream = null;
        private BinaryWriter fbw = null;
        private void writeOpus(byte[] buf)
        {
            if (fStream == null)
            {
                fStream = File.Open(fName, FileMode.Create);
                fbw = new BinaryWriter(fStream);
            }
            fbw.Write((ushort)buf.Length);
            fbw.Write(buf, 0, buf.Length);
        }
        private void closeOpus()
        {
            if (fStream != null)
            {
                fbw.Close();
                fbw.Dispose();
                fStream.Dispose();
            }
        }
#endif
#if opusInputToFile
        private const string opusIn_fName = @"c:\users\jjs\documents\tmp\opusOut.dat";
        private Stream opusIn_fStream = null;
        private BinaryWriter opusIn_fbw = null;
        private void opusIn_writeOpus(byte[] buf)
        {
            if (opusIn_fStream == null)
            {
                opusIn_fStream = File.Open(opusIn_fName, FileMode.Create);
                opusIn_fbw = new BinaryWriter(opusIn_fStream);
            }
            opusIn_fbw.Write((ushort)buf.Length);
            opusIn_fbw.Write(buf, 0, buf.Length);
        }
        private void opusIn_closeOpus()
        {
            if (opusIn_fStream != null)
            {
                opusIn_fbw.Close();
                opusIn_fbw.Dispose();
                opusIn_fStream.Dispose();
            }
        }
#endif

        private void startAudioChannel(audioChannelData chan)
        {
            Tracing.TraceLine("startAudioChannel:" + chan.Name, TraceLevel.Info);
            lock (chan)
            {
                chan.JustStarted = true; // set on each call
                if (chan.Started) return;
                Tracing.TraceLine("startAudioChannel:" + chan.Name, TraceLevel.Info);
                chan.Started = chan.PortAudioStream.StartAudio();
                if (!chan.Started)
                {
                    Tracing.TraceLine("startAudioOutputChannel portAudio didn't start", TraceLevel.Error);
                }
            }
        }

        private void stopAudioChannel(audioChannelData chan)
        {
            Tracing.TraceLine("stopAudioChannel:" + chan.Name, TraceLevel.Info);
            lock (chan)
            {
                if (!chan.Started) return;
                chan.Started = false;
                chan.PortAudioStream.StopAudio();
            }
        }

        private void startOpusOutputChannel()
        {
            Tracing.TraceLine("startOpusOutputChannel:" + 
                opusOutputChannel.Name + ' ' + opusOutputChannel.Started.ToString(), TraceLevel.Info);
            lock (opusOutputChannel)
            {
                opusOutputChannel.JustStarted = true; // set on each call
                opusOutputChannel.OpusChannel.RxMute = false;
                if (opusOutputChannel.Started) return;
                opusOutputChannel.OpusChannel.RXGain = 50;
                opusOutputChannel.Started = opusOutputChannel.PortAudioStream.StartAudio();
                if (!opusOutputChannel.Started)
                {
                    Tracing.TraceLine("startOpusOutputChannel portAudio didn't start", TraceLevel.Error);
                }
            }
        }

        private void stopOpusOutputChannel()
        {
            Tracing.TraceLine("stopOpusOutputChannel:" +
                opusOutputChannel.Name + ' ' + opusOutputChannel.Started.ToString(), TraceLevel.Info);
            lock (opusOutputChannel)
            {
                opusOutputChannel.OpusChannel.RxMute = true;
                if (!opusOutputChannel.Started) return;
                opusOutputChannel.Started = false;
                opusOutputChannel.PortAudioStream.StopAudio();
            }
        }

        private void startLocalInputChannel()
        {
            lock (inputChannel)
            {
                if (inputChannel.Started) return;
                Tracing.TraceLine("startLocalInputChannel", TraceLevel.Info);
                inputChannel.InputStream.TXGain = 50; // tbd
                inputChannel.InputStream.Transmit = true;
                inputChannel.Started = inputChannel.PortAudioStream.StartAudio();
                if (!inputChannel.Started)
                {
                    Tracing.TraceLine("startInputChannel portAudio didn't start", TraceLevel.Error);
                }
            }
        }

        private void startOpusInputChannel()
        {
            lock (opusInputChannel)
            {
                if (opusInputChannel.Started) return;
                Tracing.TraceLine("startOpusInputChannel", TraceLevel.Info);
                opusInputChannel.Started = opusInputChannel.PortAudioStream.StartAudio();
                if (!opusInputChannel.Started)
                {
                    Tracing.TraceLine("startOpusInputChannel portAudio didn't start", TraceLevel.Error);
                }
            }
        }

        private void stopLocalInputChannel()
        {
            lock (inputChannel)
            {
                if (!inputChannel.Started) return;
                Tracing.TraceLine("stopLocalInputChannel", TraceLevel.Info);
                inputChannel.PortAudioStream.StopAudio();
                inputChannel.Started = false;
                inputChannel.InputStream.Transmit = false;
            }
        }

        private void stopOpusInputChannel()
        {
            lock (opusInputChannel)
            {
                if (!opusInputChannel.Started) return;
                Tracing.TraceLine("stopOpusInputChannel", TraceLevel.Info);
                opusInputChannel.PortAudioStream.StopAudio();
                opusInputChannel.Started = false;
            }
        }

#if CWMonitor
        // Remote CW monitor
        private void CWMonInit()
        {
            Tracing.TraceLine("CWMonInit", TraceLevel.Info);
            CWMon = new Morse();
            lock (CWMon)
            {
                CWMon.Speed = (uint)theRadio.CWSpeed;
                CWMon.Frequency = (uint)theRadio.CWPitch;
                if ((theRadio.ActiveSlice == VFOToSlice(TXVFO)) && (theRadio.ActiveSlice.DemodMode == "CW"))
                {
                    CWMonStart();
                }
            }
        }

        // The monitor is started and stopped when we go in or out of transmit.
        private void CWMonStart()
        {
            Tracing.TraceLine("CWMonStart", TraceLevel.Info);
            lock (CWMon)
            {
                CWMon.Start();
                CWMon.Frequency = (uint)theRadio.CWPitch;
                CWMon.Speed = (uint)theRadio.CWSpeed;
                CWMon.Volume = theRadio.TXCWMonitorGain;
            }
        }

        private void CWMonStop()
        {
            Tracing.TraceLine("CWMonStop", TraceLevel.Info);
            lock (CWMon)
            {
                CWMon.Stop();
            }
        }

        private void CWMonDone()
        {
            Tracing.TraceLine("CWMonDone", TraceLevel.Info);
            lock (CWMon)
            {
                CWMonStop();
                CWMon.Close();
            }
        }
#endif
#endregion

        // region - cw
#region cw
        class cwText
        {
            public string Text;
            public bool Stop;
            public cwText()
            {
                Stop = true;
            }
            public cwText(string str)
            {
                Text = str;
            }
        }
        public bool SendCW(string str)
        {
            q.Enqueue(new cwText(str));
            return true;
        }
        public void StopCW()
        {
            q.Enqueue(new cwText());
        }

        protected Flex6300Filters FilterObj;
        public void CWZeroBeat()
        {
            ulong freq = 0;
            if (FilterObj != null) freq = FilterObj.ZeroBeatFreq();
            Tracing.TraceLine("CWZeroBeatFreq:" + freq.ToString(), TraceLevel.Info);
            if (freq != 0)
            {
                RITData r = RIT;
                if (r.Active)
                {
                    r.Value = (int)((long)freq - (long)RXFrequency);
                    RIT = r;
                }
                else RXFrequency = freq;
            }
        }
#endregion

#region parms
        /// <summary>
        /// Receive decoded CW.
        /// </summary>
        /// <param name="txt">the text string</param>
        public delegate void DCWText(string txt);
        /// <summary>
        /// Format the frequency for display
        /// </summary>
        /// <param name="freq">a ulong</param>
        /// <returns>string to display</returns>
        public delegate string FormatFreqDel(ulong freq);
        /// <summary>
        /// format a frequency string for the radio.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>a ulong frequency</returns>
        public delegate ulong FormatFreqForRadioDel(string str);
        /// <summary>
        /// Get the displayable SWR.
        /// </summary>
        /// <returns>SWR string</returns>
        public delegate string GetSWRTextDel();
        /// <summary>
        /// rig-dependent next value of this field.
        /// </summary>
        public delegate void NextValue1Del();

        /// <summary>
        /// Callout vector
        /// </summary>
        public class OpenParms
        {
            /// <summary>
            /// the program name.
            /// </summary>
            public string ProgramName;
            public DCWText CWTextReceiver { get; set; }
            internal void safeCWTextReceiver(string txt)
            {
                try { CWTextReceiver(txt); }
                catch (Exception ex)
                { Tracing.ErrMessageTrace(ex, false, false); }
            }
            /// <summary>
            /// Format a frequency for display
            /// </summary>
            public FormatFreqDel FormatFreq;
            /// <summary>
            /// format a string frequency for the radio
            /// </summary>
            public FormatFreqForRadioDel FormatFreqForRadio;
            /// <summary>
            /// Go to the home field.
            /// </summary>
            public delegate void GotoHomeDel();
            /// <summary>
            /// Go to the home field.
            /// </summary>
            public GotoHomeDel GotoHome;
            /// <summary>
            /// Configuration directory
            /// </summary>
            public string ConfigDirectory;
            /// <summary>
            /// Name of audio device selection file.
            /// </summary>
            public string AudioDevicesFile;
            public delegate string GetOperatorNameDel();
            /// <summary>
            /// Function to retrieve the current operator's name.
            /// </summary>
            public GetOperatorNameDel GetOperatorName;
            internal string OperatorName { get { return GetOperatorName(); } }
            /// <summary>
            /// Station name
            /// </summary>
            public string StationName;
            /// <summary>
            /// Braille display cells
            /// </summary>
            public int BrailleCells;
            /// <summary>
            /// Operator's license class.
            /// </summary>
            public Bands.Licenses License;
            /// <summary>
            /// Send CW with no preprocessing.
            /// </summary>
            public bool DirectSend;
            /// <summary>
            /// panning field
            /// </summary>
            public Control PanField;
            /// <summary>
            /// Get the displayable SWR.
            /// </summary>
            public GetSWRTextDel GetSWRText = null;
            /// <summary>
            /// rig-dependent next value.
            /// </summary>
            public NextValue1Del NextValue1;
        }
        /// <summary>
        /// Callout vector provided at open().
        /// </summary>
        internal OpenParms Callouts;
        internal string ConfigDirectory { get { return Callouts.ConfigDirectory; } }
        internal string OperatorName { get { return Callouts.OperatorName; } }
        /// <summary>
        /// Operator's directory for rig-specific stuff.
        /// </summary>
        internal string OperatorsDirectory { get { return ConfigDirectory + "\\" + OperatorName; } }

        // Formatters from callouts.
        internal static FormatFreqDel FormatFreq;
#endregion

        /// <summary>
        /// Rig's capabilities
        /// </summary>
        public RigCaps MyCaps;

        internal class q_t
        {
            //private Queue q;
            private BlockingCollection<object> q;
            public q_t()
            {
                //q = Queue.Synchronized(new Queue());
                q = new BlockingCollection<object>();
            }

            public bool MainLoop { get; set; }

            public int Count { get { return q.Count; } }

            public void Enqueue(object o, bool beforeMainLoop = false)
            {
                if (!MainLoop)
                {
                    // If can execute before the main loop, do it.
                    if (beforeMainLoop & (o is FunctionDel))
                    {
                        FunctionDel func = (FunctionDel)o;
                        if (func != null) func();
                    }
                    else Tracing.TraceLine("q:outside main loop", TraceLevel.Error);
                }
                else
                {
                    q.Add(o);
                }
            }

            public object Dequeue()
            {
                return q.Take();
            }
        }
        internal q_t q;

        public FlexBase(OpenParms p)
        {
            Tracing.TraceLine("Flex constructor", TraceLevel.Info);
            theRadio = null;
            _apiInit = false;
            wan = null;

            Callouts = p;
            FormatFreq = p.FormatFreq;
            MyCaps = new RigCaps(RigCaps.DefaultCapsList);
            // default tuner type.
            setFlexTunerTypeNotAuto();

            FMToneModes = myFMToneModes;
            // Use the TS590 fm tone values.
            ToneFrequencyTable = myToneFrequencyTable;

            FilterObj = new Flex6300Filters(this); // Sets up RigFields.

            q = new q_t();

            API.ProgramName = p.ProgramName;
            API.IsGUI = true;

            p.NextValue1 = setNextValue1;
            p.GetSWRText = SWRText;
        }

        // main thread region
#region mainThread
        private bool stopMainThread;
        internal delegate void FunctionDel();

        private const string JJRadioDefault = "JJRadioDefault";
        internal string CurrentProfile; // profile in-use.
        private string preferredProfile;
        private List<string> defaultProfiles;
        private void mainThreadProc()
        {
            Tracing.TraceLine("mainThreadProc", TraceLevel.Info);
            defaultProfiles = new List<string>();
            // add in order of preference.
            preferredProfile = Callouts.StationName + "profile";
            defaultProfiles.Add(preferredProfile);
            defaultProfiles.Add(JJRadioDefault);

            try
            {
                // Must have an antenna.
                if (!await(() =>
                {
                    return ((theRadio.RXAntList != null) && (theRadio.RXAntList.Length > 0));
                }, 5000))
                {
                    Tracing.TraceLine("Flex open:no RX antenna", TraceLevel.Error);
                    raiseNoSliceError(noRXAnt);
                    return;
                }

                // make sure we get at least 1 slice.
                if (!await(() =>
                {
                    return (MyNumSlices >= 1);
                }, 5000))
                {
                    Tracing.TraceLine("Flex open:didn't get a slice", TraceLevel.Error);
                    raiseNoSliceError(noSlice);
                    return;
                }

                // wait until the station name is set.
                if (await(() =>
                {
                    GUIClient client = theRadio.FindGUIClientByClientHandle(clientHandle);
                    if (client == null) return false;
                    return (client.Station == Callouts.StationName);
                }, 10000))
                {
                    Tracing.TraceLine("flex open:station name set " + Callouts.StationName, TraceLevel.Info);
                }
                else
                {
                    Tracing.TraceLine("Flex open:didn't get a station name", TraceLevel.Error);
                    raiseNoSliceError(noStation);
                    return;
                }

#if Profile
                // If JJRadioDefault was loaded, select it and await the pan and slices.
                if (GetProfileInfo(false))
                    {
                        Tracing.TraceLine("flex open:got profile", TraceLevel.Info);
                    }
                    else
                    {
                        setupFromScratch();
                    }
#else
                setupFromScratch();
#endif

                // Set these on every open.
                Tracing.TraceLine("flex open:#VFOs " + MyNumSlices, TraceLevel.Info);

                if (!RemoteRig)
                {
                    theRadio.MicInput = "mic";
                }
                if (!await(() =>
                {
                    return !theRadio.RemoteTxOn;
                }, 1000))
                {
                    Tracing.TraceLine("Flex open:remote tx should be off", TraceLevel.Error);
                }

                theRadio.TX1Enabled = true;
                // Turn the Vox off.
                theRadio.SimpleVOXEnable = false;
                theRadio.CWBreakIn = false;

                // Ok to queue commands now.
                q.MainLoop = true;
                Tracing.TraceLine("flex open:q.mainloop" + q.MainLoop.ToString(), TraceLevel.Info);

                cwx = theRadio.GetCWX();
                cwx.Delay = theRadio.CWDelay;
                cwx.Speed = theRadio.CWSpeed;
                cwx.CharSent += new CWX.CharSentEventHandler(charSentHandler);

                // temporary changes for Flex issue #7620.
                Tracing.TraceLine("#7620", TraceLevel.Info);
                //BreakinDelay = 300;
                SidetoneGain = 55;
                //theRadio.TXCWMonitorGain = 55;
                Keyer = IambicValues.off;
                //theRadio.CWIambic = false;
                KeyerSpeed = 20;
                //theRadio.CWSpeed = 20;
                //cwx.Speed = 20;

                // Setup pan adapter display.
                if (PanSetup != null)
                {
                    PanSetup();
                    RXFreqChange(theRadio.ActiveSlice);
                }

                if (theRadio.IsWan)
                {
                    startRemoteAudioThread();
                }

                raisePowerEvent(true);

                // Main loop.
                while (!stopMainThread)
                {
                    while (q.Count > 0)
                    {
                        object el = q.Dequeue();
                        if (el is FunctionDel)
                        {
                            FunctionDel func = (FunctionDel)el;
                            if (func != null) func();
                        }
                        else if (el is cwText)
                        {
                            cwText cwt = (cwText)el;
                            if (cwt.Stop) stopCW();
                            else sendText(cwt.Text);
                        }
                    }

                    Thread.Sleep(25);
                    //Thread.Yield();
                }
                q.MainLoop = false;

                SaveProfile(true);

                raisePowerEvent(false);
            }
            catch (ThreadAbortException) { Tracing.TraceLine("mainThread abort", TraceLevel.Error); }
            catch (Exception ex)
            {
                Tracing.ErrMessageTrace(ex, true);
            }
        }

        internal void SaveProfile(bool delay)
        {
#if Profile
            //theRadio.SaveTXProfile(CurrentProfile);
            theRadio.SaveGlobalProfile(CurrentProfile);
            if (delay) Thread.Sleep(2000);
            Tracing.TraceLine("preferred profile saved:" + preferredProfile, TraceLevel.Info);
#endif
        }

        /// <summary>
        /// Select the default profile if loaded.
        /// Before calling, call RaisePowerOff(), and PowerOn() when ready afterwards.
        /// </summary>
        /// <returns>true if selected and the info is loaded.</returns>
        /// <remarks>
        /// On an import, we'll wait for radio status of In_Use, then select the profile.
        /// </remarks>
        internal bool GetProfileInfo(bool postImport)
        {
            Tracing.TraceLine("getProfileInfo:" + postImport.ToString(), TraceLevel.Info);
            bool rv = false;

#if Profile
            // Await to see if CurrentProfile is in the profile list.
            Tracing.TraceLine("getProfileInfo:awaiting default profile in GlobalProfileList", TraceLevel.Info);
            CurrentProfile = null;
            foreach (string name in defaultProfiles)
            {
                if (await(() =>
                {
                    return (theRadio.ProfileGlobalList.Contains(name) & theRadio.ProfileTXList.Contains(name));
                }, 3000))
                {
                    CurrentProfile = name;
                    break;
                }
            }
            if (CurrentProfile != null)
            {
                // load the selected profile.
                Tracing.TraceLine("getProfileInfo:profile present " + CurrentProfile, TraceLevel.Info);
                // Select the current profile and wait til loaded.
                globalProfileLoaded = txProfileLoaded = false;
                theRadio.ProfileGlobalSelection = CurrentProfile;
                theRadio.ProfileTXSelection = CurrentProfile;
                // Wait til loaded. (long wait)
                if (await(() =>
                {
                    return (globalProfileLoaded & txProfileLoaded);
                }, 20000))
                {
                    Tracing.TraceLine("getProfileInfo:profile loaded " + CurrentProfile, TraceLevel.Info);
                    Tracing.TraceLine("dbg1panAdaptersRemaining:" + theRadio.PanadaptersRemaining);
                    rv = true;

                    // Allocate any free slices.
                    if (theRadio.PanadaptersRemaining > 0)
                    {
                        Tracing.TraceLine("GetProfileInfo:allocating free slices " + theRadio.PanadaptersRemaining, TraceLevel.Info);
                        int oldRXVFO = RXVFO;
                        int oldTXVFO = TXVFO;
                        int oldNumSlices = MyNumSlices;
                        while (theRadio.PanadaptersRemaining > 0)
                        {
                            int n = MyNumSlices;
                            theRadio.RequestPanafall();
                            if (await(() =>
                             {
                                 return (MyNumSlices > n);
                             }, 2000))
                            {
                                //Thread.Sleep(20); // wait a bit
                                //VFOToSlice(n).Mute = true;
                            }
                            else
                            {
                                Tracing.TraceLine("GetProfileInfo:free slice not allocated", TraceLevel.Error);
                            }
                        }

                        _RXVFO = oldRXVFO;
                        if (_RXVFO != noVFO) mySlices[_RXVFO].Active = true;
                        _TXVFO = oldTXVFO;
                        if (_TXVFO != noVFO) mySlices[_TXVFO].IsTransmitSlice = true;
                    }

                    // now make sure we're using the preferred profile.
                    if (preferredProfile != CurrentProfile)
                    {
                        CurrentProfile = preferredProfile;
                        Tracing.TraceLine("getProfileInfo:creating profile " + CurrentProfile, TraceLevel.Info);
                        createProfile();
                        theRadio.ProfileGlobalSelection = CurrentProfile;
                        theRadio.ProfileTXSelection = CurrentProfile;
                        SaveProfile(false);
                    }
                }
                else
                {
                    if (!globalProfileLoaded) Tracing.TraceLine("GetProfileInfo:global profile not loaded ", TraceLevel.Error);
                    if (!txProfileLoaded) Tracing.TraceLine("GetProfileInfo:TX profile not loaded ", TraceLevel.Error);
                    // rv still false.
                }
            }
            else
            {
                Tracing.TraceLine("GetProfileInfo:profile not found " + CurrentProfile, TraceLevel.Error);
                // rv still false.
            }
#else
            rv = true;
#endif

            _TotalNumSlices = theRadio.SliceList.Count;
            Tracing.TraceLine("dbg2:" + TotalNumSlices);

            if (postImport)
            {
                Tracing.TraceLine("flex import operation complete:" + rv.ToString(), TraceLevel.Info);
                if ((RXFreqChange != null) & (theRadio.ActiveSlice != null))
                {
                    RXFreqChange(theRadio.ActiveSlice);
                }
                raisePowerEvent(true);
                Directory.Delete(importDir, true);
                string msg = (rv) ? importedMsg : importFailMsg;
                MessageBox.Show(msg, statusHdr, MessageBoxButtons.OK);
            }
            return rv;
        }

        private bool setupFromScratch()
        {
            bool rv;
            // Radio was reset or never used with JJRadio before this.
            Tracing.TraceLine("setupFromScratch:", TraceLevel.Info);
            // function to get pan adapters.
            while (theRadio.PanadaptersRemaining > 0)
            {
                int rem = theRadio.PanadaptersRemaining;
                theRadio.RequestPanafall();
                // wait for at least one new pan adapter
                if (!await(() =>
                {
                    return (theRadio.PanadaptersRemaining < rem);
                }, 5000))
                {
                    Tracing.TraceLine("setupFromScratch:didn't get a pan adapter " + theRadio.PanadaptersRemaining, TraceLevel.Error);
                    break;
                }
            }
            rv = (theRadio.PanadaptersRemaining == 0);
            if (rv)
            {
                Tracing.TraceLine("setupFromScratch:have pan and slices:" + MyNumPanadapters, TraceLevel.Info);
                // We have pan adapters and slices, so we're done.
                VFOToSlice(0).Active = true;
                VFOToSlice(0).Mute = false;
                Tracing.TraceLine("setupFromScratch:have 1 active slice:" + (MyNumSlices-1), TraceLevel.Info);
                for (int i = 1; i < MyNumSlices; i++)
                {
                    mySlices[i].Mute = true;
                }

                VFOToSlice(RXVFO).TXAnt = theRadio.RXAntList[0];
                if (CanTransmit)
                {
                    _TXVFO = 0;
                    VFOToSlice(TXVFO).IsTransmitSlice = true;
                    VFOToSlice(TXVFO).TXAnt = theRadio.RXAntList[0];
                    theRadio.RFPower = 100;
                    theRadio.CWBreakIn = false;
                    theRadio.CWIambic = false;
                    theRadio.SpeechProcessorEnable = true;
                    theRadio.SimpleVOXEnable = false;

                    CurrentProfile = preferredProfile;
                    createProfile();
                }

                Tracing.TraceLine("setupFromScratch:radio setup", TraceLevel.Info);
            }
            else
            {
                Tracing.TraceLine("setupFromScratch:didn't get pans and slices:" + MyNumPanadapters + ' ' + MyNumSlices, TraceLevel.Error);
            }
            _TotalNumSlices = theRadio.SliceList.Count;
            Tracing.TraceLine("dbg3:" + TotalNumSlices);
            return rv;
        }

        private bool createProfile()
        {
            bool rv = true;
            Tracing.TraceLine("createProfile:" + CurrentProfile, TraceLevel.Info);
            theRadio.CreateTXProfile(CurrentProfile);
            if (!await(() =>
            {
                return theRadio.ProfileTXList.Contains(CurrentProfile);
            }, 2000))
            {
                Tracing.TraceLine("setupFromScratch:TX profile not created", TraceLevel.Error);
                rv = false;
            }
            return rv;
        }

        /// <summary>
        /// Flex Antenna tuner start/stop interrupt argument
        /// </summary>
        public class FlexAntTunerArg
        {
            public string Type;
            public string Status;
            public string SWR; // Good when stopped
            public FlexAntTunerArg(FlexTunerTypes type, ATUTuneStatus status, float swr)
            {
                Type = type.ToString();
                Status = status.ToString();
                SWR = swr.ToString("f1");
            }
            // Used to send a message
            public FlexAntTunerArg(string status)
            {
                Status = status;
                Type = null;
                SWR = null;
            }
        }
        public delegate void FlexAntTunerStartStopDel(FlexAntTunerArg arg);
        /// <summary>
        /// Antenna tuner start/stop event
        /// </summary>
        public event FlexAntTunerStartStopDel FlexAntTunerStartStop;
        internal void RaiseFlexAntTuneStartStop(FlexAntTunerArg arg)
        {
            if (FlexAntTunerStartStop != null)
            {
                Tracing.TraceLine("FlexAntTunerStartStop raised:" + arg.Type + ' ' + arg.Status + ' ' + arg.SWR, TraceLevel.Info);
                FlexAntTunerStartStop(arg);
            }
            else Tracing.TraceLine("FlexAntTunerStartStop not raised", TraceLevel.Verbose);
        }

        /// <summary>
        /// Argument for CapsChangeEvent
        /// </summary>
        public class CapsChangeArg
        {
            public RigCaps NewCaps;
            internal CapsChangeArg(RigCaps caps)
            {
                NewCaps = caps;
            }
        }
        public delegate void CapsChangeDel(CapsChangeArg arg);
        /// <summary>
        /// Raised when rig's capabilities change.
        /// </summary>
        public event CapsChangeDel CapsChangeEvent;
        private void raiseCapsChange(CapsChangeArg arg)
        {
            if (CapsChangeEvent != null)
            {
                Tracing.TraceLine("raiseCapsChange arg:" + +' ' + ((ulong)arg.NewCaps.setCaps).ToString("x"), TraceLevel.Error);
                CapsChangeEvent(arg);
            }
            else Tracing.TraceLine("raiseCapsChange not raised", TraceLevel.Error);
        }

        /// <summary>
        /// FlexControlKnob status
        /// </summary>
        /// <remarks>
        /// This is reported with the KnobOnOffEvent
        /// </remarks>
        public bool KnobStatus { get; set; }

        public delegate void KnobOnOffEventDel(object sender, bool OnOff);
        /// <summary>
        /// FlexKnob on/off event
        /// </summary>
        public event KnobOnOffEventDel KnobOnOffEvent;
        internal void raiseKnobOnnOff(bool onOff)
        {
            KnobStatus = onOff;
            if (KnobOnOffEvent != null)
            {
                KnobOnOffEvent(this, onOff);
            }
        }

        /// <summary>
        /// power status event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="power">event argument</param>
        public delegate void PowerStatusHandler(object sender, bool power);
        /// <summary>
        /// power status event
        /// </summary>
        public event PowerStatusHandler PowerStatus;
        private void raisePowerEvent(bool on)
        {
            bool raise = (PowerStatus != null);
            Tracing.TraceLine("raisePowerEvent:" + on.ToString() + ' ' + raise.ToString(), TraceLevel.Info);
            if (raise)
            {
                PowerStatus(this, on);
            }
        }

        public delegate void TransmitChangeDel(object sender, bool value);
        /// <summary>
        /// Transmit status change event.
        /// </summary>
        public event TransmitChangeDel TransmitChange;
        private void raiseTransmitChange(bool status)
        {
            if (TransmitChange != null)
            {
                Tracing.TraceLine("raising TransmitChange:" + status.ToString(), TraceLevel.Info);
                TransmitChange(this, status);
            }
        }
        private string importDir;
        internal void ImportProfile(string name)
        {
            // Save the import temp directory.
            importDir = name.Substring(0, name.LastIndexOf('\\'));
            raisePowerEvent(false);
            theRadio.DatabaseImportComplete = false;
            theRadio.SendDBImportFile(name);
        }

        private CWX cwx;
        private void sendText(string str)
        {
            //if (string.IsNullOrEmpty(str) || (Vox == OffOnValues.off)) return;
            if ((theRadio == null) | string.IsNullOrEmpty(str)) return;
            // send only if in transmit mode or vox is on.
            if (!(Transmit | (Vox == OffOnValues.on))) return;

            if (str[str.Length - 1] != ' ') str += " ";
            cwx.Send(str);
#if CWMonitor
            //sentChars.Append(str);
            if (useCWMon)
            {
                CWMon.Send(str);
            }
#endif
            Tracing.TraceLine("SendCW:" + str, TraceLevel.Info);
        }
        private void stopCW()
        {
            cwx.ClearBuffer();
#if CWMonitor
            if (useCWMon)
            {
                CWMonStop();
                CWMonStart();
            }
#endif
        }
#endregion

        // region - Memory stuff
#region memories
        /// <summary>
        /// current memory channel or -1.
        /// </summary>
        public int CurrentMemoryChannel
        {
            get
            {
                return ((NumberOfMemories > 0) && (memoryHandling != null)) ?
                  memoryHandling.CurrentMemoryChannel : -1;
            }
            set
            {
                if (memoryHandling != null) memoryHandling.CurrentMemoryChannel = value;
            }
        }

        /// <summary>
        /// Number of memories
        /// </summary>
        public int NumberOfMemories
        {
            get { return (memoryHandling == null) ? 0 : memoryHandling.NumberOfMemories; }
        }

        /// <summary>
        /// Select CurrentMemoryChannel's memory.
        /// </summary>
        /// <returns>true on success</returns>
        public bool SelectMemory()
        {
            Tracing.TraceLine("SelectMemory:" + CurrentMemoryChannel, TraceLevel.Info);
            if (memoryHandling != null)
            {
                return memoryHandling.SelectMemory();
            }
            else return false;
        }

        /// <summary>
        /// Select the named memory.
        /// </summary>
        /// <returns>true on success</returns>
        public bool SelectMemoryByName(string name)
        {
            Tracing.TraceLine("SelectMemoryByName:" + name, TraceLevel.Info);
            if (memoryHandling != null)
            {
                return memoryHandling.SelectMemoryByName(name);
            }
            else return false;
        }

        internal static string FullMemoryName(Memory m)
        {
            string name = (string.IsNullOrEmpty(m.Name)) ? m.Freq.ToString("F6") : m.Name;
            string group = (string.IsNullOrEmpty(m.Group)) ? "" : m.Group + '.';
            return group + name;
        }

        /// <summary>
        /// Get sorted list of full memory names.
        /// </summary>
        public List<string> MemoryNames()
        {
            List<string> rv;
            if (memoryHandling != null)
            {
                rv = memoryHandling.MemoryNames();
            }
            else
            {
                rv = new List<string>();
            }
            return rv;
        }

        /// <summary>
        /// Memory scan group
        /// </summary>
        public class ScanGroup
        {
            public string Name { get; set; }
            public List<string> Members;
            public bool Readonly; // false for a user-group
            public ScanGroup() { }
            public ScanGroup(string name, List<string> members, bool rdonly = false)
            {
                Name = name;
                Members = members;
                Readonly = rdonly;
            }
            public ScanGroup(ScanGroup group, FlexBase parent)
            {
                Name = group.Name;
                Readonly = false; // a user group.
                Members = new List<string>();
                // Add any group member that's still valid.
                foreach (Memory m in parent.theRadio.MemoryList)
                {
                    if (group.Members.Contains(m.Name))
                    {
                        Members.Add(m.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Get reserved scan groups, default is none.
        /// </summary>
        public List<ScanGroup> GetReservedGroups()
        {
            List<ScanGroup> rv = new List<ScanGroup>();
            if (memoryHandling == null) return rv;

            // Get list of all the rig's groups.
            List<string> myGroups = new List<string>();
            foreach(FlexMemories.MemoryElement el in memoryHandling.SortedMemories)
            {
                Memory m = el.Value;
                // if (!string.IsNullOrEmpty(m.Group) && !myGroups.Contains(m.Group))
                if (!myGroups.Contains(m.Group))
                {
                    myGroups.Add(m.Group);
                }
            }
            // Done if no memories.
            if (myGroups.Count == 0) return rv;

            // For each group, add the members.
            foreach(string group in myGroups)
            {
                List<string> memories = new List<string>();
                foreach(FlexMemories.MemoryElement el in memoryHandling.SortedMemories)
                {
                    Memory m = el.Value;
                    if (m.Group == group) memories.Add(FullMemoryName(m));
                }
                // Add the readOnly group.
                rv.Add(new ScanGroup(group, memories, true));
            }
            return rv;
        }
#endregion

        // Used for rig-specific functions.
        public delegate void updateDel();
        /// <summary>
        /// Allow the main program to access the radio's controls (see Flex6300Filter.cs)
        /// </summary>
        public class RigFields_t
        {
            /// <summary>
            /// RigFields form control
            /// </summary>
            public Control RigControl;
            /// <summary>
            /// RigFields update function.
            /// </summary>
            public updateDel RigUpdate;
            /// <summary>
            /// Memory info and display form.
            /// </summary>
            public Form Memories;
            /// <summary>
            /// Menu display form (unused)
            /// </summary>
            public Form Menus;
            /// <summary>
            /// Screen fields list.
            /// </summary>
            public Control[] ScreenFields;
            internal RigFields_t(Control c, updateDel rtn)
            {
                setup(c, rtn, null, null, null);
            }
            internal RigFields_t(Control c, updateDel rtn, Form f)
            {
                setup(c, rtn, f, null, null);
            }
            internal RigFields_t(Control c, updateDel rtn, Form mem, Form mnu)
            {
                setup(c, rtn, mem, mnu, null);
            }
            internal RigFields_t(Control c, updateDel rtn, Form mem, Form mnu,
                Control[] s)
            {
                setup(c, rtn, mem, mnu, s);
            }
            private void setup(Control c, updateDel rtn,
                Form mem, Form mnu, Control[] s)
            {
                RigControl = c;
                RigUpdate = rtn;
                Memories = mem;
                Menus = mnu;
                ScreenFields = s;
            }
            /// <summary>
            /// Close down the forms.
            /// </summary>
            internal void Close()
            {
                if (RigControl != null)
                {
                    RigControl.Dispose();
                    RigControl = null;
                }
                if (Memories != null)
                {
                    Memories.Dispose();
                    Memories = null;
                }
                if (Menus != null)
                {
                    Menus.Dispose();
                    Menus = null;
                }
            }
        }
        /// <summary>
        /// Gets the rig-specific fields
        /// </summary>
        public RigFields_t RigFields
        {
            get;
            internal set;
        }

        private FlexMemories memoryHandling
        {
            get { return ((RigFields != null) && (RigFields.Memories != null)) ? (FlexMemories)RigFields.Memories : null; }
        }

        /// <summary>
        /// Tone frequencies
        /// </summary>
        public float[] ToneFrequencyTable;

        // Valid tone/CTSS frequencies
        private static float[] myToneFrequencyTable =
        {
            67.0F, 69.3F, 71.9F, 74.4F, 77.0F, 79.7F, 82.5F, 85.4F, 88.5F, 91.5F,
            94.8F, 97.4F, 100.0F, 103.5F, 107.2F, 110.9F, 114.8F, 118.8F, 123.0F,
            127.3F, 131.8F, 136.5F, 141.3F, 146.2F, 151.4F, 156.7F, 162.2F, 167.9F,
            173.8F, 179.9F, 186.2F, 192.8F, 203.5F, 206.5F, 210.7F, 218.1F, 225.7F,
            229.1F, 233.6F, 241.8F, 250.3F, 254.1F, 1750F
        };
    }
}
