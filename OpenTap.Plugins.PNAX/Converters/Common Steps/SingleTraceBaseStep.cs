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
    public class SingleTraceBaseStep : TestStep
    {
        #region Settings
        [Browsable(false)]
        public bool EnableTraceSettings { get; set; } = false;


        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Trace", Groups: new[] { "Trace" }, Order: 10)]
        public string Trace { get; set; }

        //private int _TraceChannel;
        //[EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        //[Display("Channel", Groups: new[] { "Trace" }, Order: 13)]
        //public virtual int Channel
        //{
        //    get { return _TraceChannel; }
        //    set
        //    {
        //        _TraceChannel = value;
        //        UpdateTestName();
        //    }
        //}


        private int _Window;
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
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
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
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

        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [EnabledIf("IsPropertyEnabled", true, HideIfDisabled = false)]
        [Display("TNum", Groups: new[] { "Trace" }, Order: 20)]
        public int tnum { get; set; }

        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [EnabledIf("IsPropertyEnabled", true, HideIfDisabled = false)]
        [Display("MNum", Groups: new[] { "Trace" }, Order: 21)]
        [Output]
        public int mnum { get; set; }

        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [EnabledIf("IsPropertyEnabled", true, HideIfDisabled = false)]
        [Display("Meas Name", Groups: new[] { "Trace" }, Order: 22)]
        [Output]
        public String MeasName { get; set; }
        #endregion

        public SingleTraceBaseStep()
        {
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Trace Format", Groups: new[] { "Trace" }, Order: 30)]
        public virtual void AddTraceFormat()
        {
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Trace Title", Groups: new[] { "Trace" }, Order: 40)]
        public virtual void AddTraceTitle()
        {
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Marker", Groups: new[] { "Trace" }, Order: 50)]
        public virtual void AddMarker()
        {
        }

        public int NextMarker()
        {
            int retMarkerCount = 1;
            foreach(TestStep t in ChildTestSteps)
            {
                if (t is Marker)
                {
                    retMarkerCount++;
                }
            }
            return retMarkerCount++;
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Trace Limits", Groups: new[] { "Trace" }, Order: 60)]
        public virtual void AddTraceLimits()
        {
        }

        protected virtual void UpdateTestName()
        {
        }

        [Browsable(false)]
        public virtual List<(string, object)> GetMetaData()
        {
            throw new NotImplementedException();
        }



        public override void Run()
        {
        }
    }
}
