//#define AlwaysLoad
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using adif;
using JJTrace;

namespace SKCC
{
    /// <summary>
    /// SKCC membership levels.
    /// </summary>
    public enum SKCCLevelType
    {
        none,
        centurion,
        tribune,
        senator
    }

    public class SKCCLoaded
    {
        public bool Loaded;
        public SKCCLoaded()
        {
            Loaded = true;
        }
    }

    /// <summary>
    /// Essential SKCC data.
    /// </summary>
    public class SKCCElement
    {
        private string _Call;
        /// <summary>
        /// Call sign
        /// </summary>
        public string Call
        {
            get { return _Call; }
            set
            {
                if (string.IsNullOrEmpty(value)) _Call = value;
                else _Call = value.Trim().ToUpper();
            }
        }

        /// <summary>
        /// Character to SKCCLevelType mapping.
        /// </summary>
        public static Dictionary<char, SKCCLevelType> LevelMap = new Dictionary<char,SKCCLevelType>()
        {
            {'C', SKCCLevelType.centurion},
            {'T', SKCCLevelType.tribune},
            {'S', SKCCLevelType.senator},
        };
        private SKCCLevelType _Level;
        /// <summary>
        /// (ReadOnly) SKCC Level.
        /// </summary>
        /// <remarks>Set when Number is set.</remarks>
        public SKCCLevelType Level { get { return _Level; } }

        private string _Number;
        /// <summary>
        /// SKCC number.
        /// </summary>
        /// <remarks>Setting this sets Level also.</remarks>
        public string Number
        {
            get { return _Number; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _Number = null;
                    _Level=SKCCLevelType.none;
                }
                else
                {
                    value = value.Trim().ToUpper();
                    int len = value.Length;
                    char c = value[len - 1];
                    if (LevelMap.TryGetValue(c, out _Level))
                    {
                        _Number = (len > 1)? value.Substring(0, len - 1) : null;
                    }
                    else
                    {
                        _Level = SKCCLevelType.none;
                        _Number = value;
                    }
                }
            }
        }
        /// <summary>
        /// (ReadOnly) Printable number plus Level.
        /// </summary>
        public string NumberAndLevel
        {
            get
            {
                if (string.IsNullOrEmpty(_Number)) return "";
                string rv = Number;
                if (Level != SKCCLevelType.none) rv += Level.ToString()[0];
                return rv;
            }
        }
        /// <summary>
        /// (ReadOnly) Key containing Call and Number.
        /// </summary>
        public string Key
        {
            get
            {
                return (string.IsNullOrEmpty(Number) | string.IsNullOrEmpty(Call)) ? null : Call + Number;
            }
        }

        public string Name;
        public string City;
        public string SPC;
        public string OtherCalls;
        public string Date;

        /// <summary>
        /// Essential skcc element.
        /// </summary>
        /// <param name="rec">Master file record</param>
        public SKCCElement(string rec)
        {
            if (!string.IsNullOrEmpty(rec))
            {
                string[] fields = rec.Split(new char[] { '|' });
                int nFields = fields.Length;
                switch (nFields)
                {
                    case 1: break;
                    case 2:
                        Number = fields[0]; // Also sets Level.
                        break;
                    default:
                        Number = fields[0];
                        Call = fields[1];
                        Name = (nFields > 3) ? fields[2] : null;
                        City = (nFields > 4) ? fields[3] : null;
                        SPC = (nFields > 5) ? fields[4] : null;
                        OtherCalls = (nFields > 6) ? fields[5] : null;
                        Date = (nFields >= 7) ? fields[6] : null;
                        break;
                }
            }
        }

