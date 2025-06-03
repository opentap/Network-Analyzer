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

namespace OpenTap.Plugins.PNAX
{
    [Display(
        "Recall State",
        Groups: new[] { "Network Analyzer", "Load/Measure/Store" },
        Description: "Recall State File From Instrument"
    )]
    public class RecalState : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display(
            "State Filename",
            "Specfiy name of csa to be loaded, file must be on instrument",
            "Load File",
            Order: 10
        )]
        public string StateFile { get; set; }
        #endregion

        public RecalState()
        {
            StateFile = "";
            Rules.Add(() => ((StateFile.Equals("") == false)), "Must be a valid file", "StateFile");
        }

        public override void Run()
        {
            PNAX.LoadState(StateFile, false);
            PNAX.WaitForOperationComplete();

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
