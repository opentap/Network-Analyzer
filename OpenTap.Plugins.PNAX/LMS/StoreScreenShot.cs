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
    [Display("Store Screen Shot", Groups: new[] { "Network Analyzer", "Load/Measure/Store" }, Description: "Store screen shot")]
    public class StoreScreenShot : TestStep
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

        [Display("Force screen to front", "Ensures the app screen is on top", "Advanced", 1, true)]
        public bool ForceToFront { get; set; }

        [EnabledIf("ForceToFront", true, HideIfDisabled = true)]
        [Display("Delay before screenshot", "Milliseconds to delay before taking the screenshot", "Advanced", 2, true)]
        public int DelayBeforeScreenshot { get; set; }
        
        #endregion

        public StoreScreenShot()
        {
            filename = new MacroString(this) { Text = "Screen_1" };
            IsCustomPath = false;
            CustomPath = new MacroString(this) { Text = @"C:\" };
            ForceToFront = false;
            DelayBeforeScreenshot = 0;
        }

        public override void Run()
        {
            UpgradeVerdict(Verdict.NotSet);

            string dir;
            if (IsCustomPath)
            {
                dir = Path.Combine(CustomPath.Expand(PlanRun), filename.Expand(PlanRun) + ".bmp"); ;
            }
            else
            {
                string assemblyDir = AssemblyDirectory();
                dir = Path.Combine(assemblyDir, "Results", filename.Expand(PlanRun) + ".bmp");
            }

            PNAX.SaveScreen(dir, ForceToFront, DelayBeforeScreenshot);

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
