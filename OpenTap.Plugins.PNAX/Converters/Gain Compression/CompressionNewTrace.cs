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
    [AllowAsChildIn(typeof(GainCompressionChannel))]
    [AllowChildrenOfType(typeof(CompressionSingleTrace))]
    [Display("Compression Traces", Groups: new[] { "PNA-X", "Converters", "Compression" }, Description: "Insert a description here")]
    public class GainCompressionNewTrace : ConverterBaseStep
    {
        #region Settings

        private CompressionTraceEnum _Meas;
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public CompressionTraceEnum Meas
        {
            get
            {
                return _Meas;
            }
            set
            {
                _Meas = value;
            }
        }

        #endregion

        public GainCompressionNewTrace()
        {
            // ToDo: Set default values for properties / settings.
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


        [Browsable(true)]
        [Display("Add New Trace", Groups: new[] { "Trace" }, Order: 10)]
        [Layout(LayoutMode.FullRow)]
        public void AddNewTrace()
        {
            CompressionTraceEnum compressionTrace;
            if (Enum.TryParse<CompressionTraceEnum>(Meas.ToString(), out compressionTrace))
            {
                this.ChildTestSteps.Add(new CompressionSingleTrace() { Meas = compressionTrace, Channel = this.Channel });
            }


        }

    }
}
