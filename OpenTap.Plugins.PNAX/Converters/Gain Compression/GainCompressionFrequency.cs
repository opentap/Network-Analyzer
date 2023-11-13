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

        [Display("Data Acquisition Mode", Order: 2)]
        public DataAcquisitionModeEnum DataAcquisitionMode { get; set; }
        #endregion

        public GainCompressionFrequency()
        {
            IsConverter = true;
            SweepType = SweepTypeEnum.LinearSweep;
            DataAcquisitionMode = DataAcquisitionModeEnum.SMARTSweep;
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
