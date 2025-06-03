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
using System.Text;
using OpenTap;

namespace OpenTap.Plugins.PNAX
{
    public enum XAxisAnnotation
    {
        [Display("Output")]
        Output,

        [Display("Input")]
        Input,
    }

    //[AllowAsChildIn(typeof(NoiseFigureChannel))]
    [Display(
        "Noise Figure Frequency",
        Groups: new[] { "Network Analyzer", "Converters", "Noise Figure Converters" },
        Description: "Insert a description here"
    )]
    public class NoiseFigureFrequency : FrequencyBaseStep
    {
        #region Settings
        private SweepTypeEnum _sweepType;

        [Display("Sweep Type", Order: 1)]
        public SweepTypeEnum SweepType
        {
            get { return _sweepType; }
            set
            {
                _sweepType = value;
                LinearSweepEnabled = SweepTypeEnum.LinearSweep == value;
                CWFrequencyEnabled = !LinearSweepEnabled;
                EnableSegmentSweepSettings = false;
            }
        }

        [Display("X-Axis Annotation", Order: 1)]
        public XAxisAnnotation XAxisAnnotation { get; set; }
        #endregion

        public NoiseFigureFrequency()
        {
            IsConverter = true;
            SweepType = SweepTypeEnum.LinearSweep;
        }

        protected override void UpdateSweepSettings()
        {
            var DefaultValues = PNAX.GetNoiseFigureConverterFrequencyDefaultValues();

            SweepSettingsNumberOfPoints = DefaultValues.SweepSettingsNumberOfPoints;
            SweepSettingsIFBandwidth = DefaultValues.SweepSettingsIFBandwidth;
            SweepSettingsStart = DefaultValues.SweepSettingsStart;
            SweepSettingsStop = DefaultValues.SweepSettingsStop;
            SweepSettingsCenter = DefaultValues.SweepSettingsCenter;
            SweepSettingsSpan = DefaultValues.SweepSettingsSpan;
            SweepSettingsFixed = DefaultValues.SweepSettingsFixed;
        }

        protected override void SetSweepType()
        {
            PNAX.SetSweepType(Channel, SweepType);
        }

        protected override void SetMode()
        {
            // Noise Frequency does not set Mode, only Gain Compression Frequency, which also derives from same base class
        }
    }
}
