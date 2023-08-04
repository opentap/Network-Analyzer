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

        // TODO
        // Set to Browsable False for release
        // TODO
        [Output]
        [Browsable(true)]
        [Display("MetaData", Groups: new[] { "MetaData" }, Order: 1000.0)]
        public List<(string, string)> MetaData { get; private set; }

        // TODO
        // Set to Browsable False for release
        // TODO
        [Browsable(true)]
        [Display("Update MetaData", Groups: new[] { "MetaData" }, Order: 1000.2)]
        public void UpdateMetaData()
        {
            MetaData = new List<(string, string)>();

            MetaData.Add(("Channel", this.Channel.ToString()));

            foreach (var ch in this.ChildTestSteps)
            {
                List<(string, string)> ret = (ch as GeneralBaseStep).GetMetaData();
                foreach (var it in ret)
                {
                    MetaData.Add(it);
                }
            }
        }

        #endregion

        public GeneralChannelBaseStep()
        {
            MetaData = new List<(string, string)>();
            Channel = 1;
        }

        public override void Run()
        {
            UpdateMetaData();

        }
    }
}
