// Author: CMontes
// Copyright:   Copyright 2024 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using OpenTap;
using OpenTap.Plugins.PNAX.General.Spectrum_Analyzer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OpenTap.Plugins.PNAX
{
    [AllowAsChildIn(typeof(GeneralSweptIMDChannel))]
    [AllowAsChildIn(typeof(SpectrumAnalyzerChannel))]
    [AllowAsChildIn(typeof(GeneralNoiseFigureChannel))]
    [AllowAsChildIn(typeof(MODChannel))]
    [AllowAsChildIn(typeof(GeneralGainCompressionChannel))]
    [AllowAsChildIn(typeof(DIQChannel))]
    [AllowAsChildIn(typeof(ScalarMixerChannel))]
    [AllowAsChildIn(typeof(NoiseFigureChannel))]
    [AllowAsChildIn(typeof(MODXChannel))]
    [AllowAsChildIn(typeof(GainCompressionChannel))]
    [Display("Pulse Setup Basic", Groups: new[] { "Network Analyzer", "General" }, Description: "Insert a description here")]
    public class PulseSetupBasic : PNABaseStep
    {
        #region Settings
        [Display("Pulse Mode", Group: "Pulse Measurement", Order: 21)]
        public PulseModeBasicEnumtype PulseMode { get; set; }

        [Display("Pulse Width", Group: "Pulse Timing", Order: 30)]
        [Unit("sec", UseEngineeringPrefix: true, StringFormat: "000.000")]
        public double PulseWidthPrimary { get; set; }

        [Display("Pulse Period", Group: "Pulse Timing", Order: 31)]
        [Unit("sec", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double PulsePeriodPrimary { get; set; }

        [Display("Pulse Frequency", Group: "Pulse Timing", Order: 32)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double PulseFrequencyPrimary { get; set; }


        private PulseTriggerEnumtype _PulseTriggerType;
        [Display("Trigger Source", Groups: new[] { "Pulse Trigger" }, Order: 60)]
        public PulseTriggerEnumtype PulseTriggerType
        {
            get
            {
                return _PulseTriggerType;
            }
            set
            {
                _PulseTriggerType = value;
                foreach (var a in this.ChildTestSteps)
                {
                    if (a is PulseGenerators)
                    {
                        (a as PulseGenerators).PulseTriggerType = value;
                    }
                }
            }
        }


        [EnabledIf("PulseTriggerType", PulseTriggerEnumtype.External, HideIfDisabled = false)]
        [Display("Trigger Level/Edge", Groups: new[] { "Pulse Trigger" }, Order: 61)]
        public PulseTriggerLevelEdgeEnumtype pulseTriggerLevelEdge { get; set; }

        private bool _SynchADCUsingPulseTrigger;
        [Display("Synchronize ADCs Using Pulse Trigger", Groups: new[] { "Pulse Trigger" }, Order: 62)]
        public bool SynchADCUsingPulseTrigger
        {
            get
            {
                return _SynchADCUsingPulseTrigger;
            }
            set
            {
                _SynchADCUsingPulseTrigger = value;
                foreach (var a in this.ChildTestSteps)
                {
                    if (a is PulseGenerators)
                    {
                        (a as PulseGenerators).SynchADCUsingPulseTrigger = value;
                    }
                }
            }
        }

        //[Display("ADC trigger delay", Groups: new[] { "Pulse Trigger" }, Order: 63)]
        //public double ADCTriggerDelay { get; set; }

        #endregion

        public PulseSetupBasic()
        {
            PulseMode = PulseModeBasicEnumtype.Off;

            PulseWidthPrimary = 100e-6;
            PulsePeriodPrimary = 1e-3;
            PulseFrequencyPrimary = 1e3;

            PulseTriggerType = PulseTriggerEnumtype.Internal;
            pulseTriggerLevelEdge = PulseTriggerLevelEdgeEnumtype.HighLevel;
            SynchADCUsingPulseTrigger = false;
            //ADCTriggerDelay = 250e-3;

        }

        public override void Run()
        {
            // Pulse Measurement
            PNAX.PulseMode(Channel, PulseMode);

            // Pulse Timing
            PNAX.PulsePrimaryWidth(Channel, PulseWidthPrimary);
            PNAX.PulsePrimaryPeriod(Channel, PulsePeriodPrimary);
            PNAX.PulsePrimaryFrequency(Channel, PulseFrequencyPrimary);

            // Trigger
            PNAX.PulseGeneratorTrigger(Channel, PulseTriggerType);
            switch (pulseTriggerLevelEdge)
            {
                case PulseTriggerLevelEdgeEnumtype.HighLevel:
                    PNAX.PulseTriggerType(Channel, PulseTriggerTypeEnumtype.Level);
                    PNAX.PulseTriggerPolarity(Channel, PulseTriggerPolarityEnumtype.Positive);
                    break;
                case PulseTriggerLevelEdgeEnumtype.LowLevel:
                    PNAX.PulseTriggerType(Channel, PulseTriggerTypeEnumtype.Level);
                    PNAX.PulseTriggerPolarity(Channel, PulseTriggerPolarityEnumtype.Negative);
                    break;
                case PulseTriggerLevelEdgeEnumtype.PositiveEdge:
                    PNAX.PulseTriggerType(Channel, PulseTriggerTypeEnumtype.Edge);
                    PNAX.PulseTriggerPolarity(Channel, PulseTriggerPolarityEnumtype.Positive);
                    break;
                case PulseTriggerLevelEdgeEnumtype.NegativeEdge:
                    PNAX.PulseTriggerType(Channel, PulseTriggerTypeEnumtype.Edge);
                    PNAX.PulseTriggerPolarity(Channel, PulseTriggerPolarityEnumtype.Negative);
                    break;
            }
            PNAX.PulseGeneratorSyncADCs(Channel, SynchADCUsingPulseTrigger);
            // Setting Pulse 0 on Basic Pulse setup affects the Trigger on the instrument
            //PNAX.PulseGeneratorDelay(Channel, "Pulse0", ADCTriggerDelay);

            RunChildSteps(); //If the step supports child steps.

            UpgradeVerdict(Verdict.Pass);
        }

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            UpdateMetaData();
            List<(string, object)> retVal = new List<(string, object)>();

            retVal.Add(($"PulseMode", PulseMode));
            retVal.Add(($"PulseWidthPrimary", PulseWidthPrimary));
            retVal.Add(($"PulsePeriodPrimary", PulsePeriodPrimary));
            retVal.Add(($"PulseFrequencyPrimary", PulseFrequencyPrimary));

            retVal.Add(($"PulseTriggerType", PulseTriggerType));
            retVal.Add(($"pulseTriggerLevelEdge", pulseTriggerLevelEdge));
            retVal.Add(($"SynchADCUsingPulseTrigger", SynchADCUsingPulseTrigger));

            foreach (var a in MetaData)
            {
                retVal.Add(a);
            }

            return retVal;
        }
    }
}
