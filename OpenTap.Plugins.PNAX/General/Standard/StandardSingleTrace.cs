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
        [Scpi("Sds21/Scs21")]
        Sds21Scs21,
        [Display("Ssd12/Ssc12")]
        [Scpi("Ssd12/Ssc12")]
        Ssd12Ssc12,
        [Scpi("A_1")]
        A1,
        [Scpi("A_2")]
        A2,
        [Scpi("A_3")]
        A3,
        [Scpi("A_4")]
        A4,
        [Scpi("B_1")]
        B1,
        [Scpi("B_2")]
        B2,
        [Scpi("B_3")]
        B3,
        [Scpi("B_4")]
        B4,
        [Scpi("C_1")]
        C1,
        [Scpi("C_2")]
        C2,
        [Scpi("C_3")]
        C3,
        [Scpi("C_4")]
        C4,
        [Scpi("D_1")]
        D1,
        [Scpi("D_2")]
        D2,
        [Scpi("D_3")]
        D3,
        [Scpi("D_4")]
        D4,
        [Scpi("R_1")]
        R11,
        [Scpi("R_2")]
        R22,
        [Scpi("R_3")]
        R33,
        [Scpi("R_4")]
        R44,
        [Scpi("a1_1")]
        a11,
        [Scpi("a2_2")]
        a22,
        [Scpi("a3_3")]
        a33,
        [Scpi("a4_4")]
        a44,
        [Scpi("b1_1")]
        b11,
        [Scpi("b1_2")]
        b12,
        [Scpi("b1_3")]
        b13,
        [Scpi("b1_4")]
        b14,
        [Scpi("b2_1")]
        b21,
        [Scpi("b2_2")]
        b22,
        [Scpi("b2_3")]
        b23,
        [Scpi("b2_4")]
        b24,
        [Scpi("b3_1")]
        b31,
        [Scpi("b3_2")]
        b32,
        [Scpi("b3_3")]
        b33,
        [Scpi("b3_4")]
        b34,
        [Scpi("b4_1")]
        b41,
        [Scpi("b4_2")]
        b42,
        [Scpi("b4_3")]
        b43,
        [Scpi("b4_4")]
        b44,
        AuxLn11,
        AuxLn21
    }

    //[AllowAsChildIn(typeof(StandardNewTrace))]
    [Display("Standard Single Trace", Groups: new[] { "Network Analyzer", "General",  "Standard" }, Description: "Insert a description here")]
    public class StandardSingleTrace : SingleTraceBaseStep
    {

        private StandardTraceEnum _Meas;

        [EnabledIf(nameof(CustomTraceMeas), false, HideIfDisabled = true)]
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11.1)]
        public StandardTraceEnum Meas
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

        public StandardSingleTrace()
        {
            Meas = StandardTraceEnum.S11;
            measClass = "Standard";
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
