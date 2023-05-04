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
    public enum GeneralGainCompressionSweepTypeEnum
    {
        [Scpi("LIN")]
        [Display("Linear Sweep")]
        LinearSweep,
        [Scpi("LOGarithmic")]
        [Display("Log Frequency")]
        LogFrequency,
        [Scpi("SEGMent")]
        [Display("Segment Sweep")]
        SegmentSweep
    }

    [AllowAsChildIn(typeof(GeneralGainCompressionChannel))]
    [Display("Gain Compression Frequency", Groups: new[] { "PNA-X", "General", "Gain Compression" }, Description: "Insert a description here")]
    public class GeneralGainCompressionFrequency : GeneralBaseStep
    {
        #region Settings
        [Display("Data Acquisition Mode", Order: 2)]
        public DataAcquisitionModeEnum DataAcquisitionMode { get; set; }

        private GeneralGainCompressionSweepTypeEnum _SweepType;
        [Display("Sweep Type", Order: 1)]
        public GeneralGainCompressionSweepTypeEnum SweepType
        {
            get
            {
                return _SweepType;
            }
            set
            {
                _SweepType = value;
                EnableSegmentSweepSettings = false;
                if (_SweepType == GeneralGainCompressionSweepTypeEnum.SegmentSweep)
                {
                    EnableSegmentSweepSettings = true;
                }
            }
        }



        [Display("Number Of Points", Group: "Sweep Settings", Order: 10)]
        public int SweepSettingsNumberOfPoints { get; set; }

        [Display("IF Bandwidth", Group: "Sweep Settings", Order: 11)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double SweepSettingsIFBandwidth { get; set; }

        [EnabledIf("SweepType", GeneralGainCompressionSweepTypeEnum.LinearSweep, HideIfDisabled = true)]
        [Display("Start", Group: "Sweep Settings", Order: 12)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double SweepSettingsStart { get; set; }

        [EnabledIf("SweepType", GeneralGainCompressionSweepTypeEnum.LinearSweep, HideIfDisabled = true)]
        [Display("Stop", Group: "Sweep Settings", Order: 13)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsStop { get; set; }

        [EnabledIf("SweepType", GeneralGainCompressionSweepTypeEnum.LinearSweep, HideIfDisabled = true)]
        [Display("Center", Group: "Sweep Settings", Order: 14)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsCenter { get; set; }

        [EnabledIf("SweepType", GeneralGainCompressionSweepTypeEnum.LinearSweep, HideIfDisabled = true)]
        [Display("Span", Group: "Sweep Settings", Order: 15)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsSpan { get; set; }

        //[EnabledIf("SweepType", GeneralGainCompressionSweepTypeEnum.CWFrequency, HideIfDisabled = true)]
        //[Display("Fixed", Group: "Sweep Settings", Order: 16)]
        //[Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        //public double SweepSettingsFixed { get; set; }

        #endregion

        public GeneralGainCompressionFrequency()
        {
            UpdateDefaultValues();
        }

        public void UpdateDefaultValues()
        {
            var DefaultValues = PNAX.GetConverterFrequencyDefaultValues();
            SweepType = GeneralGainCompressionSweepTypeEnum.LinearSweep;
            DataAcquisitionMode         = DataAcquisitionModeEnum.SMARTSweep;

            SweepSettingsNumberOfPoints = DefaultValues.SweepSettingsNumberOfPoints;
            SweepSettingsIFBandwidth    = DefaultValues.SweepSettingsIFBandwidth;
            SweepSettingsStart          = DefaultValues.SweepSettingsStart;
            SweepSettingsStop           = DefaultValues.SweepSettingsStop;
            SweepSettingsCenter         = DefaultValues.SweepSettingsCenter;
            SweepSettingsSpan           = DefaultValues.SweepSettingsSpan;

            //SweepSettingsFixed          = DefaultValues.SweepSettingsFixed;

        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetSweepType(Channel, SweepType);

            PNAX.SetDataAcquisitionMode(Channel, DataAcquisitionMode);

            if ((SweepType == GeneralGainCompressionSweepTypeEnum.LinearSweep) ||
                (SweepType == GeneralGainCompressionSweepTypeEnum.LogFrequency))
            {
                PNAX.SetPoints(Channel, SweepSettingsNumberOfPoints);
                PNAX.SetIFBandwidth(Channel, SweepSettingsIFBandwidth);

                PNAX.SetStart(Channel, SweepSettingsStart);
                PNAX.SetStop(Channel, SweepSettingsStop);
                PNAX.SetCenter(Channel, SweepSettingsCenter);
                PNAX.SetSpan(Channel, SweepSettingsSpan);
            }
            //else if (SweepType == GeneralGainCompressionSweepTypeEnum.CWFrequency)
            //{
            //    PNAX.SetCWFreq(Channel, SweepSettingsFixed);
            //}
            else if (SweepType == GeneralGainCompressionSweepTypeEnum.SegmentSweep)
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
