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
    public enum SATraceEnum
    {
        B,
        A,
        C,
        D,
        R1,
        R2,
        R3,
        R4,
        b1,
        b2,
        b3,
        b4,
        a1,
        a2,
        a3,
        a4
    }


    [AllowAsChildIn(typeof(SANewTrace))]
    [Display("SA Single Trace", Groups: new[] { "Network Analyzer", "General", "Spectrum Analyzer" }, Description: "Insert a description here")]
    public class SASingleTrace : SingleTraceBaseStep
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
                measEnumName = value.ToString();
                UpdateTestStepName();
            }
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Multi Peak Search", Groups: new[] { "Trace" }, Order: 70)]
        public override void AddMultiPeakSearch()
        {
            this.ChildTestSteps.Add(new MultiPeakSearch() { PNAX = this.PNAX, Channel = this.Channel });
        }
        #endregion

        public SASingleTrace()
        {
            Meas = SATraceEnum.B;
            measClass = "Spectrum Analyzer";
        }

        public override void PrePlanRun()
        {
            base.PrePlanRun();

            int _tnum = 0;
            int _mnum = 0;
            string _MeasName = "";

            PNAX.AddNewTrace(Channel, Window, Trace, "Spectrum Analyzer", Meas.ToString(), ref _tnum, ref _mnum, ref _MeasName);
            tnum = _tnum;
            mnum = _mnum;
            MeasName = _MeasName;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.


            UpgradeVerdict(Verdict.Pass);
        }
    }
}
