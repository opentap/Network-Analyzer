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
    public class GeneralGainCompressionFrequency : FrequencyBaseStep
    {
        #region Settings

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
                EnableSegmentSweepSettings = value == GeneralGainCompressionSweepTypeEnum.SegmentSweep;
                LinearSweepEnabled = !EnableSegmentSweepSettings;
                CWFrequencyEnabled = false;
            }
        }

        [Display("Data Acquisition Mode", Order: 2)]
        public DataAcquisitionModeEnum DataAcquisitionMode { get; set; }

        #endregion

        public GeneralGainCompressionFrequency()
        {
            SweepType = GeneralGainCompressionSweepTypeEnum.LinearSweep;
            DataAcquisitionMode = DataAcquisitionModeEnum.SMARTSweep;
            CWFrequencyEnabled = false;
        }

        protected override void SetSweepType()
        {
            PNAX.SetSweepType(Channel, SweepType);
        }

        protected override void SetMode()
        {
            PNAX.SetDataAcquisitionMode(Channel, DataAcquisitionMode);
        }
    }
}
