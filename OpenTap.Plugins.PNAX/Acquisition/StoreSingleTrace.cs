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
    [Display("Store Single Trace", Groups: new[] { "Network Analyzer", "Acquisition" }, Description: "Stores trace data for a single given trace")]
    public class StoreSingleTraceAdvanced : StoreDataBase
    {
        #region Settings
        //[Display("PNA", Order: 0.1)]
        //public PNAX PNAX { get; set; }

        [Browsable(false)]
        [Display("Auto Select All Channels", Group: "Measurements", Order: 10)]
        override public bool AutoSelectChannels { get; set; } = true;

        [Display("MNum", Groups: new[] { "Trace" }, Order: 21)]
        public Input<int> mnum { get; set; }

        [Display("Use Trace Title as Column Name", Groups: new[] { "Publish Results" }, Order: 30)]
        public bool UseTraceTitle { get; set; }

        #endregion

        public StoreSingleTraceAdvanced()
        {
            mnum = new Input<int>();
            UseTraceTitle = false;
            MetaData = new List<(string, object)>();
        }

        public override void Run()
        {
            MetaData = new List<(string, object)>();
            // Supported child steps will provide MetaData to be added to the publish table
            RunChildSteps();

            Log.Info("MNUM from trace: " + mnum);

            SingleTraceBaseStep inputTrace = (mnum.Step as SingleTraceBaseStep);

            Log.Info("trace Window: " );
            Log.Info("trace Channel: " + inputTrace.Channel);
            Log.Info("trace Window: " + inputTrace.Window);
            Log.Info("trace Sheet: " + inputTrace.Sheet);
            Log.Info("trace tnum: " + inputTrace.tnum);
            Log.Info("trace mnum: " + inputTrace.mnum);
            Log.Info("trace MeasName: " + inputTrace.MeasName);

            int Channel = inputTrace.Channel;
            int mnumValue = inputTrace.mnum;

            UpgradeVerdict(Verdict.NotSet);

            RunChildSteps(); //If the step supports child steps.

            List<List<string>> results = PNAX.StoreTraceData(Channel, mnumValue);
            PNAX.WaitForOperationComplete();
            string MeasName = inputTrace.MeasName;

            if (UseTraceTitle)
            {
                MeasName = PNAX.GetTraceTitle(Channel, mnumValue, MeasName);
            }

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

            // Find if limit is turned on for this trace
            bool limitON = PNAX.GetLimitTestOn(Channel, mnumValue);

            if (limitON)
            {
                string strPF = PNAX.GetPF(Channel, mnumValue);
                List<string> pf = new List<string>();
                // Create a list of n values all equal to strPF
                string verdict = strPF.Equals("0") ? Verdict.Pass.ToString() : Verdict.Fail.ToString();
                for (int pfIndex = 0; pfIndex < freqLength; pfIndex++)
                {
                    pf.Add(verdict);
                }

                var limitReportAllStr = PNAX.GetLimits(Channel, mnumValue);
                var limitReportAll = limitReportAllStr.Split(',').ToList();
                var x1 = limitReportAll.Where((item, index) => ((index == 0) || ((index >= 4) && (index % 4 == 0)))).ToList();
                var x2 = limitReportAll.Where((item, index) => ((index == 1) || ((index >= 5) && (index % 4 == 1)))).ToList();
                var x3 = limitReportAll.Where((item, index) => ((index == 2) || ((index >= 6) && (index % 4 == 2)))).ToList();
                var x4 = limitReportAll.Where((item, index) => ((index == 3) || ((index >= 7) && (index % 4 == 3)))).ToList();

                // append global pf
                resultColumn = new ResultColumn($"{MeasName}_GlobalPF", pf.ToArray());
                resultColumns.Add(resultColumn);
                if (resultColumn.Data.GetValue(0).Equals("Fail"))
                {
                    Log.Warning($"Trace: {MeasName} failed limits!");
                    UpgradeVerdict(Verdict.Fail);
                }

                // append xaxisvalues
                //resultColumn = new ResultColumn($"{MeasName}_XAxis", x1[i].Select(double.Parse).Select(x => Math.Round(x, 2)).ToArray());
                //if (false)
                //{
                //    resultColumns.Add(resultColumn);
                //}

                // append pf
                List<string> pfByRow = new List<string>();
                var arraypf = x2.Select(double.Parse).Select(x => Math.Round(x, 2)).ToArray();
                foreach (var item in arraypf)
                {
                    Verdict a = item == 1 ? Verdict.Pass : Verdict.Fail;
                    pfByRow.Add(a.ToString());
                }
                resultColumn = new ResultColumn($"{MeasName}_PF", pfByRow.ToArray());
                resultColumns.Add(resultColumn);

                // append upperlimit
                resultColumn = new ResultColumn($"{MeasName}_UL", x3.Select(double.Parse).Select(x => Math.Round(x, 2)).ToArray());
                if ((double)resultColumn.Data.GetValue(0) != 3.40282346639E+38)
                {
                    resultColumns.Add(resultColumn);
                }

                // append lowerlimit
                resultColumn = new ResultColumn($"{MeasName}_LL", x4.Select(double.Parse).Select(x => Math.Round(x, 2)).ToArray());
                if ((double)resultColumn.Data.GetValue(0) != -3.40282346639E+38)
                {
                    resultColumns.Add(resultColumn);
                }
            }

            // if MetaData available
            if ((MetaData != null) && (MetaData.Count > 0))
            {
                // for every item in metadata
                foreach (var i in MetaData)
                {
                    object[] objMetaData = new object[freqLength];
                    for (int data = 0; data < freqLength; data++)
                    {
                        objMetaData[data] = i.Item2;
                    }
                    ResultColumn resultColumnMeta = new ResultColumn(i.Item1, objMetaData);

                    // create a new column with Rows = lastColumn.length
                    // column name = metadata description
                    // every element should have the same metadata value
                    // append column to resultColumns
                    resultColumns.Add(resultColumnMeta);
                }
            }


            // Publish Table
            ResultTable resultTable = new ResultTable($"{MeasName}", resultColumns.ToArray());
            Results.PublishTable(resultTable);

            UpgradeVerdict(Verdict.Pass);

        }
    }
}
