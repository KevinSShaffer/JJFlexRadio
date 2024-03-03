using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using JJTrace;

namespace adif
{
    public partial class AdifSession
    {
        /// <summary>
        /// Corrupt file exception
        /// </summary>
        public class CorruptFileException : Exception
        {
            internal CorruptFileException(string name, string msg) :
                base("Corrupt ADIF file " + name + ".\r\n" + msg)
            {
            }
        }

        /// <summary>
        /// Read an ADIF record.
        /// Should be in a try catch block.
        /// Returned dictionary count of 0 means end-of-file.
        /// </summary>
        /// <param name="getInitial">true to add the IADIF_InitialText item</param>
        /// <returns>dictionary with the fields.</returns>
        public Dictionary<string, LogFieldElement> Read(bool getInitial)
        {
            Dictionary<string, LogFieldElement> rv = new Dictionary<string, LogFieldElement>();
            try
            {
                Tracing.TraceLine("adif read:" + fileName, TraceLevel.Info);
                bool initialField = true;
                state = states.initial;
                string tag = null;

                while ((state != states.endOfRecord) &
                       (state != states.EOF))
                {
                    switch (state)
                    {
                        case states.initial:
                            // find start of tag.
                            tag = null;
                            // We may record any text prior to the initial field.
                            // Also, end-of-file is allowed here.
                            if (initialField)
                            {
                                initialField = false;
                                string iText = readUntil(beginTagChar, true);
                                if (getInitial &&
                                    ((iText != null) && (iText != "") & (iText != Environment.NewLine)))
                                {
                                    rv.Add(AdifTags.iADIF_InitialText,
                                        new LogFieldElement(AdifTags.iADIF_InitialText, iText));
                                }
                            }
                            else
                            {
                                readUntil(beginTagChar, true);
                                // Special case of <APP_> tags at end-of-file.
                                if (state == states.EOF)
                                {
                                    foreach (string apptag in rv.Keys)
                                    {
                                        if ((apptag.Length >= 4) && (apptag.Substring(0, 4) != "APP_"))
                                        {
                                            throw new CorruptFileException(fileName, "Premature end of file in Read.");
                                        }
                                    }
                                    return new Dictionary<string, LogFieldElement>();
                                }
                            }
                            // State could have changed.
                            if (state == states.initial) state = states.findTagEnd;
                            break;
                        case states.findTagEnd:
                            tag = readUntil(endTagChar, false);
                            state = states.inTag;
                            break;
                        case states.inTag:
                            // Processing this tag.
                            LogFieldElement fld = doTag(tag);
                            if (fld != null)
                            {
                                rv.Add(fld.ADIFTag, fld);
                                if ((fld.ADIFTag == AdifTags.ADIF_HeaderEnd) | (fld.ADIFTag == AdifTags.ADIF_RecordEnd))
                                {
                                    state = states.endOfRecord;
                                    break;
                                }
                            }
                            state = states.initial;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.TraceLine("adif read exception:" + ex.Message, TraceLevel.Error);
                throw ex;
            }
            return rv;
        }

        private LogFieldElement doTag(string tag)
        {
            // tag should have the form ADIFTag:Length[:type].
            // The ADIFTag can be mixed case.
            // The file must be positioned immediately following the tagEndChar.
            if (string.IsNullOrEmpty(tag)) return null;
            // Get length of the tag identifier.
            int tagLen = tag.IndexOf(tagSepChar);
            // Check for tag with no data, (e.g.) <EOR>.
            if (tagLen == -1)
            {
                return new LogFieldElement(tag.ToUpper());
            }
            // Check for something like <tag:>.
            if (tagLen == (tag.Length - 1)) return null;
            // See if there's a type.
            int tagTextLen = tag.Substring(tagLen + 1).IndexOf(tagSepChar);
            string str = (tagTextLen == -1) ?
                tag.Substring(tagLen + 1) :
                tag.Substring(tagLen + 1, tagTextLen);
            if (!System.Int32.TryParse(str, out tagTextLen)) return null;
            LogFieldElement rv = new LogFieldElement(tag.Substring(0, tagLen).ToUpper());
            rv.Data = readLength(tagTextLen);
            // See if need to convert the data.
            AdifTags.ADIFTypeField typeFld = null;
            if (AdifTags.ADIFTypeDictionary.TryGetValue(tag.Substring(0, tagLen), out typeFld) &&
                (typeFld.type == AdifTags.ADIFTypeInternal))
            {
                rv.Data = typeFld.ADIFToInternal(rv.Data);
            }
            return rv;
        }

        private string readUntil(char stopChar, bool allowEOF)
        {
            string rv = "";
            int c;
            while (true)
            {
                c = sr.Read();
                if ((char)c == stopChar) break;
                if (c == -1)
                {
                    if (allowEOF)
                    {
                        state = states.EOF;
                        break;
                    }
                    else
                    {
                        throw new CorruptFileException(fileName, "Premature end of file in readUntil.");
                    }
                }
                rv += (char)c;
            }
            return rv;
        }

        private string readLength(int len)
        {
            char[] rv = new char[len];
            int rlen = sr.Read(rv, 0, len);
            if (rlen < len)
            {
                throw new CorruptFileException(fileName, "Premature end of file in readLength(." + len.ToString() + ')');
            }
            return new string(rv);
        }
    }
}
