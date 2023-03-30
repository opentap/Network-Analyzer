using OpenTap;   // Use OpenTAP infrastructure/core components (log,TestStep definition, etc)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OpenTap.Plugins.PNAX
{

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
    [AllowAsChildIn(typeof(NoiseFigureChannel))]
    [AllowAsChildIn(typeof(ScalarMixerChannel))]
    [Display("Mixer Setup", Groups: new[] { "PNA-X", "Converters" }, Description: "Insert description here", Order: 1)]
    public class MixerSetupTestStep : ConverterBaseStep
    {
        #region Settings
        private ConverterStagesEnum _ConverterStagesEnum;
        [Display("Converter Stages", Order: 10)]
        public override ConverterStagesEnum ConverterStages
        {
            get
            {
                return _ConverterStagesEnum;
            }
            set
            {
                _ConverterStagesEnum = value;
                DoubleStage = _ConverterStagesEnum == ConverterStagesEnum._2;
                try
                {
                    var a = GetParent<ConverterChannelBase>();
                    if (a != null)
                    {
                        a.ConverterStages = value;
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("can't find parent yet! ex: " + ex.Message);
                }
            }
        }

        [Display("Port", Group: "Input Port", Order: 30)]
        public PortsEnum PortInput { get; set;}
        [Display("Port", Group: "Output Port", Order: 60)]
        public PortsEnum PortOutput { get; set; }

        private LOEnum _portLO1;
        [Output]
        [Display("Port", Group: "LO1 Port", Order: 40)]
        public LOEnum PortLO1
        {
            get { return _portLO1; }
            set
            {
                _portLO1 = value;
                try
                {
                    var a = GetParent<ConverterChannelBase>();
                    // only if there is a parent of type SweptIMDChannel
                    if (a != null)
                    {
                        a.PortLO1 = value;
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("can't find parent yet! ex: " + ex.Message);
                }
            }
        }

        private LOEnum _portLO2;

        [Display("Port", Group: "LO2 Port", Order: 50)]
        [EnabledIf("DoubleStage", true ,HideIfDisabled =true)]
        public LOEnum PortLO2
        {
            get { return _portLO2; }
            set
            {
                _portLO2 = value;
                try
                {
                    var a = GetParent<ConverterChannelBase>();
                    // only if there is a parent of type SweptIMDChannel
                    if (a != null)
                    {
                        a.PortLO2 = value;
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("can't find parent yet! ex: " + ex.Message);
                }
            }
        }

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





        [Display("Enable Embedded LO", Group: "Embedded LO", Order: 70)]
        public bool EnableEmbeddedLO { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("Tuning Method", Group: "Embedded LO", Order: 71)]
        public TuningMethodEnum TuningMethod { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("Tuning Point Type", Group: "Embedded LO", Order: 72)]
        public TuningPointTypeEnum TuningPointType { get; set; }

        // TODO: FIgure out those numbers
        private int _tuningPoint;
        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("Tuning Point", Group: "Embedded LO", Order: 73)]
        public int TuningPoint 
        {
            get
            {
                // TODO
                // Need to get the Number Of Points value from Parent step
                // this value could come from the following child steps:
                //      GainCompressionFrequency.cs
                //      NoiseFigureFrequency.cs
                //      ToneFrequency.cs    (Swept IMD)
                //      ScalarMixerSweep.cs
                // TODO

                switch (TuningPointType)
                {
                    case TuningPointTypeEnum.FirstPoint:
                        return 1;
                    case TuningPointTypeEnum.MiddlePoint:
                        return 2;
                    case TuningPointTypeEnum.LastPoint:
                        return 3;
                    case TuningPointTypeEnum.Custom:
                        return _tuningPoint;
                    default:
                        return _tuningPoint;
                }
            }
            set { _tuningPoint = value; }
        }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("Tune Every", Group: "Embedded LO", Order: 74)]
        public int TuneEvery { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("Broadband Search", Group: "Embedded LO", Order: 75)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public int BroadBandSearch { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("IF BW", Group: "Embedded LO", Order: 76)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public int IFBW { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("Max Iterations", Group: "Embedded LO", Order: 77)]
        public int MaxIterations { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("Tolerance", Group: "Embedded LO", Order: 78)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public int Tolerance { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("LO Frequency Delta", Group: "Embedded LO", Order: 79)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LOFrequencyDelta { get; set; }




        #endregion

        public MixerSetupTestStep()
        {
            IsChildEditable = true;
            UpdateDefaultValues();
            // TODO
            // Add rule to indicate PortInput has to be different than PortOutput
        }

        private void UpdateDefaultValues()
        {
            var defaultValues = PNAX.GetMixerSetupDefaultValues();
            ConverterStages = defaultValues.ConverterStages;
            PortInput = defaultValues.PortInput;
            PortOutput = defaultValues.PortOutput;
            PortLO1 = defaultValues.PortLO1;
            PortLO2 = defaultValues.PortLO2;
            InputFractionalMultiplierNumerator = defaultValues.InputFractionalMultiplierNumerator;
            InputFractionalMultiplierDenominator = defaultValues.InputFractionalMultiplierDenominator;
            LO1FractionalMultiplierNumerator = defaultValues.LO1FractionalMultiplierNumerator;
            LO1FractionalMultiplierDenominator = defaultValues.LO1FractionalMultiplierDenominator;
            LO2FractionalMultiplierNumerator = defaultValues.LO2FractionalMultiplierNumerator;
            LO2FractionalMultiplierDenominator = defaultValues.LO2FractionalMultiplierDenominator;

            EnableEmbeddedLO = defaultValues.EnableEmbeddedLO;
            TuningMethod = defaultValues.TuningMethod;
            TuningPointType = defaultValues.TuningPointType;
            TuningPoint = defaultValues.TuningPoint;
            TuneEvery = defaultValues.TuneEvery;
            BroadBandSearch = defaultValues.BroadBandSearch;
            IFBW = defaultValues.IFBW;
            MaxIterations = defaultValues.MaxIterations;
            Tolerance = defaultValues.Tolerance;
            LOFrequencyDelta = defaultValues.LOFrequencyDelta;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetConverterStages(Channel, ConverterStages);
            PNAX.SetPortInputOutput(Channel, PortInput, PortOutput);
            PNAX.SetInputFractionalMultiplierNumerator(Channel, InputFractionalMultiplierNumerator);
            PNAX.SetInputFractionalMultiplierDenominator(Channel, InputFractionalMultiplierDenominator);
            PNAX.SetLOFractionalMultiplierNumerator(Channel, 1, LO1FractionalMultiplierNumerator);
            PNAX.SetLOFractionalMultiplierDenominator(Channel, 1, LO1FractionalMultiplierDenominator);
            PNAX.SetPortLO(Channel, 1, PortLO1);
            if (ConverterStages == ConverterStagesEnum._2)
            {
                PNAX.SetLOFractionalMultiplierNumerator(Channel, 2, LO2FractionalMultiplierNumerator);
                PNAX.SetLOFractionalMultiplierDenominator(Channel, 2, LO2FractionalMultiplierDenominator);
                PNAX.SetPortLO(Channel, 2, PortLO2);
            }

            PNAX.SetEnableEmbeddedLO(Channel, EnableEmbeddedLO);
            if (EnableEmbeddedLO)
            {
                PNAX.SetTuningMethod(Channel, TuningMethod);
                PNAX.SetTuningPoint(Channel, TuningPoint);
                PNAX.SetTuningInterval(Channel, TuneEvery);
                PNAX.SetTuningSpan(Channel, BroadBandSearch);
                PNAX.SetTuningIFBW(Channel, IFBW);
                PNAX.SetTuningMaxIterations(Channel, MaxIterations);
                PNAX.SetTuningTolerance(Channel, Tolerance);
                PNAX.SetLOFrequencyDelta(Channel, LOFrequencyDelta);
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
