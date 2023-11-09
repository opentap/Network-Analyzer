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
                MeasName = value.ToString();
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
            Meas = SweptIMDTraceEnum.Pwr2Hi;
            measClass = "Swept IMD";
            //Class = TraceManagerChannelClassEnum.IMDX;
        }

        protected override void UpdateTestName()
        {
            this.Trace = $"CH{Channel}_{Meas}";
            this.Name = $"CH{Channel}_{Meas}";
        }

        public override void Run()
        {
            AddNewTrace();

            RunChildSteps(); //If the step supports child steps.
            
            UpgradeVerdict(Verdict.Pass);
        }
    }


}
