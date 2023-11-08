using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Channels;
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
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:LIMit:STATe {stateValue}");
        }

        public bool GetLimitTestOn(int Channel, int mnum)
        {
            int state = IsModelA ? ScpiQuery<int>($"CALCulate{Channel}:LIMit:STATe?") : ScpiQuery<int>($"CALCulate{ Channel }:MEASure{mnum}:LIMit:STATe?");
            return state == 1;
        }

        public void SetLimitLineOn(int Channel, int mnum, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:LIMit:DISPlay {stateValue}");
        }

        public void SetLimitTestFailOn(int Channel, int mnum, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:LIMit:SOUNd:STATe {stateValue}");
        }

        public void SetXPosition(int Window, double num)
        {
            ScpiCommand($"DISPlay:WINDow{Window}:ANNotation:LIMit:XPOSition {num}");
        }

        public void SetYPosition(int Window, double num)
        {
            ScpiCommand($"DISPlay:WINDow{Window}:ANNotation:LIMit:YPOSition {num}");
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
            string stateValue = state ? "SEGM" : "OFF";
            ScpiCommand($"DISPlay:WINDow{window}:LIMit {stateValue}");
        }

        public void SetLimitData(int Channel, int mnum, List<LimitSegmentDefinition> limitSegments)
        {
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:LIMit:DATA:DELete");

            int segm = 1;
            foreach(LimitSegmentDefinition limit in limitSegments)
            {
                string t = Scpi.Format("{0}", limit.LimitType);
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
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"DISP:FSIG {stateValue}");
        }
    }
}
