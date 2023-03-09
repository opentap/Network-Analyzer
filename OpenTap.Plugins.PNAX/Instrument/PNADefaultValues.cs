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
    public class StandardChannelValues
    {
        public string SweepType;
        public double Start;
        public double Stop;
        public double Power;
        public int Points;
        public double IFBandWidth;
        public double StartPower;
        public double StopPower;
        public double CWFrequency;
        public double StartPhase;
        public double StopPhase;
        public int ConverterStages;
        public int PortInput;
        public int PortOutput;
        public int InputFracMultiNumerator;
        public int InputFracMultiDenominator;
        public int Lo1FracMultiNumerator;
        public int Lo1FracMultiDenominator;
        public int Lo2FracMultiNumerator;
        public int Lo2FracMultiDenominator;
        public string PortLo1;
        public string PortLo2;
    }

    public class MixerPowerValues
    {
        public double Lo1Power;
        public double Lo2Power;
        public double SourceAttenuatorPowerPort3;
        public double ReceiverAttenuatorPowerPort3;
        public double SourceAttenuatorPowerPort4;
        public double ReceiverAttenuatorPowerPort4;
        public double LO1SweptPowerStart;
        public double LO1SweptPowerStop;
        public double LO1SweptPowerStep;
        public double LO2SweptPowerStart;
        public double LO2SweptPowerStop;
        public double LO2SweptPowerStep;
    }

    public class MixerFrequencyValues
    {
        public bool IsInputMixerFrequencyTypeStartStop;
        public bool IsInputMixerFrequencyTypeCenterSpan;
        public bool IsInputMixerFrequencyTypeFixed;
        //public double InputMixerFrequencyType = MixerFrequencyTypeEnum.StartStop;
        public double InputMixerFrequencyStart;
        public double InputMixerFrequencyStop;
        public double InputMixerFrequencyCenter;
        public double InputMixerFrequencySpan;
        public double InputMixerFrequencyFixed;
        public bool IsLO1MixerFrequencyTypeStartStop;
        public bool IsLO1MixerFrequencyTypeCenterSpan;
        public bool IsLO1MixerFrequencyTypeFixed;
        //public double LO1MixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
        public double LO1MixerFrequencyStart;
        public double LO1MixerFrequencyStop;
        public double LO1MixerFrequencyCenter;
        public double LO1MixerFrequencySpan;
        public double LO1MixerFrequencyFixed;
        public bool InputGTLO1;
        public bool IsIFMixerFrequencyTypeStartStop;
        public bool IsIFMixerFrequencyTypeCenterSpan;
        public bool IsIFMixerFrequencyTypeFixed;
        //public double IFSidebandType = SidebandTypeEnum.Low;
        //public double IFMixerFrequencyType = MixerFrequencyTypeEnum.CenterSpan;
        public double IFMixerFrequencyStart;
        public double IFMixerFrequencyStop;
        public double IFMixerFrequencyCenter;
        public double IFMixerFrequencySpan;
        public double IFMixerFrequencyFixed;
        public bool IsLO2MixerFrequencyTypeStartStop;
        public bool IsLO2MixerFrequencyTypeCenterSpan;
        public bool IsLO2MixerFrequencyTypeFixed;
        //public double LO2MixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
        public double LO2MixerFrequencyStart;
        public double LO2MixerFrequencyStop;
        public double LO2MixerFrequencyCenter;
        public double LO2MixerFrequencySpan;
        public double LO2MixerFrequencyFixed;
        public bool IF1GTLO2 = true;
        public bool IsOutputMixerFrequencyTypeStartStop;
        public bool IsOutputMixerFrequencyTypeCenterSpan;
        public bool IsOutputMixerFrequencyTypeFixed;
        //public double OutputSidebandType = SidebandTypeEnum.Low;
        //public double OutputMixerFrequencyType = MixerFrequencyTypeEnum.CenterSpan;
        public double OutputMixerFrequencyStart;
        public double OutputMixerFrequencyStop;
        public double OutputMixerFrequencyCenter;
        public double OutputMixerFrequencySpan;
        public double OutputMixerFrequencyFixed;
    }
    public partial class PNAX : ScpiInstrument
    {
        public StandardChannelValues GetStandardChannelValues(bool force = false)
        {
            if (force)
                this.UpdateDefaultValues();

            return standardChannelValues;
        }

        public MixerPowerValues GetMixerPowerValues()
        {
            return mixerPowerValues;
        }

        public MixerFrequencyValues GetMixerFrequencyValues()
        {
            return mixerFrequencyValues;
        }

        private void UpdateStandardValues()
        {
            if (standardChannelValues == null)
                standardChannelValues = new StandardChannelValues();

            Open();
            Log.Info("Getting default values for Standard Channel");

            this.ScpiCommand("SYSTem:PRESet");
            this.WaitForOperationComplete();

            // Channel 1 is Standard
            //string sweepType = GetStandardSweepType(1);
            //double start = GetStart(1);
            //double stop = GetStop(1);
            //double power = GetPower(1);
            //int points = GetPoints(1);
            //double ifbw = GetIFBandwidth(1);
            //double startpower = GetStartPower(1);
            //double stoppower = GetStopPower(1);
            //double cwfreq = GetCWFreq(1);
            //double startphase = GetPhaseStart(1);
            //double stopphase = GetPhaseStop(1);

            standardChannelValues.SweepType = GetStandardSweepType(1);
            standardChannelValues.Start = GetStart(1);
            standardChannelValues.Stop = GetStop(1);
            standardChannelValues.Power = GetPower(1);
            standardChannelValues.Points = GetPoints(1);
            standardChannelValues.IFBandWidth = GetIFBandwidth(1);
            standardChannelValues.StartPower = GetStartPower(1);
            standardChannelValues.StopPower = GetStopPower(1);
            standardChannelValues.StartPhase = GetPhaseStart(1);
            standardChannelValues.StopPhase = GetPhaseStop(1);
            standardChannelValues.CWFrequency = GetCWFreq(1);

            //Log.Info($"Sweep Type: {sweepType}");
            //Log.Info($"Start: {start}");
            //Log.Info($"Stop: {stop}");
            //Log.Info($"Power: {power}");
            //Log.Info($"Number of Points: {points}");
            //Log.Info($"IF Bandwidth: {ifbw}");
            //Log.Info($"Start Power: {startpower}");
            //Log.Info($"Stop Power: {stoppower}");
            //Log.Info($"CW Freq: {cwfreq}");
            //Log.Info($"Start Phase: {startphase}");
            //Log.Info($"Stop Phase: {stopphase}");

            Log.Info("Getting default values for Gain Compression Converters Channel");
            // Lets create Channel 2 - Gain Compression Converters
            ScpiCommand("CALC2:MEAS2:DEF \"SC21:Gain Compression Converters\"");
            WaitForOperationComplete();

            //int converterstages = GetConverterStages(2);
            //int portinput = GetPortInput(2);
            //int portoutput = GetPortOutput(2);
            //int inputfractionalmultipliernumerator = GetInputFractionalMultiplierNumerator(2);
            //int inputfractionalmultiplierdenominator = GetInputFractionalMultiplierDenominator(2);
            //int lo1fractionalmultipliernumerator = GetLOFractionalMultiplierNumerator(2, 1);
            //int lo1fractionalmultiplierdenominator = GetLOFractionalMultiplierDenominator(2, 1);
            ////int lo2fractionalmultipliernumerator = GetLOFractionalMultiplierNumerator(2, 2);
            ////int lo2fractionalmultiplierdenominator = GetLOFractionalMultiplierDenominator(2, 2);
            //String portlo1 = GetPortLO(2, 1);
            ////String portlo2 = GetPortLO(2, 2);

            standardChannelValues.ConverterStages = GetConverterStages(2);
            standardChannelValues.PortInput = GetPortInput(2);
            standardChannelValues.InputFracMultiNumerator = GetInputFractionalMultiplierNumerator(2);
            standardChannelValues.InputFracMultiDenominator = GetInputFractionalMultiplierDenominator(2);
            standardChannelValues.Lo1FracMultiNumerator = GetLOFractionalMultiplierNumerator(2, 1);
            standardChannelValues.Lo1FracMultiDenominator = GetLOFractionalMultiplierDenominator(2, 1);
            standardChannelValues.Lo2FracMultiNumerator = GetLOFractionalMultiplierNumerator(2, 2);
            standardChannelValues.Lo2FracMultiDenominator = GetLOFractionalMultiplierDenominator(2, 2);
            standardChannelValues.PortLo1 = GetPortLO(2, 1);
            standardChannelValues.PortLo2 = GetPortLO(2, 2);

            //String portlo2 = GetPortLO(2, 2);
            //Log.Info($"Converter Stages: {converterstages}");
            //Log.Info($"Port Input: {portinput}");
            //Log.Info($"Port Output: {portoutput}");
            //Log.Info($"Input Fractional Multiplier Numerator: {inputfractionalmultipliernumerator}");
            //Log.Info($"Input Fractional Multiplier Denominator: {inputfractionalmultiplierdenominator}");
            //Log.Info($"LO1 Fractional Multiplier Numerator: {lo1fractionalmultipliernumerator}");
            //Log.Info($"LO1 Fractional Multiplier Denominator: {lo1fractionalmultiplierdenominator}");
            ////Log.Info($"LO2 Fractional Multiplier Numerator: {lo2fractionalmultipliernumerator}");
            ////Log.Info($"LO2 Fractional Multiplier Denominator: {lo2fractionalmultiplierdenominator}");
            //Log.Info($"Port LO1: {portlo1}");

            Close();
        }

        private void UpdateMixerPowerValues()
        {
            if (mixerPowerValues == null)
                mixerPowerValues = new MixerPowerValues();

            mixerPowerValues.Lo1Power = -15;
            mixerPowerValues.Lo2Power = -15;
            mixerPowerValues.SourceAttenuatorPowerPort3 = 0;
            mixerPowerValues.ReceiverAttenuatorPowerPort3 = 0;
            mixerPowerValues.SourceAttenuatorPowerPort4 = 0;
            mixerPowerValues.ReceiverAttenuatorPowerPort4 = 0;
            mixerPowerValues.LO1SweptPowerStart = -20;
            mixerPowerValues.LO1SweptPowerStop = -10;
            mixerPowerValues.LO1SweptPowerStep = 0.05;
            mixerPowerValues.LO2SweptPowerStart = -10;
            mixerPowerValues.LO2SweptPowerStop = -10;
            mixerPowerValues.LO2SweptPowerStep = 0.0;
        }

        private void UpdateMixerFrequencyValues()
        {
            if (mixerFrequencyValues == null)
                mixerFrequencyValues = new MixerFrequencyValues();

            mixerFrequencyValues.IsInputMixerFrequencyTypeStartStop = true;
            mixerFrequencyValues.IsInputMixerFrequencyTypeCenterSpan = false;
            mixerFrequencyValues.IsInputMixerFrequencyTypeFixed = false;

            //InputMixerFrequencyType = MixerFrequencyTypeEnum.StartStop;
            mixerFrequencyValues.InputMixerFrequencyStart = 10.5e6;
            mixerFrequencyValues.InputMixerFrequencyStop = 66.9995e9;
            mixerFrequencyValues.InputMixerFrequencyCenter = 33.505e9;
            mixerFrequencyValues.InputMixerFrequencySpan = 66.989e9;
            mixerFrequencyValues.InputMixerFrequencyFixed = 1e9;

            mixerFrequencyValues.IsLO1MixerFrequencyTypeStartStop = false;
            mixerFrequencyValues.IsLO1MixerFrequencyTypeCenterSpan = false;
            mixerFrequencyValues.IsLO1MixerFrequencyTypeFixed = true;

            //LO1MixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
            mixerFrequencyValues.LO1MixerFrequencyStart = 0;
            mixerFrequencyValues.LO1MixerFrequencyStop = 0;
            mixerFrequencyValues.LO1MixerFrequencyCenter = 0;
            mixerFrequencyValues.LO1MixerFrequencySpan = 0;
            mixerFrequencyValues.LO1MixerFrequencyFixed = 0;
            mixerFrequencyValues.InputGTLO1 = true;

            mixerFrequencyValues.IsIFMixerFrequencyTypeStartStop = false;
            mixerFrequencyValues.IsIFMixerFrequencyTypeCenterSpan = true;
            mixerFrequencyValues.IsIFMixerFrequencyTypeFixed = false;

            //IFSidebandType = SidebandTypeEnum.Low;
            //IFMixerFrequencyType = MixerFrequencyTypeEnum.CenterSpan;
            mixerFrequencyValues.IFMixerFrequencyStart = 10.5e6;
            mixerFrequencyValues.IFMixerFrequencyStop = 66.9995e9;
            mixerFrequencyValues.IFMixerFrequencyCenter = 33.505e9;
            mixerFrequencyValues.IFMixerFrequencySpan = 66.989e9;
            mixerFrequencyValues.IFMixerFrequencyFixed = 10e6;

            mixerFrequencyValues.IsLO2MixerFrequencyTypeStartStop = false;
            mixerFrequencyValues.IsLO2MixerFrequencyTypeCenterSpan = false;
            mixerFrequencyValues.IsLO2MixerFrequencyTypeFixed = true;

            //mixerFrequencyValues.LO2MixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
            mixerFrequencyValues.LO2MixerFrequencyStart = 0;
            mixerFrequencyValues.LO2MixerFrequencyStop = 0;
            mixerFrequencyValues.LO2MixerFrequencyCenter = 0;
            mixerFrequencyValues.LO2MixerFrequencySpan = 0;
            mixerFrequencyValues.LO2MixerFrequencyFixed = 0;
            mixerFrequencyValues.IF1GTLO2 = true;


            mixerFrequencyValues.IsOutputMixerFrequencyTypeStartStop = false;
            mixerFrequencyValues.IsOutputMixerFrequencyTypeCenterSpan = true;
            mixerFrequencyValues.IsOutputMixerFrequencyTypeFixed = false;

            //mixerFrequencyValues.OutputSidebandType = SidebandTypeEnum.Low;
            //mixerFrequencyValues.OutputMixerFrequencyType = MixerFrequencyTypeEnum.CenterSpan;
            mixerFrequencyValues.OutputMixerFrequencyStart = 10.5e6;
            mixerFrequencyValues.OutputMixerFrequencyStop = 66.9995e9;
            mixerFrequencyValues.OutputMixerFrequencyCenter = 33.505e9;
            mixerFrequencyValues.OutputMixerFrequencySpan = 66.989e9;
            mixerFrequencyValues.OutputMixerFrequencyFixed = 10e6;
        }
    }
}
