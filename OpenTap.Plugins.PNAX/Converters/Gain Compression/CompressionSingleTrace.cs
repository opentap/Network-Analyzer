﻿// Author: MyName
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
    public enum CompressionTraceEnum
    {
        SC21,
        S11,
        SC12,
        S22,
        AI1,
        AI2,
        CompIn21,
        CompOut21,
        DeltaGain21,
        CompS11,
        RefS21,
        CompAI1,
        CompAI2,
        IPwr,
        OPwr,
        RevIPwr,
        RevOPwr
    }

    [AllowAsChildIn(typeof(GainCompressionChannel))]
    [AllowAsChildIn(typeof(GainCompressionNewTrace))]
    [Display("Compression Single Trace", Groups: new[] { "PNA-X", "Converters", "Gain Compression Converters" }, Description: "Insert a description here")]
    public class CompressionSingleTrace : ConverterSingleTraceBaseStep
    {
        #region Settings
        private CompressionTraceEnum _Meas;
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public CompressionTraceEnum Meas
        {
            get { return _Meas; }
            set
            {
                _Meas = value;
                UpdateTestName();
            }
        }

        #endregion

        public CompressionSingleTrace()
        {
            Trace = "1";
            Meas = CompressionTraceEnum.CompIn21;
            Format = PNAX.MeasurementFormatEnum.MLOGarithmic;
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
            PNAX.AddNewTrace(Channel, Window, Trace, "Gain Compression Converters", Meas.ToString(), ref _tnum, ref _mnum);
            tnum = _tnum;
            mnum = _mnum;

            PNAX.SetTraceTitle(Window, tnum, TraceTitle);

            PNAX.SetTraceFormat(Window, mnum, Format);
            
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
