using OpenTap;   // Use OpenTAP infrastructure/core components (log,TestStep definition, etc)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

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
        Port1,
        Port2,
        Port3,
        Port4
    }

    public enum LOEnum
    {
        NotControlled,
        Port3,
        Port4,
        Source3
    }

    public enum TuningMethodEnum
    {
        BroadbandAndPrecise,
        PreciseOnly,
        DisableTuning
    }

    public enum TuningPointTypeEnum
    {
        FirstPoint,
        MiddlePoint,
        LastPoint,
        Custom
    }

    [AllowAsChildIn(typeof(GainCompressionChannel))]
    [AllowAsChildIn(typeof(SweptIMDChannel))]
    [Display("Mixer Setup", Groups: new[] { "PNA-X", "Converters" }, Description: "Insert description here", Order: 1)]
    public class MixerSetupTestStep : TestStep
    {
        #region Settings

        [Browsable(false)]
        public bool IsControlledByParent { get; set; } = false;
        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
        [Display("PNA", Group: "Instrument Settings", Order: 1)]
        public PNAX PNAX { get; set; }

        private ConverterStagesEnum _ConverterStagesEnum;
        [Display("Converter Stages", Order: 10)]
        public ConverterStagesEnum ConverterStages
        {
            get
            {
                try
                {
                    _ConverterStagesEnum = GetParent<ConverterChannelBase>().ConverterStages;
                    UpdateDoubleStage();
                }
                catch (Exception ex)
                {
                    Log.Info(ex.Message);
                }
                return _ConverterStagesEnum;
            }
            set
            {
                _ConverterStagesEnum = value;
                UpdateDoubleStage();
            }
        }


        [Display("Port", Group: "Input Port", Order: 30)]
        public PortsEnum PortInput { get; set;}
        [Display("Port", Group: "Output Port", Order: 60)]
        public PortsEnum PortOutput { get; set; }

        [Output]
        [Display("Port", Group: "LO1 Port", Order: 40)]
        public LOEnum PortLO1 { get; set; }
        [Display("Port", Group: "LO2 Port", Order: 50)]
        [EnabledIf("DoubleStage", true ,HideIfDisabled =true)]
        public LOEnum PortLO2 { get; set; }

        [Display("Fractional Multiplier Numerator", Group: "Input Port", Order: 31)]
        public int InputFractionalMultiplierNumerator { get; set; }
        [Display("Fractional Multiplier Denominator", Group: "Input Port", Order: 32)]
        public int InputFractionalMultiplierDenominator { get; set; }

        [Display("Fractional Multiplier Numerator", Group: "LO1 Port", Order: 41)]
        public int LO1FractionalMultiplierNumerator { get; set; }
        [Display("Fractional Multiplier Denominator", Group: "LO1 Port", Order: 42)]
        public int LO1FractionalMultiplierDenominator { get; set; }

        [Display("Fractional Multiplier Numerator", Group: "LO2 Port", Order: 51)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        public int LO2FractionalMultiplierNumerator { get; set; }
        [Display("Fractional Multiplier Denominator", Group: "LO2 Port", Order: 52)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        public int LO2FractionalMultiplierDenominator { get; set; }

        [Browsable(false)]
        public bool DoubleStage { get; set; }

        private void UpdateDoubleStage()
        {
            if (_ConverterStagesEnum == ConverterStagesEnum._2)
            {
                DoubleStage = true;
            }
            else
            {
                DoubleStage = false;
            }
        }




        [Display("Enable Embedded LO", Group: "Embedded LO", Order: 70)]
        public bool EnableEmbeddedLO { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("Tuning Method", Group: "Embedded LO", Order: 71)]
        public TuningMethodEnum TuningMethod { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("Tuning Point Type", Group: "Embedded LO", Order: 72)]
        public TuningPointTypeEnum TuningPointType { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("Tuning Point", Group: "Embedded LO", Order: 73)]
        public int TuningPoint { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("Tune Every", Group: "Embedded LO", Order: 74)]
        public int TuneEvery { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("Broadband Search", Group: "Embedded LO", Order: 75)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double BroadBandSearch { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("IF BW", Group: "Embedded LO", Order: 76)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double IFBW { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("Max Iterations", Group: "Embedded LO", Order: 77)]
        public int MaxIterations { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("Tolerance", Group: "Embedded LO", Order: 78)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double Tolerance { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("LO Frequency Delta", Group: "Embedded LO", Order: 79)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LOFrequencyDelta { get; set; }




        #endregion

        public MixerSetupTestStep()
        {
            ConverterStages = ConverterStagesEnum._1;
            PortInput = PortsEnum.Port1;
            PortOutput = PortsEnum.Port2;
            PortLO1 = LOEnum.NotControlled;
            PortLO2 = LOEnum.NotControlled;
            InputFractionalMultiplierNumerator = 1;
            InputFractionalMultiplierDenominator = 1;
            LO1FractionalMultiplierNumerator = 1;
            LO1FractionalMultiplierDenominator = 1;
            LO2FractionalMultiplierNumerator = 1;
            LO2FractionalMultiplierDenominator = 1;

            EnableEmbeddedLO = false;
            TuningMethod = TuningMethodEnum.BroadbandAndPrecise;
            TuningPointType = TuningPointTypeEnum.MiddlePoint;
            TuningPoint = 101;
            TuneEvery = 1;
            BroadBandSearch = 3e6;
            IFBW = 30e3;
            MaxIterations = 5;
            Tolerance = 1;
            LOFrequencyDelta = 103.504999997101e9;

        }

        public override void Run()
        {
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
