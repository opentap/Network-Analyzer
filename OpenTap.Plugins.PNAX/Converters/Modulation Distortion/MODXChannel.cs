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
    [Display("Modulation Distortion Converters Channel", Groups: new[] { "Network Analyzer", "Converters", "Modulation Distortion" }, Description: "Insert a description here")]
    public class MODXChannel : PNABaseStep
    {
        #region Settings

        [Display("Sweep Mode", Group: "Settings", Order: 10)]
        public SweepModeEnumType sweepMode { get; set; }
        #endregion

        public MODXChannel()
        {
            IsControlledByParent = false;
            Channel = 1;
            sweepMode = SweepModeEnumType.SING;

            // Traces
            MODXNewTrace modNewTrace = new MODXNewTrace { IsControlledByParent = true, Channel = this.Channel };
            MODModulate modModulate = new MODModulate { IsControlledByParent = true, Channel = this.Channel };
            MODSourceCorrection modSourceCorrection = new MODSourceCorrection { IsControlledByParent = true, Channel = this.Channel };
            MODSweep modSweep = new MODSweep { IsControlledByParent = true, Channel = this.Channel };
            MODRFPath modRFPath = new MODRFPath { IsControlledByParent = true, Channel = this.Channel };
            MODXMixer modxMixer = new MODXMixer { IsControlledByParent = true, Channel = this.Channel };
            MODMeasure modMeasure = new MODMeasure { IsControlledByParent = true, Channel = this.Channel };

            this.ChildTestSteps.Add(modNewTrace);
            this.ChildTestSteps.Add(modSweep);
            this.ChildTestSteps.Add(modRFPath);
            this.ChildTestSteps.Add(modModulate);
            this.ChildTestSteps.Add(modxMixer);
            this.ChildTestSteps.Add(modSourceCorrection);
            this.ChildTestSteps.Add(modMeasure);
        }

        public override void Run()
        {
            PNAX.GetNewTraceID(Channel);
            // Define a dummy measurement so we can setup all channel parameters
            // we will add the traces during the StandardSingleTrace or StandardNewTrace test steps
            PNAX.ScpiCommand($"CALCulate{Channel}:CUST:DEFine \'CH{Channel}_DUMMY_1\',\'Modulation Distortion Converters\',\'PIn1\'");

            RunChildSteps(); //If the step supports child steps.

            PNAX.SetSweepMode(Channel, SweepModeEnumType.SING);

            UpgradeVerdict(Verdict.Pass);
            UpdateMetaData();
        }
    }
}
