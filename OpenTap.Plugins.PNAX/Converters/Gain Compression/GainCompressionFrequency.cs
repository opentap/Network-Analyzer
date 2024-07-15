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
    //[AllowAsChildIn(typeof(GainCompressionChannel))]
    [Display("Gain Compression Frequency", Groups: new[] { "Network Analyzer", "Converters", "Gain Compression Converters" }, Description: "Insert a description here")]
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
