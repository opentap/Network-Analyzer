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
    [AllowAsChildIn(typeof(GainCompressionChannel))]
    [Display("Gain Compression Frequency", Groups: new[] { "PNA-X", "Converters", "Gain Compression Converters" }, Description: "Insert a description here")]
    public class GainCompressionFrequency : FrequencyBaseStep
    {
        #region Settings
        [Display("Data Acquisition Mode", Order: 2)]
        public DataAcquisitionModeEnum DataAcquisitionMode { get; set; }
        #endregion

        public GainCompressionFrequency()
        {
            UpdateDefaultValues();
        }

        public void UpdateDefaultValues()
        {
            var DefaultValues = PNAX.GetConverterFrequencyDefaultValues();
            SweepType                   = DefaultValues.SweepType;
            DataAcquisitionMode         = DataAcquisitionModeEnum.SMARTSweep;

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

            PNAX.SetDataAcquisitionMode(Channel, DataAcquisitionMode);

            PNAX.SetPoints(Channel, SweepSettingsNumberOfPoints);
            PNAX.SetIFBandwidth(Channel, SweepSettingsIFBandwidth);

            if (SweepType == SweepTypeEnum.LinearSweep)
            {
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
            else if (SweepType == SweepTypeEnum.CWFrequency)
            {
                PNAX.SetCWFreq(Channel, SweepSettingsFixed);
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
