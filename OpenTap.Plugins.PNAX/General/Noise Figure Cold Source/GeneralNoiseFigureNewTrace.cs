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
    //[AllowAsChildIn(typeof(GeneralNoiseFigureChannel))]
    [Display("Noise Figure New Trace", Groups: new[] { "Network Analyzer", "General", "Noise Figure Cold Source" }, Description: "Insert a description here")]
    public class GeneralNoiseFigureNewTrace : AddNewTraceBaseStep
    {
        #region Settings
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public GeneralNoiseFigureTraceEnum Meas { get; set; }
        #endregion

        public GeneralNoiseFigureNewTrace()
        {
            Meas = GeneralNoiseFigureTraceEnum.NF;
            AddNewTraceChild<GeneralNoiseFigureSingleTrace>(trace => trace.Meas = Meas);
        }

        protected override void AddNewTrace()
        {
            AddNewTraceChild<GeneralNoiseFigureSingleTrace>(trace => trace.Meas = Meas);
        }

        protected override void DeleteDummyTrace()
        {
            DeleteDummyTrace("NF");
        }


    }
}
