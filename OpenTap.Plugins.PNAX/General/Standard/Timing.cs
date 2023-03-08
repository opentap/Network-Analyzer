﻿// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
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
    public enum StandardChannelSweepModeEnum
    {
        Auto,
        Stepped
    }

    public enum StandardChannelSweepSequenceEnum
    {
        Standard,
        PointSweep
    }

    [AllowAsChildIn(typeof(StandardChannel))]
    [Display("Timing", Groups: new[] { "PNA-X", "General",  "Standard" }, Description: "Insert a description here")]
    public class Timing : GeneralStandardChannelBaseStep
    {
        #region Settings
        [EnabledIf("AutoSweepTime", false, HideIfDisabled = false)]
        [Unit("S", UseEngineeringPrefix: true, PreScaling: 0.001, StringFormat: "0.000")]
        [Display("Sweep Time", Group: "Time", Order: 10)]
        public double SweepTime { get; set; }

        [EnabledIf("AutoSweepTime", false, HideIfDisabled = false)]
        [Unit("S", UseEngineeringPrefix: true, PreScaling: 1e-6)]
        [Display("Dwell Time", Group: "Time", Order: 11)]
        public double DwellTime { get; set; }

        [Unit("S", UseEngineeringPrefix: true, PreScaling: 1e-6)]
        [Display("Sweep Delay", Group: "Time", Order: 12)]
        public double SweepDelay { get; set; }

        [Display("Auto Sweep Time", Group: "Time", Order: 13)]
        public bool AutoSweepTime { get; set; }

        [Display("Fast Sweep - Reduce settling time", Group: "Time", Order: 14)]
        public bool FastSweep { get; set; }

        private StandardChannelSweepModeEnum _StandardChannelSweepMode;
        [Display("Sweep Mode", Group: "Sweep Mode", Order: 20)]
        public StandardChannelSweepModeEnum StandardChannelSweepMode {
            get
            {
                return _StandardChannelSweepMode;
            }
            set 
            {
                _StandardChannelSweepMode = value;
                if (_StandardChannelSweepMode == StandardChannelSweepModeEnum.Auto)
                {
                    SweepTime = GeneralStandardSettings.Current.SweepTimeAuto;
                }
                else if (_StandardChannelSweepMode == StandardChannelSweepModeEnum.Stepped)
                {
                    SweepTime = GeneralStandardSettings.Current.SweepTimeStepped;
                }
            }
        }

        [Display("Sweep Sequence", Group: "Sweep Sequence", Order: 30)]
        public StandardChannelSweepSequenceEnum StandardChannelSweepSequence { get; set; }

        #endregion

        public Timing()
        {
            DwellTime = GeneralStandardSettings.Current.DwellTime;
            SweepDelay = GeneralStandardSettings.Current.SweepDelay;
            AutoSweepTime = GeneralStandardSettings.Current.AutoSweepTime;
            FastSweep = GeneralStandardSettings.Current.FastSweep;
            StandardChannelSweepMode = GeneralStandardSettings.Current.StandardChannelSweepMode;
            StandardChannelSweepSequence = GeneralStandardSettings.Current.StandardChannelSweepSequence;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetAutoSweepTime(Channel, AutoSweepTime);
            if (AutoSweepTime == false)
            {
                // TODO : BUG: if trying to use seconds when the prescale is set to 1e-3, the value can never be in seconds.
                // BUG in general prescaling of decimal numbers is not working
                PNAX.SetSweepTime(Channel, SweepTime);
                PNAX.SetDwellTime(Channel, DwellTime);
            }
            PNAX.SetSweepDelay(Channel, SweepDelay);
            PNAX.SetFastSweepMode(Channel, FastSweep);

            PNAX.SetSweepMode(Channel, StandardChannelSweepMode);
            PNAX.SetSweepSequence(Channel, StandardChannelSweepSequence);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
