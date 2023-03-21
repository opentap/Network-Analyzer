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
    [Display("Swept IMD Channel", Groups: new[] { "PNA-X", "Converters", "Swept IMD Converters" }, Description: "Insert a description here", Order: 4)]
    public class SweptIMDChannel : ConverterChannelBase
    {
        #region Settings

        #endregion

        public SweptIMDChannel()
        {
            // Add child steps in the order that is required

            // Mixer Setup
            MixerSetupTestStep mixerSetupTestStep = new MixerSetupTestStep { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };
            // Mixer Power
            MixerPowerTestStep mixerPowerTestStep = new MixerPowerTestStep { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };
            // Mixer Frequency
            MixerFrequencyTestStep mixerFrequencyTestStep = new MixerFrequencyTestStep { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };

            // Tone Power
            TonePower power = new TonePower { IsControlledByParent = true };
            // Tone Frequency
            ToneFrequency frequency = new ToneFrequency { IsControlledByParent = true };

            // Traces
            SweptIMDNewTrace sweptIMDNewTrace = new SweptIMDNewTrace();

            this.ChildTestSteps.Add(mixerSetupTestStep);
            this.ChildTestSteps.Add(mixerPowerTestStep);
            this.ChildTestSteps.Add(mixerFrequencyTestStep);
            this.ChildTestSteps.Add(power);
            this.ChildTestSteps.Add(frequency);
            this.ChildTestSteps.Add(sweptIMDNewTrace);
        }

        public override void Run()
        {
            base.Run();
        }
    }
}
