// Author: CMontes
// Copyright:   Copyright 2023-2024 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using OpenTap;

namespace OpenTap.Plugins.PNAX
{
    //[AllowAsChildIn(typeof(TestPlan))]
    //[AllowAsChildIn(typeof(DIQFrequencyRange))]
    [Display(
        "DIQ Range",
        Groups: new[] { "Network Analyzer", "General", "Differential I/Q" },
        Description: "Insert a description here"
    )]
    public class DIQRange : PNABaseStep
    {
        #region Settings
        [Browsable(false)]
        public bool IsCouplingSectionEnabled { get; set; }

        [Browsable(false)]
        public bool EnableRangeField { get; set; } = false;

        [EnabledIf("EnableRangeField", true, HideIfDisabled = false)]
        [Display("Name", Group: "Settings", Order: 20)]
        public string RangeName { get; set; }

        private int _Range;

        [Display("Range", Group: "Settings", Order: 21)]
        public int Range
        {
            set
            {
                _Range = value;
                RangeName = $"F{_Range}";
                this.Name = RangeName;
                if (_Range > 1)
                {
                    IsCouplingSectionEnabled = true;
                }
                else
                {
                    IsCouplingSectionEnabled = false;
                }
            }
            get { return _Range; }
        }

        [EnabledIf("Couple", false, HideIfDisabled = false)]
        [Display("Start", Groups: new[] { "Frequency" }, Order: 31)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double FreqStart { get; set; }

        [EnabledIf("Couple", false, HideIfDisabled = false)]
        [Display("Stop", Groups: new[] { "Frequency" }, Order: 32)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double FreqStop { get; set; }

        [Display("IFBW", Groups: new[] { "Frequency" }, Order: 33)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double IFBW { get; set; }

        [EnabledIf("IsCouplingSectionEnabled", true, HideIfDisabled = true)]
        [Display("Couple", Groups: new[] { "Coupling" }, Order: 50)]
        public bool Couple { get; set; }

        // TODO
        // if parent step is DIQFrequencyRange then
        // Present list of available Ranges
        // if step is at TestPlan root level
        // allow this to be entered
        [EnabledIf("Couple", true, HideIfDisabled = true)]
        [Display("Couple to", Groups: new[] { "Coupling" }, Order: 51)]
        public int CoupleID { get; set; }

        [EnabledIf("Couple", true, HideIfDisabled = true)]
        [Display("Offset", Groups: new[] { "Coupling" }, Order: 52)]
        public int Offset { get; set; }

        [EnabledIf("Couple", true, HideIfDisabled = true)]
        [Display("Up", Groups: new[] { "Coupling" }, Order: 53)]
        public bool UpDownConversion { get; set; }

        [EnabledIf("Couple", true, HideIfDisabled = true)]
        [Display("Multiplier", Groups: new[] { "Coupling" }, Order: 54)]
        public int Multiplier { get; set; }

        [EnabledIf("Couple", true, HideIfDisabled = true)]
        [Display("Divisor", Groups: new[] { "Coupling" }, Order: 55)]
        public int Divisor { get; set; }

        #endregion

        public DIQRange()
        {
            Range = 1;
            FreqStart = 10e6;
            FreqStop = 50e9;
            IFBW = 100e3;

            Couple = false;
            CoupleID = 1;
            Offset = 0;
            UpDownConversion = true;
            Multiplier = 1;
            Divisor = 1;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            if (Range > 1)
            {
                // First range will always be available after preset
                // no need to add it
                PNAX.DIQFrequencyRangeAdd(Channel);
            }
            if (!Couple)
            {
                PNAX.DIQFrequencyRangeStart(Channel, Range, FreqStart);
                PNAX.DIQFrequencyRangeStop(Channel, Range, FreqStop);
            }
            PNAX.DIQFrequencyRangeIFBW(Channel, Range, IFBW);

            PNAX.DIQFrequencyRangeCouplingState(Channel, Range, Couple);
            if (Couple)
            {
                PNAX.DIQFrequencyRangeCouplingID(Channel, Range, CoupleID);
                PNAX.DIQFrequencyRangeCouplingOffset(Channel, Range, Offset);
                PNAX.DIQFrequencyRangeCouplingUp(Channel, Range, UpDownConversion);
                PNAX.DIQFrequencyRangeCouplingMultiplier(Channel, Range, Multiplier);
                PNAX.DIQFrequencyRangeCouplingDivisor(Channel, Range, Divisor);
            }
            PNAX.WaitForOperationComplete();

            // If Couple enabled, then read values for Freq
            if (Couple)
            {
                // Read Values from instrument
                FreqStart = PNAX.DIQFrequencyRangeStart(Channel, Range);
                FreqStop = PNAX.DIQFrequencyRangeStop(Channel, Range);
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
