// Author: MyName
// Copyright:   Copyright 2024 Keysight Technologies
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

namespace OpenTap.Plugins.PNAX.Calibration
{
    [Display("Select Calset", Groups: new[] { "Network Analyzer", "Calibration" }, Description: "Selects and applies a Cal Set to the specified channel.")]
    public class SelectCalset : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display("Channel", Order: 0.11)]
        public int Channel { get; set; }

        [Display("Cal Set Name", Order: 0.2)]
        public String CalSetName { get; set; }
        #endregion

        public SelectCalset()
        {
            Channel = 1;
            CalSetName = "MyCalSet";
        }

        public override void Run()
        {
            PNAX.LoadCalset(Channel, CalSetName, true);
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
