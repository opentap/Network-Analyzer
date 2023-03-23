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
    [Display("Noise Figure Channel", Groups: new[] { "PNA-X", "Converters", "Noise Figure Converters" }, Description: "Insert a description here")]
    public class NoiseFigureChannel : ConverterChannelBase
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public NoiseFigureChannel()
        {
            // Mixer Setup
            MixerSetupTestStep mixerSetupTestStep = new MixerSetupTestStep { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };
            // Mixer Power
            MixerPowerTestStep mixerPowerTestStep = new MixerPowerTestStep { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };
            // Mixer Frequency
            MixerFrequencyTestStep mixerFrequencyTestStep = new MixerFrequencyTestStep { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };

            // Compression
            NoiseFigure noiseFigure = new NoiseFigure { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };
            // Power
            NoiseFigurePower power = new NoiseFigurePower { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };
            // Frequency
            NoiseFigureFrequency frequency = new NoiseFigureFrequency { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };

            // Traces TODO
            NoiseFigureNewTrace noiseFigureNewTrace = new NoiseFigureNewTrace { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };

            this.ChildTestSteps.Add(mixerSetupTestStep);
            this.ChildTestSteps.Add(mixerPowerTestStep);
            this.ChildTestSteps.Add(mixerFrequencyTestStep);
            this.ChildTestSteps.Add(noiseFigure);
            this.ChildTestSteps.Add(power);
            this.ChildTestSteps.Add(frequency);
            this.ChildTestSteps.Add(noiseFigureNewTrace);
        }

        public override void Run()
        {
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
