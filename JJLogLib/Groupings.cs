using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JJTrace;

namespace JJLogLib
{
    /// <summary>
    /// used for item groups such as SKCC Senators.
    /// </summary>
    internal class Grouping
    {
        /// <summary>
        /// group type
        /// </summary>
        public enum Types
        {
            unique,
            dupsAllowed
        }
        private Types dictType;

        class dictItem
        {
            public string Key;
            public int Count;
            public dictItem(string k, int c)
            {
                Key = k;
                Count = c;
            }
        }
        Dictionary<string, dictItem> theGroup;

        /// <summary>
        /// item count
        /// </summary>
        public int Count
        {
            get;
            private set;
        }

        /// <summary>
        /// new group
        /// </summary>
        /// <param name="t">is the type</param>
        public Grouping(Types t)
        {
            dictType = t;
            theGroup = new Dictionary<string, dictItem>();
            Count = 0;
        }

        /// <summary>
        /// Add to the group
        /// </summary>
        /// <param name="str">is the string to add</param>
        /// <returns>true if added</returns>
        public bool Add(string str)
        {
            dictItem instance;
            bool rv;
            if (theGroup.TryGetValue(str, out instance))
            {
                rv = (dictType == Types.dupsAllowed);
                if (rv)
                {
                    instance.Count++;
                }
            }
            else
            {
                dictItem item = new dictItem(str, 1);
                theGroup.Add(item.Key, item);
                rv = true;
            }
            if (rv) Count++;
            return rv;
        }

        /// <summary>
        /// Remove the item
        /// </summary>
        /// <param name="str">the string to remove</param>
        public void Remove(string str)
        {
            dictItem instance;
            if (theGroup.TryGetValue(str, out instance))
            {
                if (instance.Count > 1) instance.Count--;
                else theGroup.Remove(instance.Key);
                Count--;
            }
        }
    }
}
