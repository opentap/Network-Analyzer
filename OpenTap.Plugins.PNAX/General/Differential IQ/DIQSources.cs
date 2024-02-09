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
    [AllowAsChildIn(typeof(DIQChannel))]
    [AllowChildrenOfType(typeof(DIQSource))]
    [Display("DIQ Sources", Groups: new[] { "Network Analyzer", "General", "Differential I/Q" }, Description: "Insert a description here")]
    public class DIQSources : PNABaseStep
    {
        #region Settings
        [Display("Power On (All Channels)", Group: "Power", Order: 20)]
        public bool PowerOnAllChannels { get; set; }
        #endregion

        [Browsable(true)]
        [Display("Add Source", Group: "Sources", Order: 40)]
        public void AddSource()
        {
            DIQSource newSource = new DIQSource { IsControlledByParent = true, Channel = this.Channel };
            this.ChildTestSteps.Add(newSource);
        }

        public DIQSources()
        {
            PowerOnAllChannels = true;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetPowerOnAllChannels(PowerOnAllChannels);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
