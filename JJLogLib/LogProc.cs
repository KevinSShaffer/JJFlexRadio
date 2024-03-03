#undef UseAlternateCountry
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using adif;
using HamQTHLookup;
using JJCountriesDB;
using JJTrace;
using MsgLib;
using SKCC;

namespace JJLogLib
{
    /// <summary>
    /// Log field processing, inherrited by LogElement.
    /// </summary>
    public partial class LogProc
    {
        private const string requiredFieldMissing = "Required screen field is missing.";
        internal CountriesDB countriesDB;
        private Logs.LogElement theLog;
        // Maps field names to their LogField data.
        private Dictionary<string, LogField> fieldMap;

        private long _FilePosition = -1;
        /// <summary>
        /// File's position.
        /// </summary>
        public long FilePosition
        {
            get { return _FilePosition; }
            set { _FilePosition = value; }
        }

        /// <summary>
        /// True if current record needs to be written.
        /// </summary>
        public bool NeedsWrite;
        /// <summary>
        /// true to ignore field text changes.
        /// </summary>
        public bool IgnoreTextChange;
        /// <summary>
        /// True if working with an on-file entry.
        /// </summary>
        public bool onFile;
        /// <summary>
        /// True if searching.
        /// </summary>
        public bool Searching;

        /// <summary>
        /// True if country info is needed.
        /// </summary>
        public bool NeedCountryInfo
        {
            get
            {
                return (getScreenElement(AdifTags.ADIF_Country) != null);
            }
        }

        internal void procSetup(Logs.LogElement el)
        {
            Tracing.TraceLine("procSetup:" + el.Name, TraceLevel.Info);
            theLog = el;
            fieldMap = new Dictionary<string, LogField>();
            foreach (LogField field in el.Fields.Values)
            {
                if (field.control != null)
                {
                    fieldMap.Add(field.control.Name, field);
                    // Add interrupt handlers.
                    field.control.Leave += leaveHandler;
                    field.control.TextChanged += textChangedHandler;
                    field.control.KeyDown += keyDownHandler;
#if zero
                    if (field.control.GetType().Name == "ComboBox")
                    {
                        ((ComboBox)field.control).SelectedValueChanged += comboSelectionChangedHandler;
                    }
#endif
                }
            }
            if (NeedCountryInfo) countriesDB = new CountriesDB();
            else Tracing.TraceLine("ProcSetup:no country fields.", TraceLevel.Info);
            SetupOriginalContent();
            if (Logs.lookup != null)
            {
                // Note that the following -= won't fail if the handler isn't there.
                Logs.lookup.CallsignSearchEvent -= callLookupDoneHandler;
                Logs.lookup.CallsignSearchEvent += callLookupDoneHandler;
            }
        }

        private LogField getField(object sender)
        {
            LogField rv;
            string name = ((Control)sender).Name;
            if (!fieldMap.TryGetValue(name, out rv))
            {
                MessageBox.Show("unknown field:" + name, "Error", MessageBoxButtons.OK);
                rv = null;
            }
            return rv;
        }

        /// <summary>
        /// Checkpoint the form's original content.
        /// </summary>
        public void SetupOriginalContent()
        {
            foreach (LogField fld in fieldMap.Values)
            {
                if (fld.IsDisplayed)
                {
                    fld.originalContent = fld.Item;
                }
            }
        }

        /// <summary>
        /// Restore checkpointed content.
        /// </summary>
        public void RestoreOriginalContent()
        {
            foreach (LogField fld in fieldMap.Values)
            {
                if (fld.IsDisplayed)
                {
                    fld.Item = fld.originalContent;
                }
            }
        }

        /// <summary>
        /// Clear the displayable fields.
        /// </summary>
        public void ClearAllContent()
        {
            Tracing.TraceLine("ClearAllContent", TraceLevel.Info);
            bool wasIgnored = IgnoreTextChange;
            IgnoreTextChange = true;
            theLog.TheForm.SuspendLayout();            
            foreach (LogField fld in fieldMap.Values)
            {
                if (fld.IsDisplayed)
                {
                    fld.Item = "";
                }
            }
            theLog.TheForm.ResumeLayout();
            IgnoreTextChange = wasIgnored;
        }

