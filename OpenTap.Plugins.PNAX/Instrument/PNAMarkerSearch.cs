using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using OpenTap;

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
        BOTH,
    }

    public partial class PNAX : ScpiInstrument
    {
        public void SetMarkerState(int Channel, int mnum, int mkr, SAOnOffTypeEnum state)
        {
            string scpi = Scpi.Format("{0}", state);
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:MARKer{mkr}:STATe ON");
        }

        public void SetMarkerXValue(int Channel, int mnum, int mkr, double value)
        {
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:MARKer{mkr}:X {value}");
        }

        public void MultiPeakSearchExecute(int Channel, int mnum)
        {
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:MARKer:FUNCtion:MULTi:EXECute PEAK");
        }

        public void SetMultiPeakSearchThreshold(int Channel, int mnum, double value)
        {
            ScpiCommand(
                $"CALCulate{Channel}:MEASure{mnum}:MARKer:FUNCtion:MULTi:PEAK:THReshold {value}"
            );
        }

        public void SetMultiPeakSearchExcursion(int Channel, int mnum, double value)
        {
            ScpiCommand(
                $"CALCulate{Channel}:MEASure{mnum}:MARKer:FUNCtion:MULTi:PEAK:EXCursion {value}"
            );
        }

        public void SetMultiPeakSearchPolarity(
            int Channel,
            int mnum,
            SAMultiPeakSearchPolarityEnumType value
        )
        {
            string scpi = Scpi.Format("{0}", value);
            ScpiCommand(
                $"CALCulate{Channel}:MEASure{mnum}:MARKer:FUNCtion:MULTi:PEAK:POLarity {scpi}"
            );
        }

        public void SetMarkersOff(int Channel, int mnum)
        {
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:MARKer:AOFF");
        }

        public void CalculateMeasureMarkerFunctionPeak(int Channel, int mnum, int mkr)
        {
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:MARKer{mkr}:FUNCtion:EXECute PEAK");
            WaitForOperationComplete();
        }

        public void CalculateMeasureMarkerSetCenter(int Channel, int mnum, int mkr)
        {
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:MARKer{mkr}:SET CENTer");
        }

        public double CalculateMeasureMarkerY(int Channel, int mnum, int mkr)
        {
            // get the Y value:  (Value,0)
            string yString = ScpiQuery($"CALCulate{Channel}:MEASure{mnum}:MARKer{mkr}:Y?");
            var y = yString.Split(',').Select(double.Parse).ToList();
            double mrkrY = y[0];
            return mrkrY;
        }

        public double CalculateMeasureMarkerX(int Channel, int mnum, int mkr)
        {
            return ScpiQuery<double>($"CALCulate{Channel}:MEASure{mnum}:MARKer{mkr}:X?");
        }

        public bool CalculateMeasureMarkerState(int Channel, int mnum, int mkr)
        {
            return ScpiQuery<bool>($"CALCulate{Channel}:MEASure{mnum}:MARKer{mkr}:STATe?");
        }
    }
}
