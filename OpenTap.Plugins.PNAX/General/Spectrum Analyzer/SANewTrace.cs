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

namespace OpenTap.Plugins.PNAX.General.Spectrum_Analyzer
{
    //[AllowAsChildIn(typeof(SpectrumAnalyzerChannel))]
    //[AllowChildrenOfType(typeof(SASingleTrace))]
    [Display("SA New Trace", Groups: new[] { "Network Analyzer", "General", "Spectrum Analyzer" }, Description: "Insert a description here")]
    public class SANewTrace : AddNewTraceBaseStep
    {
        #region Settings
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public SATraceEnum Meas { get; set; }
        #endregion

        public SANewTrace()
        {
            Meas = SATraceEnum.B;
            ChildTestSteps.Add(new SASingleTrace() { PNAX = this.PNAX, Meas = this.Meas, Channel = this.Channel, IsControlledByParent = true, EnableTraceSettings = true });
        }

        protected override void AddNewTrace()
        {
            ChildTestSteps.Add(new SASingleTrace() { PNAX = this.PNAX, Meas = this.Meas, Channel = this.Channel, IsControlledByParent = true, EnableTraceSettings = true });
        }


        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            List<(string, object)> retVal = new List<(string, object)>();

            return retVal;
        }

        public override void PrePlanRun()
        {
            base.PrePlanRun();

            // Delete dummy trace defined during channel setup
            // DISPlay:MEASure<mnum>:DELete?
            // CALCulate<cnum>:PARameter:DELete[:NAME] <Mname>
            PNAX.ScpiCommand($"CALCulate{Channel}:PARameter:DELete \'CH{Channel}_DUMMY_1\'");
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
