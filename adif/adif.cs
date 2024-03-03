using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using JJTrace;

namespace adif
{
    /// <summary>
    /// An ADIF session.
    /// </summary>
    public partial class AdifSession
    {
        internal enum states
        {
            initial,
            findTagEnd,
            inTag,
            endOfRecord,
            EOF,
        }

        internal string fileName;
        internal FileStream fs;
        internal StreamReader sr;
        internal StreamWriter sw;
        internal bool writeing { get { return (sw != null); } }
        internal states state;
        internal bool isOpen;

        internal const char beginTagChar = '<';
        internal const char endTagChar = '>';
        internal const char tagSepChar = ':';

        private void setupSession(string name, bool writeFlag, bool appendFlag)
        {
            Tracing.TraceLine("adif open:" + name + " " + writeFlag.ToString() + " " + appendFlag.ToString(), TraceLevel.Info);
            FileMode mode = (writeFlag) ?
                    ((appendFlag) ? FileMode.Append : FileMode.Create) :
                FileMode.Open;
            FileAccess access = (writeFlag) ?
                FileAccess.Write : FileAccess.Read;
            fileName = name;
            try
            {
                fs = new FileStream(name, mode, access, FileShare.None);
                if (writeFlag)
                {
                    sw = new StreamWriter(fs);
                }
                else
                {
                    sr = new StreamReader(fs);
                }
                isOpen = true;
            }
            catch (IOException ex)
            {
                if (fs != null)
                {
                    try { fs.Dispose(); }
                    catch { }
                }
                Tracing.TraceLine("adif open throwing exception " + ex.Message, TraceLevel.Error);
                throw ex; 
            }
        }

        /// <summary>
        /// Open an ADIF session
        /// </summary>
        /// <param name="name">of the file</param>
        /// <param name="writeFlag">true if open for write</param>
        /// <param name="appendFlag">true if open for appending</param>
        public AdifSession(string name, bool writeFlag, bool appendFlag)
        {
            setupSession(name, writeFlag, appendFlag);
        }

        /// <summary>
        /// Open an ADIF session
        /// </summary>
        /// <param name="name">of the file</param>
        /// <param name="writeFlag">true if open for write</param>
        public AdifSession(string name, bool writeFlag)
        {
            setupSession(name, writeFlag, false);
        }

        /// <summary>
        /// Open an ADIF session.  No flags means open for reading.
        /// </summary>
        /// <param name="name">of the file</param>
        public AdifSession(string name)
        {
            setupSession(name, false, false);
        }

        /// <summary>
        /// Close an adif session.
        /// </summary>
        public void Close()
        {
            if (!isOpen) return;
            try
            {
                if (writeing) sw.Dispose();
                else sr.Dispose();
                fs.Dispose();
                isOpen = false;
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        public bool IsHeader(Dictionary<string, LogFieldElement> record)
        {
            return record.Keys.Contains(AdifTags.ADIF_HeaderEnd);
        }

        public bool IsRecord(Dictionary<string, LogFieldElement> record)
        {
            return record.Keys.Contains(AdifTags.ADIF_RecordEnd);
        }
    }
}
