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
    [Display("Scalar Mixer/Converter + Phase Channel", Groups: new[] { "Network Analyzer", "Converters", "Scalar Mixer Converter + Phase" }, Description: "Insert a description here")]
    public class ScalarMixerChannel : ConverterChannelBaseStep
    {
        #region Settings
        private ScalerMixerSweepType _sweepType { get; set; }
        public void UpdateChannelSweepType(ScalerMixerSweepType value)
        {
            _sweepType = value;
            foreach (TestStep step in ChildTestSteps)
            {
                if (step is ScalarMixerPower)
                {
                    (step as ScalarMixerPower).SweepType = _sweepType;
                }
            }
        }

        

        #endregion

        public ScalarMixerChannel()
        {

            // Mixer Setup
            MixerSetupTestStep mixerSetupTestStep = ConfigureChildStep(new MixerSetupTestStep());
            // Mixer Power
            MixerPowerTestStep mixerPowerTestStep = ConfigureChildStep(new MixerPowerTestStep { EnablePort3Settings = false, EnablePort4Settings = false });
            // Mixer Frequency
            MixerFrequencyTestStep mixerFrequencyTestStep = ConfigureChildStep(new MixerFrequencyTestStep());

            // Compression
            ScalarMixerSweep scalerMixerSweep = ConfigureChildStep(new ScalarMixerSweep());
            // Power
            ScalarMixerPower scalerMixerPower = ConfigureChildStep(new ScalarMixerPower());

            // Traces
            ScalarMixerNewTrace scalarMixerNewTrace = ConfigureChildStep(new ScalarMixerNewTrace());


            // Defaults for MixerPowerTestStep
            mixerPowerTestStep.LO1SweptPowerStart = -10.0;
            mixerPowerTestStep.LO1SweptPowerStop = 0.0;
            mixerPowerTestStep.LO1SweptPowerStep = 0.050;
            mixerPowerTestStep.LO2SweptPowerStart = -10.0;
            mixerPowerTestStep.LO2SweptPowerStop = 0.0;
            mixerPowerTestStep.LO2SweptPowerStep = 0.050;

            this.ChildTestSteps.Add(scalerMixerSweep);
            this.ChildTestSteps.Add(scalerMixerPower);
            this.ChildTestSteps.Add(mixerFrequencyTestStep);
            this.ChildTestSteps.Add(mixerPowerTestStep);
            this.ChildTestSteps.Add(mixerSetupTestStep);
            this.ChildTestSteps.Add(scalarMixerNewTrace);

            // Once we have all child steps, lets get the number of points
            this.UpdateNumberOfPoints();
        }

        public override void Run()
        {
            DefineDummyTrace("Scalar Mixer/Converter", "SC21");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
            UpdateMetaData();
        }
    }
}
