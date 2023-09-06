﻿// Author: MyName
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
    [Display("Multi Peak Search", Groups: new[] { "PNA-X", "Trace" }, Description: "Insert a description here")]
    public class MultiPeakSearch : GeneralBaseStep
    {
        #region Settings
        [Display("Marker Number", Groups: new[] { "Trace" }, Order: 10)]
        public int mkr { get; set; }

        [Display("Peak Threshold", Groups: new[] { "Trace" }, Order: 11)]
        public double PeakThreshold { get; set; }

        [Display("Peak Excursion", Groups: new[] { "Trace" }, Order: 12)]
        public double PeakExcursion{ get; set; }

        [Display("Peak Polarity", Groups: new[] { "Trace" }, Order: 13)]
        public SAMultiPeakSearchPolarityEnumType PeakPolarity { get; set; }
        #endregion

        public MultiPeakSearch()
        {
            IsControlledByParent = true;
            PeakThreshold = 0.003;
            PeakExcursion = 0.001;
            PeakPolarity = SAMultiPeakSearchPolarityEnumType.POS;
        }

        public override void Run()
        {
            mnum = GetParent<SingleTraceBaseStep>().mnum;

            PNAX.SetMultiPeakSearchThreshold(Channel, mnum, PeakThreshold);
            PNAX.SetMultiPeakSearchExcursion(Channel, mnum, PeakExcursion);
            PNAX.SetMultiPeakSearchPolarity(Channel, mnum, PeakPolarity);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
