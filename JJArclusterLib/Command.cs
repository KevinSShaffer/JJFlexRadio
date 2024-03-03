using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JJArClusterLib
{
    public partial class Command : Form
    {
        public Command()
        {
            InitializeComponent();
        }

        public string TheCommand
        {
            get { return CommandBox.Text; }
        }
        private void OkButton_Click(object sender, EventArgs e)
        {
            if (CommandBox.Text.Length > 0) DialogResult = DialogResult.OK;
            else DialogResult = DialogResult.Cancel;
        }

        private void CnclButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
