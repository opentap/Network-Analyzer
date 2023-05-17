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
    public class GeneralChannelBaseStep : TestStep
    {
        #region Settings

        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        private int _channel;
        [Display("Channel", Order: 1)]
        [Output]
        public int Channel
        {
            get { return _channel; }
            set
            {
                _channel = value;
                foreach (var a in this.ChildTestSteps)
                {
                    if (a is GeneralBaseStep)
                    {
                        (a as GeneralBaseStep).Channel = value;
                    }
                }
            }
        }
        #endregion

        public GeneralChannelBaseStep()
        {
            Channel = 1;
        }

        public override void Run()
        {

        }
    }
}
