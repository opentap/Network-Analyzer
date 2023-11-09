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
    public enum GeneralGainCompressionTraceEnum
    {
        S21,
        S11,
        S12,
        S22,
        AI1,
        AI2,
        CompIn21,
        CompOut21,
        DeltaGain21,
        CompGain21,
        CompS11,
        RefS21,
        CompAI1,
        CompAI2,
    }

    [AllowAsChildIn(typeof(GeneralGainCompressionChannel))]
    [AllowAsChildIn(typeof(GeneralGainCompressionNewTrace))]
    [Display("Compression Single Trace", Groups: new[] { "PNA-X", "General", "Gain Compression" }, Description: "Insert a description here")]
    public class GeneralGainCompressionSingleTrace : GeneralSingleTraceBaseStep
    {
        #region Settings

        private GeneralGainCompressionTraceEnum _Meas;
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public GeneralGainCompressionTraceEnum Meas
        {
            get
            {
                return _Meas;
            }
            set
            {
                _Meas = value;
                measEnumName = value.ToString();
                UpdateTestName();
            }
        }
        #endregion

        public GeneralGainCompressionSingleTrace()
        {
            Meas = GeneralGainCompressionTraceEnum.CompIn21;
            measClass = "Gain Compression";
        }

        protected override void UpdateTestName()
        {
            this.Trace = $"CH{Channel}_{Meas}";
            this.Name = $"CH{Channel}_{Meas}";
        }
    }
}
