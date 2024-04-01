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
    [Display("Store SNP", Groups: new[] { "Network Analyzer", "Load/Measure/Store" }, Description: "Store SNP File")]
    public class StoreSnp : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display("Auto Select All Channels", Group: "Measurements", Order: 10)]
        public bool AutoSelectChannels { get; set; }

        [EnabledIf("AutoSelectChannels", false, HideIfDisabled = true)]
        [Display("Channel", Description: "Choose which channel to grab data from.", "Measurements", Order: 10)]
        public List<int> channels { get; set; }

        //[Display("MNum", Groups: new[] { "Trace" }, Order: 21)]
        //public int mnum { get; set; }

        [Display("Ports", Groups: new[] { "Trace" }, Order: 22)]
        public List<int> Ports { get; set; }

        [Display("File Name", Groups: new[] { "File Name Details" }, Order: 30)]
        public MacroString filename { get; set; }

        [Display("Enable Custom Path", Groups: new[] { "File Name Details" }, Order: 31, Description: "Enable to enter a custom path, Disable to use \\Test Automation\\Results")]
        public bool IsCustomPath { get; set; }

        [EnabledIf("IsCustomPath", true, HideIfDisabled = true)]
        [DirectoryPath]
        [Display("Custom Path", Groups: new[] { "File Name Details" }, Order: 32)]
        public MacroString CustomPath { get; set; }
        #endregion

        public StoreSnp()
        {
            channels = new List<int>() { 1 };
            //mnum = 1;
            Ports = new List<int>() { 1, 2 };
            filename = new MacroString(this) { Text = "<PartId>" };
            IsCustomPath = false;
            CustomPath = new MacroString(this) { Text = @"C:\" };
        }

        public override void Run()
        {
            UpgradeVerdict(Verdict.NotSet);

            if (AutoSelectChannels)
            {
                channels = PNAX.GetActiveChannels();
            }

            foreach (int channel in channels)
            {
                Log.Info("Storing SNP for Channel : " + channel);

                // Get all measurements for current channel
                List<int> measurements = PNAX.GetChannelMeasurements(channel);

                Log.Info("MNUMs for channel: " + string.Join<int>(",", measurements));

                string dir = "";
                // Port Count to Update file extension s<n>p
                int PortCount = Ports.Count;

                MacroString macroString = new MacroString(this) { Text = filename.Text + "_CH" + channel };
                if (IsCustomPath)
                {
                    dir = Path.Combine(CustomPath.Expand(PlanRun), macroString.Expand(PlanRun) + $".s{PortCount}p"); ;
                }
                else
                {
                    string assemblyDir = AssemblyDirectory();
                    dir = Path.Combine(assemblyDir, "Results", macroString.Expand(PlanRun) + $".s{PortCount}p");
                }

                // Saving to file:
                Log.Info("Storing SNP to file: " + dir);

                PNAX.SaveSnP(channel, measurements[0], Ports, dir);
            }

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
