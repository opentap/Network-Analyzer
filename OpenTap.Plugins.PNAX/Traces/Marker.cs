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

        }

        public override void Run()
        {
            mnum = GetParent<SingleTraceBaseStep>().mnum;
            // CALCulate<cnum>:MEASure<mnum>:MARKer<mkr>:STATe?
            PNAX.ScpiCommand($"CALCulate{Channel.ToString()}:MEASure{mnum.ToString()}:MARKer{mkr.ToString()}:STATe ON");

            PNAX.ScpiCommand($"CALCulate{Channel.ToString()}:MEASure{mnum.ToString()}:MARKer{mkr.ToString()}:X {XAxisPosition.ToString()}");

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
