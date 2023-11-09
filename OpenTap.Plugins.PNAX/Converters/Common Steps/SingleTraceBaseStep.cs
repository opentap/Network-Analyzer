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
using System.Runtime.Remoting.Channels;
using System.Text;

namespace OpenTap.Plugins.PNAX
{
    [Browsable(false)]
    public class SingleTraceBaseStep : TestStep
    {
        #region Settings

        [Browsable(false)]
        public bool IsControlledByParent { get; set; } = false;
        private PNAX _PNAX;
        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
        [Display("PNA", Order: 0.1)]
        public virtual PNAX PNAX
        {
            get
            {
                return _PNAX;
            }
            set
            {
                _PNAX = value;

                // Update traces
                foreach (var a in this.ChildTestSteps)
                {
                    if (a.GetType().IsSubclassOf(typeof(SingleTraceBaseStep)))
                    {
                        (a as SingleTraceBaseStep).PNAX = value;
                    }
                }
            }
        }

        private int _Channel;
        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
        [Display("Channel", Order: 1)]
        public virtual int Channel
        {
            get
            {
                return _Channel;
            }
            set
            {
                _Channel = value;

                // Update traces
                foreach (var a in this.ChildTestSteps)
                {
                    if (a.GetType().IsSubclassOf(typeof(SingleTraceBaseStep)))
                    {
                        (a as SingleTraceBaseStep).Channel = value;
                    }
                }
            }
        }

        [Browsable(false)]
        public bool EnableTraceSettings { get; set; } = false;


        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Trace", Groups: new[] { "Trace" }, Order: 10)]
        public string Trace { get; set; }

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
        public string MeasName { get; set; }
        #endregion

        protected string measClass;
        protected string measEnumName;

        public SingleTraceBaseStep()
        {
            Trace = "1";
            Window = 1;
            Sheet = 1;
            Channel = 1;
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Trace Format", Groups: new[] { "Trace" }, Order: 30)]
        public virtual void AddTraceFormat()
        {
            this.ChildTestSteps.Add(new TraceFormat() { PNAX = this.PNAX, Channel = this.Channel });
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Trace Title", Groups: new[] { "Trace" }, Order: 40)]
        public virtual void AddTraceTitle()
        {
            this.ChildTestSteps.Add(new TraceTitle() { PNAX = this.PNAX, Channel = this.Channel });
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Marker", Groups: new[] { "Trace" }, Order: 50)]
        public virtual void AddMarker()
        {
            this.ChildTestSteps.Add(new Marker() { PNAX = this.PNAX, Channel = this.Channel, mkr = NextMarker() });
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
            this.ChildTestSteps.Add(new TraceLimits() { PNAX = this.PNAX, Channel = this.Channel });
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Multi Peak Search", Groups: new[] { "Trace" }, Order: 70)]
        public virtual void AddMultiPeakSearch()
        {
        }


        protected virtual void UpdateTestName()
        {
            Trace = $"CH{Channel}_{measEnumName}";
            Name = $"CH{Channel}_{measEnumName}";
        }

        [Browsable(false)]
        public virtual List<(string, object)> GetMetaData()
        {
            throw new NotImplementedException();
        }

        public override void Run()
        {
            AddNewTrace();

            RunChildSteps(); //If the step supports child steps.

            UpgradeVerdict(Verdict.Pass);
        }

        protected void AddNewTrace()
        {
            int _tnum = 0;
            int _mnum = 0;
            string _MeasName = "";
            PNAX.AddNewTrace(Channel, Window, Trace, measClass, measEnumName.ToString(), ref _tnum, ref _mnum, ref _MeasName);
            tnum = _tnum;
            mnum = _mnum;
            MeasName = _MeasName;
        }

    }
}
