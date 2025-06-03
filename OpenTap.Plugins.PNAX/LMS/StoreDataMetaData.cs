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
            List<(string, object)> _parentsMetaData = GetParent<StoreDataBase>().MetaData;



            // if MetaData available
            if ((MetaData.Property != null) && (MetaData.Value.Count > 1))
            {
                // for every item in metadata
                foreach (var i in MetaData.Value)
                {
                    // Append Parent Step's metadata
                    _parentsMetaData.Add(i);
                }
            }
            else
            {
                // Looks like the input step does not have metadata available,
                // lets get it and add it
                PNABaseStep x = (MetaData.Step as PNABaseStep);
                Log.Info("Get MetaData: ");
                List<(string, object)> ret = x.GetMetaData();
                foreach (var it in ret)
                {
                    _parentsMetaData.Add(it);
                    Log.Info("Adding metadata: " + x.GetMetaData());
                }
            }



            UpgradeVerdict(Verdict.Pass);
        }
    }
}
