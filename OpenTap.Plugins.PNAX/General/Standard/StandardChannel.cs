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
    public class StandardChannel : TestStep
    {
        #region Settings
        private PNAX _pNAX;
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX 
        { 
            get { return _pNAX; }
            set
            {
                if (value != _pNAX)
                {
                    _pNAX = value;
                    foreach (var step in this.ChildTestSteps)
                    {
                        var childType = ((GeneralStandardChannelBaseStep)(step));
                        childType.PNAX = value;
                    }
                }
            } 
        }

        private int _Channel;
        [Display("Channel", Order: 1)]
        public int Channel
        {
            get
            {
                return _Channel;
            }
            set
            {
                _Channel = value;
            }
        }

        #endregion

        public StandardChannel()
        {
            Channel = 1;

            // Sweep Type
            SweepType sweepType = new SweepType { isControlledByParent = true };
            // Timing
            Timing timing = new Timing { isControlledByParent = true };
            // Traces
            StandardNewTrace standardNewTrace = new StandardNewTrace { isControlledByParent = true };

            this.ChildTestSteps.Add(sweepType);
            this.ChildTestSteps.Add(timing);
            this.ChildTestSteps.Add(standardNewTrace);
        }

        public override void Run()
        {
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
