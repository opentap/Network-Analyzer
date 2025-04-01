// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using OpenTap;
using OpenTap.Plugins.PNAX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OpenTap.Plugins.PNAX
{
    //[AllowAsChildIn(typeof(SingleTraceBaseStep))]
    [Display("Peak Search Setup", Groups: new[] { "Network Analyzer", "Trace" }, Description: "Insert a description here")]
    public class PeakSearch : PNABaseStep
    {
        #region Settings
        [Display("Marker Number", Groups: new[] { "Trace" }, Order: 10)]
        public int mkr { get; set; }

        [Display("Peak Threshold", Groups: new[] { "Trace" }, Order: 11)]
        public double PeakThreshold { get; set; }

        [Display("Peak Excursion", Groups: new[] { "Trace" }, Order: 12)]
        public double PeakExcursion{ get; set; }

        [Display("Peak Polarity", Groups: new[] { "Trace" }, Order: 13)]
        public SAMultiPeakSearchPolarityEnumType PeakPolarity { get; set; }
        #endregion

        public PeakSearch()
        {
            IsControlledByParent = true;
            PeakThreshold = -100;
            PeakExcursion = 3;
            PeakPolarity = SAMultiPeakSearchPolarityEnumType.POS;
            mkr = 1;
        }

        public override void Run()
        {
            int mnum = GetParent<SingleTraceBaseStep>().mnum;

            PNAX.SetMarkerState(Channel, mnum, mkr, SAOnOffTypeEnum.On);
            PNAX.CalculateMeasureMarkerFunctionPeakThreshold(Channel, mnum, mkr, PeakThreshold);
            PNAX.CalculateMeasureMarkerFunctionPeakExcursion(Channel, mnum, mkr, PeakExcursion);
            PNAX.CalculateMeasureMarkerFunctionPeakPolarity(Channel, mnum, mkr, PeakPolarity);

            UpgradeVerdict(Verdict.Pass);
        }
    }

    [Display("Peak Search Execute", Groups: new[] { "Network Analyzer", "Trace" }, Description: "Insert a description here")]
    public class PeakSearchExecute : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }


        [Display("Channel", Description: "Choose which channel to grab data from.", "Measurements", Order: 10)]
        public int Channel { get; set; }

        [Display("MNum", Groups: new[] { "Trace" }, Order: 21)]
        public int mnum { get; set; }

        [Display("Marker Number", Groups: new[] { "Trace" }, Order: 30)]
        public int mkr { get; set; }


        [Display("X", Groups: new[] { "Results" }, Order: 40)]
        public double mrkrX { get; set; }

        [Display("Y", Groups: new[] { "Results" }, Order: 40)]
        public double mrkrY { get; set; }
        #endregion

        public PeakSearchExecute()
        {
            Channel = 1;
            mnum = 1;
        }

        public override void Run()
        {
            mrkrY = double.NaN;

            PNAX.SetMarkerState(Channel, mnum, mkr, SAOnOffTypeEnum.On);
            PNAX.CalculateMeasureMarkerFunctionPeak(Channel, mnum, mkr);

            mrkrX = PNAX.ScpiQuery<double>($"CALCulate{Channel}:MEASure{mnum}:MARKer{mkr}:X?");
            var yString = PNAX.ScpiQuery($"CALCulate{Channel}:MEASure{mnum}:MARKer{mkr}:Y?");
            var y = yString.Split(',').Select(double.Parse).ToList();
            mrkrY = y[0];
            Log.Info($"Found Marker {mkr} X:{mrkrX} Y:{mrkrY}");

        }

    }


}
