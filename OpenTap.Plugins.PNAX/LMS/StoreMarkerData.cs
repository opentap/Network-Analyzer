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
using System.Reflection;
using System.Text;
using OpenTap.Plugins.PNAX.LMS;
//using Newtonsoft.Json;

namespace OpenTap.Plugins.PNAX
{
    [AllowAsChildIn(typeof(TestPlan))]
    [AllowAsChildIn(typeof(StoreData))]
    [AllowAsChildIn(typeof(StoreDispTraceData))]
    [AllowAsChildIn(typeof(StoreMultiPeakSearch))]
    [Display("Store Marker Data", Groups: new[] { "Network Analyzer", "Load/Measure/Store" }, Description: "Stores trace data from all channels.")]
    public class StoreMarkerData : StoreDataBase
    {
        #region Settings
        #endregion

        public StoreMarkerData()
        {
            channels = new List<int> { };
            MetaData = new List<(string, object)>();
        }

        public override void Run()
        {
            MetaData = new List<(string, object)>();
            UpgradeVerdict(Verdict.NotSet);

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
                        bool mrkrState = PNAX.ScpiQuery<bool>($"CALCulate{channel}:MEASure{trace}:MARKer{mkr}:STATe?");

                        List<string> ResultNames = new List<string>();
                        List<IConvertible> ResultValues = new List<IConvertible>();

                        if (mrkrState)
                        {
                            // Now lets get the X value
                            double mrkrX = PNAX.ScpiQuery<double>($"CALCulate{channel}:MEASure{trace}:MARKer{mkr}:X?");

                            string measFormat = PNAX.ScpiQuery($"CALCulate{channel}:MEASure{trace}:FORMat?");
                            string mrkrFormat = PNAX.ScpiQuery($"CALCulate{channel}:MEASure{trace}:MARKer{mkr}:FORMat?");

                            double mrkrY = double.NaN;

                            // Trace Format OR Marker Format
                            if ((measFormat.Contains("SMIT")) ||
                                (measFormat.Contains("POL")) ||
                                (mrkrFormat.Contains("POL")) ||
                                (mrkrFormat.Contains("POL")) ||
                                (mrkrFormat.Contains("LINP")) ||
                                (mrkrFormat.Contains("LOGP")))
                            {
                                // Finally get the Y value:  (Real, Imaginary)
                                var yString = PNAX.ScpiQuery($"CALCulate{channel}:MEASure{trace}:MARKer{mkr}:Y?");
                                var y = yString.Split(',').Select(double.Parse).ToList();
                                mrkrY = y[0];

                            }
                            else
                            {
                                // Finally get the Y value:  (Value,0)
                                var yString = PNAX.ScpiQuery($"CALCulate{channel}:MEASure{trace}:MARKer{mkr}:Y?");
                                var y = yString.Split(',').Select(double.Parse).ToList();
                                mrkrY = y[0];
                            }

                            Log.Info($"Found Marker {mkr} X:{mrkrX} Y:{mrkrY}");

                            ResultNames.Add("Test Type");
                            ResultValues.Add("Marker");

                            ResultNames.Add("Frequency (Hz)");
                            ResultValues.Add(mrkrX);

                            ResultNames.Add(tracename);
                            ResultValues.Add(mrkrY);

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
                                List<(string, object)> ParentMetaData = GetParent<StoreMultiPeakSearch>().MetaData;
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
                            Results.Publish($"Channel_Markers_{channel.ToString()}", ResultNames, ResultValues.ToArray());

                        }


                    }
                }
            }

            UpgradeVerdict(Verdict.Pass);
        }

    }

}
