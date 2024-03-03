using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JJTrace;

namespace JJW2WattMeter
{
    public class W2 : IDisposable
    {
        public Config ConfigInfo;
        private string configFile;
        private Reader reader;
        internal Serial serial;

        /// <summary>
        /// Is the meter configured?
        /// </summary>
        public bool IsConfigured { get { return ConfigInfo.IsConfigured; } }

        /// <summary>
        /// Is the meter open?
        /// </summary>
        public bool IsOpen { get { return serial.IsOpen; } }

        /// <summary>
        /// Is meter useable?
        /// </summary>
        public bool IsUseable
        {
            get { return (IsOpen & (ConfigInfo.Info.Disposition == Config.Disposition_t.useIfOpen)); }
        }

        /// <summary>
        /// Show SWR.
        /// </summary>
        /// <remarks>For use with JJRadio</remarks>
        public bool ShowSWR
        {
            get { return (IsUseable & (ConfigInfo.Info.PowerType == Config.power_t.swr)); }
        }

        /// <summary>
        /// W2 watt meter object.
        /// </summary>
        /// <param name="name">config file path</param>
        public W2(string name)
        {
            Tracing.TraceLine("W2:" + name, TraceLevel.Info);
            configFile = name;
            serial = new Serial();
            ConfigInfo = new Config(configFile);
            ConfigInfo.Read();
            //reader = new Reader(this);
        }

        /// <summary>
        /// Setup the watt meter.
        /// </summary>
        /// <param name="getConfig">(optional) true to get config info, default false.</param>
        public void Setup(bool getConfig = false)
        {
            if (getConfig)
            {
                ConfigForm cfg = new ConfigForm(ConfigInfo);
                Form theForm = (Form)cfg;
                if (theForm.ShowDialog() == DialogResult.OK)
                {
                    // handle config changes.
                    ConfigInfo.Write();
                }
                theForm.Dispose();
            }

            if (reader != null) reader.Close();
            if (serial.IsOpen) serial.Close();
            if (ConfigInfo.Info.Disposition != Config.Disposition_t.dontUse)
            {
                serial.Open(ConfigInfo.Info);
                reader = new Reader(this);
                Tracing.TraceLine("W2 Setup:" + (string)((IsOpen) ? "open" : "not open"), TraceLevel.Info);
            }
        }

        // Format is nnnnnDm.  m = # of digits after decimal point.
        private string parsePower(string resp)
        {
            string rv = "0";
            if (!string.IsNullOrEmpty(resp) && (resp.IndexOf('D') != -1))
            {
                string[] parts = resp.Substring(1).Split(new char[] { 'D' });
                double f = 0;
                double d = 0;
                if (System.Double.TryParse(parts[0], out f) & System.Double.TryParse(parts[1], out d))
                {
                    f = f / Math.Pow(10, d);
                    // Round to nearest watt.
                    rv = ((int)(f + 0.5)).ToString();
                }
            }
            return rv;
        }

        private string _ForwardPower = "0";
        public string ForwardPower
        {
            get { return parsePower(_ForwardPower); }
            internal set { _ForwardPower = value; }
        }

        private string _SWR = "0";
        public string SWR
        {
            get
            {
                string rv = "0";
                string resp = _SWR;
                if (resp.Length == 5)
                {
                    string left = resp.Substring(1, 2);
                    string right = resp.Substring(3, 2);
                    if (left[0] == '0') left = left.Substring(1, 1);
                    rv = left + '.' + right;
                }
                return rv;
            }
            internal set { _SWR = value; }
        }

        private static Dictionary<string, Config.power_t> powerTypeTable = new Dictionary<string, Config.power_t>()
        {
            { "A", Config.power_t.average },
            { "P", Config.power_t.pep },
        };
        // We can only set this.
        internal bool MeterPowerSetting(Config.power_t level)
        {
            // Posative result if not to be set here.
            if (!powerTypeTable.Values.Contains(level)) return true;

            // The loop provides the sanity check.
            for (int i = 0; i < Enum.GetNames(typeof(Config.power_t)).Length; i++)
            {
                string v0 = serial.Read("m");
                if (string.IsNullOrEmpty(v0)) return false;
                Config.power_t v1 = Config.power_t.average;
                if ((v0.Length > 1) &&
                    powerTypeTable.TryGetValue(v0.Substring(1), out v1) &&
                    (v1 == level))
                {
                    // It's set.
                    return true;
                }
            }
            return false;
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
            Tracing.TraceLine("W2 Dispose:" + disposing.ToString(), TraceLevel.Info);
            if (!disposed)
            {
                if (disposing)
                {
                    component.Dispose();
                }
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
                if (serial != null)
                {
                    serial.Close();
                    serial = null;
                }
                disposed = true;
            }
        }

        ~W2()
        {
            Dispose(false);
        }
        #endregion
    }
}
