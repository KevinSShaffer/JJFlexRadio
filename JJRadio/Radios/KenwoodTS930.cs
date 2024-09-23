using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using JJTrace;

namespace Radios
{
    /// <summary>
    /// Kenwood TS-930
    /// </summary>
    class KenwoodTS930 : Kenwood
    {
        private RigCaps.Caps[] capsList =
        {
            RigCaps.Caps.FrGet,
            RigCaps.Caps.FrSet,
            RigCaps.Caps.MemGet,
            RigCaps.Caps.MemSet,
            RigCaps.Caps.RITGet,
            RigCaps.Caps.RITSet,
            RigCaps.Caps.SMGet,
            RigCaps.Caps.VFOGet,
            RigCaps.Caps.XFGet,
            RigCaps.Caps.XFSet
        };
        public KenwoodTS930()
        {
            Tracing.TraceLine("KenwoodTS930 constructor", TraceLevel.Info);
            myCaps = new RigCaps(capsList);
            // 100 watts fixed power
            _XmitPower = 100;
        }

        protected override void MRThreadProc(object o)
        {
            Tracing.TraceLine("MRThreadProc:" + o.ToString(), TraceLevel.Info);
            string str = o.ToString();
 
            MemoryData m = null;
            try
            {
                int ofst = 3; // first digit is 0.
                string memNoString;
                char c = str[ofst++];
                if (c == ' ') c = '0';
                memNoString = c.ToString() + str.Substring(ofst, 2);
                ofst += 2;
                int memNo;
                memNo = System.Int32.Parse(memNoString);
                m = Memories.mems[memNo];
                m.myLock.WaitOne();
                m.Type = (memNo < 100) ? MemoryTypes.Normal :
                    (memNo < 108) ? MemoryTypes.MemorySwitch :
                    MemoryTypes.PowerOn;
                m.Number = memNo;
                string wkstr;
                wkstr = str.Substring(ofst, 11);
                m.Frequency[0] = System.UInt64.Parse(wkstr);
                m.Present = (m.Frequency[0] != 0);
                m.State = memoryStates.complete;
            }
            catch (Exception ex)
            {
                Tracing.TraceLine("MRThread exception" + ex.Message, TraceLevel.Error);
            }
            finally
            {
                if (m != null)
                {
                    m.myLock.ReleaseMutex();
                }
            }
        }
    }
}
