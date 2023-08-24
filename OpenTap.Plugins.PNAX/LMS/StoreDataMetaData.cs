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
    [Display("Store Trace Data - Meta Data", Groups: new[] { "PNA-X", "Load/Measure/Store" }, Description: "Appends Meta data to trace.")]
    [AllowAsChildIn(typeof(StoreData))]
    public class StoreDataMetaData : TestStep
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
            List<(string, object)> _parentsMetaData = GetParent<StoreData>().MetaData;

            // if MetaData available
            if ((MetaData.Property != null) && (MetaData.Value.Count > 0))
            {
                // for every item in metadata
                foreach (var i in MetaData.Value)
                {
                    // Append Parent Step's metadata
                    _parentsMetaData.Add(i);
                }
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
