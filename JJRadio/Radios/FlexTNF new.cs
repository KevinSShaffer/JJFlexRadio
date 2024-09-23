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

        public FlexTNF(Flex r, string dir)
        {
            InitializeComponent();

            Tracing.TraceLine("FlexTNF", TraceLevel.Info);
            rig = r;

            // Configure TNF repository.
            setupRepository(dir);

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
            PermanentBox.TheList=permanentList;
            PermanentBox.UpdateDisplayFunction = () =>
                { return tnfPermanent; };
            PermanentBox.UpdateRigFunction =
                (object v) => { tnfPermanent = (bool)v; };
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
            TNFList.Items.RemoveAt(TNFList.SelectedIndex);
            TNFList.SelectedIndex = -1;
            TNFList.Focus();
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        // Region - TNF repository
        #region repository
        public class SavedTNF
        {
            public double Frequency;
            public uint Depth;
            public bool Permanent;
            public double Bandwidth;
            [XmlIgnore]
            public uint ID;
            public SavedTNF() { }
            public SavedTNF(TNF tnf)
            {
                Frequency = tnf.Frequency;
                Depth = tnf.Depth;
                Bandwidth = tnf.Bandwidth;
                ID = tnf.ID;
            }
        }
        public class SavedTNFs
        {
            public List<SavedTNF> TNFs;
            public SavedTNFs()
            {
                TNFs = new List<SavedTNF>();
            }
        }
        private SavedTNFs savedTNFs;

        private const string repositoryName = "TNFRepository.xml";
        private string repositoryPath;
        private void setupRepository(string dir)
        {
            repositoryPath = dir + '\\' + repositoryName;
            if (File.Exists(repositoryPath))
            {
                // TNFs may have been saved.
                Stream repos = null;
                XmlSerializer xs = null;
                try
                {
                    repos = File.Open(repositoryPath, FileMode.Open);
                    xs = new XmlSerializer(SavedTNFs);
                    savedTNFs = xs.Deserialize(repos);
                }
                catch(Exception ex)
                {
                    Tracing.TraceLine("setupRepository exception:" + ex.Message, TraceLevel.Error);
                }
                finally
                {
                    if (repos != null) repos.Dispose();
                }
                // Remove existing permanent TNFs.
                foreach(TNF tnf in rig.TNFs)
                {
                    if (tnf.Permanent)
                    {
                        int ct = rig.TNFs.Count;
                        rig.q.Enqueue((Flex.FunctionDel)(() => { tnf.Close(); });
                        if (!AllRadios.await(() => { return (rig.TNFs.Count == ct); }, 1000))
                        {
                            Tracing.TraceLine("setupRepository:TNF not removed", TraceLevel.Error);
                        }
                    }
                }
                // Create permanent TNFs.
                foreach (SavedTNF tnf in savedTNFs.TNFs)
                {
                    int ct = rig.TNFs.Count;
                    rig.q.Enqueue((Flex.FunctionDel)(() => { rig.theRadio.RequestTNF(tnf.Frequency, panadapter.StreamID); }));
                    if (!AllRadios.await(() => { return (rig.TNFs.Count > ct); }, 1000))
                    {
                        Tracing.TraceLine("setupRepository:TNF not created", TraceLevel.Error);
                        continue;
                    }
                    TNF newTNF = rig.TNFs[ct];
                    rig.q.Enqueue((Flex.FunctionDel)(() => { newTNF.Depth = tnf.Depth; });
                    rig.q.Enqueue((Flex.FunctionDel)(() => { newTNF.Bandwidth = tnf.Bandwidth; });
                    rig.q.Enqueue((Flex.FunctionDel)(() => { newTNF.Permanent = tnf.Permanent; });
                    tnf.ID = newTNF.ID; // reset the ID.
                }
            }
            else
            {
                // No TNFs configured for this operator.
                // Use any existing ones.  Only save permanent ones.
                savedTNFs = new SavedTNFs();
                foreach(TNF tnf in rig.TNFs)
                {
                    if (tnf.Permanent) savedTNFs.TNFs.Add(new SavedTNF(tnf));
                }
                writeRepository();
            }
        }

        private void writeRepository()
        {
            Tracing.TraceLine("writeRepository", TraceLevel.Info);
            try
            {
                Stream repos = File.Open(repositoryPath, FileMode.Create);
                XmlSerializer xs = new XmlSerializer(SavedTNFs);
                xs.Serialize(repos, savedTNFs);
            }
            catch(Exception ex)
            {
                Tracing.TraceLine("writeRepository exception:" + ex.Message, TraceLevel.Error);
            }
        }
        #endregion
    }
}
