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
        #region Tone Power

        public int GetIMDPortInput(int Channel)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENS{Channel}:IMD:PMAP:INP?");
            return retVal;
        }

        public int GetIMDPortOutput(int Channel)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENS{Channel}:IMD:PMAP:OUTP?");
            return retVal;
        }

        public void SetIMDPortInputOutput(int Channel, DutInputPortsEnum inport, DutOutputPortsEnum outport)
        {
            string inp = Scpi.Format("{0}", inport);
            string outp = Scpi.Format("{0}", outport);
            ScpiCommand($"SENS{Channel}:IMD:PMAP {inp},{outp}");
        }

        public bool GetIMDCoupledTonePowers(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel}:IMD:TPOWer:COUPle?");
            return !retStr.Equals("0");
        }

        public void SetIMDCoupledTonePowers(int Channel, bool mode)
        {
            string stateValue = mode ? "1" : "0";
            ScpiCommand($"SENSe{Channel}:IMD:TPOWer:COUPle {stateValue}");
        }

        // SOURce<cnum>:POWer<port>:ALC:MODE?
        public bool GetIMDALCHardware(int Channel, int Port)
        {
            string retStr = ScpiQuery($"SOURce{Channel}:POWer{Port}:ALC:MODE?");
            if (retStr.Equals("OPEN"))
            {
                return false;
            }
            else if (retStr.Equals("INT"))
            {
                return true;
            }
            else
            {
                throw new Exception("Undefined ALC mode");
            }
        }

        // TODO same as SetSourceLevelingMode
        public void SetIMDALCHardware(int Channel, int Port, bool mode)
        {
            string stateValue = mode ? "INT" : "OPEN";
            ScpiCommand($"SOURce{Channel}:POWer{Port}:ALC:MODE {stateValue}");
        }

        // SENSe<cnum>:IMD:TPOWer:LEVel
        public PowerLevelingEnum GetIMDPowerLevelingMode(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel}:IMD:TPOWer:LEVel?");
            if (retStr.Equals("NONE"))
            {
                return PowerLevelingEnum.SetInputPower;
            }
            else if (retStr.Equals("INP"))
            {
                return PowerLevelingEnum.SetInputPowerReceiverLeveling;
            }
            else if (retStr.Equals("EQU"))
            {
                return PowerLevelingEnum.SetInputPowerEqualTonesAtOutput;
            }
            else if (retStr.Equals("OUTP"))
            {
                return PowerLevelingEnum.SetOutputPowerReceiverLeveling;
            }
            else
            {
                throw new Exception("Undefined ALC mode");
            }
        }

        public void SetIMDPowerLevelingMode(int Channel, PowerLevelingEnum mode)
        {
            if (mode == PowerLevelingEnum.SetInputPower)
            {
                ScpiCommand($"SENSe{Channel}:IMD:TPOWer:LEVel NONE");
            }
            else if (mode == PowerLevelingEnum.SetInputPowerReceiverLeveling)
            {
                ScpiCommand($"SENSe{Channel}:IMD:TPOWer:LEVel INP");
            }
            else if (mode == PowerLevelingEnum.SetInputPowerEqualTonesAtOutput)
            {
                ScpiCommand($"SENSe{Channel}:IMD:TPOWer:LEVel EQU");
            }
            else if (mode == PowerLevelingEnum.SetOutputPowerReceiverLeveling)
            {
                ScpiCommand($"SENSe{Channel}:IMD:TPOWer:LEVel OUTP");
            }
        }

        // SOURce<ch>:POWer<port>:ALC[:MODE]:RECeiver[:STATe]? [src]
        public bool GetIMDReceiverLevelingMode(int Channel, int Port)
        {
            string retStr = ScpiQuery($"SOURce{Channel}:POWer{Port}:ALC:RECeiver?");
            return !retStr.Equals("0");
        }

        public void SetIMDReceiverLevelingMode(int Channel, int Port, bool mode)
        {
            string stateValue = mode ? "1" : "0";
            ScpiCommand($"SOURce{Channel}:POWer{Port}:ALC:RECeiver {stateValue}");
        }


        public double GetIMDFixedPowerF1(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:IMD:TPOWer:F1?");
        }

        public void SetIMDFixedPowerF1(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:IMD:TPOWer:F1 {power}");
        }

        public double GetIMDFixedPowerF2(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:IMD:TPOWer:F2?");
        }

        public void SetIMDFixedPowerF2(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:IMD:TPOWer:F2 {power}");
        }

        public double GetIMDStartPowerF1(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:IMD:TPOWer:F1:STARt?");
        }

        public void SetIMDStartPowerF1(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:IMD:TPOWer:F1:STARt {power}");
        }

        public double GetIMDStartPowerF2(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:IMD:TPOWer:F2:STARt?");
        }

        public void SetIMDStartPowerF2(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:IMD:TPOWer:F2:STARt {power}");
        }

        public double GetIMDStopPowerF1(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:IMD:TPOWer:F1:STOP?");
        }

        public void SetIMDStopPowerF1(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:IMD:TPOWer:F1:STOP {power}");
        }

        public double GetIMDStopPowerF2(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:IMD:TPOWer:F2:STOP?");
        }

        public void SetIMDStopPowerF2(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:IMD:TPOWer:F2:STOP {power}");
        }
        #endregion

        #region Tone Frequency
        public ToneFrequencySweepTypeEnum GetIMDSweepType(int Channel)
        {
            string retString = ScpiQuery($"SENSe{Channel}:IMD:SWEep:TYPE?");
            if (retString.Equals("DFR"))
            {
                return ToneFrequencySweepTypeEnum.SweepDeltaF;
            }
            else if (retString.StartsWith("POW"))
            {
                return ToneFrequencySweepTypeEnum.PowerSweep;
            }
            else if (retString.Equals("CW"))
            {
                return ToneFrequencySweepTypeEnum.CW;
            }
            else if (retString.Equals("LOP"))
            {
                return ToneFrequencySweepTypeEnum.LOPowerSweep;
            }
            else
            {
                return ToneFrequencySweepTypeEnum.SweepFc;
            }
        }

        public void SetIMDSweepType(int Channel, ToneFrequencySweepTypeEnum sweeptype)
        {
            string sweep = Scpi.Format("{0}", sweeptype);
            ScpiCommand($"SENSe{Channel}:IMD:SWEep:TYPE {sweep}");
        }

        public void SetIMDSweepType(int Channel, GeneralToneFrequencySweepTypeEnum sweeptype)
        {
            string sweep = Scpi.Format("{0}", sweeptype);
            ScpiCommand($"SENSe{Channel}:IMD:SWEep:TYPE {sweep}");
        }

        // TODO
        // this command is not responding properly
        // waiting for input from R&D on how to set/get this value
        public XAxisDisplayAnnotationEnum GetAnnotation(int Channel, int Meas = 1)
        {
            string retString = ScpiQuery($"CALC{Channel}:MEAS{Meas}:MIXer:XAXis?");
            if (retString.Equals("INP"))
            {
                return XAxisDisplayAnnotationEnum.Input;
            }
            else if (retString.StartsWith("LO_1"))
            {
                return XAxisDisplayAnnotationEnum.LO1;
            }
            else if (retString.Equals("LO_2"))
            {
                return XAxisDisplayAnnotationEnum.LO2;
            }
            else
            {
                return XAxisDisplayAnnotationEnum.Output;
            }
        }

        public void SetAnnotation(int Channel, XAxisDisplayAnnotationEnum sweeptype)
        {
            string sweep = Scpi.Format("{0}", sweeptype);
            ScpiCommand($"SENSe{Channel}:SWEep:TYPE {sweep}");
        }

        public double GetIMDSweepSettingsStartfc(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:IMD:FREQuency:FCENter:STARt?");
        }

        public void SetIMDSweepSettingsStartfc(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:IMD:FREQuency:FCENter:STARt {power}");
        }

        public double GetIMDSweepSettingsStopfc(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:IMD:FREQuency:FCENter:STOP?");
        }

        public void SetIMDSweepSettingsStopfc(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:IMD:FREQuency:FCENter:STOP {power}");
        }

        public double GetIMDSweepSettingsCenterfc(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:IMD:FREQuency:FCENter:CENTer?");
        }

        public void SetIMDSweepSettingsCenterfc(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:IMD:FREQuency:FCENter:CENTer {power}");
        }

        public double GetIMDSweepSettingsSpanfc(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:IMD:FREQuency:FCENter:SPAN?");
        }

        public void SetIMDSweepSettingsSpanfc(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:IMD:FREQuency:FCENter:SPAN {power}");
        }

        public double GetIMDSweepSettingsFixedDeltaF(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:IMD:FREQuency:DFRequency?");
        }

        public void SetIMDSweepSettingsFixedDeltaF(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:IMD:FREQuency:DFRequency {power}");
        }

        public double GetIMDSweepSettingsStartDeltaF(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:IMD:FREQuency:DFRequency:STARt?");
        }

        public void SetIMDSweepSettingsStartDeltaF(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:IMD:FREQuency:DFRequency:STARt {power}");
        }

        public double GetIMDSweepSettingsStopDeltaF(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:IMD:FREQuency:DFRequency:STOP?");
        }

        public void SetIMDSweepSettingsStopDeltaF(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:IMD:FREQuency:DFRequency:STOP {power}");
        }

        public double GetIMDSweepSettingsCenterfcFixed(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:IMD:FREQuency:FCENter:CW?");
        }

        public void SetIMDSweepSettingsCenterfcFixed(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:IMD:FREQuency:FCENter:CW {power}");
        }

        public double GetIMDSweepSettingsFixedf1(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:IMD:FREQuency:F1:CW?");
        }

        public void SetIMDSweepSettingsFixedf1(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:IMD:FREQuency:F1:CW {power}");
        }

        public double GetIMDSweepSettingsFixedf2(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:IMD:FREQuency:F2:CW?");
        }

        public void SetIMDSweepSettingsFixedf2(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:IMD:FREQuency:F2:CW {power}");
        }

        public double GetIMDSweepSettingsMainToneIFBW(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:IMD:IFBWidth:MAIN?");
        }

        public void SetIMDSweepSettingsMainToneIFBW(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:IMD:IFBWidth:MAIN {power}");
        }

        public double GetIMDSweepSettingsIMToneIFBW(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:IMD:IFBWidth:IMTone?");
        }

        public void SetIMDSweepSettingsIMToneIFBW(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:IMD:IFBWidth:IMTone {power}");
        }

        // SENSe<cnum>:BANDwidth | BWIDth:TRACk:FORCe
        public bool GetLFAutoBW(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel}:BWIDth:TRACk:FORCe?");
            return !retStr.Equals("0");
        }

        public void SetLFAutoBW(int Channel, bool mode)
        {
            string stateValue = mode ? "1" : "0";
            ScpiCommand($"SENSe{Channel}:BWIDth:TRACk:FORCe {stateValue}");
        }

        #endregion

    }
}
