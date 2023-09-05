﻿using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;

namespace OpenTap.Plugins.PNAX
{
    [Display("Store Trace Data", Groups: new[] { "PNA-X", "Load/Measure/Store" }, Description: "Stores trace data from all channels.")]
    public class StoreData : StoreDataBase
    {
        #region Settings

        [Display("Group Results by Channel", Description: "Generate one Result file per Channel.", "Measurements", Order: 20)]
        public bool GroupByChannel { get; set; }

        [Display("Enable Limits?", "Choose if you want limit checking and Pass/Fail", "Measurements", Order: 30)]
        public bool EnableLimits { get; set; }

        [EnabledIf("EnableLimits", true, HideIfDisabled = true)]
        [Display("Limits File", "Insert .csv file containing limits to check against (*.csv)", "Measurements", Order: 40)]
        [FilePath(FilePathAttribute.BehaviorChoice.Open, "csv")]
        public string LimitsFile { get; set; }

        #endregion

        public StoreData()
        {
            channels = new List<int> { };
            // ToDo: Set default values for properties / settings.
            Rules.Add(IsFileValid, "Must be a valid file", "LimitsFile");
            EnableLimits = false;
            GroupByChannel = true;

            MetaData = new List<(string, object)>();
        }

        public override void Run()
        {
            MetaData = new List<(string, object)>();
            UpgradeVerdict(Verdict.NotSet);

            // Supported child steps will provide MetaData to be added to the publish table
            RunChildSteps(); 
            bool includePassFail = true;

            try
            {
                if (GroupByChannel)
                {
                    foreach (int channel in channels)
                    {
                        List<List<string>> results = PNAX.StoreTraceData(new List<int>() { channel }, includePassFail);
                        PNAX.WaitForOperationComplete();

                        // Grab trace number list and remove from list
                        var traces = results[0];
                        results.RemoveAt(0);

                        // Grab trace titles and remove from list
                        var allTraceTitles = results[0];
                        results.RemoveAt(0);

                        // Full trace name
                        var FullTraceName = results[0];
                        results.RemoveAt(0);

                        List<List<String>> xResult = new List<List<string>>();
                        List<List<String>> yResult = new List<List<String>>();
                        List<List<String>> pf = new List<List<String>>();
                        List<List<String>> x1 = new List<List<String>>();
                        List<List<String>> x2 = new List<List<String>>();
                        List<List<String>> x3 = new List<List<String>>();
                        List<List<String>> x4 = new List<List<String>>();

                        if (includePassFail)
                        {
                            xResult = results.Where((item, index) => ((index == 0) || ((index >= 7) && (index % 7 == 0)))).ToList();
                            yResult = results.Where((item, index) => ((index == 1) || ((index >= 8) && (index % 7 == 1)))).ToList();

                            pf = results.Where((item, index) => ((index == 2) || ((index >= 9) && (index % 7 == 2)))).ToList();

                            x1 = results.Where((item, index) => ((index == 3) || ((index >= 10) && (index % 7 == 3)))).ToList();
                            x2 = results.Where((item, index) => ((index == 4) || ((index >= 11) && (index % 7 == 4)))).ToList();
                            x3 = results.Where((item, index) => ((index == 5) || ((index >= 12) && (index % 7 == 5)))).ToList();
                            x4 = results.Where((item, index) => ((index == 6) || ((index >= 13) && (index % 7 == 6)))).ToList();
                        }
                        else
                        {
                            xResult = results.Where((item, index) => index % 2 == 0).ToList();
                            yResult = results.Where((item, index) => index % 2 != 0).ToList();
                        }
                        int freqLength = 0;

                        List<ResultColumn> resultColumns = new List<ResultColumn>();
                        for (var i = 0; i < traces.Count; i++)
                        {
                            if (i == 0)
                            {
                                freqLength = xResult[i].Count;
                                ResultColumn resultColumn = new ResultColumn("Frequency (Hz)", xResult[i].Select(double.Parse).Select(x => Math.Round(x, 2)).ToArray());
                                resultColumns.Add(resultColumn);
                            }

                            if (xResult[i].Count == yResult[i].Count)
                            {
                                // one data per frequency point
                                ResultColumn resultColumn = new ResultColumn($"{FullTraceName[i]}", yResult[i].Select(double.Parse).Select(x => Math.Round(x, 2)).ToArray());
                                resultColumns.Add(resultColumn);
                            }
                            else
                            {
                                // most likely we have complex data, i.e. two numbers per data point
                                var twoPoints = yResult[i].Select(double.Parse).Select(x => Math.Round(x, 2)).ToArray();
                                double[] point1 = new double[freqLength];
                                double[] point2 = new double[freqLength];
                                int j = 0;
                                for (int index = 0; index < freqLength; index++)
                                {
                                    point1[index] = twoPoints[j++];
                                    point2[index] = twoPoints[j++];
                                }

                                ResultColumn resultColumni = new ResultColumn($"{FullTraceName[i]}_i", point1);
                                resultColumns.Add(resultColumni);
                                ResultColumn resultColumnj = new ResultColumn($"{FullTraceName[i]}_j", point2);
                                resultColumns.Add(resultColumnj);

                            }

                            if (includePassFail)
                            {
                                // Find if limit is turned on for this trace
                                int mnum = int.Parse(FullTraceName[i].Split('_').Last());
                                bool limitON = PNAX.GetLimitTestOn(channel, mnum);

                                if (limitON)
                                {
                                    // append global pf
                                    ResultColumn resultColumn = new ResultColumn($"{FullTraceName[i]}_GlobalPF", pf[i].ToArray());
                                    resultColumns.Add(resultColumn);
                                    if (resultColumn.Data.GetValue(0).Equals("Fail"))
                                    {
                                        Log.Warning($"Trace: {FullTraceName[i]} failed limits!");
                                        UpgradeVerdict(Verdict.Fail);
                                    }

                                    // append xaxisvalues
                                    resultColumn = new ResultColumn($"{FullTraceName[i]}_XAxis", x1[i].Select(double.Parse).Select(x => Math.Round(x, 2)).ToArray());
                                    if (false)
                                    {
                                        resultColumns.Add(resultColumn);
                                    }

                                    // append pf
                                    List<String> pfByRow = new List<string>();
                                    var arraypf = x2[i].Select(double.Parse).Select(x => Math.Round(x, 2)).ToArray();
                                    foreach (var item in arraypf)
                                    {
                                        Verdict a = item == 1 ? Verdict.Pass : Verdict.Fail;
                                        pfByRow.Add(a.ToString());
                                    }
                                    resultColumn = new ResultColumn($"{FullTraceName[i]}_PF", pfByRow.ToArray());
                                    resultColumns.Add(resultColumn);

                                    // append upperlimit
                                    resultColumn = new ResultColumn($"{FullTraceName[i]}_UL", x3[i].Select(double.Parse).Select(x => Math.Round(x, 2)).ToArray());
                                    if ((double)resultColumn.Data.GetValue(0) != 3.40282346639E+38)
                                    {
                                        resultColumns.Add(resultColumn);
                                    }

                                    // append lowerlimit
                                    resultColumn = new ResultColumn($"{FullTraceName[i]}_LL", x4[i].Select(double.Parse).Select(x => Math.Round(x, 2)).ToArray());
                                    if ((double)resultColumn.Data.GetValue(0) != -3.40282346639E+38)
                                    {
                                        resultColumns.Add(resultColumn);
                                    }
                                }
                            }
                        }

                        // if MetaData available
                        if ((MetaData != null) && (MetaData.Count > 0))
                        {
                            // for every item in metadata
                            foreach(var i in MetaData)
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

                        ResultTable resultTable = new ResultTable($"Channel_{channel.ToString()}", resultColumns.ToArray());
                        Results.PublishTable(resultTable);
                    }
                }
                else
                {
                    List<List<string>> results = PNAX.StoreTraceData(channels);
                    PNAX.WaitForOperationComplete();

                    // Grab trace number list and remove from list
                    var traces = results[0];
                    results.RemoveAt(0);

                    // Grab trace titles and remove from list
                    var allTraceTitles = results[0];
                    results.RemoveAt(0);

                    // Full trace name
                    var FullTraceName = results[0];
                    results.RemoveAt(0);

                    var xResult = results.Where((item, index) => index % 2 == 0).ToList();
                    var yResult = results.Where((item, index) => index % 2 != 0).ToList();

                    var traceTitle = new List<string>();
                    var maxSpec = new List<string>();
                    var minSpec = new List<string>();
                    var max = new List<string>();
                    var min = new List<string>();
                    if (EnableLimits)
                    {
                        var allInfo = PNAX.GetMinMax(channels);
                        var allMax = allInfo.Where((item, index) => index % 2 == 0).ToList();
                        var allMin = allInfo.Where((item, index) => index % 2 != 0).ToList();

                        // Add Spec Info Table
                        string[] csvLines = File.ReadAllLines(LimitsFile);
                        foreach (var line in csvLines)
                        {
                            Log.Info(line);
                            var lineArr = line.Split(',');

                            var testParam = lineArr[0];
                            if (allTraceTitles.Contains(testParam))
                            {
                                var traceIndex = allTraceTitles.IndexOf(testParam);
                                var traceUnits = PNAX.GetUnits(testParam);
                                traceTitle.Add(allTraceTitles[traceIndex] + $" ({traceUnits})");
                                min.Add(allMin[traceIndex]);
                                max.Add(allMax[traceIndex]);
                                minSpec.Add(lineArr[1]);
                                maxSpec.Add(lineArr[2]);
                            }
                        }

                        Results.PublishTable("Information",
                            new List<string> { "Trace Name", "Actual Min", "Actual Max", "Min Spec", "Max Spec" },
                            traceTitle.ToArray(),
                            min.Select(double.Parse).Select(x => Math.Round(x, 2)).ToArray(),
                            max.Select(double.Parse).Select(x => Math.Round(x, 2)).ToArray(),
                            minSpec.ToArray(),
                            maxSpec.ToArray()
                           );

                        // Check Pass/Fail
                        bool verdict = true;
                        for (var i = 0; i < maxSpec.Count; i++)
                        {
                            if (verdict == false)
                                break;

                            if (maxSpec[i] == "-")
                                continue;

                            if (Convert.ToDouble(max[i]) > Convert.ToDouble(maxSpec[i]))
                                verdict = false;
                        }

                        for (var i = 0; i < minSpec.Count; i++)
                        {
                            if (verdict == false)
                                break;

                            if (minSpec[i] == "-")
                                continue;

                            if (Convert.ToDouble(min[i]) < Convert.ToDouble(minSpec[i]))
                                verdict = false;
                        }

                        if (verdict)
                            UpgradeVerdict(Verdict.Pass);
                        else
                            UpgradeVerdict(Verdict.Fail);
                    }

                    for (var i = 0; i < traces.Count; i++)
                    {
                        var traceUnits = PNAX.GetUnits(allTraceTitles[i]);
                        if (xResult[i].Count == yResult[i].Count)
                        {
                            // one data per frequency point
                            Results.PublishTable($"{FullTraceName[i]}",
                                                 new List<string> { "Frequency (Hz)", $"{FullTraceName[i]}" },
                                                 xResult[i].Select(double.Parse).Select(x => Math.Round(x, 2)).ToArray(),
                                                 yResult[i].Select(double.Parse).Select(x => Math.Round(x, 2)).ToArray()
                                                );
                        }
                        else
                        {
                            // most likely we have complex data, i.e. two numbers per data point
                            int freqLength = xResult[i].Count;
                            var twoPoints = yResult[i].Select(double.Parse).Select(x => Math.Round(x, 2)).ToArray();
                            double[] point1 = new double[freqLength];
                            double[] point2 = new double[freqLength];
                            int j = 0;
                            for (int index = 0; index < freqLength; index++)
                            {
                                point1[index] = twoPoints[j++];
                                point2[index] = twoPoints[j++];
                            }

                            Results.PublishTable($"{FullTraceName[i]}",
                                                 new List<string> { "Frequency (Hz)", $"{FullTraceName[i]}_i", $"{FullTraceName[i]}_j" },
                                                 xResult[i].Select(double.Parse).Select(x => Math.Round(x, 2)).ToArray(),
                                                 point1,
                                                 point2
                                                );
                        }
                    }
                }

            }
            catch (IndexOutOfRangeException ex)
            {
                throw ex;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            UpgradeVerdict(Verdict.Pass);
        }

        private bool IsFileValid()
        {
            if (string.IsNullOrEmpty(LimitsFile)) return false;

            return File.Exists(LimitsFile);
        }
    }
}
