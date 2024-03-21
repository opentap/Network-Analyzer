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

namespace OpenTap.Plugins.PNAX
{
    [Display("MOD Get Data", Groups: new[] { "Network Analyzer", "General", "Modulation Distortion" }, Description: "Insert a description here")]
    public class MODGetData : StoreDataBase
    {
        #region Settings
        [Display("All data", Group: "Settings", Order: 21)]
        public bool AllData { get; set; }

        [EnabledIf("AllData", false, HideIfDisabled = true)]
        [Display("Parameter Name", Group: "Settings", Order: 22)]
        public string paramName { get; set; }

        #endregion

        public MODGetData()
        {
            channels = new List<int>() { 1 };
            AutoSelectChannels = true;
            AllData = true;
            paramName = "Carrier In1 dBm";
        }

        public override void Run()
        {
            MetaData = new List<(string, object)>();
            UpgradeVerdict(Verdict.NotSet);
            AutoSelectChannelsAvailableOnInstrument();

            RunChildSteps(); //If the step supports child steps.

            foreach (int Channel in channels)
            {
                List<string> ResultNames = new List<string>();
                List<IConvertible> ResultValues = new List<IConvertible>();

                if (AllData)
                {
                    List<string> allColumns = PNAX.MODGetAllColumnNames(Channel);
                    foreach (string paramName in allColumns)
                    {
                        double value = PNAX.MODGetDataValue(Channel, paramName);

                        ResultNames.Add(paramName);
                        ResultValues.Add((IConvertible)value);
                    }
                }
                else
                {
                    // Query a particular field
                    double value = PNAX.MODGetDataValue(Channel, paramName);

                    ResultNames.Add(paramName);
                    ResultValues.Add((IConvertible)value);
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

                Results.Publish($"MOD_Data_Channel_{Channel.ToString()}", ResultNames, ResultValues.ToArray());
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
