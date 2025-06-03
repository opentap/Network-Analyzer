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
    [Display("Swept IMD Channel", Groups: new[] { "Network Analyzer", "General", "Swept IMD" }, Description: "Insert a description here", Order: 4)]
    public class GeneralSweptIMDChannel : PNABaseStep
    {
        #region Settings
        private GeneralToneFrequencySweepTypeEnum ChannelSweepType { get; set; }
        #endregion

        public void UpdateChannelSweepType(GeneralToneFrequencySweepTypeEnum value)
        {
            ChannelSweepType = value;
            foreach (TestStep step in ChildTestSteps)
            {
                if (step is GeneralTonePower)
                {
                    (step as GeneralTonePower).ToneFrequencySweepType = ChannelSweepType;
                }
            }
        }

        public GeneralSweptIMDChannel()
        {
            IsControlledByParent = false;
            // Add child steps in the order that is required
            GeneralSweptIMDConfigure configure = new GeneralSweptIMDConfigure { IsControlledByParent = true, Channel = this.Channel };
            // Tone Power
            GeneralTonePower power = new GeneralTonePower { IsControlledByParent = true, Channel = this.Channel, ToneFrequencySweepType = this.ChannelSweepType };
            // Tone Frequency
            GeneralToneFrequency frequency = new GeneralToneFrequency { IsControlledByParent = true, Channel = this.Channel };

            // Traces
            GeneralSweptIMDNewTrace sweptIMDNewTrace = new GeneralSweptIMDNewTrace { IsControlledByParent = true, Channel = this.Channel };

            this.ChildTestSteps.Add(configure);
            this.ChildTestSteps.Add(power);
            this.ChildTestSteps.Add(frequency);
            this.ChildTestSteps.Add(sweptIMDNewTrace);

            // Number of points are not needed in another step for General Swept IMD
            // Once we have all child steps, lets get the number of points
            //this.UpdateNumberOfPoints();
        }

        public override void Run()
        {
            PNAX.GetNewTraceID(Channel);
            // Define a dummy measurement so we can setup all channel parameters
            // we will add the traces during the StandardSingleTrace or StandardNewTrace test steps
            PNAX.ScpiCommand($"CALCulate{Channel}:CUST:DEFine \'CH{Channel.ToString()}_DUMMY_PwrMain_1\',\'Swept IMD\',\'PwrMain\'");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
