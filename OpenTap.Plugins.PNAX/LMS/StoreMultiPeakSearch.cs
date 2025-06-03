// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using OpenTap;

namespace OpenTap.Plugins.PNAX
{
    [Display(
        "Store Multi Peak Search",
        Groups: new[] { "Network Analyzer", "Load/Measure/Store" },
        Description: "Insert a description here"
    )]
    public class StoreMultiPeakSearch : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display(
            "Channel",
            Description: "Choose which channel to grab data from.",
            "Measurements",
            Order: 10
        )]
        public int Channel { get; set; }

        [Display("MNum", Groups: new[] { "Trace" }, Order: 21)]
        public int mnum { get; set; }

        [Browsable(false)]
        [Display("MetaData", Groups: new[] { "MetaData" }, Order: 1000.0)]
        public List<(string, object)> MetaData { get; private set; }
        #endregion

        public StoreMultiPeakSearch()
        {
            MetaData = new List<(string, object)>();
        }

        public override void Run()
        {
            // if Sweep Mode is LinFreq
            bool IsLinearFrequencySweep = false;
            List<String> sources = PNAX.SourceCatalog(Channel);
            List<String> LinFreSources = new List<string>();
            Dictionary<String, double> SourceCellSweepValues = new Dictionary<string, double>();

            // Find all enabled sources that are LinearFrequency Sweep type
            foreach (String source in sources)
            {
                SASourceSweepTypeEnum sweepType = PNAX.GetSASweepType(Channel, source);
                SAOnOffTypeEnum state = PNAX.GetSASourcePowerMode(Channel, source);
                if (
                    (sweepType == SASourceSweepTypeEnum.LinearFrequency)
                    && (state == SAOnOffTypeEnum.On)
                )
                {
                    IsLinearFrequencySweep = true;
                    LinFreSources.Add(source);

                    double start = PNAX.GetSAFrequencyStart(Channel, source);
                    double stop = PNAX.GetSAFrequencyStop(Channel, source);
                    SourceCellSweepValues.Add($"{source}_start", start);
                    SourceCellSweepValues.Add($"{source}_stop", stop);
                }
            }

            if (IsLinearFrequencySweep)
            {
                // get values
                int steps = PNAX.GetSAFrequencySteps(Channel);

                //  setup manual trigger
                PNAX.SetTriggerSource(TriggerSourceEnumType.MAN);
                // set sweep trigger mode
                PNAX.SetTriggerMode(Channel, TriggerModeEnumType.SWE);
                // set single trigger
                PNAX.SetSweepMode(Channel, SweepModeEnumType.SING);

                // for each sweep rep
                int reps = PNAX.GetSAFrequencySteps(Channel);
                for (int rep = 0; rep < reps; rep++)
                {
                    Log.Info($"Rep {rep} of {reps}");
                    PNAX.SendTrigger(1);
                    PNAX.WaitForOperationComplete();
                    // Execute Multi peak search
                    PNAX.MultiPeakSearchExecute(Channel, mnum);

                    // Store Markers
                    MetaData = new List<(string, object)> { ("Sweep Step", rep + 1) };
                    foreach (string source in LinFreSources)
                    {
                        MetaData.Add(
                            (
                                $"{source} Freq",
                                GetCurrentSweepFreq(
                                    SourceCellSweepValues[$"{source}_start"],
                                    SourceCellSweepValues[$"{source}_stop"],
                                    steps,
                                    rep
                                )
                            )
                        );
                    }
                    RunChildSteps();
                }
            }
            else
            {
                // Execute Multi peak search
                PNAX.MultiPeakSearchExecute(Channel, mnum);

                // Grab all markers and publish
                RunChildSteps();
            }

            UpgradeVerdict(Verdict.Pass);
        }

        private double GetCurrentSweepFreq(double start, double stop, int steps, int currentStep)
        {
            double stepsize = (stop - start) / (steps - 1);
            return start + (currentStep * stepsize);
        }
    }
}
