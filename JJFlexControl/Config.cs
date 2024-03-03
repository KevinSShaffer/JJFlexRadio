using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using JJTrace;

namespace JJFlexControl
{
    public class Config
    {
        public class KnobConfig
        {
            public string Port;
            public List<FlexControl.KeyAction_t> Actions;
            [XmlIgnore]
            internal bool NeedsWrite = false;
            public KnobConfig() { }
            public KnobConfig(KnobConfig config, FlexControl topLevel)
            {
                Port = config.Port;
                Actions = new List<FlexControl.KeyAction_t>();
                foreach (FlexControl.KeyAction_t ka in config.Actions)
                {
                    if (topLevel.FindAction(ka.ActionName, topLevel.ValidActions) != null)
                    {
                        Actions.Add(ka);
                    }
                }
            }
        }
        public KnobConfig Info;

        private FlexControl topLevel;
        private string fileName;

        /// <summary>
        /// Configuration data.
        /// </summary>
        /// <param name="top">FlexControl object</param>
        /// <param name="name">Configuration file path</param>
        internal Config(FlexControl top, string name)
        {
            topLevel = top;
            fileName = name;
            Info = new KnobConfig();
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
                XmlSerializer xs = new XmlSerializer(typeof(KnobConfig));
                KnobConfig myInfo = (KnobConfig)xs.Deserialize(stream);
                Info = new KnobConfig(myInfo, topLevel);
            }
            catch(Exception ex)
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
                XmlSerializer xs = new XmlSerializer(typeof(KnobConfig));
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
