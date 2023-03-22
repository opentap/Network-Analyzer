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

    [AllowAsChildIn(typeof(ScalerMixerChannel))]
    [Display("Scaler Mixer Sweep", Groups: new[] { "PNA-X", "Converters", "Scaler Mixer Converter + Phase" }, Description: "Insert a description here")]
    public class ScalerMixerSweep : ConverterBaseStep
    {
        #region Settings
        private ScalerMixerSweepType _sweepType;
        [Display("Sweep Type", Order: 21)]
        public ScalerMixerSweepType SweepType
        {
            get { return _sweepType; }
            set
            {
                _sweepType = value;
                if (value == ScalerMixerSweepType.SegmentSweep)
                    PhasePoint = ScalerMixerPhasePoint.MiddlePoint;
            }
        }

        [Display("X-Axis Point Spacing", Order: 22)]
        public bool IsXAxisPointSpacing { get; set; }

        [Display("Avoid Spurs", Order: 23)]
        public bool IsAvoidSpurs { get; set; }

        [Display("Reversed Port 2 Coupler", Order: 24)]
        public bool IsReversedPortTwoCoupler { get; set; }

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

        [Display("IF Bandwidth", Order: 26)]
        [Unit("kHz", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double IFBandwidth { get; set; }

        [Display("Enable Phase", Group: "Phase Reference Point", Order: 31)]
        public bool IsEnablePhase { get; set; }

        [Display("Use Absolute Phase", Group: "Phase Reference Point", Order: 32)]
        public bool UseAbsolutePhase { get; set; }

        [EnabledIf("UseAbsolutePhase", false, HideIfDisabled = true)]
        [Display("Phase Reference Point", Group: "Phase Reference Point", Order: 33)]
        public ScalerMixerPhasePoint PhasePoint { get; set; }

        private int _phasePointValue = 1;
        [EnabledIf("PhasePoint", ScalerMixerPhasePoint.SpecifyPoint)]
        [Display("Phase Reference Point Value", Group: "Phase Reference Point", Order: 34)]
        public int PhasePointValue
        {
            get
            {
                if (SweepType == ScalerMixerSweepType.SegmentSweep)
                    return 11;
                else
                    switch (PhasePoint)
                    {
                        case ScalerMixerPhasePoint.FirstPoint:
                            return 1;
                        case ScalerMixerPhasePoint.MiddlePoint:
                            return (int)Math.Ceiling(NumberOfPoints / 2.0);
                        case ScalerMixerPhasePoint.LastPoint:
                            return _numberOfPoints;
                        case ScalerMixerPhasePoint.SpecifyPoint:
                            return _phasePointValue;
                        default:
                            return _phasePointValue;
                    }
            }
            set
            {
                _phasePointValue = value;
            }
        }

        #endregion

        public ScalerMixerSweep()
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