        /// <summary>
        /// (overloaded) get the LogField item for a screen field.
        /// </summary>
        /// <param name="adif">tag</param>
        /// <param name="required">true if field is required, default is False.</param>
        /// <returns>the LogField</returns>
        public LogField getScreenElement(string adif, bool required)
        {
            LogField rv;
            if (!theLog.Fields.TryGetValue(adif, out rv))
            {
                rv = null;
                if (required)
                {
                    MessageBox.Show(requiredFieldMissing, "Error", MessageBoxButtons.OK);
                }
            }
            return rv;
        }
        public LogField getScreenElement(string adif)
        {
            return getScreenElement(adif, false);
        }

        /// <summary>
        /// (Overloaded) Get the text for the given screen field.
        /// </summary>
        /// <param name="adif">tag</param>
        /// <param name="required">True if required, default is False.</param>
        /// <returns>text string</returns>
        public bool getScreenText(string adif, bool required, out string txt)
        {
            bool rv;
            LogField fld = getScreenElement(adif, required);
            if (fld == null) 
            {
                txt = null;
                rv = false;
            }
            else
            {
                rv = true;
                txt = fld.Item;
            }
            return rv;
        }
        public bool getScreenText(string adif, out string txt)
        {
            return getScreenText(adif, false, out txt);
        }

        /// <summary>
        /// (Overloaded) Set a screen field's text.
        /// </summary>
        /// <param name="adif">tag</param>
        /// <param name="txt">the text</param>
        /// <param name="required">true if required, default is False.</param>
        public void setScreenText(string adif, string txt, bool required)
        {
            LogField fld = getScreenElement(adif, required);
            if (fld != null) fld.Item=txt;
        }
        public void setScreenText(string adif, string txt)
        {
            setScreenText(adif, txt, false);
        }

        /// <summary>
        /// Set a screen field's text if not set.
        /// </summary>
        /// <param name="adif">tag</param>
        /// <param name="txt">the text</param>
        public void setScreenTextIfNotSet(string adif, string txt)
        {
            LogField fld = getScreenElement(adif);
            if ((fld != null) && (fld.Item == "")) fld.Item = txt;
        }

#if zero
        /// <summary>
        /// setProcessor routine for a country combobox.
        /// </summary>
        /// <param name="fld"></param>
        /// <param name="text"></param>
        internal void countrySetup(LogField fld, string text)
        {
            TextBox tb = (TextBox)fld.control;
            if (!string.IsNullOrEmpty(text))
            {
                Tracing.TraceLine("countrySetup:" + text, TraceLevel.Info);
                tb.Text = text;
                setCountryItems();
            }
            else
            {
                Tracing.TraceLine("countrySetup:null", TraceLevel.Info);
                tb.Text = "";
            }
        }
#endif

        /// <summary>
        /// Set the country comboBox.
        /// </summary>
        /// <remarks>
        /// This is used as the set processor for the country field.
        /// Thus it may not set the text using the LogField.Item property.
        /// </remarks>
        internal void setCountryItems()
        {
            Tracing.TraceLine("setCountryItems", TraceLevel.Info);
            // Leave if there's no call or country data.
            string callText;
            LogField countryFld = getScreenElement(AdifTags.ADIF_Country);
            if ((!getScreenText(AdifTags.ADIF_Call, true, out callText) || (callText == "")) |
                !NeedCountryInfo)
            {
                return;
            }

            // Lookup the country by call.
            Record rec = countriesDB.LookupByCall(callText);
            // Leave everything as is if not found.
            if (rec == null)
            {
                Tracing.TraceLine("setCountryItems:call not found", TraceLevel.Info);
                return;
            }

            // If country field is empty, use the call's values.
            if (countryFld.Item == "")
            {
                countryFld.Item = rec.Country;
            }
            else
            {
#if UseAlternateCountry
                // If country not valid for this call, try to use it.
                if (countryFld.Item != rec.Country)
                {
                    Tracing.TraceLine("setCountryItems:alternate country", TraceLevel.Info);
                    Record rec2 = countriesDB.LookupByName(countryFld.Item);
                    if (rec2 != null)
                    {
                        rec = rec2;
                        countryFld.Item = rec.Country;
                    }
                    else
                    {
                        countryFld.Item = rec.Country;
                    }
                }
#endif
            }

            // If the country is valid,
            if (rec != null)
            {
                // setup the zone boxes, etc.
                LogField fld;
                if ((fld = getScreenElement(AdifTags.ADIF_ITUZone)) != null)
                {
                    fld.Item = rec.ITUZone;
                }
                if ((fld = getScreenElement(AdifTags.ADIF_CQZone)) != null)
                {
                    fld.Item = rec.CQZone;
                }
                if ((fld = getScreenElement(AdifTags.iADIF_Latlong)) != null)
                {
                    fld.Item = rec.Latitude + '/' + rec.Longitude;
                }
                if ((fld = getScreenElement(AdifTags.iADIF_Timezone)) != null)
                {
                    fld.Item = rec.TimeZone;
                }
                if ((fld = getScreenElement(AdifTags.ADIF_DXCC)) != null)
                {
                    fld.Item = rec.CountryID;
                }
            }
            // else leave as is.
        }

