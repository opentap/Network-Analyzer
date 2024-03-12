// Author: CMontes
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
    [AllowAsChildIn(typeof(StandardChannel))]
    [Display("Pulse Setup", Groups: new[] { "Network Analyzer", "General" }, Description: "Insert a description here")]
    public class PulseSetup : PNABaseStep
    {
        #region Settings

        [Display("Pulse Mode", Group: "Pulse Measurement", Order: 21)]
        public PulseModeEnumtype PulseMode { get; set; }

        [Display("Pulse Width", Group: "Pulse Timing", Order: 30)]
        [Unit("sec", UseEngineeringPrefix: true, StringFormat: "000.000")]
        public double PulseWidthPrimary { get; set; }

        [Display("Pulse Period", Group: "Pulse Timing", Order: 31)]
        [Unit("sec", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double PulsePeriodPrimary { get; set; }

        [Display("Pulse Frequency", Group: "Pulse Timing", Order: 32)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double PulseFrequencyPrimary { get; set; }


        [Display("Autoselect Pulse Detection Method", Groups: new[] { "Properties", "Pulse Detection Method" }, Order: 41)]
        public bool PulseDetectionMethodAuto { get; set; }

        [EnabledIf("PulseDetectionMethodAuto", false, HideIfDisabled = false)]
        [Display("Autoselect Pulse Detection Method", Groups: new[] { "Properties", "Pulse Detection Method" }, Order: 42)]
        public PulseDetectionMethodEnumtype PulseDetectionMethod { get; set; }

        [EnabledIf("PulseDetectionMethodAuto", false, HideIfDisabled = false)]
        [EnabledIf("PulseDetectionMethod", PulseDetectionMethodEnumtype.Narrowband, HideIfDisabled = false)]
        [Display("SW Gating", Groups: new[] { "Properties", "Pulse Detection Method" }, Order: 43)]
        public bool PulseDetectionMethodSWGating { get; set; }

        [Display("Autoselect IF Path Gain and Loss", Groups: new[] { "Properties"}, Order: 44)]
        public bool IfPathGainAndLossAuto { get; set; }

        [EnabledIf("PulseDetectionMethodAuto", false, HideIfDisabled = false)]
        [EnabledIf("PulseDetectionMethod", PulseDetectionMethodEnumtype.Narrowband, HideIfDisabled = false)]
        [Display("Optimize Pulse Frequency", Groups: new[] { "Properties" }, Order: 45)]
        public bool OptimizePulseFrequency { get; set; }

        [Display("Autoselect Profile Sweep Time", Groups: new[] { "Properties" }, Order: 46)]
        public bool ProfileSweepTimeAuto { get; set; }

        [Display("Sweep Time", Groups: new[] { "Properties" }, Order: 47)]
        [Unit("sec", UseEngineeringPrefix: true, StringFormat: "00.000")]
        public double SweepTime { get; set; }

        [Display("Number of Points", Groups: new[] { "Properties" }, Order: 48)]
        public int NumberOfPoints { get; set; }

        [Display("IFBW", Groups: new[] { "Properties" }, Order: 49)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "00")]
        public double IFBW { get; set; }

        [Display("Primary Clock", Groups: new[] { "Measurement Timing" }, Order: 50)]
        public PulsePrimaryClockEnumtype PulseClockPrimary { get; set; }

        [Display("Autoselect Width & Delay", Groups: new[] { "Measurement Timing" }, Order: 51)]
        public bool WidthAndDelayAuto { get; set; }

        [Display("Autoselect Pulse Generators", Groups: new[] { "Measurement Timing" }, Order: 52)]
        public bool PulseGeneratorsAuto { get; set; }

        #endregion

        public PulseSetup()
        {
            PulseMode = PulseModeEnumtype.Off;

            PulseWidthPrimary = 100e-6;
            PulsePeriodPrimary = 1e-3;
            PulseFrequencyPrimary = 1e3;

            PulseDetectionMethodAuto = true;
            PulseDetectionMethod = PulseDetectionMethodEnumtype.Wideband;
            PulseDetectionMethodSWGating = true;
            IfPathGainAndLossAuto = true;
            OptimizePulseFrequency = true;
            ProfileSweepTimeAuto = true;
            SweepTime = 16.884e-3;
            NumberOfPoints = 201;
            IFBW = 15e3;

            PulseClockPrimary = PulsePrimaryClockEnumtype.Internal;
            WidthAndDelayAuto = true;
            PulseGeneratorsAuto = true;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
