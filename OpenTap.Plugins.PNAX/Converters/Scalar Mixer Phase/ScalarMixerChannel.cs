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
    [Display("Scalar Mixer/Converter + Phase Channel", Groups: new[] { "PNA-X", "Converters", "Scalar Mixer Converter + Phase" }, Description: "Insert a description here")]
    public class ScalarMixerChannel : ConverterChannelBase
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
            MixerSetupTestStep mixerSetupTestStep = new MixerSetupTestStep { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };
            // Mixer Power
            MixerPowerTestStep mixerPowerTestStep = new MixerPowerTestStep { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages, EnablePort3Settings = false, EnablePort4Settings = false };
            // Mixer Frequency
            MixerFrequencyTestStep mixerFrequencyTestStep = new MixerFrequencyTestStep { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };

            // Compression
            ScalarMixerSweep scalerMixerSweep = new ScalarMixerSweep { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };
            // Power
            ScalarMixerPower scalerMixerPower = new ScalarMixerPower { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };

            // Traces
            ScalarMixerNewTrace scalarMixerNewTrace = new ScalarMixerNewTrace { IsControlledByParent = true, Channel = this.Channel, ConverterStages = this.ConverterStages };


            // Defaults for MixerPowerTestStep
            mixerPowerTestStep.LO1SweptPowerStart = -10.0;
            mixerPowerTestStep.LO1SweptPowerStop = 0.0;
            mixerPowerTestStep.LO1SweptPowerStep = 0.050;
            mixerPowerTestStep.LO2SweptPowerStart = -10.0;
            mixerPowerTestStep.LO2SweptPowerStop = 0.0;
            mixerPowerTestStep.LO2SweptPowerStep = 0.050;

            this.ChildTestSteps.Add(mixerSetupTestStep);
            this.ChildTestSteps.Add(mixerPowerTestStep);
            this.ChildTestSteps.Add(mixerFrequencyTestStep);
            this.ChildTestSteps.Add(scalerMixerPower);
            this.ChildTestSteps.Add(scalerMixerSweep);
            this.ChildTestSteps.Add(scalarMixerNewTrace);

            // Once we have all child steps, lets get the number of points
            this.UpdateNumberOfPoints();
        }

        public override void Run()
        {
            int traceid = PNAX.GetNewTraceID(Channel);
            // Define a dummy measurement so we can setup all channel parameters
            // we will add the traces during the StandardSingleTrace or StandardNewTrace test steps
            PNAX.ScpiCommand($"CALCulate{Channel.ToString()}:CUST:DEFine \'CH{Channel.ToString()}_DUMMY_SC21_1\',\'Scalar Mixer/Converter\',\'SC21\'");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
            UpdateMetaData();
        }
    }
}
