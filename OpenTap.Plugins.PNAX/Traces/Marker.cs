// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using OpenTap;
using OpenTap.Plugins.PNAX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OpenTap.Plugins.PNAX
{
    [AllowAsChildIn(typeof(SingleTraceBaseStep))]
    [Display("Marker", Groups: new[] { "PNA-X", "Trace" }, Description: "Insert a description here")]
    public class Marker : GeneralBaseStep
    {
        #region Settings
        [Display("Marker Number", Groups: new[] { "Trace" }, Order: 10)]
        public int mkr { get; set; }

        [Display("X-Axis Position", Groups: new[] { "Trace" }, Order: 11)]
        public double XAxisPosition { get; set; }
        #endregion

        public Marker()
        {
            IsControlledByParent = true;
        }

        public override void Run()
        {
            mnum = GetParent<SingleTraceBaseStep>().mnum;

            PNAX.SetMarkerState(Channel, mnum, mkr, SAOnOffTypeEnum.On);

            PNAX.SetMarkerXValue(Channel, mnum, mkr, XAxisPosition);

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
