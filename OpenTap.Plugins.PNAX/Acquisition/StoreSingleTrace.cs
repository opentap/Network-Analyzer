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
    [Display("Store Single Trace", Groups: new[] { "PNA-X", "Acquisition" }, Description: "Stores trace data for a single given trace")]
    public class StoreSingleTraceAdvanced : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }


        [Display("Channel", Description: "Choose which channel to grab data from.", "Measurements", Order: 10)]
        public Input<int> Channel { get; set; }

        [Display("MNum", Groups: new[] { "Trace" }, Order: 21)]
        public Input<int> mnum { get; set; }
        #endregion

        public StoreSingleTraceAdvanced()
        {
            Channel = new Input<int>();
            mnum = new Input<int>();
        }

        public override void Run()
        {
            Log.Info("Channel from trace: " + Channel);
            Log.Info("MNUM from trace: " + mnum);

            SingleTraceBaseStep x = (mnum.Step as SingleTraceBaseStep);

            Log.Info("trace Window: " );
            Log.Info("trace Window: " + x.Window);
            Log.Info("trace Sheet: " + x.Sheet);
            Log.Info("trace tnum: " + x.tnum);
            Log.Info("trace mnum: " + x.mnum);
            Log.Info("trace MeasName: " + x.MeasName);

            UpgradeVerdict(Verdict.NotSet);

            RunChildSteps(); //If the step supports child steps.

            List<List<string>> results = PNAX.StoreTraceData(Channel.Value, mnum.Value);
            PNAX.WaitForOperationComplete();

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
                ResultColumn resultColumn2 = new ResultColumn($"{x.MeasName}", yResult[0].Select(double.Parse).Select(z => Math.Round(z, 2)).ToArray());
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

                ResultColumn resultColumni = new ResultColumn($"{x.MeasName}_i", point1);
                resultColumns.Add(resultColumni);
                ResultColumn resultColumnj = new ResultColumn($"{x.MeasName}_j", point2);
                resultColumns.Add(resultColumnj);

            }

            ResultTable resultTable = new ResultTable($"Channel_{Channel.ToString()}", resultColumns.ToArray());
            Results.PublishTable(resultTable);

            UpgradeVerdict(Verdict.Pass);

        }
    }
}
