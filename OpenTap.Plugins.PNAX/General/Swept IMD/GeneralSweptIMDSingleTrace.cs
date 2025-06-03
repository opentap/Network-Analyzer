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

namespace OpenTap.Plugins.PNAX
{
    //[AllowAsChildIn(typeof(GeneralSweptIMDChannel))]
    //[AllowAsChildIn(typeof(GeneralSweptIMDNewTrace))]
    [Display(
        "Swept IMD Single Trace",
        Groups: new[] { "Network Analyzer", "General", "Swept IMD" },
        Description: "Insert a description here"
    )]
    public class GeneralSweptIMDSingleTrace : SingleTraceBaseStep
    {
        #region Settings
        private SweptIMDTraceEnum _Meas;

        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public SweptIMDTraceEnum Meas
        {
            get { return _Meas; }
            set
            {
                _Meas = value;
                measEnumName = value.ToString();
                UpdateTestStepName();
            }
        }

        #endregion

        public GeneralSweptIMDSingleTrace()
        {
            Meas = SweptIMDTraceEnum.Pwr2Hi;
            measClass = "Swept IMD";
        }
    }
}
