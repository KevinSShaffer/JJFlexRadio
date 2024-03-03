using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JJTrace;
using adif;

namespace JJLogLib
{
    public partial class FieldDay : UserControl
    {
        public FieldDay(Logs.LogElement le)
        {
            InitializeComponent();

            // Setup the field dictionary.
            // These must be in the order of occurrence in the log.
            le.addField(new LogField(AdifTags.ADIF_DateOn, DateBox));
            le.addField(new LogField(AdifTags.ADIF_TimeOn, TimeBox));
            le.addField(new LogField(AdifTags.ADIF_Call, CallBox, "LogCall"));
            le.addField(new LogField(AdifTags.ADIF_Class, ClassBox));
            le.addField(new LogField(AdifTags.ADIF_ARRLSect, SectionCombo));
            le.addField(new LogField(AdifTags.ADIF_RXFreq, FreqBox));
            le.addField(new LogField(AdifTags.ADIF_Mode, ModeBox));
            le.addField(new LogField(AdifTags.ADIF_Band, BandBox));
            le.addField(new LogField(AdifTags.ADIF_SentSerial, QSOBox));
        }
    }
}
