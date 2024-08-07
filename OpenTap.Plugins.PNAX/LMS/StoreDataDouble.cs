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

namespace OpenTap.Plugins.PNAX.LMS
{
    [Display("Store Trace Data - Double", Groups: new[] { "Network Analyzer", "Load/Measure/Store" }, Description: "Appends a double data to trace.")]
    //[AllowAsChildIn(typeof(StoreDataBase))]
    public class StoreDataDouble : TestStep
    {
        #region Settings
        public String Desc { get; set; }
        public double Value { get; set; }
        #endregion

        public StoreDataDouble()
        {
            Desc = "";
            Value = double.NaN;
        }

        public override void Run()
        {
            List<(string, object)> _parentsMetaData = GetParent<StoreDataBase>().MetaData;

            if (!Desc.Equals("") && (Value != double.NaN))
            {
                _parentsMetaData.Add((Desc, Value));
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }

}
