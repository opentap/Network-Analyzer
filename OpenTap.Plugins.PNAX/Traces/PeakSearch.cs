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
    [Display("Peak Search", Groups: new[] { "Network Analyzer", "Trace" }, Description: "Insert a description here")]
    public class PeakSearch : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display("Use Trace Output", Description: "Select a trace to obtain the values of: Channel, Window, TNUM and MNUM", Groups: new[] { "Measurements" }, Order: 10)]
        public bool UseTraceOutput { get; set; }

        [EnabledIf("UseTraceOutput", true, HideIfDisabled = true)]
        [Display("MNum", Groups: new[] { "Trace" }, Order: 20)]
        public Input<int> mnum { get; set; }

        [EnabledIf("UseTraceOutput", false, HideIfDisabled = true)]
        [Display("Channel", Description: "Choose which channel to grab data from.", "Measurements", Order: 11)]
        public int Channel { get; set; }

        [Display("Window", Groups: new[] { "Trace" }, Order: 21)]
        public int Window { get; set; }

        [EnabledIf("UseTraceOutput", false, HideIfDisabled = true)]
        [Display("TNum", Groups: new[] { "Trace" }, Order: 22)]
        public int tnum { get; set; }

        [EnabledIf("UseTraceOutput", false, HideIfDisabled = true)]
        [Display("MNum Value", Groups: new[] { "Trace" }, Order: 23)]
        public int mnumValue { get; set; }

        [Display("Marker Number", Groups: new[] { "Trace" }, Order: 30)]
        public int mkr { get; set; }

        [Display("Peak Threshold", Groups: new[] { "Trace" }, Order: 31)]
        public double PeakThreshold { get; set; }

        [Display("Peak Excursion", Groups: new[] { "Trace" }, Order: 32)]
        public double PeakExcursion { get; set; }

        [Display("Peak Polarity", Groups: new[] { "Trace" }, Order: 33)]
        public SAMultiPeakSearchPolarityEnumType PeakPolarity { get; set; }


        [Display("Publish Results", Groups: new[] { "Results" }, Order: 40)]
        public bool PublishResults { get; set; }

        [EnabledIf("PublishResults", true, HideIfDisabled = true)]
        [Display("Result File Name", Group: "Results", Order: 41)]
        public MacroString FileName { get; set; }

        [Display("X", Groups: new[] { "Results" }, Order: 51)]
        public double mrkrX { get; set; }

        [Display("Y", Groups: new[] { "Results" }, Order: 52)]
        public double mrkrY { get; set; }
        #endregion

        public PeakSearch()
        {
            UseTraceOutput = true;

            mnum = new Input<int>();

            Channel = 1;
            Window = 1;
            mnumValue = 1;
            tnum = 1;
            mkr = 1;

            PeakThreshold = -100;
            PeakExcursion = 3;
            PeakPolarity = SAMultiPeakSearchPolarityEnumType.POS;

            PublishResults = true;
            FileName = new MacroString(this) { Text = "PeakSearch_Markers" };
        }

        public override void Run()
        {
            mrkrY = double.NaN;
            string MeasName = "";

            if (UseTraceOutput)
            {
                if (mnum == null)
                {
                    Log.Error("Make sure to select a trace");
                    UpgradeVerdict(Verdict.Error);
                }

                // Get the values from the input
                SingleTraceBaseStep x = (mnum.Step as SingleTraceBaseStep);

                Log.Info("trace Window: ");
                Log.Info("trace Channel: " + x.Channel);
                Log.Info("trace Window: " + x.Window);
                Log.Info("trace Sheet: " + x.Sheet);
                Log.Info("trace tnum: " + x.tnum);
                Log.Info("trace mnum: " + x.mnum);
                Log.Info("trace MeasName: " + x.MeasName);

                Channel = x.Channel;
                Window = x.Window;
                mnumValue = x.mnum;
                tnum = x.tnum;
                MeasName = x.MeasName;
            }
            else
            {
                MeasName = PNAX.GetMeasurementName(mnumValue);
            }

            // Set Marker state
            PNAX.SetMarkerState(Channel, mnumValue, mkr, SAOnOffTypeEnum.On);

            // Set search settings
            PNAX.CalculateMeasureMarkerFunctionPeakThreshold(Channel, mnumValue, mkr, PeakThreshold);
            PNAX.CalculateMeasureMarkerFunctionPeakExcursion(Channel, mnumValue, mkr, PeakExcursion);
            PNAX.CalculateMeasureMarkerFunctionPeakPolarity(Channel, mnumValue, mkr, PeakPolarity);

            // Execute Peak Search
            PNAX.CalculateMeasureMarkerFunctionPeak(Channel, mnumValue, mkr);

            // Get the marker value
            MeasName = PNAX.GetTraceTitle(Channel, mnumValue, MeasName);
            mrkrX = PNAX.ScpiQuery<double>($"CALCulate{Channel}:MEASure{mnumValue}:MARKer{mkr}:X?");
            var yString = PNAX.ScpiQuery($"CALCulate{Channel}:MEASure{mnumValue}:MARKer{mkr}:Y?");
            var y = yString.Split(',').Select(double.Parse).ToList();
            mrkrY = y[0];
            Log.Info($"Found Marker {mkr} for Trace: {MeasName}, X:{mrkrX} Y:{mrkrY}");



            // publish it
            if (PublishResults)
            {
                string publishFileName = FileName.Expand(PlanRun);
                List<string> ResultNames = new List<string>();
                List<IConvertible> ResultValues = new List<IConvertible>();

                ResultNames.Add("Test Type");
                ResultValues.Add("Marker");

                ResultNames.Add("Frequency (Hz)");
                ResultValues.Add(mrkrX);

                ResultNames.Add(MeasName);
                ResultValues.Add(mrkrY);

                Results.Publish(publishFileName, ResultNames, ResultValues.ToArray());
            }

            UpgradeVerdict(Verdict.Pass);
        }

    }


}
