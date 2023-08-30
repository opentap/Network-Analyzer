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
    [Display("SA Single Trace", Groups: new[] { "PNA-X", "General", "Spectrum Analyzer" }, Description: "Insert a description here")]
    public class SASingleTrace : GeneralSingleTraceBaseStep
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
                UpdateTestName();
            }
        }
        #endregion

        public SASingleTrace()
        {
            Trace = "1";
            Meas = SATraceEnum.B;
            //Class = TraceManagerChannelClassEnum.STD;
            Window = 1;
            Sheet = 1;
            Channel = 1;
        }

        protected override void UpdateTestName()
        {
            this.Trace = $"CH{Channel.ToString()}_{Meas}";
            this.Name = $"CH{Channel.ToString()}_{Meas}";
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Trace Format", Groups: new[] { "Trace" }, Order: 30)]
        public override void AddTraceFormat()
        {
            this.ChildTestSteps.Add(new TraceFormat() { Channel = this.Channel });
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Trace Title", Groups: new[] { "Trace" }, Order: 40)]
        public override void AddTraceTitle()
        {
            this.ChildTestSteps.Add(new TraceTitle() { Channel = this.Channel });
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Marker", Groups: new[] { "Trace" }, Order: 50)]
        public override void AddMarker()
        {
            this.ChildTestSteps.Add(new Marker() { Channel = this.Channel, mkr = NextMarker() });
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Trace Limits", Groups: new[] { "Trace" }, Order: 60)]
        public override void AddTraceLimits()
        {
            this.ChildTestSteps.Add(new TraceLimits() { Channel = this.Channel });
        }

        public override void Run()
        {
            int _tnum = 0;
            int _mnum = 0;
            String _MeasName = "";

            PNAX.AddNewTrace(Channel, Window, Trace, "Spectrum Analyzer", Meas.ToString(), ref _tnum, ref _mnum, ref _MeasName);
            tnum = _tnum;
            mnum = _mnum;
            MeasName = _MeasName;

            RunChildSteps(); //If the step supports child steps.


            UpgradeVerdict(Verdict.Pass);
        }
    }
}
