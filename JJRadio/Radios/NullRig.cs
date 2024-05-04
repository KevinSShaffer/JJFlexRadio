using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using JJTrace;

namespace Radios
{
    public class NullRig : AllRadios
    {
        private static ModeValue[] myModeTable =
            {
                new ModeValue(0, '0',"none"),
            };

        public NullRig()
        {
            Tracing.TraceLine("NullRig", TraceLevel.Info);
            ModeTable = myModeTable;
        }

        public override bool Open(OpenParms p)
        {
            Tracing.TraceLine("NullRig Open", TraceLevel.Info);
            return base.Open(p);
        }

        public override void close()
        {
            Tracing.TraceLine("NullRig Close", TraceLevel.Info);
            base.close();
        }
    }
}
