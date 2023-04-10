﻿// Author: MyName
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
    public class GeneralBaseStep : TestStep
    {
        #region Settings
        [Browsable(false)]
        public bool IsControlledByParent { get; set; } = false;
        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        private int _Channel;
        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
        [Display("Channel", Order: 1)]
        public virtual int Channel
        {
            get
            {
                return _Channel;
            }
            set
            {
                _Channel = value;

                // Update traces
                foreach (var a in this.ChildTestSteps)
                {
                    if (a is GeneralSingleTraceBaseStep)
                    {
                        (a as GeneralSingleTraceBaseStep).Channel = value;
                    }
                }
            }
        }

        #endregion


        public GeneralBaseStep()
        {
        }

        public override void Run()
        {
        }
    }
}
