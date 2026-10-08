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
    [Display("Gain Compression Converters Channel", Groups: new[] { "Network Analyzer", "Converters", "Gain Compression Converters" }, Description: "Gain Compression for Amplifiers GCA (Opt S9x086A 086)")]
    public class GainCompressionChannel : ConverterChannelBaseStep
    {
        #region Settings
        #endregion

        public GainCompressionChannel()
        {
            // Add child steps in the order that is required
            
            // Mixer Setup/Power/Frequency
            AddMixerChildSteps(enableSweptPowerSettings: false);

            // Compression
            Compression compression = ConfigureChildStep(new Compression());
            // Power
            MixerConverterPowerStep power = ConfigureChildStep(new MixerConverterPowerStep());
            // Frequency
            GainCompressionFrequency frequency = ConfigureChildStep(new GainCompressionFrequency());

            // Traces
            GainCompressionNewTrace gainCompressionNewTrace = ConfigureChildStep(new GainCompressionNewTrace());

            this.ChildTestSteps.Add(frequency);
            this.ChildTestSteps.Add(power);
            this.ChildTestSteps.Add(compression);
            this.ChildTestSteps.Add(gainCompressionNewTrace);

            // Once we have all child steps, lets get the number of points
            this.UpdateNumberOfPoints();
        }

        public override void Run()
        {
            DefineDummyTrace("Gain Compression Converters", "SC21");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
            //UpdateMetaData();
        }
    }
}
