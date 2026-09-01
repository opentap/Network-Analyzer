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
    [Display("Gain Compression Channel", Groups: new[] { "Network Analyzer", "General", "Gain Compression" }, Description: "Insert a description here")]
    public class GeneralGainCompressionChannel : PNABaseStep
    {
        #region Settings
        #endregion

        public GeneralGainCompressionChannel()
        {
            IsControlledByParent = false;
            // Add child steps in the order that is required

            // Compression
            GeneralGainCompression compression = ConfigureChildStep(new GeneralGainCompression());
            // Power
            GeneralGainCompressionPower power = ConfigureChildStep(new GeneralGainCompressionPower());
            // Frequency
            GeneralGainCompressionFrequency frequency = ConfigureChildStep(new GeneralGainCompressionFrequency());
            // Traces
            GeneralGainCompressionNewTrace gainCompressionNewTrace = ConfigureChildStep(new GeneralGainCompressionNewTrace());

            this.ChildTestSteps.Add(compression);
            this.ChildTestSteps.Add(power);
            this.ChildTestSteps.Add(frequency);
            this.ChildTestSteps.Add(gainCompressionNewTrace);
        }

        public override void Run()
        {
            DefineDummyTrace("Gain Compression", "S21");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
