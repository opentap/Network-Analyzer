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
    public class NoiseFigureNewTrace : ConverterNewTraceBaseStep
    {
        #region Settings
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public NoiseFigureTraceEnum Meas { get; set; }
        #endregion

        public NoiseFigureNewTrace()
        {
            Meas = NoiseFigureTraceEnum.NF;
            ChildTestSteps.Add(new NoiseFigureSingleTrace() { Meas = NoiseFigureTraceEnum.NF});
        }

        public override void Run()
        {
            // Delete dummy trace defined during channel setup
            // DISPlay:MEASure<mnum>:DELete?
            // CALCulate<cnum>:PARameter:DELete[:NAME] <Mname>
            PNAX.ScpiCommand($"CALCulate{Channel}:PARameter:DELete \'CH{Channel}_DUMMY_NF_1\'");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }

        protected override void AddNewTrace()
        {
            // Validate the meas has not been added before
            foreach (TestStep child in ChildTestSteps)
            {
                if (child is NoiseFigureSingleTrace)
                {
                    if ((child as NoiseFigureSingleTrace).Meas == this.Meas)
                    {
                        Log.Info("Can't add duplicate measurement!");
                        return;
                    }
                }
            }

            this.ChildTestSteps.Add(new NoiseFigureSingleTrace() { Meas = this.Meas, Channel = this.Channel });
        }

    }
}
