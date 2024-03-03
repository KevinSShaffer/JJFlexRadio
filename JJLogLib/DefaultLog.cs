using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using adif;
using JJCountriesDB;
using JJTrace;
using SKCC;

namespace JJLogLib
{
    public partial class DefaultLog : UserControl
    {
        private const string recordVersion = "3";
        private Logs.LogElement logElement;

        public DefaultLog(Logs.LogElement le)
        {
            InitializeComponent();

            Tracing.TraceLine("DefaultLog:" + le.Name, System.Diagnostics.TraceLevel.Info);
            logElement = le;
            // Set the record version.
            le.RecordVersion = recordVersion;
            // Setup the field dictionary.
            // These must be in the order of occurrence in the log.
            le.addField(new LogField(AdifTags.iADIF_RecordVersion, null, LogField.fieldAttributes.log,
                null, getVersion, null));
            le.addField(new LogField(AdifTags.ADIF_DateOn, QSODate, "LogDateTime"));
            le.addField(new LogField(AdifTags.ADIF_TimeOn, QSOTime));
            le.addField(new LogField(AdifTags.ADIF_Mode, Mode, "LogMode"));
            le.addField(new LogField(AdifTags.ADIF_RXFreq, RX));
            le.addField(new LogField(AdifTags.ADIF_TXFreq, TX));
            le.addField(new LogField(AdifTags.ADIF_Call, CallSign, "LogCall"));
            le.addField(new LogField(AdifTags.ADIF_HisRST, HisRST, "LogHisRST"));
            le.addField(new LogField(AdifTags.ADIF_MyRST, MyRST, "LogMyRST"));
            le.addField(new LogField(AdifTags.ADIF_QTH, QTH, "LogQTH"));
            le.addField(new LogField(AdifTags.ADIF_State, State, "LogState"));
            //le.addField(new LogField(AdifTags.ADIF_Country, CountryBox,
            //    LogField.fieldAttributes.log | LogField.fieldAttributes.display,
            //    null, null, le.countrySetup));
            le.addField(new LogField(AdifTags.ADIF_Country, CountryBox));
            le.addField(new LogField(AdifTags.ADIF_ITUZone, ITUBox));
            le.addField(new LogField(AdifTags.ADIF_CQZone, CQBox));
            le.addField(new LogField(AdifTags.ADIF_Name, Handl, "LogHandle"));
            le.addField(new LogField(AdifTags.ADIF_Rig, Rig, "LogRig"));
            le.addField(new LogField(AdifTags.ADIF_Antenna, Ant, "LogAnt"));
            le.addField(new LogField(AdifTags.ADIF_Comment, Comments, "LogComments"));
            le.addField(new LogField(AdifTags.ADIF_DateOff, DateOut));
            le.addField(new LogField(AdifTags.ADIF_TimeOff, TimeOut));
            le.addField(new LogField(AdifTags.iADIF_Latlong, LatLongBox, LogField.fieldAttributes.display));
            le.addField(new LogField(AdifTags.iADIF_Timezone, TimeZoneBox, LogField.fieldAttributes.display));
            le.addField(new LogField(AdifTags.ADIF_SentSerial, SerialBox));
            le.addField(new LogField(AdifTags.ADIF_ReceivedSerial, null, LogField.fieldAttributes.log));
            le.addField(new LogField(AdifTags.ADIF_Band, BandBox));
            le.addField(new LogField(AdifTags.iADIF_DupCount, DupBox, LogField.fieldAttributes.display));
            // Present in the log only.
            le.addField(new LogField(AdifTags.ADIF_SKCC, SKCCBox, LogField.fieldAttributes.log));
            le.addField(new LogField(AdifTags.ADIF_Fists, FistsBox, LogField.fieldAttributes.log));
            le.addField(new LogField(AdifTags.ADIF_FistsCC, FistsCCBox, LogField.fieldAttributes.log));

            le.addField(new LogField(AdifTags.ADIF_Grid, GridBox, "LogGrid"));
            // These were reserved4 and 5.
            le.addField(new LogField(AdifTags.ADIF_QSL_RCVD, QSLRcvdBox));
            le.addField(new LogField(AdifTags.ADIF_QSL_SENT, QSLSentBox));

            le.addField(new LogField(AdifTags.ADIF_DXCC, DXCCBox));

            if (Logs.persist == null)
            {
                List<LogStats.RowIDs> parm2 = new List<LogStats.RowIDs>();
                // Lookup and show the country's name.
                parm2.Add(new LogStats.RowIDs(AdifTags.ADIF_DXCC, "Countries",
                    (Dictionary<string, LogFieldElement> el) =>
                    {
                        string dxcc = el[AdifTags.ADIF_DXCC].Data;
                        Record rec = le.countriesDB.LookupByDXCC(dxcc);
                        return (rec != null) ? rec.Country : "";
                    },
                    (Dictionary<string, LogFieldElement> el) => { return true; }));
                // Just show the state as is.
                parm2.Add(new LogStats.RowIDs(AdifTags.ADIF_State, "States",
                    (Dictionary<string,LogFieldElement> el) =>
                    {
                        return el[AdifTags.ADIF_State].Data;
                    },
                    (Dictionary<string,LogFieldElement> el) =>
                    {
                        string dxcc = el[AdifTags.ADIF_DXCC].Data;
                        return ((dxcc == "110") | (dxcc == "6") | (dxcc == "291")) ? true : false;
                    }));
                // Just show the Canadian province as is.
                parm2.Add(new LogStats.RowIDs(AdifTags.ADIF_State, "Canadian provinces",
                    (Dictionary<string, LogFieldElement> el) =>
                    {
                        return el[AdifTags.ADIF_State].Data;
                    },
                    (Dictionary<string, LogFieldElement> el) =>
                    {
                        string dxcc = el[AdifTags.ADIF_DXCC].Data;
                        return (dxcc == "1") ? true : false;
                    }));
                Logs.persist = new LogStats(le, parm2);
            }

            if (le.Name == Logs.DefaultLogname4skcc)
            {
                logElement.SKCCDB = new SKCCType(Logs.ConfigDirectory + "\\" + Logs.SKCCConfigFile);
            }

            le.WriteEntry = writeEntry;
            le.RecordConverter = null;
        }

        private string getVersion(LogField fld, string txt)
        {
            return recordVersion;
        }

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
            LogFieldElement fld = null;
            string callSign = null;
            string oldCallSign = null;
            if ((fields == null) || !fields.TryGetValue(AdifTags.ADIF_Call, out fld) ||
                string.IsNullOrEmpty(callSign = fld.Data))
            {
                Tracing.TraceLine("writeEntry:null record or call", TraceLevel.Error);
                return;
            }
            Tracing.TraceLine("writeEntry:" + callSign, TraceLevel.Info);
            if ((oldFields != null) && oldFields.TryGetValue(AdifTags.ADIF_Call, out fld))
            {
                oldCallSign = fld.Data;
            }
            if (oldCallSign == callSign)
            {
                // Call didn't change, quit.
                Tracing.TraceLine("writeEntry:calls are equal", TraceLevel.Info);
                return;
            }

            // Remove if call changed.
            if (!string.IsNullOrEmpty(oldCallSign)) ((LogStats)Logs.persist).StatRemove(oldFields);
            ((LogStats)Logs.persist).StatAdd(fields);
        }
    }
}
