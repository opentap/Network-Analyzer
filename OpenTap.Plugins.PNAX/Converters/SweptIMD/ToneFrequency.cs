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
    public enum ToneFrequencySweepTypeEnum
    {
        [Scpi("FCEN")]
        [Display("Sweep fc")]
        SweepFc,
        [Scpi("DFR")]
        [Display("Sweep DeltaF")]
        SweepDeltaF,
        [Scpi("POW")]
        [Display("Power Sweep")]
        PowerSweep,
        [Scpi("CW")]
        [Display("CW")]
        CW,
        [Scpi("LOP")]
        [Display("LO Power Sweep")]
        LOPowerSweep
    }

    public enum XAxisDisplayAnnotationEnum
    {
        [Scpi("INP")]
        [Display("Input")]
        Input,
        [Scpi("OUTP")]
        [Display("Output")]
        Output,
        [Scpi("LO_1")]
        [Display("LO 1")]
        LO1,
        [Scpi("LO_2")]
        [Display("LO 2")]
        LO2
    }

    [AllowAsChildIn(typeof(SweptIMDChannel))]
    [Display("Tone Frequency", Groups: new[] { "Network Analyzer", "Converters", "Swept IMD Converters" }, Description: "Insert a description here", Order: 4)]
    public class ToneFrequency : ToneFrequencyBaseStep
    {
        #region Settings

        [Browsable(false)]

        private ToneFrequencySweepTypeEnum _ToneFrequencySweepType;
        [Display("Sweep Type", Groups: new[] { "Tone Frequency", "Sweep Type" }, Order: 1)]
        public ToneFrequencySweepTypeEnum ToneFrequencySweepType
        {
            get
            {
                return _ToneFrequencySweepType;
            }
            set
            {
                _ToneFrequencySweepType = value;

                IsSweepFCEnabled = value == ToneFrequencySweepTypeEnum.SweepFc;
                IsFixedDeltaFEnabled = IsSweepFCEnabled;
                IsSweepDeltaFEnabled = value == ToneFrequencySweepTypeEnum.SweepDeltaF;
                IsPowerSweepEnabled = value == ToneFrequencySweepTypeEnum.CW || value == ToneFrequencySweepTypeEnum.PowerSweep || value == ToneFrequencySweepTypeEnum.LOPowerSweep;

                // Update Channel value
                try
                {
                    var a = GetParent<SweptIMDChannel>();
                    // only if there is a parent of type SweptIMDChannel
                    if (a != null)
                    {
                        a.UpdateChannelSweepType(_ToneFrequencySweepType);
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("can't find parent yet! ex: " + ex.Message);
                }

            }
        }

        [Display("Annotation", Groups: new[] { "Tone Frequency", "X-Axis Display" }, Order: 50)]
        public XAxisDisplayAnnotationEnum XAxisDisplayAnnotation { get; set; }
        #endregion

        public ToneFrequency()
        {
            IsSweepPointsEnabled = true;
            IsSegmentEnabled = false;
            IsConverter = true;
        }

        protected override void UpdateTypeAndNotation()
        {
            var DefaultValues = PNAX.GetToneFrequencyDefaultValues();
            ToneFrequencySweepType = DefaultValues.ToneFrequencySweepType;
            XAxisDisplayAnnotation = DefaultValues.XAxisDisplayAnnotation;
        }

        protected override void UpdateMixerSweepPoints()
        {
            try
            {
                var a = GetParent<ConverterChannelBaseStep>();
                // only if there is a parent of type ScalarMixerChannel
                if (a != null)
                {
                    a.SweepPoints = SweepFcNumberOfPoints;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("can't find parent yet! ex: " + ex.Message);
            }
        }

        protected override void SetSweepType()
        {
            PNAX.SetIMDSweepType(Channel, ToneFrequencySweepType);
        }
    }
}
