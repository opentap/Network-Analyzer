﻿using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

//Note this template assumes that you have a SCPI based instrument, and accordingly
//extends the ScpiInstrument base class.

//If you do NOT have a SCPI based instrument, you should modify this instance to extend
//the (less powerful) Instrument base class.

namespace OpenTap.Plugins.PNAX
{
    public partial class PNAX : ScpiInstrument
    {
        public void DIQFrequencyRangeAdd(int Channel)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:FREQuency:RANGe:ADD");
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

        public void DIQFrequencyRangeStop(int Channel, int range, double value)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:FREQuency:RANGe{range}:STOP {value}");
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
        public void DIQSourceState(int Channel, string source, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:STATe {stateValue},\"{source}\"");
        }

        public void DIQSourceFreqRange(int Channel, string source, double value)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:RANGe {value},\"{source}\"");
        }

        public void DIQSourceExternalPort(int Channel, string source, int port)
        {
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
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:POWer:ALC:MODE {mode},\"{source}\"");
        }

        public void DIQSourcePowerAttenuation(int Channel, string source, double value)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:POWer:ATTenuation {value},\"{source}\"");
        }

        public void DIQSourcePowerAttenuationAuto(int Channel, string source, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:POWer:ATTenuation:AUTO {stateValue},\"{source}\"");
        }
        #endregion

        #region Source Phase
        public void DIQSourcePhaseState(int Channel, string source, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
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

        public void DIQSourcePhasecontrolParam(int Channel, string source, string param)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:PHASe:PARameter \"{param}\",\"{source}\"");
        }

        public void DIQSourcePhaseTolerance(int Channel, string source, double value)
        {
            ScpiCommand($"SOURce{Channel}:PHASe:CONTrol:TOLerance {value},\"{source}\"");
        }

        public void DIQSourcePhaseIterations(int Channel, string source, double value)
        {
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

        public void DIQSourceMatchCorrectionReferenceReceiver(int Channel, string source, string recvr)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:MATCh:RRECeiver \"{recvr}\",\"{source}\"");
        }

        public void DIQSourceMatchCorrectionRange(int Channel, string source, string ranges)
        {
            ScpiCommand($"SENSe{Channel}:DIQ:PORT:MATCh:RANGe \"{ranges}\",\"{source}\"");
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
