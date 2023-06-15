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
    [Display("Store Marker Data", Groups: new[] { "PNA-X", "Load/Measure/Store" }, Description: "Stores trace data from all channels.")]
    public class StoreMarkerData : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }


        [Display("Channels", Description: "Choose which channels to grab data from.", "Measurements", Order: 10)]
        public List<int> channels { get; set; }
        #endregion

        public StoreMarkerData()
        {
            channels = new List<int> { };
        }

        public override void Run()
        {
            UpgradeVerdict(Verdict.NotSet);

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
                    PNAX.ScpiCommand($"CALC{channel}:PAR:MNUM:SEL {trace}");

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

                            String measFormat = PNAX.ScpiQuery($"CALCulate{channel}:MEASure{trace}:FORMat?");
                            String mrkrFormat = PNAX.ScpiQuery($"CALCulate{channel}:MEASure{trace}:MARKer{mkr}:FORMat?");

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

                            Results.Publish($"Channel_Markers_{channel.ToString()}", ResultNames, ResultValues.ToArray());

                        }


                    }
                }
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
