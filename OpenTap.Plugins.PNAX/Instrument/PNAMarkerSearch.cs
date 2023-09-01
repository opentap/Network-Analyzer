using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

//Note this template assumes that you have a SCPI based instrument, and accordingly
//extends the ScpiInstrument base class.

//If you do NOT have a SCPI based instrument, you should modify this instance to extend
//the (less powerful) Instrument base class.

namespace OpenTap.Plugins.PNAX
{
    public enum SAMultiPeakSearchPolarityEnumType
    {
        [Scpi("NEG")]
        [Display("Negative")]
        NEG,
        [Scpi("POS")]
        [Display("Positive")]
        POS,
        [Scpi("BOTH")]
        [Display("Both")]
        BOTH
    }

    public partial class PNAX : ScpiInstrument
    {
        public void SetMarkerState(int Channel, int mnum, int mkr, SAOnOffTypeEnum state)
        {
            string scpi = Scpi.Format("{0}", state);
            ScpiCommand($"CALCulate{Channel.ToString()}:MEASure{mnum.ToString()}:MARKer{mkr.ToString()}:STATe ON");
        }

        public void SetMarkerXValue(int Channel, int mnum, int mkr, double value)
        {
            ScpiCommand($"CALCulate{Channel.ToString()}:MEASure{mnum.ToString()}:MARKer{mkr.ToString()}:X {value}");
        }

        public void MultiPeakSearchExecute(int Channel, int mnum)
        {
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:MARKer:FUNCtion:MULTi:EXECute PEAK");
        }

        public void SetMultiPeakSearchThreshold(int Channel, int mnum, double value)
        {
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:MARKer:FUNCtion:MULTi:PEAK:THReshold {value}");
        }

        public void SetMultiPeakSearchExcursion(int Channel, int mnum, double value)
        {
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:MARKer:FUNCtion:MULTi:PEAK:EXCursion {value}");
        }

        public void SetMultiPeakSearchPolarity(int Channel, int mnum, SAMultiPeakSearchPolarityEnumType value)
        {
            string scpi = Scpi.Format("{0}", value);
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:MARKer:FUNCtion:MULTi:PEAK:POLarity {scpi}");
        }
    }
}
