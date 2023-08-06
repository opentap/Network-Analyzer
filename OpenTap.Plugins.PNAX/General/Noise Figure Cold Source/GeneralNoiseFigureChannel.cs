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
    [Display("Noise Figure Cold Source Channel", Groups: new[] { "PNA-X", "General", "Noise Figure Cold Source" }, Description: "Insert a description here")]
    public class GeneralNoiseFigureChannel : GeneralChannelBaseStep
    {
        #region Settings
        #endregion

        public GeneralNoiseFigureChannel()
        {
            // NoiseFigure
            GeneralNoiseFigure noiseFigure = new GeneralNoiseFigure { IsControlledByParent = true, Channel = this.Channel };
            // Power
            GeneralNoiseFigurePower power = new GeneralNoiseFigurePower { IsControlledByParent = true, Channel = this.Channel };
            // Frequency
            GeneralNoiseFigureFrequency frequency = new GeneralNoiseFigureFrequency { IsControlledByParent = true, Channel = this.Channel };

            // Trace
            GeneralNoiseFigureNewTrace noiseFigureNewTrace = new GeneralNoiseFigureNewTrace { IsControlledByParent = true, Channel = this.Channel };

            this.ChildTestSteps.Add(noiseFigure);
            this.ChildTestSteps.Add(power);
            this.ChildTestSteps.Add(frequency);
            this.ChildTestSteps.Add(noiseFigureNewTrace);

            // Once we have all child steps, lets get the number of points
            //this.UpdateNumberOfPoints();
        }

        public override void Run()
        {
            int traceid = PNAX.GetNewTraceID(Channel);
            // Define a dummy measurement so we can setup all channel parameters
            // we will add the traces during the StandardSingleTrace or StandardNewTrace test steps
            PNAX.ScpiCommand($"CALCulate{Channel.ToString()}:CUST:DEFine \'CH{Channel.ToString()}_DUMMY_NF_1\',\'Noise Figure Cold Source\',\'NF\'");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
            //UpdateMetaData();
        }
    }
}
