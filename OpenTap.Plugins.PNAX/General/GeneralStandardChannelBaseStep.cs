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
    public class GeneralStandardChannelBaseStep : TestStep
    {
        #region Settings

        [Browsable(false)]
        public virtual bool isControlledByParent { get; set; } = false;
        [EnabledIf("isControlledByParent", false, HideIfDisabled = false)]

        [Display("PNA", Group: "Instrument Settings", Order: 1)]
        public PNAX PNAX { get; set; }

        private int _Channel;
        [EnabledIf("isControlledByParent", false, HideIfDisabled = false)]
        [Display("Channel", Group: "Instrument Settings", Order: 2)]
        public virtual int Channel { get; set; }
        #endregion

        public GeneralStandardChannelBaseStep()
        {

        }

        public override void Run()
        {

        }
    }
}
