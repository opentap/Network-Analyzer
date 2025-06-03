// Author: CMontes
// Copyright:   Copyright 2024 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using OpenTap;

namespace OpenTap.Plugins.PNAX
{
    //[AllowAsChildIn(typeof(TestPlan))]
    //[AllowAsChildIn(typeof(MODChannel))]
    [Display(
        "MOD Save Distortion Table",
        Groups: new[] { "Network Analyzer", "General", "Modulation Distortion" },
        Description: "Specifies the file path to save a modulation distortion table file"
    )]
    public class MODSaveDistortionTable : PNABaseStep
    {
        #region Settings
        [Display("Modulation Distortion File", Groups: new[] { "File Name Details" }, Order: 30)]
        public MacroString ModulationDistortionFile { get; set; }

        [Display(
            "Enable Custom Path",
            Groups: new[] { "File Name Details" },
            Order: 31,
            Description: "Enable to enter a custom path, Disable to use \\Test Automation\\Results"
        )]
        public bool IsCustomPath { get; set; }

        [EnabledIf("IsCustomPath", true, HideIfDisabled = true)]
        [DirectoryPath]
        [Display("Custom Path", Groups: new[] { "File Name Details" }, Order: 32)]
        public MacroString CustomPath { get; set; }
        #endregion

        public MODSaveDistortionTable()
        {
            ModulationDistortionFile = new MacroString(this) { Text = "myModDistortionTable" };
            IsCustomPath = false;
            CustomPath = new MacroString(this) { Text = @"C:\" };
        }

        public override void Run()
        {
            UpgradeVerdict(Verdict.NotSet);

            string dir = "";
            if (IsCustomPath)
            {
                dir = Path.Combine(
                    CustomPath.Expand(PlanRun),
                    ModulationDistortionFile.Expand(PlanRun) + ".csv"
                );
                ;
            }
            else
            {
                String assemblyDir = AssemblyDirectory();
                dir = Path.Combine(
                    assemblyDir,
                    "Results",
                    ModulationDistortionFile.Expand(PlanRun) + ".csv"
                );
            }

            PNAX.MODSaveDistortionTable(Channel, dir);

            UpgradeVerdict(Verdict.Pass);
        }

        public string AssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}
