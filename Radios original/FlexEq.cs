using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Flex.Smoothlake.FlexLib;
using JJTrace;
using RadioBoxes;

namespace Radios
{
    public partial class FlexEq : Form
    {
        private const string RXEqTitle = "Receive equalizer";
        private const string TXEqTitle = "Transmit equalizer";
        private FlexBase rig;
        private Radio theRadio
        {
            get { return (rig != null) ? rig.theRadio : null; }
        }
        private Equalizer eq;
        private Collection<NumberBox> boxes;
        private int[] originals;

        public FlexEq(FlexBase r, Equalizer q)
        {
            InitializeComponent();

            rig = r;
            eq = q;
            this.Text = (eq.EQ_select == EqualizerSelect.RX) ? RXEqTitle : TXEqTitle;

            boxes = new Collection<NumberBox>();

            Level63Control.UpdateDisplayFunction =
                () => { return eq.level_63Hz; };
            Level63Control.UpdateRigFunction =
                (int v) => { rig.q.Enqueue((FlexBase.FunctionDel)(() => { eq.level_63Hz = v; })); };
            boxes.Add(Level63Control);
            Level125Control.UpdateDisplayFunction =
                () => { return eq.level_125Hz; };
            Level125Control.UpdateRigFunction =
                (int v) => { rig.q.Enqueue((FlexBase.FunctionDel)(() => { eq.level_125Hz = v; })); };
            boxes.Add(Level125Control);
            Level250Control.UpdateDisplayFunction =
                () => { return eq.level_250Hz; };
            Level250Control.UpdateRigFunction =
                (int v) => { rig.q.Enqueue((FlexBase.FunctionDel)(() => { eq.level_250Hz = v; })); };
            boxes.Add(Level250Control);
            Level500Control.UpdateDisplayFunction =
                () => { return eq.level_500Hz; };
            Level500Control.UpdateRigFunction =
                (int v) => { rig.q.Enqueue((FlexBase.FunctionDel)(() => { eq.level_500Hz = v; })); };
            boxes.Add(Level500Control);
            Level1000Control.UpdateDisplayFunction =
                () => { return eq.level_1000Hz; };
            Level1000Control.UpdateRigFunction =
                (int v) => { rig.q.Enqueue((FlexBase.FunctionDel)(() => { eq.level_1000Hz = v; })); };
            boxes.Add(Level1000Control);
            Level2000Control.UpdateDisplayFunction =
                () => { return eq.level_2000Hz; };
            Level2000Control.UpdateRigFunction =
                (int v) => { rig.q.Enqueue((FlexBase.FunctionDel)(() => { eq.level_2000Hz = v; })); };
            boxes.Add(Level2000Control);
            Level4000Control.UpdateDisplayFunction =
                () => { return eq.level_4000Hz; };
            Level4000Control.UpdateRigFunction =
                (int v) => { rig.q.Enqueue((FlexBase.FunctionDel)(() => { eq.level_4000Hz = v; })); };
            boxes.Add(Level4000Control);
            Level8000Control.UpdateDisplayFunction =
                () => { return eq.level_8000Hz; };
            Level8000Control.UpdateRigFunction =
                (int v) => { rig.q.Enqueue((FlexBase.FunctionDel)(() => { eq.level_8000Hz = v; })); };
            boxes.Add(Level8000Control);

            originals = new int[boxes.Count];

            foreach (NumberBox box in boxes)
            {
                box.LowValue = -10;
                box.HighValue = 10;
                box.Increment = 1;
            }
        }

        private void FlexEq_Load(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;

            updateBoxes();

            // Set original values.
            for (int i = 0; i < boxes.Count; i++)
            {
                originals[i] = boxes[i].UpdateDisplayFunction();
            }
        }

        private void updateBoxes()
        {
            foreach (NumberBox box in boxes)
            {
                box.UpdateDisplay(true);
            }
        }

        private void RestoreButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].UpdateRigFunction(originals[i]);
                FlexBase.await(() => { return boxes[i].UpdateDisplayFunction() == originals[i]; }, 100, 10);
            }
            updateBoxes();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            foreach (NumberBox box in boxes)
            {
                box.UpdateRigFunction(0);
                FlexBase.await(() => { return box.UpdateDisplayFunction() == 0; }, 1000, 10);
            }

            updateBoxes();
        }

        private void FinishedButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void CnclButton_Click(object sender, EventArgs e)
        {
            // restore old values.
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].UpdateRigFunction(originals[i]);
            }
            DialogResult = DialogResult.Cancel;
        }
    }
}
