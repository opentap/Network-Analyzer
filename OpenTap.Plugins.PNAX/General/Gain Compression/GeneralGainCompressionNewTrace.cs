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
    //[AllowAsChildIn(typeof(GeneralGainCompressionChannel))]
    //[AllowChildrenOfType(typeof(GeneralGainCompressionSingleTrace))]
    [Display("Compression Traces", Groups: new[] { "Network Analyzer", "General", "Gain Compression" }, Description: "Insert a description here")]
    public class GeneralGainCompressionNewTrace : AddNewTraceBaseStep
    {
        #region Settings
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public GeneralGainCompressionTraceEnum Meas { get; set; }

        #endregion

        public GeneralGainCompressionNewTrace()
        {
            Meas = GeneralGainCompressionTraceEnum.S21;
            measEnumName = Meas.ToString();
            ChildTestSteps.Add(new GeneralGainCompressionSingleTrace() { PNAX = this.PNAX, Meas = this.Meas, Channel = this.Channel, IsControlledByParent = true, EnableTraceSettings = true });
        }

        protected override void AddNewTrace()
        {
            ChildTestSteps.Add(new GeneralGainCompressionSingleTrace() { PNAX = this.PNAX, Meas = this.Meas, Channel = this.Channel, IsControlledByParent = true, EnableTraceSettings = true });
        }

        protected override void DeleteDummyTrace()
        {
            PNAX.ScpiCommand($"CALCulate{Channel}:PARameter:DELete \'CH{Channel}_DUMMY_S21_1\'");
        }

    }
}
