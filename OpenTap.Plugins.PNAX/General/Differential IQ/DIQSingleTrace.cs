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
    [AllowAsChildIn(typeof(DIQNewTrace))]
    [Display("DIQ Single Trace", Groups: new[] { "Network Analyzer", "General", "Differential I/Q" }, Description: "Insert a description here")]
    public class DIQSingleTrace : SingleTraceBaseStep
    {

        private DIQTraceEnum _Meas;

        [EnabledIf(nameof(CustomTraceMeas), false, HideIfDisabled = true)]
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11.1)]
        public DIQTraceEnum Meas
        {
            get
            {
                return _Meas;
            }
            set
            {
                _Meas = value;
                string scpi = Scpi.Format("{0}", value);
                measEnumName = scpi;    // value.ToString();
                UpdateTestStepName();
            }
        }

        public DIQSingleTrace()
        {
            Meas = DIQTraceEnum.IPwrF1;
            measClass = "Differential I/Q";
        }

        public void AddTraceFormat(PNAX.MeasurementFormatEnum format)
        {
            TraceFormat trFormat = new TraceFormat() { PNAX = this.PNAX, Channel = this.Channel };
            trFormat.Format = format;
            this.ChildTestSteps.Add(trFormat);
        }

        public void AddTraceTitle(string title)
        {
            TraceTitle trTitle = new TraceTitle() { PNAX = this.PNAX, Channel = this.Channel };
            trTitle.Title = title;
            this.ChildTestSteps.Add(trTitle);
        }

    }
}
