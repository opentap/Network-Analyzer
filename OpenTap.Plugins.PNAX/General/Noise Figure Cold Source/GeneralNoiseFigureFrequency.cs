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

    [AllowAsChildIn(typeof(GeneralNoiseFigureChannel))]
    [Display("Noise Figure Frequency", Groups: new[] { "PNA-X", "General", "Noise Figure Cold Source" }, Description: "Insert a description here")]
    public class GeneralNoiseFigureFrequency : GeneralBaseStep
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
                EnableSegmentSweepSettings = false;
                if (_SweepType == GeneralNFSweepTypeEnum.SegmentSweep)
                {
                    EnableSegmentSweepSettings = true;
                }
            }
        }




        [Display("X-Axis Annotation", Order: 1)]
        public XAxisAnnotation XAxisAnnotation { get; set; }

        private int _SweepSettingsNumberOfPoints;
        [Display("Number Of Points", Group: "Sweep Settings", Order: 10)]
        public int SweepSettingsNumberOfPoints
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
                    var a = GetParent<ConverterChannelBase>();
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

        [Display("IF Bandwidth", Group: "Sweep Settings", Order: 11)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double SweepSettingsIFBandwidth { get; set; }

        [EnabledIf("SweepType", GeneralNFSweepTypeEnum.LinearSweep, GeneralNFSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [Display("Type", Group: "Sweep Settings", Order: 11.9)]
        public SweepSSCSTypeEnum IsStartStopCenterSpan { get; set; }

        [EnabledIf("SweepType", GeneralNFSweepTypeEnum.LinearSweep, GeneralNFSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [EnabledIf("IsStartStopCenterSpan", SweepSSCSTypeEnum.StartStop, HideIfDisabled = true)]
        [Display("Start", Group: "Sweep Settings", Order: 12)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double SweepSettingsStart { get; set; }

        [EnabledIf("SweepType", GeneralNFSweepTypeEnum.LinearSweep, GeneralNFSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [EnabledIf("IsStartStopCenterSpan", SweepSSCSTypeEnum.StartStop, HideIfDisabled = true)]
        [Display("Stop", Group: "Sweep Settings", Order: 13)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsStop { get; set; }

        [EnabledIf("SweepType", GeneralNFSweepTypeEnum.LinearSweep, GeneralNFSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [EnabledIf("IsStartStopCenterSpan", SweepSSCSTypeEnum.CenterSpan, HideIfDisabled = true)]
        [Display("Center", Group: "Sweep Settings", Order: 14)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsCenter { get; set; }

        [EnabledIf("SweepType", GeneralNFSweepTypeEnum.LinearSweep, GeneralNFSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [EnabledIf("IsStartStopCenterSpan", SweepSSCSTypeEnum.CenterSpan, HideIfDisabled = true)]
        [Display("Span", Group: "Sweep Settings", Order: 15)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsSpan { get; set; }

        [EnabledIf("SweepType", GeneralNFSweepTypeEnum.CWFrequency, HideIfDisabled = true)]
        [Display("Fixed", Group: "Sweep Settings", Order: 16)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsFixed { get; set; }
        #endregion

        public GeneralNoiseFigureFrequency()
        {
            UpdateDefaultValues();
        }

        public void UpdateDefaultValues()
        {
            var DefaultValues = PNAX.GetNoiseFigureConverterFrequencyDefaultValues();
            SweepType                   = DefaultValues.GeneralNFSweepType;

            SweepSettingsNumberOfPoints = DefaultValues.SweepSettingsNumberOfPoints;
            SweepSettingsIFBandwidth    = DefaultValues.SweepSettingsIFBandwidth;
            SweepSettingsStart          = DefaultValues.SweepSettingsStart;
            SweepSettingsStop           = DefaultValues.SweepSettingsStop;
            SweepSettingsCenter         = DefaultValues.SweepSettingsCenter;
            SweepSettingsSpan           = DefaultValues.SweepSettingsSpan;
            SweepSettingsFixed          = DefaultValues.SweepSettingsFixed;

            IsStartStopCenterSpan = SweepSSCSTypeEnum.StartStop;
        }
        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetSweepType(Channel, SweepType);

            PNAX.SetIFBandwidth(Channel, SweepSettingsIFBandwidth);

            if ((SweepType == GeneralNFSweepTypeEnum.LinearSweep) ||
                (SweepType == GeneralNFSweepTypeEnum.LogFrequency))
            {
                PNAX.SetPoints(Channel, SweepSettingsNumberOfPoints);
                if (IsStartStopCenterSpan == SweepSSCSTypeEnum.StartStop)
                {
                    PNAX.SetStart(Channel, SweepSettingsStart);
                    PNAX.SetStop(Channel, SweepSettingsStop);
                }
                else
                {
                    PNAX.SetCenter(Channel, SweepSettingsCenter);
                    PNAX.SetSpan(Channel, SweepSettingsSpan);
                }
            }
            else if (SweepType == GeneralNFSweepTypeEnum.CWFrequency)
            {
                PNAX.SetPoints(Channel, SweepSettingsNumberOfPoints);
                PNAX.SetCWFreq(Channel, SweepSettingsFixed);
            }
            else if (SweepType == GeneralNFSweepTypeEnum.SegmentSweep)
            {
                SetSegmentValues();
            }
            else
            {
                throw new Exception("Undefined sweep type!");
            }


            UpgradeVerdict(Verdict.Pass);
        }
    }
}
