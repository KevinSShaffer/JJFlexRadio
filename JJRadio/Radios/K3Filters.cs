using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JJTrace;
using RadioBoxes;

namespace Radios
{
    internal partial class K3Filters : UserControl
    {
        private ElecraftK3 rig;

        private Collection<Combo> combos;

        internal class offOnElement
        {
            private Radios.AllRadios.OffOnValues val;
            public string Display { get { return val.ToString(); } }
            public Radios.AllRadios.OffOnValues RigItem { get { return val; } }
            public offOnElement(Radios.AllRadios.OffOnValues v)
            {
                val = v;
            }
        }
        private ArrayList cwVoxList, cwQSKList;
        private ArrayList rfAttList, preAmpList;

        private ArrayList TXAntList;

        internal class trueFalseElement
        {
            private bool val;
            public string Display { get { return val.ToString(); } }
            public bool RigItem { get { return val; } }
            public trueFalseElement(bool v)
            {
                val = v;
            }
        }
        private ArrayList RXAntList, linkedList, diversityList, PLToneList;

        public enum subRXAnts
        {
            aux,
            main
        }
        internal class subRXAntElement
        {
            private subRXAnts val;
            public string Display { get { return val.ToString(); } }
            public subRXAnts RigItem { get { return val; } }
            public subRXAntElement(subRXAnts v)
            {
                val = v;
            }
        }
        private ArrayList subRXAntList;

        internal class offsetElement
        {
            private Elecraft.elecraftOffsetDirections val;
            public string Display { get { return val.ToString(); } }
            public Elecraft.elecraftOffsetDirections RigItem { get { return val; } }
            public offsetElement(Elecraft.elecraftOffsetDirections v)
            {
                val = v;
            }
        }
        private ArrayList offsetList;

        private ArrayList mainSQList, subSQList;
        private ArrayList NBList, NRList, NBSubList, NRSubList;
        private ArrayList notchList, manNotchList;

