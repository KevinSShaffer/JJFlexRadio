using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using JJTrace;

namespace JJLogLib
{
    public partial class LogProc
    {
        private bool _LookingUp;
        /// <summary>
        /// True if looking up calls.
        /// </summary>
        public bool LookingUp
        {
            get { return _LookingUp; }
            set
            {
                // Must be able to lookup if setting to true.
                _LookingUp = (value & Logs.CanLookup);
            }
        }

        /// <summary>
        /// HamQTH Session
        /// </summary>
        public class Session
        {
            public string session_id;
            public string error;
        }
        /// <summary>
        /// HamQTH operator data returned from a search.
        /// </summary>
        public class Search
        {
            public string callsign;
            public string nick;
            public string qth;
            public string adr_city;
            public string adr_zip;
            public string adr_country;
            public string adr_adif;
            public string district;
            [XmlIgnore]
            public string State
            {
                get
                {
                    string rv = (!string.IsNullOrEmpty(district)) ? district : "";
                    if ((rv == "") && !string.IsNullOrEmpty(adr_zip) && (adr_zip.Length >= 5))
                    {
                        int n = 0;
                        if (System.Int32.TryParse(adr_zip.Substring(0, 3), out n))
                        {
                            rv = Logs.StateMap.State[n];
                        }
                    }
                    return rv;
                }
            }
            public string country;
            public string adif;
            public string itu;
            public string cq;
            public string grid;
            public string latitude;
            [XmlIgnore] public string lat
            {
                get
                {
                    string rv = latitude;
                    double l = 0;
                    if (System.Double.TryParse(latitude, out l))
                    {
                        char ns = (l < 0) ? 'S' : 'N';
                        l = Math.Abs(l) + 0.5;
                        rv = ((int)l).ToString() + ns;
                    }
                    return rv;
                }
            }
            public string longitude;
            [XmlIgnore]
            public string lgt
            {
                get
                {
                    string rv = longitude;
                    double l = 0;
                    if (System.Double.TryParse(longitude, out l))
                    {
                        char ew = (l < 0) ? 'W' : 'E';
                        l = Math.Abs(l) + 0.5;
                        rv = ((int)l).ToString() + ew;
                    }
                    return rv;
                }
            }
            [XmlIgnore]
            public string LatLong
            {
                get { return lat + '/' + lgt; }
            }
            public string continent;
            public string utc_offset;
            public string lotw;
            public string qsl;
            public string qsldirect;
            public string eqsl;
            public string email;
            public string jabber;
            public string skype;
            public string birth_year;
            public string lic_year;
            public string web;
            public string picture;
        }
        /// <summary>
        /// Data returned by HamQTH search.
        /// </summary>
        public class HamQTH
        {
            public Session session;
            public Search search;
        }

        internal Thread webThread;
        internal delegate void CallsignSearchDel(HamQTH e);
        internal event CallsignSearchDel CallsignSearchEvent;
        internal void onCallsignSearch(HamQTH qth)
        {
            if (CallsignSearchEvent != null)
            {
                Tracing.TraceLine("onCallsignSearch", TraceLevel.Info);
                CallsignSearchEvent(qth);
            }
            else
            {
                Tracing.TraceLine("onCallsignSearch no event", TraceLevel.Error);
            }
        }

        public void CallLookupDone()
        {
            if (Logs.loginTimer != null) Logs.loginTimer.Dispose();
            Logs.loginTimer = null;
        }

        /// <summary>
        /// Login to the site.
        /// LoginInfo should be locked.
        /// </summary>
        /// <returns>True on success</returns>
        private bool siteLogin()
        {
            Tracing.TraceLine("siteLogin", TraceLevel.Info);
            bool rv = true;
            WebClient web = null;
            web = new WebClient();
            web.BaseAddress = Logs.siteBaseAddress;
            Stream page = null;
            HamQTH dat = null;
            try
            {
                page = web.OpenRead("/xml.php?u=" + Logs.LoginInfo.LoginID + "&p=" + Logs.LoginInfo.Password);
                XmlSerializer xs = new XmlSerializer(typeof(HamQTH), Logs.hamqthRoot);
                dat = (HamQTH)xs.Deserialize(page);
                if (dat.session.error != null)
                {
                    //throw (new Exception(dat.session.error));
                    rv = false;
                }
                else
                {
                    Logs.LoginInfo.SessionID = dat.session.session_id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.InnerException.Message, "login error", MessageBoxButtons.OK);
                rv = false;
            }
            finally
            {
                if (page != null) page.Dispose();
                if (web != null) web.Dispose();
            }
            return rv;
        }

