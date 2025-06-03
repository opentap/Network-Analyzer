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
    [Display("User Preset Save File", Groups: new[] { "Network Analyzer" }, Description: "Saves the current instrument settings as UserPreset.sta.  Subsequent execution of SYSTem:UPReset will cause the VNA to assume this instrument state." +
        "Regardless of the state of the User Preset Enable checkbox, the SYST:PRESet command will always preset the VNA to the factory preset settings, and SYST:UPReset will always perform a User Preset.")]
    public class UserPresetSaveFile : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }
        #endregion

        public UserPresetSaveFile()
        {
        }

        public override void Run()
        {
            PNAX.UserPresetSaveState();

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