        /// <summary>
        /// Setup a comboBox
        /// </summary>
        /// <param name="combo">the comboBox</param>
        /// <param name="items">string array of choices</param>
        /// <param name="oldText">is the choice upon invocation.</param>
        /// <returns>index of the selected item in items, or -1.</returns>
        /// <remarks>
        /// If oldText is empty, the first item in items is selected.
        /// If oldText is found in items, that item is selected.
        /// Otherwise, the choices are set, but the text is left unchanged.
        /// </remarks>
        private int comboSetup(ComboBox combo, string[] items, string oldText)
        {
            bool wasIgnored = IgnoreTextChange;
            IgnoreTextChange = true;
            combo.Items.Clear();
            combo.Items.AddRange(items);
            int id;
            // if box was empty, use the first one in the list.
            if (oldText == "") combo.SelectedIndex = id = 0;
            // else if box was in the list, use that one,
            else
            {
                if ((id = Array.IndexOf(items, oldText)) >= 0)
                    combo.SelectedIndex = id;
                // else leave as is.
                else
                {
                    combo.Text = oldText;
                    // id = -1;
                }
            }
            IgnoreTextChange = wasIgnored;
            return id;
        }

        // get formats "s" "s,s,s" "s-s"
        private string[] getZonesList(string txt)
        {
            if ((txt == null) || (txt == "")) return null;
            Collection<string> items = new Collection<string>();
            string[] ar = txt.Split(new string[] { "," }, 
                StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in ar)
            {
                int id = s.IndexOf('-');
                if (id >= 0)
                {
                    try
                    {
                        int n1 = System.Int32.Parse(s.Substring(0, id));
                        int n2 = System.Int32.Parse(s.Substring(id + 1));
                        if (n1 >= n2)
                        {
                            throw new Exception("SNAFU");
                        }
                        for (int i = n1; i <= n2; i++)
                        {
                            items.Add(i.ToString("d2"));
                        }
                    }
                    catch
                    {
                        // just add the string
                        items.Add(s);
                    }
                }
                else
                {
                    items.Add(s);
                }
            }
            return items.ToArray();
        }

        private void leaveHandler(object sender, EventArgs e)
        {
            // Get the ADIF tag.
            LogField field = getField(sender);
            if (field != null)
            {
                switch (field.ADIFTag)
                {
                    case AdifTags.ADIF_Call: callSignLeave(field, e); break;
                    case AdifTags.ADIF_Country: countryLeave(field, e); break;
                }
            }
        }

        protected virtual void callSignLeave(LogField field, EventArgs e)
        {
            if (Searching) return;
            Tracing.TraceLine("callsign_leave:" + field.Item, TraceLevel.Info);

            // call lookup or country change if field changed.
            if (field.originalContent != field.Item)
            {
                // Remove any whitespace.  Maybe put there by speech recognition.
                string cal = "";
                foreach(char c in field.Item)
                {
                    if (c != ' ') cal += c;
                }
                field.Item = cal;

                if (LookingUp)
                {
                    cal = field.Item.ToUpper();
                    // look for portable or mobile.
                    int id = cal.IndexOf('/');
                    if (id == cal.Length-2)
                    {
                        // xxx/[0-9m]
                        cal = cal.Substring(0, id);
                        id = -1; // cal is ok now.
                    }

                    if (id == -1) Logs.lookup.LookupCall(cal);
                    else callLookupDoneHandler(null);
                }
                else
                {
                    // Treat as if search failed.  Try other lookups.
                    callLookupDoneHandler(null);
                }
            }
        }

