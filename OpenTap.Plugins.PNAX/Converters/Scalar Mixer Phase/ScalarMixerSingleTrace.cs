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
    public enum SMCTraceEnum
    {
        SC21,
        SC12,
        S11,
        S22,
        IPwr,
        OPwr,
        RevIPwr,
        RevOPwr,
        AI1_1,
        AI2_1,
        AI1_2,
        AI2_2,
    }


    [AllowAsChildIn(typeof(ScalarMixerChannel))]
    [AllowAsChildIn(typeof(ScalarMixerNewTrace))]
    [Display("Scalar Mixer Single Trace", Groups: new[] { "PNA-X", "Converters", "Scalar Mixer Converter + Phase" }, Description: "Insert a description here")]
    public class ScalarMixerSingleTrace : ConverterSingleTraceBaseStep
    {
        #region Settings
        private SMCTraceEnum _Meas;
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public SMCTraceEnum Meas
        {
            get { return _Meas; }
            set
            {
                _Meas = value;
                UpdateTestName();
            }
        }
        #endregion

        public ScalarMixerSingleTrace()
        {
            Trace = "1";
            Meas = SMCTraceEnum.SC21;
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
            this.ChildTestSteps.Add(new TraceFormat() { PNAX = this.PNAX, Channel = this.Channel });
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Trace Title", Groups: new[] { "Trace" }, Order: 40)]
        public override void AddTraceTitle()
        {
            this.ChildTestSteps.Add(new TraceTitle() { PNAX = this.PNAX, Channel = this.Channel });
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Marker", Groups: new[] { "Trace" }, Order: 50)]
        public override void AddMarker()
        {
            this.ChildTestSteps.Add(new Marker() { PNAX = this.PNAX, Channel = this.Channel, mkr = NextMarker() });
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Trace Limits", Groups: new[] { "Trace" }, Order: 60)]
        public override void AddTraceLimits()
        {
            this.ChildTestSteps.Add(new TraceLimits() { PNAX = this.PNAX, Channel = this.Channel });
        }

        public override void Run()
        {
            int _tnum = 0;
            int _mnum = 0;
            String _MeasName = "";
            PNAX.AddNewTrace(Channel, Window, Trace, "Scalar Mixer/Converter", Meas.ToString(), ref _tnum, ref _mnum, ref _MeasName);
            tnum = _tnum;
            mnum = _mnum;
            MeasName = _MeasName;
            RunChildSteps(); //If the step supports child steps.
            
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
