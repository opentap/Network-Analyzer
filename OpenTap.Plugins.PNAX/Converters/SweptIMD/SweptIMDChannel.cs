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
    [Display("Swept IMD Converters Channel", Groups: new[] { "PNA-X", "Converters", "Swept IMD Converters" }, Description: "Insert a description here", Order: 4)]
    public class SweptIMDChannel : ConverterChannelBase
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
            MixerSetupTestStep mixerSetupTestStep = new MixerSetupTestStep { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };
            // Mixer Power
            MixerPowerTestStep mixerPowerTestStep = new MixerPowerTestStep { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages, EnablePort3Settings = false, EnablePort4Settings = false };
            // Mixer Frequency
            MixerFrequencyTestStep mixerFrequencyTestStep = new MixerFrequencyTestStep { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };

            // Tone Power
            TonePower power = new TonePower { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages, ToneFrequencySweepType = this.ChannelSweepType };
            // Tone Frequency
            ToneFrequency frequency = new ToneFrequency { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };

            // Traces
            SweptIMDNewTrace sweptIMDNewTrace = new SweptIMDNewTrace { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };


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



            this.ChildTestSteps.Add(mixerSetupTestStep);
            this.ChildTestSteps.Add(mixerPowerTestStep);
            this.ChildTestSteps.Add(mixerFrequencyTestStep);
            this.ChildTestSteps.Add(power);
            this.ChildTestSteps.Add(frequency);
            this.ChildTestSteps.Add(sweptIMDNewTrace);

            // Once we have all child steps, lets get the number of points
            this.UpdateNumberOfPoints();
        }

        public override void Run()
        {
            int traceid = PNAX.GetNewTraceID(Channel);
            // Define a dummy measurement so we can setup all channel parameters
            // we will add the traces during the StandardSingleTrace or StandardNewTrace test steps
            PNAX.ScpiCommand($"CALCulate{Channel.ToString()}:CUST:DEFine \'CH{Channel.ToString()}_DUMMY_PwrMain_1\',\'Swept IMD Converters\',\'PwrMain\'");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
