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
    public partial class TS2000Filters : UserControl
    {
        protected KenwoodTS2000 rig;
        private Collection<object> combos;
        private enum filterValues
        {
            a = 1,
            b
        }
        private class filterElement
        {
            private filterValues val;
            public string Display { get { return val.ToString(); } }
            public filterValues RigItem { get { return val; } }
            public filterElement(filterValues v)
            {
                val = v;
            }
        }
        private ArrayList filterList;
        private filterElement[] filterItems =
        {new filterElement(filterValues.a),
         new filterElement(filterValues.b)};

        private class numericElement
        {
            private int val;
            public string Display { get { return val.ToString(); } }
            public int RigItem { get { return val; } }
            public numericElement(int v)
            {
                val = v;
            }
        }
        private ArrayList cwShiftList;

        private numericElement[] cwWidthValues =
        {new numericElement(50), new numericElement(80),
         new numericElement(100), new numericElement(150),
         new numericElement(200),
         new numericElement(300), new numericElement(400),
         new numericElement(500), new numericElement(600),
         new numericElement(1000),
         new numericElement(2000)};
        private ArrayList cwWidthList;

        private numericElement[] fskWidthValues =
        {new numericElement(250), new numericElement(500),
         new numericElement(1000), new numericElement(1500)};
        private ArrayList fskWidthList;

        // Map SH/SL control values to HZ
        private int[] SSBFMFMDLowValues = 
        { 0, 50, 100, 200, 300, 400, 500,
          600, 700, 800, 900, 1000 };
        private int[] SSBFMFMDHighValues =
        {1400,1600,1800,2000,2200,2400,2600,2800,
         3000,3400,4000,5000};
        private int[] AMLowValues = { 0, 100, 200, 500 };
        private int[] AMHighValues = { 2500, 3000, 4000, 5000 };
        // width uses SH, offset uses SL.
        private int[] SSBDOffsetValues =
        { 1000, 1100, 1200, 1300, 1400, 1500, 1600, 1700, 1800, 1900, 2000, 2100, 2210 };
        private int[] SSBDWidthValues = { 50, 80, 100, 150, 200, 250, 300, 400, 500, 600, 1000, 1500, 2000, 2500 };
        private class SHSL
        {
            int id;
            int[] disp;
            public SHSL(int i, int[] arr)
            {
                id = i;
                disp = arr;
            }
            public string Display
            {
                get { return (id<disp.Length)?disp[id].ToString():""; }
            }
            public int RigItem { get { return id; } }
        }
        private ArrayList SSBFMFMDLowList, SSBFMFMDHighList;
        private ArrayList SSBDOffsetList, SSBDWidthList;
        private ArrayList AMLowList, AMHighList;

        // TextBox setup data.
        // The ReadOnly TextBoxes are set using a table of values.
        private class textBoxValueClass
        {
            private TextBox box;
            private string[] tbl;
            public delegate int RigValueDel();
            private RigValueDel rigValue;
            private delegate void del(int val);
            private del dispFunc;
            private KenwoodTS2000 radio;
            public textBoxValueClass(TextBox tb, string[] t, RigValueDel rtn, KenwoodTS2000 rad)
            {
                box = tb;
                tbl = t;
                rigValue = rtn;
                dispFunc = (int val) => { box.Text = ((val >= 0) && (val < tbl.Length)) ? tbl[val] : ""; };
                radio = rad;
            }
            private int oldVal = -1;
            public void Value()
            {
                int val = rigValue();
                if (val != oldVal)
                {
                    oldVal = val;
                    if (box.InvokeRequired)
                    {
                        box.Invoke(dispFunc, new object[] { val });
                    }
                    else dispFunc(val);
                    //Tracing.TraceLine(box.Name + ":" + tbl[val]);
                }
            }
        }
        private Collection<textBoxValueClass> textBoxes;
        private string[] SWRValues =
        {"1.0","1.0","1.5","1.5","1.5","1.5","1.5",
         "2.0","2.0","2.0","2.0","2.0","3.0","3.0","3.0","3.0","3.0",
         "5.0","5.0","5.0","5.0","5.0","5.0","5.0","5.0",
         "Over","Over","Over","Over","Over","Over"};
        private string[] ALCValues =
        {"0","1","2","3","4","5","6","7","8","9",
         "10","11","12","13","14","over","over","over","over","over",
         "over","over","over","over","over","over",
         "over","over","over","over","over"};
        private string[] CompValues =
        {"0","10","10","10","10","10","10","10","10","10","10",
            "20","20","20","20","20","20","20","20","20","20",
            "over","over","over","over","over","over","over","over","over","over"};

        public TS2000Filters(KenwoodTS2000 r)
        {
            InitializeComponent();
            rig = r;
            Tracing.TraceLine("TS2000Filters constructor", TraceLevel.Info);

            // setup the boxes
            combos = new Collection<object>();

            // filter in use
            filterList = new ArrayList();
            foreach (filterElement f in filterItems)
            {
                filterList.Add(f);
            }
            FilterControl.TheList = filterList;
            FilterControl.UpdateDisplayFunction =
                () => { return (filterValues)rig.filterNum; };
            FilterControl.UpdateRigFunction =
                (object v) => { rig.filterNum = (int)v; };
            combos.Add(FilterControl);

            // cw shift
            cwShiftList = new ArrayList();
            // Values are from 400 to 1000 step 50.
            for (int i = 400; i <= 1000; i += 50)
            {
                cwShiftList.Add(new numericElement(i));
            }
            CWOffsetControl.TheList = cwShiftList;
            CWOffsetControl.UpdateDisplayFunction =
                () => { return rig.filterOffset; };
            CWOffsetControl.UpdateRigFunction =
                (object v) => { rig.filterOffset = (int)v; };
            combos.Add(CWOffsetControl);

            // cw width
            cwWidthList = new ArrayList();
            foreach (numericElement e in cwWidthValues)
            {
                cwWidthList.Add(e);
            }
            CWWidthControl.TheList = cwWidthList;
            CWWidthControl.UpdateDisplayFunction =
                () => { return rig.filterWidth; };
            CWWidthControl.UpdateRigFunction =
                (object v) => { rig.filterWidth = (int)v; };
            combos.Add(CWWidthControl);

            // FSK width
            fskWidthList = new ArrayList();
            foreach (numericElement e in fskWidthValues)
            {
                fskWidthList.Add(e);
            }
            FSKWidthControl.TheList = fskWidthList;
            FSKWidthControl.UpdateDisplayFunction =
                () => { return rig.filterWidth; };
            FSKWidthControl.UpdateRigFunction =
                (object v) => { rig.filterWidth = (int)v; };
            combos.Add(FSKWidthControl);

            SSBFMFMDLowList = new ArrayList();
            for (int i=0; i<SSBFMFMDLowValues.Length; i++)
            {
                SSBFMFMDLowList.Add(new SHSL(i, SSBFMFMDLowValues));
            }
            SSBFMFMDLowControl.TheList = SSBFMFMDLowList;
            SSBFMFMDLowControl.UpdateDisplayFunction =
                () => { return safeTableValue(SSBFMFMDLowValues,rig.filterLow); };
            SSBFMFMDLowControl.UpdateRigFunction =
                (object v) => { rig.filterLow = (int)v; };
            combos.Add(SSBFMFMDLowControl);

            SSBFMFMDHighList = new ArrayList();
            for (int i=0; i<SSBFMFMDHighValues.Length; i++)
            {
                SSBFMFMDHighList.Add(new SHSL(i, SSBFMFMDHighValues));
            }
            SSBFMFMDHighControl.TheList = SSBFMFMDHighList;
            SSBFMFMDHighControl.UpdateDisplayFunction =
                () => { return safeTableValue(SSBFMFMDHighValues,rig.filterHigh); };
            SSBFMFMDHighControl.UpdateRigFunction =
                (object v) => { rig.filterHigh = (int)v; };
            combos.Add(SSBFMFMDHighControl);

            SSBDOffsetList = new ArrayList();
            for (int i = 0; i < SSBDOffsetValues.Length; i++)
            {
                SSBDOffsetList.Add(new SHSL(i, SSBDOffsetValues));
            }
            SSBDOffsetControl.TheList = SSBDOffsetList;
            SSBDOffsetControl.UpdateDisplayFunction =
                () => { return safeTableValue(SSBDOffsetValues,rig.filterLow); };
            SSBDOffsetControl.UpdateRigFunction =
                (object v) => { rig.filterLow = (int)v; };
            combos.Add(SSBDOffsetControl);

            SSBDWidthList = new ArrayList();
            for (int i = 0; i < SSBDWidthValues.Length; i++)
            {
                SSBDWidthList.Add(new SHSL(i, SSBDWidthValues));
            }
            SSBDWidthControl.TheList = SSBDWidthList;
            SSBDWidthControl.UpdateDisplayFunction =
                () => { return safeTableValue(SSBDWidthValues,rig.filterHigh); };
            SSBDWidthControl.UpdateRigFunction =
                (object v) => { rig.filterHigh = (int)v; };
            combos.Add(SSBDWidthControl);

            AMLowList = new ArrayList();
            for (int i = 0; i < AMLowValues.Length; i++)
            {
                AMLowList.Add(new SHSL(i, AMLowValues));
            }
            AMLowControl.TheList = AMLowList;
            AMLowControl.UpdateDisplayFunction =
                () => { return safeTableValue(AMLowValues,rig.filterLow); };
            AMLowControl.UpdateRigFunction =
                (object v) => { rig.filterLow = (int)v; };
            combos.Add(AMLowControl);

            AMHighList = new ArrayList();
            for (int i = 0; i < AMHighValues.Length; i++)
            {
                AMHighList.Add(new SHSL(i, AMHighValues));
            }
            AMHighControl.TheList = AMHighList;
            AMHighControl.UpdateDisplayFunction =
                () => { return safeTableValue(AMHighValues, rig.filterHigh); };
            AMHighControl.UpdateRigFunction =
                (object v) => { rig.filterHigh = (int)v; };
            combos.Add(AMHighControl);

            // readOnly textboxes
            textBoxes = new Collection<textBoxValueClass>();
            textBoxes.Add(new textBoxValueClass(SWRBox, SWRValues,
                () => { return rig.SWRRaw; }, rig));
            textBoxes.Add(new textBoxValueClass(CompBox, CompValues,
                () => { return rig.compRaw; }, rig));
            textBoxes.Add(new textBoxValueClass(ALCBox, ALCValues,
                () => { return rig.ALCRaw; }, rig));

            // setup the mode change stuff.
            modeChange = new modeChangeClass(this);

            // setup RigFields
            rig.RigFields = new AllRadios.RigInfo(this, updateBoxes);
        }
        private int safeTableValue(int[] arr, int id)
        {
            return (id < arr.Length) ? arr[id] : 0;
        }

        private void Filters_Load(object sender, EventArgs e)
        {
            Tracing.TraceLine("TS2000Filters_load", TraceLevel.Info);
        }

        private void updateBoxes()
        {
            Tracing.TraceLine("updateBoxes", TraceLevel.Verbose);
            try
            {
                // enable/disable boxes for this mode.
                modeChange.enableDisable(rig.Mode, rig.DataMode);

                foreach (Combo c in combos)
                {
                    if (c.Enabled)
                    {
                        c.UpdateDisplay();
                    }
                }
                // Currently the text boxes are always enabled.
                foreach (textBoxValueClass t in textBoxes)
                {
                    t.Value();
                }
            }
            catch (Exception ex)
            {
                Tracing.TraceLine("updateBoxes:" + ex.Message + ex.StackTrace, TraceLevel.Error);
            }
        }

        private class modeChangeClass
        {
            // A mode's filter controls are enabled when that mode is active.
            // First, the specified controls for the other modes are disabled,
            // unless they're just going to be enabled again.
            // Note each mode has two arrays, without and with data mode.
            private class controlsClass
            {
                public Control[,] controls;
                public controlsClass(Control[] wo,Control[] w)
                {
                    controls=new Control[2,2];
                    for (int i = 0; i < wo.Length; i++)
                    {
                        controls[0, i] = wo[i];
                    }
                    for (int i = 0; i < w.Length; i++)
                    {
                        controls[1, i] = w[i];
                    }
                }
            }
            private controlsClass[] modeControls;
            private TS2000Filters parent;
            public modeChangeClass(TS2000Filters p)
            {
                parent = p;
                modeControls = new controlsClass[parent.rig.Mode.GetType().GetEnumNames().Length];

                // setup the mode to combobox mapping, without and with datamode.
                modeControls[(int)AllRadios.Modes.lsb] = new controlsClass(
                    new Control[] { parent.SSBFMFMDLowControl, parent.SSBFMFMDHighControl },
                    new Control[] { parent.SSBDOffsetControl, parent.SSBDWidthControl });
                modeControls[(int)AllRadios.Modes.usb] = new controlsClass(
                    new Control[] { parent.SSBFMFMDLowControl, parent.SSBFMFMDHighControl },
                    new Control[] { parent.SSBDOffsetControl, parent.SSBDWidthControl });
                modeControls[(int)AllRadios.Modes.cw] = new controlsClass(
                    new Control[] { parent.CWOffsetControl, parent.CWWidthControl },
                    new Control[] { parent.CWOffsetControl, parent.CWWidthControl });
                modeControls[(int)AllRadios.Modes.cwr] = new controlsClass(
                    new Control[] { parent.CWOffsetControl, parent.CWWidthControl },
                    new Control[] { parent.CWOffsetControl, parent.CWWidthControl });
                modeControls[(int)AllRadios.Modes.fsk] = new controlsClass(
                    new Control[] {parent.FSKWidthControl},
                    new Control[] {parent.FSKWidthControl});
                modeControls[(int)AllRadios.Modes.fskr] = new controlsClass(
                    new Control[] { parent.FSKWidthControl },
                    new Control[] { parent.FSKWidthControl });
                modeControls[(int)AllRadios.Modes.fm] = new controlsClass(
                    new Control[] { parent.SSBFMFMDLowControl, parent.SSBFMFMDHighControl },
                    new Control[] { parent.SSBFMFMDLowControl, parent.SSBFMFMDHighControl });
                modeControls[(int)AllRadios.Modes.am] = new controlsClass(
                    new Control[] { parent.AMLowControl, parent.AMHighControl },
                    new Control[] { parent.AMLowControl, parent.AMHighControl });
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
            private AllRadios.Modes oldMode = AllRadios.Modes.None;
            private AllRadios.DataModes oldDataMode = AllRadios.DataModes.unset;
            public void enableDisable(AllRadios.Modes mode, AllRadios.DataModes dataMode)
            {
                // Just quit if the mode hasn't changed.
                if ((mode == oldMode) && (dataMode == oldDataMode)) return;
                oldMode = mode;
                oldDataMode = dataMode;
                int mod = (int)mode;
                // quit if no controls for this mode.
                if (modeControls[mod] == null) return;
                // dat is the datamode index.
                int dat = (dataMode == AllRadios.DataModes.off) ? 0 : 1;
                // enables holds the controls to be enabled.
                Control[] enables = new Control[modeControls[mod].controls.GetLength(1)];
                for (int i=0; i<enables.Length; i++)
                {
                    enables[i] = modeControls[mod].controls[dat, i];
                }
                parent.SuspendLayout();
                for (int i = 0; i < modeControls.Length; i++)
                {
                    if (modeControls[i] == null) continue;
                    for (int j = 0; j < modeControls[i].controls.GetLength(0); j++)
                    {
                        for (int k = 0; k < modeControls[i].controls.GetLength(1); k++)
                        {
                            Control c = modeControls[i].controls[j, k];
                            if (c == null) continue;
                            if (Array.IndexOf(enables,c) >= 0)
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
                }
                parent.ResumeLayout();
            }
        }
        private modeChangeClass modeChange;

        private void BoxKeydownDefault(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }
    }
}
