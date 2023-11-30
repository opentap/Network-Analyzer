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
    public enum GeneralToneFrequencySweepTypeEnum
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
        [Scpi("SEGM")]
        [Display("Segment Sweep fc")]
        SegmentSweepfc
    }


    [AllowAsChildIn(typeof(GeneralSweptIMDChannel))]
    [Display("Tone Frequency", Groups: new[] { "PNA-X", "General", "Swept IMD" }, Description: "Insert a description here", Order: 4)]
    public class GeneralToneFrequency : ToneFrequencyBaseStep
    {
        #region Settings

        [Browsable(false)]

        private GeneralToneFrequencySweepTypeEnum _ToneFrequencySweepType;
        [Display("Sweep Type", Groups: new[] { "Tone Frequency", "Sweep Type" }, Order: 1)]
        public GeneralToneFrequencySweepTypeEnum ToneFrequencySweepType
        {
            get
            {
                return _ToneFrequencySweepType;
            }
            set
            {
                _ToneFrequencySweepType = value;
                // Update Segment Settings Visibility
                IsSegmentEnabled = value == GeneralToneFrequencySweepTypeEnum.SegmentSweepfc;
                IsSweepFCEnabled = value == GeneralToneFrequencySweepTypeEnum.SweepFc;
                IsFixedDeltaFEnabled = IsSweepFCEnabled || IsSegmentEnabled;
                IsSweepDeltaFEnabled = value == GeneralToneFrequencySweepTypeEnum.SweepDeltaF;
                IsPowerSweepEnabled = value == GeneralToneFrequencySweepTypeEnum.CW || value == GeneralToneFrequencySweepTypeEnum.PowerSweep;
                IsSweepPointsEnabled = !IsSegmentEnabled;

                // Update Channel value
                try
                {
                    var a = GetParent<GeneralSweptIMDChannel>();
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

        #endregion

        #region Segment Sweep
        [Browsable(false)]
        public bool EnableSegmentSweepSettings { get; set; } = false;

        //[EnabledIf("StandardSweepType", StandardSweepTypeEnum.SegmentSweep, HideIfDisabled = true)]
        [EnabledIf("EnableSegmentSweepSettings", true, HideIfDisabled = true)]
        [Display("Segment Definition Type", Group: "Sweep Properties", Order: 30)]
        public SegmentDefinitionTypeEnum SegmentDefinitionType { get; set; }

        //[EnabledIf("StandardSweepType", StandardSweepTypeEnum.SegmentSweep, HideIfDisabled = true)]
        [EnabledIf("EnableSegmentSweepSettings", true, HideIfDisabled = true)]
        [EnabledIf("SegmentDefinitionType", SegmentDefinitionTypeEnum.File, HideIfDisabled = false)]
        [Display("Segment Table File Name", Group: "Sweep Properties", Order: 31)]
        [FilePath]
        public string SegmentTable { get; set; }

        //[EnabledIf("StandardSweepType", StandardSweepTypeEnum.SegmentSweep, HideIfDisabled = true)]
        [EnabledIf("EnableSegmentSweepSettings", true, HideIfDisabled = true)]
        [EnabledIf("SegmentDefinitionType", SegmentDefinitionTypeEnum.List, HideIfDisabled = false)]
        [Display("Segment Table", Group: "Sweep Properties", Order: 32)]
        public List<SegmentDefinition> SegmentDefinitions { get; set; }

        //[EnabledIf("StandardSweepType", StandardSweepTypeEnum.SegmentSweep, HideIfDisabled = true)]
        [EnabledIf("EnableSegmentSweepSettings", true, HideIfDisabled = true)]
        [EnabledIf("SegmentDefinitionType", SegmentDefinitionTypeEnum.List, HideIfDisabled = false)]
        [Display("Show Table", Group: "Sweep Properties", Order: 33)]
        public bool ShowTable { get; set; }


        #endregion

        public GeneralToneFrequency()
        {
        }

        protected override void UpdateTypeAndNotation()
        {
            var DefaultValues = PNAX.GetToneFrequencyDefaultValues();
            ToneFrequencySweepType = DefaultValues.GeneralToneFrequencySweepType;
        }

        protected override void SetSweepType()
        {
            PNAX.SetIMDSweepType(Channel, ToneFrequencySweepType);
        }

        protected override void SetSegmentValues()
        {
            PNAX.SetSegmentValues(SegmentDefinitionType, Channel, SegmentDefinitions, ShowTable);
        }
    }
}
