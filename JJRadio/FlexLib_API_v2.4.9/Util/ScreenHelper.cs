// ****************************************************************************
///*!	\file ScreenHelper.cs
// *	\brief Class to help handle screen settings like orientation
// *
// *	\copyright	Copyright 2012-2016 FlexRadio Systems.  All Rights Reserved.
// *				Unauthorized use, duplication or distribution of this software is
// *				strictly prohibited by law.
// *
// *	\date 2016-11-18
// *	\author Eric Wachsmann, KE5DTO
// */
// ****************************************************************************

using System;
using System.Runtime.InteropServices;


namespace Util
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DEVMODE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmDeviceName;

        public short dmSpecVersion;
        public short dmDriverVersion;
        public short dmSize;
        public short dmDriverExtra;
        public int dmFields;
        public int dmPositionX;
        public int dmPositionY;
        public int dmDisplayOrientation;
        public int dmDisplayFixedOutput;
        public short dmColor;
        public short dmDuplex;
        public short dmYResolution;
        public short dmTTOption;
        public short dmCollate;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmFormName;

        public short dmLogPixels;
        public short dmBitsPerPel;
        public int dmPelsWidth;
        public int dmPelsHeight;
        public int dmDisplayFlags;
        public int dmDisplayFrequency;
        public int dmICMMethod;
        public int dmICMIntent;
        public int dmMediaType;
        public int dmDitherType;
        public int dmReserved1;
        public int dmReserved2;
        public int dmPanningWidth;
        public int dmPanningHeight;
    };

    public class ScreenHelper
    {
        [DllImport("user32.dll")] 
        private static extern int EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode); 
        [DllImport("user32.dll")]
        private static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);

        private const int ENUM_CURRENT_SETTINGS = -1;
        private const int CDS_UPDATEREGISTRY = 0x01;
        private const int CDS_TEST = 0x02;
        private const int DISP_CHANGE_SUCCESSFUL = 0;
        private const int DISP_CHANGE_RESTART = 1;
        private const int DISP_CHANGE_FAILED = -1;
        private const int DMDO_DEFAULT = 0;
        private const int DMDO_90 = 1;
        private const int DMDO_180 = 2;
        private const int DMDO_270 = 3;

        public static void ChangeOrientation(int degrees)
        {
            short new_orientation = DMDO_DEFAULT;

            switch (degrees)
            {
                case 90: new_orientation = DMDO_90; break;
                case 180: new_orientation = DMDO_180; break;
                case 270: new_orientation = DMDO_270; break;
                default: new_orientation = DMDO_DEFAULT; break;
            }

            DEVMODE dm = GetDevMode1(); 

            if (0 != EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref dm)) 
            { 
                
                dm.dmDisplayOrientation = new_orientation;

                int iRet = ChangeDisplaySettings(ref dm, CDS_TEST); 

                if (iRet == DISP_CHANGE_FAILED) 
                {
                    return;
                } 
                else 
                { 
                    iRet = ChangeDisplaySettings(ref dm, CDS_UPDATEREGISTRY); 
                    switch (iRet) 
                    { 
                        case DISP_CHANGE_SUCCESSFUL: 
                            { 
                                return; 
                            } 
                        case DISP_CHANGE_RESTART: 
                            { 
                                return; 
                            } 
                        default: 
                            { 
                                return; 
                            } 
                    } 
                }
            } 
            else 
            { 
                return; 
            } 
        } 

        private static DEVMODE GetDevMode1() 
        { 
            DEVMODE dm = new DEVMODE(); 
            dm.dmDeviceName = new String(new char[32]); 
            dm.dmFormName = new String(new char[32]);
            dm.dmSize = 158;// (short)Marshal.SizeOf(typeof(DEVMODE1)); 
            return dm; 
        } 
    } 
} 