﻿// Author: MyName
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
    [Display("Store SNP", Groups: new[] { "PNA-X", "Load/Measure/Store" }, Description: "Store SNP File")]
    public class StoreSnp : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display("Channel", Description: "Choose which channel to grab data from.", "Measurements", Order: 10)]
        public int Channel { get; set; }

        [Display("MNum", Groups: new[] { "Trace" }, Order: 21)]
        public int mnum { get; set; }

        [Display("Ports", Groups: new[] { "Trace" }, Order: 22)]
        public List<int> Ports { get; set; }

        public string filename { get; set; }
        #endregion

        public StoreSnp()
        {
            Channel = 1;
            mnum = 1;
            Ports = new List<int>() { 1, 2 };
            filename = "CH1";
        }

        public override void Run()
        {
            Log.Info("Channel from trace: " + Channel);
            Log.Info("MNUM from trace: " + mnum);

            UpgradeVerdict(Verdict.NotSet);

            string dir = AssemblyDirectory();

            PNAX.SaveSnP(Channel, mnum, Ports, dir + "\\Results\\" + filename + ".s2p");  //"yyyyMMdd_HHmmssfff_") + SpecName + ".s2p");

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
