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
    public enum GeneralNFSweepTypeEnum
    {
        [Scpi("LIN")]
        [Display("Linear Sweep")]
        LinearSweep,
        [Scpi("LOGarithmic")]
        [Display("Log Frequency")]
        LogFrequency,
        [Scpi("CW")]
        [Display("CW Frequency")]
        CWFrequency,
        [Scpi("SEGMent")]
        [Display("Segment Sweep")]
        SegmentSweep
    }

    public enum SweepSSCSTypeEnum
    {
        [Display("Start/Stop")]
        StartStop,
        [Display("Center/Span")]
        CenterSpan
    }

    //[AllowAsChildIn(typeof(GeneralNoiseFigureChannel))]
    [Display("Noise Figure Frequency", Groups: new[] { "Network Analyzer", "General", "Noise Figure Cold Source" }, Description: "Insert a description here")]
    public class GeneralNoiseFigureFrequency : FrequencyBaseStep
    {
        #region Settings
        private GeneralNFSweepTypeEnum _SweepType;
        [Display("Sweep Type", Order: 1)]
        public GeneralNFSweepTypeEnum SweepType
        {
            get
            {
                return _SweepType;
            }
            set
            {
                _SweepType = value;
                EnableSegmentSweepSettings = value == GeneralNFSweepTypeEnum.SegmentSweep;
                CWFrequencyEnabled = value == GeneralNFSweepTypeEnum.CWFrequency;
                LinearSweepEnabled = value == GeneralNFSweepTypeEnum.LinearSweep ||
                                    value == GeneralNFSweepTypeEnum.LogFrequency;
            }
        }

        [Display("X-Axis Annotation", Order: 1)]
        public XAxisAnnotation XAxisAnnotation { get; set; }

        private int _SweepSettingsNumberOfPoints;
        [Display("Number Of Points", Group: "Sweep Settings", Order: 10)]
        public override int SweepSettingsNumberOfPoints
        {
            get
            {
                return _SweepSettingsNumberOfPoints;
            }
            set
            {
                _SweepSettingsNumberOfPoints = value;
                // Update Points on Parent step
                try
                {
                    var a = GetParent<ConverterChannelBaseStep>();
                    // only if there is a parent of type ScalarMixerChannel
                    if (a != null)
                    {
                        a.SweepPoints = _SweepSettingsNumberOfPoints;
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("can't find parent yet! ex: " + ex.Message);
                }

            }
        }

        #endregion

        public GeneralNoiseFigureFrequency()
        {
            XAxisAnnotation = XAxisAnnotation.Output;
        }

        protected override void UpdateSweepSettings()
        {
            var DefaultValues = PNAX.GetNoiseFigureConverterFrequencyDefaultValues();
            SweepType = DefaultValues.GeneralNFSweepType;

            SweepSettingsNumberOfPoints = DefaultValues.SweepSettingsNumberOfPoints;
            SweepSettingsIFBandwidth = DefaultValues.SweepSettingsIFBandwidth;
            SweepSettingsStart = DefaultValues.SweepSettingsStart;
            SweepSettingsStop = DefaultValues.SweepSettingsStop;
            SweepSettingsCenter = DefaultValues.SweepSettingsCenter;
            SweepSettingsSpan = DefaultValues.SweepSettingsSpan;
            SweepSettingsFixed = DefaultValues.SweepSettingsFixed;

            IsStartStopCenterSpan = SweepSSCSTypeEnum.StartStop;
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
