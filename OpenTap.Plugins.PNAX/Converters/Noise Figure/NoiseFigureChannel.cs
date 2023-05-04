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
    [Display("Noise Figure Converters Channel", Groups: new[] { "PNA-X", "Converters", "Noise Figure Converters" }, Description: "Insert a description here")]
    public class NoiseFigureChannel : ConverterChannelBase
    {
        #region Settings
        #endregion

        public NoiseFigureChannel()
        {
            // Mixer Setup
            MixerSetupTestStep mixerSetupTestStep = new MixerSetupTestStep { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };
            // Mixer Power
            MixerPowerTestStep mixerPowerTestStep = new MixerPowerTestStep { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages, EnablePort3Settings = false, EnablePort4Settings = false, EnableSweptPowerSettings = false };
            // Mixer Frequency
            MixerFrequencyTestStep mixerFrequencyTestStep = new MixerFrequencyTestStep { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };

            // Compression
            NoiseFigure noiseFigure = new NoiseFigure { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };
            // Power
            NoiseFigurePower power = new NoiseFigurePower { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };
            // Frequency
            NoiseFigureFrequency frequency = new NoiseFigureFrequency { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };

            // Trace
            NoiseFigureNewTrace noiseFigureNewTrace = new NoiseFigureNewTrace { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };

            this.ChildTestSteps.Add(mixerSetupTestStep);
            this.ChildTestSteps.Add(mixerPowerTestStep);
            this.ChildTestSteps.Add(mixerFrequencyTestStep);
            this.ChildTestSteps.Add(noiseFigure);
            this.ChildTestSteps.Add(power);
            this.ChildTestSteps.Add(frequency);
            this.ChildTestSteps.Add(noiseFigureNewTrace);

            // Once we have all child steps, lets get the number of points
            this.UpdateNumberOfPoints();
        }

        public override void Run()
        {
            int traceid = PNAX.GetNewTraceID(Channel);
            // Define a dummy measurement so we can setup all channel parameters
            // we will add the traces during the StandardSingleTrace or StandardNewTrace test steps
            PNAX.ScpiCommand($"CALCulate{Channel.ToString()}:CUST:DEFine \'CH{Channel.ToString()}_DUMMY_NF_1\',\'Noise Figure Converters\',\'NF\'");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
