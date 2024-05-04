using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Windows.Forms;
using JJTrace;

namespace Radios
{
    internal partial class K3Menus : Form
    {
        private class menuType
        {
            public int Number { get; set; }
            public string Title { get; set; }
            public string Description;
            public bool Tech;
            public menuType(int n, string t, string d)
            {
                setup(n, false, t, d);
            }
            public menuType(int n, bool te, string t, string d)
            {
                setup(n, te, t, d);
            }
            private void setup(int n, bool te, string t, string d)
            {
                Number = n;
                Title = t;
                Description = d;
                Tech = te;
            }
        }
        private static menuType[] menus =
        {
new menuType(72, "TECH MD",
"TECH MD: OFF: Set to ON to enable Tech Mode menu entries (those marked with '(tech)' in this list)."),
new menuType(0, "Alarm",
"ALARM: OFF: Set alarm/Auto-Power-On time. Tap 1 to turn alarm on/off; tap 2 / 3 to set HH / MM."),
new menuType(2, "LCD ADJ",
"LCD ADJ: 8: LCD viewing angle and contrast. Use higher settings if the radio is used at or above eye level. If adjusted incorrectly, bargraphs will be too light or heavy during keying."),
new menuType(3, "LCD BRT",
"LCD BRT: 6: LCD backlight brightness. Use DAY in bright sunlight, 2 to 8 for indoor lighting."),
new menuType(4, "LED BRT",
"LED BRT: 4: LED brightness (relative to LCD backlight brightness). Exception: if LCD BRT is set to DAY, LEDs are set to their maximum brightness."),
new menuType(53, "mic sel",
"MIC SEL FP, low Mic/line transmit audio source, mic gain range, and mic bias. Source selections: range, FP (front panel 8-pin <MIC> jack), RP (rear panel 3.5 mm <MIC> jack), and LIN E IN bias on (rear-panel LINE IN jack). Tap 1 to toggle between .Low and .High mic gain range for the selected mic. Tap 2 to turn mic BIAS on/off (turn on for electret mics)."),
new menuType(5, "MSG RPT",
"MSG RPT: 6: Message repeat interval in seconds (0 to 255). To repeat a message, hold <M1> – <M4> rather than tap. A 6 - 10 sec. interval is about right for casual CQing. Shorter intervals may be needed during contests, and longer for periodic CW beacons."),
new menuType(7, "RPT OFS",
"RPT OFS: 2400: Sets the transmit offset (in kHz) for repeater operation, from –2560 kHz to +2540 kHz. Per VHF band or band segment."),
new menuType(8, "RX EQ",
"RX EQ: +0 dB: Receiver audio graphic equalizer. <VFO A> is used as an 8-band bargraph, where each each character shows the boost or cut (-16 dB to +16 dB in 1 dB increments) for a given band <AF> band. The 8 bands are 0.05, 0.1, 0.2, 0.4, 0.8, 1.2, 2.4 and 3.2 kHz.  Tap 1 -8 to select an <AF> band. <VFO A> selects boost/cut. Tap <CLR> to reset all bands to +0 dB."),
new menuType(9, "TX EQ",
"TX EQ: +0 dB: Transmit audio graphic equalizer (voice modes only). Functions the same as RX EQ, TX* EQ each above, and can be adjusted while in transmit mode. TX*EQ indicates TX ESSB in band effect, which has its own set of transmit equalization settings."),
new menuType(10, "VOX GN",
"VOX GN: 0: Adjusts the sensitivity of the VOX to match your mic and voice."),
new menuType(11, "ANTIVOX",
"ANTIVOX: 0: Adjusts immunity of the VOX circuit to false triggering as a result of audio from the speaker or 'leaked' from headphones."),
new menuType(13, true, "2 TONE (tech)",
"2 TONE: (Troubleshooting.) Turns on built-in 2-tone generator for SSB transmit tests.  The internal 2-tone generator only works if LSB mode is selected."),
new menuType(95, "AF GAIN OFF",
"AF GAIN OFF: LO: Sets AF GAIN range. Available selections are HI or LO."),
new menuType(14, true, "AFV TIM (tech)",
"AFV TIM: (Advanced.) Integration time for AFV and dBV displays in ms. See <VFO B> alternate displays and K3 Service Manual."),
new menuType(20, true, "AGC HLD (tech)",
"AGC HLD: 1000: (Advanced.) AGC “hold” time for voice modes. Specifies the number of milliseconds that the SLOW AGC value is held after the signal drops below the level that set the AGC. This is often helpful for SSB voice operation."),
new menuType(99, true, "AGC PLS (tech)",
"AGC PLS: 000: NOR: (Advanced.) NOR enables AGC noise pulse rejection."),
new menuType(17, true, "AGC SLP (tech)",
"AGC SLP: (Advanced.) Higher values result in ‘flatter’ AGC (making signals at all amplitudes closer in <AF> output level)."),
new menuType(74, true, "AGC THR (tech)",
"AGC THR: 10: 5 (Advanced.) Sets AGC onset point; a higher number moves the onset up."),
new menuType(61, true, "AGC-F (tech)",
"AGC-F: 120: (Advanced.) Sets fast AGC decay rate; a higher number means faster decay."),
new menuType(37, true, "AGC-S (tech)",
"AGC-S: 20: (Advanced.) Sets slow AGC decay rate; a higher number means faster decay."),
new menuType(24, "BAT MIN",
"BAT MIN: 11.0: Low-battery warning threshold; 11.0 recommended. The parameter flashes if it is set higher than the present supply voltage."),
new menuType(1, "CW IAMB",
"CW IAMB: A: Iambic keying mode (A or B). Both modes produce self-completing dots and dashes. Mode B is more efficient for operators who practice “squeeze” keying (pressing both paddles at once), because an extra dot or dash is inserted on squeeze release. Mode A lacks this feature, and may be more appropriate for those who only press one paddle at a time (often called “slap” keying)."),
new menuType(6, "CW PADL",
"CW PADL: Tip=dot: Specifies whether left keyer paddle (“tip” contact on the plug) is DOT or DASH."),
new menuType(112, "CW QRQ",
"CW QRQ: OFF: If on, keying speed may be increased to 100 wpm."),
new menuType(12, "CW WGHT",
"CW WGHT: 1.00: CW keying weight. Adjusts element/space timing ratio for the internal keyer."),
new menuType(29, "DATE",
"DATE: N/A: Real-time-clock date, shown as in the format selected by 'CONFIG:DATE MD' (MM.DD.YY or DD.MM.YY). Tap 1 / 2 / 3 to select month / day / year."),
new menuType(30, "DATE MD",
"DATE MD: US: Select US (MM.DD.YY) or EU (DD.MM.YY) date formats."),
new menuType(31, true, "DDS FRQ (tech)",
"DDS FRQ: {DDS: (Troubleshooting.) Controls DDS tuning directly to check DDS <XFIL> range for freq synthesizer troubleshooting purposes. Rotate <VFO A> CCW and CW to find limits where L (lock) changes to U (unlock). Correct DDS frequency is restored after exiting the menu and rotating either VFO."),
new menuType(19, "DIGOUT{n}",
"DIGOUT{n}: LO: Directly controls two general-purpose digital output lines on the AUX I/O connector. {n} = 0 or 1. Tap 1 to alternate between the two. These are TTL-level signals with a nominal output voltage of 0 V (LO) or 5 V (HI).  Recommended max. load is 15 mA (enough to power one LED, for example)."),
new menuType(38, "FLx BW (Sub)",
"FLx BW: 2.70 (FL1): Crystal filter FL1-5 bandwidth in kHz, where x=1 to 5 (FL1-FL5). Tap 1 -5 or <XFIL> to select filters."),
new menuType(39, "FLx FRQ (SUB)",
"FLx FRQ: 0.00 (FL1): Crystal filter FLx center freq as offset from nominal (8215.0 kHz).  Use the offset value specified on the crystal filter’s label or PC board, if any. For example, if an Elecraft 5-pole, 200-Hz filter were labeled “-0.91”, adjust <VFO A> for –0.91."),
new menuType(40, "FLx GN (Sub)",
"FLx GN: 0 dB (FL1): Crystal filter FLx loss compensation in dB. 0 dB recommended for wide filters; 2 dB for 400 or 500 Hz filters, and 4 dB for 200 or 250 Hz filters."),
new menuType(41, "FLx ON (Sub)",
"FLx ON: ON (FL1), per-mode: Used to specify which filters are available during receive. Each filter must be set to ON or OFF in each mode. You can tap <MODE> within the menu entry."),
new menuType(42, "FLTX{md}",
"FLTX{md}: FL1 (all modes): Used to specify which crystal filter to use during TX. {md} = CW/SB/AM/FM.  Choose filters with bandwidths as follows: SSB, 2.7 or 2.8 kHz (also applies to data); CW, 2.7 or 2.8 kHz; AM, 6 kHz; FM, 12 kHz or higher.\r\nNote: If you’re using a 2.7-kHz 5-pole filter for SSB transmit, you can optionally fine-tune its FLx FRQ parameter to equalize LSB / USB transmit characteristics.  Monitor your signal on a separate receiver, using headphones."),
new menuType(43, "FP TEMP",
"FP TEMP: N/A: Used to calibrate the front panel temperature sensor. It must be calibrated if you wish to use the REF xxC menu entry to calibrate the optional 1 PPM TCXO. You must convert °F to °C in order to enter the value. Deg. C = (deg. F - 32) * 0.555."),
new menuType(44, "FSK POL",
"FSK POL: 1: 0 = Invert FSK transmit data polarity, 1 = Normal data polarity."),
new menuType(28, "FW REVS",
"FW REVS: N/A: Rotate <VFO A> to see firmware revisions of the MCU (uC), main DSP (d1), aux DSP (d2), flash parameters (FL), and KDVR3 controller (dr)."),
new menuType(23, "KAT3",
"KAT3: Not Inst: KAT3 <ATU> mode; normally set to BYP or AUTO (you can alternate between these settings using the <ATU> switch). Modes L1-L8, C1-C8, and Ct are used to test KAT3 relays. Mode LCSET allows manual adjustment of L/C/net settings.  When in this mode, tapping <ATU> <TUNE> shows the L & C value; C is changed with <VFO A>, L is changed with <VFO B>, and ANT toggles between Ca and Ct."),
new menuType(46, "KBPF3",
"KBPF3: Not Inst: If KBPF3 option is installed: set to ON, exit menu, and turn power off/on."),
new menuType(36, "KDVR3",
"KDVR3: Not Inst: If KDVR3 option is installed: set to ON, exit menu, and turn power off/on."),
new menuType(33, "KIO3",
"KIO3: NOR: Determines function of BAND0-3 outputs on AUX I/O connector. See pg. 19."),
new menuType(48, "KNB3",
"KNB3: Not Inst If KNB3 option is installed: set to ON, exit menu, and turn power off/on.\r\nNote: the K3 can’t be used without a KNB3; Not Inst is for troubleshooting only."),
new menuType(55, "KPA3",
"KPA3: Not Inst: Set to PA NOR if KPA3 100-W amp installed. Set to PAIO NOR if KPA3 is not installed, but the KPAIO3 transition PC board is. Other settings include PA BYP (disables KPA3 if installed), PA fan test settings (PA FN1-FN4 or PA IO FN1-FN4), and PAIO BYP (if transition board is installed, but not the KPA3 module, this setting can be used to test the high power bypass relay)."),
new menuType(49, "KRC2",
"KRC2: - Controls the KRC2 band decoder’s accessory output settings. Shows OFF or ACC 1-3 if a KRC2 is detected; --if not."),
new menuType(50, "KRX3",
"KRX3: Not Inst: If KRX3 option (sub receiver) is installed, set parameter to NOR-ANTx where x is your selected wiring for the sub receiver’s auxiliary antenna (1 = unused KAT3 ATU antenna, 2 = AUX RF jack, 3 = AUX RF in parallel with the <RX ANT> IN jack on the KXV3). For details on sub receiver antenna sources, see pg. 22.  Note: the sub receiver option includes three modules: receiver, synthesizer, and DSP. All three must be installed, or the K3 will report an error on power-up."),
new menuType(51, "KXV3",
"KXV3: Not Inst: If KXV3 option is installed: set to ON, exit menu, and turn power off/on. This option is required for use of RXAN T and low-level transverter I/O."),
new menuType(52, "LCD TST",
"LCD TST: OFF: Changing the parameter turns on all LCD segments."),
new menuType(32, "LIN OUT",
"LIN OUT: 30: Sets the LINE OUT level. LINE OUT connections go to PC soundcard inputs."),
new menuType(54, "NB SAVE",
"NB SAVE: NO: Set to YES to save noise blanker on/off state per-band. Noise blanker levels, both DSP and I.F., are always saved per-band regardless of this setting."),
new menuType(56, "PA TEMP",
"PA TEMP: N/A: If a KPA3 (100-W PA module) is installed, shows KPA3 heatsink temperature and allows it to be adjusted. See calibration procedure on page 48."),
new menuType(103, "PTT-KEY",
"PTT-KEY: OFF-OFF: Allows selection of RTS or DTR RS232 lines to activate PTT or key the K3. See pg. 18. Note: If a computer or other device asserts RTS or DTR while you’re in this menu entry, the K3 will switch to TEST mode (zero power output) as a precaution. The TX icon will flash as a reminder. To avoid this, make sure software applications have flow control and/or keying options turned OFF while you’re changing the PTT-KEY selection."),
new menuType(62, "REF CAL",
"REF CAL: or 49380000: Used to precisely calibrate the K3’s reference oscillator,.  <VFO A> is used to set the REF xxC Hz reference oscillator frequency in Hz. Tap 1 to alternate between REF CAL and [T] REF xxC. xx is a data entry point from –20 to +70.  REF CAL can be used with either TCXO (see Method 1 or 2, pg. 47).  REF xxC is used with the 1 ppm TCXO (Method 3). Tap 2 or 3 to move the data entry point up or down. See calibration procedure, pg. 47."),
new menuType(57, "RS232",
"RS232: 4800 b: RS232 communications rate in bits per second (bps). During firmware download (via the K3FW PC program), the baud rate is set automatically to 38400 baud, but it is then restored to the value selected in this menu entry."),
new menuType(26, "SER NUM",
"SER NUM: N/A: K3 serial number, e.g. 02000. Cannot be edited from the menu."),
new menuType(65, "SMTR OF",
"SMTR OF: 024: S-Meter offset; see calibration procedure (pg. 48)."),
new menuType(66, "SMTR SC",
"SMTR SC: 014: S-Meter scale; S-9 = 50 uV, S=3 = 1 uV with Preamp = ON, and AGC ON.  See calibration procedure (pg. 48)."),
new menuType(67, "SMTR PK",
"SMTR PK: OFF: Set to ON for peak-reading S-meter."),
new menuType(68, "SPLT SV",
"SPLT SV: NO: If set to YES, <SPLIT> state is saved per-band."),
new menuType(69, "SPKRS",
"SPKRS: 1: Set to 2 if using two external speakers. This enables binaural effects in conjunction with the <AFX> switch, as well as stereo dual-receive if the sub receiver is installed. For further details, see pg. 33."),
new menuType(97, "SPKR+PH",
"SPKR+PH: NO: YES = Speaker is ON, even when headphones are plugged into PHONES jack.  See detailed discussion on pg. 19."),
new menuType(63, "SQ MAIN",
"SQ MAIN: 0: Main receiver squelch value or RF gain potentiometer assignment. If set to RF INN ER, the inner (smaller) RF/SQL knob controls main squelch rather than RF gain. If set to RF OUTER, the outer ring (larger) RF/SQL knob controls main squelch rather than sub receiver RF gain."),
new menuType(64, "SQ SUB",
"SQ SUB: 0: Sub receiver squelch value or RF gain pot assignment. If set to RF OUTER, the outer ring (larger) RF/SQL knob controls sub squelch rather than sub RF gain."),
new menuType(70, true, "SW TEST (tech)",
"SW TEST: OFF: Changing the parameter displays SCN ADC. Hold any switch to see scan row and switch ADC reading. Used for troubleshooting only."),
new menuType(71, "SW TONE",
"SW TONE: OFF: Sets up switch tones or audio Morse feedback on any control activation."),
new menuType(73, "TIME",
"TIME: N/A: Real-time-clock view/set. Tap 1 / 2 / 3 to set HH / MM / SS. To see the time and other displays during normal operation, tap <DISP> (see pg. 34)."),
new menuType(77, "TTY LTR",
"TTY LTR: Performs an RTTY FIGS to LTRS shift when the text decoder is enabled in RTTY modes. Cannot be changed within the menu itself; must be assigned to a programmable function switch."),
new menuType(78, true, "TX ALC (tech)",
"TX ALC: (Troubleshooting.) Set to OFF to disable transmit ALC in CW mode.  This is required when adjusting band-pass filters and can also be used for troubleshooting, but should not be used during normal operation. (Be sure to set parameter to ON afterward.)"),
new menuType(96, true, "TX ESSB (tech)",
"TX ESSB: OFF: Extended SSB transmit bandwidth (3.0, 3.5, 4.0 kHz, etc.) or OFF. See pg. 44."),
new menuType(79, true, "TXGN pwr (tech)",
"TXGN pwr (Troubleshooting.): 00: Shows transmit gain constant for the present band and power mode, where {pwr} = LP (0-12W), HP (15-120W), or MW (0-1.5 mW).  The gain constant is updated whenever the <TUNE> function is activated on a given band at one of three specific power levels: 5.0 W, 50 W, and 1.00 milliwatt. See transmit gain calibration procedure, pg. 46.  If TX ALC (above) is OFF, the TXGN parameter can be set manually, at very fine resolution. This should only be done for troubleshooting purposes."),
new menuType(27, true, "TXG VCE (tech)",
"TXG VCE: 0.0 dB: Balances voice transmit peak power in relation to CW peak  power in <TUNE> mode. Typically left at 0.0."),
new menuType(83, true, "VCO MD (tech)",
"VCO MD: 064: (Troubleshooting.) VCO L-C range view/change/calibrate. Once the VCO is calibrated (pg. 46), the parameter which appears here will include NOR at all times. You can change the setting to troubleshoot VCO L-C ranges.  When finished, set the parameter back to NOR 127, then exit the menu and change bands to restore the original setting."),
new menuType(98, "VFO B->A",
"VFO B->A: Copies <VFO B>’s frequency to <VFO A>. Cannot be used within the menu itself; must be assigned to a programmable function switch."),
new menuType(104, "VFO CRS",
"VFO CRS: Per-mode coarse tuning rate (hold COARSE and tune <VFO A> or B). Also applies to the <RIT>/<XIT> tuning knob if 'CONFIG:VFO OFS' is set to ON, and both <RIT> and <XIT> are turned OFF."),
new menuType(84, "VFO CTS",
"VFO CTS Per-mode: 200: VFO counts per turn (100, 200, or 400). Smaller values result in easier fine- tuning of VFO; larger values result in faster QSY."),
new menuType(85, "VFO FST",
"VFO FST: Specifies the faster of the two VFO tuning rates (<RATE> ). The faster rate is 50 Hz per step by default, but can be set to 20 Hz if desired. In this case, VFO CTS = 400 is recommended to ensure adequate fast-QSY speed."),
new menuType(86, "VFO IND",
"VFO IND: 50 Hz: If set to YES, <VFO B> can be set to a different band than <VFO A>, which allows listening to two bands at once (main/sub). This menu entry is not available unless the subreceiver is installed."),
new menuType(87, "VFO OFS",
"VFO OFS: NO: If ON, the <RIT>/<XIT> offset control can be used to tune <VFO A> in large steps when both <RIT> and <XIT> are turned off. The step sizes vary with mode (see VFO CRS), and are the same as the COARSE VFO tuning rates."),
new menuType(88, "WMTR pwr",
"WMTR {pwr}: 100: OFF: Wattmeter calibration parameter. {pwr} is the power mode: LP (0-12W), HP (15-120W), or MW (0-1.5 mW). See calibration procedure (pg. 46)."),
new menuType(89, "XVx ON",
"XVx ON: NO: Set to YES to turn on transverter band x (1-9); tap 1 – 9 to select xvtr band."),
new menuType(90, "XVx RF",
"XVx RF: 144: Lower edge for xvtr band x (1-9); 0-999 MHz. Tap 1 – 9 to select xvtr band."),
new menuType(91, "XVx IF",
"XVx IF: 28: Specify K3 band to use as the I.F. for transverter band x (1-9). Tap 1 – 9 to select xvtr band. I.F. band selections include 7, 14, 21, 28, and 50 MHz."),
new menuType(92, "XVx PWR",
"XVx PWR: H: 0.1 Allows fixed or variable power level for XVTR band x. Tap 1 – 9 to select xvtr band. H x.x (High power level) specifies a value in watts, and use of the main antenna jack(s). This should be used with caution, as you could damage a transverter left connected to these antenna jacks accidentally. L x.x (Low power level) species a value in milliwatts, which requires the KXV3 option."),
new menuType(93, "XVx OFS",
"XVx OFS: 0.00: Offset (–9.99 to +9.99 kHz) for transverter band x (1-9). Tap 1 – 9 to select xvtr band. Compensates for oscillator/multiplier chain errors."),
new menuType(94, "XVx ADR",
"XVx ADR: TRN1: Physical decode address (1 to 9) assigned to transverter band x (1-9). Tap 1 – 9 to select xvtr band. Applies to attached Elecraft XV-series transverters and Elecraft KRC2. Also see 'CONFIG:KIO3'.\r\nNote: Decode address range may vary depending on the type of attached device."),
        };

