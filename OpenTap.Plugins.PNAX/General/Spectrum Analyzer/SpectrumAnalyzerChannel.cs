﻿// Author: MyName
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

namespace OpenTap.Plugins.PNAX.General.Spectrum_Analyzer
{
    [Display("Sectrum Analyzer Channel", Groups: new[] { "PNA-X", "General", "Spectrum Analyzer" }, Description: "Insert a description here")]
    public class SpectrumAnalyzerChannel : GeneralChannelBaseStep
    {
        #region Settings
        #endregion

        public SpectrumAnalyzerChannel()
        {
            Channel = 1;

            // SA Setup
            SASetup saSetup = new SASetup { IsControlledByParent = true, Channel = this.Channel };
            SASource saSource = new SASource { IsControlledByParent = true, Channel = this.Channel };
            SANewTrace saNewTrace = new SANewTrace { IsControlledByParent = true, Channel = this.Channel };

            this.ChildTestSteps.Add(saSetup);
            this.ChildTestSteps.Add(saSource);
            this.ChildTestSteps.Add(saNewTrace);

        }

        public override void Run()
        {
            int traceid = PNAX.GetNewTraceID(Channel);
            // Define a dummy measurement so we can setup all channel parameters
            // we will add the traces during the StandardSingleTrace or StandardNewTrace test steps
            PNAX.ScpiCommand($"CALCulate{Channel.ToString()}:CUST:DEFine \'CH{Channel.ToString()}_DUMMY_1\',\'Spectrum Analyzer\',\'B\'");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
            //UpdateMetaData();
        }
    }
}