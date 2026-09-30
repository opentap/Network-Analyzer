// Author: MyName
// Copyright:   Copyright 2026 Keysight Technologies
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

namespace OpenTap.Plugins.PNAX.General.Standard
{
    [Display("Trigger Single Sweep", Groups: new[] { "Network Analyzer", "General", "Standard" }, Description: "Triggers a single sweep on VNA, may hold test step until complete")]
    public class TriggerSingleSweep : PNABaseStep
    {
        #region Settings
        [Display("Hold Until Complete", Group: "Settings", Order:2, Description: "If true, the test step will wait until the sweep is complete before proceeding to the next step.")]
        public bool HoldUntilComplete { get; set; } = true;
        [Display("Polling Interval", Group: "Settings", Order:2, Description: "When 'Hold Until Complete' is true, this sets the interval at which the step checks if the sweep is complete.")]
        [Unit("s",UseEngineeringPrefix:true)]
        public double PollingIntervalSeconds { get; set; } = 0.1;
        [Display("Timeout", Group: "Settings", Order:2, Description: "When 'Hold Until Complete' is true, this sets the maximum time to wait for the sweep to complete before throwing a timeout exception.")]
        [Unit("s", UseEngineeringPrefix: true)]
        public double TimeoutSeconds { get; set; } = 30;
        #endregion

        public TriggerSingleSweep()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            PNAX.SetSweepMode(Channel, SweepModeEnumType.SING);
            if (HoldUntilComplete)
            {
                DateTime startTime = DateTime.Now;
                while (PNAX.GetSweepStatus(Channel) == SweepModeEnumType.SING)
                {
                    if ((DateTime.Now - startTime).TotalSeconds > TimeoutSeconds)
                    {
                        throw new TimeoutException("Timeout waiting for sweep to complete.");
                    }
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(PollingIntervalSeconds));
                }
            }
            Verdict = Verdict.Pass;
        }
    }
}
