using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using JJTrace;

namespace JJFlexControl
{
    class Serial
    {
        private SerialPort knobPort;
        internal bool IsOpen
        {
            get
            {
                return ((knobPort != null) && knobPort.IsOpen);
            }
        }

        private FlexControl parent;
        internal Serial(FlexControl p)
        {
            Tracing.TraceLine("Serial", TraceLevel.Info);
            parent = p;
        }

        /// <summary>
        /// Open the port.
        /// </summary>
        /// <param name="port">name of port to open</param>
        /// <returns></returns>
        internal bool Open(string port)
        {
            Tracing.TraceLine("Serial.Open:" + port, TraceLevel.Info);
            bool rv = true;
            knobPort = new SerialPort();
            try
            {
                knobPort.PortName = port;
                knobPort.BaudRate = 9600;
                knobPort.Parity = Parity.None;
                knobPort.Handshake = Handshake.RequestToSend;
                knobPort.DataBits = 8;
                knobPort.StopBits = StopBits.One;
                knobPort.Encoding = System.Text.Encoding.UTF8;
                knobPort.Open();
            }
            catch(Exception ex)
            {
                Tracing.TraceLine("Serial:Open exception:" + ex.Message, TraceLevel.Error);
                if (knobPort != null) knobPort.Dispose();
                rv = false;
            }

            // Create area to hold characters.
            if (rv)
            {
                buf = new bufType(knobPort);
                knobPort.DataReceived += dataReceived;
            }

            return rv;
        }

        /// <summary>
        /// Close the port.
        /// </summary>
        internal void Close()
        {
            Tracing.TraceLine("Serial.close",TraceLevel.Info);
            if (knobPort != null) knobPort.Dispose();
        }

        private class bufType
        {
            private StringBuilder buf;
            private const char delim = ';';
            private SerialPort dev;

            public bufType(SerialPort port)
            {
                dev = port;
                buf = new StringBuilder();
            }

            public void Add()
            {
                int len = dev.BytesToRead;
                char[] cBuf = new char[len];
                dev.Read(cBuf, 0, len);
                // Remove 0x00
                for(int i = 0; i < cBuf.Length; i++)
                {
                    if (cBuf[i] != 0x0) buf.Append(cBuf[i]);
                }
            }

            /// <summary>
            /// Read a command w/o the delimiter.
            /// </summary>
            /// <returns>a string containing the first command w/o delimiter, or the empty string.</returns>
            public string Read()
            {
                StringBuilder rv = new StringBuilder();
                for(int i = 0; i < buf.Length; i++)
                {
                    if (buf[i] == delim)
                    {
                        if (i == 0) buf.Remove(0, 1);
                        else
                        {
                            rv.Append(buf.ToString(0, i));
                            buf.Remove(0, i + 1);
                            break;
                        }
                    }
                }
                return rv.ToString();
            }
        }
        private bufType buf;

        private void dataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            buf.Add();
            string data;
            while ((data = buf.Read()) != "")
            {
                parent.OnKnobOutput(data);                
            }
        }
    }
}
