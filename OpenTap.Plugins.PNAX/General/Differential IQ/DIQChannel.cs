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
    [Display("Differential I/Q Channel", Groups: new[] { "Network Analyzer", "General",  "Differential I/Q" }, Description: "Insert a description here")]
    public class DIQChannel : PNABaseStep
    {
        #region Settings
        [Browsable(false)]
        public bool IsRangesVisible { get; set; } = false;

        private int _NumberOfRanges;
        [EnabledIf("IsRangesVisible", true, HideIfDisabled = true)]
        public int NumberOfRanges
        {
            set
            {
                _NumberOfRanges = value;
                UpdateChildStepRanges();
            }
            get
            {
                return _NumberOfRanges;
            }
        }

        #endregion

        private void UpdateChildStepRanges()
        {
            foreach (var step in this.ChildTestSteps)
            {
                if (step.GetType().Equals(typeof(DIQSources)))
                {
                    (step as DIQSources).NumberOfRanges = _NumberOfRanges;
                }
            }
        }

        public DIQChannel()
        {
            IsControlledByParent = false;
            Channel = 1;
            NumberOfRanges = 1;

            // Traces
            DIQNewTrace standardNewTrace = new DIQNewTrace { IsControlledByParent = true, Channel = this.Channel };
            DIQFrequencyRange freqRange = new DIQFrequencyRange { IsControlledByParent = true, Channel = this.Channel };
            DIQSources sources = new DIQSources { IsControlledByParent = true, Channel = this.Channel };

            this.ChildTestSteps.Add(standardNewTrace);
            this.ChildTestSteps.Add(freqRange);
            this.ChildTestSteps.Add(sources);
        }


        public override void Run()
        {
            PNAX.GetNewTraceID(Channel);
            // Define a dummy measurement so we can setup all channel parameters
            // we will add the traces during the StandardSingleTrace or StandardNewTrace test steps
            PNAX.ScpiCommand($"CALCulate{Channel}:CUST:DEFine \'CH{Channel}_DUMMY_1\',\'Differential I/Q\',\'IPwrF1\'");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
            //UpdateMetaData();
        }
    }
}