        private bool wasActive;
        private ArrayList items;

        private ElecraftK3 rig;
        private System.Timers.Timer watchTimer;
        private const int timerInterval = 250;

        private const string techmdTest = "TECH MD";
        private bool techmdRead;
        private bool old_techmd;
        private bool techmd;

        internal K3Menus(ElecraftK3 r)
        {
            Tracing.TraceLine("K3Menus", TraceLevel.Info);
            InitializeComponent();

            rig = r;
        }

        private void K3Menus_Load(object sender, EventArgs e)
        {
            Tracing.TraceLine("K3Menus_Load", TraceLevel.Info);
            wasActive = false;

            // MenuList is setup when MenuList is entered.

            // Watch the display.
            watchTimer = new System.Timers.Timer();
            watchTimer.Interval = timerInterval;
            watchTimer.AutoReset = true;
            watchTimer.Elapsed += new ElapsedEventHandler(watcher);
            watchTimer.Enabled = true;

            // Wait to see if tech menus are on.
            techmdRead = false;
            rig.menuNumber = 72;
            while (!techmdRead)
            {
                Thread.Sleep(timerInterval);
            }        
            Tracing.TraceLine("k3menu_load:" + techmd.ToString());
            rig.menuNumber = Elecraft.noMenu;
        }

        private void setupMenuList(bool justStarted)
        {
            Tracing.TraceLine("k3menu setup:" + justStarted.ToString() + " " +
                techmd.ToString() + " " + old_techmd.ToString(), TraceLevel.Info);
            // Setup if we've just started or the tech menu value has changed.
            if (justStarted || (old_techmd != techmd))
            {
                MenuList.SuspendLayout();
                old_techmd = techmd;
                // Initialize the list
                items = new ArrayList();
                foreach (menuType m in menus)
                {
                    if (old_techmd || !m.Tech) items.Add(m);
                }
                MenuList.DisplayMember = "Title";
                MenuList.ValueMember = "Number";
                MenuList.DataSource = items;
                MenuList.ResumeLayout();
            }
            MenuList_SelectedIndexChanged(null, null);
        }

