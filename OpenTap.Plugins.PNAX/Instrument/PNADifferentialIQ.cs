using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using OpenTap;

//Note this template assumes that you have a SCPI based instrument, and accordingly
//extends the ScpiInstrument base class.

//If you do NOT have a SCPI based instrument, you should modify this instance to extend
//the (less powerful) Instrument base class.

namespace OpenTap.Plugins.PNAX
{
    public enum DIQPortStateEnumtype
    {
        [Display("Auto On")]
        [Scpi("AUTO")]
        Auto,

        [Display("Always On")]
        [Scpi("ON")]
        On,

        [Display("Off")]
        [Scpi("OFF")]
        Off,
    }

    public enum DIQPhaseStateEnumtype
    {
        [Display("Off")]
        [Scpi("OFF")]
        Off,

        [Display("Controlled")]
        [Scpi("CONT")]
        Controlled,

        [Display("Open loop")]
        [Scpi("OPEN")]
        Oopenloop,
    }

    public partial class PNAX : ScpiInstrument
    {
        public void DIQFrequencyRangeAdd(int Channel)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:FREQuency:RANGe:ADD");
        }

        public void DIQFrequencyRangeDelete(int Channel)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:FREQuency:RANGe:DELete");
        }

        public int DIQFrequencyRangeCount(int Channel)
        {
            return ScpiQuery<int>($"SENSe{Channel}:DIQ:FREQuency:RANGe:COUNt?");
        }

        #region Frequency
        public void DIQFrequencyRangeStart(int Channel, int range, double value)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:FREQuency:RANGe{range}:STARt {value}");
        }

        public double DIQFrequencyRangeStart(int Channel, int range)
        {
            return ScpiQuery<double>($"SENSe{Channel}:DIQ:FREQuency:RANGe{range}:STARt?");
        }

        public void DIQFrequencyRangeStop(int Channel, int range, double value)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:FREQuency:RANGe{range}:STOP {value}");
        }

        public double DIQFrequencyRangeStop(int Channel, int range)
        {
            return ScpiQuery<double>($"SENSe{Channel}:DIQ:FREQuency:RANGe{range}:STOP?");
        }

        public void DIQFrequencyRangeIFBW(int Channel, int range, double value)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:FREQuency:RANGe{range}:IFBW {value}");
        }
        #endregion

        #region Coupling
        public void DIQFrequencyRangeCouplingState(int Channel, int range, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:DIQ:FREQuency:RANGe{range}:COUPle:STATe {stateValue}");
        }

        public void DIQFrequencyRangeCouplingID(int Channel, int range, int value)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:FREQuency:RANGe{range}:COUPle:ID {value}");
        }

        public void DIQFrequencyRangeCouplingOffset(int Channel, int range, int value)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:FREQuency:RANGe{range}:COUPle:OFFSet {value}");
        }

        public void DIQFrequencyRangeCouplingUp(int Channel, int range, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:DIQ:FREQuency:RANGe{range}:COUPle:UCONvert {stateValue}");
        }

        public void DIQFrequencyRangeCouplingMultiplier(int Channel, int range, int value)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:FREQuency:RANGe{range}:COUPle:MULTiplier {value}");
        }

        public void DIQFrequencyRangeCouplingDivisor(int Channel, int range, int value)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:FREQuency:RANGe{range}:COUPle:DIVisor {value}");
        }
        #endregion

        #region Source Configuration
        public void DIQSourceState(int Channel, string source, DIQPortStateEnumtype state)
        {
            string stateValue = Scpi.Format("{0}", state);
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:STATe {stateValue},\"{source}\"");
        }

        public void DIQSourceFreqRange(int Channel, string source, int value)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:RANGe {value},\"{source}\"");
        }

        public void DIQSourceFreqRange(int Channel, string source, string value)
        {
            // Expecting a string like "F1", need to remove the 'F' and parse the number to int
            string replaceValue = value.Replace("F", "");
            int range;
            if (!int.TryParse(replaceValue, out range))
                throw new Exception($"Invalid Frequency Range: {value}");
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:RANGe {range},\"{source}\"");
        }

        public void DIQSourceExternalPort(int Channel, string source, int port)
        {
            if (!OptionS93088)
            {
                Log.Warning(
                    "Option S93088A/B not on instrument, skipping command: "
                        + $"SOURce{Channel}:PHASe:EXTernal:PORT {port},\"{source}\""
                );
                return;
            }
            ScpiCommand($"SOURce{Channel}:PHASe:EXTernal:PORT {port},\"{source}\"");
        }
        #endregion

        #region Source Power
        public void DIQSourcePowerState(int Channel, string source, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:POWer:SWEep:STATe {stateValue},\"{source}\"");
        }

        public void DIQSourcePowerStart(int Channel, string source, double value)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:POWer:STARt {value},\"{source}\"");
        }

        public void DIQSourcePowerStop(int Channel, string source, double value)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:POWer:STOP {value},\"{source}\"");
        }

        public void DIQSourceLevelingMode(int Channel, string source, string mode)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:POWer:ALC:MODE \"{mode}\",\"{source}\"");
        }

        public void DIQSourcePowerAttenuation(int Channel, string source, double value)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:POWer:ATTenuation {value},\"{source}\"");
        }

        public void DIQSourcePowerAttenuationAuto(int Channel, string source, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand(
                $"SENSe{Channel}:DIQ:PORT:POWer:ATTenuation:AUTO {stateValue},\"{source}\""
            );
        }
        #endregion

        #region Source Phase
        public void DIQSourcePhaseState(int Channel, string source, DIQPhaseStateEnumtype state)
        {
            string stateValue = Scpi.Format("{0}", state);
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:PHASe:STATe {stateValue},\"{source}\"");
        }

        public void DIQSourcePhaseSweepState(int Channel, string source, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:PHASe:SWEep:STATe {stateValue},\"{source}\"");
        }

        public void DIQSourcePhaseStart(int Channel, string source, double value)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:PHASe:STARt {value},\"{source}\"");
        }

        public void DIQSourcePhaseStop(int Channel, string source, double value)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:PHASe:STOP {value},\"{source}\"");
        }

        public void DIQSourcePhaseReference(int Channel, string source, string refer)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:PHASe:REFerence \"{refer}\",\"{source}\"");
        }

        public void DIQSourcePhaseControlParam(int Channel, string source, string param)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:PHASe:PARameter \"{param}\",\"{source}\"");
        }

        public void DIQSourcePhaseTolerance(int Channel, string source, double value)
        {
            if (!OptionS93088)
            {
                Log.Warning(
                    "Option S93088A/B not on instrument, skipping command: "
                        + $"SOURce{Channel}:PHASe:CONTrol:TOLerance {value},\"{source}\""
                );
                return;
            }
            ScpiCommand($"SOURce{Channel}:PHASe:CONTrol:TOLerance {value},\"{source}\"");
        }

        public void DIQSourcePhaseIterations(int Channel, string source, double value)
        {
            if (!OptionS93088)
            {
                Log.Warning(
                    "Option S93088A/B not on instrument, skipping command: "
                        + $"SOURce{Channel}:PHASe:CONTrol:ITERation {value},\"{source}\""
                );
                return;
            }
            ScpiCommand($"SOURce{Channel}:PHASe:CONTrol:ITERation {value},\"{source}\"");
        }
        #endregion

        #region Source Match correction
        public void DIQSourceMatchCorrectionState(int Channel, string source, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:MATCh:STATe {stateValue},\"{source}\"");
        }

        public void DIQSourceMatchCorrectionTestReceiver(int Channel, string source, string recvr)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:MATCh:TRECeiver \"{recvr}\",\"{source}\"");
        }

        public void DIQSourceMatchCorrectionReferenceReceiver(
            int Channel,
            string source,
            string recvr
        )
        {
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:MATCh:RRECeiver \"{recvr}\",\"{source}\"");
        }

        public void DIQSourceMatchCorrectionRange(int Channel, string source, string ranges)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:MATCh:RANGe \"{ranges}\",\"{source}\"");
        }

        public void DIQSourceMatchCorrectionRange(int Channel, string source, List<string> ranges)
        {
            // overload that takes a list of strings
            string rangeList = string.Join(",", ranges);
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:MATCh:RANGe \"{rangeList}\",\"{source}\"");
        }
        #endregion

        #region Parameters
        public void DIQParameterDefine(int Channel, string name, string expression)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:PARameter:DEFine \"{name}\",\"{expression}\"");
        }

        public void DIQParameterDelete(int Channel, string name)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:PARameter:DELete \"{name}\"");
        }

        public List<string> DIQParameterCatalog(int Channel)
        {
            string retString = ScpiQuery($"SENSe{Channel}:DIQ:PARameter:CATalog?");
            retString = retString.Replace("\"", "");
            List<string> retVal = retString.Split(',').ToList<string>();
            return retVal;
        }
        #endregion
    }
}
