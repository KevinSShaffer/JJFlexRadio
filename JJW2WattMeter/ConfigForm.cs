using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JJW2WattMeter
{
    public partial class ConfigForm : Form
    {
        private Config configInfo;
        private const string mustSelect = "You must select a com port.";
        private const string mustSelectDisposition = "You must select a disposition.";
        private const string mustSelectPowerType = "You must select a power type.";
        private const string error = "Error";

        /// <summary>
        /// Configure the watt meter.
        /// </summary>
        /// <param name="info">Config data object</param>
        public ConfigForm(Config info)
        {
            InitializeComponent();

            configInfo = info;
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;
            PortList.Items.AddRange(SerialPort.GetPortNames());
            PortList.SelectedIndex = (string.IsNullOrEmpty(configInfo.Info.Port)) ? -1 :
                PortList.Items.IndexOf(configInfo.Info.Port);

            foreach (string txt in Enum.GetNames(typeof(Config.Disposition_t)))
            {
                UsageList.Items.Add(txt);
            }
            UsageList.SelectedIndex = (int)configInfo.Info.Disposition;

            foreach (string txt in Enum.GetNames(typeof(Config.power_t)))
            {
                PowerTypeListBox.Items.Add(txt);
            }
            PowerTypeListBox.SelectedIndex = (int)configInfo.Info.PowerType;
        }

        private void ConfigureButton_Click(object sender, EventArgs e)
        {
            if (UsageList.SelectedIndex == -1)
            {
                MessageBox.Show(mustSelectDisposition, error, MessageBoxButtons.OK);
                UsageList.Focus();
                return;
            }
            configInfo.Info.Disposition = (Config.Disposition_t)UsageList.SelectedIndex;

            // no port is ok if not using it.
            if (PortList.SelectedIndex == -1)
            {
                if (configInfo.Info.Disposition != Config.Disposition_t.dontUse)
                {
                    MessageBox.Show(mustSelect, error, MessageBoxButtons.OK);
                    PortList.Focus();
                    return;
                }
                configInfo.Info.Port = null;
            }
            else
            {
                configInfo.Info.Port = (string)PortList.Items[PortList.SelectedIndex];
            }
            if (PowerTypeListBox.SelectedIndex == -1)
            {
                MessageBox.Show(mustSelectPowerType, error, MessageBoxButtons.OK);
                PowerTypeListBox.Focus();
                return;
            }
            configInfo.Info.PowerType = (Config.power_t)PowerTypeListBox.SelectedIndex;

            DialogResult = DialogResult.OK;
        }

        private void CnclButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
