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


    [AllowAsChildIn(typeof(SweptIMDChannel))]
    [Display("Tone Power", Groups: new[] { "Network Analyzer", "Converters", "Swept IMD Converters" }, Description: "Insert a description here")]
    public class TonePower : TonePowerBaseStep
    {
        #region Settings

        private ToneFrequencySweepTypeEnum _ToneFrequencySweepType;
        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
        [Display("Sweep Type", Order: 0.5)]
        public ToneFrequencySweepTypeEnum ToneFrequencySweepType 
        {
            get
            {
                return _ToneFrequencySweepType;
            }
            set
            {
                _ToneFrequencySweepType = value;
                IsPowerSweep = _ToneFrequencySweepType == ToneFrequencySweepTypeEnum.PowerSweep;
            } 
        }

        #endregion

        public TonePower()
        {
            IsConverter = true;
        }
    }
}
