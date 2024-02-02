// Author: MyName
// Copyright:   Copyright 2024 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OpenTap.Plugins.PNAX
{
    [AllowAsChildIn(typeof(TestPlan))]
    [AllowAsChildIn(typeof(MODXChannel))]
    [Display("Mixer", Groups: new[] { "Network Analyzer", "Converters", "Modulation Distortion" }, Description: "Insert a description here")]
    public class MODXMixer : PNABaseStep
    {
        #region Settings

        [Display("Input", Groups: new[] { "Mixer Setup", "Input" }, Order: 10)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.0")]
        public double InputMixerFrequency { get; set; }

        [Display("Port", Groups: new[] { "Mixer Setup", "Input" }, Order: 10.1)]
        public MODDutPortEnum InputPort { get; set; }


        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("IF", Groups: new[] { "Mixer Setup", "IF" }, Order: 11)]
        public SidebandTypeEnum IFMixerFrequency { get; set; }

        [Display("Output", Groups: new[] { "Mixer Setup", "Output" }, Order: 12)]
        public SidebandTypeEnum OutputMixerFrequency { get; set; }

        [Display("Port", Groups: new[] { "Mixer Setup", "Output" }, Order: 13)]
        public MODDutPortEnum OutputPort { get; set; }



        [Display("LO1 Fractional Multiplier Numerator", Groups: new[] { "Mixer Setup", "LO1" }, Order: 20)]
        public int LO1FractionalMultiplierNumerator { get; set; }

        [Display("LO1 Fractional Multiplier Denominator", Groups: new[] { "Mixer Setup", "LO1" }, Order: 21)]
        public int LO1FractionalMultiplierDenominator { get; set; }

        [Display("LO1", Groups: new[] { "Mixer Setup", "LO1" }, Order: 22)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.0")]
        public double LO1MixerFrequency { get; set; }


        [Display("LO2 Fractional Multiplier Numerator", Groups: new[] { "Mixer Setup", "LO2" }, Order: 30)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        public int LO2FractionalMultiplierNumerator { get; set; }

        [Display("LO2 Fractional Multiplier Denominator", Groups: new[] { "Mixer Setup", "LO2" }, Order: 31)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        public int LO2FractionalMultiplierDenominator { get; set; }

        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("LO2", Groups: new[] { "Mixer Setup", "LO2" }, Order: 32)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.0")]
        public double LO2MixerFrequency { get; set; }




        [Display("Enable Embedded LO", Groups: new[] { "Mixer Setup", "Embedded LO" }, Order: 40)]
        public bool EnableEmbeddedLO { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("Tuning Method", Groups: new[] { "Mixer Setup", "Embedded LO" }, Order: 41)]
        public TuningMethodEnum TuningMethod { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("Tune Every", Groups: new[] { "Mixer Setup", "Embedded LO" }, Order: 44)]
        public int TuneEvery { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("Broadband Search", Groups: new[] { "Mixer Setup", "Embedded LO" }, Order: 45)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public int BroadBandSearch { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("Noise BW", Groups: new[] { "Mixer Setup", "Embedded LO" }, Order: 46)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public int NoiseBW { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("Max Iterations", Groups: new[] { "Mixer Setup", "Embedded LO" }, Order: 47)]
        public int MaxIterations { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("Tolerance", Groups: new[] { "Mixer Setup", "Embedded LO" }, Order: 48)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public int Tolerance { get; set; }

        [EnabledIf("EnableEmbeddedLO", true, HideIfDisabled = true)]
        [Display("LO Frequency Delta", Groups: new[] { "Mixer Setup", "Embedded LO" }, Order: 49)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double LOFrequencyDelta { get; set; }




        [Display("LO1 Source", Groups: new[] { "Power", "LO1" }, Order: 50)]
        public string LO1Source { get; set; }

        [Display("LO1 Power", Groups: new[] { "Power", "LO1" }, Order: 51)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double LO1Power { get; set; }

        [Display("LO1 Leveling", Groups: new[] { "Power", "LO1" }, Order: 52)]
        public SourceLevelingModeType LO1SourceLevelingMode{ get; set; }

        [Display("LO1 Attenuator", Groups: new[] { "Power", "LO1" }, Order: 53)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0")]
        public double LO1Attenuator { get; set; }




        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [EnabledIf("ConverterStages", ConverterStagesEnum._2, HideIfDisabled = true)]
        [Display("LO2 Source", Groups: new[] { "Power", "LO2" }, Order: 60)]
        public string LO2Source { get; set; }

        [EnabledIf("ConverterStages", ConverterStagesEnum._2, HideIfDisabled = true)]
        [Display("LO2 Power", Groups: new[] { "Power", "LO2" }, Order: 61)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double LO2Power { get; set; }

        [EnabledIf("ConverterStages", ConverterStagesEnum._2, HideIfDisabled = true)]
        [Display("LO2 Leveling", Groups: new[] { "Power", "LO2" }, Order: 62)]
        public SourceLevelingModeType LO2SourceLevelingMode { get; set; }

        [EnabledIf("ConverterStages", ConverterStagesEnum._2, HideIfDisabled = true)]
        [Display("LO2 Attenuator", Groups: new[] { "Power", "LO2" }, Order: 63)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0")]
        public double LO2Attenuator { get; set; }

        #endregion

        public MODXMixer()
        {
            IsConverterEditable = true;
            IsConverter = true;

            ConverterStages = ConverterStagesEnum._1;
            InputMixerFrequency = 1.5e9;
            InputPort = MODDutPortEnum.Port1;
            IFMixerFrequency = SidebandTypeEnum.Low;
            OutputMixerFrequency = SidebandTypeEnum.Low;
            OutputPort = MODDutPortEnum.Port2;

            LO1FractionalMultiplierNumerator = 1;
            LO1FractionalMultiplierDenominator = 1;
            LO1MixerFrequency = 0;
            LO2FractionalMultiplierNumerator = 1;
            LO2FractionalMultiplierDenominator = 1;
            LO2MixerFrequency = 0;

            EnableEmbeddedLO = false;
            TuningMethod = TuningMethodEnum.BroadbandAndPrecise;
            TuneEvery = 1;
            BroadBandSearch = 3000000;
            NoiseBW = 3200;
            MaxIterations = 5;
            Tolerance = 1;
            LOFrequencyDelta = 0;

            LO1Source = "Not controlled";
            LO1Power = -10;
            LO1SourceLevelingMode = SourceLevelingModeType.OPENloop;
            LO1Attenuator = 0;

            LO2Source = "Not controlled";
            LO2Power = -10;
            LO2SourceLevelingMode = SourceLevelingModeType.OPENloop;
            LO2Attenuator = 0;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            // Start from scratch
            PNAX.MixerDiscard(Channel);

            // Setup
            PNAX.SetConverterStages(Channel, ConverterStages);
            PNAX.MODCarrierFreq(Channel, InputMixerFrequency);
            PNAX.SetFrequencyOutputSideband(Channel, OutputMixerFrequency);

            PNAX.SetLOFractionalMultiplierNumerator(Channel, 1, LO1FractionalMultiplierNumerator);
            PNAX.SetLOFractionalMultiplierDenominator(Channel, 1, LO1FractionalMultiplierDenominator);
            PNAX.SetFrequencyLOFixed(Channel, 1, LO1MixerFrequency);
            if (ConverterStages == ConverterStagesEnum._2)
            {
                PNAX.SetFrequencyIFSideband(Channel, IFMixerFrequency);

                PNAX.SetLOFractionalMultiplierNumerator(Channel, 2, LO2FractionalMultiplierNumerator);
                PNAX.SetLOFractionalMultiplierDenominator(Channel, 2, LO2FractionalMultiplierDenominator);
                PNAX.SetFrequencyLOFixed(Channel, 2, LO2MixerFrequency);
                PNAX.MixerCalc(Channel);
            }
            //PNAX.MixerApply(Channel);

            // Embedded
            PNAX.SetEnableEmbeddedLO(Channel, EnableEmbeddedLO);
            if (EnableEmbeddedLO)
            {
                PNAX.SetTuningMethod(Channel, TuningMethod);
                PNAX.SetTuningInterval(Channel, TuneEvery);
                PNAX.SetTuningSpan(Channel, BroadBandSearch);
                PNAX.SetTuningNoiseBW(Channel, NoiseBW);
                PNAX.SetTuningMaxIterations(Channel, MaxIterations);
                PNAX.SetTuningTolerance(Channel, Tolerance);
                PNAX.SetLOFrequencyDelta(Channel, LOFrequencyDelta);
            }
            PNAX.MixerApply(Channel);


            // Power
            PNAX.SetPortLO(Channel, 1, LO1Source);
            //PNAX.MODMixerSourceRole(Channel, "INPUT", "Device0");
            PNAX.SetLOPower(Channel, 1, LO1Power);
            PNAX.SetSourceLevelingMode(Channel, InputPort, LO1SourceLevelingMode.ToString());
            PNAX.SetSourceAttenuator(Channel, InputPort, LO1Attenuator);

            if (ConverterStages == ConverterStagesEnum._2)
            {
                PNAX.SetPortLO(Channel, 2, LO2Source);
                //PNAX.MODMixerSourceRole(Channel, "INPUT", "Device0");
                PNAX.SetLOPower(Channel, 2, LO2Power);
                PNAX.SetSourceLevelingMode(Channel, InputPort, LO1SourceLevelingMode.ToString());
                PNAX.SetSourceAttenuator(Channel, InputPort, LO1Attenuator);
            }
            // Apply changes to instrument
            PNAX.MixerCalc(Channel);
            PNAX.MixerApply(Channel);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
