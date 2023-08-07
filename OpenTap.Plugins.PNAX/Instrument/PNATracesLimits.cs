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

    public enum LimitType
    {
        [Scpi("OFF")]
        Off,
        [Scpi("LMAX")]
        Max,
        [Scpi("LMIN")]
        Min
    }

    public class LimitSegmentDefinition
    {
        [Display("Type", Order: 1)]
        public LimitType LimitType { get; set; }
        [Display("Begin Stim", Order: 2)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double BeginStim { get; set; }
        [Display("End Stim", Order: 3)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double EndStim { get; set; }
        [Display("Begin Resp", Order: 4)]
        [Unit("dB", UseEngineeringPrefix: true)]
        public double BeginResp { get; set; }
        [Display("End Resp", Order: 5)]
        [Unit("dB", UseEngineeringPrefix: true)]
        public double EndResp { get; set; }
    }

    public partial class PNAX : ScpiInstrument
    {
        public void SetLimitTestOn(int Channel, int mnum, bool state)
        {
            if (state)
            {
                ScpiCommand($"CALCulate{ Channel }:MEASure{mnum}:LIMit:STATe ON");
            }
            else
            {
                ScpiCommand($"CALCulate{ Channel }:MEASure{mnum}:LIMit:STATe OFF");
            }
        }

        public void SetLimitLineOn(int Channel, int mnum, bool state)
        {
            if (state)
            {
                ScpiCommand($"CALCulate{ Channel }:MEASure{mnum}:LIMit:DISPlay ON");
            }
            else
            {
                ScpiCommand($"CALCulate{ Channel }:MEASure{mnum}:LIMit:DISPlay OFF");
            }
        }

        public void SetLimitTestFailOn(int Channel, int mnum, bool state)
        {
            if (state)
            {
                ScpiCommand($"CALCulate{ Channel }:MEASure{mnum}:LIMit:SOUNd:STATe ON");
            }
            else
            {
                ScpiCommand($"CALCulate{ Channel }:MEASure{mnum}:LIMit:SOUNd:STATe OFF");
            }
        }

        public void SetXPosition(int Window, double num)
        {
            ScpiCommand($"DISPlay:WINDow{Window.ToString()}:ANNotation:LIMit:XPOSition {num}");
        }

        public void SetYPosition(int Window, double num)
        {
            ScpiCommand($"DISPlay:WINDow{Window.ToString()}:ANNotation:LIMit:YPOSition {num}");
        }

        public void SetLimitTableShow(int window, bool state)
        {
            // Lets review if the window exists
            int windowState = ScpiQuery<int>($"DISPlay:WINDow{window}?");
            if (windowState == 0)
            {
                // if does not exist lets enable it
                ScpiCommand($"DISPlay:WINDow{window} ON");
            }

            // Set the window state
            if (state)
            {
                ScpiCommand($"DISPlay:WINDow{window}:LIMit SEGM");
            }
            else
            {
                ScpiCommand($"DISPlay:WINDow{window}:LIMit OFF");
            }
        }

        public void SetLimitData(int Channel, int mnum, List<LimitSegmentDefinition> limitSegments)
        {
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:LIMit:DATA:DELete");

            int segm = 1;
            foreach(LimitSegmentDefinition limit in limitSegments)
            {
                String t = Scpi.Format("{0}", limit.LimitType);
                ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:LIMit:SEGMent{segm}:TYPE {t}");
                ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:LIMit:SEGMent{segm}:STIMulus:STARt {limit.BeginStim}");
                ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:LIMit:SEGMent{segm}:STIMulus:STOP {limit.EndStim}");
                ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:LIMit:SEGMent{segm}:AMPLitude:STARt {limit.BeginResp}");
                ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:LIMit:SEGMent{segm}:AMPLitude:STOP {limit.EndResp}");
                segm++;
            }
        }

        public void SetGlobalPF(bool state)
        {
            if (state)
            {
                ScpiCommand($"DISP:FSIG ON");
            }
            else
            {
                ScpiCommand($"DISP:FSIG OFF");
            }
        }
    }
}
