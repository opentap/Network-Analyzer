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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenTap.Plugins.PNAX.LMS
{
    [Display("Store Displayed Trace Data", Groups: new[] { "Network Analyzer", "Load/Measure/Store" }, Description: "Store all trace data on screen to CSV")]
    public class StoreDispTraceData : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display("File Name", Groups: new[] { "File Name Details" }, Order: 30)]
        public MacroString filename { get; set; }

        [Display("Enable Custom Path", Groups: new[] { "File Name Details" }, Order: 31, Description: "Enable to enter a custom path, Disable to use \\Test Automation\\Results")]
        public bool IsCustomPath { get; set; }

        [EnabledIf("IsCustomPath", true, HideIfDisabled = true)]
        [DirectoryPath]
        [Display("Custom Path", Groups: new[] { "File Name Details" }, Order: 32)]
        public MacroString CustomPath { get; set; }
        #endregion

        public StoreDispTraceData()
        {
            filename = new MacroString(this) { Text = "All_Channels" };
            IsCustomPath = false;
            CustomPath = new MacroString(this) { Text = @"C:\" };
        }

        public override void Run()
        {
            UpgradeVerdict(Verdict.NotSet);

            string dir = "";
            if (IsCustomPath)
            {
                dir = Path.Combine(CustomPath.Expand(PlanRun), filename.Expand(PlanRun) + ".csv"); ;
            }
            else
            {
                String assemblyDir = AssemblyDirectory();
                dir = Path.Combine(assemblyDir, "Results", filename.Expand(PlanRun) + ".csv");
            }

            PNAX.SaveDispState(dir);

            // Supported child steps will provide MetaData to be added to the publish table
            RunChildSteps();

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
