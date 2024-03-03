using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using JJTrace;

namespace JJW2WattMeter
{
    public class Config
    {
        /// <summary>
        /// dispositions
        /// </summary>
        public enum Disposition_t
        {
            useIfOpen,
            dontUse
        }

        /// <summary>
        /// power types
        /// </summary>
        public enum power_t
        {
            average,
            pep,
            swr
        }

        public class W2Config
        {
            public string Port;
            public Disposition_t Disposition = Disposition_t.useIfOpen;
            public power_t PowerType = power_t.average;
            public W2Config() { }
        }
        public W2Config Info;
        public bool IsConfigured { get { return !string.IsNullOrEmpty(Info.Port); } }

        private string fileName;

        /// <summary>
        /// Configuration data.
        /// </summary>
        /// <param name="name">Configuration file path</param>
        internal Config(string name)
        {
            fileName = name;
            Info = new W2Config();
        }

        /// <summary>
        /// Read from config file.
        /// </summary>
        public void Read()
        {
            Tracing.TraceLine("Config.Read", TraceLevel.Info);
            Stream stream = null;
            if (!File.Exists(fileName)) return;

            try
            {
                stream = File.Open(fileName, FileMode.Open);
                XmlSerializer xs = new XmlSerializer(typeof(W2Config));
                Info = (W2Config)xs.Deserialize(stream);
            }
            catch (Exception ex)
            {
                Tracing.TraceLine("Config.Read exception:" + ex.Message, TraceLevel.Error);
            }
            finally
            {
                if (stream != null) stream.Dispose();
            }
        }

        /// <summary>
        /// Write configuration for the port.
        /// </summary>
        public void Write()
        {
            Tracing.TraceLine("Config.Write", TraceLevel.Info);
            Stream stream = null;
            try
            {
                string dir = fileName.Substring(0, fileName.LastIndexOf('\\'));
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                stream = File.Open(fileName, FileMode.Create);
                XmlSerializer xs = new XmlSerializer(typeof(W2Config));
                xs.Serialize(stream, Info);
            }
            catch (Exception ex)
            {
                Tracing.TraceLine("Config.Write exception:" + ex.Message, TraceLevel.Error);
            }
            finally
            {
                if (stream != null) stream.Dispose();
            }
        }
    }
}
