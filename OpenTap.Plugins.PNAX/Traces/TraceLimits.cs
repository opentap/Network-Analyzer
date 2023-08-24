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
    [AllowAsChildIn(typeof(SingleTraceBaseStep))]
    [Display("Trace Limits", Groups: new[] { "PNA-X", "Trace" }, Description: "Set Limits for a trace")]
    public class TraceLimits : GeneralBaseStep
    {
        #region Settings
        [Display("Limit Test ON", Group: "Limit Test", Order: 1)]
        public bool LimitTest { get; set; }

        [Display("Limit Line ON", Group: "Limit Test", Order: 2)]
        public bool LimitLine { get; set; }

        [Display("Sound ON Fail", Group: "Limit Test", Order: 3)]
        public bool SoundOn { get; set; }

        [Display("X", Group: "Pass/Fail Position", Order: 10)]
        public double PositionX { get; set; }

        [Display("Y", Group: "Pass/Fail Position", Order: 11)]
        public double PositionY { get; set; }

        [Display("Limit Table", Group: "Limit Table", Order: 20)]
        public List<LimitSegmentDefinition> limitSegments { get; set; }

        [Display("Global Pass Fail", Group: "Global Pass/Fail ON", Order: 30)]
        public bool GlobalPassFail { get; set; }

        #endregion

        public TraceLimits()
        {
            IsControlledByParent = true;

            LimitTest = false;
            LimitLine = true;
            SoundOn = false;

            PositionX = 7.0;
            PositionY = 0.0;

            limitSegments = new List<LimitSegmentDefinition>();
            limitSegments.Add(new LimitSegmentDefinition { LimitType = LimitType.Off, BeginStim = 0, EndStim = 0, BeginResp = 0, EndResp = 0});

            GlobalPassFail = false;
        }

        public override void Run()
        {
            mnum = GetParent<SingleTraceBaseStep>().mnum;
            Window = GetParent<SingleTraceBaseStep>().Window;

            PNAX.SetLimitTestOn(Channel, mnum, LimitTest);
            PNAX.SetLimitLineOn(Channel, mnum, LimitLine);
            PNAX.SetLimitTestFailOn(Channel, mnum, SoundOn);

            PNAX.SetXPosition(Window, PositionX);
            PNAX.SetYPosition(Window, PositionY);

            PNAX.SetLimitData(Channel, mnum, limitSegments);

            PNAX.SetGlobalPF(GlobalPassFail);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
