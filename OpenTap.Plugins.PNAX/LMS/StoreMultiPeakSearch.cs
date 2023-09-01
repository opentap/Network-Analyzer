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

namespace OpenTap.Plugins.PNAX.LMS
{
    [Display("StoreMultiPeakSearch", Groups: new[] { "PNA-X", "Load/Measure/Store" }, Description: "Insert a description here")]
    public class StoreMultiPeakSearch : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }


        [Display("Channel", Description: "Choose which channel to grab data from.", "Measurements", Order: 10)]
        public int Channel { get; set; }

        [Display("MNum", Groups: new[] { "Trace" }, Order: 21)]
        public int mnum { get; set; }
        #endregion

        public StoreMultiPeakSearch()
        {

        }

        public override void Run()
        {
            // Execute Multi peak search
            PNAX.MultiPeakSearchExecute(Channel, mnum);

            // Grab all markers and publish

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
