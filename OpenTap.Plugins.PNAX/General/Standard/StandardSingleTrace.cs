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
    public enum StandardTraceEnum
    {
        S11,
        S12,
        S13,
        S14,
        S21,
        S22,
        S23,
        S24,
        S31,
        S32,
        S33,
        S34,
        S41,
        S42,
        S43,
        S44,
        Sss11,
        Ssd12,
        Ssc12,
        Sds21,
        Sdd22,
        Sdc22,
        Scs21,
        Scd22,
        Scc22,
        [Display("Sds21/Scs21")]
        Sds21Scs21,
        [Display("Ssd12/Ssc12")]
        Ssd12Ssc12,
        A1,
        A2,
        A3,
        A4,
        B1,
        B2,
        B3,
        B4,
        C1,
        C2,
        C3,
        C4,
        D1,
        D2,
        D3,
        D4,
        R11,
        R22,
        R33,
        R44,
        a11,
        a22,
        a33,
        a44,
        b11,
        b12,
        b13,
        b14,
        b21,
        b22,
        b23,
        b24,
        b31,
        b32,
        b33,
        b34,
        b41,
        b42,
        b43,
        b44,
        AuxLn11,
        AuxLn21
    }

    [AllowAsChildIn(typeof(StandardNewTrace))]
    [Display("Standard Single Trace", Groups: new[] { "PNA-X", "General",  "Standard" }, Description: "Insert a description here")]
    public class StandardSingleTrace : GeneralSingleTraceBaseStep
    {
        private StandardTraceEnum _Meas;
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public StandardTraceEnum Meas
        {
            get
            {
                return _Meas;
            }
            set
            {
                _Meas = value;
                UpdateTestName();
            }
        }



        public StandardSingleTrace()
        {
            Trace = "1";
            Meas = StandardTraceEnum.S11;
            Format = PNAX.MeasurementFormatEnum.MLOGarithmic;
            Class = TraceManagerChannelClassEnum.STD;
            Window = 1;
            Sheet = 1;
            Channel = 1;
        }

        protected override void UpdateTestName()
        {
            this.Trace = $"CH{Channel.ToString()}_{Meas}";
            this.Name = $"CH{Channel.ToString()}_{Meas}";
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            int _tnum = 0;
            int _mnum = 0;
            PNAX.AddNewTrace(Channel, Window, Trace, "Standard", Meas.ToString(), ref _tnum, ref _mnum);
            tnum = _tnum;
            mnum = _mnum;

            PNAX.SetTraceTitle(Window, tnum, TraceTitle);

            PNAX.SetTraceFormat(Window, mnum, Format);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
