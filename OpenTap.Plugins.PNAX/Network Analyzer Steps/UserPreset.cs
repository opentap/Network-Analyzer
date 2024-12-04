// Author: CMontes
// Copyright:   Copyright 2023-2024 Keysight Technologies
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
    [Display("User Preset", Groups: new[] { "Network Analyzer" }, Description: "Performs a User Preset. There must be an active User Preset state file (see Load and Save) or an error will be returned" + 
        ".Regardless of the state of the User Preset Enable checkbox, the SYST:PRESet command will always preset the VNA to the factory preset settings, and SYST: UPReset will always perform a User Preset.")]
    public class UserPreset : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }
        #endregion

        public UserPreset()
        {
        }

        public override void Run()
        {
            PNAX.UserPreset();

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
