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


    [AllowAsChildIn(typeof(GeneralNoiseFigureChannel))]
    [Display("Noise Figure Frequency", Groups: new[] { "PNA-X", "General", "Noise Figure Cold Source" }, Description: "Insert a description here")]
    public class GeneralNoiseFigureFrequency : GeneralFrequencyBaseStep
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

        }
        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetSweepType(Channel, SweepType);

            PNAX.SetPoints(Channel, SweepSettingsNumberOfPoints);
            PNAX.SetIFBandwidth(Channel, SweepSettingsIFBandwidth);

            if ((SweepType == GeneralNFSweepTypeEnum.LinearSweep) ||
                (SweepType == GeneralNFSweepTypeEnum.LogFrequency))
            {
                PNAX.SetStart(Channel, SweepSettingsStart);
                PNAX.SetStop(Channel, SweepSettingsStop);
                PNAX.SetCenter(Channel, SweepSettingsCenter);
                PNAX.SetSpan(Channel, SweepSettingsSpan);
            }
            else if (SweepType == GeneralNFSweepTypeEnum.CWFrequency)
            {
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
