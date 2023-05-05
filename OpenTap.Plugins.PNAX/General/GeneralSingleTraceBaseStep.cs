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
    [Browsable(false)]
    public class GeneralSingleTraceBaseStep : GeneralBaseStep
    {
        [Browsable(false)]
        public bool IsControlledByParentTrace { get; set; } = true;
        private int _Channel;
        [EnabledIf("IsControlledByParentTrace", false, HideIfDisabled = false)]
        [Display("Channel", Order: 1)]
        public override int Channel
        {
            get
            {
                return _Channel;
            }
            set
            {
                _Channel = value;
                UpdateTestName();
            }
        }

        public GeneralSingleTraceBaseStep()
        {
            EnableTraceSettings = true;
        }

        protected override void UpdateTestName()
        {
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.
        }

    }
}
