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
    [Display("User Preset Load File", Groups: new[] { "Network Analyzer" }, Description: "Loads an existing instrument state file (.sta or .cst) to be used for User Preset. Subsequent execution of SYSTem:UPReset will cause the VNA to assume this instrument state." +
        "Regardless of the state of the User Preset Enable checkbox, the SYST:PRESet command will always preset the VNA to the factory preset settings, and SYST:UPReset will always perform a User Preset.")]
    public class UserPresetLoadFile : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display("User Preset State File", "Specfiy an existing instrument state file (.sta or .cst) to be loaded", "Load File", Order: 10)]
        [FilePath(FilePathAttribute.BehaviorChoice.Open, "cst")]
        public string file { get; set; }

        #endregion

        public UserPresetLoadFile()
        {
            file = "UserPreset.cst";
        }

        public override void Run()
        {
            PNAX.UserPresetLoadFile(file);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
