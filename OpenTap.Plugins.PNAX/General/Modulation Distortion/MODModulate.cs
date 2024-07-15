// Author: CMontes
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
    
    //[AllowAsChildIn(typeof(TestPlan))]
    //[AllowAsChildIn(typeof(MODChannel))]
    //[AllowAsChildIn(typeof(MODXChannel))]
    [Display("MOD Modulation", Groups: new[] { "Network Analyzer", "General", "Modulation Distortion" }, Description: "Insert a description here")]
    public class MODModulate : PNABaseStep
    {
        #region Settings

        [Display("Source", Group: "Settings", Order: 10)]
        public String MODSource { get; set; }

        [Display("Modulation File", Group: "Settings", Order: 11)]
        public String ModulationFile { get; set; }

        [Display("Enable Modulation", Group: "Settings", Order: 12)]
        public bool EnableModulation { get; set; }


        #endregion

        public MODModulate()
        {
            MODSource = "Port 1";
            ModulationFile = @"D:\data\d.mdx";
            EnableModulation = false;
        }

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            List<(string, object)> retVal = new List<(string, object)>();
            retVal.Add(("MODSource", MODSource));
            retVal.Add(("ModulationFile", ModulationFile));
            retVal.Add(("EnableModulation", EnableModulation));

            return retVal;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetMODSource(Channel, MODSource);
            PNAX.MODLoadFile(Channel, MODSource, ModulationFile);
            PNAX.MODEnableModulation(Channel, MODSource, EnableModulation);
            PNAX.WaitForOperationComplete();

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
