using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using JJTrace;

namespace Radios
{
    internal class ElecraftK3 : Elecraft
    {
        // Region - capabilities
        #region capabilities
        private RigCaps.Caps[] capsList =
        {
            RigCaps.Caps.AFGet,
            RigCaps.Caps.AFSet,
            RigCaps.Caps.AGGet,
            RigCaps.Caps.AGSet,
            RigCaps.Caps.AGTimeGet,
            RigCaps.Caps.AGTimeSet,
            RigCaps.Caps.ALCGet,
            RigCaps.Caps.ANGet,
            RigCaps.Caps.ANSet,
            RigCaps.Caps.ATGet,
            RigCaps.Caps.ATSet,
            RigCaps.Caps.BCGet,
            RigCaps.Caps.BCSet,
            RigCaps.Caps.CLGet,
            RigCaps.Caps.CLSet,
            RigCaps.Caps.CTSSFreqGet,
            RigCaps.Caps.CTSSFreqSet,
            RigCaps.Caps.CTModeGet,
            RigCaps.Caps.CTModeSet,
            RigCaps.Caps.CWAutoTuneGet,
            RigCaps.Caps.CWAutoTuneSet,
            RigCaps.Caps.CWDelayGet,
            RigCaps.Caps.CWDelaySet,
            RigCaps.Caps.DMGet,
            RigCaps.Caps.DMSet,
            RigCaps.Caps.EQRGet,
            RigCaps.Caps.EQRSet,
            RigCaps.Caps.EQTGet,
            RigCaps.Caps.EQTSet,
            RigCaps.Caps.FrGet,
            RigCaps.Caps.FrSet,
            RigCaps.Caps.FSGet,
            RigCaps.Caps.FSSet,
            RigCaps.Caps.FWGet,
            RigCaps.Caps.FWSet,
            RigCaps.Caps.IDGet,
            RigCaps.Caps.KSGet,
            RigCaps.Caps.KSSet,
            RigCaps.Caps.LKGet,
            RigCaps.Caps.LKSet,
            RigCaps.Caps.MemGet,
            RigCaps.Caps.MemSet,
            RigCaps.Caps.MGGet,
            RigCaps.Caps.MGSet,
            RigCaps.Caps.ModeGet,
            RigCaps.Caps.ModeSet,
            RigCaps.Caps.NBGet,
            RigCaps.Caps.NBSet,
            RigCaps.Caps.NFGet,
            RigCaps.Caps.NFSet,
            RigCaps.Caps.NTGet,
            RigCaps.Caps.NTSet,
            RigCaps.Caps.PAGet,
            RigCaps.Caps.PASet,
            RigCaps.Caps.RAGet,
            RigCaps.Caps.RASet,
            RigCaps.Caps.RFGet,
            RigCaps.Caps.RFSet,
            RigCaps.Caps.RITGet,
            RigCaps.Caps.RITSet,
            RigCaps.Caps.SMGet,
            RigCaps.Caps.SPGet,
            RigCaps.Caps.SPSet,
            RigCaps.Caps.SQGet,
            RigCaps.Caps.SQSet,
            RigCaps.Caps.SWRGet,
            RigCaps.Caps.TOGet,
            RigCaps.Caps.TOSet,
            RigCaps.Caps.TXITGet,
            RigCaps.Caps.TXITSet,
            RigCaps.Caps.TXMonGet,
            RigCaps.Caps.TXMonSet,
            RigCaps.Caps.VDGet,
            RigCaps.Caps.VDSet,
            RigCaps.Caps.VFOGet,
            RigCaps.Caps.VFOSet,
            RigCaps.Caps.VGGet,
            RigCaps.Caps.VGSet,
            RigCaps.Caps.VSGet,
            RigCaps.Caps.VSSet,
            RigCaps.Caps.XFGet,
            RigCaps.Caps.XFSet,
            RigCaps.Caps.XPGet,
            RigCaps.Caps.XPSet,
            RigCaps.Caps.Pan,
            RigCaps.Caps.ManualTransmit
        };
        #endregion

        // region - rig-specific properties
        #region RigSpecificProperties
        public override int RigID
        {
            get { return RadioSelection.RIGIDElecraftK3; }
        }

        public override OffOnValues Vox
        {
            get
            {
                bool sw;
                if ((TXMode == myModeTable[(int)modes.cw]) ||
                    (TXMode == myModeTable[(int)modes.cwr])) sw = b_CWVox;
                else sw = b_PhoneVox;
                return (sw) ? OffOnValues.on : OffOnValues.off;
            }
            set
            {
                bool sw = (value == OffOnValues.on) ? true : false;
                if (((TXMode == myModeTable[(int)modes.cw]) ||
                    (TXMode == myModeTable[(int)modes.cwr])))
                {
                    sw = ((sw && !b_CWVox) || (!sw && b_CWVox));
                }
                else
                {
                    sw = ((sw && !b_PhoneVox) || (!sw && b_PhoneVox));
                }
                if (sw) Callouts.safeSend(BldCmd(ecmdSWH + "09"));
            }
        }

        /// <summary>
        /// QSK on/off
        /// </summary>
        internal OffOnValues QSK
        {
            get
            {
                return (b_CWQSK) ? OffOnValues.on : OffOnValues.off;
            }
            set
            {
                bool sw = (value == OffOnValues.on) ? true : false;
                sw = ((sw && !b_CWQSK) || (!sw && b_CWQSK));
                if (sw) Callouts.safeSend(BldCmd(ecmdSWH + "10"));
            }
        }

        #endregion

        private static string[] myStatCommands =
        {
            ecmdK3 + "1", // K3 extended mode
            ecmdFA,
            ecmdFB,
            ecmdDB,
            ecmdDS,
            ecmdAG,
            ecmdAG + '$',
            ecmdGT,
            ecmdIC,
            ecmdIF,
            ecmdKS,
            ecmdRG,
            ecmdRG + '$',
        };

        public ElecraftK3()
        {
            Tracing.TraceLine("Elecraft K3 constructor", TraceLevel.Info);
            myCaps = new RigCaps(capsList);
            statCommands = myStatCommands;
        }

        public override bool Open(OpenParms p)
        {
            Tracing.TraceLine("Elecraft K3 open", TraceLevel.Info);
            bool rv = base.Open(p);
            IsOpen = rv;
            if (rv) new K3Filters(this);
            return rv;
        }

        public override void close()
        {
            base.close();
        }

        // region - menu stuff
    }
}
