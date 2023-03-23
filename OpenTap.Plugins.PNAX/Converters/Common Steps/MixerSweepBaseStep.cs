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
    public enum ScalerMixerSweepType
    {
        LinearFrequency = StandardSweepTypeEnum.LinearFrequency,
        CWTime = StandardSweepTypeEnum.CWTime,
        SegmentSweep = StandardSweepTypeEnum.SegmentSweep,
        [Display("Power")]
        Power = StandardSweepTypeEnum.PowerSweep
    }

    public enum ScalerMixerPhasePoint
    {
        [Display("Normalize First Point")]
        FirstPoint,
        [Display("Normalize Middle Point")]
        MiddlePoint,
        [Display("Normalize Last Point")]
        LastPoint,
        [Display("Specify Normalization Point")]
        SpecifyPoint
    }


    [Browsable(false)]
    public class MixerSweepBaseStep : ConverterBaseStep
    {
        #region Settings
        [Display("Sweep Type", Order: 21)]
        public virtual ScalerMixerSweepType SweepType { get; set; }

        [Display("X-Axis Point Spacing", Order: 22)]
        public bool IsXAxisPointSpacing { get; set; }

        [Display("Avoid Spurs", Order: 23)]
        public bool IsAvoidSpurs { get; set; }

        private int _numberOfPoints;
        [EnabledIf("SweepType", ScalerMixerSweepType.LinearFrequency, ScalerMixerSweepType.CWTime, ScalerMixerSweepType.Power)]
        [Display("Number Of Points", Order: 25)]
        public int NumberOfPoints
        {
            get
            {
                if (SweepType == ScalerMixerSweepType.SegmentSweep)
                    return 21;
                else
                    return _numberOfPoints;
            }
            set
            {
                _numberOfPoints = value;
            }
        }
        #endregion

        public MixerSweepBaseStep()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