        /// <summary>
        /// Essential skcc element.
        /// </summary>
        /// <param name="rec">ADIF dictionary</param>
        public SKCCElement(Dictionary<string, LogFieldElement> rec)
        {
            LogFieldElement el;
            Call = (rec.TryGetValue(AdifTags.ADIF_Call, out el)) ? el.Data : null;
            Number = (rec.TryGetValue(AdifTags.ADIF_SKCC, out el)) ? el.Data : null;
            Name = (rec.TryGetValue(AdifTags.ADIF_Name, out el)) ? el.Data : null;
            City = (rec.TryGetValue(AdifTags.ADIF_QTH, out el)) ? el.Data : null;
            SPC = (rec.TryGetValue(AdifTags.ADIF_State, out el)) ? el.Data : null;
            OtherCalls = Date = null;
        }
    }

    public class SKCCChainElement
    {
        public SKCCChainElement Next;
        public SKCCElement Item;
        public SKCCChainElement(SKCCElement item)
        {
            Next = null;
            Item = item;
        }
        public SKCCChainElement(string rec)
        {
            Item = new SKCCElement(rec);
            Next = null;
        }
        public SKCCChainElement(Dictionary<string, LogFieldElement> rec)
        {
            Item = new SKCCElement(rec);
            Next = null;
        }
    }

    public class Stats
    {
        public int Records;
        public int[] RecordsPerLevel;
        internal Stats()
        {
            Records = 0;
            RecordsPerLevel = new int[Enum.GetNames(typeof(SKCCLevelType)).Length];
        }

        internal void Add(SKCCLevelType level)
        {
            Records++;
            RecordsPerLevel[(int)level]++;
        }
    }

    /// <summary>
    /// SKCC Database.
    /// </summary>
    public class SKCCType
    {
        /// <summary>
        /// Call sign dictionary
        /// </summary>
        private Dictionary<string, SKCCChainElement> CallDict;
        /// <summary>
        /// SKCC numbers dictionary
        /// </summary>
        private Dictionary<string, SKCCChainElement> NumberDict;
        /// <summary>
        /// SKCC level dictionary
        /// </summary>
        private Dictionary<SKCCLevelType, SKCCChainElement> LevelDict;
        /// <summary>#if zero
        /// SKCC key dictionary (call and number)
        /// </summary>
        private Dictionary<string, SKCCChainElement> KeyDict;

#if zero
        // dictionary/web locking
        private Mutex dictionaryWebLock = null;
        private bool useLock { get { return (dictionaryWebLock != null); } }
        private void getLock(bool lockIt)
        {
            Tracing.TraceLine("getLock:" + lockIt.ToString() + ' ' + useLock.ToString(), TraceLevel.Info);
            if (useLock)
            {
                if (lockIt) dictionaryWebLock.WaitOne();
                else dictionaryWebLock.ReleaseMutex();
            }
        }
#endif

        /// <summary>
        /// Start date of entries from an SKCC session.
        /// </summary>
        public string startDate;
        /// <summary>
        /// Start SKCC level of entries from an SKCC session.
        /// </summary>
        public SKCCLevelType startLevel;

        /// <summary>
        /// ADIF records if multiple files are used.
        /// </summary>
        public ArrayList Records;

        /// <summary>
        /// Database statistics
        /// </summary>
        public Stats Statistics = new Stats();

#if AlwaysLoad
        private const int reloadDays = 0;
#else
        private const int reloadDays = 1;
#endif
        private const string skccBase = "http://www.skccgroup.com";
        private const string roster = "/search/skcclist.txt";

        private void createDictionaries()
        {
            CallDict = new Dictionary<string, SKCCChainElement>();
            LevelDict = new Dictionary<SKCCLevelType, SKCCChainElement>();
            NumberDict = new Dictionary<string, SKCCChainElement>();
            KeyDict = new Dictionary<string, SKCCChainElement>();
        }

        private void addToDictionaries(SKCCElement val)
        {
            if (string.IsNullOrEmpty(val.Number) || string.IsNullOrEmpty(val.Call))
            {
                Tracing.TraceLine("addToDictionaries:number and/or call is null", TraceLevel.Error);
                return;
            }
            SKCCChainElement chain = null;
            SKCCChainElement newElement = new SKCCChainElement(val);
            if (CallDict.TryGetValue(val.Call, out chain))
            {
                CallDict.Remove(val.Call);
            }
            newElement.Next = chain;
            CallDict.Add(val.Call, newElement);
            newElement = new SKCCChainElement(val);
            if (NumberDict.TryGetValue(val.Number, out chain))
            {
                NumberDict.Remove(val.Number);
            }
            else
            {
                // Stats only count unique numbers.
                Statistics.Add(val.Level);
            }
            newElement.Next = chain;
            NumberDict.Add(val.Number, newElement);
            newElement = new SKCCChainElement(val);
            if (LevelDict.TryGetValue(val.Level, out chain))
            {
                LevelDict.Remove(val.Level);
            }
            newElement.Next = chain;
            LevelDict.Add(val.Level, newElement);
            string key = val.Key;
            if (key != null)
            {
                newElement = new SKCCChainElement(val);
                if (KeyDict.TryGetValue(key, out chain))
                {
                    KeyDict.Remove(key);
                }
                newElement.Next = chain;
                KeyDict.Add(key, newElement);
            }
        }

        /// <summary>
        /// Initialize database
        /// </summary>
        /// <param name="fileName">Name of file from the web</param>
        public SKCCType(string fileName)
        {
            getDB(fileName);
        }

        private void getDB(string fileName)
        {
            Exception err = null;
            // See if must reload.
            DateTime reloadTime = DateTime.Now.AddDays(-reloadDays);
            bool forceFlag = (!File.Exists(fileName) || (File.GetLastWriteTime(fileName) < reloadTime));
            Tracing.TraceLine("getDB:" + fileName + ' ' + forceFlag.ToString() + ' ' + reloadTime.ToString(), TraceLevel.Info);
            // Get from the web if file doesn't exist or has expired.
            if (forceFlag)
            {
                // Setup the dictionary/web lock
                //dictionaryWebLock = new Mutex();
                // Get from the web.
                err = getDBFromWeb(fileName);
            }
            else
            {
                // Get data from the current file.
                err = getDBFromFile(fileName);
            }

            if (err != null)
            {
                Tracing.ErrMessageTrace(err, true, true);
            }
        }

        private Exception getDBFromWeb(string fileName)
        {
            Exception err = null;
            Tracing.TraceLine("getDB:get info from web", TraceLevel.Info);
            FileStream myDB = null;
            string tempName = null;
            WebClient web = null;
            Stream page = null;
            try
            {
                web = new WebClient();
                web.BaseAddress = skccBase;
                page = web.OpenRead(roster);
                Tracing.TraceLine("getDB:data read from web", TraceLevel.Info);
                tempName = Path.GetTempFileName();
                myDB = new FileStream(tempName, FileMode.Create);
            }
            catch (Exception ex)
            {
                err = ex;
                Tracing.TraceLine("getDB from web:" + err.Message, TraceLevel.Error);
            }
            finally
            {
                web.Dispose();
                if ((page != null) && (myDB != null))
                {
                    // need dictionary/web lock here.
                    //getLock(true);
                    page.CopyTo(myDB);
                    myDB.Close();
                    if (File.Exists(fileName))
                    {
                        // Replace local database.
                        File.Replace(tempName, fileName, null);
                    }
                    else
                    {
                        // no local database, create it.
                        File.Copy(tempName, fileName);
                    }
                    //getLock(false);
                }
                if (page != null) page.Dispose();
                if (myDB != null) myDB.Dispose();
            }
            Exception err2 = getDBFromFile(fileName);
            // Web error is primary.
            return (err != null) ? err : err2;
        }

        private Exception getDBFromFile(string fileName)
        {
            Exception err = null;
            Tracing.TraceLine("getDB:get info from " + fileName, TraceLevel.Info);
            FileStream myDB = null;

            // grab dictionary/web lock.
            //getLock(true);

            createDictionaries();

            try
            {
                myDB = new FileStream(fileName, FileMode.Open);
                StreamReader sr = new StreamReader(myDB);
                string line = sr.ReadLine();
                while (line != null)
                {
                    addToDictionaries(new SKCCElement(line));
                    line = sr.ReadLine();
                }
            }
            catch (Exception ex)
            {
                err = ex;
                Tracing.TraceLine("getDB from file:" + err.Message, TraceLevel.Error);
            }
            finally
            {
                if (myDB != null) myDB.Dispose();
            }

            // release dictionary/web lock
            //getLock(false);
            return err;
        }

        /// <summary>
        /// Initialize database
        /// </summary>
        /// <param name="adifFile">ADIF session</param>
        public SKCCType(AdifSession adifFile)
        {
            startDate = null;
            startLevel = SKCCLevelType.none;

            createDictionaries();

            try
            {
                skccSession(adifFile, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Initialize database
        /// </summary>
        /// <param name="adifFile">ADIF session</param>
        /// <param name="dt">start date in ADIF format, YYYYMMDD.</param>
        /// <param name="lvl">skccLevelType value to start with.</param>
        /// <remarks>User should close the adif session.</remarks>
        public SKCCType(AdifSession adifFile,
            string dt, SKCCLevelType lvl)
        {
            startDate = dt;
            startLevel = lvl;

            createDictionaries();

            try
            {
                skccSession(adifFile, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Initialize database
        /// </summary>
        /// <param name="files">array of files</param>
        /// <param name="dt">start date in ADIF format, YYYYMMDD.</param>
        /// <param name="lvl">skccLevelType value to start with.</param>
        public SKCCType(string[] files,
            string dt, SKCCLevelType lvl)
        {
            startDate = dt;
            startLevel = lvl;

            createDictionaries();

            // Records is used for multiple files.
            Records = new ArrayList();

            foreach (string name in files)
            {
                AdifSession session = null;
                try
                {
                    session = new AdifSession(name);
                    skccSession(session, true);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (session != null) session.Close();
                }
            }
        }

        /// <summary>
        /// Add this session to the dictionaries.
        /// </summary>
        /// <param name="adifFile">ADIF session</param>
        /// <param name="multiFile">true if part of a multi-file job.</param>
        /// <remarks>startDate and startLevel must be set.</remarks>
        private void skccSession(AdifSession adifFile, bool multiFile)
        {
            bool checkDate = !string.IsNullOrEmpty(startDate);
            bool checkLevel = (startLevel != SKCCLevelType.none);

            Dictionary<string, LogFieldElement> record;
            while (((record = adifFile.Read(false)) != null) &&
                (record.Count > 0))
            {
                if (record.Keys.Contains(AdifTags.ADIF_HeaderEnd)) continue;

                SKCCElement val = new SKCCElement(record);
                LogFieldElement el = null;

                // If no number, check for skcc in the comment field.
                if ((val.Number == null) &&
                    record.TryGetValue(AdifTags.ADIF_Comment, out el))
                {
                    string[] comArray =
                        el.Data.ToUpper().Split(new char[] { ' ' });
                    if ((comArray.Length > 1) && (comArray[0] == "SKCC"))
                    {
                        val.Number = comArray[1]; // also sets the level
                        // Add the SKCC ADIF item to the record if not already there.
                        if (!record.Keys.Contains(AdifTags.ADIF_SKCC))
                        {
                            el = new LogFieldElement(AdifTags.ADIF_SKCC, comArray[1]);
                            record.Add(el.ADIFTag, el);
                        }
                    }
                }

                // Check start date and level.
                if (checkDate &&
                    (!record.TryGetValue(AdifTags.ADIF_DateOn, out el) ||
                    (string.Compare(el.Data, startDate) < 0)))
                {
                    // either no date or record is too early.
                    continue;
                }

                if (checkLevel)
                {
                    if (val.Level < startLevel) continue;
                }

                if ((val.Call == null) | (val.Number == null)) continue;
                addToDictionaries(val);

                // Save this record if a multi-file group.
                if (multiFile) Records.Add(record);
            }
        }

        /// <summary>
        /// Lookup types.
        /// </summary>
        public enum Lookups
        {
            byCall,
            byNumber,
            byCallNumber,
        }

        /// <summary>
        /// Lookup argument by type.
        /// </summary>
        /// <param name="arg">a string argument</param>
        /// <param name="typ">lookup type from Lookups</param>
        /// <returns>an SKCCChainElement or null.</returns>
        public SKCCChainElement Lookup(string arg, Lookups typ)
        {
            Dictionary<string, SKCCChainElement> dict = null;
            SKCCChainElement rv = null;
            // Grab dictionary/web lock
            //getLock(true);
            switch (typ)
            {
                case Lookups.byCall:
                    dict = CallDict;
                    arg = arg.ToUpper();
                    break;
                case Lookups.byNumber:
                    dict = NumberDict;
                    break;
                case Lookups.byCallNumber:
                    dict = KeyDict;
                    arg = arg.ToUpper();
                    break;
            }
            if (dict != null)
            {
                dict.TryGetValue(arg, out rv);
            }
            // release dictionary/web lock
            //getLock(false);
            return rv;
        }
    }
}
