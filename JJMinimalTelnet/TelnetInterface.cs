// minimalistic telnet implementation
// originally conceived by Tom Janssens on 2007/06/06  for codeproject
//
// http://www.corebvba.be
// Modified by Jim Shaffer, February 4, 2017.



using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Net.Sockets;
using JJTrace;

namespace JJMinimalTelnet
{
    enum Verbs {
        WILL = 251,
        WONT = 252,
        DO = 253,
        DONT = 254,
        IAC = 255
    }

    enum Options
    {
        SGA = 3
    }

    internal class TelnetConnection
    {
        internal const string TCNewline = "\r\n";
        TcpClient tcpSocket;

        int TimeOutMs = 100;

        /// <summary>
        /// new connection
        /// </summary>
        /// <param name="hostname">string hostname</param>
        /// <param name="port">int port, 0 means use 23.</param>
        internal TelnetConnection(string hostname, int port)
        {
            Tracing.TraceLine("TelnetConnection:" + hostname + ' ' + port.ToString(), TraceLevel.Info);
            if (port == 0) port = 23;
            tcpSocket = new TcpClient(hostname, port);
        }

        internal void Close()
        {
            Tracing.TraceLine("TelnetConnection close", TraceLevel.Info);            
            if (tcpSocket != null) tcpSocket.Close();
        }

        internal string LoginString;
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="Username">optional user name string</param>
        /// <param name="Password">optional password string</param>
        /// <param name="LoginTimeOutMs">int timeout in ms, 0 means use default.</param>
        /// <param name="loginError">login error message returned</param>
        /// <returns>true on success</returns>
        /// <remarks>
        /// A successful return doesn't mean you necessarily got logged in.
        /// </remarks>
        internal bool Login(string Username,string Password,int LoginTimeOutMs,
            out string loginError)
        {
            Tracing.TraceLine("TelnetConnection login:" + Username + ' ' + Password + ' ' + LoginTimeOutMs.ToString(), TraceLevel.Info);
            loginError = "";
            int oldTimeOutMs = TimeOutMs;
            if (LoginTimeOutMs != 0) TimeOutMs = LoginTimeOutMs;

            if (Username != null)
            {
                LoginString = Read();
                if (!LoginString.TrimEnd().EndsWith(":"))
                {
                    loginError = "No login prompt";
                    return false;
                }

                WriteLine(Username);

                if (!string.IsNullOrEmpty(Password))
                {
                    LoginString += Read();
                    if (!LoginString.TrimEnd().EndsWith(":"))
                    {
                        loginError = "No login prompt";
                        return false;
                    }
                    WriteLine(Password);
                }
            }

            TimeOutMs = oldTimeOutMs;
            return true;
        }

        internal void WriteLine(string cmd)
        {
            Tracing.TraceLine("TelnetConnection WriteLine:", TraceLevel.Info);
            Write(cmd + TCNewline);
        }

        internal void Write(string cmd)
        {
            Tracing.TraceLine("TelnetConnection Write:" + cmd, TraceLevel.Info);
            if (!tcpSocket.Connected) return;
            byte[] buf = System.Text.ASCIIEncoding.ASCII.GetBytes(cmd.Replace("\0xFF","\0xFF\0xFF"));
            tcpSocket.GetStream().Write(buf, 0, buf.Length);
        }

        internal string Read()
        {
            Tracing.TraceLine("TelnetConnection read", TraceLevel.Verbose);
            if (!tcpSocket.Connected) return null;
            StringBuilder sb=new StringBuilder();
            do
            {
                ParseTelnet(sb);
                System.Threading.Thread.Sleep(TimeOutMs);
            } while (tcpSocket.Available > 0);
            return sb.ToString();
        }

        internal bool IsConnected
        {
            get { return tcpSocket.Connected; }
        }

        private void ParseTelnet(StringBuilder sb)
        {
            while (tcpSocket.Available > 0)
            {
                int input = tcpSocket.GetStream().ReadByte();
                switch (input)
                {
                    case -1 :
                        break;
                    case (int)Verbs.IAC:
                        // interpret as command
                        int inputverb = tcpSocket.GetStream().ReadByte();
                        if (inputverb == -1) break;
                        switch (inputverb)
                        {
                            case (int)Verbs.IAC: 
                                //literal IAC = 255 escaped, so append char 255 to string
                                sb.Append(inputverb);
                                break;
                            case (int)Verbs.DO: 
                            case (int)Verbs.DONT:
                            case (int)Verbs.WILL:
                            case (int)Verbs.WONT:
                                // reply to all commands with "WONT", unless it is SGA (suppres go ahead)
                                int inputoption = tcpSocket.GetStream().ReadByte();
                                if (inputoption == -1) break;
                                tcpSocket.GetStream().WriteByte((byte)Verbs.IAC);
                                if (inputoption == (int)Options.SGA )
                                    tcpSocket.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WILL:(byte)Verbs.DO); 
                                else
                                    tcpSocket.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WONT : (byte)Verbs.DONT); 
                                tcpSocket.GetStream().WriteByte((byte)inputoption);
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        sb.Append( (char)input );
                        break;
                }
            }
        }
    }
}
