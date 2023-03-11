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

    public enum ConverterStagesEnum
    {
        [Display("1")]
        _1,
        [Display("2")]
        _2
    }

    public enum PortsEnum
    {
        [Scpi("1")]
        Port1,
        [Scpi("2")]
        Port2,
        [Scpi("3")]
        Port3,
        [Scpi("4")]
        Port4
    }

    public enum LOEnum
    {
        [Scpi("Not controlled")]
        NotControlled,
        [Scpi("Port 3")]
        Port3,
        [Scpi("Port 4")]
        Port4,
        [Scpi("Source 3")]
        Source3
    }

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
        public ConverterStagesEnum ConverterStages;
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

    public class MixerSetupValues
    {
        public ConverterStagesEnum ConverterStages;
        public PortsEnum PortInput;
        public PortsEnum PortOutput;
        public LOEnum PortLO1;
        public LOEnum PortLO2;
        public int InputFractionalMultiplierNumerator;
        public int InputFractionalMultiplierDenominator;
        public int LO1FractionalMultiplierNumerator;
        public int LO1FractionalMultiplierDenominator;
        public int LO2FractionalMultiplierNumerator;
        public int LO2FractionalMultiplierDenominator;
        public bool EnableEmbeddedLO;
        public TuningMethodEnum TuningMethod;
        public TuningPointTypeEnum TuningPointType;
        public int TuningPoint;
        public int TuneEvery;
        public int BroadBandSearch;
        public int IFBW;
        public int MaxIterations;
        public int Tolerance;
        public double LOFrequencyDelta;

        public static MixerSetupValues GetPresetValues()
        {
            MixerSetupValues mixerSetupValues = new MixerSetupValues();
            mixerSetupValues.ConverterStages = ConverterStagesEnum._1;
            mixerSetupValues.PortInput = PortsEnum.Port1;
            mixerSetupValues.PortOutput = PortsEnum.Port2;
            mixerSetupValues.PortLO1 = LOEnum.NotControlled;
            mixerSetupValues.PortLO2 = LOEnum.NotControlled;
            mixerSetupValues.InputFractionalMultiplierNumerator = 1;
            mixerSetupValues.InputFractionalMultiplierDenominator = 1;
            mixerSetupValues.LO1FractionalMultiplierNumerator = 1;
            mixerSetupValues.LO1FractionalMultiplierDenominator = 1;
            mixerSetupValues.LO2FractionalMultiplierNumerator = 1;
            mixerSetupValues.LO2FractionalMultiplierDenominator = 1;
            mixerSetupValues.EnableEmbeddedLO = false;
            mixerSetupValues.TuningMethod = TuningMethodEnum.BroadbandAndPrecise;
            mixerSetupValues.TuningPointType = TuningPointTypeEnum.MiddlePoint;
            mixerSetupValues.TuningPoint = 101;
            mixerSetupValues.TuneEvery = 1;
            mixerSetupValues.BroadBandSearch = 3000000;
            mixerSetupValues.IFBW = 30000;
            mixerSetupValues.MaxIterations = 5;
            mixerSetupValues.Tolerance = 1;
            mixerSetupValues.LOFrequencyDelta = 0;
            return mixerSetupValues;
        }
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

        public static MixerPowerValues GetPresetValues()
        {
            MixerPowerValues mixerPowerValues = new MixerPowerValues();
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
            return mixerPowerValues;
        }
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

        public static MixerFrequencyValues GetPresetValues()
        {
            MixerFrequencyValues mixerFrequencyValues = new MixerFrequencyValues();

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

            return mixerFrequencyValues;
        }
    }

    public class ToneFrequencyValues
    {
        public ToneFrequencySweepTypeEnum ToneFrequencySweepType;
        public double SweepFcStartFc;
        public double SweepFcStopFc;
        public double SweepFcCenterFc;
        public double SweepFcSpanFc;
        public double SweepFcFixedDeltaF;
        public int SweepFcNumberOfPoints;
        public double SweepFcMixedToneIFBW;
        public double SweepFcIMToneIFBW;
        public bool SweepFcReduceIFBW;
        public double SweepDeltaFStartDeltaF;
        public double SweepDeltaFStopDeltaF;
        public double SweepDeltaFFixedFc;
        public double PowerSweepCWF1;
        public double PowerSweepCWF2;
        public double PowerSweepCWFc;
        public double PowerSweepCWDeltaF;

        public static ToneFrequencyValues GetPresetValues()
        {
            ToneFrequencyValues toneFrequencyValues = new ToneFrequencyValues();
            toneFrequencyValues.ToneFrequencySweepType = ToneFrequencySweepTypeEnum.SweepFc;
            toneFrequencyValues.SweepFcStartFc = 10.5e6;
            toneFrequencyValues.SweepFcStopFc = 66.9995e9;
            toneFrequencyValues.SweepFcCenterFc = 33.505e9;
            toneFrequencyValues.SweepFcSpanFc = 66.9995e9;
            toneFrequencyValues.SweepFcFixedDeltaF = 1e6;
            toneFrequencyValues.SweepFcNumberOfPoints = 201;
            toneFrequencyValues.SweepFcMixedToneIFBW = 1e3;
            toneFrequencyValues.SweepFcIMToneIFBW = 1e3;
            toneFrequencyValues.SweepFcReduceIFBW = true;
            toneFrequencyValues.SweepDeltaFStartDeltaF = 1e6;
            toneFrequencyValues.SweepDeltaFStopDeltaF = 10e6;
            toneFrequencyValues.SweepDeltaFFixedFc = 1e9;
            toneFrequencyValues.PowerSweepCWF1 = 999.5e6;
            toneFrequencyValues.PowerSweepCWF2 = 1.0005e9;
            toneFrequencyValues.PowerSweepCWFc = 1e9;
            toneFrequencyValues.PowerSweepCWDeltaF = 1e6;
            return toneFrequencyValues;
        }
    }

    public class TonePowerValues
    {
        public double FixedF1Power;
        public double FixedF2Power;
        public double StartF1Power;
        public double StartF2Power;
        public double StopF1Power;
        public double StopF2Power;
        public double StepF1Power;
        public double StepF2Power;
        public static TonePowerValues GetPresetValues()
        {
            TonePowerValues tonePowerValues = new TonePowerValues();
            tonePowerValues.FixedF1Power = -24;
            tonePowerValues.FixedF2Power = -24;
            tonePowerValues.StartF1Power = -24;
            tonePowerValues.StartF2Power = -24;
            tonePowerValues.StopF1Power = -10;
            tonePowerValues.StopF2Power = -10;
            tonePowerValues.StepF1Power = 0.070;
            tonePowerValues.StepF2Power = 0.070;
            return tonePowerValues;
        }
    }

    public class CompressionValues
    {
        public CompressionMethodEnum CompressionMethod;
        public double CompressionLevel;
        public double CompressionBackOff;
        public double CompressionDeltaX;
        public double CompressionDeltaY;
        public double CompressionFromMaxPout;
        public double SMARTSweepTolerance;
        public int SMARTSweepIterations;
        public bool SMARTSweepShowIterations;
        public bool SMARTSweepReadDC;
        public bool SMARTSweepSafeMode;
        public int SMARTSweepCoarseIncrement;
        public double SMARTSweepFineIncrement;
        public double SMARTSweepFineThreshold;
        public double SMARTSweepMaxOutputPower;
        public bool CompressionPointInterpolation;
        public EndOfSweepConditionEnum EndOfSweepCondition;
        public double SettlingTime;

        public static CompressionValues GetPresetValues()
        {
            CompressionValues compressionValues = new CompressionValues();
            compressionValues.CompressionMethod = CompressionMethodEnum.CompressionFromLinearGain;
            compressionValues.CompressionLevel = 1;
            compressionValues.CompressionBackOff = 10;
            compressionValues.CompressionDeltaX = 10;
            compressionValues.CompressionDeltaY = 9;
            compressionValues.CompressionFromMaxPout = 0.1;
            compressionValues.SMARTSweepTolerance = 0.05;
            compressionValues.SMARTSweepIterations = 20;
            compressionValues.SMARTSweepShowIterations = false;
            compressionValues.SMARTSweepReadDC = false;
            compressionValues.SMARTSweepSafeMode = false;
            compressionValues.SMARTSweepCoarseIncrement = 3;
            compressionValues.SMARTSweepFineIncrement = 1;
            compressionValues.SMARTSweepFineThreshold = 0.5;
            compressionValues.SMARTSweepMaxOutputPower = 30;
            compressionValues.CompressionPointInterpolation = false;
            compressionValues.EndOfSweepCondition = EndOfSweepConditionEnum.Default;
            compressionValues.SettlingTime = 0;
            return compressionValues;
        }
    }


    public partial class PNAX : ScpiInstrument
    {
        public StandardChannelValues GetStandardChannelValues(bool force = false)
        {
            if (force)
                this.UpdateDefaultValues();

            return DefaultStandardChannelValues;
        }
        private void UpdateStandardValues()
        {
            if (DefaultStandardChannelValues == null)
                DefaultStandardChannelValues = new StandardChannelValues();
            try
            {
                Open();
                Log.Info("Getting default values for Standard Channel");

                this.ScpiCommand("SYSTem:PRESet");
                this.WaitForOperationComplete();

                // Channel 1 is Standard
                DefaultStandardChannelValues.SweepType = GetStandardSweepType(1);
                DefaultStandardChannelValues.Start = GetStart(1);
                DefaultStandardChannelValues.Stop = GetStop(1);
                DefaultStandardChannelValues.Power = GetPower(1);
                DefaultStandardChannelValues.Points = GetPoints(1);
                DefaultStandardChannelValues.IFBandWidth = GetIFBandwidth(1);
                DefaultStandardChannelValues.StartPower = GetStartPower(1);
                DefaultStandardChannelValues.StopPower = GetStopPower(1);
                DefaultStandardChannelValues.StartPhase = GetPhaseStart(1);
                DefaultStandardChannelValues.StopPhase = GetPhaseStop(1);
                DefaultStandardChannelValues.CWFrequency = GetCWFreq(1);

                Log.Info("Getting default values for Gain Compression Converters Channel");
                // Lets create Channel 2 - Gain Compression Converters
                ScpiCommand("CALC2:MEAS2:DEF \"SC21:Gain Compression Converters\"");
                WaitForOperationComplete();

                DefaultStandardChannelValues.ConverterStages = GetConverterStages(2);
                DefaultStandardChannelValues.PortInput = GetPortInput(2);
                DefaultStandardChannelValues.InputFracMultiNumerator = GetInputFractionalMultiplierNumerator(2);
                DefaultStandardChannelValues.InputFracMultiDenominator = GetInputFractionalMultiplierDenominator(2);
                DefaultStandardChannelValues.Lo1FracMultiNumerator = GetLOFractionalMultiplierNumerator(2, 1);
                DefaultStandardChannelValues.Lo1FracMultiDenominator = GetLOFractionalMultiplierDenominator(2, 1);
                DefaultStandardChannelValues.Lo2FracMultiNumerator = GetLOFractionalMultiplierNumerator(2, 2);
                DefaultStandardChannelValues.Lo2FracMultiDenominator = GetLOFractionalMultiplierDenominator(2, 2);
                DefaultStandardChannelValues.PortLo1 = GetPortLO(2, 1);
                DefaultStandardChannelValues.PortLo2 = GetPortLO(2, 2);

                Close();
            }
            catch (Exception)
            {
                Log.Error("Cannot update default values, use preset instead");
                return;
            }

        }

        public MixerSetupValues GetMixerSetupDefaultValues()
        {
            if (DefaultMixerSetupValues == null)
                return MixerSetupValues.GetPresetValues();
            return DefaultMixerSetupValues;
        }
        private void UpdateMixerSetupValues()
        {
            if (DefaultMixerSetupValues == null)
                DefaultMixerSetupValues = new MixerSetupValues();

            DefaultMixerSetupValues.ConverterStages = ConverterStagesEnum._1;
            DefaultMixerSetupValues.PortInput = PortsEnum.Port1;
            DefaultMixerSetupValues.PortOutput = PortsEnum.Port2;
            DefaultMixerSetupValues.PortLO1 = LOEnum.NotControlled;
            DefaultMixerSetupValues.PortLO2 = LOEnum.NotControlled;
            DefaultMixerSetupValues.InputFractionalMultiplierNumerator = 1;
            DefaultMixerSetupValues.InputFractionalMultiplierDenominator = 1;
            DefaultMixerSetupValues.LO1FractionalMultiplierNumerator = 1;
            DefaultMixerSetupValues.LO1FractionalMultiplierDenominator = 1;
            DefaultMixerSetupValues.LO2FractionalMultiplierNumerator = 1;
            DefaultMixerSetupValues.LO2FractionalMultiplierDenominator = 1;
            DefaultMixerSetupValues.EnableEmbeddedLO = false;
            DefaultMixerSetupValues.TuningMethod = TuningMethodEnum.BroadbandAndPrecise;
            DefaultMixerSetupValues.TuningPointType = TuningPointTypeEnum.MiddlePoint;
            DefaultMixerSetupValues.TuningPoint = 101;
            DefaultMixerSetupValues.TuneEvery = 1;
            DefaultMixerSetupValues.BroadBandSearch = 3000000;
            DefaultMixerSetupValues.IFBW = 30000;
            DefaultMixerSetupValues.MaxIterations = 5;
            DefaultMixerSetupValues.Tolerance = 1;
            DefaultMixerSetupValues.LOFrequencyDelta = 0;
        }

        public MixerPowerValues GetMixerPowerDefaultValues()
        {
            if (DefaultMixerPowerValues == null)
                return MixerPowerValues.GetPresetValues();
            return DefaultMixerPowerValues;
        }
        private void UpdateMixerPowerValues()
        {
            if (DefaultMixerPowerValues == null)
                DefaultMixerPowerValues = new MixerPowerValues();

            DefaultMixerPowerValues.Lo1Power = -15;
            DefaultMixerPowerValues.Lo2Power = -15;
            DefaultMixerPowerValues.SourceAttenuatorPowerPort3 = 0;
            DefaultMixerPowerValues.ReceiverAttenuatorPowerPort3 = 0;
            DefaultMixerPowerValues.SourceAttenuatorPowerPort4 = 0;
            DefaultMixerPowerValues.ReceiverAttenuatorPowerPort4 = 0;
            DefaultMixerPowerValues.LO1SweptPowerStart = -20;
            DefaultMixerPowerValues.LO1SweptPowerStop = -10;
            DefaultMixerPowerValues.LO1SweptPowerStep = 0.05;
            DefaultMixerPowerValues.LO2SweptPowerStart = -10;
            DefaultMixerPowerValues.LO2SweptPowerStop = -10;
            DefaultMixerPowerValues.LO2SweptPowerStep = 0.0;
        }

        public MixerFrequencyValues GetMixerFrequencyDefaultValues()
        {
            if (DefaultMixerFrequencyValues == null)
                return MixerFrequencyValues.GetPresetValues();
            return DefaultMixerFrequencyValues;
        }
        private void UpdateMixerFrequencyValues()
        {
            if (DefaultMixerFrequencyValues == null)
                DefaultMixerFrequencyValues = new MixerFrequencyValues();

            DefaultMixerFrequencyValues.IsInputMixerFrequencyTypeStartStop = true;
            DefaultMixerFrequencyValues.IsInputMixerFrequencyTypeCenterSpan = false;
            DefaultMixerFrequencyValues.IsInputMixerFrequencyTypeFixed = false;

            //InputMixerFrequencyType = MixerFrequencyTypeEnum.StartStop;
            DefaultMixerFrequencyValues.InputMixerFrequencyStart = 10.5e6;
            DefaultMixerFrequencyValues.InputMixerFrequencyStop = 66.9995e9;
            DefaultMixerFrequencyValues.InputMixerFrequencyCenter = 33.505e9;
            DefaultMixerFrequencyValues.InputMixerFrequencySpan = 66.989e9;
            DefaultMixerFrequencyValues.InputMixerFrequencyFixed = 1e9;

            DefaultMixerFrequencyValues.IsLO1MixerFrequencyTypeStartStop = false;
            DefaultMixerFrequencyValues.IsLO1MixerFrequencyTypeCenterSpan = false;
            DefaultMixerFrequencyValues.IsLO1MixerFrequencyTypeFixed = true;

            //LO1MixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
            DefaultMixerFrequencyValues.LO1MixerFrequencyStart = 0;
            DefaultMixerFrequencyValues.LO1MixerFrequencyStop = 0;
            DefaultMixerFrequencyValues.LO1MixerFrequencyCenter = 0;
            DefaultMixerFrequencyValues.LO1MixerFrequencySpan = 0;
            DefaultMixerFrequencyValues.LO1MixerFrequencyFixed = 0;
            DefaultMixerFrequencyValues.InputGTLO1 = true;

            DefaultMixerFrequencyValues.IsIFMixerFrequencyTypeStartStop = false;
            DefaultMixerFrequencyValues.IsIFMixerFrequencyTypeCenterSpan = true;
            DefaultMixerFrequencyValues.IsIFMixerFrequencyTypeFixed = false;

            //IFSidebandType = SidebandTypeEnum.Low;
            //IFMixerFrequencyType = MixerFrequencyTypeEnum.CenterSpan;
            DefaultMixerFrequencyValues.IFMixerFrequencyStart = 10.5e6;
            DefaultMixerFrequencyValues.IFMixerFrequencyStop = 66.9995e9;
            DefaultMixerFrequencyValues.IFMixerFrequencyCenter = 33.505e9;
            DefaultMixerFrequencyValues.IFMixerFrequencySpan = 66.989e9;
            DefaultMixerFrequencyValues.IFMixerFrequencyFixed = 10e6;

            DefaultMixerFrequencyValues.IsLO2MixerFrequencyTypeStartStop = false;
            DefaultMixerFrequencyValues.IsLO2MixerFrequencyTypeCenterSpan = false;
            DefaultMixerFrequencyValues.IsLO2MixerFrequencyTypeFixed = true;

            //mixerFrequencyValues.LO2MixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
            DefaultMixerFrequencyValues.LO2MixerFrequencyStart = 0;
            DefaultMixerFrequencyValues.LO2MixerFrequencyStop = 0;
            DefaultMixerFrequencyValues.LO2MixerFrequencyCenter = 0;
            DefaultMixerFrequencyValues.LO2MixerFrequencySpan = 0;
            DefaultMixerFrequencyValues.LO2MixerFrequencyFixed = 0;
            DefaultMixerFrequencyValues.IF1GTLO2 = true;


            DefaultMixerFrequencyValues.IsOutputMixerFrequencyTypeStartStop = false;
            DefaultMixerFrequencyValues.IsOutputMixerFrequencyTypeCenterSpan = true;
            DefaultMixerFrequencyValues.IsOutputMixerFrequencyTypeFixed = false;

            //mixerFrequencyValues.OutputSidebandType = SidebandTypeEnum.Low;
            //mixerFrequencyValues.OutputMixerFrequencyType = MixerFrequencyTypeEnum.CenterSpan;
            DefaultMixerFrequencyValues.OutputMixerFrequencyStart = 10.5e6;
            DefaultMixerFrequencyValues.OutputMixerFrequencyStop = 66.9995e9;
            DefaultMixerFrequencyValues.OutputMixerFrequencyCenter = 33.505e9;
            DefaultMixerFrequencyValues.OutputMixerFrequencySpan = 66.989e9;
            DefaultMixerFrequencyValues.OutputMixerFrequencyFixed = 10e6;
        }

        public ToneFrequencyValues GetToneFrequencyDefaultValues()
        {
            if (DefaultToneFrequencyValues == null)
                return ToneFrequencyValues.GetPresetValues();
            return DefaultToneFrequencyValues;
        }
        private void UpdateToneFrequencyValues()
        {
            if (DefaultToneFrequencyValues == null)
                DefaultToneFrequencyValues = new ToneFrequencyValues();

            DefaultToneFrequencyValues.ToneFrequencySweepType = ToneFrequencySweepTypeEnum.SweepFc;
            DefaultToneFrequencyValues.SweepFcStartFc = 10.5e6;
            DefaultToneFrequencyValues.SweepFcStopFc = 66.9995e9;
            DefaultToneFrequencyValues.SweepFcCenterFc = 33.505e9;
            DefaultToneFrequencyValues.SweepFcSpanFc = 66.9995e9;
            DefaultToneFrequencyValues.SweepFcFixedDeltaF = 1e6;
            DefaultToneFrequencyValues.SweepFcNumberOfPoints = 201;
            DefaultToneFrequencyValues.SweepFcMixedToneIFBW = 1e3;
            DefaultToneFrequencyValues.SweepFcIMToneIFBW = 1e3;
            DefaultToneFrequencyValues.SweepFcReduceIFBW = true;
            DefaultToneFrequencyValues.SweepDeltaFStartDeltaF = 1e6;
            DefaultToneFrequencyValues.SweepDeltaFStopDeltaF = 10e6;
            DefaultToneFrequencyValues.SweepDeltaFFixedFc = 1e9;
            DefaultToneFrequencyValues.PowerSweepCWF1 = 999.5e6;
            DefaultToneFrequencyValues.PowerSweepCWF2 = 1.0005e9;
            DefaultToneFrequencyValues.PowerSweepCWFc = 1e9;
            DefaultToneFrequencyValues.PowerSweepCWDeltaF = 1e6;
        }

        public TonePowerValues GetTonePowerDefaultValues()
        {
            if (DefaultTonePowerValues == null)
                return TonePowerValues.GetPresetValues();
            return DefaultTonePowerValues;
        }

        private void UpdateTonePowerValues()
        {
            if (DefaultTonePowerValues == null)
                DefaultTonePowerValues = new TonePowerValues();

            DefaultTonePowerValues.FixedF1Power = -24;
            DefaultTonePowerValues.FixedF2Power = -24;
            DefaultTonePowerValues.StartF1Power = -24;
            DefaultTonePowerValues.StartF2Power = -24;
            DefaultTonePowerValues.StopF1Power = -10;
            DefaultTonePowerValues.StopF2Power = -10;
            DefaultTonePowerValues.StepF1Power = 0.070;
            DefaultTonePowerValues.StepF2Power = 0.070;
        }

        public CompressionValues GetCompressionDefaultValues()
        {
            if (DefaultCompressionValues == null)
                return CompressionValues.GetPresetValues();
            return DefaultCompressionValues;
        }

        private void UpdateCompressionValues()
        {
            if (DefaultCompressionValues == null)
                DefaultCompressionValues = new CompressionValues();

            DefaultCompressionValues.CompressionMethod = CompressionMethodEnum.CompressionFromLinearGain;
            DefaultCompressionValues.CompressionLevel = 1;
            DefaultCompressionValues.CompressionBackOff = 10;
            DefaultCompressionValues.CompressionDeltaX = 10;
            DefaultCompressionValues.CompressionDeltaY = 9;
            DefaultCompressionValues.CompressionFromMaxPout = 0.1;
            DefaultCompressionValues.SMARTSweepTolerance = 0.05;
            DefaultCompressionValues.SMARTSweepIterations = 20;
            DefaultCompressionValues.SMARTSweepShowIterations = false;
            DefaultCompressionValues.SMARTSweepReadDC = false;
            DefaultCompressionValues.SMARTSweepSafeMode = false;
            DefaultCompressionValues.SMARTSweepCoarseIncrement = 3;
            DefaultCompressionValues.SMARTSweepFineIncrement = 1;
            DefaultCompressionValues.SMARTSweepFineThreshold = 0.5;
            DefaultCompressionValues.SMARTSweepMaxOutputPower = 30;
            DefaultCompressionValues.CompressionPointInterpolation = false;
            DefaultCompressionValues.EndOfSweepCondition = EndOfSweepConditionEnum.Default;
            DefaultCompressionValues.SettlingTime = 0;
        }
    }
}
