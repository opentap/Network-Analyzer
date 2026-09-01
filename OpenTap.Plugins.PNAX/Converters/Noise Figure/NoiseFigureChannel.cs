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
    [Display("Noise Figure Converters Channel", Groups: new[] { "Network Analyzer", "Converters", "Noise Figure Converters" }, Description: "Insert a description here")]
    public class NoiseFigureChannel : ConverterChannelBaseStep
    {
        #region Settings
        #endregion

        public NoiseFigureChannel()
        {
            // Mixer Setup
            MixerSetupTestStep mixerSetupTestStep = ConfigureChildStep(new MixerSetupTestStep());
            // Mixer Power
            MixerPowerTestStep mixerPowerTestStep = ConfigureChildStep(new MixerPowerTestStep { EnablePort3Settings = false, EnablePort4Settings = false, EnableSweptPowerSettings = false });
            // Mixer Frequency
            MixerFrequencyTestStep mixerFrequencyTestStep = ConfigureChildStep(new MixerFrequencyTestStep());

            // Compression
            NoiseFigure noiseFigure = ConfigureChildStep(new NoiseFigure());
            // Power
            NoiseFigurePower power = ConfigureChildStep(new NoiseFigurePower());
            // Frequency
            NoiseFigureFrequency frequency = ConfigureChildStep(new NoiseFigureFrequency());

            // Trace
            NoiseFigureNewTrace noiseFigureNewTrace = ConfigureChildStep(new NoiseFigureNewTrace());

            this.ChildTestSteps.Add(frequency);
            this.ChildTestSteps.Add(power);
            this.ChildTestSteps.Add(noiseFigure);
            this.ChildTestSteps.Add(mixerFrequencyTestStep);
            this.ChildTestSteps.Add(mixerPowerTestStep);
            this.ChildTestSteps.Add(mixerSetupTestStep);
            this.ChildTestSteps.Add(noiseFigureNewTrace);

            // Once we have all child steps, lets get the number of points
            this.UpdateNumberOfPoints();
        }

        public override void Run()
        {
            DefineDummyTrace("Noise Figure Converters", "NF");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
