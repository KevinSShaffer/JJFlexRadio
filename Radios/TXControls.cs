using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Flex.Smoothlake.FlexLib;
using Flex.Util;
using JJTrace;
using MsgLib;
using RadioBoxes;

namespace Radios
{
    public partial class TXControls : Form
    {
        private FlexBase rig;
        private Radio theRadio { get { return rig.theRadio; } }
        private Collection<Combo> combos;
        private Collection<NumberBox> numberBoxes;
        private ArrayList TXReqRCAList, TXReqAccList;

        private enum highLow
        {
            low,
            high
        }

        private class highLowElement
        {
            private highLow val;
            public string Display { get { return val.ToString(); } }
            public highLow RigItem { get { return val; } }
            public highLowElement(highLow v)
            {
                val = v;
            }
        }
        private ArrayList TXReqRCAPolarityList, TXReqAccPolarityList;

        private ArrayList TX1RCAList, TX2RCAList, TX3RCAList, TXAccList;
        private const int TXRCADelayMin = 0;
        private const int TXRCADelayMax = 500;
        private const int TXRCADelayIncrement = 1;
        private ArrayList alcList, remoteOnList;

        public TXControls(FlexBase r)
        {
            InitializeComponent();

            Tracing.TraceLine("TXControls", TraceLevel.Info);
            rig = r;
            DialogResult = DialogResult.None;
            combos = new Collection<Combo>();
            numberBoxes = new Collection<NumberBox>();

            // TXReqRCAEnabled
            TXReqRCAList = new ArrayList();
            TXReqRCAList.Add(new Flex6300Filters.trueFalseElement(false));
            TXReqRCAList.Add(new Flex6300Filters.trueFalseElement(true));
            TXReqRCAControl.TheList = TXReqRCAList;
            TXReqRCAControl.UpdateDisplayFunction =
                () => { return theRadio.TXReqRCAEnabled; };
            TXReqRCAControl.UpdateRigFunction =
                (object v) => { theRadio.TXReqRCAEnabled = (bool)v; };
            combos.Add(TXReqRCAControl);

            // TXReqRCAPolarity
            TXReqRCAPolarityList = new ArrayList();
            TXReqRCAPolarityList.Add(new highLowElement(highLow.low));
            TXReqRCAPolarityList.Add(new highLowElement(highLow.high));
            TXReqRCAPolarityControl.TheList = TXReqRCAPolarityList;
            TXReqRCAPolarityControl.UpdateDisplayFunction =
                () =>
                {
                    return (highLow)((theRadio.TXReqRCAPolarity) ? highLow.high : highLow.low);
                };
            TXReqRCAPolarityControl.UpdateRigFunction =
                (object v) =>
                {
                    theRadio.TXReqRCAPolarity = ((highLow)v == highLow.high);
                };
            combos.Add(TXReqRCAPolarityControl);

            // TXReqACCEnabled
            TXReqAccList = new ArrayList();
            TXReqAccList.Add(new Flex6300Filters.trueFalseElement(false));
            TXReqAccList.Add(new Flex6300Filters.trueFalseElement(true));
            TXReqAccConttrol.TheList = TXReqAccList;
            TXReqAccConttrol.UpdateDisplayFunction =
                () => { return theRadio.TXReqACCEnabled; };
            TXReqAccConttrol.UpdateRigFunction =
                (object v) => { theRadio.TXReqACCEnabled = (bool)v; };
            combos.Add(TXReqAccConttrol);

            // TXReqACCPolarity
            TXReqAccPolarityList = new ArrayList();
            TXReqAccPolarityList.Add(new highLowElement(highLow.low));
            TXReqAccPolarityList.Add(new highLowElement(highLow.high));
            TXReqAccPolarityControl.TheList = TXReqAccPolarityList;
            TXReqAccPolarityControl.UpdateDisplayFunction =
                () =>
                {
                    return (highLow)((theRadio.TXReqACCPolarity) ? highLow.high : highLow.low);
                };
            TXReqAccPolarityControl.UpdateRigFunction =
                (object v) =>
                {
                    theRadio.TXReqACCPolarity = ((highLow)v == highLow.high);
                };
            combos.Add(TXReqAccPolarityControl);

            // TX1RCAEnabled
            TX1RCAList = new ArrayList();
            TX1RCAList.Add(new Flex6300Filters.trueFalseElement(false));
            TX1RCAList.Add(new Flex6300Filters.trueFalseElement(true));
            TX1RCAControl.TheList = TX1RCAList;
            TX1RCAControl.UpdateDisplayFunction =
                () => { return theRadio.TX1Enabled; };
            TX1RCAControl.UpdateRigFunction =
                (object v) => { theRadio.TX1Enabled = (bool)v; };
            combos.Add(TX1RCAControl);

            TX1RCADelayControl.LowValue = TXRCADelayMin;
            TX1RCADelayControl.HighValue = TXRCADelayMax;
            TX1RCADelayControl.Increment = TXRCADelayIncrement;
            TX1RCADelayControl.UpdateDisplayFunction =
                () => { return theRadio.TX1Delay; };
            TX1RCADelayControl.UpdateRigFunction =
                (int v) => { theRadio.TX1Delay = v; };
            numberBoxes.Add(TX1RCADelayControl);

            // TX2RCAEnabled
            TX2RCAList = new ArrayList();
            TX2RCAList.Add(new Flex6300Filters.trueFalseElement(false));
            TX2RCAList.Add(new Flex6300Filters.trueFalseElement(true));
            TX2RCAControl.TheList = TX2RCAList;
            TX2RCAControl.UpdateDisplayFunction =
                () => { return theRadio.TX2Enabled; };
            TX2RCAControl.UpdateRigFunction =
                (object v) => { theRadio.TX2Enabled = (bool)v; };
            combos.Add(TX2RCAControl);

            TX2RCADelayControl.LowValue = TXRCADelayMin;
            TX2RCADelayControl.HighValue = TXRCADelayMax;
            TX2RCADelayControl.Increment = TXRCADelayIncrement;
            TX2RCADelayControl.UpdateDisplayFunction =
                () => { return theRadio.TX2Delay; };
            TX2RCADelayControl.UpdateRigFunction =
                (int v) => { theRadio.TX2Delay = v; };
            numberBoxes.Add(TX2RCADelayControl);

            // TX3RCAEnabled
            TX3RCAList = new ArrayList();
            TX3RCAList.Add(new Flex6300Filters.trueFalseElement(false));
            TX3RCAList.Add(new Flex6300Filters.trueFalseElement(true));
            TX3RCAControl.TheList = TX3RCAList;
            TX3RCAControl.UpdateDisplayFunction =
                () => { return theRadio.TX3Enabled; };
            TX3RCAControl.UpdateRigFunction =
                (object v) => { theRadio.TX3Enabled = (bool)v; };
            combos.Add(TX3RCAControl);

            TX3RCADelayControl.LowValue = TXRCADelayMin;
            TX3RCADelayControl.HighValue = TXRCADelayMax;
            TX3RCADelayControl.Increment = TXRCADelayIncrement;
            TX3RCADelayControl.UpdateDisplayFunction =
                () => { return theRadio.TX3Delay; };
            TX3RCADelayControl.UpdateRigFunction =
                (int v) => { theRadio.TX3Delay = v; };
            numberBoxes.Add(TX3RCADelayControl);

            // TXACCEnabled
            TXAccList = new ArrayList();
            TXAccList.Add(new Flex6300Filters.trueFalseElement(false));
            TXAccList.Add(new Flex6300Filters.trueFalseElement(true));
            TXAccControl.TheList = TXAccList;
            TXAccControl.UpdateDisplayFunction =
                () => { return theRadio.TXACCEnabled; };
            TXAccControl.UpdateRigFunction =
                (object v) => { theRadio.TXACCEnabled = (bool)v; };
            combos.Add(TXAccControl);

            TXAccDelayControl.LowValue = TXRCADelayMin;
            TXAccDelayControl.HighValue = TXRCADelayMax;
            TXAccDelayControl.Increment = TXRCADelayIncrement;
            TXAccDelayControl.UpdateDisplayFunction =
                () => { return theRadio.TXACCDelay; };
            TXAccDelayControl.UpdateRigFunction =
                (int v) => { theRadio.TXACCDelay = v; };
            numberBoxes.Add(TXAccDelayControl);

            // HWALCEnabled
            alcList = new ArrayList();
            alcList.Add(new Flex6300Filters.trueFalseElement(false));
            alcList.Add(new Flex6300Filters.trueFalseElement(true));
            ALCControl.TheList = alcList;
            ALCControl.UpdateDisplayFunction =
                () => { return theRadio.HWAlcEnabled; };
            ALCControl.UpdateRigFunction =
                (object v) => { theRadio.HWAlcEnabled = (bool)v; };
            combos.Add(ALCControl);

            // RemoteOnEnabled
            remoteOnList = new ArrayList();
            remoteOnList.Add(new Flex6300Filters.trueFalseElement(false));
            remoteOnList.Add(new Flex6300Filters.trueFalseElement(true));
            RemoteOnControl.TheList = remoteOnList;
            RemoteOnControl.UpdateDisplayFunction =
                () => { return theRadio.RemoteOnEnabled; };
            RemoteOnControl.UpdateRigFunction =
                (object v) => { theRadio.RemoteOnEnabled = (bool)v; };
            combos.Add(RemoteOnControl);

            foreach (Combo c in combos)
            {
                c.BoxKeydown += BoxKeydownHandler;
                c.UpdateDisplay(true);
            }

            foreach (NumberBox c in numberBoxes)
            {
                c.BoxKeydown += BoxKeydownHandler;
                c.UpdateDisplay(true);
            }
        }

        private void BoxKeydownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}
