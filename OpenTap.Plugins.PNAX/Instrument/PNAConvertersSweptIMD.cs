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
            retVal = ScpiQuery<int>($"SENS{Channel.ToString()}:IMD:PMAP:INP?");
            return retVal;
        }

        public int GetIMDPortOutput(int Channel)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENS{Channel.ToString()}:IMD:PMAP:OUTP?");
            return retVal;
        }

        public void SetIMDPortInputOutput(int Channel, DutInputPortsEnum inport, DutOutputPortsEnum outport)
        {
            String inp = Scpi.Format("{0}", inport);
            String outp = Scpi.Format("{0}", outport);
            ScpiCommand($"SENS{Channel.ToString()}:IMD:PMAP {inp},{outp}");
        }

        public bool GetIMDCoupledTonePowers(int Channel)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:IMD:TPOWer:COUPle?");
            if (retStr.Equals("0"))
            {
                retVal = false;
            }
            else
            {
                retVal = true;
            }
            return retVal;
        }

        public void SetIMDCoupledTonePowers(int Channel, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:IMD:TPOWer:COUPle 1");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:IMD:TPOWer:COUPle 0");
            }
        }

        // SOURce<cnum>:POWer<port>:ALC:MODE?
        public bool GetIMDALCHardware(int Channel, int Port)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SOURce{Channel.ToString()}:POWer{Port.ToString()}:ALC:MODE?");
            if (retStr.Equals("OPEN"))
            {
                retVal = false;
            }
            else if (retStr.Equals("INT"))
            {
                retVal = true;
            }
            else
            {
                throw new Exception("Undefined ALC mode");
            }
            return retVal;
        }

        // TODO same as SetSourceLevelingMode
        public void SetIMDALCHardware(int Channel, int Port, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SOURce{Channel.ToString()}:POWer{Port.ToString()}:ALC:MODE INT");
            }
            else
            {
                ScpiCommand($"SOURce{Channel.ToString()}:POWer{Port.ToString()}:ALC:MODE OPEN");
            }
        }

        // SENSe<cnum>:IMD:TPOWer:LEVel
        public PowerLevelingEnum GetIMDPowerLevelingMode(int Channel)
        {
            PowerLevelingEnum retVal = PowerLevelingEnum.SetInputPower;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:IMD:TPOWer:LEVel?");
            if (retStr.Equals("NONE"))
            {
                retVal = PowerLevelingEnum.SetInputPower;
            }
            else if (retStr.Equals("INP"))
            {
                retVal = PowerLevelingEnum.SetInputPowerReceiverLeveling;
            }
            else if (retStr.Equals("EQU"))
            {
                retVal = PowerLevelingEnum.SetInputPowerEqualTonesAtOutput;
            }
            else if (retStr.Equals("OUTP"))
            {
                retVal = PowerLevelingEnum.SetOutputPowerReceiverLeveling;
            }
            else
            {
                throw new Exception("Undefined ALC mode");
            }
            return retVal;
        }

        public void SetIMDPowerLevelingMode(int Channel, PowerLevelingEnum mode)
        {
            if (mode == PowerLevelingEnum.SetInputPower)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:IMD:TPOWer:LEVel NONE");
            }
            else if (mode == PowerLevelingEnum.SetInputPowerReceiverLeveling)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:IMD:TPOWer:LEVel INP");
            }
            else if (mode == PowerLevelingEnum.SetInputPowerEqualTonesAtOutput)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:IMD:TPOWer:LEVel EQU");
            }
            else if (mode == PowerLevelingEnum.SetOutputPowerReceiverLeveling)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:IMD:TPOWer:LEVel OUTP");
            }
        }

        // SOURce<ch>:POWer<port>:ALC[:MODE]:RECeiver[:STATe]? [src]
        public bool GetIMDReceiverLevelingMode(int Channel, int Port)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SOURce{Channel.ToString()}:POWer{Port.ToString()}:ALC:RECeiver?");
            if (retStr.Equals("0"))
            {
                retVal = false;
            }
            else
            {
                retVal = true;
            }
            return retVal;
        }

        public void SetIMDReceiverLevelingMode(int Channel, int Port, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SOURce{Channel.ToString()}:POWer{Port.ToString()}:ALC:RECeiver 1");
            }
            else
            {
                ScpiCommand($"SOURce{Channel.ToString()}:POWer{Port.ToString()}:ALC:RECeiver 0");
            }
        }


        public double GetIMDFixedPowerF1(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:IMD:TPOWer:F1?");
            return retVal;
        }

        public void SetIMDFixedPowerF1(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:IMD:TPOWer:F1 {power.ToString()}");
        }

        public double GetIMDFixedPowerF2(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:IMD:TPOWer:F2?");
            return retVal;
        }

        public void SetIMDFixedPowerF2(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:IMD:TPOWer:F2 {power.ToString()}");
        }

        public double GetIMDStartPowerF1(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:IMD:TPOWer:F1:STARt?");
            return retVal;
        }

        public void SetIMDStartPowerF1(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:IMD:TPOWer:F1:STARt {power.ToString()}");
        }

        public double GetIMDStartPowerF2(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:IMD:TPOWer:F2:STARt?");
            return retVal;
        }

        public void SetIMDStartPowerF2(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:IMD:TPOWer:F2:STARt {power.ToString()}");
        }

        public double GetIMDStopPowerF1(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:IMD:TPOWer:F1:STOP?");
            return retVal;
        }

        public void SetIMDStopPowerF1(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:IMD:TPOWer:F1:STOP {power.ToString()}");
        }

        public double GetIMDStopPowerF2(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:IMD:TPOWer:F2:STOP?");
            return retVal;
        }

        public void SetIMDStopPowerF2(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:IMD:TPOWer:F2:STOP {power.ToString()}");
        }
        #endregion

        #region Tone Frequency
        public ToneFrequencySweepTypeEnum GetIMDSweepType(int Channel)
        {
            ToneFrequencySweepTypeEnum retVal = ToneFrequencySweepTypeEnum.SweepFc;
            String retString = ScpiQuery($"SENSe{Channel.ToString()}:IMD:SWEep:TYPE?");
            if (retString.Equals("FCEN"))
            {
                retVal = ToneFrequencySweepTypeEnum.SweepFc;
            }
            else if (retString.Equals("DFR"))
            {
                retVal = ToneFrequencySweepTypeEnum.SweepDeltaF;
            }
            else if (retString.StartsWith("POW"))
            {
                retVal = ToneFrequencySweepTypeEnum.PowerSweep;
            }
            else if (retString.Equals("CW"))
            {
                retVal = ToneFrequencySweepTypeEnum.CW;
            }
            else if (retString.Equals("LOP"))
            {
                retVal = ToneFrequencySweepTypeEnum.LOPowerSweep;
            }
            return retVal;
        }

        public void SetIMDSweepType(int Channel, ToneFrequencySweepTypeEnum sweeptype)
        {
            String sweep = Scpi.Format("{0}", sweeptype);
            ScpiCommand($"SENSe{Channel.ToString()}:IMD:SWEep:TYPE {sweep}");
        }

        // TODO
        // this command is not responding properly
        // waiting for input from R&D on how to set/get this value
        public XAxisDisplayAnnotationEnum GetAnnotation(int Channel, int Meas = 1)
        {
            XAxisDisplayAnnotationEnum retVal = XAxisDisplayAnnotationEnum.Output;
            String retString = ScpiQuery($"CALC{Channel.ToString()}:MEAS{Meas.ToString()}:MIXer:XAXis?");
            if (retString.Equals("OUTP"))
            {
                retVal = XAxisDisplayAnnotationEnum.Output;
            }
            else if (retString.Equals("INP"))
            {
                retVal = XAxisDisplayAnnotationEnum.Input;
            }
            else if (retString.StartsWith("LO_1"))
            {
                retVal = XAxisDisplayAnnotationEnum.LO1;
            }
            else if (retString.Equals("LO_2"))
            {
                retVal = XAxisDisplayAnnotationEnum.LO2;
            }
            return retVal;
        }

        public void SetAnnotation(int Channel, XAxisDisplayAnnotationEnum sweeptype)
        {
            String sweep = Scpi.Format("{0}", sweeptype);
            ScpiCommand($"SENSe{Channel.ToString()}:SWEep:TYPE {sweep}");
        }

        public double GetIMDSweepSettingsStartfc(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:IMD:FREQuency:FCENter:STARt?");
            return retVal;
        }

        public void SetIMDSweepSettingsStartfc(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:IMD:FREQuency:FCENter:STARt {power.ToString()}");
        }

        public double GetIMDSweepSettingsStopfc(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:IMD:FREQuency:FCENter:STOP?");
            return retVal;
        }

        public void SetIMDSweepSettingsStopfc(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:IMD:FREQuency:FCENter:STOP {power.ToString()}");
        }

        public double GetIMDSweepSettingsCenterfc(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:IMD:FREQuency:FCENter:CENTer?");
            return retVal;
        }

        public void SetIMDSweepSettingsCenterfc(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:IMD:FREQuency:FCENter:CENTer {power.ToString()}");
        }

        public double GetIMDSweepSettingsSpanfc(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:IMD:FREQuency:FCENter:SPAN?");
            return retVal;
        }

        public void SetIMDSweepSettingsSpanfc(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:IMD:FREQuency:FCENter:SPAN {power.ToString()}");
        }

        public double GetIMDSweepSettingsFixedDeltaF(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:IMD:FREQuency:DFRequency?");
            return retVal;
        }

        public void SetIMDSweepSettingsFixedDeltaF(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:IMD:FREQuency:DFRequency {power.ToString()}");
        }

        public double GetIMDSweepSettingsStartDeltaF(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:IMD:FREQuency:DFRequency:STARt?");
            return retVal;
        }

        public void SetIMDSweepSettingsStartDeltaF(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:IMD:FREQuency:DFRequency:STARt {power.ToString()}");
        }

        public double GetIMDSweepSettingsStopDeltaF(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:IMD:FREQuency:DFRequency:STOP?");
            return retVal;
        }

        public void SetIMDSweepSettingsStopDeltaF(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:IMD:FREQuency:DFRequency:STOP {power.ToString()}");
        }

        public double GetIMDSweepSettingsCenterfcFixed(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:IMD:FREQuency:FCENter:CW?");
            return retVal;
        }

        public void SetIMDSweepSettingsCenterfcFixed(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:IMD:FREQuency:FCENter:CW {power.ToString()}");
        }

        public double GetIMDSweepSettingsFixedf1(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:IMD:FREQuency:F1:CW?");
            return retVal;
        }

        public void SetIMDSweepSettingsFixedf1(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:IMD:FREQuency:F1:CW {power.ToString()}");
        }

        public double GetIMDSweepSettingsFixedf2(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:IMD:FREQuency:F2:CW?");
            return retVal;
        }

        public void SetIMDSweepSettingsFixedf2(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:IMD:FREQuency:F2:CW {power.ToString()}");
        }

        public double GetIMDSweepSettingsMainToneIFBW(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:IMD:IFBWidth:MAIN?");
            return retVal;
        }

        public void SetIMDSweepSettingsMainToneIFBW(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:IMD:IFBWidth:MAIN {power.ToString()}");
        }

        public double GetIMDSweepSettingsIMToneIFBW(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:IMD:IFBWidth:IMTone?");
            return retVal;
        }

        public void SetIMDSweepSettingsIMToneIFBW(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:IMD:IFBWidth:IMTone {power.ToString()}");
        }

        // SENSe<cnum>:BANDwidth | BWIDth:TRACk:FORCe
        public bool GetLFAutoBW(int Channel)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:BWIDth:TRACk:FORCe?");
            if (retStr.Equals("0"))
            {
                retVal = false;
            }
            else
            {
                retVal = true;
            }
            return retVal;
        }

        public void SetLFAutoBW(int Channel, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:BWIDth:TRACk:FORCe 1");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:BWIDth:TRACk:FORCe 0");
            }
        }

        #endregion

    }
}
