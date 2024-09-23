using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Flex.Smoothlake.FlexLib;
using JJTrace;
using RadioBoxes;

namespace Radios
{
    public partial class FlexTNF : Form
    {
        private bool wasActive;
        private Flex rig;
        private Panadapter panadapter { get { return rig.Panadapter; } }
        /// <summary>
        /// Selected TNF
        /// </summary>
        /// <remarks>Set in TNFList_SelectedIndexChanged().</remarks>
        private TNF theTNF;

        private int tnfWidth
        {
            get
            {
                if (theTNF == null) return 0;
                return (int)(theTNF.Bandwidth * 1e6);
            }
            set
            {
                if (theTNF != null)
                {
                    rig.q.Enqueue((Flex.FunctionDel)(() => { theTNF.Bandwidth = (double)value * 1e-6; }));
                }
            }
        }
        private int tnfDepth
        {
            get
            {
                if (theTNF == null) return 0;
                return (int)theTNF.Depth;
            }
            set
            {
                if (theTNF != null)
                {
                    rig.q.Enqueue((Flex.FunctionDel)(() => { theTNF.Depth = (uint)value; }));
                }
            }
        }
        private bool tnfPermanent
        {
            get
            {
                if (theTNF == null) return false;
                return theTNF.Permanent;
            }
            set
            {
                if (theTNF != null)
                {
                    rig.q.Enqueue((Flex.FunctionDel)(() => { theTNF.Permanent = value; }));
                }
            }
        }

        private ArrayList permanentList;

        public FlexTNF(Flex r)
        {
            InitializeComponent();

            Tracing.TraceLine("FlexTNF", TraceLevel.Info);
            rig = r;

            WidthBox.Increment = 50;
            WidthBox.LowValue = WidthBox.Increment;
            WidthBox.HighValue = 5000;
            WidthBox.UpdateDisplayFunction = () =>
                { return tnfWidth; };
            WidthBox.UpdateRigFunction =
                (int v) => { tnfWidth = v; };

            DepthBox.Increment = 1;
            DepthBox.LowValue = 1;
            DepthBox.HighValue = 3;
            DepthBox.UpdateDisplayFunction = () =>
                { return tnfDepth; };
            DepthBox.UpdateRigFunction =
                (int v) => { tnfDepth = v; };

            // permanent
            permanentList = new ArrayList();
            permanentList.Add(new TS590Filters.trueFalseElement(false));
            permanentList.Add(new TS590Filters.trueFalseElement(true));
            PermanentBox.TheList = permanentList;
            PermanentBox.UpdateDisplayFunction = () =>
                { return tnfPermanent; };
            PermanentBox.UpdateRigFunction =
                (object v) => { tnfPermanent = (bool)v; };

            setupConfigTNFs();
        }

        private void FlexTNF_Load(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;
            wasActive = false;
            Tracing.TraceLine("FlexTNF load:TNFs=" + rig.TNFs.Count.ToString(), TraceLevel.Info);

            if (TNFList.Items.Count == 0)
            {
                // setup the list
                foreach (TNF tnf in rig.TNFs)
                {
                    TNFList.Items.Add(rig.Callouts.FormatFreq(rig.LibFreqtoLong(tnf.Frequency)));
                }
            }
            if (TNFList.Items.Count > 0) TNFList.SelectedIndex = 0;
            else TNFList.SelectedIndex = -1;
        }

        private void FlexTNF_Activated(object sender, EventArgs e)
        {
            if (!wasActive)
            {
                wasActive = true;
                TNFList.Focus();
            }
        }

