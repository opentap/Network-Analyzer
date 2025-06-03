using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using OpenTap;

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
        _2,
    }

    public enum PortsEnum
    {
        [Scpi("1")]
        Port1 = 1,

        [Scpi("2")]
        Port2 = 2,

        [Scpi("3")]
        Port3 = 3,

        [Scpi("4")]
        Port4 = 4,
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
        Source3,
    }

    public class StandardChannelValues
    {
        public StandardSweepTypeEnum SweepType;
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

        public double SweepTimeAuto;
        public double SweepTimeStepped;
        public double DwellTime;
        public double SweepDelay;
        public bool AutoSweepTime;
        public bool FastSweep;
        public StandardChannelSweepModeEnum StandardChannelSweepMode;
        public StandardChannelSweepSequenceEnum StandardChannelSweepSequence;

        public static StandardChannelValues GetPresetValues()
        {
            StandardChannelValues standardChannelValues = new StandardChannelValues();
            standardChannelValues.SweepType = StandardSweepTypeEnum.LinearFrequency;
            standardChannelValues.Start = 10e6;
            standardChannelValues.Stop = 50e9;
            standardChannelValues.Power = -15;
            standardChannelValues.Points = 201;
            standardChannelValues.IFBandWidth = 100e3;
            standardChannelValues.StartPower = -10;
            standardChannelValues.StopPower = 0;
            standardChannelValues.CWFrequency = 1e9;
            standardChannelValues.StartPhase = 0;
            standardChannelValues.StopPhase = 0;

            standardChannelValues.SweepTimeAuto = 0.016884;
            standardChannelValues.SweepTimeStepped = 0.002010; // for N5247B: 190.520e-3;
            standardChannelValues.DwellTime = 0;
            standardChannelValues.SweepDelay = 0;
            standardChannelValues.AutoSweepTime = true;
            standardChannelValues.FastSweep = false;
            standardChannelValues.StandardChannelSweepMode = StandardChannelSweepModeEnum.Auto;
            standardChannelValues.StandardChannelSweepSequence =
                StandardChannelSweepSequenceEnum.Standard;

            return standardChannelValues;
        }
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
        public bool PowerOnAllChannels;
        public double Lo1Power;
        public SourceLevelingModeType SourceLevelingModeLO1;
        public double Lo2Power;
        public SourceLevelingModeType SourceLevelingModeLO2;
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
            mixerPowerValues.PowerOnAllChannels = true;
            mixerPowerValues.Lo1Power = -15;
            mixerPowerValues.SourceLevelingModeLO1 = SourceLevelingModeType.INTernal;
            mixerPowerValues.Lo2Power = -15;
            mixerPowerValues.SourceLevelingModeLO2 = SourceLevelingModeType.INTernal;
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
        public MixerFrequencyTypeEnum InputMixerFrequencyType;
        public double InputMixerFrequencyStart;
        public double InputMixerFrequencyStop;
        public double InputMixerFrequencyCenter;
        public double InputMixerFrequencySpan;
        public double InputMixerFrequencyFixed;

        public MixerFrequencyTypeEnum LO1MixerFrequencyType;
        public double LO1MixerFrequencyStart;
        public double LO1MixerFrequencyStop;
        public double LO1MixerFrequencyCenter;
        public double LO1MixerFrequencySpan;
        public double LO1MixerFrequencyFixed;
        public bool InputGTLO1;

        public SidebandTypeEnum IFSidebandType;
        public MixerFrequencyTypeEnum IFMixerFrequencyType;
        public double IFMixerFrequencyStart;
        public double IFMixerFrequencyStop;
        public double IFMixerFrequencyCenter;
        public double IFMixerFrequencySpan;
        public double IFMixerFrequencyFixed;

        public MixerFrequencyTypeEnum LO2MixerFrequencyType;
        public double LO2MixerFrequencyStart;
        public double LO2MixerFrequencyStop;
        public double LO2MixerFrequencyCenter;
        public double LO2MixerFrequencySpan;
        public double LO2MixerFrequencyFixed;
        public bool IF1GTLO2 = true;

        public SidebandTypeEnum OutputSidebandType;
        public MixerFrequencyTypeEnum OutputMixerFrequencyType;
        public double OutputMixerFrequencyStart;
        public double OutputMixerFrequencyStop;
        public double OutputMixerFrequencyCenter;
        public double OutputMixerFrequencySpan;
        public double OutputMixerFrequencyFixed;

        public static MixerFrequencyValues GetPresetValues()
        {
            MixerFrequencyValues mixerFrequencyValues = new MixerFrequencyValues();

            mixerFrequencyValues.InputMixerFrequencyType = MixerFrequencyTypeEnum.StartStop;
            mixerFrequencyValues.InputMixerFrequencyStart = 10e6;
            mixerFrequencyValues.InputMixerFrequencyStop = 50e9;
            mixerFrequencyValues.InputMixerFrequencyCenter = 25.005e9;
            mixerFrequencyValues.InputMixerFrequencySpan = 49.99e9;
            mixerFrequencyValues.InputMixerFrequencyFixed = 1e9;

            mixerFrequencyValues.LO1MixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
            mixerFrequencyValues.LO1MixerFrequencyStart = 0;
            mixerFrequencyValues.LO1MixerFrequencyStop = 0;
            mixerFrequencyValues.LO1MixerFrequencyCenter = 0;
            mixerFrequencyValues.LO1MixerFrequencySpan = 0;
            mixerFrequencyValues.LO1MixerFrequencyFixed = 0;
            mixerFrequencyValues.InputGTLO1 = true;

            mixerFrequencyValues.IFSidebandType = SidebandTypeEnum.Low;
            mixerFrequencyValues.IFMixerFrequencyType = MixerFrequencyTypeEnum.StartStop;
            mixerFrequencyValues.IFMixerFrequencyStart = 10e6;
            mixerFrequencyValues.IFMixerFrequencyStop = 50e9;
            mixerFrequencyValues.IFMixerFrequencyCenter = 25.005e9;
            mixerFrequencyValues.IFMixerFrequencySpan = 49.99e9;
            mixerFrequencyValues.IFMixerFrequencyFixed = 1e9;

            mixerFrequencyValues.LO2MixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
            mixerFrequencyValues.LO2MixerFrequencyStart = 0;
            mixerFrequencyValues.LO2MixerFrequencyStop = 0;
            mixerFrequencyValues.LO2MixerFrequencyCenter = 0;
            mixerFrequencyValues.LO2MixerFrequencySpan = 0;
            mixerFrequencyValues.LO2MixerFrequencyFixed = 0;
            mixerFrequencyValues.IF1GTLO2 = true;

            mixerFrequencyValues.OutputSidebandType = SidebandTypeEnum.Low;
            mixerFrequencyValues.OutputMixerFrequencyType = MixerFrequencyTypeEnum.StartStop;
            mixerFrequencyValues.OutputMixerFrequencyStart = 10e6;
            mixerFrequencyValues.OutputMixerFrequencyStop = 50e9;
            mixerFrequencyValues.OutputMixerFrequencyCenter = 25.005e9;
            mixerFrequencyValues.OutputMixerFrequencySpan = 49.99e9;
            mixerFrequencyValues.OutputMixerFrequencyFixed = 1e9;

            return mixerFrequencyValues;
        }
    }

    public class ToneFrequencyValues
    {
        public ToneFrequencySweepTypeEnum ToneFrequencySweepType;
        public GeneralToneFrequencySweepTypeEnum GeneralToneFrequencySweepType;
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
        public XAxisDisplayAnnotationEnum XAxisDisplayAnnotation;

        public static ToneFrequencyValues GetPresetValues()
        {
            ToneFrequencyValues toneFrequencyValues = new ToneFrequencyValues();
            toneFrequencyValues.ToneFrequencySweepType = ToneFrequencySweepTypeEnum.SweepFc;
            toneFrequencyValues.GeneralToneFrequencySweepType =
                GeneralToneFrequencySweepTypeEnum.SweepFc;
            toneFrequencyValues.SweepFcStartFc = 10.5e6;
            toneFrequencyValues.SweepFcStopFc = 49.9995e9;
            toneFrequencyValues.SweepFcCenterFc = 25.005e9;
            toneFrequencyValues.SweepFcSpanFc = 49.989e9;
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
            toneFrequencyValues.XAxisDisplayAnnotation = XAxisDisplayAnnotationEnum.Output;
            return toneFrequencyValues;
        }
    }

    public class TonePowerValues
    {
        public DutInputPortsEnum PortInput;
        public DutOutputPortsEnum PortOutput;

        public double InputPortSourceAttenuator;
        public double InputPortReceiverAttenuator;
        public double OutputPortSourceAttenuator;
        public double OutputPortReceiverAttenuator;

        public bool CoupleTonePowers;
        public bool ALCOn;
        public PowerLevelingEnum PowerLeveling;
        public double FixedF1Power;
        public double FixedF2Power;
        public double StartF1Power;
        public double StartF2Power;
        public double StopF1Power;
        public double StopF2Power;

        public static TonePowerValues GetPresetValues()
        {
            TonePowerValues tonePowerValues = new TonePowerValues();
            tonePowerValues.PortInput = DutInputPortsEnum.Port1;
            tonePowerValues.PortOutput = DutOutputPortsEnum.Port2;
            tonePowerValues.InputPortSourceAttenuator = 0;
            tonePowerValues.InputPortReceiverAttenuator = 0;
            tonePowerValues.OutputPortSourceAttenuator = 10;
            tonePowerValues.OutputPortReceiverAttenuator = 0;
            tonePowerValues.CoupleTonePowers = true;
            tonePowerValues.ALCOn = true;
            tonePowerValues.PowerLeveling = PowerLevelingEnum.SetInputPower;
            tonePowerValues.FixedF1Power = -24;
            tonePowerValues.FixedF2Power = -24;
            tonePowerValues.StartF1Power = -24;
            tonePowerValues.StartF2Power = -24;
            tonePowerValues.StopF1Power = -10;
            tonePowerValues.StopF2Power = -10;
            return tonePowerValues;
        }
    }

    public class ConverterCompressionValues
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

        public static ConverterCompressionValues GetPresetValues()
        {
            ConverterCompressionValues compressionValues = new ConverterCompressionValues();
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

    public class ConverterFrequencyValues
    {
        public SweepTypeEnum SweepType;
        public int SweepSettingsNumberOfPoints;
        public double SweepSettingsIFBandwidth;
        public double SweepSettingsStart;
        public double SweepSettingsStop;
        public double SweepSettingsCenter;
        public double SweepSettingsSpan;
        public double SweepSettingsFixed;

        public static ConverterFrequencyValues GetPresetValues()
        {
            ConverterFrequencyValues converterFrequencyValues = new ConverterFrequencyValues();
            converterFrequencyValues.SweepType = SweepTypeEnum.LinearSweep;
            converterFrequencyValues.SweepSettingsNumberOfPoints = 201;
            converterFrequencyValues.SweepSettingsIFBandwidth = 100e3;
            converterFrequencyValues.SweepSettingsStart = 10e6;
            converterFrequencyValues.SweepSettingsStop = 50e9;
            converterFrequencyValues.SweepSettingsCenter = 25.005e9;
            converterFrequencyValues.SweepSettingsSpan = 49.99e9;
            converterFrequencyValues.SweepSettingsFixed = 1e9;

            return converterFrequencyValues;
        }
    }

    public class ConverterPowerBaseValues
    {
        public bool PowerOnAllChannels;
        public bool PortPowersCoupled;
        public double InputPortLinearInputPower;
        public bool AutoInputPortSourceAttenuator;
        public double InputPortSourceAttenuator;
        public double InputPortReceiverAttenuator;
        public InputSourceLevelingModeEnum InputSourceLevelingMode;

        public double OutputPortReversePower;
        public bool AutoOutputPortSourceAttenuator;
        public double OutputPortSourceAttenuator;
        public double OutputPortReceiverAttenuator;
        public OutputSourceLevelingModeEnum OutputSourceLevelingMode;
    }

    public class MixerConverterPowerValue : ConverterPowerBaseValues
    {
        public double PowerSweepStartPower;
        public double PowerSweepStopPower;
        public int PowerSweepPowerPoints;
        public double PowerSweepPowerStep;

        public static MixerConverterPowerValue GetPresetValues()
        {
            MixerConverterPowerValue converterPowerValues = new MixerConverterPowerValue();

            converterPowerValues.PowerOnAllChannels = true;
            converterPowerValues.PortPowersCoupled = true;
            converterPowerValues.InputPortLinearInputPower = -25;
            converterPowerValues.InputPortSourceAttenuator = 0;
            converterPowerValues.InputPortReceiverAttenuator = 0;
            converterPowerValues.InputSourceLevelingMode = InputSourceLevelingModeEnum.Internal;

            converterPowerValues.OutputPortReversePower = -5;
            converterPowerValues.AutoOutputPortSourceAttenuator = false;
            converterPowerValues.OutputPortSourceAttenuator = 0;
            converterPowerValues.OutputPortReceiverAttenuator = 0;
            converterPowerValues.OutputSourceLevelingMode = OutputSourceLevelingModeEnum.Internal;

            converterPowerValues.PowerSweepStartPower = -25;
            converterPowerValues.PowerSweepStopPower = -5;
            converterPowerValues.PowerSweepPowerPoints = 21;
            converterPowerValues.PowerSweepPowerStep = 1;

            return converterPowerValues;
        }
    }

    public class GeneralGainCompressionPowerValues : MixerConverterPowerValue
    {
        public static new GeneralGainCompressionPowerValues GetPresetValues()
        {
            GeneralGainCompressionPowerValues generalGainCompressionPowerValues =
                new GeneralGainCompressionPowerValues();

            generalGainCompressionPowerValues.PowerOnAllChannels = true;
            generalGainCompressionPowerValues.PortPowersCoupled = true;
            generalGainCompressionPowerValues.InputPortLinearInputPower = -25;
            generalGainCompressionPowerValues.InputPortSourceAttenuator = 0;
            generalGainCompressionPowerValues.InputPortReceiverAttenuator = 0;
            generalGainCompressionPowerValues.InputSourceLevelingMode =
                InputSourceLevelingModeEnum.Internal;

            generalGainCompressionPowerValues.OutputPortReversePower = -15;
            generalGainCompressionPowerValues.AutoOutputPortSourceAttenuator = false;
            generalGainCompressionPowerValues.OutputPortSourceAttenuator = 0;
            generalGainCompressionPowerValues.OutputPortReceiverAttenuator = 0;
            generalGainCompressionPowerValues.OutputSourceLevelingMode =
                OutputSourceLevelingModeEnum.Internal;

            generalGainCompressionPowerValues.PowerSweepStartPower = -25;
            generalGainCompressionPowerValues.PowerSweepStopPower = -5;
            generalGainCompressionPowerValues.PowerSweepPowerPoints = 21;
            generalGainCompressionPowerValues.PowerSweepPowerStep = 1;

            return generalGainCompressionPowerValues;
        }
    }

    public class ScalarMixerConverterPowerValue : ConverterPowerBaseValues
    {
        public double InputPowerSweepStartPower;
        public double InputPowerSweepStopPower;
        public int InputPowerSweepPowerPoints;
        public double InputPowerSweepPowerStep;

        public double OutputPowerSweepStartPower;
        public double OutputPowerSweepStopPower;

        public static ScalarMixerConverterPowerValue GetPresetValues()
        {
            ScalarMixerConverterPowerValue converterPowerValues =
                new ScalarMixerConverterPowerValue();

            converterPowerValues.PowerOnAllChannels = true;
            converterPowerValues.PortPowersCoupled = true;

            converterPowerValues.AutoInputPortSourceAttenuator = true;
            converterPowerValues.InputPortLinearInputPower = -15;
            converterPowerValues.InputPortSourceAttenuator = 0;
            converterPowerValues.InputPortReceiverAttenuator = 0;
            converterPowerValues.InputSourceLevelingMode = InputSourceLevelingModeEnum.Internal;

            converterPowerValues.OutputPortReversePower = -15;
            converterPowerValues.AutoOutputPortSourceAttenuator = true;
            converterPowerValues.OutputPortSourceAttenuator = 0;
            converterPowerValues.OutputPortReceiverAttenuator = 0;
            converterPowerValues.OutputSourceLevelingMode = OutputSourceLevelingModeEnum.Internal;

            converterPowerValues.InputPowerSweepStartPower = -15;
            converterPowerValues.InputPowerSweepStopPower = -15;
            converterPowerValues.InputPowerSweepPowerPoints = 201;
            converterPowerValues.InputPowerSweepPowerStep = 0;

            converterPowerValues.OutputPowerSweepStartPower = -15;
            converterPowerValues.OutputPowerSweepStopPower = -15;

            return converterPowerValues;
        }
    }

    public class NoiseFigureConverterValues
    {
        public NoiseBandwidthNoise NoiseBandwidthNoise;
        public NoiseBandwidthNormal NoiseBandwidthNormal;
        public NoiseReceiver NoiseReceiver;
        public int AverageNumberNoise;
        public int AverageNumberNormal;
        public bool IsAverageOnNoise;
        public bool IsAverageOnNormal;
        public bool UseNarrowbandCompensation;
        public ReceiverGain ReceiverGain;
        public double SourceTemperature;
        public bool Use302K;
        public int MaxImpedanceStates;
        public bool EnableSourcePulling;
        public String NoiseTunerFile;

        public static NoiseFigureConverterValues GetPresetValues()
        {
            NoiseFigureConverterValues NFDefault = new NoiseFigureConverterValues();

            NFDefault.NoiseReceiver = NoiseReceiver.NoiseReceiver;
            NFDefault.NoiseBandwidthNoise = NoiseBandwidthNoise.Four;
            NFDefault.NoiseBandwidthNormal = NoiseBandwidthNormal.OnePointTwo;
            NFDefault.AverageNumberNoise = 1;
            NFDefault.AverageNumberNormal = 100;
            NFDefault.IsAverageOnNoise = false;
            NFDefault.IsAverageOnNormal = true;
            NFDefault.UseNarrowbandCompensation = false;
            NFDefault.ReceiverGain = ReceiverGain.High;
            NFDefault.SourceTemperature = 297;
            NFDefault.Use302K = true;
            NFDefault.MaxImpedanceStates = 5;
            NFDefault.EnableSourcePulling = false;
            NFDefault.NoiseTunerFile = "";

            return NFDefault;
        }
    }

    public class NoiseFigureConverterPowerValue : ConverterPowerBaseValues
    {
        public static NoiseFigureConverterPowerValue GetPresetValues()
        {
            NoiseFigureConverterPowerValue converterPowerValues =
                new NoiseFigureConverterPowerValue();

            converterPowerValues.PowerOnAllChannels = true;
            converterPowerValues.InputPortLinearInputPower = -30;
            converterPowerValues.InputPortSourceAttenuator = 10;
            converterPowerValues.InputPortReceiverAttenuator = 0;
            converterPowerValues.InputSourceLevelingMode = InputSourceLevelingModeEnum.Internal;

            converterPowerValues.OutputPortReversePower = -20;
            converterPowerValues.AutoOutputPortSourceAttenuator = false;
            converterPowerValues.OutputPortSourceAttenuator = 0;
            converterPowerValues.OutputPortReceiverAttenuator = 0;
            converterPowerValues.OutputSourceLevelingMode = OutputSourceLevelingModeEnum.Internal;

            return converterPowerValues;
        }
    }

    public class NoiseFigureConverterFrequencyValues
    {
        public SweepTypeEnum SweepType;
        public GeneralNFSweepTypeEnum GeneralNFSweepType { get; set; }

        public int SweepSettingsNumberOfPoints;
        public double SweepSettingsIFBandwidth;
        public double SweepSettingsStart;
        public double SweepSettingsStop;
        public double SweepSettingsCenter;
        public double SweepSettingsSpan;
        public double SweepSettingsFixed;

        public static NoiseFigureConverterFrequencyValues GetPresetValues()
        {
            NoiseFigureConverterFrequencyValues converterFrequencyValues =
                new NoiseFigureConverterFrequencyValues();
            converterFrequencyValues.SweepType = SweepTypeEnum.LinearSweep;
            converterFrequencyValues.GeneralNFSweepType = GeneralNFSweepTypeEnum.LinearSweep;
            converterFrequencyValues.SweepSettingsNumberOfPoints = 201;
            converterFrequencyValues.SweepSettingsIFBandwidth = 1e3;
            converterFrequencyValues.SweepSettingsStart = 10e6;
            converterFrequencyValues.SweepSettingsStop = 50e9;
            converterFrequencyValues.SweepSettingsCenter = 25.005e9;
            converterFrequencyValues.SweepSettingsSpan = 49.99e9;
            converterFrequencyValues.SweepSettingsFixed = 1e9;

            return converterFrequencyValues;
        }
    }

    public class MixerSweepValue
    {
        public ScalerMixerSweepType SweepType;
        public bool IsXAxisPointSpacing;
        public bool IsReversedPortTwoCoupler;
        public bool IsAvoidSpurs;
        public int NumberOfPoints;
        public double IFBandwidth;
        public bool IsEnablePhase;
        public ScalerMixerPhasePoint PhasePoint = ScalerMixerPhasePoint.MiddlePoint;

        public static MixerSweepValue GetPresetValues()
        {
            MixerSweepValue mixerSweepValue = new MixerSweepValue();
            mixerSweepValue.SweepType = ScalerMixerSweepType.LinearFrequency;
            mixerSweepValue.IsXAxisPointSpacing = false;
            mixerSweepValue.IsReversedPortTwoCoupler = false;
            mixerSweepValue.IsAvoidSpurs = false;
            mixerSweepValue.NumberOfPoints = 201;
            mixerSweepValue.IFBandwidth = 10e3;
            mixerSweepValue.IsEnablePhase = false;
            mixerSweepValue.PhasePoint = ScalerMixerPhasePoint.MiddlePoint;
            return mixerSweepValue;
        }
    }

    public partial class PNAX : ScpiInstrument
    {
        public StandardChannelValues GetStandardChannelDefaultValues()
        {
            if (DefaultStandardChannelValues == null)
                return StandardChannelValues.GetPresetValues();

            return DefaultStandardChannelValues;
        }

        private void UpdateStandardValues()
        {
            try
            {
                Open();
                Log.Info("Getting default values for Standard Channel");

                this.ScpiCommand("SYSTem:PRESet");
                this.WaitForOperationComplete();

                if (DefaultStandardChannelValues == null)
                    DefaultStandardChannelValues = new StandardChannelValues();

                // Channel 1 is Standard
                //DefaultStandardChannelValues.SweepType = GetStandardSweepType(1);
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

                //DefaultStandardChannelValues.ConverterStages = GetConverterStages(2);
                ////DefaultStandardChannelValues.PortInput = GetPortInput(2);
                //DefaultStandardChannelValues.InputFracMultiNumerator = GetInputFractionalMultiplierNumerator(2);
                //DefaultStandardChannelValues.InputFracMultiDenominator = GetInputFractionalMultiplierDenominator(2);
                //DefaultStandardChannelValues.Lo1FracMultiNumerator = GetLOFractionalMultiplierNumerator(2, 1);
                //DefaultStandardChannelValues.Lo1FracMultiDenominator = GetLOFractionalMultiplierDenominator(2, 1);
                //DefaultStandardChannelValues.Lo2FracMultiNumerator = GetLOFractionalMultiplierNumerator(2, 2);
                //DefaultStandardChannelValues.Lo2FracMultiDenominator = GetLOFractionalMultiplierDenominator(2, 2);
                //DefaultStandardChannelValues.PortLo1 = GetPortLO(2, 1);
                //DefaultStandardChannelValues.PortLo2 = GetPortLO(2, 2);

                // TODO: MixerPowerDefaultValue updated by INstrument

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

            //InputMixerFrequencyType = MixerFrequencyTypeEnum.StartStop;
            DefaultMixerFrequencyValues.InputMixerFrequencyStart = 10.5e6;
            DefaultMixerFrequencyValues.InputMixerFrequencyStop = 66.9995e9;
            DefaultMixerFrequencyValues.InputMixerFrequencyCenter = 33.505e9;
            DefaultMixerFrequencyValues.InputMixerFrequencySpan = 66.989e9;
            DefaultMixerFrequencyValues.InputMixerFrequencyFixed = 1e9;

            //LO1MixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
            DefaultMixerFrequencyValues.LO1MixerFrequencyStart = 0;
            DefaultMixerFrequencyValues.LO1MixerFrequencyStop = 0;
            DefaultMixerFrequencyValues.LO1MixerFrequencyCenter = 0;
            DefaultMixerFrequencyValues.LO1MixerFrequencySpan = 0;
            DefaultMixerFrequencyValues.LO1MixerFrequencyFixed = 0;
            DefaultMixerFrequencyValues.InputGTLO1 = true;

            //IFSidebandType = SidebandTypeEnum.Low;
            //IFMixerFrequencyType = MixerFrequencyTypeEnum.CenterSpan;
            DefaultMixerFrequencyValues.IFMixerFrequencyStart = 10.5e6;
            DefaultMixerFrequencyValues.IFMixerFrequencyStop = 66.9995e9;
            DefaultMixerFrequencyValues.IFMixerFrequencyCenter = 33.505e9;
            DefaultMixerFrequencyValues.IFMixerFrequencySpan = 66.989e9;
            DefaultMixerFrequencyValues.IFMixerFrequencyFixed = 10e6;

            //mixerFrequencyValues.LO2MixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
            DefaultMixerFrequencyValues.LO2MixerFrequencyStart = 0;
            DefaultMixerFrequencyValues.LO2MixerFrequencyStop = 0;
            DefaultMixerFrequencyValues.LO2MixerFrequencyCenter = 0;
            DefaultMixerFrequencyValues.LO2MixerFrequencySpan = 0;
            DefaultMixerFrequencyValues.LO2MixerFrequencyFixed = 0;
            DefaultMixerFrequencyValues.IF1GTLO2 = true;

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
            DefaultToneFrequencyValues.SweepFcStopFc = 49.9995e9;
            DefaultToneFrequencyValues.SweepFcCenterFc = 25.005e9;
            DefaultToneFrequencyValues.SweepFcSpanFc = 49.989e9;
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
        }

        public ConverterCompressionValues GetConverterCompressionDefaultValues()
        {
            if (DefaultConverterCompressionValues == null)
                return ConverterCompressionValues.GetPresetValues();
            return DefaultConverterCompressionValues;
        }

        private void UpdateCompressionValues()
        {
            if (DefaultConverterCompressionValues == null)
                DefaultConverterCompressionValues = new ConverterCompressionValues();

            DefaultConverterCompressionValues.CompressionMethod =
                CompressionMethodEnum.CompressionFromLinearGain;
            DefaultConverterCompressionValues.CompressionLevel = 1;
            DefaultConverterCompressionValues.CompressionBackOff = 10;
            DefaultConverterCompressionValues.CompressionDeltaX = 10;
            DefaultConverterCompressionValues.CompressionDeltaY = 9;
            DefaultConverterCompressionValues.CompressionFromMaxPout = 0.1;
            DefaultConverterCompressionValues.SMARTSweepTolerance = 0.05;
            DefaultConverterCompressionValues.SMARTSweepIterations = 20;
            DefaultConverterCompressionValues.SMARTSweepShowIterations = false;
            DefaultConverterCompressionValues.SMARTSweepReadDC = false;
            DefaultConverterCompressionValues.SMARTSweepSafeMode = false;
            DefaultConverterCompressionValues.SMARTSweepCoarseIncrement = 3;
            DefaultConverterCompressionValues.SMARTSweepFineIncrement = 1;
            DefaultConverterCompressionValues.SMARTSweepFineThreshold = 0.5;
            DefaultConverterCompressionValues.SMARTSweepMaxOutputPower = 30;
            DefaultConverterCompressionValues.CompressionPointInterpolation = false;
            DefaultConverterCompressionValues.EndOfSweepCondition = EndOfSweepConditionEnum.Default;
            DefaultConverterCompressionValues.SettlingTime = 0;
        }

        public ConverterFrequencyValues GetConverterFrequencyDefaultValues()
        {
            if (DefaultConverterFrequencyValues == null)
                return ConverterFrequencyValues.GetPresetValues();
            return DefaultConverterFrequencyValues;
        }

        private void UpdateConverterFrequencyValues()
        {
            // TODO: If values are from instrument, split into gaincompression frequency and noise figure
            if (DefaultConverterFrequencyValues == null)
                DefaultConverterFrequencyValues = new ConverterFrequencyValues();

            DefaultConverterFrequencyValues.SweepType = SweepTypeEnum.LinearSweep;
            DefaultConverterFrequencyValues.SweepSettingsNumberOfPoints = 201;
            DefaultConverterFrequencyValues.SweepSettingsIFBandwidth = 100e3;
            DefaultConverterFrequencyValues.SweepSettingsStart = 10e6;
            DefaultConverterFrequencyValues.SweepSettingsStop = 50e9;
            DefaultConverterFrequencyValues.SweepSettingsCenter = 25.005e9;
            DefaultConverterFrequencyValues.SweepSettingsSpan = 49.99e9;
            DefaultConverterFrequencyValues.SweepSettingsFixed = 1e9;
        }

        public MixerConverterPowerValue GetMixerConverterPowerDefaultValues()
        {
            if (DefaultMixerConverterPowerValues == null)
                return MixerConverterPowerValue.GetPresetValues();
            return DefaultMixerConverterPowerValues;
        }

        public NoiseFigureConverterValues GetNoiseFigureConverterDefaultValues()
        {
            if (DefaultNoiseFigureConverterValues == null)
                return NoiseFigureConverterValues.GetPresetValues();
            return DefaultNoiseFigureConverterValues;
        }

        private void UpdateMixerConverterPowerValues()
        {
            if (DefaultMixerConverterPowerValues == null)
                DefaultMixerConverterPowerValues = new MixerConverterPowerValue();

            DefaultMixerConverterPowerValues.PowerOnAllChannels = true;

            DefaultMixerConverterPowerValues.InputSourceLevelingMode =
                InputSourceLevelingModeEnum.Internal;
            DefaultMixerConverterPowerValues.InputPortLinearInputPower = -25;
            DefaultMixerConverterPowerValues.InputPortSourceAttenuator = 0;
            DefaultMixerConverterPowerValues.InputPortReceiverAttenuator = 0;

            DefaultMixerConverterPowerValues.OutputSourceLevelingMode =
                OutputSourceLevelingModeEnum.Internal;
            DefaultMixerConverterPowerValues.OutputPortReversePower = -5;
            DefaultMixerConverterPowerValues.AutoOutputPortSourceAttenuator = false;
            DefaultMixerConverterPowerValues.OutputPortSourceAttenuator = 0;
            DefaultMixerConverterPowerValues.OutputPortReceiverAttenuator = 0;

            DefaultMixerConverterPowerValues.PowerSweepStartPower = -25;
            DefaultMixerConverterPowerValues.PowerSweepStopPower = -5;
            DefaultMixerConverterPowerValues.PowerSweepPowerPoints = 21;
            DefaultMixerConverterPowerValues.PowerSweepPowerStep = 1;
        }

        public ScalarMixerConverterPowerValue GetScalarMixerConverterPowerDefaultValues()
        {
            if (DefaultScalarMixerConverterPowerValues == null)
                return ScalarMixerConverterPowerValue.GetPresetValues();
            return DefaultScalarMixerConverterPowerValues;
        }

        private void UpdateScalarMixerConverterPowerValues()
        {
            if (DefaultScalarMixerConverterPowerValues == null)
                DefaultScalarMixerConverterPowerValues = new ScalarMixerConverterPowerValue();

            DefaultScalarMixerConverterPowerValues.PowerOnAllChannels = true;
            DefaultScalarMixerConverterPowerValues.PortPowersCoupled = true;

            DefaultScalarMixerConverterPowerValues.InputSourceLevelingMode =
                InputSourceLevelingModeEnum.Internal;
            DefaultScalarMixerConverterPowerValues.InputPortLinearInputPower = -15;
            DefaultScalarMixerConverterPowerValues.AutoInputPortSourceAttenuator = true;
            DefaultScalarMixerConverterPowerValues.InputPortSourceAttenuator = 0;
            DefaultScalarMixerConverterPowerValues.InputPortReceiverAttenuator = 0;

            DefaultScalarMixerConverterPowerValues.OutputSourceLevelingMode =
                OutputSourceLevelingModeEnum.Internal;
            DefaultScalarMixerConverterPowerValues.OutputPortReversePower = -15;
            DefaultScalarMixerConverterPowerValues.AutoOutputPortSourceAttenuator = true;
            DefaultScalarMixerConverterPowerValues.OutputPortSourceAttenuator = 0;
            DefaultScalarMixerConverterPowerValues.OutputPortReceiverAttenuator = 0;

            DefaultScalarMixerConverterPowerValues.InputPowerSweepStartPower = -15;
            DefaultScalarMixerConverterPowerValues.InputPowerSweepStopPower = -15;
            DefaultScalarMixerConverterPowerValues.InputPowerSweepPowerPoints = 201;
            DefaultScalarMixerConverterPowerValues.InputPowerSweepPowerStep = 0;
            DefaultScalarMixerConverterPowerValues.OutputPowerSweepStartPower = -15;
            DefaultScalarMixerConverterPowerValues.OutputPowerSweepStopPower = -15;
        }

        public NoiseFigureConverterPowerValue GetNoiseFigureConverterPowerDefaultValues()
        {
            if (DefaultNoiseFigureConverterPowerValues == null)
                return NoiseFigureConverterPowerValue.GetPresetValues();
            return DefaultNoiseFigureConverterPowerValues;
        }

        public NoiseFigureConverterFrequencyValues GetNoiseFigureConverterFrequencyDefaultValues()
        {
            if (DefaultNoiseFigureConverterFrequencyValues == null)
                return NoiseFigureConverterFrequencyValues.GetPresetValues();
            return DefaultNoiseFigureConverterFrequencyValues;
        }

        private void UpdateConverterPowerValues()
        {
            UpdateMixerConverterPowerValues();
            UpdateScalarMixerConverterPowerValues();
            UpdateNoiseFigureConverterPowerValues();
        }

        private void UpdateNoiseFigureConverterPowerValues()
        {
            if (DefaultNoiseFigureConverterPowerValues == null)
                DefaultNoiseFigureConverterPowerValues = new NoiseFigureConverterPowerValue();

            DefaultNoiseFigureConverterPowerValues.PowerOnAllChannels = true;
            DefaultNoiseFigureConverterPowerValues.PortPowersCoupled = true;

            DefaultNoiseFigureConverterPowerValues.InputSourceLevelingMode =
                InputSourceLevelingModeEnum.Internal;
            DefaultNoiseFigureConverterPowerValues.InputPortLinearInputPower = -30;
            DefaultNoiseFigureConverterPowerValues.AutoInputPortSourceAttenuator = false;
            DefaultNoiseFigureConverterPowerValues.InputPortSourceAttenuator = 20;
            DefaultNoiseFigureConverterPowerValues.InputPortReceiverAttenuator = 0;

            DefaultNoiseFigureConverterPowerValues.OutputSourceLevelingMode =
                OutputSourceLevelingModeEnum.Internal;
            DefaultNoiseFigureConverterPowerValues.OutputPortReversePower = -20;
            DefaultNoiseFigureConverterPowerValues.AutoOutputPortSourceAttenuator = false;
            DefaultNoiseFigureConverterPowerValues.OutputPortSourceAttenuator = 20;
            DefaultNoiseFigureConverterPowerValues.OutputPortReceiverAttenuator = 0;
        }

        public MixerSweepValue GetMixerSweepDefaultValues()
        {
            if (DefaultMixerSweepValue == null)
                return MixerSweepValue.GetPresetValues();
            return DefaultMixerSweepValue;
        }

        private void UpdateMixerSweepDefaultValues()
        {
            if (DefaultMixerSweepValue == null)
                DefaultMixerSweepValue = new MixerSweepValue();

            DefaultMixerSweepValue.SweepType = ScalerMixerSweepType.LinearFrequency;
            DefaultMixerSweepValue.IsXAxisPointSpacing = false;
            DefaultMixerSweepValue.IsReversedPortTwoCoupler = false;
            DefaultMixerSweepValue.IsAvoidSpurs = false;
            DefaultMixerSweepValue.NumberOfPoints = 201;
            DefaultMixerSweepValue.IFBandwidth = 10e3;
            DefaultMixerSweepValue.IsEnablePhase = false;
            DefaultMixerSweepValue.PhasePoint = ScalerMixerPhasePoint.MiddlePoint;
        }

        public GeneralGainCompressionPowerValues GetGeneralGainCompressionPowerDefaultValues()
        {
            if (DefaultGeneralGainCompressionPowerValues == null)
                return GeneralGainCompressionPowerValues.GetPresetValues();
            return DefaultGeneralGainCompressionPowerValues;
        }
    }
}
