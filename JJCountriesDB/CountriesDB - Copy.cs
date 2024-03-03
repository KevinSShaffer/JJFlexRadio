#undef useAfterSlash
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace JJCountriesDB
{
    internal class TextData
    {
        const int nFields = 10;
        const char sepChar = ':';
        const char prefixDeleted = '*';
        const char range = '-';
        const char prefixSep = ',';

        /// <summary>
        /// a raw input record
        /// </summary>
        private class rawRec
        {
            public string[] fields = new string[nFields];
            public enum fieldIDs
            {
                mainPrefix,
                country,
                continents,
                itu,
                cq,
                timeZone,
                latitude,
                longitude,
                ituPrefix,
                otherPrefix
            }
        }

        internal class keyExpression
        {
            public const string expressionTest = "[-_/]";
            public Regex key;
            public string keyString;
            public int sig;
            public bool isMain;
            public keyExpression(intermediate inm)
            {
                key = inm.exp;
                keyString = inm.keyString; ;
                sig = inm.sig;
                isMain = inm.isMain;
            }
        }

        internal class initialData
        {
            internal Collection<keyExpression> keys;
            internal CountriesDB.CountryInfo info;
            public initialData()
            {
                keys = new Collection<keyExpression>();
                info = new CountriesDB.CountryInfo();
            }
        }

        internal class intermediate
        {
            public int sig;
            public Regex exp;
            public bool isMain;
            public string keyString; // original key string
            public intermediate(string k, int sg, string ks)
            {
                keyString = ks;
                sig = sg;
                isMain = false;
                if (k == "") exp = null;
                else exp = new Regex("^"+k);
            }
        }

        private class intrCompare : IComparer<intermediate>
        {
            public int Compare(intermediate k1, intermediate k2)
            {
                return string.Compare(k1.keyString, k2.keyString, true);
            }
        }

        /// <summary>
        /// construct an output record
        /// </summary>
        /// <param name="rr">raw record to use</param>
        /// <returns>a new initialData</returns>
        private initialData buildinitialData(rawRec rr)
        {
            initialData oRec = new initialData();
            oRec.info.country = rr.fields[(int)rawRec.fieldIDs.country];
            oRec.info.continents = rr.fields[(int)rawRec.fieldIDs.continents];
            oRec.info.itu = rr.fields[(int)rawRec.fieldIDs.itu];
            oRec.info.cq = rr.fields[(int)rawRec.fieldIDs.cq];
            oRec.info.timezone = rr.fields[(int)rawRec.fieldIDs.timeZone];
            oRec.info.latitude = rr.fields[(int)rawRec.fieldIDs.latitude];
            oRec.info.longitude = rr.fields[(int)rawRec.fieldIDs.longitude];
            oRec.info.mainPrefix = rr.fields[(int)rawRec.fieldIDs.mainPrefix];
            oRec.info.ituPrefix = rr.fields[(int)rawRec.fieldIDs.ituPrefix];
            oRec.info.otherPrefix = rr.fields[(int)rawRec.fieldIDs.otherPrefix];
            // Use an intermediate key collection for now.
            Collection<intermediate> intr = new Collection<intermediate>();
            getSeparateFields(ref intr, oRec.info.mainPrefix);
            // Indicate isMain for the main prefix(s).
            for (int i = 0; (i < intr.Count); i++) intr[i].isMain = true;
            getSeparateFields(ref intr, oRec.info.ituPrefix);
            getSeparateFields(ref intr, oRec.info.otherPrefix);
            oRec.keys = new Collection<keyExpression>();
            // Add to the oRec.Keys collection.
            // Sort and eliminate duplicates
            string lastKey = null;
            IOrderedEnumerable<intermediate> sorter = 
                intr.OrderBy(inm => inm, new intrCompare());
            foreach (intermediate inm in sorter)
            {
                // See if this duplicates the last added entry
                if ((lastKey != null) && (inm.keyString == lastKey) &&
                    (!inm.isMain)) continue;
                lastKey = inm.keyString;
                // if not an expression, see if it's covered by
                // any other expression.
                // Keep the main prefix anyway.
                bool addIt = true;
                if (inm.exp == null)
                {
                    if (!inm.isMain)
                    {
                        for (int i = 0; (i < intr.Count); i++)
                        {
                            if ((intr[i].exp != null) &&
                                (intr[i].exp.IsMatch(inm.keyString)))
                            {
                                addIt = false;
                                break;
                            }
                        }
                    }
                    inm.exp = new Regex("^" + inm.keyString);
                }
                if (addIt)
                {
                    oRec.keys.Add(new keyExpression(inm));
                    _keyCount++;
                }
            }
            return (oRec);
        }

        /// <summary>
        /// Build a key string
        /// </summary>
        /// <param name="txt">the string</param>
        /// <returns>an intermediate object, null on error</returns>
        private intermediate goodKey(string txt)
        {
            // allow some alphanumerics, followed by one optional 
            // one "_", or one "-alphanum, or one optional "/alphanum".
            Regex r1 = new Regex("^[A-Z0-9]+(_[A-Z0-9]*)?(-[A-Z0-9]+)?(/[A-Z0-9]+)?$");
            // Only allow one '_', '-', or '/' expression.
            Regex r2 = new Regex(".*[-_/].*[-_/]");
            Regex r3 = new Regex(keyExpression.expressionTest);
            // Throw out the "~" for unofficial prefixes.
            if (txt[txt.Length - 1] == '~') txt = txt.Substring(0, txt.Length - 1);
            // Validate the key string.
            if (!r1.IsMatch(txt) || r2.IsMatch(txt)) return (null);
            // Get expression data.
            Match m = r3.Match(txt);
            int signif = txt.Length;
            string origKey = txt; // original key string
            if (m.Success)
            {
                // It's an expression
                string sLeft = txt.Substring(0, m.Index);
                string sRight;
                if (m.Index != txt.Length - 1) sRight = txt.Substring(m.Index + 1);
                else sRight = "";
                switch (txt[m.Index])
                {
                    case '/':
                        txt = sLeft + ".*/" + sRight;
                        break;
                    case '-':
                        // the form must be kx-ky or kx-y
                        int ll = sLeft.Length;
                        int rl = sRight.Length;
                        if (ll < rl) return (null);
                        if ((ll > rl) && (rl != 1)) return (null);
                        if ((ll == rl) &&
                            (sLeft.Substring(0, ll - 1) !=
                             sRight.Substring(0, rl - 1)))
                            return (null);
                        string cl = sLeft.Substring(ll - 1);
                        string cr = sRight.Substring(rl - 1);
                        txt = (ll > 1) ? sLeft.Substring(0, ll - 1) : "";
                        // "xA-xZ" really just means "x".
                        if (!((cl == "A") && (cr == "Z")))
                        {
                            txt += "[" + sLeft.Substring(ll - 1) + "-";
                            txt += sRight.Substring(rl - 1) + "]";
                        }
                        else
                        {
                            ll -= 1;
                            origKey = txt;
                        }
                        signif = ll;
                        break;
                    case '_':
                        txt = sLeft + "\\d*" + sRight;
                        // Leave significant chars as is.
                        break;
                }
            }
            else
            {
                txt = "";
            }
            // txt contains the expression, or "" if not an expression.
            return (new intermediate(txt, signif, origKey));
        }

        /// <summary>
        /// build separate key fields
        /// </summary>
        /// <param name="keys">keys collection</param>
        /// <param name="txt">string containing prefix(s)</param>
        private void getSeparateFields(ref Collection<intermediate> keys, 
            string txt)
        {
            if (txt == "") return;
            int pos = 0;
            int i;
            for (i = 0; (i < txt.Length); i++)
            {
                if (txt[i] == prefixSep)
                {
                    if (i != pos)
                    {
                        string keyStr = txt.Substring(pos, i - pos).ToUpper();
                        intermediate kexp = goodKey(keyStr);
                        if (kexp != null) keys.Add(kexp);
                    }
                    pos = i + 1;
                }
            }
            if (i != pos)
            {
                string keyStr = txt.Substring(pos, i - pos).ToUpper();
                intermediate kexp = goodKey(keyStr);
                if (kexp != null) keys.Add(kexp);
            }
        }

        public Collection<initialData> outRecs;
        public initialData this[int id]
        {
            get { return outRecs[id]; }
        }
        public int Count
        {
            get { return outRecs.Count; }
        }
        private int _keyCount;
        /// <summary> readOnly key count </summary>
        public int keyCount
        {
            get { return _keyCount; }
        }

        /// <summary>
        /// determine if this string is a good record
        /// </summary>
        /// <param name="txt">the string</param>
        /// <returns>new rawRec if looks good, null if not</returns>
        private rawRec goodRec(string txt)
        {
            rawRec rr = new rawRec();
            int pos = 0;
            int fieldCount = 0;
            int i;
            string fld;
            for (i = 0; (i < txt.Length) && (fieldCount <= nFields); i++)
            {
                if (txt[i] == sepChar)
                {
                    fld = (i == pos) ? "" : txt.Substring(pos, i - pos);
                    rr.fields[fieldCount++] = fld;
                    pos = i + 1;
                }
            }
            if (fieldCount != nFields - 1) return (rawRec)null;
            else
            {
                fld = (i == pos) ? "" : txt.Substring(pos, i - pos);
                rr.fields[fieldCount] = fld;
            }
            // good if first and second fields present, and nFields fields.
            if (rr.fields[(int)rawRec.fieldIDs.mainPrefix] == "") return (rawRec)null;
            if (rr.fields[(int)rawRec.fieldIDs.country] == "") return (rawRec)null;
            return rr;
        }

        /// <summary>
        /// text data using the specified file
        /// </summary>
        /// <param name="fName">file name</param>
        public TextData(string fName)
        {
            StreamReader sr;
            outRecs = new Collection<initialData>();
            _keyCount = 0;
            try
            {
                sr = new StreamReader(fName);
                string txt;
                while ((txt = sr.ReadLine()) != null)
                {
                    rawRec rr = goodRec(txt);
                    if (rr != (rawRec)null)
                    {
                        initialData rec = buildinitialData(rr);
                        // We may have ended up with no keys.
                        if (rec.keys.Count > 0) outRecs.Add(rec);
                    }
                }
            }
            catch { throw; }
            if (outRecs.Count == 0) throw new CountriesDB.NotCountries();
            sr.Close();
            sr.Dispose();
        }
    }

    /// <summary> Countries and their prefixes </summary>
    public class CountriesDB
    {
        /// <summary>
        /// JJCountriesDB.dll version
        /// </summary>
        public static Version Version
        {
            get
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                AssemblyName asmName = asm.GetName();
                return asmName.Version;
            }
        }

        internal TextData stage1;

        /// <summary> Country and prefix information </summary>
        public class CountryInfo
        {
            /// <summary> main prefix </summary>
            public string mainPrefix { get; internal set; }
            /// <summary> ITU prefixes </summary>
            public string ituPrefix { get; internal set; }
            /// <summary> other prefixes </summary>
            public string otherPrefix { get; internal set; }
            /// <summary> country name </summary>
            public string country { get; internal set; }
            /// <summary> continent(s) </summary>
            public string continents { get; internal set; }
            /// <summary> ITU zones </summary>
            public string itu { get; internal set; }
            /// <summary> CQ zones </summary>
            public string cq { get; internal set; }
            /// <summary> time difference from UTC </summary>
            public string timezone { get; internal set; }
            /// <summary> latitude </summary>
            public string latitude { get; internal set; }
            /// <summary> longitude </summary>
            public string longitude { get; internal set; }
            /// <summary> The key that was matched by countryLookup </summary>
            public string matchedKeyString { get; internal set; }
            /// <summary> object instance </summary>
            internal CountriesDB db { get; set; }
            /// <summary> id in KeyedCountries array </summary>
            internal int id { get; set; }
            /// <summary> overloaded, produce an anotated string of this information </summary>
            /// <param name="expSW"> true to include keys' regular expression(s) </param>
            public string ToString(bool expSW)
            {
                string sep = "; ";
                string rv = country + "(" + mainPrefix + ")" + sep + continents + sep +
                    "itu:" + itu + sep + "CQ:" + cq + sep +
                    "TZ:" + timezone + sep +
                    "lat/long:" + latitude + "/" + longitude;
                if (ituPrefix != "") rv += sep + "ITU prefix(s):" + ituPrefix;
                if (otherPrefix != "") rv += sep + "other prefix(s):" + otherPrefix;
                if (expSW)
                {
                    rv += " (";
                    bool sw = false;
                    for (int i = 0; (i < db.KeyedCountries.Length); i++)
                    {
                        if (db.KeyedCountries[i].CountryName == country)
                        {
                            if (sw) rv += ", ";
                            else sw = true;
                            rv += db.KeyedCountries[i].key.ToString();
                        }
                    }
                    rv += ")";
                }
                if (matchedKeyString != "") 
                    rv += " matched:" + matchedKeyString;
                return rv;
            }
            /// <summary> overloaded, produce an anotated string of this information </summary>
            public override string ToString()
            {
                return ToString(false);
            }
            /// <summary> </summary>
            public CountryInfo() 
            {
                matchedKeyString = "";
            }
        }
        /// <summary> countryInfo sorted by country </summary>
        public CountryInfo[] Countries;
#if keepThis
        /// <summary> readOnly countryInfo sorted by country </summary>
        public CountryInfo[] Countries
        {
            get
            {
                CountryInfo[] rv = new CountryInfo[countries.Length];
                for (int i = 0; (i < rv.Length); i++) rv[i] = new CountryInfo(countries[i]);
                return rv;
            }
        }
#endif

        internal class keyInfo
        {
            internal Regex key;
            internal string keyString;
            internal int significant;
            internal CountryInfo country;
            internal bool isMain;
            internal keyInfo(TextData.keyExpression k, CountryInfo ci)
            {
                key = k.key;
                keyString = k.keyString;
                significant = k.sig;
                isMain = k.isMain;
                country = ci;
            }
        }

        /// <summary> key with associated CountryInfo </summary>
        public class KeyAndCountry
        {
            internal keyInfo ki;
            /// <summary> regex key </summary>
            internal Regex key { get { return ki.key; } }
            /// <summary> ReadOnly key string </summary>
            public string KeyString { get { return ki.keyString; } }
            /// <summary> ReadOnly CountryInfo object </summary>
            public CountryInfo Country { get { return ki.country; } }
            /// <summary> ReadOnly country name </summary>
            public string CountryName
            {
                get { return Country.country; }
            }
            /// <summary> ReadOnly continent </summary>
            public string Continent
            {
                get { return Country.continents; }
            }
            /// <summary> ReadOnly ITU zones </summary>
            public string ITUZones
            {
                get { return Country.itu; }
            }
            /// <summary> ReadOnly CQ zones </summary>
            public string CQZones
            {
                get { return Country.cq; }
            }
            /// <summary> ReadOnly ITU prefixes </summary>
            public string ITUPrefix
            {
                get { return Country.ituPrefix; }
            }
            /// <summary> ReadOnly Other prefixes </summary>
            public string OtherPrefix
            {
                get { return Country.otherPrefix; }
            }
            /// <summary> ReadOnly Main prefix </summary>
            public string MainPrefix
            {
                get { return Country.mainPrefix; }
            }
            /// <summary> ReadOnly Latitude </summary>
            public string Latitude
            {
                get { return Country.latitude; }
            }
            /// <summary> ReadOnly Longitude </summary>
            public string Longitude
            {
                get { return Country.longitude; }
            }
            /// <summary> ReadOnly Matched key </summary>
            public string MatchedKeyString
            {
                get { return Country.matchedKeyString; }
            }
            internal KeyAndCountry(keyInfo k)
            {
                ki = k;
            }
            /// <summary> </summary>
            /// <param name="kc"> the KeyAndCountry item </param>
            public KeyAndCountry(KeyAndCountry kc)
            {
                ki = kc.ki;
            }
        }
        /// <summary> array of KeyAndCountry objects in lookup order </summary>
        internal KeyAndCountry[] KeyedCountries;
        /// <summary> Alphabetic keys and countries </summary>
        public KeyAndCountry[] KeysAndCountries
        {
            get
            {
                KeyAndCountry[] kc = new KeyAndCountry[KeyedCountries.Length];
                int i = 0;
                IOrderedEnumerable<KeyAndCountry> kcSorter =
                    KeyedCountries.OrderBy(k => k, new kcCompare());
                foreach (KeyAndCountry k in kcSorter)
                {
                    kc[i++] = k;
                }
                return kc;
            }
        }

        /// <summary>
        /// indexes the KeyedCountries array by first character of the call.
        /// there is one of these for each number and character.
        /// </summary>
        internal class keyIndex
        {
            internal const int nIDs = 36;
            internal int id;
            internal keyIndex(int i)
            {
                id = i;
            }
            internal static int index(string c)
            {
                return index(c[0]);
            }
            internal static int index(char c0)
            {
                if (c0 <= '9') return (int)(c0 - '0');
                if ((c0 >= 'A') && (c0 <= 'Z')) return (int)(c0 - 'A' + 10);
                return -1;
            }
        }
        internal keyIndex[] IDs;

        private int _countriesCount, _keysCount;
        /// <summary> number of countries </summary>
        public int CountriesCount
        {
            get { return _countriesCount; }
        }
        /// <summary> number of country keys </summary>
        public int KeysCount
        {
            get { return _keysCount; }
        }

        // comparer classes
        private class kcCompare : IComparer<KeyAndCountry>
        {
            public int Compare(KeyAndCountry k1, KeyAndCountry k2)
            {
                return string.Compare(k1.KeyString, k2.KeyString, true);
            }
        }
        private class keyCompare : IComparer<keyInfo>
        {
            public int Compare(keyInfo k1, keyInfo k2)
            {
                int rv = k1.keyString[0] - k2.keyString[0];
                // Note that significant chars sort in inverse order.
                if (rv == 0) rv = k2.significant - k1.significant;
                return rv;
            }
        }

        /// <summary> Not a K2DI countries text file </summary>
        public class NotCountries : Exception
        {
            /// <summary> Not a K2DI countries text file </summary>
            public NotCountries() : base("This file contains no country data.") { }
        }

        /// <summary> Lookup country(s) by call sign </summary>
        /// <param name="cs"> call sign string </param>
        /// <returns> countryInfo array </returns>
        public CountryInfo[] CountryLookup(string cs)
        {
            Collection<CountryInfo> rv = iCountryLookup(cs);
            return (rv == null)? null: rv.ToArray();
        }
        private Collection<CountryInfo> iCountryLookup(string cs)
        {
            Regex goodCall = new Regex("[A-Z0-9/]");
            cs = cs.ToUpper();
            if (!goodCall.IsMatch(cs)) return null;
            int slashID = cs.IndexOf('/');
            // can't have '/' as the first or last character.
            if ((slashID == 0) || (slashID == cs.Length - 1)) return null;
            Collection<CountryInfo> rv = new Collection<CountryInfo>();
            char c0 = cs[0];
            // Make sure we have some prefixes, (e.g.) '0'.
            if (IDs[keyIndex.index(c0)] == null) return null;
            int sig = 0;
            bool mainMatch = false;
            for (int id = IDs[keyIndex.index(c0)].id;
                (id < KeyedCountries.Length) && 
                (c0 == KeyedCountries[id].KeyString[0]); 
                id++)
            {
                if (KeyedCountries[id].key.IsMatch(cs))
                {
                    keyInfo ki = KeyedCountries[id].ki;
                    // Give preference to main prefixes.
                    if (ki.isMain) mainMatch = true;
                    else
                    {
                        if (mainMatch) continue; // don't return this one.
                    }
#if useAfterSlash
                    // ignore a slash in the call if a slash was found in this expression.
                    if ((slashID != -1) && (ki.keyString.IndexOf('/') != -1))
                    {
                        slashID = -1;
                    }
#endif
                    // quit if the significant count drops.
                    if (sig == 0) sig = ki.significant;
                    else
                    {
                        if (sig > ki.significant) break;
                    }
                    CountryInfo ci = ki.country;
                    ci.matchedKeyString = ki.keyString;
                    rv.Add(ci);
                }
            }
#if useAfterSlash
            // If a slash was in the call, but not any of the expressions, 
            // and there are multiple characters after the slash, or just a letter,
            // lookup the part after the slash.
            if (slashID != -1)
            {
                string cs1 = cs.Substring(slashID + 1);
                if ((cs1.Length > 1) || (cs1[0] > '9'))
                {
                    rv = this.iCountryLookup(cs1);
                }
            }
#endif
            return rv;
        }

        /// <summary> </summary>
        /// <param name="textFile">file containing K2DI colon-separated list </param>
        public CountriesDB(string textFile)
        {
            _countriesCount = 0;
            _keysCount = 0;
            try
            {
                // get countries and keys from the text file
                stage1 = new TextData(textFile);
            }
            catch { throw; }
            // create the sorted countries array.
            _countriesCount = stage1.Count;
            _keysCount = stage1.keyCount;
            Countries = new CountryInfo[stage1.Count];
            keyInfo[] myKeys = new keyInfo[stage1.keyCount]; // intermediate keyInfo array
            int id = 0;
            int kID = 0;
            IEnumerable<TextData.initialData> countrySort = stage1.outRecs.OrderBy(o => o.info.country);
            foreach (TextData.initialData o in countrySort)
            {
                o.info.db = this;
                o.info.id = id;
                Countries[id] = o.info;
                // save the keys for now.
                foreach (TextData.keyExpression k in o.keys)
                {
                    myKeys[kID++] = new keyInfo(k, Countries[id]);
                }
                id++;
            }
            // Put the keys in place.
            KeyedCountries = new KeyAndCountry[stage1.keyCount];
            IDs = new keyIndex[keyIndex.nIDs]; // first key char to key id
            kID = 0;
            char lastFirst = (char)0;
            IOrderedEnumerable<keyInfo> keySorter = 
                myKeys.OrderBy(k => k, new keyCompare());
            foreach (keyInfo k in keySorter)
            {
                char c = k.keyString[0];
                if (c != lastFirst)
                {
                    // new IDs item.
                    IDs[keyIndex.index(c)] = new keyIndex(kID);
                    lastFirst = c;
                }
                KeyedCountries[kID++] = new KeyAndCountry(k);
            }
        }
    }
}
