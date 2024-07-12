// Author: CMontes
// Copyright:   Copyright 2023-2024 Keysight Technologies
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
    //[AllowAsChildIn(typeof(DIQChannel))]
    //[AllowChildrenOfType(typeof(DIQSource))]
    [Display("DIQ Sources", Groups: new[] { "Network Analyzer", "General", "Differential I/Q" }, Description: "Insert a description here")]
    public class DIQSources : PNABaseStep
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

        [Display("Power On (All Channels)", Group: "Power", Order: 20)]
        public bool PowerOnAllChannels { get; set; }
        #endregion

        [Browsable(true)]
        [Display("Add Source", Group: "Sources", Order: 40)]
        public void AddSource()
        {
            DIQSource newSource = new DIQSource { IsControlledByParent = true, Channel = this.Channel , SourceName = "Source" };
            this.ChildTestSteps.Add(newSource);
        }

        private void UpdateChildStepRanges()
        {
            foreach (var step in this.ChildTestSteps)
            {
                if (step.GetType().Equals(typeof(DIQSource)))
                {
                    (step as DIQSource).NumberOfRanges = _NumberOfRanges;
                }
            }
        }

        public DIQSources()
        {
            PowerOnAllChannels = true;

            // Add Default Sources
            DIQSource newSource = new DIQSource { IsControlledByParent = true, Channel = this.Channel, SourceName = "Port 1" };
            newSource.SourceState = DIQPortStateEnumtype.Auto;
            this.ChildTestSteps.Add(newSource);
            newSource = new DIQSource { IsControlledByParent = true, Channel = this.Channel, SourceName = "Port 2" };
            newSource.SourceState = DIQPortStateEnumtype.Off;
            newSource.ReferencedTo = "Port 4";
            newSource.rCont = "a2";
            newSource.rRef = "a4";
            newSource.TRec = "b2";
            newSource.RRec = "a2";
            this.ChildTestSteps.Add(newSource);
            newSource = new DIQSource { IsControlledByParent = true, Channel = this.Channel, SourceName = "Port 3" };
            newSource.SourceState = DIQPortStateEnumtype.Off;
            newSource.ReferencedTo = "Port 1";
            newSource.rCont = "a3";
            newSource.rRef = "a1";
            newSource.TRec = "b3";
            newSource.RRec = "a3";
            this.ChildTestSteps.Add(newSource);
            newSource = new DIQSource { IsControlledByParent = true, Channel = this.Channel, SourceName = "Port 4" };
            newSource.SourceState = DIQPortStateEnumtype.Off;
            newSource.ReferencedTo = "Port 2";
            newSource.rCont = "a4";
            newSource.rRef = "a2";
            newSource.TRec = "b4";
            newSource.RRec = "a4";
            this.ChildTestSteps.Add(newSource);
            newSource = new DIQSource { IsControlledByParent = true, Channel = this.Channel, SourceName = "Port 1 Src2" };
            newSource.SourceState = DIQPortStateEnumtype.Off;
            newSource.ReferencedTo = "Source3";
            newSource.rCont = "a1";
            newSource.rRef = "a3";
            newSource.TRec = "b1";
            newSource.RRec = "a1";
            this.ChildTestSteps.Add(newSource);
            newSource = new DIQSource { IsControlledByParent = true, Channel = this.Channel, SourceName = "Source3" };
            newSource.SourceState = DIQPortStateEnumtype.Off;
            newSource.ReferencedTo = "Port 4";
            newSource.rCont = "a1";
            newSource.rRef = "a4";
            newSource.TRec = "b4";
            newSource.RRec = "a4";
            this.ChildTestSteps.Add(newSource);

        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetPowerOnAllChannels(PowerOnAllChannels);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
