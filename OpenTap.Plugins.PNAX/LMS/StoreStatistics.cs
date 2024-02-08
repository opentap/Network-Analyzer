// Author: CMontes
// Copyright:   Copyright 2024 Keysight Technologies
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
using OpenTap.Plugins.PNAX.LMS;

namespace OpenTap.Plugins.PNAX
{
    [Display("Store Statistics Data", Groups: new[] { "Network Analyzer", "Load/Measure/Store" }, Description: "Stores trace data from all channels.")]
    public class StoreStatistics : StoreDataBase
    {
        #region Settings

        [Display("MNum", Groups: new[] { "Trace" }, Order: 20)]
        public int mnum { get; set; }

        [Display("All Statistics", Group: "Settings", Order: 30)]
        public bool AllData { get; set; }

        [EnabledIf("AllData", false, HideIfDisabled = true)]
        [Display("Statistic Type", Group: "Settings", Order: 31)]
        public MathStatisticTypeEnum MathStatisticType { get; set; }

        #endregion

        public StoreStatistics()
        {
            channels = new List<int>() { 1 };
            AllData = true;
            MathStatisticType = MathStatisticTypeEnum.Ptp;
        }

        public override void Run()
        {
            MetaData = new List<(string, object)>();
            UpgradeVerdict(Verdict.NotSet);

            RunChildSteps(); //If the step supports child steps.

            double result;

            if (AllData)
            {
                MathStatisticType = MathStatisticTypeEnum.Ptp | MathStatisticTypeEnum.Std | MathStatisticTypeEnum.Mean | MathStatisticTypeEnum.Min | MathStatisticTypeEnum.Max;
            }

            foreach (int Channel in channels)
            {
                List<string> ResultNames = new List<string>();
                List<IConvertible> ResultValues = new List<IConvertible>();

                if (MathStatisticType.HasFlag(MathStatisticTypeEnum.Ptp))
                {
                    PNAX.MathExecuteStatistics(Channel, mnum);
                    PNAX.MathType(Channel, mnum, MathStatisticTypeEnum.Ptp);
                    result = PNAX.MathData(Channel, mnum);
                    Log.Info($"Peak to Peak: {result}");
                    ResultNames.Add(MathStatisticTypeEnum.Ptp.ToString());
                    ResultValues.Add((IConvertible)result);
                }
                if (MathStatisticType.HasFlag(MathStatisticTypeEnum.Std))
                {
                    PNAX.MathExecuteStatistics(Channel, mnum);
                    PNAX.MathType(Channel, mnum, MathStatisticTypeEnum.Std);
                    result = PNAX.MathData(Channel, mnum);
                    Log.Info($"Std dev: {result}");
                    ResultNames.Add(MathStatisticTypeEnum.Std.ToString());
                    ResultValues.Add((IConvertible)result);
                }
                if (MathStatisticType.HasFlag(MathStatisticTypeEnum.Mean))
                {
                    PNAX.MathExecuteStatistics(Channel, mnum);
                    PNAX.MathType(Channel, mnum, MathStatisticTypeEnum.Mean);
                    result = PNAX.MathData(Channel, mnum);
                    Log.Info($"Mean: {result}");
                    ResultNames.Add(MathStatisticTypeEnum.Mean.ToString());
                    ResultValues.Add((IConvertible)result);
                }
                if (MathStatisticType.HasFlag(MathStatisticTypeEnum.Min))
                {
                    PNAX.MathExecuteStatistics(Channel, mnum);
                    PNAX.MathType(Channel, mnum, MathStatisticTypeEnum.Min);
                    result = PNAX.MathData(Channel, mnum);
                    Log.Info($"Min: {result}");
                    ResultNames.Add(MathStatisticTypeEnum.Min.ToString());
                    ResultValues.Add((IConvertible)result);
                }
                if (MathStatisticType.HasFlag(MathStatisticTypeEnum.Max))
                {
                    PNAX.MathExecuteStatistics(Channel, mnum);
                    PNAX.MathType(Channel, mnum, MathStatisticTypeEnum.Max);
                    result = PNAX.MathData(Channel, mnum);
                    Log.Info($"Max: {result}");
                    ResultNames.Add(MathStatisticTypeEnum.Max.ToString());
                    ResultValues.Add((IConvertible)result);
                }

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

                Results.Publish($"Statistics_Data_Channel_{Channel.ToString()}", ResultNames, ResultValues.ToArray());
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
