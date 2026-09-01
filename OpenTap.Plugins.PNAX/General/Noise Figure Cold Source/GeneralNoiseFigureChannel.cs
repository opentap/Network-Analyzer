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
    [Display("Noise Figure Cold Source Channel", Groups: new[] { "Network Analyzer", "General", "Noise Figure Cold Source" }, Description: "Insert a description here")]
    public class GeneralNoiseFigureChannel : PNABaseStep
    {
        #region Settings
        #endregion

        public GeneralNoiseFigureChannel()
        {
            IsControlledByParent = false;

            // NoiseFigure
            GeneralNoiseFigure noiseFigure = ConfigureChildStep(new GeneralNoiseFigure());
            // Power
            GeneralNoiseFigurePower power = ConfigureChildStep(new GeneralNoiseFigurePower());
            // Frequency
            GeneralNoiseFigureFrequency frequency = ConfigureChildStep(new GeneralNoiseFigureFrequency());
            // Trace
            GeneralNoiseFigureNewTrace noiseFigureNewTrace = ConfigureChildStep(new GeneralNoiseFigureNewTrace());

            this.ChildTestSteps.Add(noiseFigure);
            this.ChildTestSteps.Add(power);
            this.ChildTestSteps.Add(frequency);
            this.ChildTestSteps.Add(noiseFigureNewTrace);

            // Once we have all child steps, lets get the number of points
            //this.UpdateNumberOfPoints();
        }

        public override void Run()
        {
            DefineDummyTrace("Noise Figure Cold Source", "NF");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
