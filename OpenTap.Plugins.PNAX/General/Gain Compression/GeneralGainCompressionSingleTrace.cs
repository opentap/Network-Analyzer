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
    public enum GeneralGainCompressionTraceEnum
    {
        S21,
        S11,
        S12,
        S22,
        AI1,
        AI2,
        CompIn21,
        CompOut21,
        DeltaGain21,
        CompGain21,
        CompS11,
        RefS21,
        CompAI1,
        CompAI2,
    }

    [AllowAsChildIn(typeof(GeneralGainCompressionChannel))]
    [AllowAsChildIn(typeof(GeneralGainCompressionNewTrace))]
    [Display("Compression Single Trace", Groups: new[] { "PNA-X", "General", "Gain Compression" }, Description: "Insert a description here")]
    public class GeneralGainCompressionSingleTrace : GeneralSingleTraceBaseStep
    {
        #region Settings

        private GeneralGainCompressionTraceEnum _Meas;
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public GeneralGainCompressionTraceEnum Meas
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

        public GeneralGainCompressionSingleTrace()
        {
            Trace = "1";
            Meas = GeneralGainCompressionTraceEnum.CompIn21;
            Channel = 1;
            Window = 1;
            Sheet = 1;
        }

        protected override void UpdateTestName()
        {
            this.Trace = $"CH{Channel}_{Meas}";
            this.Name = $"CH{Channel}_{Meas}";
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

        public override void Run()
        {
            int _tnum = 0;
            int _mnum = 0;
            PNAX.AddNewTrace(Channel, Window, Trace, "Gain Compression", Meas.ToString(), ref _tnum, ref _mnum);
            tnum = _tnum;
            mnum = _mnum;

            RunChildSteps(); //If the step supports child steps.
            
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
