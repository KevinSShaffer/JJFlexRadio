using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MsgLib
{
    public partial class MessageForm : Form
    {
        class MessageEventArg
        {
            public DialogResult Result;
            public MessageEventArg(DialogResult r)
            {
                Result = r;
            }
        }
        private delegate void MessageEventDel(MessageEventArg e);
        private event MessageEventDel MessageEvent;

        private OptionalMessageElement configArg;

        public MessageForm(OptionalMessageElement parm)
        {
            InitializeComponent();

            TextOut.DisplayText(this, parm.Title);
            configArg = parm;
            MessageEvent += MessageEventHandler;

            parm.Control.Form = this;
            parm.Control.Location = new Point(0, 0);
            Controls.Add(parm.Control);
        }

        private void MessageForm_Load(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;

            // See if we should show this.
            OptionalMessageElement el = OptionalMessage.FindItem(configArg.Tag);
            if ((el != null) && el.Ignore)
            {
                DialogResult = el.Result;
            }
        }

        private void MessageEventHandler(MessageEventArg e)
        {
            if (DontshowBox.Checked)
            {
                configArg.Result = e.Result;
                configArg.Ignore = true;
                OptionalMessage.WriteResult(configArg);
            }

            DialogResult = e.Result;
        }

        /// <summary>
        /// Called by the UserControl when finished.
        /// </summary>
        /// <param name="result">DialogResult</param>
        public void SendMessageEvent(DialogResult result)
        {
            MessageEvent(new MessageEventArg(result));
        }

        private bool wasActive = false;
        private void MessageForm_Activated(object sender, EventArgs e)
        {
            if (!wasActive) configArg.Control.Focus();
            wasActive = true;
        }
    }
}
