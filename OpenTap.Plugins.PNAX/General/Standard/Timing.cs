// Author: MyName
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
    public class Timing : PNABaseStep
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

        private double _SweepTimeAuto;
        private double _SweepTimeStepped;
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
                    SweepTime = _SweepTimeAuto;
                }
                else if (_StandardChannelSweepMode == StandardChannelSweepModeEnum.Stepped)
                {
                    SweepTime = _SweepTimeStepped;
                }
            }
        }

        [Display("Sweep Sequence", Group: "Sweep Sequence", Order: 30)]
        public StandardChannelSweepSequenceEnum StandardChannelSweepSequence { get; set; }

        #endregion

        public Timing()
        {
            UpdateDefaultValues();
        }

        private void UpdateDefaultValues()
        {
            var defaultValues = PNAX.GetStandardChannelDefaultValues();
            _SweepTimeAuto = defaultValues.SweepTimeAuto;
            _SweepTimeStepped = defaultValues.SweepTimeStepped;
            DwellTime = defaultValues.DwellTime;
            SweepDelay = defaultValues.SweepDelay;
            AutoSweepTime = defaultValues.AutoSweepTime;
            FastSweep = defaultValues.FastSweep;
            StandardChannelSweepMode = defaultValues.StandardChannelSweepMode;
            StandardChannelSweepSequence = defaultValues.StandardChannelSweepSequence;
        }

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            List<(string, object)> retVal = new List<(string, object)>
            {
                ("Auto Sweep Time", AutoSweepTime)
            };
            if (AutoSweepTime == false)
            {
                retVal.Add(("Sweep Time", SweepTime));
                retVal.Add(("Dwell Time", DwellTime));
            }

            retVal.Add(("Sweep Delay", SweepDelay));
            retVal.Add(("Fast Sweep Reduce Settling Time", FastSweep));
            retVal.Add(("Sweep Mode", StandardChannelSweepMode));
            retVal.Add(("Sweep Sequence", StandardChannelSweepSequence));

            return retVal;
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
