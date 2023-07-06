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
    [Display("Store SNP", Groups: new[] { "PNA-X", "Acquisition" }, Description: "Store SNP File")]
    public class StoreSnpAdvanced : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display("Channel", Description: "Choose which channel to grab data from.", "Measurements", Order: 10)]
        public Input<int> Channel { get; set; }

        [Display("MNum", Groups: new[] { "Trace" }, Order: 21)]
        public Input<int> mnum { get; set; }

        [Display("Ports", Groups: new[] { "Trace" }, Order: 22)]
        public List<int> Ports { get; set; }

        [Display("File Name", Groups: new[] { "File Name Details" }, Order: 30)]
        public string filename { get; set; }

        [Display("Enable Custom Path", Groups: new[] { "File Name Details" }, Order: 31, Description: "Enable to enter a custom path, Disable to use \\Test Automation\\Results")]
        public bool IsCustomPath { get; set; }

        [EnabledIf("IsCustomPath", true, HideIfDisabled = true)]
        [Display("Custom Path", Groups: new[] { "File Name Details" }, Order: 32)]
        public String CustomPath { get; set; }
        #endregion

        public StoreSnpAdvanced()
        {
            Channel = new Input<int>();
            mnum = new Input<int>();
            Ports = new List<int>() { 1, 2 };
            filename = "CH1";
            IsCustomPath = false;
            CustomPath = @"C:\";
        }

        public override void Run()
        {
            Log.Info("Channel from trace: " + Channel);
            Log.Info("MNUM from trace: " + mnum);

            SingleTraceBaseStep x = (mnum.Step as SingleTraceBaseStep);

            Log.Info("trace Window: ");
            Log.Info("trace Window: " + x.Window);
            Log.Info("trace Sheet: " + x.Sheet);
            Log.Info("trace tnum: " + x.tnum);
            Log.Info("trace mnum: " + x.mnum);
            Log.Info("trace MeasName: " + x.MeasName);

            UpgradeVerdict(Verdict.NotSet);

            string dir = "";
            if (IsCustomPath)
            {
                dir = Path.Combine(CustomPath, filename + ".s2p"); ;
            }
            else
            {
                String assemblyDir = AssemblyDirectory();
                dir = Path.Combine(assemblyDir, "Results", filename + ".s2p");
            }

            PNAX.SaveSnP(Channel.Value, mnum.Value, Ports, dir);

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
