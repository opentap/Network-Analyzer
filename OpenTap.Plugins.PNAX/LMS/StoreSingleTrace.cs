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
    [Display("Store Single Trace", Groups: new[] { "Network Analyzer", "Load/Measure/Store" }, Description: "Stores trace data for a single given trace")]
    public class StoreSingleTrace : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }


        [Display("Channel", Description: "Choose which channel to grab data from.", "Measurements", Order: 10)]
        public int Channel { get; set; }

        [Display("MNum", Groups: new[] { "Trace" }, Order: 21)]
        public int mnum { get; set; }

        [Display("File Name", Groups: new[] { "Trace" }, Order: 22)]
        public string filename { get; set; }
        #endregion

        public StoreSingleTrace()
        {
            Channel = 1;
            mnum = 1;
            filename = "MyTrace";
        }

        public override void Run()
        {
            Log.Info("Channel from trace: " + Channel);
            Log.Info("MNUM from trace: " + mnum);

            UpgradeVerdict(Verdict.NotSet);

            RunChildSteps(); //If the step supports child steps.

            List<List<string>> results = PNAX.StoreTraceData(Channel, mnum);
            PNAX.WaitForOperationComplete();
            string MeasName = PNAX.GetTraceName(Channel, mnum);

            var xResult = results.Where((item, index) => index % 2 == 0).ToList();
            var yResult = results.Where((item, index) => index % 2 != 0).ToList();

            int freqLength = 0;

            List<ResultColumn> resultColumns = new List<ResultColumn>();

            freqLength = xResult[0].Count;
            ResultColumn resultColumn = new ResultColumn("Frequency (Hz)", xResult[0].Select(double.Parse).Select(z => Math.Round(z, 2)).ToArray());
            resultColumns.Add(resultColumn);

            if (xResult[0].Count == yResult[0].Count)
            {
                // one data per frequency point
                ResultColumn resultColumn2 = new ResultColumn($"{MeasName}", yResult[0].Select(double.Parse).Select(z => Math.Round(z, 2)).ToArray());
                resultColumns.Add(resultColumn2);
            }
            else
            {
                // most likely we have complex data, i.e. two numbers per data point
                var twoPoints = yResult[0].Select(double.Parse).Select(z => Math.Round(z, 2)).ToArray();
                double[] point1 = new double[freqLength];
                double[] point2 = new double[freqLength];
                int j = 0;
                for (int index = 0; index < freqLength; index++)
                {
                    point1[index] = twoPoints[j++];
                    point2[index] = twoPoints[j++];
                }

                ResultColumn resultColumni = new ResultColumn($"{MeasName}_i", point1);
                resultColumns.Add(resultColumni);
                ResultColumn resultColumnj = new ResultColumn($"{MeasName}_j", point2);
                resultColumns.Add(resultColumnj);

            }

            ResultTable resultTable = new ResultTable($"{filename}", resultColumns.ToArray());
            Results.PublishTable(resultTable);

            UpgradeVerdict(Verdict.Pass);

        }
    }
}
