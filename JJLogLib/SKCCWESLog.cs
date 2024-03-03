#define PrimeWithSKCC
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using adif;
using JJTrace;
using SKCC;

namespace JJLogLib
{
    public partial class SKCCWESLog : UserControl
    {
        private Logs.LogElement logElement;

        class persistentType
        {
            public Grouping spcGroup;
            public Grouping cGroup, tGroup, sGroup;
            public int QSOs;
            public persistentType()
            {
                spcGroup = new Grouping(Grouping.Types.unique);
                cGroup = new Grouping(Grouping.Types.unique);
                tGroup = new Grouping(Grouping.Types.unique);
                sGroup = new Grouping(Grouping.Types.unique);
                QSOs = 0;
            }
        }

#if PrimeWithSKCC
        private SKCCType skccDB = null;
#endif

        public SKCCWESLog(Logs.LogElement le)
        {
            InitializeComponent();

            logElement = le;
            // Setup the field dictionary.
            // These must be in the order of occurrence in the log.
            le.addField(new LogField(AdifTags.ADIF_DateOn, DateBox));
            le.addField(new LogField(AdifTags.ADIF_TimeOn, TimeBox));
            le.addField(new LogField(AdifTags.ADIF_Call, CallBox, "LogCall"));
            le.addField(new LogField(AdifTags.ADIF_HisRST, HisRSTBox));
            le.addField(new LogField(AdifTags.ADIF_MyRST, MyRSTBox));
            le.addField(new LogField(AdifTags.ADIF_State, SPCBox));
            le.addField(new LogField(AdifTags.ADIF_Name, NameBox));
            le.addField(new LogField(AdifTags.ADIF_SKCC, SKCCBox));
            le.addField(new LogField(AdifTags.ADIF_RXFreq, FreqBox));
            le.addField(new LogField(AdifTags.ADIF_Mode, null, LogField.fieldAttributes.log, null, CWMode, null));
            le.addField(new LogField(AdifTags.ADIF_Band, BandBox));
            le.addField(new LogField(AdifTags.ADIF_SentSerial, QSOBox));
            le.addField(new LogField(AdifTags.ADIF_Comment, CommentsBox, "LogComments"));

            // Setup scoring.
            if (Logs.persist == null) Logs.persist = new persistentType();
            else showScore();

            le.WriteEntry = writeEntry;

#if PrimeWithSKCC
            // Setup the SKCC lookup stuff.
            try
            {
                skccDB = new SKCCType(Logs.ConfigDirectory + "\\" + Logs.SKCCConfigFile);
                CallBox.Leave += callSign_leave;
                logElement.LookingUp = false; // Don't do callbook lookups.
            }
            catch (Exception ex)
            {
                Tracing.TraceLine("SKCCWesLog skccdb error:" + ex.Message, TraceLevel.Error);
                skccDB = null;
            }
#endif
        }

        private string CWMode(LogField fld, string text)
        {
            return "CW";
        }

#if PrimeWithSKCC
        /// <summary>
        /// Lookup the SKCC info for the call.
        /// Fill in fields if they're blank.
        /// </summary>
        private void callSign_leave(object sender, EventArgs e)
        {
            string text = CallBox.Text.Trim();
            if ((skccDB == null) | string.IsNullOrEmpty(text)) return;
            SKCCChainElement rv = skccDB.Lookup(text, SKCCType.Lookups.byCall);
            if (rv != null)
            {
                if (SPCBox.Text == "") SPCBox.Text = rv.Item.SPC;
                if (NameBox.Text == "") NameBox.Text = rv.Item.Name;
                if (SKCCBox.Text == "")
                {
                    SKCCBox.Text = rv.Item.NumberAndLevel;
                }
            }
            else
            {
                // Beep if not SKCC member.
                logElement.Beeps(2);
            }
        }
#endif

        /// <summary>
        /// Keep score and convert SKCC level to uppercase.
        /// </summary>
        /// <param name="fields">field dictionary</param>
        /// <param name="oldFields">old record or null</param>
        /// <remarks>
        /// This is also called for each record when configuring a new log.
        /// </remarks>
        private void writeEntry(
            Dictionary<string, LogFieldElement> fields,
            Dictionary<string, LogFieldElement> oldFields)
        {
            // Currently only handle new items.
            if (oldFields != null) return;

            string spc = fields[AdifTags.ADIF_State].Data;
            LogFieldElement skccFld = fields[AdifTags.ADIF_SKCC];
            if ((spc == "") | (skccFld.Data == "")) return;
            skccFld.Data = skccFld.Data.ToUpper();

            persistentType persist = (persistentType)Logs.persist;
            persist.QSOs++;
            persist.spcGroup.Add(spc);
            switch (skccFld.Data[skccFld.Data.Length - 1])
            {
                case 'C': persist.cGroup.Add(skccFld.Data); break;
                case 'T': persist.tGroup.Add(skccFld.Data); break;
                case 'S': persist.sGroup.Add(skccFld.Data); break;
            }
            showScore();
        }

        private void showScore()
        {
            try
            {
                persistentType persist = (persistentType)Logs.persist;
                SPCsBox.Text = persist.spcGroup.Count.ToString();
                CBox.Text = persist.cGroup.Count.ToString();
                TBox.Text = persist.tGroup.Count.ToString();
                SBox.Text = persist.sGroup.Count.ToString();
                PointsBox.Text = ((persist.QSOs * persist.spcGroup.Count) +
                    (persist.cGroup.Count * 5) +
                    (persist.tGroup.Count * 10) +
                    (persist.sGroup.Count * 15)).ToString();
            }
            catch { }
        }

        private void HisRSTBox_Leave(object sender, EventArgs e)
        {
            if (!logElement.checkRST(HisRSTBox.Text, true)) logElement.Beeps(1);
        }

        private void MyRSTBox_Leave(object sender, EventArgs e)
        {
            if (!logElement.checkRST(MyRSTBox.Text, true)) logElement.Beeps(1);
        }
    }
}
