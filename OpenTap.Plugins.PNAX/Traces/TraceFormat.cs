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
    //[AllowAsChildIn(typeof(SingleTraceBaseStep))]
    [Display("TraceFormat", Groups: new[] { "Network Analyzer", "Trace"}, Description: "Insert a description here")]
    public class TraceFormat : PNABaseStep
    {
        #region Settings
        [Display("Format", Groups: new[] { "Trace" }, Order: 11.5)]
        public PNAX.MeasurementFormatEnum Format { get; set; }
        #endregion

        public TraceFormat()
        {
            IsControlledByParent = true;
            Format = PNAX.MeasurementFormatEnum.MLOGarithmic;
        }

        public override void Run()
        {
            //Channel = GetParent<GeneralSingleTraceBaseStep>().Channel;
            int mnum  = GetParent<SingleTraceBaseStep>().mnum;

            PNAX.SetTraceFormat(Channel, mnum, Format);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
