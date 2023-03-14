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
    public partial class PNAX : ScpiInstrument
    {
        #region Mixer Setup
        public ConverterStagesEnum GetConverterStages(int Channel)
        {
            int retInt = ScpiQuery<int>($"SENSe{ Channel }:MIXer:STAGe?");
            return retInt == 1 ? ConverterStagesEnum._1 : ConverterStagesEnum._2;
        }

        public void SetConverterStages(int Channel, ConverterStagesEnum stages)
        {
            if (stages == ConverterStagesEnum._1)
                ScpiCommand($"SENSe{ Channel }:MIXer:STAGe 1");
            else
                ScpiCommand($"SENSe{ Channel }:MIXer:STAGe 2");
        }

        public int GetPortInput(int Channel)
        {
            return ScpiQuery<int>($"SENS{ Channel }:MIXer:PMAP:INP?");
        }

        public int GetPortOutput(int Channel)
        {
            return ScpiQuery<int>($"SENS{ Channel }:MIXer:PMAP:OUTP?");
        }

        public void SetPortInputOutput(int Channel, PortsEnum inport, PortsEnum outport)
        {
            string inp = Scpi.Format("{0}", inport);
            string outp = Scpi.Format("{0}", outport);
            ScpiCommand($"SENS{ Channel }:MIXer:PMAP {inp},{outp}");
        }

        public int GetInputFractionalMultiplierNumerator(int Channel)
        {
            return ScpiQuery<int>($"SENSe{ Channel }:MIXer:INPut:FREQ:NUMerator?");
        }

        public void SetInputFractionalMultiplierNumerator(int Channel, int value)
        {
            ScpiCommand($"SENSe{ Channel }:MIXer:INPut:FREQ:NUMerator { value }");
        }

        public int GetInputFractionalMultiplierDenominator(int Channel)
        {
            return ScpiQuery<int>($"SENSe{ Channel }:MIXer:INPut:FREQ:DENominator?");
        }

        public void SetInputFractionalMultiplierDenominator(int Channel, int value)
        {
            ScpiCommand($"SENSe{ Channel }:MIXer:INPut:FREQ:DENominator { value }");
        }

        public int GetLOFractionalMultiplierNumerator(int Channel, int Stage)
        {
            return ScpiQuery<int>($"SENSe{ Channel }:MIXer:LO{ Stage }:FREQuency:NUMerator?");
        }

        public void SetLOFractionalMultiplierNumerator(int Channel, int Stage, int value)
        {
            ScpiCommand($"SENSe{ Channel }:MIXer:LO{ Stage }:FREQuency:NUMerator { value }");
        }

        public int GetLOFractionalMultiplierDenominator(int Channel, int Stage)
        {
            return ScpiQuery<int>($"SENSe{ Channel }:MIXer:LO{ Stage }:FREQuency:DENominator?"); ;
        }

        public void SetLOFractionalMultiplierDenominator(int Channel, int Stage, int value)
        {
            ScpiCommand($"SENSe{ Channel }:MIXer:LO{ Stage }:FREQuency:DENominator { value }");
        }

        // SENSe<ch>:MIXer:LO<n>:NAME?
        public string GetPortLO(int Channel, int Stage)
        {
            return ScpiQuery($"SENSe{ Channel }:MIXer:LO{ Stage }:NAME?"); ;
        }

        public void SetPortLO(int Channel, int Stage, LOEnum value)
        {
            string str = Scpi.Format("{0}", value);
            ScpiCommand($"SENSe{ Channel }:MIXer:LO{ Stage }:NAME \"{str}\"");
        }

        public bool GetEnableEmbeddedLO(int Channel)
        {
            return ScpiQuery<bool>($"SENS{ Channel }:MIXer:ELO:STATe?");
        }

        public void SetEnableEmbeddedLO(int Channel, bool value)
        {
            if (value)
                ScpiCommand($"SENS{ Channel }:MIXer:ELO:STATe 1");
            else
                ScpiCommand($"SENS{ Channel }:MIXer:ELO:STATe 0");
        }

        // SENSe<ch>:MIXer:ELO:TUNing:MODE?
        public TuningMethodEnum GetTuningMethod(int Channel)
        {
            TuningMethodEnum retVal = TuningMethodEnum.BroadbandAndPrecise;
            string retStr = ScpiQuery($"SENS{ Channel }:MIXer:ELO:TUNing:MODE?");
            if (retStr.Equals("BRO"))
            {
                retVal = TuningMethodEnum.BroadbandAndPrecise;
            }
            else if (retStr.Equals("PREC"))
            {
                retVal = TuningMethodEnum.PreciseOnly;
            }
            else if (retStr.Equals("NON"))
            {
                retVal = TuningMethodEnum.DisableTuning;
            }
            else
            {
                throw new Exception("unknown tuning method");
            }
            return retVal;
        }

        public void SetTuningMethod(int Channel, TuningMethodEnum tuning)
        {
            if (tuning == TuningMethodEnum.BroadbandAndPrecise)
            {
                ScpiCommand($"SENS{ Channel }:MIXer:ELO:TUNing:MODE BROadband");
            }
            else if (tuning == TuningMethodEnum.PreciseOnly)
            {
                ScpiCommand($"SENS{ Channel }:MIXer:ELO:TUNing:MODE PRECise");
            }
            else if (tuning == TuningMethodEnum.DisableTuning)
            {
                ScpiCommand($"SENS{ Channel }:MIXer:ELO:TUNing:MODE NONe");
            }
            else
            {
                throw new Exception("unknown tuning method");
            }
        }

        // SENSe<ch>:MIXer:ELO:NORMalize:POINt?
        public int GetTuningPoint(int Channel)
        {
            return ScpiQuery<int>($"SENS{ Channel }:MIXer:ELO:NORMalize:POINt?");
        }

        public void SetTuningPoint(int Channel, int point)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:ELO:NORMalize:POINt { point }");
        }

        // PNA UI shows "Tune every" for this parameter
        public int GetTuningInterval(int Channel)
        {
            return ScpiQuery<int>($"SENS{ Channel }:MIXer:ELO:TUNing:INTerval?");
        }

        public void SetTuningInterval(int Channel, int interval)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:ELO:TUNing:INTerval { interval }");
        }

        // PNA UI shows "Broadband Search" for this parameter
        public int GetTuningSpan(int Channel)
        {
            return ScpiQuery<int>($"SENS{ Channel }:MIXer:ELO:TUNing:SPAN?");
        }

        public void SetTuningSpan(int Channel, int span)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:ELO:TUNing:SPAN { span }");
        }

        // PNA UI shows "IFBW" for this parameter
        public int GetTuningIFBW(int Channel)
        {
            return ScpiQuery<int>($"SENS{ Channel }:MIXer:ELO:TUNing:IFBW?");
        }

        public void SetTuningIFBW(int Channel, int value)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:ELO:TUNing:IFBW { value }");
        }

        // PNA UI shows "Max Iterations" for this parameter
        public int GetTuningMaxIterations(int Channel)
        {
            return ScpiQuery<int>($"SENS{ Channel }:MIXer:ELO:TUNing:ITERations?");
        }

        public void SetTuningMaxIterations(int Channel, int value)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:ELO:TUNing:ITERations { value }");
        }

        // PNA UI shows "Tolerance" for this parameter
        public int GetTuningTolerance(int Channel)
        {
            return ScpiQuery<int>($"SENS{ Channel }:MIXer:ELO:TUNing:TOLerance?");
        }

        public void SetTuningTolerance(int Channel, int value)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:ELO:TUNing:TOLerance { value }");
        }

        // PNA UI shows "LO Frequency Delta" for this parameter
        public double GetLOFrequencyDelta(int Channel)
        {
            return ScpiQuery<int>($"SENS{ Channel }:MIXer:ELO:LO:DELTa?");
        }

        public void SetLOFrequencyDelta(int Channel, double value)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:ELO:LO:DELTa { value }");
        }

        #endregion

        #region Mixer Power

        public bool GetPowerOnAllChannels()
        {
            return ScpiQuery<bool>($"OUTPut:STATe?");
        }

        public void SetPowerOnAllChannels(bool value)
        {
            if (value)
            {
                ScpiCommand($"OUTPut:STATe 1");
            }
            else
            {
                ScpiCommand($"OUTPut:STATe 0");
            }
        }

        public double GetLOPower(int Channel, int stage)
        {
            return ScpiQuery<double>($"SENS{ Channel }:MIXer:LO{ stage }:POWer?");
        }

        public void SetLOPower(int Channel, int stage, double power)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:LO{ stage }:POWer {power.ToString()}");
        }

        public string GetSourceLevelingMode(int Channel, int port)
        {
            return ScpiQuery($"SOURce{ Channel }:POWer{ port }:ALC:MODE?");
        }

        public void SetSourceLevelingMode(int Channel, int port, string mode)
        {
            ScpiCommand($"SOURce{ Channel }:POWer{ port }:ALC:MODE {mode}");
        }

        public void SetSourceLevelingMode(int Channel, PortsEnum port, string mode)
        {
            string strPort = Scpi.Format("{0}", port);
            ScpiCommand($"SOURce{ Channel }:POWer{strPort}:ALC:MODE {mode}");
        }

        public string GetSourceLevelingModes(int Channel, int port)
        {
            return ScpiQuery($"SOURce{ Channel }:POWer{ port }:ALC:MODE:CATalog?");
        }

        public double GetSourceAttenuator(int Channel, int port)
        {
            return ScpiQuery<double>($"SOURce{ Channel }:POWer{ port }:ATTenuation?");
        }

        public void SetSourceAttenuator(int Channel, int port, double value)
        {
            ScpiCommand($"SOURce{ Channel }:POWer{ port }:ATTenuation { value }");
        }

        public void SetSourceAttenuator(int Channel, PortsEnum port, double value)
        {
            string strPort = Scpi.Format("{0}", port);
            ScpiCommand($"SOURce{ Channel }:POWer{strPort}:ATTenuation { value }");
        }

        public double GetReceiverAttenuator(int Channel, int port)
        {
            return ScpiQuery<double>($"SOURce{ Channel }:POWer{ port }:ATTenuation:RECeiver:TEST?");
        }

        public void SetReceiverAttenuator(int Channel, int port, double value)
        {
            ScpiCommand($"SOURce{ Channel }:POWer{ port }:ATTenuation:RECeiver:TEST { value }");
        }

        public void SetReceiverAttenuator(int Channel, PortsEnum port, double value)
        {
            string strPort = Scpi.Format("{0}", port);
            ScpiCommand($"SOURce{ Channel }:POWer{strPort}:ATTenuation:RECeiver:TEST { value }");
        }

        public double GetLOSweptPowerStart(int Channel, int stage)
        {
            return ScpiQuery<double>($"SENS{ Channel }:MIXer:LO{ stage }:FREQuency:STARt?");
        }

        public void SetLOSweptPowerStart(int Channel, int stage, double value)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:LO{ stage }:FREQuency:STARt { value }");
        }

        public double GetLOSweptPowerStop(int Channel, int stage)
        {
            return ScpiQuery<double>($"SENS{ Channel }:MIXer:LO{ stage }:FREQuency:STOP?");
        }

        public void SetLOSweptPowerStop(int Channel, int stage, double value)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:LO{ stage }:FREQuency:STOP { value }");
        }

        #endregion

        #region Mixer Frequency
        public double GetFrequencyInputStart(int Channel)
        {
            return ScpiQuery<double>($"SENS{ Channel }:MIXer:INPut:FREQuency:STARt?");
        }

        public void SetFrequencyInputStart(int Channel, double value)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:INPut:FREQuency:STARt { value }");
        }

        public double GetFrequencyInputStop(int Channel)
        {
            return ScpiQuery<double>($"SENS{ Channel }:MIXer:INPut:FREQuency:STOP?");
        }

        public void SetFrequencyInputStop(int Channel, double value)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:INPut:FREQuency:STOP { value }");
        }

        public double GetFrequencyInputFixed(int Channel)
        {
            return ScpiQuery<double>($"SENS{ Channel }:MIXer:INPut:FREQuency:FIXed?");
        }

        public void SetFrequencyInputFixed(int Channel, double value)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:INPut:FREQuency:FIXed { value }");
        }

        public double GetFrequencyLOStart(int Channel, int stage)
        {
            return ScpiQuery<double>($"SENS{ Channel }:MIXer:LO{ stage }:FREQuency:STARt?");
        }

        public void SetFrequencyLOStart(int Channel, int stage, double value)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:LO{ stage }:FREQuency:STARt { value }");
        }

        public double GetFrequencyLOStop(int Channel, int stage)
        {
            return ScpiQuery<double>($"SENS{ Channel }:MIXer:LO{ stage }:FREQuency:STOP?");
        }

        public void SetFrequencyLOStop(int Channel, int stage, double value)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:LO{ stage }:FREQuency:STOP { value }");
        }

        public double GetFrequencyLOFixed(int Channel, int stage)
        {
            return ScpiQuery<double>($"SENS{ Channel }:MIXer:LO{ stage }:FREQuency:FIXed?");
        }

        public void SetFrequencyLOFixed(int Channel, int stage, double value)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:LO{ stage }:FREQuency:FIXed { value }");
        }

        public bool GetLOILTI(int Channel, int stage)
        {
            return ScpiQuery<bool>($"SENS{ Channel }:MIXer:LO{ stage }:FREQuency:ILTI?");
        }

        public void SetLOILTI(int Channel, int stage, bool value)
        {
            if (value)
            {
                ScpiCommand($"SENS{ Channel }:MIXer:LO{ stage }:FREQuency:ILTI 1");
            }
            else
            {
                ScpiCommand($"SENS{ Channel }:MIXer:LO{ stage }:FREQuency:ILTI 0");
            }
        }

        public double GetFrequencyIFStart(int Channel)
        {
            return ScpiQuery<double>($"SENS{ Channel }:MIXer:IF:FREQuency:STARt?");
        }

        public void SetFrequencyIFStart(int Channel, double value)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:IF:FREQuency:STARt { value }");
        }

        public double GetFrequencyIFStop(int Channel)
        {
            return ScpiQuery<double>($"SENS{ Channel }:MIXer:IF:FREQuency:STOP?");
        }

        public void SetFrequencyIFStop(int Channel, double value)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:IF:FREQuency:STOP { value }");
        }

        // TODO
        // How to setup IF Fixed?
        // Mixer Frequency Tab shows this as an option
        // TODO
        //public double GetFrequencyIFFixed(int Channel)
        //{
        //    double retVal = double.NaN;
        //    retVal = ScpiQuery<double>($"SENS{ Channel }:MIXer:IF:FREQuency:FIXed?");
        //    return retVal;
        //}

        //public void SetFrequencyIFFixed(int Channel, double value)
        //{
        //    ScpiCommand($"SENS{ Channel }:MIXer:IF:FREQuency:FIXed { value }");
        //}

        public SidebandTypeEnum GetFrequencyIFSideband(int Channel)
        {
            SidebandTypeEnum retVal;
            string retStr = ScpiQuery($"SENSe{ Channel }:MIXer:IF:FREQuency:SIDeband?");
            if (retStr.Equals("HIGH"))
            {
                retVal = SidebandTypeEnum.High;
            }
            else if (retStr.Equals("LOW"))
            {
                retVal = SidebandTypeEnum.Low;
            }
            else
            {
                throw new Exception("unknown side band");
            }
            return retVal;
        }

        public void SetFrequencyIFSideband(int Channel, SidebandTypeEnum sideband)
        {
            if (sideband == SidebandTypeEnum.High)
            {
                ScpiCommand($"SENSe{ Channel }:MIXer:IF:FREQuency:SIDeband HIGH");
            }
            else if (sideband == SidebandTypeEnum.Low)
            {
                ScpiCommand($"SENSe{ Channel }:MIXer:IF:FREQuency:SIDeband LOW");
            }
            else
            {
                throw new Exception("unknown side band");
            }
        }









        public double GetFrequencyOutputStart(int Channel)
        {
            return ScpiQuery<double>($"SENS{ Channel }:MIXer:OUTPut:FREQuency:STARt?");
        }

        public void SetFrequencyOutputStart(int Channel, double value)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:OUTPut:FREQuency:STARt { value }");
        }

        public double GetFrequencyOutputStop(int Channel)
        {
            return ScpiQuery<double>($"SENS{ Channel }:MIXer:OUTPut:FREQuency:STOP?");
        }

        public void SetFrequencyOutputStop(int Channel, double value)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:OUTPut:FREQuency:STOP { value }");
        }

        public double GetFrequencyOutputFixed(int Channel)
        {
            return ScpiQuery<double>($"SENS{ Channel }:MIXer:OUTPut:FREQuency:FIXed?");
        }

        public void SetFrequencyOutputFixed(int Channel, double value)
        {
            ScpiCommand($"SENS{ Channel }:MIXer:OUTPut:FREQuency:FIXed { value }");
        }

        public SidebandTypeEnum GetFrequencyOutputSideband(int Channel)
        {
            SidebandTypeEnum retVal;
            string retStr = ScpiQuery($"SENSe{ Channel }:MIXer:OUTPut:FREQuency:SIDeband?");
            if (retStr.Equals("HIGH"))
            {
                retVal = SidebandTypeEnum.High;
            }
            else if (retStr.Equals("LOW"))
            {
                retVal = SidebandTypeEnum.Low;
            }
            else
            {
                throw new Exception("unknown side band");
            }
            return retVal;
        }

        public void SetFrequencyOutputSideband(int Channel, SidebandTypeEnum sideband)
        {
            if (sideband == SidebandTypeEnum.High)
            {
                ScpiCommand($"SENSe{ Channel }:MIXer:OUTPut:FREQuency:SIDeband HIGH");
            }
            else if (sideband == SidebandTypeEnum.Low)
            {
                ScpiCommand($"SENSe{ Channel }:MIXer:OUTPut:FREQuency:SIDeband LOW");
            }
            else
            {
                throw new Exception("unknown side band");
            }
        }

        #endregion
    }
}
