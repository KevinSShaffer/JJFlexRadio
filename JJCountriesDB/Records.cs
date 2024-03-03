using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JJCountriesDB
{
    internal class KeyData
    {
        public Regex Key;
        public char ID;
        public uint Priority;
        public string CQZone = null;
        public string ItUZone = null;
        public bool AlternateZones
        {
            get { return ((CQZone != null) | (ItUZone != null)); }
        }
        public Record Record;
        public KeyData(Regex key, uint priority)
        {
            Key = key;
            Priority = priority;
        }

        public static int SortProc(KeyData k1, KeyData k2)
        {
            // compare equal length strings.
            string s1 = k1.Key.ToString();
            string s2 = k2.Key.ToString();
            int l1 = s1.Length;
            int l2 = s2.Length;
            int len = (l1 < l2) ? l1 : l2;
            int rv = string.Compare(s1.Substring(0, len), s2.Substring(0, len));
            // If equal, highest priority preceeds.
            if (rv == 0) rv = (k1.Priority == k2.Priority) ? 0 : ((k1.Priority > k2.Priority) ? -1 : 1);
            return rv;
        }
    }

    public class Record
    {
        internal enum states
        {
            EOF,
            good,
            goodEOF,
            bad
        }
        internal states State;
        private static int nFields = Enum.GetNames(typeof(fieldIDs)).Length;
        public enum fieldIDs
        {
            country,
            cq,
            itu,
            continents,
            latitude,
            longitude,
            timeZone,
            mainPrefix,
            ituPrefix,
            otherPrefix,
            countryID
        }
        private string[] _fields = new string[nFields];
        internal string lookupName; // uppercase country name

        public string MainPrefix
        {
            get { return _fields[(int)fieldIDs.mainPrefix]; }
            set { _fields[(int)fieldIDs.mainPrefix] = value; }
        }
        //public string ITUPrefix { get { return _fields[(int)fieldIDs.ituPrefix]; } }
        public string OtherPrefix
        {
            get { return _fields[(int)fieldIDs.otherPrefix]; }
            set { _fields[(int)fieldIDs.otherPrefix] = value; }
        }
        public string Country { get { return _fields[(int)fieldIDs.country]; } }
        public string Continent { get { return _fields[(int)fieldIDs.continents]; } }
        public string CQZone { get { return _fields[(int)fieldIDs.cq]; } }
        public string ITUZone { get { return _fields[(int)fieldIDs.itu]; } }
        public string Latitude { get { return _fields[(int)fieldIDs.latitude]; } }
        public string Longitude { get { return _fields[(int)fieldIDs.longitude]; } }
        public string TimeZone { get { return _fields[(int)fieldIDs.timeZone]; } }
        public string CountryID { get { return _fields[(int)fieldIDs.countryID]; } }

        public string this[int id]
        {
            get { return _fields[id]; }
            set
            {
                fieldIDs fid = (fieldIDs)id;
                double d = 0;
                value = value.Trim();
                switch (fid)
                {
                    case fieldIDs.country:
                        _fields[(int)fieldIDs.country] = value;
                        lookupName = value.ToUpper();
                        break;
                    case fieldIDs.latitude:
                        d = 0;
                        if (System.Double.TryParse(value, out d))
                        {
                            char dir = (d < 0) ? 'S' : 'N';
                            int di = (int)(Math.Abs(d) + .5);
                            _fields[id] = di.ToString() + dir;
                        }
                        break;
                    case fieldIDs.longitude:
                        d = 0;
                        if (System.Double.TryParse(value, out d))
                        {
                            char dir = (d < 0) ? 'E' : 'W';
                            int di = (int)(Math.Abs(d) + .5);
                            _fields[id] = di.ToString() + dir;
                        }
                        break;
                    default:
                        _fields[id] = value;
                        break;
                }
            }
        }

        internal static int PrefixCompare(string s1,string s2)
        {
#if zero
            int l1 = s1.Length;
            int l2 = s2.Length;
            if (l1 < l2) return string.Compare(s1, s2.Substring(0, l1));
            else if (l1 > l2) return string.Compare(s1.Substring(0, l2), s2);
            else return string.Compare(s1, s2);
#endif
            return string.Compare(s1, s2);
        }
        internal static int SortByDXCC(Record r1, Record r2)
        {
            return String.Compare(r1.CountryID, r2.CountryID);
        }

        private static Regex astrisk=new Regex("\\*");
        private static Regex literalCall = new Regex("^\\=.+");
        private static Regex slash = new Regex("\\/");
        private static string keyString(string p)
        {
            return '^' + p;
        }
        private static Regex zoneTest = new Regex(@".*[\(\[].*");
        private static Regex cqLeft = new Regex(@"[\(]");
        private static Regex ituLeft = new Regex(@"[\[]");
        private static Regex zoneRight = new Regex(@"[\)\]]");
        private static string getZone(Match m)
        {
            int commaID;
            string cq = cqLeft.Replace(m.Value, ",");
            // 1 means cq zone not specified.
            commaID = (cq == m.Value) ? 1 : 0;
            string rv = ituLeft.Replace(cq, ",");
            // 2 means itu zone not specified.
            commaID = (rv == cq) ? 2 : commaID;
            rv = zoneRight.Replace(rv, "");
            if (commaID != 0)
            {
                // Add a comma.
                int id = rv.IndexOf(',');
                rv = (commaID == 1) ?
                    rv.Substring(0, id) + ',' + rv.Substring(id) : rv + ',';
            }
            return rv;
        }
        internal static List<KeyData> buildKeys(string prefixes)
        {
            List<KeyData> rv = new List<KeyData>();
            string[] prefix = prefixes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i=0;i<prefix.Length;i++)
            {
                string p = astrisk.Replace(prefix[i], "");
                if (p == "") continue;
                KeyData k;
                if (literalCall.IsMatch(p))
                {
                    // literal callsign, remove leading '='.
                    p = p.Substring(1);
                }
                else
                {
                    if (slash.IsMatch(p))
                    {
                        // remove the slash.
                        p = slash.Replace(p, "");
                    }
                }

                // Prefixes may contain CQ and ITU zones.
                string p1 = zoneTest.Replace(p, getZone);
                if (p1 != p)
                {
                    string[] zones = p1.Split(',');
                    k = new KeyData(new Regex(keyString(zones[0])), (uint)zones[0].Length);
                    if (!string.IsNullOrEmpty(zones[1])) k.CQZone = zones[1];
                    if (!string.IsNullOrEmpty(zones[2])) k.ItUZone=zones[2];
                }
                else k = new KeyData(new Regex(keyString(p)), (uint)p.Length);
                k.ID = p[0];
                rv.Add(k);
            }
            return rv;
        }

        internal Record() { }
        internal Record(Record rec)
        {
            State = rec.State;
            lookupName = rec.lookupName;
            Array.Copy(rec._fields, _fields, nFields);
        }
    }
}
