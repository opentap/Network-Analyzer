// Author: MyName
// Copyright:   Copyright 2021 Keysight Technologies
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
    [Display("Trigger Channels", Groups: new[] { "PNA-X", "L / M / S" }, Description: "Triggers every channel")]
    public class Measure : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display("Channels", Description: "Choose which channels to trigger.", "Measurements")]
        public List<int> channels { get; set; }
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public Measure()
        {
            channels = new List<int> { };
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            UpgradeVerdict(Verdict.NotSet);

            try
            {
                PNAX.MeasureState(channels);
                PNAX.WaitForOperationComplete();
            }
            catch (IndexOutOfRangeException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
