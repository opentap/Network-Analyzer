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

namespace OpenTap.Plugins.PNAX
{
    [AllowAsChildIn(typeof(TestPlan))]
    [AllowAsChildIn(typeof(MODChannel))]
    [Display("MOD Save Distortion Table", Groups: new[] { "Network Analyzer", "General", "Modulation Distortion" }, Description: "Specifies the file path to save a modulation distortion table file")]
    public class MODSaveDistortionTable : PNABaseStep
    {
        #region Settings
        [Display("Modulation Distortion File", Group: "Settings", Order: 11)]
        public String ModulationDistortionFile { get; set; }

        #endregion

        public MODSaveDistortionTable()
        {
            ModulationDistortionFile = "myModDistortionTable.csv";
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.MODSaveDistortionTable(Channel, ModulationDistortionFile);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
