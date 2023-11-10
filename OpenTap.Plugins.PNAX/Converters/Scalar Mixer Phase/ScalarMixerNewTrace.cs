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
    [AllowAsChildIn(typeof(ScalarMixerChannel))]
    [Display("Scalar Mixer New Trace", Groups: new[] { "PNA-X", "Converters", "Scalar Mixer Converter + Phase" }, Description: "Insert a description here")]
    public class ScalarMixerNewTrace : AddNewTraceBaseStep
    {
        #region Settings
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public SMCTraceEnum Meas { get; set; }

        #endregion

        public ScalarMixerNewTrace()
        {
            Meas = SMCTraceEnum.SC21;
            IsConverter = true;
            AddNewTrace();
        }

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            List<(string, object)> retVal = new List<(string, object)>();

            return retVal;
        }

        protected override void AddNewTrace()
        {
            ChildTestSteps.Add(new ScalarMixerSingleTrace() { PNAX = this.PNAX, Meas = this.Meas, Channel = this.Channel, IsControlledByParent = true, EnableTraceSettings = true });
        }

    }
}
