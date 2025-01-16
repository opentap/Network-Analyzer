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
    [Display("System Preset", Groups: new[] { "Network Analyzer" }, Description: "Deletes all traces, measurements, and windows. In addition, resets the analyzer to factory defined default settings and creates a S11 measurement named \"CH1_S11_1\". For a list of default settings, see Preset." +
        "Regardless of the state of the User Preset Enable checkbox, the SYST:PRESet command will always preset the VNA to the factory preset settings, and SYST:UPReset will always perform a User Preset." +
        "If the VNA display is disabled with DISP:ENAB OFF then SYST:PRES will NOT enable the display." +
        "This command performs the same function as *RST with one exception: Syst:Preset does NOT reset Calc:FORMAT to ASCII as does *RST.")]
    public class SystemPreset : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }
        #endregion

        public SystemPreset()
        {
        }

        public override void Run()
        {
            PNAX.SystemPreset();

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
