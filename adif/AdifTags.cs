using System.Collections.Generic;

namespace adif
{
    public class AdifTags
    {
        // ADIF field names.
        public const string iADIF_RecordVersion = "$VERSION"; // pseudo tag
        public const string iADIF_Latlong = "$LATLON"; // pseudo tag
        public const string iADIF_Timezone = "$TIMEZONE"; // pseudo tag
        public const string iADIF_DupCount = "$DUPCOUNT";
        public const string iADIF_InitialText = "$IText"; // Text prior to the first field.
        public const string iADIF_Reserved0 = "$RSVD0";
        public const string iADIF_Reserved1 = "$RSVD1";
        public const string iADIF_Reserved2 = "$RSVD2";
        public const string iADIF_Reserved3 = "$RSVD3";
        public const string iADIF_Reserved4 = "$RSVD4";
        public const string iADIF_Reserved5 = "$RSVD5";
        public const string iADIF_Reserved6 = "$RSVD6";
        public const string iADIF_Reserved7 = "$RSVD7";

        public const string ADIF_HeaderEnd = "EOH";
        public const string ADIF_RecordEnd = "EOR";

        // Header tags.
        public const string HDR_ADIFVersion = "ADIF_VER";
        public const string HDR_ProgramID = "PROGRAMID";
        public const string HDR_ProgramVersion = "PROGRAMVERSION";
        public const string HDR_LogHeaderVersion = "APP_JJRADIO_LOGHEADERVERSION";
        public const string HDR_StartingSerial = "APP_JJRADIO_HDRSTX";
        public const string HDR_DupCheck = "APP_JJRADIO_DUPCHK";
        public const string HDR_FormNAME = "APP_JJRADIO_FORMNAME";
        public const string HDR_CallLookup = "APP_JJRADIO_CallLookup";
        public const string Current_ADIF_Version = "3.0.3";

        // QSO tags.
        public const string ADIF_Call = "CALL";
        public const string ADIF_DateOn = "QSO_DATE";
        public const string ADIF_TimeOn = "TIME_ON";
        public const string ADIF_Mode = "MODE";
        public const string ADIF_RXFreq = "FREQ";
        public const string ADIF_TXFreq = "TXFREQ";
        public const string ADIF_HisRST = "RST_SENT";
        public const string ADIF_MyRST = "RST_RCVD";
        public const string ADIF_QTH = "QTH";
        public const string ADIF_State = "STATE";
        public const string ADIF_Country = "COUNTRY";
        public const string ADIF_CQZone = "CQZ";
        public const string ADIF_ITUZone = "ITUZ";
        public const string ADIF_Name = "NAME";
        public const string ADIF_Rig = "RIG";
        public const string ADIF_Antenna = "ANTENNA";
        public const string ADIF_Comment = "COMMENT";
        public const string ADIF_DateOff = "DATE_OFF";
        public const string ADIF_TimeOff = "TIME_OFF";
        public const string ADIF_SentSerial = "STX";
        public const string ADIF_SentSerialString = "STX_STRING";
        public const string ADIF_ReceivedSerial = "SRX";
        public const string ADIF_ReceivedSerialString = "SRX_STRING";
        public const string ADIF_SKCC = "SKCC";
        public const string ADIF_Band = "BAND";
        public const string ADIF_ContestID = "CONTEST_ID";
        public const string ADIF_ARRLSect = "ARRL_SECT";
        public const string ADIF_Class = "CLASS";
        public const string ADIF_Fists = "FISTS";
        public const string ADIF_FistsCC = "FISTS_CC";
        public const string ADIF_Grid = "GRIDSQUARE";
        public const string ADIF_EQSL_QSL_RCVD = "EQSL_QSL_RCVD";
        public const string ADIF_EQSL_QSL_SENT = "EQSL_QSL_SENT";
        public const string ADIF_LOTW_QSL_RCVD = "LOTW_QSL_RCVD";
        public const string ADIF_LOTW_QSL_SENT = "LOTW_QSL_SENT";
        public const string ADIF_QSL_RCVD = "QSL_RCVD";
        public const string ADIF_QSL_SENT = "QSL_SENT";
        public const string ADIF_DXCC = "DXCC";

        public const string ADIFTypeInternal = "$";
        public delegate string ADIFToInternalDel(string str);
        public delegate string InternalToADIFDel(string str);

        /// <summary>
        /// Type of a value for the ADIFTypeDictionary.
        /// </summary>
        public class ADIFTypeField
        {
            public string tag;
            public string type;
            public ADIFToInternalDel ADIFToInternal;
            public InternalToADIFDel InternalToADIF;
            internal ADIFTypeField(string t, string c)
            {
                tag = t;
                type = c;
            }

            internal ADIFTypeField(string t, string c,
                ADIFToInternalDel atoi, InternalToADIFDel itoa)
            {
                tag = t;
                type = c;
                ADIFToInternal = atoi;
                InternalToADIF = itoa;
            }
        }

