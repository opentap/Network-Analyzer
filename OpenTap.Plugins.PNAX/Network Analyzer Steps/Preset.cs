// Author: CMontes
// Copyright:   Copyright 2023-2024 Keysight Technologies
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

namespace OpenTap.Plugins.PNAX
{
    [Display(
        "Preset",
        Groups: new[] { "Network Analyzer" },
        Description: "Sends command SYSTem:FPReset which performs a standard Preset, then deletes the default trace, measurement, and window. The VNA screen becomes blank"
    )]
    public class Preset : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }
        #endregion

        public Preset() { }

        public override void Run()
        {
            PNAX.Preset();

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
