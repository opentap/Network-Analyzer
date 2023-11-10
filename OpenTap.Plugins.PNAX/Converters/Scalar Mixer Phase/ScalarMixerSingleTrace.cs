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
    public enum SMCTraceEnum
    {
        SC21,
        SC12,
        S11,
        S22,
        IPwr,
        OPwr,
        RevIPwr,
        RevOPwr,
        AI1_1,
        AI2_1,
        AI1_2,
        AI2_2,
    }


    [AllowAsChildIn(typeof(ScalarMixerChannel))]
    [AllowAsChildIn(typeof(ScalarMixerNewTrace))]
    [Display("Scalar Mixer Single Trace", Groups: new[] { "PNA-X", "Converters", "Scalar Mixer Converter + Phase" }, Description: "Insert a description here")]
    public class ScalarMixerSingleTrace : ConverterSingleTraceBaseStep
    {
        #region Settings
        private SMCTraceEnum _Meas;
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public SMCTraceEnum Meas
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

        public ScalarMixerSingleTrace()
        {
            Meas = SMCTraceEnum.SC21;
            measClass = "Scalar Mixer/Converter";
        }

    }
}
