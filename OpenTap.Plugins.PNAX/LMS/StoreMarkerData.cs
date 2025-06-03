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
using System.Reflection;
using System.Text;
using OpenTap;
using OpenTap.Plugins.PNAX.LMS;

//using Newtonsoft.Json;

namespace OpenTap.Plugins.PNAX
{
    //[AllowAsChildIn(typeof(TestPlan))]
    //[AllowAsChildIn(typeof(StoreData))]
    //[AllowAsChildIn(typeof(StoreDispTraceData))]
    //[AllowAsChildIn(typeof(StoreMultiPeakSearch))]
    [Display(
        "Store Marker Data",
        Groups: new[] { "Network Analyzer", "Load/Measure/Store" },
        Description: "Stores trace data from all channels.\n"
            + "This test should be placed as a child of:"
            + "\n\tStore Trace Data"
            + "\n\tStore Displayed Trace Data"
            + "\n\tStore Multi Peak Search"
            + "\n\tTest Plan"
            + "\n\tSequence"
    )]
    public class StoreMarkerData : StoreDataBase
    {
        #region Settings
        [Output]
        [Display("Marker Xs", Group: "Results", Order: 30)]
        public List<double> ListXs { get; set; }

        [Output]
        [Display("Marker Ys", Group: "Results", Order: 31)]
        public List<double> ListYs { get; set; }

        [Display("Result File Name", Group: "Publish Results", Order: 40)]
        public MacroString FileName { get; set; }

        [Display("Split Results By Channel", Group: "Publish Results", Order: 41)]
        public bool SplitByChannel { get; set; }
        #endregion

        public StoreMarkerData()
        {
            channels = new List<int>() { 1 };
            AutoSelectChannels = true;
            MetaData = new List<(string, object)>();
            ListXs = new List<double>();
            ListYs = new List<double>();
            FileName = new MacroString(this) { Text = "Channel_Markers" };
            SplitByChannel = true;
        }

        public override void Run()
        {
            ListXs = new List<double>();
            ListYs = new List<double>();
            MetaData = new List<(string, object)>();
            UpgradeVerdict(Verdict.NotSet);
            AutoSelectChannelsAvailableOnInstrument();

            // Supported child steps will provide MetaData to be added to the publish table
            RunChildSteps();

            foreach (var channel in channels)
            {
                string[] tracesList = PNAX.GetTraceNames(channel);
                //Log.Debug("TraceList: " + tracesList.ToString());
                // Create list of available traces
                var traceNumList = new List<string>();
                var FullTraceName = new List<string>();
                var allTraceNumList = new List<string>();

                for (var i = 0; i < tracesList.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        var traceNum = tracesList[i].Split('_').Last();
                        traceNumList.Add(traceNum);
                        allTraceNumList.Add(traceNum);
                        FullTraceName.Add(tracesList[i]);
                    }
                }

                // Marker information
                for (int tr = 0; tr < traceNumList.Count; tr++)
                {
                    string trace = traceNumList[tr];
                    string tracename = FullTraceName[tr];

                    Log.Info($"Looking for Markers for Channel{channel} Trace: {trace}");

                    // trace is MNUM in help file
                    PNAX.SelectMeasurement(channel, trace);

                    for (int mkr = 1; mkr < 16; mkr++)
                    {
                        // first lets find all markers that are enabled for this trace
                        bool mrkrState = PNAX.ScpiQuery<bool>(
                            $"CALCulate{channel}:MEASure{trace}:MARKer{mkr}:STATe?"
                        );

                        List<string> ResultNames = new List<string>();
                        List<IConvertible> ResultValues = new List<IConvertible>();

                        if (mrkrState)
                        {
                            // Now lets get the X value
                            double mrkrX = PNAX.ScpiQuery<double>(
                                $"CALCulate{channel}:MEASure{trace}:MARKer{mkr}:X?"
                            );

                            string measFormat = PNAX.ScpiQuery(
                                $"CALCulate{channel}:MEASure{trace}:FORMat?"
                            );
                            string mrkrFormat = PNAX.ScpiQuery(
                                $"CALCulate{channel}:MEASure{trace}:MARKer{mkr}:FORMat?"
                            );

                            double mrkrY = double.NaN;

                            // Trace Format OR Marker Format
                            if (
                                (measFormat.Contains("SMIT"))
                                || (measFormat.Contains("POL"))
                                || (mrkrFormat.Contains("POL"))
                                || (mrkrFormat.Contains("POL"))
                                || (mrkrFormat.Contains("LINP"))
                                || (mrkrFormat.Contains("LOGP"))
                            )
                            {
                                // Finally get the Y value:  (Real, Imaginary)
                                var yString = PNAX.ScpiQuery(
                                    $"CALCulate{channel}:MEASure{trace}:MARKer{mkr}:Y?"
                                );
                                var y = yString.Split(',').Select(double.Parse).ToList();
                                mrkrY = y[0];
                            }
                            else
                            {
                                // Finally get the Y value:  (Value,0)
                                var yString = PNAX.ScpiQuery(
                                    $"CALCulate{channel}:MEASure{trace}:MARKer{mkr}:Y?"
                                );
                                var y = yString.Split(',').Select(double.Parse).ToList();
                                mrkrY = y[0];
                            }

                            Log.Info($"Found Marker {mkr} X:{mrkrX} Y:{mrkrY}");

                            ResultNames.Add("Test Type");
                            ResultValues.Add("Marker");

                            ResultNames.Add("Frequency (Hz)");
                            ResultValues.Add(mrkrX);
                            ListXs.Add(mrkrX);

                            ResultNames.Add(tracename);
                            ResultValues.Add(mrkrY);
                            ListYs.Add(mrkrY);

                            //if MetaData available
                            if ((MetaData != null) && (MetaData.Count > 0))
                            {
                                // for every item in metadata
                                for (int i = 0; i < MetaData.Count; i++)
                                {
                                    ResultNames.Add(MetaData[i].Item1);
                                    ResultValues.Add((IConvertible)MetaData[i].Item2);
                                }
                            }

                            // if parent metadata
                            if (this.Parent is StoreMultiPeakSearch)
                            {
                                List<(string, object)> ParentMetaData =
                                    GetParent<StoreMultiPeakSearch>().MetaData;
                                if ((ParentMetaData != null) && (ParentMetaData.Count > 0))
                                {
                                    // for every item in metadata
                                    for (int i = 0; i < ParentMetaData.Count; i++)
                                    {
                                        ResultNames.Add(ParentMetaData[i].Item1);
                                        ResultValues.Add((IConvertible)ParentMetaData[i].Item2);
                                    }
                                }
                            }

                            string publishFileName = FileName.Expand(PlanRun);
                            if (SplitByChannel)
                            {
                                publishFileName = $"{FileName}_{channel.ToString()}";
                            }
                            Results.Publish(publishFileName, ResultNames, ResultValues.ToArray());
                        }
                    }
                }
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
