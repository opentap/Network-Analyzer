// Author: CMontes
// Copyright:   Copyright 2023-2024 Keysight Technologies
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
            Expression = "";
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

        public override void Run()
        {
            AddNewTraceToPNAX();

            RunChildSteps(); //If the step supports child steps.

            UpgradeVerdict(Verdict.Pass);
        }

        protected override void AddNewTraceToPNAX()
        {
            int _tnum = 0;
            int _mnum = 0;
            string _MeasName = "";
            if (CustomTraceMeas)
            {
                // Define new trace
                PNAX.DIQParameterDefine(Channel, CustomMeas, Expression);
                // add new trace
                PNAX.AddNewTrace(Channel, Window, Trace, measClass, CustomMeas, ref _tnum, ref _mnum, ref _MeasName);
            }
            else
            {
                PNAX.AddNewTrace(Channel, Window, Trace, measClass, finalMeasEnumName, ref _tnum, ref _mnum, ref _MeasName);
            }
            tnum = _tnum;
            mnum = _mnum;
            MeasName = _MeasName;
        }

    }
}
