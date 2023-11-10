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
    [AllowAsChildIn(typeof(NoiseFigureChannel))]
    [Display("Noise Figure New Trace", Groups: new[] { "PNA-X", "Converters", "Noise Figure Converters" }, Description: "Insert a description here")]
    public class NoiseFigureNewTrace : AddNewTraceBaseStep
    {
        #region Settings
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public NoiseFigureTraceEnum Meas { get; set; }
        #endregion

        public NoiseFigureNewTrace()
        {
            Meas = NoiseFigureTraceEnum.NF;
            IsConverter = true;
            AddNewTrace();
        }

        protected override void AddNewTrace()
        {
            this.ChildTestSteps.Add(new NoiseFigureSingleTrace() { PNAX = this.PNAX, Meas = this.Meas, Channel = this.Channel, IsControlledByParent = true, EnableTraceSettings = true });
        }

    }
}
