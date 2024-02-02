// Author: MyName
// Copyright:   Copyright 2024 Keysight Technologies
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
    [AllowAsChildIn(typeof(MODXChannel))]
    [AllowChildrenOfType(typeof(MODXSingleTrace))]
    [Display("Modulation Distortion New Trace", Groups: new[] { "Network Analyzer", "Converters", "Modulation Distortion" }, Description: "Insert a description here")]
    public class MODXNewTrace : AddNewTraceBaseStep
    {
        #region Settings
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public MODTraceEnum Meas { get; set; }
        #endregion

        public MODXNewTrace()
        {
            Meas = MODTraceEnum.PIn1;
            ChildTestSteps.Add(new MODXSingleTrace() { PNAX = this.PNAX, Meas = this.Meas, Channel = this.Channel, IsControlledByParent = true, EnableTraceSettings = true });
        }

        protected override void DeleteDummyTrace()
        {
            PNAX.ScpiCommand($"CALCulate{Channel}:PARameter:DELete \'CH{Channel}_DUMMY_1\'");
        }

        protected override void AddNewTrace()
        {
            ChildTestSteps.Add(new MODXSingleTrace() { PNAX = this.PNAX, Meas = this.Meas, Channel = this.Channel, IsControlledByParent = true, EnableTraceSettings = true });
        }
    }
}
