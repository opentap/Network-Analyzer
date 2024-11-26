// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;

namespace OpenTap.Plugins.PNAX
{
    [Browsable(false)]
    public class SingleTraceBaseStep : PNABaseStep
    {
        #region Settings
        [Browsable(false)]
        public bool EnableTraceSettings { get; set; } = false;

        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Trace", Groups: new[] { "Trace" }, Order: 10)]
        public string Trace { get; set; }

        private bool _CustomTraceMeas;
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Custom Meas", Groups: new[] { "Trace" }, Order: 10.9)]
        public bool CustomTraceMeas
        {
            get
            {
                return _CustomTraceMeas;
            }
            set
            {
                _CustomTraceMeas = value;
                UpdateTestStepName();
            }
        }

        private string _CustomMeas;
        [EnabledIf(nameof(CustomTraceMeas), true, HideIfDisabled = true)]
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11.2)]
        public String CustomMeas
        {
            get
            {
                return _CustomMeas;
            }
            set
            {
                _CustomMeas = value;
                if (CustomTraceMeas)
                {
                    //measEnumName = value.ToString();
                    UpdateTestStepName();
                }
            }
        }

        [EnabledIf(nameof(CustomTraceMeas), true, HideIfDisabled = true)]
        [Display("Expression", Groups: new[] { "Trace" }, Order: 11.21)]
        public string Expression { get; set; }


        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Window", Groups: new[] { "Trace" }, Order: 14)]
        public int Window { get; set; }

        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Sheet", Groups: new[] { "Trace" }, Order: 15)]
        public int Sheet { get; set; }

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
        protected string finalMeasEnumName;

        public SingleTraceBaseStep()
        {
            Trace = "1";
            Window = 1;
            Sheet = 1;
            CustomTraceMeas = false;
            CustomMeas = "Device0_AM1";
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Trace Format", Groups: new[] { "Trace" }, Order: 30)]
        public virtual void AddTraceFormat()
        {
            ChildTestSteps.Add(new TraceFormat() { PNAX = PNAX, Channel = Channel });
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Trace Title", Groups: new[] { "Trace" }, Order: 40)]
        public virtual void AddTraceTitle()
        {
            ChildTestSteps.Add(new TraceTitle() { PNAX = PNAX, Channel = Channel });
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Marker", Groups: new[] { "Trace" }, Order: 50)]
        public virtual void AddMarker()
        {
            ChildTestSteps.Add(new Marker() { PNAX = PNAX, Channel = Channel, mkr = NextMarker() });
        }

        public int NextMarker()
        {
            int retMarkerCount = 1;
            foreach (TestStep t in ChildTestSteps)
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
            ChildTestSteps.Add(new TraceLimits() { PNAX = PNAX, Channel = Channel });
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Multi Peak Search", Groups: new[] { "Trace" }, Order: 70)]
        public virtual void AddMultiPeakSearch()
        {
        }


        public virtual void UpdateTestStepName()
        {
            if (CustomTraceMeas)
            {
                finalMeasEnumName = CustomMeas;
            }
            else
            {
                finalMeasEnumName = measEnumName;
            }

            Trace = $"CH{Channel}_{finalMeasEnumName}";
            Name = $"CH{Channel}_{finalMeasEnumName}";
        }

        public override void Run()
        {
            AddNewTraceToPNAX();

            RunChildSteps(); //If the step supports child steps.

            UpgradeVerdict(Verdict.Pass);
        }

        protected virtual void AddNewTraceToPNAX()
        {
            int _tnum = 0;
            int _mnum = 0;
            string _MeasName = "";
            UpdateTestStepName();
            //if (CustomTraceMeas)
            //{
            //    PNAX.AddNewTrace(Channel, Window, Trace, measClass, CustomMeas, ref _tnum, ref _mnum, ref _MeasName);
            //}
            //else
            //{
            PNAX.AddNewTrace(Channel, Window, Trace, measClass, finalMeasEnumName, ref _tnum, ref _mnum, ref _MeasName);
            //}
            tnum = _tnum;
            mnum = _mnum;
            MeasName = _MeasName;
        }

    }
}