        private void MenuList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Tracing.TraceLine("MenuList_SelectedIndexChanged", TraceLevel.Info);
            int id = MenuList.SelectedIndex;
            if (id >= 0)
            {
                rig.menuNumber = (int)MenuList.SelectedValue;
                DescriptionBox.Text = menus[id].Description;
            }
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private delegate void del();
        private void watcher(object s, ElapsedEventArgs e)
        {
            Tracing.TraceLine("k3menu watcher:" + rig.menuNumber.ToString(), TraceLevel.Info);
            if (rig.menuNumber == 72)
            {
                techmd = rig.VFOADispOn;
                techmdRead = true;
                Tracing.TraceLine("k3menu watcher:techmd=" + techmd.ToString(), TraceLevel.Info);
            }
            del d = () => ValueBox.Text = rig.VFOADisplay;
            ValueBox.Invoke(d);
        }

        private void DescriptionBox_Enter(object sender, EventArgs e)
        {
            DescriptionBox.SelectionStart = 0;
        }

        private void K3Menus_Activated(object sender, EventArgs e)
        {
            Tracing.TraceLine("K3Menus_Activated", TraceLevel.Info);
            if (!wasActive)
            {
                wasActive = true;
                if (MenuList.Focused)
                {
                    setupMenuList(true);
                }
                else
                {
                    // MenuList_Enter calls setupMenuList
                    MenuList.Focus();
                }
            }
        }

        private void MenuList_Enter(object sender, EventArgs e)
        {
            Tracing.TraceLine("MenuList_Enter", TraceLevel.Info);
            // Note this can be called before K3Menus_Activated.
            setupMenuList(!wasActive);
        }

        private void K3Menus_FormClosing(object sender, FormClosingEventArgs e)
        {
            Tracing.TraceLine("K3Menus_FormClosing", TraceLevel.Info);
            watchTimer.Enabled = false;
            Thread.Sleep(timerInterval);
            watchTimer.Dispose();
            rig.menuNumber = Elecraft.noMenu;
        }
    }
}
