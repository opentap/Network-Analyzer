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
            MODXNewTrace modNewTrace = ConfigureChildStep(new MODXNewTrace());
            MODModulate modModulate = ConfigureChildStep(new MODModulate());
            MODSourceCorrection modSourceCorrection = ConfigureChildStep(new MODSourceCorrection());
            MODSweep modSweep = ConfigureChildStep(new MODSweep());
            MODRFPath modRFPath = ConfigureChildStep(new MODRFPath());
            MODXMixer modxMixer = ConfigureChildStep(new MODXMixer());
            MODMeasure modMeasure = ConfigureChildStep(new MODMeasure());

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
            DefineDummyTrace("Modulation Distortion Converters", "PIn1", string.Empty);

            RunChildSteps(); //If the step supports child steps.

            PNAX.SetSweepMode(Channel, SweepModeEnumType.SING);

            UpgradeVerdict(Verdict.Pass);
            UpdateMetaData();
        }
    }
}
