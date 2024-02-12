// Author: MyName
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
    [AllowAsChildIn(typeof(TestPlan))]
    [AllowAsChildIn(typeof(DIQSources))]
    [Display("DIQ Source", Groups: new[] { "Network Analyzer", "General", "Differential I/Q" }, Description: "Insert a description here")]
    public class DIQSource : PNABaseStep
    {
        #region Settings
        [Display("Name", Group: "Source", Order: 20)]
        public string SourceName
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }

        [Display("Source State", Groups: new[] { "Source" }, Order: 21)]
        public DIQPortStateEnumtype SourceState { get; set; }

        [Display("Frequency Range", Groups: new[] { "Source" }, Order: 22)]
        public int FreqRange { get; set; }


        private bool _SweepPower;
        [Display("Sweep Power", Groups: new[] { "Power" }, Order: 30)]
        public bool SweepPower 
        {
            get
            {
                return _SweepPower;
            }
            set
            {
                _SweepPower = value;
                if (_SweepPower)
                {
                    Autorange = false;
                }
            }
        }


        [Display("Start Power", Groups: new[] { "Power" }, Order: 31)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double StartPower { get; set; }

        [EnabledIf("SweepPower", true, HideIfDisabled = false)]
        [Display("Stop Power", Groups: new[] { "Power" }, Order: 32)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double StopPower { get; set; }

        [Display("Leveling Mode", Groups: new[] { "Power" }, Order: 33)]
        public string LevelingMode { get; set; }

        private bool _Autorange;
        [EnabledIf("SweepPower", false, HideIfDisabled = false)]
        [Display("Auto range source attenuator", Groups: new[] { "Power" }, Order: 34)]
        public bool Autorange
        {
            get
            {
                return _Autorange;
            }
            set
            {
                _Autorange = value;
                if (_Autorange)
                {
                    SourceAttenuator = 0;
                }
            }
        }


        [EnabledIf("Autorange", false, HideIfDisabled = false)]
        [Display("Source Attenuator", Groups: new[] { "Power" }, Order: 35)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0")]
        public int SourceAttenuator { get; set; }
        #endregion

        public DIQSource()
        {
            SourceName = "Source";
            SourceState = DIQPortStateEnumtype.Auto;
            FreqRange = 1;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
