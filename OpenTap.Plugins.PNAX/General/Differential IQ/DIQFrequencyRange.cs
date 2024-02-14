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
    [AllowAsChildIn(typeof(DIQChannel))]
    [AllowChildrenOfType(typeof(DIQRange))]
    [Display("DIQ Frequency Range", Groups: new[] { "Network Analyzer", "General", "Differential I/Q" }, Description: "Insert a description here")]
    public class DIQFrequencyRange : PNABaseStep
    {
        #region Settings

        #endregion

        [Browsable(true)]
        [Display("Add Frequency Range", Group: "Frequency Range", Order: 40)]
        public void AddRange()
        {
            int childCount = this.ChildTestSteps.Count;
            childCount++;

            DIQRange newRange = new DIQRange { IsControlledByParent = true, Channel = this.Channel, Range = childCount };
            this.ChildTestSteps.Add(newRange);
        }

        public DIQFrequencyRange()
        {
            ChildTestSteps.ChildStepsChanged += childStepsChanged;

            DIQRange newRange = new DIQRange { IsControlledByParent = true, Channel = this.Channel, Range = 1 };
            this.ChildTestSteps.Add(newRange);
        }

        void childStepsChanged(TestStepList sender, TestStepList.ChildStepsChangedAction Action, ITestStep Object, int Index)
        {
            // Count how many childs the Frequency Range step has
            Log.Info($"DIQFrequencyRange has {this.ChildTestSteps.Count}");
            Log.Info($"Action: {Action}");

            // and send this value to the DIQ Channel so it can pass it down to the DIQ Source / DIQ Sources
            UpdateChanelRangeCount();
        }

        protected void UpdateChanelRangeCount()
        {
            try
            {
                var a = GetParent<DIQChannel>();
                if (a != null)
                {
                    a.NumberOfRanges = this.ChildTestSteps.Count;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("can't find parent yet! ex: " + ex.Message);
            }
        }


        public override void Run()
        {
            RunChildSteps();

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
