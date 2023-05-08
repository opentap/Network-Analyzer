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
    [Display("TraceTitle", Groups: new[] { "PNA-X", "Trace" }, Description: "Insert a description here")]
    public class TraceTitle : GeneralBaseStep
    {
        #region Settings
        [Display("Trace Title", Groups: new[] { "Trace" }, Order: 9)]
        public String Title { get; set; }

        #endregion

        public TraceTitle()
        {
            IsControlledByParent = true;
        }

        public override void Run()
        {
            Window = GetParent<SingleTraceBaseStep>().Window;
            tnum = GetParent<SingleTraceBaseStep>().tnum;

            if (Title != "")
            {
                PNAX.SetTraceTitle(Window, tnum, Title);
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
