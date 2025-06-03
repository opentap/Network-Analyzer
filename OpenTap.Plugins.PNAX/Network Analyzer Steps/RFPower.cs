// Author: CMontes
// Copyright:   Copyright 2024 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using OpenTap;

namespace OpenTap.Plugins.PNAX.Network_Analyzer_Steps
{
    [Display("RF Power", Groups: new[] { "Network Analyzer" }, Description: "Set Power On/Off")]
    public class RFPower : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display("Power On (All Channels)", Order: 10)]
        public bool PowerOnAllChannels { get; set; }
        #endregion

        public RFPower()
        {
            PowerOnAllChannels = true;
        }

        public override void Run()
        {
            PNAX.SetPowerOnAllChannels(PowerOnAllChannels);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
