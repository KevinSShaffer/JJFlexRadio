using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Radios
{
    public partial class GetFile : Form
    {
        private const string existHdr = "Exists";
        private const string replaceFile = " exists, replace it?";
        private bool replaceCheck;
        internal string FileName;

        /// <summary>
        /// Get a file.
        /// </summary>
        /// <param name="title">dialogue title</param>
        /// <param name="suffix">file's suffix</param>
        /// <param name="replace">true for warning if file exists</param>
        public GetFile(string title, string suffix,
            bool replace=false)
        {
            InitializeComponent();

            this.Text = title;
            replaceCheck = replace;
            Opener.DefaultExt = suffix;
            Opener.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        private void GetFile_Load(object sender, EventArgs e)
        {
            DialogResult rv;
            while (true)
            {
                Opener.FileName = Opener.SafeFileName;
                rv = Opener.ShowDialog();
                FileName = Opener.FileName;
                if (string.IsNullOrEmpty(FileName)) break;
                if ((rv == DialogResult.OK) & replaceCheck)
                {
                    if (File.Exists(FileName))
                    {
                        if (MessageBox.Show(Opener.SafeFileName + replaceFile, existHdr, MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            // get another file
                            continue;
                        }
                    }
                    // doesn't exist.
                }
                break;
            }
            DialogResult = (string.IsNullOrEmpty(FileName)) ?
                DialogResult.Cancel : DialogResult.OK;
        }
    }
}
