using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MsgLib
{
    public partial class DefaultUserControl : CalledUserControl
    {
        public DefaultUserControl(OptionalMessageElement msg)
        {
            InitializeComponent();
            Message = msg;
            TextOut.DisplayText(MessageText, msg.Text);
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Message.Result = DialogResult.OK;
            Form.SendMessageEvent(DialogResult.OK);
        }
    }
}
