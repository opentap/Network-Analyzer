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

        #endregion

        public PulseSetupBasic()
        {
            PulseMode = PulseModeBasicEnumtype.Off;

            PulseWidthPrimary = 100e-6;
            PulsePeriodPrimary = 1e-3;
            PulseFrequencyPrimary = 1e3;
        }

        public override void Run()
        {
            // Pulse Measurement
            PNAX.PulseMode(Channel, PulseMode);

            // Pulse Timing
            PNAX.PulsePrimaryWidth(Channel, PulseWidthPrimary);
            PNAX.PulsePrimaryPeriod(Channel, PulsePeriodPrimary);
            PNAX.PulsePrimaryFrequency(Channel, PulseFrequencyPrimary);

            RunChildSteps(); //If the step supports child steps.

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
