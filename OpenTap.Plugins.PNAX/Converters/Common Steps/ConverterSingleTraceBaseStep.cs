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
    [Browsable(false)]
    public class ConverterSingleTraceBaseStep : ConverterBaseStep
    {
        #region Settings
        [Display("Trace Title", Groups: new[] { "Trace" }, Order: 9)]
        public String TraceTitle { get; set; }

        [Display("Trace", Groups: new[] { "Trace" }, Order: 10)]
        public string Trace { get; set; }

        [Display("Format", Groups: new[] { "Trace" }, Order: 11.5)]
        public PNAX.MeasurementFormatEnum Format { get; set; }

        private int _Channel;
        [Display("Channel", Groups: new[] { "Trace" }, Order: 13)]
        public override int Channel
        {
            get { return _Channel; }
            set
            {
                _Channel = value;
                UpdateTestName();
            }
        }


        private int _Window;
        [Display("Window", Groups: new[] { "Trace" }, Order: 14)]
        public int Window
        {
            get { return _Window; }
            set
            {
                _Window = value;
                UpdateTestName();
            }
        }


        private int _Sheet;
        [Display("Sheet", Groups: new[] { "Trace" }, Order: 15)]
        public int Sheet
        {
            get { return _Sheet; }
            set
            {
                _Sheet = value;
                UpdateTestName();
            }
        }

        [Browsable(false)]
        public bool IsPropertyEnabled { get; set; } = false;
        [EnabledIf("IsPropertyEnabled", true, HideIfDisabled = false)]
        [Display("TNum", Groups: new[] { "Trace" }, Order: 20)]
        public int tnum { get; set; }

        [EnabledIf("IsPropertyEnabled", true, HideIfDisabled = false)]
        [Display("MNum", Groups: new[] { "Trace" }, Order: 21)]
        public int mnum { get; set; }
        #endregion

        public ConverterSingleTraceBaseStep()
        {
            IsControlledByParent = true;
        }

        protected virtual void UpdateTestName()
        {
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
