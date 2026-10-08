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
    [Display("Swept IMD Converters Channel", Groups: new[] { "Network Analyzer", "Converters", "Swept IMD Converters" }, Description: "Insert a description here", Order: 4)]
    public class SweptIMDChannel : ConverterChannelBaseStep
    {
        #region Settings
        private ToneFrequencySweepTypeEnum ChannelSweepType { get; set; }
        #endregion

        public void UpdateChannelSweepType(ToneFrequencySweepTypeEnum value)
        {
            ChannelSweepType = value;
            foreach(TestStep step in ChildTestSteps)
            {
                if (step is TonePower)
                {
                    (step as TonePower).ToneFrequencySweepType = ChannelSweepType;
                }
            }
        }

        public SweptIMDChannel()
        {
            // Add child steps in the order that is required

            // Mixer Setup/Power/Frequency
            var mixerSteps = AddMixerChildSteps();

            // Tone Power
            TonePower power = ConfigureChildStep(new TonePower { ToneFrequencySweepType = this.ChannelSweepType });
            // Tone Frequency
            ToneFrequency frequency = ConfigureChildStep(new ToneFrequency());

            // Traces
            SweptIMDNewTrace sweptIMDNewTrace = ConfigureChildStep(new SweptIMDNewTrace());


            // Defaults
            mixerSteps.Frequency.InputMixerFrequencyStart = 10.5e6;
            mixerSteps.Frequency.InputMixerFrequencyStop = 49.9995e9;
            mixerSteps.Frequency.InputMixerFrequencyCenter = 25.005e9;
            mixerSteps.Frequency.InputMixerFrequencySpan = 49.99e9;

            mixerSteps.Frequency.IFMixerFrequencyStart = 10.5e6;
            mixerSteps.Frequency.IFMixerFrequencyStop = 49.9995e9;
            mixerSteps.Frequency.IFMixerFrequencyCenter = 25.005e9;
            mixerSteps.Frequency.IFMixerFrequencySpan = 49.99e9;
            mixerSteps.Frequency.IFMixerFrequencyFixed = 10e6;

            mixerSteps.Frequency.OutputMixerFrequencyStart = 10.5e6;
            mixerSteps.Frequency.OutputMixerFrequencyStop = 49.9995e9;
            mixerSteps.Frequency.OutputMixerFrequencyCenter = 25.005e9;
            mixerSteps.Frequency.OutputMixerFrequencySpan = 49.99e9;



            this.ChildTestSteps.Add(frequency);
            this.ChildTestSteps.Add(power);
            this.ChildTestSteps.Add(sweptIMDNewTrace);

            // Once we have all child steps, lets get the number of points
            this.UpdateNumberOfPoints();
        }

        public override void Run()
        {
            DefineDummyTrace("Swept IMD Converters", "PwrMain");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
