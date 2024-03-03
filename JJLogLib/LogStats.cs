using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using adif;
using JJCountriesDB;
using JJTrace;

namespace JJLogLib
{
    public class LogStats
    {
        private Logs.LogElement le;

        internal class statItem
        {
            public string Arg; // Argument (E.G.) DXCC
            public string Name; // display name
            public uint Total = 0;
            public uint Confirmed = 0;
            public statItem(string arg, string name)
            {
                Arg = arg;
                Name = name;
            }
            public statItem(statItem item)
            {
                Name = item.Name;
                Arg = item.Arg;
                Total = item.Total;
                Confirmed = item.Confirmed;
            }
        }

        /// <summary>
        /// Get the item row's name.
        /// </summary>
        /// <param name="el">log record</param>
        /// <returns>string name</returns>
        public delegate string ItemRowNameDel(Dictionary<string, LogFieldElement> el);
        /// <summary>
        /// Return true if item is to be considered.
        /// </summary>
        /// <param name="le">log record</param>
        /// <returns>true or false</returns>
        public delegate bool CriteriaDel(Dictionary<string, LogFieldElement> le);

        internal class PersistentStats
        {
            /// <summary>
            /// ADIF for the field to use.
            /// </summary>
            public string Key;
            /// <summary>
            /// Row name for the totals.
            /// </summary>
            public string Label;
            /// <summary>
            /// Get the name for each detail row.
            /// </summary>
            public ItemRowNameDel ItemRowName;
            /// <summary>
            /// Return true if the item should be considered, (e.g.) only consider the state for the U.S.
            /// </summary>
            public CriteriaDel Criteria;
            /// <summary>
            /// Total worked
            /// </summary>
            public uint Total = 0;
            /// <summary>
            /// total confirmed.
            /// </summary>
            public uint Confirmed = 0;
            /// <summary>
            /// Dictionary for each entry's stats.
            /// </summary>
            public Dictionary<string, statItem> Stats;
            public PersistentStats(string key, string label, ItemRowNameDel itemRowName, CriteriaDel criteria)
            {
                Key = key;
                Label = label;
                ItemRowName = itemRowName;
                Criteria = criteria;
                Stats = new Dictionary<string, statItem>();
            }
        }

        /// <summary>
        /// Static list of stats.
        /// </summary>
        internal static List<PersistentStats> statList;

        /// <summary>
        /// One of these for each item, see LogStats constructor.
        /// </summary>
        public class RowIDs
        {
            /// <summary>
            /// ADIF of the log field to use
            /// </summary>
            public string FieldTag;
            /// <summary>
            /// The general display name such as "country" or "state".
            /// </summary>
            public string TotalRowName;
            /// <summary>
            /// Function to get the row name, (i.e.) the individual name, such as the country's name.
            /// </summary>
            public ItemRowNameDel ItemRowName;
            /// <summary>
            /// Return true if the item should be considered, (e.g.) only consider the state for the U.S.
            /// </summary>
            public CriteriaDel Criteria;
            public RowIDs(string fieldTag, string totalRowName, ItemRowNameDel itemRowName, CriteriaDel criteria)
            {
                FieldTag = fieldTag;
                TotalRowName = totalRowName;
                ItemRowName = itemRowName;
                Criteria = criteria;
            }
        }

        /// <summary>
        /// new logStats
        /// </summary>
        /// <param name="loge">LogElement class</param>
        /// <param name="labelListIn">List of RowIDs for each individual item.</param>
        public LogStats(Logs.LogElement loge, List<RowIDs> labelListIn)
        {
            Tracing.TraceLine("LogStats", TraceLevel.Info);
            le = loge;
            statList = new List<PersistentStats>();
            foreach (RowIDs row in labelListIn)
            {
                statList.Add(new PersistentStats(row.FieldTag, row.TotalRowName, row.ItemRowName, row.Criteria));
            }
            Logs.ShowStats = showStats;
        }

        private bool statError = false; // If true, don't do any more adds or removes.
        /// <summary>
        /// Add to the stats.
        /// </summary>
        /// <param name="fields">record dictionary</param>
        public void StatAdd(Dictionary<string, LogFieldElement> fields)
        {
            if (statError) return;
            Tracing.TraceLine("StatAdd", TraceLevel.Info);
            try
            {
                for (int i = 0; i < statList.Count; i++)
                {
                    PersistentStats stat = statList[i];
                    string key = fields[stat.Key].Data;
                    if (!string.IsNullOrEmpty(key))
                    {
                        if (!stat.Criteria(fields)) continue; // ignore
                        key = key.ToUpper(); // case-blind
                        statItem item = null;
                        if (!stat.Stats.TryGetValue(key, out item))
                        {
                            string name = stat.ItemRowName(fields);
                            if (string.IsNullOrEmpty(name))
                            {
                                name = stat.ItemRowName(fields);
                            }
                            item = new statItem(key, name);
                            stat.Stats.Add(key, item);
                            stat.Total++; // This total is the number of entries.
                        }
                        item.Total++; // # entries for this item
                        string cfm = fields[AdifTags.ADIF_QSL_RCVD].Data;
                        if (!string.IsNullOrEmpty(cfm) && (cfm[0] == 'Y'))
                        {
                            item.Confirmed++; // Total confirmed for this item
                            if (item.Confirmed == 1) stat.Confirmed++; // overall confirmed
                        }
                        Tracing.TraceLine("StatAdd:" + item.Name + ' ' + stat.Total.ToString() + ' ' + stat.Confirmed.ToString() + ' ' + item.Total.ToString() + ' ' + item.Confirmed.ToString(), TraceLevel.Verbose);
                    }
                    else Tracing.TraceLine("StatAdd:no value for " + fields[AdifTags.ADIF_Call].Data + ' ' + stat.Key, TraceLevel.Info);
                }
            }
            catch (Exception ex)
            {
                statError = true;
                Tracing.ErrMessageTrace(ex, true);
            }
        }

