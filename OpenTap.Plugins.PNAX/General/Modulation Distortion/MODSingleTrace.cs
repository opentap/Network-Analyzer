// Author: CMontes
// Copyright:   Copyright 2024 Keysight Technologies
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
    public enum MODTraceEnum
    {
        PIn1,
        PInWav,
        POut1,
        Pout2,
        PDlvrIn1,
        PDlvrOut2,
        DutNois2,
        MSig2,
        MDist2,
        MGain21,
        PGain21,
        LMatch2,
        CarrIn1,
        CarrOut2,
        CarrGain21,
    }

    //[AllowAsChildIn(typeof(MODNewTrace))]
    [Display(
        "Modulation Distortion Single Trace",
        Groups: new[] { "Network Analyzer", "General", "Modulation Distortion" },
        Description: "Insert a description here"
    )]
    public class MODSingleTrace : SingleTraceBaseStep
    {
        #region Settings
        private MODTraceEnum _Meas;

        [EnabledIf(nameof(CustomTraceMeas), false, HideIfDisabled = true)]
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11.1)]
        public MODTraceEnum Meas
        {
            get { return _Meas; }
            set
            {
                _Meas = value;
                string scpi = Scpi.Format("{0}", value);
                measEnumName = scpi; // value.ToString();
                UpdateTestStepName();
            }
        }

        #endregion

        public MODSingleTrace()
        {
            Meas = MODTraceEnum.PIn1;
            measClass = "Modulation Distortion";
        }
    }
}
