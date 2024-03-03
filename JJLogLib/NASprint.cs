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

namespace JJLogLib
{
    public partial class NASprint : UserControl
    {
        private Logs.LogElement logElement;

        public NASprint(Logs.LogElement le)
        {
            InitializeComponent();

            logElement = le;
            // Setup the field dictionary.
            // These must be in the order of occurrence in the log.
            le.addField(new LogField(AdifTags.ADIF_DateOn, DateBox));
            le.addField(new LogField(AdifTags.ADIF_TimeOn, TimeBox));
            le.addField(new LogField(AdifTags.ADIF_Call, CallBox, "LogCall"));
            le.addField(new LogField(AdifTags.ADIF_ReceivedSerial, HisSerialBox));
            le.addField(new LogField(AdifTags.ADIF_State, StateBox));
            le.addField(new LogField(AdifTags.ADIF_Name, NameBox));
            le.addField(new LogField(AdifTags.ADIF_RXFreq, FreqBox));
            le.addField(new LogField(AdifTags.ADIF_Mode, null, LogField.fieldAttributes.log, null, CWMode, null));
            le.addField(new LogField(AdifTags.ADIF_Band, BandBox));
            le.addField(new LogField(AdifTags.ADIF_SentSerial, QSOBox));

            le.WriteEntry = writeEntry;
        }

        private string CWMode(LogField fld, string text)
        {
            return "CW";
        }

        /// <summary>
        /// convert state to uppercase.
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
            if (string.IsNullOrEmpty(fields[AdifTags.ADIF_State].Data)) return;

            fields[AdifTags.ADIF_State].Data = fields[AdifTags.ADIF_State].Data.ToUpper();
        }
    }
}