        /// <summary>
        /// Dictionary of ADIF field types.
        /// </summary>
        public static Dictionary<string, ADIFTypeField> ADIFTypeDictionary = new Dictionary<string, ADIFTypeField>
        {
            { HDR_LogHeaderVersion, new ADIFTypeField(HDR_LogHeaderVersion, "S") },
            { HDR_StartingSerial, new ADIFTypeField(HDR_StartingSerial, "N") },
            { HDR_DupCheck, new ADIFTypeField(HDR_DupCheck, "N") },
            { HDR_FormNAME, new ADIFTypeField(HDR_FormNAME, "S") },
            { HDR_CallLookup, new ADIFTypeField(HDR_CallLookup, "N") },
            { ADIF_DateOn, new ADIFTypeField(ADIF_DateOn, "D") },
            { ADIF_TimeOn, new ADIFTypeField(ADIF_TimeOn, "T") },
            { ADIF_DateOff, new ADIFTypeField(ADIF_DateOff, "D") },
            { ADIF_TimeOff, new ADIFTypeField(ADIF_TimeOff, "T") },
            { ADIF_RXFreq, new ADIFTypeField(ADIF_RXFreq, "N") },
            { ADIF_TXFreq, new ADIFTypeField(ADIF_TXFreq, "N") },
            { ADIF_CQZone, new ADIFTypeField(ADIF_CQZone, "N") },
            { ADIF_ITUZone, new ADIFTypeField(ADIF_ITUZone, "N") },
            { ADIF_ReceivedSerial, new ADIFTypeField(ADIF_ReceivedSerial, "N") },
            { ADIF_SentSerial, new ADIFTypeField(ADIF_SentSerial, "N") },
            // These tags require data reformatting.
            { ADIF_Call, new ADIFTypeField(ADIF_Call, ADIFTypeInternal, toUpper_atoi, toUpper_itoa) },
            { ADIF_Mode, new ADIFTypeField(ADIF_Mode, ADIFTypeInternal, toUpper_atoi, toUpper_itoa) },
            { ADIF_SKCC, new ADIFTypeField(ADIF_SKCC, ADIFTypeInternal, toUpper_atoi, toUpper_itoa) },
            { ADIF_Band, new ADIFTypeField(ADIF_Band, ADIFTypeInternal, toUpper_atoi, toUpper_itoa) },
            { ADIF_EQSL_QSL_RCVD, new ADIFTypeField(ADIF_EQSL_QSL_RCVD, ADIFTypeInternal, QSL_Rcvd_atoi, QSL_Rcvd_itoa) },
            { ADIF_ARRLSect, new ADIFTypeField(ADIF_ARRLSect, ADIFTypeInternal, toUpper_atoi, toUpper_itoa) },
            { ADIF_Class, new ADIFTypeField(ADIF_Class, ADIFTypeInternal, toUpper_atoi, toUpper_itoa) },
            { ADIF_Grid, new ADIFTypeField(ADIF_Grid, ADIFTypeInternal, toUpper_atoi, toUpper_itoa) },
            { ADIF_EQSL_QSL_SENT, new ADIFTypeField(ADIF_EQSL_QSL_SENT, ADIFTypeInternal, QSL_sent_atoi, QSL_sent_itoa) },
            { ADIF_LOTW_QSL_RCVD, new ADIFTypeField(ADIF_LOTW_QSL_RCVD, ADIFTypeInternal, QSL_Rcvd_atoi, QSL_Rcvd_itoa) },
            { ADIF_LOTW_QSL_SENT, new ADIFTypeField(ADIF_LOTW_QSL_SENT, ADIFTypeInternal, QSL_sent_atoi, QSL_sent_itoa) },
            { ADIF_QSL_RCVD, new ADIFTypeField(ADIF_QSL_RCVD, ADIFTypeInternal, QSL_Rcvd_atoi, QSL_Rcvd_itoa) },
            { ADIF_QSL_SENT, new ADIFTypeField(ADIF_QSL_SENT, ADIFTypeInternal, QSL_sent_atoi, QSL_sent_itoa) },
        };

        private static string toUpper_atoi(string call)
        {
            // Use case from the ADIF input.
            return call;
        }
        private static string toUpper_itoa(string call)
        {
            return call.ToUpper();
        }

        private static string QSL_Rcvd_atoi(string adif)
        {
            string rv = "Invalid";
            if (!string.IsNullOrEmpty(adif))
            {
                switch (adif)
                {
                    case "Y": rv = "Yes"; break;
                    case "N": rv = "No"; break;
                    case "R": rv = "Requested"; break;
                    case "V": rv = "Verified"; break;
                }
            }
            return rv;
        }
        private static string QSL_Rcvd_itoa(string intern)
        {
            string rv = "I";
            if (!string.IsNullOrEmpty(intern))
            {
                switch (intern.ToUpper()[0])
                {
                    case 'Y': rv = "Y"; break;
                    case 'N': rv = "N"; break;
                    case 'R': rv = "R"; break;
                    case 'V': rv = "V"; break;
                }
            }
            return rv;
        }

        private static string QSL_sent_atoi(string adif)
        {
            string rv = "Invalid";
            if (!string.IsNullOrEmpty(adif))
            {
                switch (adif)
                {
                    case "Y": rv = "Yes"; break;
                    case "N": rv = "No"; break;
                    case "R": rv = "Requested"; break;
                    case "Q": rv = "Queued"; break;
                }
            }
            return rv;
        }
        private static string QSL_sent_itoa(string intern)
        {
            string rv = "I";
            if (!string.IsNullOrEmpty(intern))
            {
                switch (intern.ToUpper()[0])
                {
                    case 'Y': rv = "Y"; break;
                    case 'N': rv = "N"; break;
                    case 'R': rv = "R"; break;
                    case 'Q': rv = "Q"; break;
                }
            }
            return rv;
        }
    }
}
