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
    public class ScalarMixerNewTrace : ConverterNewTraceBaseStep
    {
        #region Settings
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public SMCTraceEnum Meas { get; set; }

        #endregion

        public ScalarMixerNewTrace()
        {
            ChildTestSteps.Add(new ScalarMixerSingleTrace() { PNAX = this.PNAX, Meas = SMCTraceEnum.SC21});
        }

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            List<(String, object)> retVal = new List<(string, object)>();

            return retVal;
        }

        public override void Run()
        {
            // Delete dummy trace defined during channel setup
            // DISPlay:MEASure<mnum>:DELete?
            // CALCulate<cnum>:PARameter:DELete[:NAME] <Mname>
            PNAX.ScpiCommand($"CALCulate{Channel}:PARameter:DELete \'CH{Channel}_DUMMY_SC21_1\'");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }

        protected override void AddNewTrace()
        {
            this.ChildTestSteps.Add(new ScalarMixerSingleTrace() { PNAX = this.PNAX, Meas = this.Meas, Channel = this.Channel });
        }

    }
}
