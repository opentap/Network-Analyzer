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
