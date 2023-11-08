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
    [Display("Standard Channel", Groups: new[] { "PNA-X", "General",  "Standard" }, Description: "Insert a description here")]
    public class StandardChannel : GeneralChannelBaseStep
    {
        #region Settings
        #endregion

        public StandardChannel()
        {
            Channel = 1;

            // Sweep Type
            SweepType sweepType = new SweepType { IsControlledByParent = true, Channel = this.Channel };
            // Timing
            Timing timing = new Timing { IsControlledByParent = true, Channel = this.Channel };
            // Traces
            StandardNewTrace standardNewTrace = new StandardNewTrace { IsControlledByParent = true, Channel = this.Channel };

            this.ChildTestSteps.Add(sweepType);
            this.ChildTestSteps.Add(timing);
            this.ChildTestSteps.Add(standardNewTrace);
        }

        // Overloaded Constructor for Test Plan Generator
        public StandardChannel(int Ch, StandardSweepTypeEnum sweeptype, double Start, double Stop, double Power, double CW, int Points, double IFBW, List<StandardTrace> standardTraces)
        {
            Channel = Ch;

            // Sweep Type
            SweepType sweepTypeChildStep = new SweepType
            {
                IsControlledByParent = true,
                Channel = this.Channel,
                SweepPropertiesPoints = Points,
                SweepPropertiesIFBandwidth = IFBW
            };

            sweepTypeChildStep.StandardSweepType = sweeptype;
            if (sweeptype == StandardSweepTypeEnum.LinearFrequency || sweeptype == StandardSweepTypeEnum.LogFrequency)
            {
                sweepTypeChildStep.SweepPropertiesStart = Start;
                sweepTypeChildStep.SweepPropertiesStop = Stop;
                sweepTypeChildStep.SweepPropertiesPower = Power;
            }
            else if (sweeptype == StandardSweepTypeEnum.PowerSweep)
            {
                sweepTypeChildStep.SweepPropertiesStartPower = Start;
                sweepTypeChildStep.SweepPropertiesStopPower = Stop;
                sweepTypeChildStep.SweepPropertiesCWFreq = CW;
            }
            else if (sweeptype == StandardSweepTypeEnum.CWTime)
            {
                sweepTypeChildStep.SweepPropertiesPower = Power;
                sweepTypeChildStep.SweepPropertiesCWFreq = CW;
            }
            else if (sweeptype == StandardSweepTypeEnum.PhaseSweep)
            {
                sweepTypeChildStep.SweepPropertiesStartPhase = Start;
                sweepTypeChildStep.SweepPropertiesStopPhase = Stop;
                sweepTypeChildStep.SweepPropertiesCWFreq = CW;
            }

            // Timing
            Timing timing = new Timing { IsControlledByParent = true, Channel = this.Channel };
            // Traces
            StandardNewTrace standardNewTrace = new StandardNewTrace(standardTraces) { IsControlledByParent = true, Channel = this.Channel };

            this.ChildTestSteps.Add(sweepTypeChildStep);
            this.ChildTestSteps.Add(timing);
            this.ChildTestSteps.Add(standardNewTrace);
        }

        public override void Run()
        {
            int traceid = PNAX.GetNewTraceID(Channel);
            // Define a dummy measurement so we can setup all channel parameters
            // we will add the traces during the StandardSingleTrace or StandardNewTrace test steps
            PNAX.ScpiCommand($"CALCulate{Channel}:CUST:DEFine \'CH{Channel}_DUMMY_1\',\'Standard\',\'S11\'");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
            UpdateMetaData();
        }
    }
}
