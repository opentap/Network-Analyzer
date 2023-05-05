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
    [AllowAsChildIn(typeof(GeneralSweptIMDChannel))]
    [AllowAsChildIn(typeof(GeneralSweptIMDNewTrace))]
    [Display("Swept IMD Single Trace", Groups: new[] { "PNA-X", "General", "Swept IMD" }, Description: "Insert a description here")]
    public class GeneralSweptIMDSingleTrace : GeneralSingleTraceBaseStep
    {
        #region Settings
        private SweptIMDTraceEnum _Meas;
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public SweptIMDTraceEnum Meas
        {
            get { return _Meas; }
            set
            {
                _Meas = value;
                UpdateTestName();
            }
        }

        //private TraceManagerChannelClassEnum _Class;
        //[Display("Class", Groups: new[] { "Trace" }, Order: 12)]
        //public override TraceManagerChannelClassEnum Class
        //{
        //    get { return _Class; }
        //    set
        //    {
        //        _Class = value;
        //        UpdateTestName();
        //    }
        //}


        #endregion

        public GeneralSweptIMDSingleTrace()
        {
            Trace = "1";
            Meas = SweptIMDTraceEnum.Pwr2Hi;
            Format = PNAX.MeasurementFormatEnum.MLOGarithmic;
            //Class = TraceManagerChannelClassEnum.IMDX;
            Channel = 1;
            Window = 1;
            Sheet = 1;
        }

        protected override void UpdateTestName()
        {
            this.Trace = $"CH{Channel}_{Meas}";
            this.Name = $"CH{Channel}_{Meas}";
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            int _tnum = 0;
            int _mnum = 0;
            PNAX.AddNewTrace(Channel, Window, Trace, "Swept IMD", Meas.ToString(), ref _tnum, ref _mnum);
            tnum = _tnum;
            mnum = _mnum;

            PNAX.SetTraceTitle(Window, tnum, TraceTitle);

            PNAX.SetTraceFormat(Window, mnum, Format);
            
            UpgradeVerdict(Verdict.Pass);
        }
    }


}