        // Used to sort Controls in order of their .Text.
        private class mySortClass : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                Control c1 = (Control)x;
                Control c2 = (Control)y;
                return string.Compare((string)c1.Tag, (string)c2.Tag);
            }
        }

        internal K3Filters(ElecraftK3 r)
        {
            InitializeComponent();

            rig = r;
            Tracing.TraceLine("K3Filters setup", TraceLevel.Info);

            // Setup the boxes
            combos = new Collection<Combo>();

            // CW Vox/QSK.
            cwVoxList = new ArrayList();
            cwVoxList.Add(new offOnElement(Radios.AllRadios.OffOnValues.off));
            cwVoxList.Add(new offOnElement(Radios.AllRadios.OffOnValues.on));
            CWVoxControl.TheList = cwVoxList; ;
            CWVoxControl.UpdateDisplayFunction =
                () => { return rig.Vox; };
            CWVoxControl.UpdateRigFunction =
                (object v) => { rig.Vox = (Radios.AllRadios.OffOnValues)v; };
            combos.Add(CWVoxControl);
            cwQSKList = new ArrayList();
            cwQSKList.Add(new offOnElement(Radios.AllRadios.OffOnValues.off));
            cwQSKList.Add(new offOnElement(Radios.AllRadios.OffOnValues.on));
            CWQSKControl.TheList = cwQSKList;
            CWQSKControl.UpdateDisplayFunction =
                () => { return rig.QSK; };
            CWQSKControl.UpdateRigFunction =
                (object v) => { rig.QSK = (Radios.AllRadios.OffOnValues)v; };
            combos.Add(CWQSKControl);

            // Antenna
            TXAntList = new ArrayList();
            TXAntList.Add(new trueFalseElement(false));
            TXAntList.Add(new trueFalseElement(true));
            TXAntControl.TheList = TXAntList;
            TXAntControl.UpdateDisplayFunction =
                () => { return rig.b_ANT2; };
            TXAntControl.UpdateRigFunction =
                (object v) => { rig.TXAntenna = ((bool)v) ? 1 : 0; };
            combos.Add(TXAntControl);

            // RX Antenna
            RXAntList = new ArrayList();
            RXAntList.Add(new trueFalseElement(false));
            RXAntList.Add(new trueFalseElement(true));
            RXAntControl.TheList = RXAntList;
            RXAntControl.UpdateDisplayFunction =
                () => { return rig.RXAntenna; };
            RXAntControl.UpdateRigFunction =
                (object v) => { rig.RXAntenna = (bool)v; };
            combos.Add(RXAntControl);

            // RF Attenuator
            rfAttList = new ArrayList();
            rfAttList.Add(new offOnElement(Radios.AllRadios.OffOnValues.off));
            rfAttList.Add(new offOnElement(Radios.AllRadios.OffOnValues.on));
            RFAttControl.TheList = rfAttList;
            RFAttControl.UpdateDisplayFunction =
                () => { return rig.RFAttenuator; };
            RFAttControl.UpdateRigFunction =
                (object v) => { rig.RFAttenuator = (Radios.AllRadios.OffOnValues)v; };
            combos.Add(RFAttControl);

            // PreAmp
            preAmpList = new ArrayList();
            preAmpList.Add(new offOnElement(Radios.AllRadios.OffOnValues.off));
            preAmpList.Add(new offOnElement(Radios.AllRadios.OffOnValues.on));
            PreAmpControl.TheList = preAmpList;
            PreAmpControl.UpdateDisplayFunction =
                () => { return rig.PreAmp; };
            PreAmpControl.UpdateRigFunction =
                (object v) => { rig.PreAmp = (Radios.AllRadios.OffOnValues)v; };
            combos.Add(PreAmpControl);

            linkedList = new ArrayList();
            linkedList.Add(new trueFalseElement(false));
            linkedList.Add(new trueFalseElement(true));
            LinkedControl.TheList = linkedList;
            LinkedControl.UpdateDisplayFunction =
                () => { return rig.VFOsLinked; };
            LinkedControl.UpdateRigFunction =
                (object v) => { rig.VFOsLinked = (bool)v; };
            combos.Add(LinkedControl);

            diversityList = new ArrayList();
            diversityList.Add(new trueFalseElement(false));
            diversityList.Add(new trueFalseElement(true));
            DiversityControl.TheList = diversityList;
            DiversityControl.UpdateDisplayFunction =
                () => { return rig.diversity; };
            DiversityControl.UpdateRigFunction =
                (object v) => { rig.diversity = (bool)v; };
            combos.Add(DiversityControl);

            subRXAntList = new ArrayList();
            subRXAntList.Add(new subRXAntElement(subRXAnts.aux));
            subRXAntList.Add(new subRXAntElement(subRXAnts.main));
            SubAntMainControl.TheList = subRXAntList;
            SubAntMainControl.UpdateDisplayFunction =
                () => { return (rig.subAntMain) ? subRXAnts.main : subRXAnts.aux; };
            SubAntMainControl.UpdateRigFunction =
                (object v) => { rig.subAntMain = ((subRXAnts)v == subRXAnts.main)? true: false; };
            combos.Add(SubAntMainControl);

            PLToneList = new ArrayList();
            PLToneList.Add(new trueFalseElement(false));
            PLToneList.Add(new trueFalseElement(true));
            PLToneControl.TheList = PLToneList;
            PLToneControl.UpdateDisplayFunction =
                () => { return rig.b_PLTone; };
            PLToneControl.UpdateRigFunction = null;
            combos.Add(PLToneControl);

            offsetList = new ArrayList();
            offsetList.Add(new offsetElement(Elecraft.elecraftOffsetDirections.off));
            offsetList.Add(new offsetElement(Elecraft.elecraftOffsetDirections.minus));
            offsetList.Add(new offsetElement(Elecraft.elecraftOffsetDirections.plus));
            OffsetControl.TheList = offsetList;
            OffsetControl.UpdateDisplayFunction =
                () => { return rig.fmOffset; };
            OffsetControl.UpdateRigFunction =
                (object v) => { rig.fmOffset= (Elecraft.elecraftOffsetDirections)v; };
            combos.Add(OffsetControl);

            mainSQList = new ArrayList();
            mainSQList.Add(new trueFalseElement(false));
            mainSQList.Add(new trueFalseElement(true));
            MainSQControl.TheList = mainSQList;
            MainSQControl.UpdateDisplayFunction =
                () => { return rig.b_MainSQ; };
            MainSQControl.UpdateRigFunction = null;
            combos.Add(MainSQControl);

            subSQList = new ArrayList();
            subSQList.Add(new trueFalseElement(false));
            subSQList.Add(new trueFalseElement(true));
            SubSQControl.TheList=subSQList;
            SubSQControl.UpdateDisplayFunction =
                () => { return rig.b_SubSQ; };
            SubSQControl.UpdateRigFunction = null;
            combos.Add(SubSQControl);
            
            // noise blanker and reduction
            NBList = new ArrayList();
            NBList.Add(new offOnElement(Radios.AllRadios.OffOnValues.off));
            NBList.Add(new offOnElement(Radios.AllRadios.OffOnValues.on));
            NBControl.TheList = NBList;
            NBControl.UpdateDisplayFunction =
                () => { return rig.NoiseBlanker; };
            NBControl.UpdateRigFunction =
                (object v) => { rig.NoiseBlanker = (Radios.AllRadios.OffOnValues)v; };
            combos.Add(NBControl);

            NBSubList = new ArrayList();
            NBSubList.Add(new offOnElement(Radios.AllRadios.OffOnValues.off));
            NBSubList.Add(new offOnElement(Radios.AllRadios.OffOnValues.on));
            NBSubControl.TheList = NBSubList;
            NBSubControl.UpdateDisplayFunction =
                () => { return rig.NoiseBlankerSub; };
            NBSubControl.UpdateRigFunction =
                (object v) => { rig.NoiseBlankerSub = (Radios.AllRadios.OffOnValues)v; };
            combos.Add(NBSubControl);

            NRList = new ArrayList();
            NRList.Add(new offOnElement(Radios.AllRadios.OffOnValues.off));
            NRList.Add(new offOnElement(Radios.AllRadios.OffOnValues.on));
            NRControl.TheList = NRList;
            NRControl.UpdateDisplayFunction =
                () => { return rig.NoiseReduction; };
            NRControl.UpdateRigFunction =
                (object v) => { rig.NoiseReduction = (Radios.AllRadios.OffOnValues)v; };
            combos.Add(NRControl);

            NRSubList = new ArrayList();
            NRSubList.Add(new offOnElement(Radios.AllRadios.OffOnValues.off));
            NRSubList.Add(new offOnElement(Radios.AllRadios.OffOnValues.on));
            NRSubControl.TheList = NRSubList;
            NRSubControl.UpdateDisplayFunction =
                () => { return rig.NoiseReductionSub; };
            NRSubControl.UpdateRigFunction =
                (object v) => { rig.NoiseReductionSub = (Radios.AllRadios.OffOnValues)v; };
            combos.Add(NRSubControl);

            notchList = new ArrayList();
            notchList.Add(new offOnElement(Radios.AllRadios.OffOnValues.off));
            notchList.Add(new offOnElement(Radios.AllRadios.OffOnValues.on));
            NotchControl.TheList = notchList;
            NotchControl.UpdateDisplayFunction =
                () => { return rig.AutoNotch; };
            NotchControl.UpdateRigFunction =
                (object v) => { rig.AutoNotch = (Radios.AllRadios.OffOnValues)v; };
            combos.Add(NotchControl);

            manNotchList = new ArrayList();
            manNotchList.Add(new offOnElement(Radios.AllRadios.OffOnValues.off));
            manNotchList.Add(new offOnElement(Radios.AllRadios.OffOnValues.on));
            ManNotchControl.TheList = manNotchList;
            ManNotchControl.UpdateDisplayFunction =
                () => { return rig.ManualNotch; };
            ManNotchControl.UpdateRigFunction =
                (object v) => { rig.ManualNotch = (Radios.AllRadios.OffOnValues)v; };
            combos.Add(ManNotchControl);

            Control[] myControls = new Control[combos.Count + 2];
            for (int i = 0; i < combos.Count; i++)
            {
                myControls[i] = (Control)combos[i];
                myControls[i].Tag = combos[i].Header;
            }
            myControls[combos.Count] = (Control)MainDisplay;
            myControls[combos.Count].Tag = "VFOA";
            myControls[combos.Count + 1] = (Control)SecondaryDisplay;
            myControls[combos.Count + 1].Tag = "VFOB";
            IComparer mySort = new mySortClass();
            Array.Sort(myControls, mySort);

            //modeChange = new modeChangeClass(this);

            // setup the memory display.
            //Form memDisp = new TS590Memories(rig);
            Form mnu = new K3Menus(rig);
            // setup RigFields
            rig.RigFields = new AllRadios.RigInfo(this, updateBoxes, null, mnu, myControls);
        }

        private string oldA = "";
        private string oldB = "";
        private void updateBoxes()
        {
            if (rig.Mode == null) return;
            Tracing.TraceLine("updateBoxes", TraceLevel.Verbose);

            // enable/disable
            if (rig.Mode == Elecraft.myModeTable[(int)Elecraft.modes.fm])
            {
                enable(true, PLToneControl);
                enable(true, OffsetControl);
            }
            else
            {
                enable(false, PLToneControl);
                enable(false, OffsetControl);
            }

            // the subRX nb and nr are only modified if the subRX is on.
            // In addition, BSet must be on to modify the nr value.
            if (rig.b_SubRX)
            {
                NBControl.ReadOnly = false;
                NRControl.ReadOnly = false;
                NBSubControl.ReadOnly = false;
                if (rig.b_BSet) NRSubControl.ReadOnly = false;
                else NRSubControl.ReadOnly = true;
            }
            else
            {
                NBSubControl.ReadOnly = true;
                NRSubControl.ReadOnly = true;
                // BSet must be off to change these.
                if (rig.b_BSet)
                {
                    NBControl.ReadOnly = true;
                    NRControl.ReadOnly = true;
                }
                else
                {
                    NBControl.ReadOnly = false;
                    NRControl.ReadOnly = false;
                }
            }

            foreach (Combo c in combos)
            {
                if (c.Enabled)
                {
                    c.UpdateDisplay();
                }
            }

            // write the displays.
            writeADisplay(MainDisplay, rig.VFOADisplay, ref oldA);
            writeADisplay(SecondaryDisplay, rig.VFOBDisplay, ref oldB);
        }

        private static void writeADisplay(TextBox box, string value, ref string oldValue)
        {
            if (value == oldValue) return;
            if (box.InvokeRequired)
            {
                box.Invoke(writeTextBox, new object[] { value, box });
            }
            else box.Text = value;
            oldValue = value;
        }
        private delegate void wrtTBDel(string str, TextBox box);
        private static wrtTBDel writeTextBox = writeTBox;
        private static void writeTBox(string str, TextBox box)
        {
            box.Text = str;
        }

        private static void enable(bool val, Combo c)
        {
            if (c.InvokeRequired) c.Invoke(enab, new object[] { val, c });
            else enab(val, c);
        }
        private delegate void enabDel(bool val, Combo c);
        private static enabDel enab = enabCB;
        private static void enabCB(bool val, Combo c)
        {
            if (c.Enabled != val)
            {
                c.Enabled = val;
                c.Visible = val;
            }
        }

