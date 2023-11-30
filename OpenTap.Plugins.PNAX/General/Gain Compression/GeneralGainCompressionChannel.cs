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
    [Display("Gain Compression Channel", Groups: new[] { "PNA-X", "General", "Gain Compression" }, Description: "Insert a description here")]
    public class GeneralGainCompressionChannel : PNABaseStep
    {
        #region Settings
        #endregion

        public GeneralGainCompressionChannel()
        {
            IsControlledByParent = false;
            // Add child steps in the order that is required

            // Compression
            GeneralGainCompression compression = new GeneralGainCompression { IsControlledByParent = true, Channel = this.Channel};
            // Power
            GeneralGainCompressionPower power = new GeneralGainCompressionPower { IsControlledByParent = true, Channel = this.Channel};
            // Frequency
            GeneralGainCompressionFrequency frequency = new GeneralGainCompressionFrequency { IsControlledByParent = true, Channel = this.Channel};
            // Traces
            GeneralGainCompressionNewTrace gainCompressionNewTrace = new GeneralGainCompressionNewTrace { IsControlledByParent = true, Channel = this.Channel};

            this.ChildTestSteps.Add(compression);
            this.ChildTestSteps.Add(power);
            this.ChildTestSteps.Add(frequency);
            this.ChildTestSteps.Add(gainCompressionNewTrace);
        }

        public override void Run()
        {
            PNAX.GetNewTraceID(Channel);
            // Define a dummy measurement so we can setup all channel parameters
            // we will add the traces during the StandardSingleTrace or StandardNewTrace test steps
            PNAX.ScpiCommand($"CALCulate{Channel}:CUST:DEFine \'CH{Channel}_DUMMY_S21_1\',\'Gain Compression\',\'S21\'");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
