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
    [Display("Preset", Groups: new[] { "PNA-X" }, Description: "Insert a description here")]
    public class Preset : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }
        #endregion

        public Preset()
        {
        }

        public override void Run()
        {
            PNAX.Preset();

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
