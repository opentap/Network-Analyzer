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
    public enum SweepTypeEnum
    {
        [Scpi("LIN")]
        [Display("Linear Sweep")]
        LinearSweep,
        [Scpi("CW")]
        [Display("CW Frequency")]
        CWFrequency
    }

    public enum DataAcquisitionModeEnum
    {
        [Scpi("SMAR")]
        [Display("SMART Sweep")]
        SMARTSweep,
        [Scpi("PFREQ")]
        [Display("Sweep Power Per Frequency 2D")]
        SweepPowerPerFrequency2D,
        [Scpi("FPOW")]
        [Display("Sweep Frequency Per Power 2D")]
        SweepFrequencyPerPower2D
    }

    [AllowAsChildIn(typeof(GainCompressionChannel))]
    [Display("Frequency", Groups: new[] { "PNA-X", "Converters", "Compression" }, Description: "Insert a description here")]
    public class Frequency : ConverterCompressionBaseStep
    {
        #region Settings
        [Browsable(false)]
        public bool IsLinearSweep { get; set; }
        [Browsable(false)]
        public bool IsCWFrequency { get; set; }


        private SweepTypeEnum _SweepType;
        [Display("Sweep Type", Order: 1)]
        public SweepTypeEnum SweepType
        {
            get
            {
                return _SweepType;
            }
            set
            {
                _SweepType = value;

                IsLinearSweep = false;
                IsCWFrequency = false;
                switch (_SweepType)
                {
                    case SweepTypeEnum.LinearSweep:
                        IsLinearSweep = true;
                        break;
                    case SweepTypeEnum.CWFrequency:
                        IsCWFrequency = true;
                        break;
                }
            }
        }


        [Browsable(false)]
        public bool IsSMARTSweep { get; set; }
        [Browsable(false)]
        public bool IsSweepPowerPerFrequency { get; set; }
        [Browsable(false)]
        public bool IsSweepFrequencyPerPower { get; set; }


        private DataAcquisitionModeEnum _DataAcquisitionMode;
        [Display("Data Acquisition Mode", Order: 2)]
        public DataAcquisitionModeEnum DataAcquisitionMode
        {
            get
            {
                return _DataAcquisitionMode;
            }
            set
            {
                _DataAcquisitionMode = value;

                IsSMARTSweep = false;
                IsSweepPowerPerFrequency = false;
                IsSweepFrequencyPerPower = false;
                switch (_DataAcquisitionMode)
                {
                    case DataAcquisitionModeEnum.SMARTSweep:
                        IsSMARTSweep = true;
                        break;
                    case DataAcquisitionModeEnum.SweepPowerPerFrequency2D:
                        IsSweepPowerPerFrequency = true;
                        break;
                    case DataAcquisitionModeEnum.SweepFrequencyPerPower2D:
                        IsSweepFrequencyPerPower = true;
                        break;
                }
            }
        }


        [Display("Number Of Points", Group: "Sweep Settings", Order: 10)]
        public int SweepSettingsNumberOfPoints { get; set; }

        [Display("IF Bandwidth", Group: "Sweep Settings", Order: 11)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double SweepSettingsIFBandwidth { get; set; }

        [EnabledIf("IsLinearSweep", true, HideIfDisabled =true)]
        [Display("Start", Group: "Sweep Settings", Order: 12)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double SweepSettingsStart { get; set; }

        [EnabledIf("IsLinearSweep", true, HideIfDisabled = true)]
        [Display("Stop", Group: "Sweep Settings", Order: 13)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsStop { get; set; }

        [EnabledIf("IsLinearSweep", true, HideIfDisabled = true)]
        [Display("Center", Group: "Sweep Settings", Order: 14)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsCenter { get; set; }

        [EnabledIf("IsLinearSweep", true, HideIfDisabled = true)]
        [Display("Span", Group: "Sweep Settings", Order: 15)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsSpan { get; set; }

        [EnabledIf("IsCWFrequency", true, HideIfDisabled = true)]
        [Display("Fixed", Group: "Sweep Settings", Order: 16)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsFixed { get; set; }

        #endregion

        public Frequency()
        {
            UpdateDefaultValues();
        }

        public void UpdateDefaultValues()
        {
            var DefaultValues = PNAX.GetConverterFrequencyDefaultValues();
            SweepType                   = DefaultValues.SweepType;
            DataAcquisitionMode         = DefaultValues.DataAcquisitionMode;

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

            PNAX.SetDataAcquisitionMode(Channel, DataAcquisitionMode);

            PNAX.SetPoints(Channel, SweepSettingsNumberOfPoints);
            PNAX.SetIFBandwidth(Channel, SweepSettingsIFBandwidth);

            if (SweepType == SweepTypeEnum.LinearSweep)
            {
                PNAX.SetStart(Channel, SweepSettingsStart);
                PNAX.SetStop(Channel, SweepSettingsStop);
                PNAX.SetCenter(Channel, SweepSettingsCenter);
                PNAX.SetSpan(Channel, SweepSettingsSpan);
            }
            else if (SweepType == SweepTypeEnum.CWFrequency)
            {
                PNAX.SetCWFreq(Channel, SweepSettingsFixed);
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
