// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using OpenTap;

namespace OpenTap.Plugins.PNAX
{
    [Browsable(false)]
    public class StoreDataBase : TestStep
    {
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display("Auto Select All Channels", Group: "Measurements", Order: 10)]
        public bool AutoSelectChannels { get; set; }

        [EnabledIf("AutoSelectChannels", false, HideIfDisabled = true)]
        [Display(
            "Channels",
            Description: "Choose which channels to grab data from.",
            "Measurements",
            Order: 10.1
        )]
        public List<int> channels { get; set; }

        [Browsable(false)]
        [Display("MetaData", Groups: new[] { "MetaData" }, Order: 50)]
        public List<(string, object)> MetaData { get; set; }

        public override void Run()
        {
            throw new NotImplementedException();
        }

        public void AutoSelectChannelsAvailableOnInstrument()
        {
            if (AutoSelectChannels)
            {
                channels = PNAX.GetActiveChannels();
            }
        }
    }
}
