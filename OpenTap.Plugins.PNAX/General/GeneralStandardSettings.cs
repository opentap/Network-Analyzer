using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OpenTap.Plugins.PNAX
{
    [Display("General Standard Settings", Description: "Add a description here")]
    public class GeneralStandardSettings : ComponentSettings<GeneralStandardSettings>
    {
        // TODO: Add settings here as properties, use DisplayAttribute to indicate to the GUI
        //       how the setting should be displayed.
        //       Example:
        [Display("Category\\Example Setting", Description:" Just an example")]
        public bool ExampleSetting { get; set; }

        #region General|Standard
        public StandardSweepTypeEnum StandardSweepType { get; set; } = StandardSweepTypeEnum.LinearFrequency;
        public double SweepPropertiesStart { get; set; } = 10e6;
        public double SweepPropertiesStop { get; set; } = 50e9;         // for N5247B: 67e9;
        public double SweepPropertiesStartPower { get; set; } = -10;
        public double SweepPropertiesStopPower { get; set; } = 0;
        public double SweepPropertiesStartPhase { get; set; } = 0;
        public double SweepPropertiesStopPhase { get; set; } = 0;
        public double SweepPropertiesCWFreq { get; set; } = 1e9;
        public double SweepPropertiesPower { get; set; } = -15;         // for N5247B: -5;
        public int SweepPropertiesPoints { get; set; } = 201;
        public double SweepPropertiesIFBandwidth { get; set; } = 100e3;


        public double SweepTimeAuto { get; set; } = 0.016884;
        public double SweepTimeStepped { get; set; } = 0.002010;        // for N5247B: 190.520e-3;
        public double DwellTime { get; set; } = 0;
        public double SweepDelay { get; set; } = 0;
        public bool AutoSweepTime { get; set; } = true;
        public bool FastSweep { get; set; } = false;
        public StandardChannelSweepModeEnum StandardChannelSweepMode { get; set; } = StandardChannelSweepModeEnum.Auto;
        public StandardChannelSweepSequenceEnum StandardChannelSweepSequence { get; set; } = StandardChannelSweepSequenceEnum.Standard;
        #endregion

        #region Converters
        public ConverterStagesEnum ConverterStages { get; set; } = ConverterStagesEnum._1;
        public PortsEnum PortInput { get; set; } = PortsEnum.Port1;
        public PortsEnum PortOutput { get; set; } = PortsEnum.Port2;
        public LOEnum PortLO1 { get; set; } = LOEnum.NotControlled;
        public LOEnum PortLO2 { get; set; } = LOEnum.NotControlled;
        public int InputFractionalMultiplierNumerator { get; set; } = 1;
        public int InputFractionalMultiplierDenominator { get; set; } = 1;
        public int LO1FractionalMultiplierNumerator { get; set; } = 1;
        public int LO1FractionalMultiplierDenominator { get; set; } = 1;
        public int LO2FractionalMultiplierNumerator { get; set; } = 1;
        public int LO2FractionalMultiplierDenominator { get; set; } = 1;
        public bool EnableEmbeddedLO { get; set; } = false;
        public TuningMethodEnum TuningMethod { get; set; } = TuningMethodEnum.BroadbandAndPrecise;
        public TuningPointTypeEnum TuningPointType { get; set; } = TuningPointTypeEnum.MiddlePoint;
        public int TuningPoint { get; set; } = 101;
        public int TuneEvery { get; set; } = 1;
        public int BroadBandSearch { get; set; } = 3000000;
        public int IFBW { get; set; } = 30000;
        public int MaxIterations { get; set; } = 5;
        public int Tolerance { get; set; } = 1;
        public double LOFrequencyDelta { get; set; } = 0;



        public bool PowerOnAllChannels { get; set; } = true;
        public double LO1Power { get; set; } = -15;
        public SourceLevelingModeType SourceLevelingModeLO1 { get; set; } = SourceLevelingModeType.INTernal;
        public double LO2Power { get; set; } = -15;
        public SourceLevelingModeType SourceLevelingModeLO2 { get; set; } = SourceLevelingModeType.INTernal;
        public double SourceAttenuatorPowerPort3 { get; set; } = 0;
        public double ReceiverAttenuatorPowerPort3 { get; set; } = 0;
        public double SourceAttenuatorPowerPort4 { get; set; } = 0;
        public double ReceiverAttenuatorPowerPort4 { get; set; } = 0;
        public double LO1SweptPowerStart { get; set; } = -20;
        public double LO1SweptPowerStop { get; set; } = -10;
        public double LO1SweptPowerStep { get; set; } = 0.05;
        public double LO2SweptPowerStart { get; set; } = -10;
        public double LO2SweptPowerStop { get; set; } = -10;
        public double LO2SweptPowerStep { get; set; } = 0.0;



        public MixerFrequencyTypeEnum InputMixerFrequencyType { get; set; } = MixerFrequencyTypeEnum.StartStop;
        public double InputMixerFrequencyStart { get; set; } = 10e6;
        public double InputMixerFrequencyStop { get; set; } = 50e9;
        public double InputMixerFrequencyCenter { get; set; } = 25.005e9;
        public double InputMixerFrequencySpan { get; set; } = 49.99e9;
        public double InputMixerFrequencyFixed { get; set; } = 1e9;

        public MixerFrequencyTypeEnum LO1MixerFrequencyType { get; set; } = MixerFrequencyTypeEnum.Fixed;
        public double LO1MixerFrequencyStart { get; set; } = 0;
        public double LO1MixerFrequencyStop { get; set; } = 0;
        public double LO1MixerFrequencyCenter { get; set; } = 0;
        public double LO1MixerFrequencySpan { get; set; } = 0;
        public double LO1MixerFrequencyFixed { get; set; } = 0;
        public bool InputGTLO1 { get; set; } = true;

        public MixerFrequencyTypeEnum IFMixerFrequencyType { get; set; } = MixerFrequencyTypeEnum.StartStop;
        public SidebandTypeEnum IFSidebandType { get; set; } = SidebandTypeEnum.Low;
        public double IFMixerFrequencyStart { get; set; } = 10e6;
        public double IFMixerFrequencyStop { get; set; } = 50e9;
        public double IFMixerFrequencyCenter { get; set; } = 25.005e9;
        public double IFMixerFrequencySpan { get; set; } = 49.99e9;
        public double IFMixerFrequencyFixed { get; set; } = 1e9;


        public MixerFrequencyTypeEnum LO2MixerFrequencyType { get; set; } = MixerFrequencyTypeEnum.Fixed;
        public double LO2MixerFrequencyStart { get; set; } = 0;
        public double LO2MixerFrequencyStop { get; set; } = 0;
        public double LO2MixerFrequencyCenter { get; set; } = 0;
        public double LO2MixerFrequencySpan { get; set; } = 0;
        public double LO2MixerFrequencyFixed { get; set; } = 0;
        public bool IF1GTLO2 { get; set; } = true;

        public MixerFrequencyTypeEnum OutputMixerFrequencyType { get; set; } = MixerFrequencyTypeEnum.StartStop;
        public SidebandTypeEnum OutputSidebandType { get; set; } = SidebandTypeEnum.Low;
        public double OutputMixerFrequencyStart { get; set; } = 10e6;
        public double OutputMixerFrequencyStop { get; set; } = 50e9;
        public double OutputMixerFrequencyCenter { get; set; } = 25.005e9;
        public double OutputMixerFrequencySpan { get; set; } = 49.99e9;
        public double OutputMixerFrequencyFixed { get; set; } = 1e9;


        #endregion

        #region Converters|Gain Compression

        public CompressionMethodEnum CompressionMethod { get; set; } = CompressionMethodEnum.CompressionFromLinearGain;
        public double CompressionLevel { get; set; } = 1;
        public double CompressionBackOff { get; set; } = 10;
        public double CompressionDeltaX { get; set; } = 10;
        public double CompressionDeltaY { get; set; } = 9;
        public double CompressionFromMaxPout { get; set; } = 0.1;
        public double SMARTSweepTolerance { get; set; } = 0.05;
        public int SMARTSweepIterations { get; set; } = 20;
        public bool SMARTSweepShowIterations { get; set; } = false;
        public bool SMARTSweepReadDC { get; set; } = false;
        public bool SMARTSweepSafeMode { get; set; } = false;
        public int SMARTSweepCoarseIncrement { get; set; } = 3;
        public double SMARTSweepFineIncrement { get; set; } = 1;
        public double SMARTSweepFineThreshold { get; set; } = 0.5;
        public double SMARTSweepMaxOutputPower { get; set; } = 30;
        public bool CompressionPointInterpolation { get; set; } = false;
        public EndOfSweepConditionEnum EndOfSweepCondition { get; set; } = EndOfSweepConditionEnum.Default;
        public double SettlingTime { get; set; } = 0.000;

        #endregion

        #region Converters||Power
        // public bool PowerOnAllChannels { get; set; } // Already defined
        // public PortsEnum PortInput { get; set; }// Already defined
        public double InputPortLinearInputPower { get; set; } = -25;
        public double InputPortSourceAttenuator { get; set; } = 0;
        public double InputPortReceiverAttenuator { get; set; } = 0;
        public InputSourceLevelingModeEnum InputSourceLevelingMode { get; set; } = InputSourceLevelingModeEnum.Internal;
        // public PortsEnum PortOutput { get; set; }// Already defined
        public double OutputPortReversePower { get; set; } = -5;
        public bool AutoOutputPortSourceAttenuator { get; set; } = false;
        public double OutputPortSourceAttenuator { get; set; } = 0;
        public double OutputPortReceiverAttenuator { get; set; } = 0;
        public OutputSourceLevelingModeEnum OutputSourceLevelingMode { get; set; } = OutputSourceLevelingModeEnum.Internal;
        public double PowerSweepStartPower { get; set; } = -25;
        public double PowerSweepStopPower { get; set; } = -5;
        public int PowerSweepPowerPoints { get; set; } = 21;
        public double PowerSweepPowerStep { get; set; } = 1;
        #endregion

        #region Converters|Frequency
        public SweepTypeEnum SweepType { get; set; } = SweepTypeEnum.LinearSweep;
        public DataAcquisitionModeEnum DataAcquisitionMode { get; set; } = DataAcquisitionModeEnum.SMARTSweep;
        public int SweepSettingsNumberOfPoints { get; set; } = 201;
        public double SweepSettingsIFBandwidth { get; set; } = 100e3;
        public double SweepSettingsStart { get; set; } = 10e6;
        public double SweepSettingsStop { get; set; } = 67e9;
        public double SweepSettingsCenter { get; set; } = 33.505e9;
        public double SweepSettingsSpan { get; set; } = 66.99e9;
        public double SweepSettingsFixed { get; set; } = 1e9;

        #endregion
    }
}
