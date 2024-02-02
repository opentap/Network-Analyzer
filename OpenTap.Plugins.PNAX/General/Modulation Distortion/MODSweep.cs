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
    [AllowAsChildIn(typeof(MODChannel))]
    [AllowAsChildIn(typeof(MODXChannel))]
    [Display("MOD Sweep", Groups: new[] { "Network Analyzer", "General", "Modulation Distortion" }, Description: "Insert a description here")]
    public class MODSweep : PNABaseStep
    {
        #region Settings
        [Display("Sweep Type", Group: "Sweep Settings", Order: 10)]
        public MODSweepTypeEnum MODSweepType { get; set; }

        [Display("Carrier Frequency", Group: "Sweep Settings", Order: 11)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double CarrierFrequency { get; set; }

        [Display("Span", Group: "Sweep Settings", Order: 12)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double Span { get; set; }

        [Display("Noise BW", Group: "Sweep Settings", Order: 13)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0")]
        public int NoiseBW { get; set; }

        [Display("Power Port", Group: "Sweep Settings", Order: 14)]
        public MODPowerPortEnum PowerPort { get; set; }

        [EnabledIf("MODSweepType", MODSweepTypeEnum.Fixed, HideIfDisabled = true)]
        [Display("Power", Group: "Sweep Settings", Order: 15)]
        [Unit("dBm", UseEngineeringPrefix: false, StringFormat: "0.00")]
        public double MODPower { get; set; }

        [EnabledIf("MODSweepType", MODSweepTypeEnum.Power, HideIfDisabled = true)]
        [Display("Start Power", Group: "Sweep Settings", Order: 16)]
        [Unit("dBm", UseEngineeringPrefix: false, StringFormat: "0.00")]
        public double MODStartPower { get; set; }

        [EnabledIf("MODSweepType", MODSweepTypeEnum.Power, HideIfDisabled = true)]
        [Display("Stop Power", Group: "Sweep Settings", Order: 17)]
        [Unit("dBm", UseEngineeringPrefix: false, StringFormat: "0.00")]
        public double MODStopPower { get; set; }

        [EnabledIf("MODSweepType", MODSweepTypeEnum.Power, HideIfDisabled = true)]
        [Display("Number of Points", Group: "Sweep Settings", Order: 18)]
        public int NumberOfPoints { get; set; }

        [EnabledIf("MODSweepType", MODSweepTypeEnum.Power, HideIfDisabled = true)]
        [Display("Auto-Increase NBW", Group: "Sweep Settings", Order: 19)]
        public bool AutoIncreaseNBW { get; set; }

        #endregion

        public MODSweep()
        {
            MODSweepType = MODSweepTypeEnum.Fixed;
            CarrierFrequency = 1.5e9;
            Span = 300e6;
            NoiseBW = 100;
            PowerPort = MODPowerPortEnum.Din;
            MODPower = -10;

            MODStartPower = -20;
            MODStopPower = -10;
            NumberOfPoints = 11;
            AutoIncreaseNBW = false;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.MODSweepType(Channel, MODSweepType);

            PNAX.MODCarrierFreq(Channel, CarrierFrequency);
            PNAX.MODSpan(Channel, Span);
            PNAX.MODNoiseBW(Channel, NoiseBW);
            PNAX.MODSetPowerPort(Channel, PowerPort);
            PNAX.MODSetPower(Channel, MODPower);

            if (MODSweepType == MODSweepTypeEnum.Power)
            {
                PNAX.MODPowerSweepStart(Channel, MODStartPower);
                PNAX.MODPowerSweepStop(Channel, MODStopPower);
                PNAX.MODPowerSweepNumberOfPoints(Channel, NumberOfPoints);
                PNAX.MODPowerSweepNoiseBW(Channel, NoiseBW);
                PNAX.MODPowerSweepAutoIncreaseBW(Channel, AutoIncreaseNBW);
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