        /// <summary>
        /// Internet call search, done under a thread.
        /// </summary>
        /// <param name="o">object, callsign</param>
        private void siteLookupCall(object o)
        {
            try
            {
                string callSign = (string)o;
                Tracing.TraceLine("siteLookupCall:" + callSign, TraceLevel.Info);
                HamQTH rv = null;
                WebClient web = null;
                web = new WebClient();
                web.BaseAddress = Logs.siteBaseAddress;
                Stream page = null;
                lock (Logs.LoginInfo)
                {
                    // login if needed.
                    if (Logs.LoginInfo.NeedLogin)
                    {
                        Logs.LoginInfo.NeedLogin = !siteLogin();
                        if (Logs.LoginInfo.NeedLogin)
                        {
                            // Login failed.
                            Logs.LoginInfo.FailCount++;
                            Tracing.TraceLine("SiteLookupCall:login failure #" + Logs.LoginInfo.FailCount.ToString(), TraceLevel.Error);
                            if (Logs.LoginInfo.FailCount == Logs.maxLoginFailures)
                            {
                                LookingUp = false;
                            }
                        }
                        else
                        {
                            // login success.
                            Logs.loginTimer.Interval = Logs.loginTimeout;
                            Logs.loginTimer.Enabled = true;
                        }
                    }

                    // Shouldn't need login now.
                    if (!Logs.LoginInfo.NeedLogin)
                    {
                        try
                        {
                            page = web.OpenRead("/xml.php?id=" + Logs.LoginInfo.SessionID + "&callsign=" + callSign + "&prg=JJRadio");
                            XmlSerializer xs = new XmlSerializer(typeof(HamQTH), Logs.hamqthRoot);
                            rv = (HamQTH)xs.Deserialize(page);
                            if (((rv.session != null) &&
                                !string.IsNullOrEmpty(rv.session.error)) ||
                                (rv.search == null)) rv = null;
                        }
                        catch (Exception ex)
                        {
                            Tracing.ErrMessageTrace(ex);
                            // rv already null;
                        }
                        finally
                        {
                            if (page != null) page.Dispose();
                            if (web != null) web.Dispose();
                        }
                    }
                }
                if (rv != null)
                {
                    rv.search.callsign = rv.search.callsign.ToUpper();
                    if (!Logs.callDictionary.Keys.Contains(rv.search.callsign))
                    {
                        Logs.callDictionary.Add(rv.search.callsign, rv);
                    }
                }
                onCallsignSearch(rv);
            }
            catch (ThreadAbortException)
            {
                Tracing.TraceLine("siteLookupCall aborted", TraceLevel.Error);
            }
            catch (Exception ex)
            {
                Tracing.TraceLine("siteLookupCall exception:" + ex.Message, TraceLevel.Error);
            }
        }

        public void LookupCall(string callSign)
        {
            if (!LookingUp) return;
            HamQTH rv = null;
            // First check the cache.
            if (!Logs.callDictionary.TryGetValue(callSign.ToUpper(), out rv))
            {
                webThread = new Thread(siteLookupCall);
                webThread.Name = "webThread";
                // webThread posts the event.
                webThread.Start(callSign);
            }
            else onCallsignSearch(rv);
        }

        /// <summary>
        /// Used to kill the lookup thread.
        /// </summary>
        public void KillLookupThread()
        {
            if (LookingUp && (webThread != null))
            {
                try
                {
                    if (webThread.IsAlive)
                    {
                        Tracing.TraceLine("log form closing:aborting call lookup thread", TraceLevel.Info);
                        webThread.Abort();
                    }
                }
                catch { }
            }
        }
    }
}
