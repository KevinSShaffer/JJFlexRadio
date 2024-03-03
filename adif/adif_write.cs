using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using JJTrace;

namespace adif
{
    public partial class AdifSession
    {
        /// <summary>
        /// Write an ADIF record.
        /// </summary>
        /// <param name="record">Dictionary of LogFieldElements</param>
        /// <param name="initialText">set to text written prior to the ADIF data.</param>
        public void Write(Dictionary<string, LogFieldElement> record,
            string initialText)
        {
            try
            {
                Tracing.TraceLine("adif write:" + fileName, TraceLevel.Info);
                // First write any initial text.
                if (initialText != null) sw.Write(initialText);
                LogFieldElement endItem = null;
                foreach (LogFieldElement el in record.Values)
                {
                    if (el.ADIFTag[0] == '$') continue; // no metatags.
                    if ((el.ADIFTag == AdifTags.ADIF_HeaderEnd) | (el.ADIFTag == AdifTags.ADIF_RecordEnd))
                    {
                        endItem = el; // Save until the end.
                    }
                    else
                    {
                        ADIFOut(el, true);
                    }
                }
                if (endItem != null) ADIFOut(endItem, false);
                sw.Write(Environment.NewLine);
            }
            catch (Exception ex) { throw ex; }
        }

        private void ADIFOut(LogFieldElement fld, bool addBlank)
        {
            int len = (fld.Data == null) ? 0 : fld.Data.Length;
            AdifTags.ADIFTypeField typeFld;
            AdifTags.ADIFTypeDictionary.TryGetValue(fld.ADIFTag, out typeFld);
            string str;
            if (len == 0) str = beginTagChar + fld.ADIFTag + endTagChar;
            else if (typeFld == null)
                str = beginTagChar + fld.ADIFTag + tagSepChar + len.ToString() + endTagChar + fld.Data;
            else if (typeFld.type == "i")
                str = beginTagChar + fld.ADIFTag + tagSepChar + len.ToString() +
                    endTagChar + typeFld.InternalToADIF(fld.Data);
            else
                str = beginTagChar + fld.ADIFTag + tagSepChar + len.ToString() +
                    tagSepChar + typeFld.type + endTagChar + fld.Data;
            sw.Write((addBlank) ? str + " " : str);
        }
    }
}
