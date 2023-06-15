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
    [Display("Store Screen Shot", Groups: new[] { "PNA-X", "Load/Measure/Store" }, Description: "Store screen shot")]
    public class StoreScreenShot : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        public string filename { get; set; }
        #endregion

        public StoreScreenShot()
        {
            filename = "Screen_1";
        }

        public override void Run()
        {
            UpgradeVerdict(Verdict.NotSet);

            string dir = AssemblyDirectory();

            PNAX.SaveScreen(dir + "\\Results\\" + filename +  ".bmp");// "yyyyMMdd_HHmmssfff_") + SpecName + ".bmp");

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