        protected virtual void countryLeave(LogField field, EventArgs e)
        {
            if (Searching) return;
            Tracing.TraceLine("countryLeave:" + field.Item, TraceLevel.Info);
            setCountryItems();
        }

#if zero
        internal bool IsGrid(string grid)
        {
            int len = grid.Length;
            if ((len != 4) & (len != 6)) return false;
            Regex exp = new Regex((len == 4) ? "^[a-z][a-z][0-9][0-9]"
                : "^[a-z][a-z][0-9][0-9][a-z][a-z]", RegexOptions.IgnoreCase);
            return exp.IsMatch(grid);
        }
#endif

        protected virtual void textChangedHandler(object sender, EventArgs e)
        {
            if (IgnoreTextChange) return;
            LogField field = getField(sender);
            if ((field != null) && field.IsLogged)
            {
                NeedsWrite = true;                
            }
        }

        protected virtual void keyDownHandler(object sender, KeyEventArgs e)
        {
#if zero
            LogField field = getField(sender);
            if (field != null)
            {
            }
#endif
        }

        /// <summary>
        /// Check an RST.
        /// </summary>
        /// <param name="rst">string</param>
        /// <param name="cw">true if 3-digit CW RST.</param>
        /// <returns>true if good</returns>
        internal bool checkRST(string rst, bool cw)
        {
            bool rv;
            string checkString = null;
            if (cw)
            {
                if (rst.Length == 3) checkString = rst;
            }
            else
            {
                if (rst.Length >= 2) checkString = rst.Substring(0, 2);
            }
            rv = (checkString != null);
            if (rv)
            {
                rv = ((checkString[0] >= '1') && (checkString[0] <= '5') && (checkString[1] >= '1') && (checkString[1] <= '9')) ? true : false;
                if (rv && cw) rv = ((checkString[2] >= '1') && (checkString[2] <= '9')) ? true : false;
            }
            return rv;
        }

        /// <summary>
        /// Produce some beeps.
        /// </summary>
        /// <param name="numbeeps">number of beeps</param>
        internal void Beeps(int numbeeps)
        {
            Console.Beep(880, 250);
            while (--numbeeps > 0)
            {
                Thread.Sleep(100);
                Console.Beep(880, 250);
            }
        }

        private void callLookupDoneHandler(CallbookLookup.HamQTH e)
        {
            if ((theLog == null) || (theLog.TheForm == null)) return;

            if (e != null)
            {
                MsgLib.TextOut.PerformGenericFunction(theLog.TheForm, () =>
                    {
                        setScreenTextIfNotSet(AdifTags.ADIF_QTH, e.search.qth);
                        setScreenTextIfNotSet(AdifTags.ADIF_State, e.search.State);
                        setScreenTextIfNotSet(AdifTags.ADIF_Name, e.search.nick);
                        setScreenTextIfNotSet(AdifTags.ADIF_Grid, e.search.grid);
                        //setScreenTextIfNotSet(AdifTags.ADIF_Country, e.search.country);
                        //setScreenTextIfNotSet(AdifTags.ADIF_ITUZone, e.search.itu);
                        //setScreenTextIfNotSet(AdifTags.ADIF_CQZone, e.search.cq);
                        //setScreenTextIfNotSet(AdifTags.iADIF_Latlong, e.search.LatLong);
                        //setScreenTextIfNotSet(AdifTags.iADIF_Timezone, e.search.utc_offset);
                    });
            }

            MsgLib.TextOut.PerformGenericFunction(theLog.TheForm, () =>
            {
                // Do country lookup.
                setScreenText(AdifTags.ADIF_Country, ""); // so SetCountryItems will work.
                setCountryItems();
            });

            if (theLog.DelayedSKCCLookup)
            {
                // Ignore if an on-file record.
                if (theLog.onFile) return;

                MsgLib.TextOut.PerformGenericFunction(theLog.TheForm, () =>
                    {
                        string text = null;
                        getScreenText(AdifTags.ADIF_Call, out text);
                        if (string.IsNullOrEmpty(text)) return;
                        SKCCChainElement rv = theLog.SKCCDB.Lookup(text, SKCCType.Lookups.byCall);
                        if (rv != null)
                        {
                            setScreenTextIfNotSet(AdifTags.ADIF_State, rv.Item.SPC);
                            setScreenTextIfNotSet(AdifTags.ADIF_Name, rv.Item.Name);
                            setScreenTextIfNotSet(AdifTags.ADIF_Comment, "skcc " + rv.Item.NumberAndLevel);
                            theLog.Beeps(2);
                        }
                    });
            }
        }

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
                _LookingUp = (value & Logs.lookup.CanLookup);
            }
        }
    }
}
