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
    //[AllowAsChildIn(typeof(TestPlan))]
    //[AllowAsChildIn(typeof(SingleTraceBaseStep))]
    [Display("Statistics", Groups: new[] { "Network Analyzer", "Trace" }, Description: "Set Statistics for a trace")]
    public class TraceStatistics : PNABaseStep
    {
        #region Settings
        // Override for Channel so we can call ShowTraceSettings
        private int _Channel;
        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
        [Display("Channel", Order: 0.11)]
        public override int Channel
        {
            get
            {
                ShowTraceSettings();
                return _Channel;
            }
            set
            {
                ShowTraceSettings();
                _Channel = value;

                // Update traces
                foreach (var a in ChildTestSteps)
                {
                    if (a.GetType().IsSubclassOf(typeof(PNABaseStep)))
                    {
                        (a as PNABaseStep).Channel = value;
                    }
                    if (a is SingleTraceBaseStep)
                    {
                        (a as SingleTraceBaseStep).UpdateTestStepName();
                    }
                }
            }
        }

        [EnabledIf("IsControlledByParent", false,HideIfDisabled = true)]
        [Display("Window (standalone)", Groups: new[] { "Trace" }, Order: 14)]
        public int Window { get; set; }

        [EnabledIf("IsControlledByParent", false, HideIfDisabled = true)]
        [Display("MNum (standalone)", Groups: new[] { "Trace" }, Order: 21)]
        public int mnum { get; set; }

        [Display("Enable Statistics", Groups: new[] { "Trace Statistics" }, Order: 30)]
        public bool EnableStatistics { get; set; }

        [Display("Statistics Range", Groups: new[] { "Trace Statistics" }, Order: 31)]
        public MathStatisticsRangeEnum MathStatisticsRange { get; set; }

        [EnabledIf("MathStatisticsRange", MathStatisticsRangeEnum.User1, MathStatisticsRangeEnum.User2, MathStatisticsRangeEnum.User3
            , MathStatisticsRangeEnum.User4, MathStatisticsRangeEnum.User5, MathStatisticsRangeEnum.User6
            , MathStatisticsRangeEnum.User7, MathStatisticsRangeEnum.User8, MathStatisticsRangeEnum.User9
            , MathStatisticsRangeEnum.User10, MathStatisticsRangeEnum.User11, MathStatisticsRangeEnum.User12
            , MathStatisticsRangeEnum.User13, MathStatisticsRangeEnum.User14, MathStatisticsRangeEnum.User15
            , MathStatisticsRangeEnum.User16, HideIfDisabled = true)]
        [Display("User Start", Groups: new[] { "Trace Statistics" }, Order: 32)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double UserStart { get; set; }

        [EnabledIf("MathStatisticsRange", MathStatisticsRangeEnum.User1, MathStatisticsRangeEnum.User2, MathStatisticsRangeEnum.User3
            , MathStatisticsRangeEnum.User4, MathStatisticsRangeEnum.User5, MathStatisticsRangeEnum.User6
            , MathStatisticsRangeEnum.User7, MathStatisticsRangeEnum.User8, MathStatisticsRangeEnum.User9
            , MathStatisticsRangeEnum.User10, MathStatisticsRangeEnum.User11, MathStatisticsRangeEnum.User12
            , MathStatisticsRangeEnum.User13, MathStatisticsRangeEnum.User14, MathStatisticsRangeEnum.User15
            , MathStatisticsRangeEnum.User16, HideIfDisabled = true)]
        [Display("User Stop", Groups: new[] { "Trace Statistics" }, Order: 33)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double UserStop { get; set; }

        [Display("Show Smith Chart statistics in Ohms", Groups: new[] { "Trace Statistics" }, Order: 34)]
        public bool ShowResistance { get; set; }


        #endregion

        private void ShowTraceSettings()
        {
            // What type of parent test we have?
            if (this.Parent != null)
            {
                Type parType = this.Parent.GetType();
                if (parType.Equals(typeof(TestPlan)))
                {
                    IsControlledByParent = false;
                }
                else
                {
                    // Parent must be a SingleTraceBaseStep
                    IsControlledByParent = true;
                }
            }
        }

        public TraceStatistics()
        {
            Window = 1;
            mnum = 1;
            Channel = 1;

            EnableStatistics = false;
            MathStatisticsRange = MathStatisticsRangeEnum.FullSpan;
            UserStart = 100e3;
            UserStop = 44e9;
            ShowResistance = false;
        }

        public override void Run()
        {
            if (IsControlledByParent)
            {
                mnum = GetParent<SingleTraceBaseStep>().mnum;
                Channel = GetParent<SingleTraceBaseStep>().Channel;
            }

            PNAX.MathStatistics(Channel, mnum, EnableStatistics);
            if (EnableStatistics)
            {
                PNAX.MathStatisticsRange(Channel, mnum, MathStatisticsRange);
                if (MathStatisticsRange != MathStatisticsRangeEnum.FullSpan)
                {
                    PNAX.MathStatisticsRangeStart(Channel, mnum, MathStatisticsRange, UserStart);
                    PNAX.MathStatisticsRangeStop(Channel, mnum, MathStatisticsRange, UserStop);
                }
                PNAX.MathShowResistance(Channel, mnum, ShowResistance);
            }

            UpgradeVerdict(Verdict.Pass);
            UpdateMetaData();
        }

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            List<(string, object)> retVal = new List<(string, object)>();

            retVal.Add(("Channel", Channel));
            retVal.Add(("EnableStatistics", EnableStatistics));
            retVal.Add(("MathStatisticsRange", MathStatisticsRange));
            if (MathStatisticsRange != MathStatisticsRangeEnum.FullSpan)
            {
                retVal.Add(("UserStart", UserStart));
                retVal.Add(("UserStop", UserStop));
            }
            retVal.Add(("ShowResistance", ShowResistance));

            return retVal;
        }

        public override void UpdateMetaData()
        {
            MetaData = new List<(string, object)> { ("Channel", Channel) };

            List<(string, object)> ret = GetMetaData();
            foreach (var it in ret)
            {
                MetaData.Add(it);
            }
        }

    }
}
