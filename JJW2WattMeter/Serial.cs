using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using JJTrace;

namespace JJW2WattMeter
{
    internal class Serial
    {
        private SerialPort w2Port;
        internal bool IsOpen
        {
            get
            {
                return ((w2Port != null) && w2Port.IsOpen);
            }
        }

        internal Serial()
        {
            Tracing.TraceLine("Serial", TraceLevel.Info);
        }

        /// <summary>
        /// Open and setup the port.
        /// </summary>
        /// <param name="info">configuration info</param>
        internal bool Open(Config.W2Config info)
        {
            string port = info.Port;
            if (string.IsNullOrEmpty(port))
            {
                Tracing.TraceLine("Serial.Open:no port", TraceLevel.Info);
                return false;
            }
            Tracing.TraceLine("Serial.Open:" + port, TraceLevel.Info);
            bool rv = true;
            w2Port = new SerialPort();
            try
            {
                w2Port.PortName = port;
                w2Port.BaudRate = 9600;
                w2Port.Parity = Parity.None;
                w2Port.Handshake = Handshake.None;
                w2Port.DataBits = 8;
                w2Port.StopBits = StopBits.One;
                w2Port.Encoding = System.Text.Encoding.UTF8;
                w2Port.ReadTimeout = 250;
                w2Port.WriteTimeout = 250;
                w2Port.Open();
            }
            catch(Exception ex)
            {
                Tracing.TraceLine("Serial:Open exception:" + ex.Message, TraceLevel.Error);
                if (w2Port != null) w2Port.Dispose();
                rv = false;
            }
            return rv;
        }

        /// <summary>
        /// Close the port.
        /// </summary>
        internal void Close()
        {
            Tracing.TraceLine("Serial.close",TraceLevel.Info);
            if (w2Port != null) w2Port.Dispose();
        }

        private bool timeoutError; // Don't keep tracing these
        public string Read(string command)
        {
            if (!IsOpen) return null;
            Tracing.TraceLine("serial:Read:" + command, TraceLevel.Verbose);
            StringBuilder rv = new StringBuilder();
            try
            {
                w2Port.Write(command);
                while (true)
                {
                    char c = (char)w2Port.ReadChar();
                    if (c == ';') break;
                    rv.Append(c);
                }
                // success
                if (timeoutError)
                {
                    Tracing.TraceLine("serial:timeout cleared", TraceLevel.Error);
                    timeoutError = false;
                }
            }
            catch (TimeoutException)
            {
                if (!timeoutError)
                {
                    Tracing.TraceLine("serial:Read(" + command + ") timeout", TraceLevel.Error);
                    timeoutError = true;
                }
                rv.Clear();
            }
            catch (Exception ex)
            {
                Tracing.TraceLine("serial:Read(" + command + ") = " + rv.ToString() + " exception:" + ex.Message, TraceLevel.Error);
                timeoutError = false;
                rv.Clear();
            }
            return rv.ToString();
        }
    }
}
