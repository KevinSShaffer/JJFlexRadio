using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using JJTrace;

namespace Radios
{
    public partial class AuthForm : Form
    {
        public string[] Tokens;

        public AuthForm()
        {
            InitializeComponent();
        }

        private void AuthForm_Load(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;
            try
            {
                string uriString = "http://frtest.auth0.com/authorize?";
                uriString += "response_type=token&";
                uriString += "client_id=4Y9fEIIsVYyQo5u6jr7yBWc4lV5ugC2m&";
                uriString += "redirect_uri=https://frtest.auth0.com/mobile&";
                uriString += "scope=openid offline_access email given_name family_name picture&";
                string state = "";
                Random r = new Random();
                for (int i = 0; i < 16; i++)
                {
                    int j = r.Next(0x41, 0x5a);
                    state += (char)j;
                }
                //uriString += "state=ypfolheqwpezryrc&";
                uriString += "state=" + state;
                uriString += "&device=JJRadio";
                Tracing.TraceLine("AuthForm URI:" + uriString, TraceLevel.Info);
                Uri uri = new Uri(uriString);
                Browser.DocumentCompleted += documentLoadedHandler;
                Browser.Navigate(uri);
            }
            catch (Exception ex)
            {
                Tracing.TraceLine("AuthForm Exception: " + ex.Message, TraceLevel.Error);
                DialogResult = DialogResult.Abort;
            }
        }

        private void documentLoadedHandler(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Tracing.TraceLine("AuthForm documentLoadedHandler:" + e.ToString(), TraceLevel.Info);
            DialogResult = DialogResult.Abort; // default on error.
            if (Browser.Url.AbsolutePath == "/mobile")
            {
                string str = Browser.Url.ToString();
                Tracing.TraceLine("AuthForm received:" + str, TraceLevel.Info);
                Tokens = str.Split(new char[] { '&' });
                DialogResult = DialogResult.OK;
                //this.Close();
            }
        }
    }
}
