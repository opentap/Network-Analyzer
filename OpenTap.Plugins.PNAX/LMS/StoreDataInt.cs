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
using System.Text;
using OpenTap;

namespace OpenTap.Plugins.PNAX.LMS
{
    [Display(
        "Store Trace Data - Integer",
        Groups: new[] { "Network Analyzer", "Load/Measure/Store" },
        Description: "Appends an integer data to trace."
    )]
    [AllowAsChildIn(typeof(StoreDataBase))]
    public class StoreDataInt : TestStep
    {
        #region Settings
        public String Desc { get; set; }
        public int Value { get; set; }
        #endregion

        public StoreDataInt()
        {
            Desc = "";
            Value = 0;
        }

        public override void Run()
        {
            List<(string, object)> _parentsMetaData = GetParent<StoreDataBase>().MetaData;

            if (!Desc.Equals(""))
            {
                _parentsMetaData.Add((Desc, Value));
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