#if zero
        private class modeChangeClass
        {
            // A mode's filter controls are enabled when that mode is active.
            // First, the specified controls for the other modes are disabled,
            // unless they're just going to be enabled again.
            private class controlsClass
            {
                public Control[] controls;
                public controlsClass(Control[] controlArray)
                {
                    int controlDim = controlArray.Length;
                    controls = new Control[controlDim];
                    for (int i = 0; i < controlDim; i++)
                    {
                        controls[i] = controlArray[i];
                    }
                }
            }
            private controlsClass[] modeControls;
            private K3Filters parent;
            public modeChangeClass(K3Filters p)
            {
                parent = p;
                modeControls = new controlsClass[Elecraft.myModeTable.Length];

                // setup the mode to combobox mapping.
            }
            private delegate void rtn(Control c);
            private rtn enab = (Control c) =>
            {
                if (!c.Enabled)
                {
                    c.Enabled = true;
                    c.Visible = true;
                    c.BringToFront();
                }
            };
            private rtn disab = (Control c) =>
            {
                if (c.Enabled)
                {
                    c.Enabled = false;
                    c.Visible = false;
                    c.SendToBack();
                }
            };
            private AllRadios.ModeValue oldMode = Kenwood.myModeTable[0];
            public void enableDisable(Kenwood.ModeValue mode)
            {
                // Just quit if the mode hasn't changed.
                if (mode == oldMode) return;
                oldMode = mode;
                int mod = mode.id;
                // quit if no controls for this mode.
                if (modeControls[mod] == null) return;
                // enables holds the controls to be enabled.
                Control[] enables = new Control[modeControls[mod].controls.Length];
                for (int i = 0; i < enables.Length; i++)
                {
                    // We need to quit if no more controls for this mode.
                    if (modeControls[mod].controls[i] == null) break;
                    enables[i] = modeControls[mod].controls[i];
                }
                parent.SuspendLayout();
                for (int i = 0; i < modeControls.Length; i++)
                {
                    if (modeControls[i] == null) continue;
                    for (int j = 0; j < modeControls[i].controls.Length; j++)
                    {
                        Control c = modeControls[i].controls[j];
                        if (c == null) break;
                        if (Array.IndexOf(enables, c) >= 0)
                        {
                            // enable
                            if (parent.InvokeRequired)
                            {
                                parent.Invoke(enab, new object[] { c });
                            }
                            else
                            {
                                enab(c);
                            }
                        }
                        else
                        {
                            // disable
                            if (parent.InvokeRequired)
                            {
                                parent.Invoke(disab, new object[] { c });
                            }
                            else
                            {
                                disab(c);
                            }
                        }
                    }
                }
                parent.ResumeLayout();
            }
        }
        private modeChangeClass modeChange;
#endif

        private void BoxKeydownDefault(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }
    }
}
