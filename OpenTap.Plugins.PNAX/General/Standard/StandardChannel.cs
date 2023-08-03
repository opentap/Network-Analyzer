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
using System.Linq;
using System.Text;

namespace OpenTap.Plugins.PNAX
{
    [Display("Standard Channel", Groups: new[] { "PNA-X", "General",  "Standard" }, Description: "Insert a description here")]
    public class StandardChannel : GeneralChannelBaseStep
    {
        #region Settings
        [Output]
        [Browsable(false)]
        public List<(string, string)> MetaData { get; private set; }

        [Browsable(false)]
        public void UpdateMetaData()
        {
            MetaData = new List<(string, string)>();

            MetaData.Add(("Channel", this.Channel.ToString()));

            foreach(var ch in this.ChildTestSteps)
            {
                List<(string, string)> ret = (ch as GeneralBaseStep).GetMetaData();
                foreach(var it in ret)
                {
                    MetaData.Add(it);
                }
            }
            //MetaData.Add(("", ));
        }

        #endregion

        public StandardChannel()
        {
            MetaData = new List<(string, string)>();
            Channel = 1;

            // Sweep Type
            SweepType sweepType = new SweepType { IsControlledByParent = true, Channel = this.Channel };
            // Timing
            Timing timing = new Timing { IsControlledByParent = true, Channel = this.Channel };
            // Traces
            StandardNewTrace standardNewTrace = new StandardNewTrace { IsControlledByParent = true, Channel = this.Channel };

            this.ChildTestSteps.Add(sweepType);
            this.ChildTestSteps.Add(timing);
            this.ChildTestSteps.Add(standardNewTrace);
        }

        public override void Run()
        {
            UpdateMetaData();

            int traceid = PNAX.GetNewTraceID(Channel);
            // Define a dummy measurement so we can setup all channel parameters
            // we will add the traces during the StandardSingleTrace or StandardNewTrace test steps
            PNAX.ScpiCommand($"CALCulate{Channel.ToString()}:CUST:DEFine \'CH{Channel.ToString()}_DUMMY_1\',\'Standard\',\'S11\'");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
