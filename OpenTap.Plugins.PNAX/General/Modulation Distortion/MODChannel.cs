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
    [Display("Modulation Distortion Channel", Groups: new[] { "Network Analyzer", "General", "Modulation Distortion" }, Description: "Insert a description here")]
    public class MODChannel : PNABaseStep
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public MODChannel()
        {
            IsControlledByParent = false;
            Channel = 1;

            // Traces
            MODNewTrace modNewTrace = new MODNewTrace { IsControlledByParent = true, Channel = this.Channel };
            MODModulate modModulate = new MODModulate { IsControlledByParent = true, Channel = this.Channel };
            MODSweep modSweep = new MODSweep { IsControlledByParent = true, Channel = this.Channel };
            MODMeasure modMeasure = new MODMeasure { IsControlledByParent = true, Channel = this.Channel };

            this.ChildTestSteps.Add(modNewTrace);
            this.ChildTestSteps.Add(modSweep);
            this.ChildTestSteps.Add(modModulate);
            this.ChildTestSteps.Add(modMeasure);
        }

        public override void Run()
        {
            PNAX.GetNewTraceID(Channel);
            // Define a dummy measurement so we can setup all channel parameters
            // we will add the traces during the StandardSingleTrace or StandardNewTrace test steps
            PNAX.ScpiCommand($"CALCulate{Channel}:CUST:DEFine \'CH{Channel}_DUMMY_1\',\'Modulation Distortion\',\'PIn1\'");

            RunChildSteps(); //If the step supports child steps.

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
