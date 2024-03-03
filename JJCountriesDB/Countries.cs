using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using JJTrace;

namespace JJCountriesDB
{
    class Countries
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
        }
    }
}
