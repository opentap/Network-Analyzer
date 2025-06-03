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

namespace OpenTap.Plugins.PNAX.LMS
{
    [Display(
        "Store Trace Data - DUT Meta Data",
        Groups: new[] { "Network Analyzer", "Load/Measure/Store" },
        Description: "Appends DUT Meta data to publish table"
    )]
    [AllowAsChildIn(typeof(StoreDataBase))]
    public class StoreDutMetaData : TestStep
    {
        #region Settings
        [Display("DUT", Order: 1)]
        public GenericDUT Dut { get; set; }

        #endregion

        public StoreDutMetaData() { }

        public override void Run()
        {
            List<(string, object)> _parentsMetaData = GetParent<StoreDataBase>().MetaData;

            Type myType = Dut.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(Dut, null);

                foreach (var a in prop.CustomAttributes)
                {
                    if (a.AttributeType.Name.Equals("MetaDataAttribute"))
                    {
                        _parentsMetaData.Add((prop.Name, propValue));
                        break;
                    }
                }
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
