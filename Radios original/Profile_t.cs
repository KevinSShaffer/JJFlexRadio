using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Flex.Smoothlake.FlexLib;

namespace Radios
{
    // config data.
    /// <summary>
    /// profile types
    /// </summary>
    public enum ProfileTypes
    {
        none = -1,
        global = 0,
        tx = 1,
        mic = 2,
        display = 3
    }

    public class Profile_t
    {
        /// <summary>
        /// Profile name
        /// </summary>
        public string Name;
        /// <summary>
        /// True if default.
        /// </summary>
        public bool Default = false;
        /// <summary>
        /// Profile type enumeration
        /// </summary>
        public ProfileTypes ProfileType = ProfileTypes.none;

        /// <summary>
        /// New Profile_t
        /// </summary>
        public Profile_t() { }

        /// <summary>
        /// New Profile_t
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pType">type</param>
        /// <param name="dflt">default</param>
        public Profile_t(string name, ProfileTypes pType,
            bool dflt)
        {
            Name = name;
            ProfileType = pType;
            Default = dflt;
        }

        /// <summary>
        /// Make a copy, preserving Current.
        /// </summary>
        /// <param name="p">a Profile_t</param>
        public Profile_t(Profile_t p)
        {
            Name = p.Name;
            ProfileType = p.ProfileType;
            Default = p.Default;
        }

        public override string ToString()
        {
            string rv = "";
            if (!string.IsNullOrEmpty(Name)) rv += "Name:" + Name + ',';
            rv += "ProfileType:" + ProfileType.ToString() + ' ';
            rv += "Default:" + Default.ToString();
            return rv;
        }

        /// <summary>
        /// Generate the default profile name.
        /// </summary>
        /// <param name="prefix">currently unused</param>
        /// <returns></returns>
        public static string GenerateProfileName(string prefix)
        {
            //return prefix + "profile";
            return "JJRadioDefault";
        }

        public static bool Current(FlexBase f, Profile_t p)
        {
            bool rv = false;
            Radio r = f.theRadio;
            switch (p.ProfileType)
            {
                case ProfileTypes.display:
                    rv = (r.ProfileDisplaySelection == p.Name);
                    break;
                case ProfileTypes.global:
                    rv = (r.ProfileGlobalSelection == p.Name);
                    break;
                case ProfileTypes.mic:
                    rv = (r.ProfileMICSelection == p.Name);
                    break;
                case ProfileTypes.tx:
                    rv = (r.ProfileTXSelection == p.Name);
                    break;
            }
            return rv;
        }
    }
}
