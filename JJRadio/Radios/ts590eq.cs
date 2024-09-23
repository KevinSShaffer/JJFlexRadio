using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RadioBoxes;

namespace Radios
{
    public partial class ts590eq : Form
    {
        private KenwoodTS590 rig;
        private bool tx;
        private Control lastActiveField;

        private Collection<NumberBox> numberBoxes;

        private int getVal(int num)
        {
            return (tx) ? rig.GetTXEQ(num) : rig.GetRXEQ(num);
        }

        private void setVal(int num, int val)
        {
            if (tx) rig.SetTXEQ(num, val);
            else rig.SetRXEQ(num, val);
        }

        public ts590eq(KenwoodTS590 r, bool txFlag)
        {
            InitializeComponent();

            rig = r;
            tx = txFlag;
            numberBoxes = new Collection<NumberBox>();

            // the min, max, and increments are set in the designer.
            hz0.UpdateDisplayFunction =
                () => { return getVal(0); };
            hz0.UpdateRigFunction =
                (int v) => { setVal(0, v); };
            numberBoxes.Add(hz0);
            hz300.UpdateDisplayFunction =
                () => { return getVal(1); };
            hz300.UpdateRigFunction =
                (int v) => { setVal(1, v); };
            numberBoxes.Add(hz300);
            hz600.UpdateDisplayFunction =
                () => { return getVal(2); };
            hz600.UpdateRigFunction =
                (int v) => { setVal(2, v); };
            numberBoxes.Add(hz600);
            hz900.UpdateDisplayFunction =
                () => { return getVal(3); };
            hz900.UpdateRigFunction =
                (int v) => { setVal(3, v); };
            numberBoxes.Add(hz900);
            hz1200.UpdateDisplayFunction =
                () => { return getVal(4); };
            hz1200.UpdateRigFunction =
                (int v) => { setVal(4, v); };
            numberBoxes.Add(hz1200);
            hz1500.UpdateDisplayFunction =
                () => { return getVal(5); };
            hz1500.UpdateRigFunction =
                (int v) => { setVal(5, v); };
            numberBoxes.Add(hz1500);
            hz1800.UpdateDisplayFunction =
                () => { return getVal(6); };
            hz1800.UpdateRigFunction =
                (int v) => { setVal(6, v); };
            numberBoxes.Add(hz1800);
            hz2100.UpdateDisplayFunction =
                () => { return getVal(7); };
            hz2100.UpdateRigFunction =
                (int v) => { setVal(7, v); };
            numberBoxes.Add(hz2100);
            hz2400.UpdateDisplayFunction =
                () => { return getVal(8); };
            hz2400.UpdateRigFunction =
                (int v) => { setVal(8, v); };
            numberBoxes.Add(hz2400);
            hz2700.UpdateDisplayFunction =
                () => { return getVal(9); };
            hz2700.UpdateRigFunction =
                (int v) => { setVal(9, v); };
            numberBoxes.Add(hz2700);
            hz3000.UpdateDisplayFunction =
                () => { return getVal(10); };
            hz3000.UpdateRigFunction =
                (int v) => { setVal(10, v); };
            numberBoxes.Add(hz3000);
            hz3300.UpdateDisplayFunction =
                () => { return getVal(11); };
            hz3300.UpdateRigFunction =
                (int v) => { setVal(11, v); };
            numberBoxes.Add(hz3300);
            hz3600.UpdateDisplayFunction =
                () => { return getVal(12); };
            hz3600.UpdateRigFunction =
                (int v) => { setVal(12, v); };
            numberBoxes.Add(hz3600);
            hz3900.UpdateDisplayFunction =
                () => { return getVal(13); };
            hz3900.UpdateRigFunction =
                (int v) => { setVal(13, v); };
            numberBoxes.Add(hz3900);
            hz4200.UpdateDisplayFunction =
                () => { return getVal(14); };
            hz4200.UpdateRigFunction =
                (int v) => { setVal(14, v); };
            numberBoxes.Add(hz4200);
            hz4500.UpdateDisplayFunction =
                () => { return getVal(15); };
            hz4500.UpdateRigFunction =
                (int v) => { setVal(15, v); };
            numberBoxes.Add(hz4500);
            hz4800.UpdateDisplayFunction =
                () => { return getVal(16); };
            hz4800.UpdateRigFunction =
                (int v) => { setVal(16, v); };
            numberBoxes.Add(hz4800);
            hz5100.UpdateDisplayFunction =
                () => { return getVal(17); };
            hz5100.UpdateRigFunction =
                (int v) => { setVal(17, v); };
            numberBoxes.Add(hz5100);

            // default to enter at hz0.
            lastActiveField = hz0;
        }

        internal void UpdateBoxes()
        {
            foreach (NumberBox n in numberBoxes)
            {
                n.UpdateDisplay();
            }
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void hz_Enter(object sender, EventArgs e)
        {
            lastActiveField = (Control)sender;
        }

        private void ts590eq_Activated(object sender, EventArgs e)
        {
            lastActiveField.Focus();
        }
    }
}
