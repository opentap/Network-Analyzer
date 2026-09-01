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

            // Mixer Setup
            MixerSetupTestStep mixerSetupTestStep = ConfigureChildStep(new MixerSetupTestStep());
            // Mixer Power
            MixerPowerTestStep mixerPowerTestStep = ConfigureChildStep(new MixerPowerTestStep { EnablePort3Settings = false, EnablePort4Settings = false });
            // Mixer Frequency
            MixerFrequencyTestStep mixerFrequencyTestStep = ConfigureChildStep(new MixerFrequencyTestStep());

            // Tone Power
            TonePower power = ConfigureChildStep(new TonePower { ToneFrequencySweepType = this.ChannelSweepType });
            // Tone Frequency
            ToneFrequency frequency = ConfigureChildStep(new ToneFrequency());

            // Traces
            SweptIMDNewTrace sweptIMDNewTrace = ConfigureChildStep(new SweptIMDNewTrace());


            // Defaults
            mixerFrequencyTestStep.InputMixerFrequencyStart = 10.5e6;
            mixerFrequencyTestStep.InputMixerFrequencyStop = 49.9995e9;
            mixerFrequencyTestStep.InputMixerFrequencyCenter = 25.005e9;
            mixerFrequencyTestStep.InputMixerFrequencySpan = 49.99e9;

            mixerFrequencyTestStep.IFMixerFrequencyStart = 10.5e6;
            mixerFrequencyTestStep.IFMixerFrequencyStop = 49.9995e9;
            mixerFrequencyTestStep.IFMixerFrequencyCenter = 25.005e9;
            mixerFrequencyTestStep.IFMixerFrequencySpan = 49.99e9;
            mixerFrequencyTestStep.IFMixerFrequencyFixed = 10e6;

            mixerFrequencyTestStep.OutputMixerFrequencyStart = 10.5e6;
            mixerFrequencyTestStep.OutputMixerFrequencyStop = 49.9995e9;
            mixerFrequencyTestStep.OutputMixerFrequencyCenter = 25.005e9;
            mixerFrequencyTestStep.OutputMixerFrequencySpan = 49.99e9;



            this.ChildTestSteps.Add(frequency);
            this.ChildTestSteps.Add(power);
            this.ChildTestSteps.Add(mixerFrequencyTestStep);
            this.ChildTestSteps.Add(mixerPowerTestStep);
            this.ChildTestSteps.Add(mixerSetupTestStep);
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