        /// <summary>
        /// Remove from the stats.
        /// </summary>
        /// <param name="fields">record dictionary</param>
        public void StatRemove(Dictionary<string, LogFieldElement> fields)
        {
            if (statError) return;
            Tracing.TraceLine("StatRemove", TraceLevel.Info);
            try
            {
                for (int i = 0; i < statList.Count; i++)
                {
                    PersistentStats stat = statList[i];
                    string key = fields[stat.Key].Data;
                    if (!string.IsNullOrEmpty(key))
                    {
                        statItem item = null;
                        if (!stat.Stats.TryGetValue(key, out item)) continue;
                        item.Total--;
                        if (item.Total == 0) stat.Total--;
                        string cfm = fields[AdifTags.ADIF_QSL_RCVD].Data;
                        if (!String.IsNullOrEmpty(cfm) && (cfm[0] == 'Y'))
                        {
                            item.Confirmed--;
                            if (item.Confirmed == 0) stat.Confirmed--;
                        }
                    }
                    else Tracing.TraceLine("StatRemove:no value for " + fields[AdifTags.ADIF_Call].Data + ' ' + stat.Key, TraceLevel.Info);
                }
            }
            catch (Exception ex)
            {
                statError = true;
                Tracing.ErrMessageTrace(ex, true);
            }
        }

        private void showStats()
        {
            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) + "\\JJRadioStats.htm";
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(fileName);
                sw.WriteLine("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">");
                sw.WriteLine("<HTML>");
                sw.WriteLine("<STYLE TYPE=\"text/css\" MEDIA=screen>");
                sw.WriteLine("#countryCol { width: 70% }");
                sw.WriteLine("#valueCol { width: 15% }");
                sw.WriteLine("</STYLE>");
                sw.WriteLine("<table>");
                sw.WriteLine("<thead>");
                sw.WriteLine("<tr>");
                sw.WriteLine("<th id=\"countryCol\">overall</th>");
                sw.WriteLine("<th id=\"valueCol\">total</th>");
                sw.WriteLine("<th id=\"valueCol\">confirmed</th>");
                sw.WriteLine("</tr>");
                sw.WriteLine("</thead>");
                sw.WriteLine("<tbody>");
                foreach (PersistentStats stats in statList)
                {
                    sw.WriteLine("<tr>");
                    sw.WriteLine("<td id=\"countryCol\">" + stats.Label + "</th>");
                    sw.WriteLine("<td id=\"valueCol\">" + stats.Total + "</th>");
                    sw.WriteLine("<td id=\"valueCol\">" + stats.Confirmed + "</th>");
                    sw.WriteLine("</tr>");
                }
                sw.WriteLine("</tbody>");
                sw.WriteLine("</table>");

                // Stats for each detail
                mkTable(sw);
                sw.WriteLine("</HTML>");
                sw.Close();
                Process.Start("FILE:" + fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }
            finally
            {
                if (sw != null) sw.Dispose();
            }
        }
        private void mkTable(StreamWriter sw)
        {
            foreach (PersistentStats stats in statList)
            {
                sw.WriteLine("<table>");
                sw.WriteLine("<thead>");
                sw.WriteLine("<tr>");
                sw.WriteLine("<th id=\"countryCol\">" + stats.Label + "</th>");
                sw.WriteLine("<th id=\"valueCol\">total</th>");
                sw.WriteLine("<th id=\"valueCol\">confirmed</th>");
                sw.WriteLine("</tr>");
                sw.WriteLine("</thead>");
                sw.WriteLine("<tbody>");
                List<statItem> sorted = new List<statItem>();
                foreach (statItem item in stats.Stats.Values)
                {
                    sorted.Add(item);
                }
                sorted.Sort(nameCompare);

                foreach (statItem item in sorted)
                {
                    sw.WriteLine("<tr>");
                    sw.WriteLine("<th id=\"countryCol\">" + item.Name + "</th>");
                    sw.WriteLine("<th id=\"valueCol\">" + item.Total.ToString() + "</th>");
                    sw.WriteLine("<th id=\"valueCol\">" + item.Confirmed.ToString() + "</th>");
                    sw.WriteLine("</tr>");
                }
                sw.WriteLine("</tbody>");
                sw.WriteLine("</table>");
            }
        }
        private int nameCompare(statItem i1,statItem i2)
        {
            return string.Compare(i1.Name, i2.Name);
        }
    }
}
