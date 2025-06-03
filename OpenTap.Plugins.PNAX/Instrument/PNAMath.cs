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

    [Flags]
    public enum MathStatisticTypeEnum
    {
        [Scpi("PTP")]
        [Display("Peak to Peak")]
        Ptp = 1,
        [Scpi("STDEV")]
        [Display("Std Dev")]
        Std = 2,
        [Scpi("MEAN")]
        [Display("Mean")]
        Mean = 4,
        [Scpi("MIN")]
        [Display("Min")]
        Min = 8,
        [Scpi("MAX")]
        [Display("Max")]
        Max = 16
    }

    public enum MathStatisticsRangeEnum
    {
        [Scpi("0")]
        [Display("Full Span")]
        FullSpan,
        [Scpi("1")]
        [Display("User 1")]
        User1,
        [Scpi("2")]
        [Display("User 2")]
        User2,
        [Scpi("3")]
        [Display("User 3")]
        User3,
        [Scpi("4")]
        [Display("User 4")]
        User4,
        [Scpi("5")]
        [Display("User 5")]
        User5,
        [Scpi("6")]
        [Display("User 6")]
        User6,
        [Scpi("7")]
        [Display("User 7")]
        User7,
        [Scpi("8")]
        [Display("User 8")]
        User8,
        [Scpi("9")]
        [Display("User 9")]
        User9,
        [Scpi("10")]
        [Display("User 10")]
        User10,
        [Scpi("11")]
        [Display("User 11")]
        User11,
        [Scpi("12")]
        [Display("User 12")]
        User12,
        [Scpi("13")]
        [Display("User 13")]
        User13,
        [Scpi("14")]
        [Display("User 14")]
        User14,
        [Scpi("15")]
        [Display("User 15")]
        User15,
        [Scpi("16")]
        [Display("User 16")]
        User16,
    }

    public partial class PNAX : ScpiInstrument
    {
        #region Limits
        public void SetLimitTestOn(int Channel, int mnum, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:LIMit:STATe {stateValue}");
        }

        public bool GetLimitTestOn(int Channel, int mnum)
        {
            int state = IsModelA ? ScpiQuery<int>($"CALCulate{Channel}:LIMit:STATe?") : ScpiQuery<int>($"CALCulate{Channel}:MEASure{mnum}:LIMit:STATe?");
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
            foreach (LimitSegmentDefinition limit in limitSegments)
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
        #endregion

        #region Statistics
        public void MathStatistics(int Channel, int mnum, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:FUNCtion:STATistics:STATe {stateValue}");
        }

        public bool MathStatistics(int Channel, int mnum)
        {
            return ScpiQuery<bool>($"CALCulate{Channel}:MEASure{mnum}:FUNCtion:STATistics:STATe?");
        }

        public void MathType(int Channel, int mnum, MathStatisticTypeEnum mathStatisticType)
        {
            string StatisticType = Scpi.Format("{0}", mathStatisticType);
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:FUNCtion:TYPE {StatisticType}");
        }

        public void MathExecuteStatistics(int Channel, int mnum)
        {
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:FUNCtion:EXECute");
        }

        public double MathData(int Channel, int mnum)
        {
            return ScpiQuery<double>($"CALCulate{Channel}:MEASure{mnum}:FUNCtion:DATA?");
        }

        public void MathShowResistance(int Channel, int mnum, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:FUNCtion:STATistics:RESistance:STATe {stateValue}");
        }

        public void MathStatisticsRange(int Channel, int mnum, MathStatisticsRangeEnum mathStatisticsRange)
        {
            string StatisticRange = Scpi.Format("{0}", mathStatisticsRange);
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:FUNCtion:DOMain:USER:RANGe {StatisticRange}");
        }

        public void MathStatisticsRangeStart(int Channel, int mnum, MathStatisticsRangeEnum mathStatisticsRange, double value)
        {
            if (mathStatisticsRange == MathStatisticsRangeEnum.FullSpan)
            {
                throw new Exception("Full span can't set start/stop");
            }
            string StatisticRange = Scpi.Format("{0}", mathStatisticsRange);
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:FUNCtion:DOMain:USER:STARt {StatisticRange}, {value}");
        }

        public void MathStatisticsRangeStop(int Channel, int mnum, MathStatisticsRangeEnum mathStatisticsRange, double value)
        {
            if (mathStatisticsRange == MathStatisticsRangeEnum.FullSpan)
            {
                throw new Exception("Full span can't set start/stop");
            }
            string StatisticRange = Scpi.Format("{0}", mathStatisticsRange);
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:FUNCtion:DOMain:USER:STOP {StatisticRange}, {value}");
        }
        #endregion
    }
}