        private void TNFList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TNFList.SelectedIndex == -1) theTNF = null;
            else theTNF = rig.TNFs[TNFList.SelectedIndex];
            Tracing.TraceLine("TNFList_SelectedIndexChanged:" + TNFList.SelectedIndex.ToString() + ' ' +
                (string)((theTNF == null) ? "" : theTNF.ToString()), TraceLevel.Info);
            updateTNF();
        }
        private void updateTNF()
        {
            // theTNF might be null.
            WidthBox.UpdateDisplay(true);
            DepthBox.UpdateDisplay(true);
            PermanentBox.UpdateDisplay(true);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            Tracing.TraceLine("AddButton_Click", TraceLevel.Info);
            DialogResult = DialogResult.None;
            // Create a TNF at the current frequency.
            int ct = rig.TNFs.Count;
            rig.q.Enqueue((Flex.FunctionDel)(() => { rig.theRadio.RequestTNF(rig.VFOToSlice(rig.RXVFO).Freq, panadapter.StreamID); }));
            if (!AllRadios.await(() => { return (rig.TNFs.Count > ct); }, 1000))
            {
                Tracing.TraceLine("AddButton:TNF not received", TraceLevel.Error);
                return;
            }
            theTNF = rig.TNFs[rig.TNFs.Count - 1];
            TNFList.Items.Add(rig.Callouts.FormatFreq(rig.LibFreqtoLong(theTNF.Frequency)));
            TNFList.SelectedIndex = TNFList.Items.Count - 1; // updates the info.
            //tnfWidth = WidthBox.LowValue;
            //tnfDepth = DepthBox.LowValue;
            TNFList.Focus();
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            Tracing.TraceLine("RemoveButton_Click", TraceLevel.Info);
            if (theTNF == null) return;
            rig.q.Enqueue((Flex.FunctionDel)(() => { theTNF.Close(); }));
            if (!AllRadios.await(() => { return !rig.TNFs.Contains(theTNF); }, 500))
            {
                Tracing.TraceLine("RemoveButton_Click:didn't remove", TraceLevel.Error);
                return;
            }
            updateConfiguredTNFs(theTNF);
            TNFList.Items.RemoveAt(TNFList.SelectedIndex);
            TNFList.SelectedIndex = -1;
            TNFList.Focus();
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        #region ConfiguredTNFs
        public class TNFConfig
        {
            public double Frequency;
            public uint Depth;
            public bool Permanent;
            public double Bandwidth;
            public TNFConfig() { }
            public TNFConfig(TNF t)
            {
                Frequency = t.Frequency;
                Depth = t.Depth;
                Bandwidth = t.Bandwidth;
                Permanent = t.Permanent;
            }
        }
        public class TNFsConfigured
        {
            public List<TNFConfig> list;
            public TNFsConfigured()
            {
                list = new List<TNFConfig>();
            }
        }
        // Note that this is recreated by updateTNFs.
        public TNFsConfigured ConfiguredTNFs;

        private string configFileName { get { return rig.ConfigDirectory + '\\' + rig.OperatorName + '\\' + "tnfinfo.xml"; } }

        private void setupConfigTNFs()
        {
            Tracing.TraceLine("FlexTNF.setupConfigTNFs", TraceLevel.Info);
            // If nothing on file, finished.
            if (File.Exists(configFileName))
            {
                // Remove all the rig's permanent TNFs.
                List<TNF> rmv = new List<TNF>();
                foreach (TNF t in rig.TNFs)
                {
                    if (t.Permanent) rmv.Add(t);
                }
                if (rmv.Count > 0) Tracing.TraceLine("FlexTNF.setupConfigTNFs:removing old permanent TNFs", TraceLevel.Info);
                foreach (TNF t in rmv)
                {
                    t.Close();
                }

                // Load configured TNFs
                Stream cfgStream = null;
                try
                {
                    cfgStream = File.Open(configFileName, FileMode.Open);
                    XmlSerializer xs = new XmlSerializer(typeof(TNFsConfigured));
                    ConfiguredTNFs = (TNFsConfigured)xs.Deserialize(cfgStream);
                }
                catch (Exception ex)
                {
                    Tracing.TraceLine("FlexTNF.setupConfigTNFs exception:" + ex.Message, TraceLevel.Error);
                }
                finally
                {
                    if (cfgStream != null) cfgStream.Dispose();
                }

                // for each configed TNF, if not in current list, add it.
                Slice s = rig.theRadio.ActiveSlice;
                foreach (TNFConfig t in ConfiguredTNFs.list)
                {
                    Tracing.TraceLine("FlexTNF.setupConfigTNFs:adding TNF at " + t.Frequency.ToString(), TraceLevel.Info);
                    int ct = rig.TNFs.Count;
                    rig.theRadio.RequestTNF(t.Frequency, s.Panadapter.StreamID);
                    if (!AllRadios.await(() => { return (rig.TNFs.Count > ct); }, 1000))
                    {
                        Tracing.TraceLine("FlexTNF.setupConfigTNFs:TNF not received", TraceLevel.Error);
                        continue;
                    }
                    TNF t1 = rig.TNFs[ct];
                    t1.Depth = t.Depth;
                    t1.Bandwidth = t.Bandwidth;
                    t1.Permanent = t.Permanent;
                }
            }

            // process TNF updates here from now on.
            rig.UpdateConfiguredTNFs = updateConfiguredTNFs;
        }

        // This is called whenever a TNF is added or removed.
        // However, for a removal, it is called only when the TNF is removed by the operator, (i.e.) from the remove button.
        private void updateConfiguredTNFs(TNF tnf)
        {
            // Only process permanent TNFs.
            if (!tnf.Permanent) return;

            Tracing.TraceLine("FlexTNF.updateConfiguredTNFs", TraceLevel.Info);
            ConfiguredTNFs = new TNFsConfigured();
            foreach (TNF t in rig.TNFs)
            {
                ConfiguredTNFs.list.Add(new TNFConfig(t));
            }

            Stream cfgStream = null;
            try
            {
                cfgStream = File.Open(configFileName, FileMode.Create);
                XmlSerializer xs = new XmlSerializer(typeof(TNFsConfigured));
                xs.Serialize(cfgStream, ConfiguredTNFs);
            }
            catch (Exception ex)
            {
                Tracing.TraceLine("FlexTNF.updateConfiguredTNFs exception:" + ex.Message, TraceLevel.Error);
            }
            finally
            {
                if (cfgStream != null) cfgStream.Dispose();
            }
        }
        #endregion
    }
}