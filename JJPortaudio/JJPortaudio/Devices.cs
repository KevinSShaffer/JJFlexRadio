using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using JJTrace;
using PortAudioSharp;

namespace JJPortaudio
{
    public class Devices
    {
        /// <summary>
        /// Device type, input or output.
        /// </summary>
        public enum DeviceTypes
        {
            none = 0,
            input,
            output
        }

        /// <summary>
        /// Audio device abstraction.
        /// </summary>
        public class Device
        {
            public int DevinfoID; // infoList ID.
            public DeviceTypes Type;
            public string Name;
            public int hostApi;
            public int maxInputChannels;
            public int maxOutputChannels;
            public double defaultLowInputLatency;
            public double defaultLowOutputLatency;
            public double defaultHighInputLatency;
            public double defaultHighOutputLatency;
            public double defaultSampleRate;
            [XmlIgnore]
            public string ConfigFile;
        }

        // Configured devices, 1 input, 1 output.
        public class cfg
        {
            public Device[] devs = new Device[2];
        }
        private cfg configured = new cfg();

        /// <summary>
        /// Chosen input device.
        /// </summary>
        public Device InputDevice
        {
            get { return configured.devs[0]; }
            private set { configured.devs[0] = value; }
        }
        /// <summary>
        /// Chosen output device.
        /// </summary>
        public Device OutputDevice
        {
            get { return configured.devs[1]; }
            private set { configured.devs[1] = value; }
        }

        private string cfgFile;

        /// <summary>
        /// Retrieve or select the audio device.
        /// </summary>
        /// <param name="fileName">name of config file.</param>
        public Devices(string fileName)
        {
            cfgFile = fileName;
        }

        /// <summary>
        /// Setup audio devices.
        /// </summary>
        /// <returns>true on success.</returns>
        public bool Setup()
        {
            if (!devList.Setup())
            {
                return false;
            }

            if (!string.IsNullOrEmpty(cfgFile) && File.Exists(cfgFile))
            {
                return readCFG();
            }

            return true;
        }

        private bool readCFG()
        {
            bool rv = true;
            Stream stream = null;
            XmlSerializer xs = null;
            try
            {
                stream = File.Open(cfgFile, FileMode.Open, FileAccess.Read,FileShare.Read);
                xs = new XmlSerializer(typeof(cfg));
                configured = (cfg)xs.Deserialize(stream);

                // Set the config file names.
                if (InputDevice != null) InputDevice.ConfigFile = cfgFile;
                if (OutputDevice != null) OutputDevice.ConfigFile = cfgFile;
            }
            catch(Exception ex)
            {
                Tracing.ErrMessageTrace(ex, true);
                rv = false;
            }
            finally
            {
                if (stream != null) stream.Dispose();
            }
            return rv;
        }

        private void writeCFG()
        {
            Stream stream = null;
            XmlSerializer xs = null;
            try
            {
                stream = File.Open(cfgFile, FileMode.Create);
                xs = new XmlSerializer(typeof(cfg));
                xs.Serialize(stream, configured);
            }
            catch(Exception ex)
            {
                Tracing.ErrMessageTrace(ex, true);
            }
            finally
            {
                if (stream != null) stream.Dispose();
            }
        }

        /// <summary>
        /// Get the configured input or output device.
        /// </summary>
        /// <param name="type">DeviceTypes value, input or output</param>
        /// <param name="getNew">(optional) allow user to select device if none selected.</param>
        /// <returns>the selected devicde</returns>
        public Device GetConfiguredDevice(DeviceTypes type, bool getNew = false)
        {
            Device dev = (type == DeviceTypes.input) ? InputDevice : OutputDevice;
            // If no such device is selected.
            if ((dev == null) || !devList.FindDevice(dev))
            {
                if (getNew) return getNewDevice(type);
                return null;
            }
            return dev;
        }

        /// <summary>
        /// Select a new input or output device.
        /// </summary>
        /// <param name="type">DeviceTypes value, input or output.</param>
        /// <returns>selected device</returns>
        public Device getNewDevice(DeviceTypes type)
        {
            Device rv = null;
            // Show devices to the user.
            devList listDevs = new devList(type);
            if (((Form)listDevs).ShowDialog() == DialogResult.OK)
            {
                // Create device abstraction
                int id = (type == DeviceTypes.input) ? 0 : 1;
                configured.devs[id] = new Device
                {
                    Type = type,
                    DevinfoID = listDevs.SelectedDevice.DeviceID,
                    Name = listDevs.SelectedDevice.Info.name,
                    hostApi = listDevs.SelectedDevice.Info.hostApi,
                    maxInputChannels = listDevs.SelectedDevice.Info.maxInputChannels,
                    maxOutputChannels = listDevs.SelectedDevice.Info.maxOutputChannels,
                    defaultLowInputLatency = listDevs.SelectedDevice.Info.defaultLowInputLatency,
                    defaultLowOutputLatency = listDevs.SelectedDevice.Info.defaultLowOutputLatency,
                    defaultHighInputLatency = listDevs.SelectedDevice.Info.defaultHighInputLatency,
                    defaultHighOutputLatency = listDevs.SelectedDevice.Info.defaultHighOutputLatency,
                    defaultSampleRate = listDevs.SelectedDevice.Info.defaultSampleRate,
                    ConfigFile=cfgFile
                };
                // Save this choice.
                writeCFG();
                rv = configured.devs[id];
            }
            listDevs.Dispose();
            return rv;
        }
    }
}
