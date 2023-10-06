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

namespace OpenTap.Plugins.PNAX.General.Spectrum_Analyzer
{
    [AllowAsChildIn(typeof(SpectrumAnalyzerChannel))]
    [AllowChildrenOfType(typeof(SASingleTrace))]
    [Display("SA New Trace", Groups: new[] { "PNA-X", "General", "Spectrum Analyzer" }, Description: "Insert a description here")]
    public class SANewTrace : GeneralNewTraceBaseStep
    {
        #region Settings
        private SATraceEnum _Meas;
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public SATraceEnum Meas
        {
            get
            {
                return _Meas;
            }
            set
            {
                _Meas = value;
            }
        }
        #endregion

        public SANewTrace()
        {
            ChildTestSteps.Add(new SASingleTrace() { PNAX = this.PNAX, Meas = SATraceEnum.B });
        }

        public override void PrePlanRun()
        {
            base.PrePlanRun();

            // Delete dummy trace defined during channel setup
            // DISPlay:MEASure<mnum>:DELete?
            // CALCulate<cnum>:PARameter:DELete[:NAME] <Mname>
            PNAX.ScpiCommand($"CALCulate{Channel.ToString()}:PARameter:DELete \'CH{Channel.ToString()}_DUMMY_1\'");
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }

        protected override void AddNewTrace()
        {
            SATraceEnum saTrace;
            if (Enum.TryParse<SATraceEnum>(Meas.ToString(), out saTrace))
            {
                this.ChildTestSteps.Add(new SASingleTrace() { PNAX = this.PNAX, Meas = saTrace, Channel = this.Channel });
            }
        }

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            List<(String, object)> retVal = new List<(string, object)>();

            return retVal;
        }


    }
}
