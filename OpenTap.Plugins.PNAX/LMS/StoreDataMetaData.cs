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

namespace OpenTap.Plugins.PNAX.LMS
{
    [Display("Store Trace Data - Meta Data", Groups: new[] { "Network Analyzer", "Load/Measure/Store" }, Description: "Appends Meta data to trace.")]
    [AllowAsChildIn(typeof(StoreDataBase))]
    public class StoreDataMetaData : StoreDataMetaDataChildBase
    {
        #region Settings
        [Browsable(true)]
        [Display("MetaData", Groups: new[] { "MetaData" }, Order: 50)]
        public Input<List<(string, object)>> MetaData { get; set; }
        #endregion

        public StoreDataMetaData()
        {
            MetaData = new Input<List<(string, object)>>();
        }

        public override void Run()
        {
            if (MetaData.Property != null)
            {
                if (MetaData.Value != null)
                {
                    foreach (var item in MetaData.Value)
                    {
                        ParentMetaData.Add(item);
                    }
                }
            }
            else if (MetaData.Step is PNABaseStep step)
            {
                Log.Info("Get MetaData:");
                foreach (var item in step.GetMetaData())
                {
                    ParentMetaData.Add(item);
                    Log.Info("Adding metadata: " + item);
                }
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
