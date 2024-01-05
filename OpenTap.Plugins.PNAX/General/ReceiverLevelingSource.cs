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

namespace OpenTap.Plugins.PNAX.General
{
    [AllowAsChildIn(typeof(ReceiverLeveling))]
    [Display("Controlled Source", Groups: new[] { "Network Analyzer", "General" },
        Description: "Controlled Source\nCan be added as a child to the following Channels:\n\tReceiver Leveling")]
    public class ReceiverLevelingSource : PNABaseStep
    {
        #region Settings
        [Display("Controlled Source", Group: "Controlled Source Properties", Order: 20)]
        public string ControlledSource
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }

        [Display("Leveling Receiver", Group: "Controlled Source Properties", Order: 22)]
        public string LevelingReceiver { get; set; }
        #endregion

        public ReceiverLevelingSource()
        {

        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            UpgradeVerdict(Verdict.Pass);
        }

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            List<(string, object)> retVal = new List<(string, object)>();

            retVal.Add(($"{ControlledSource} LevelingReceiver", LevelingReceiver));

            return retVal;
        }

    }
}
