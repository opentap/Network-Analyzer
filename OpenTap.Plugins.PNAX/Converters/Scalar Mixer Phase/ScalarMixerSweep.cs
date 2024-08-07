﻿// Author: MyName
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


    //[AllowAsChildIn(typeof(ScalarMixerChannel))]
    [Display("Scalar Mixer Sweep", Groups: new[] { "Network Analyzer", "Converters", "Scalar Mixer Converter + Phase" }, Description: "Insert a description here")]
    public class ScalarMixerSweep : PNABaseStep
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
                {
                    PhasePoint = ScalerMixerPhasePoint.MiddlePoint;
                }

                // Update Sweep Type on Power Test Step
                try
                {
                    var a = GetParent<ScalarMixerChannel>();
                    // only if there is a parent of type ScalarMixerChannel
                    if (a != null)
                    {
                        a.UpdateChannelSweepType(_sweepType);
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("can't find parent yet! ex: " + ex.Message);
                }

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

                // Update Points on Parent step
                try
                {
                    var a = GetParent<ConverterChannelBaseStep>();
                    // only if there is a parent of type ScalarMixerChannel
                    if (a != null)
                    {
                        a.SweepPoints = _numberOfPoints;
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("can't find parent yet! ex: " + ex.Message);
                }

            }
        }

        [Display("IF Bandwidth", Order: 26)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double IFBandwidth { get; set; }

        [Display("Enable Phase", Group: "Phase Reference Point", Order: 31)]
        public bool IsEnablePhase { get; set; }

        [EnabledIf("IsEnablePhase", true, HideIfDisabled = true)]
        [Display("Phase Reference Point", Group: "Phase Reference Point", Order: 33)]
        public ScalerMixerPhasePoint PhasePoint { get; set; }

        // TODO
        // validate internal source as LO from MixerSetupTestStep.cs
        // if its NotControlled then disable option AbsolutePhase 
        // TODO
        private int _phasePointValue = 1;
        [EnabledIf("IsEnablePhase", true, HideIfDisabled = false)]
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
                            return NumberOfPoints;
                        case ScalerMixerPhasePoint.SpecifyPoint:
                            return _phasePointValue;
                        case ScalerMixerPhasePoint.AbsolutePhase:
                            return (int)Math.Ceiling(NumberOfPoints / 2.0);
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

        public ScalarMixerSweep()
        {
            UpdateDefaultValues();
        }
        
        private void UpdateDefaultValues()
        {
            var DefaultValues = PNAX.GetMixerSweepDefaultValues();

            SweepType                = DefaultValues.SweepType;
            IsXAxisPointSpacing      = DefaultValues.IsXAxisPointSpacing;
            IsReversedPortTwoCoupler = DefaultValues.IsReversedPortTwoCoupler;
            IsAvoidSpurs             = DefaultValues.IsAvoidSpurs;
            NumberOfPoints           = DefaultValues.NumberOfPoints;
            IFBandwidth              = DefaultValues.IFBandwidth;
            IsEnablePhase = DefaultValues.IsEnablePhase;
            PhasePoint = DefaultValues.PhasePoint;
        }
        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetStandardSweepType(Channel, SweepType);

            if (SweepType == ScalerMixerSweepType.SegmentSweep)
            {
                // X-Axis Point Spacing option is enabled
                PNAX.SetXAxisPointSpacing(Channel, IsXAxisPointSpacing);
                // Number of Points forced to 21
                // Phase reference point forced to Normalize Middle Point
            }

            PNAX.SetAvoidSpurs(Channel, IsAvoidSpurs);
            PNAX.SetReversedPort2Coupler(Channel, IsReversedPortTwoCoupler);
            PNAX.SetPoints(Channel, NumberOfPoints);
            PNAX.SetIFBandwidth(Channel, IFBandwidth);

            PNAX.SetMixerPhase(Channel, IsEnablePhase);
            PNAX.SetNormalizingDataPoint(Channel, PhasePointValue);
            if (PhasePoint == ScalerMixerPhasePoint.AbsolutePhase)
            {
                PNAX.SetMixerUseAbsolutePhase(Channel, true);
            }
            else
            {
                PNAX.SetMixerUseAbsolutePhase(Channel, false);
            }
            UpgradeVerdict(Verdict.Pass);
        }

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            List<(string, object)> retVal = new List<(string, object)>
            {
                ("SMC_SweepType", SweepType)
            };

            if (SweepType == ScalerMixerSweepType.SegmentSweep)
            {
                retVal.Add(("IsXAxisPointSpacing", IsXAxisPointSpacing));
            }

            retVal.Add(("SMC_AvoidSpurs", IsAvoidSpurs));
            retVal.Add(("SMC_ReversedPortTwoCoupler", IsReversedPortTwoCoupler));
            retVal.Add(("SMC_NumberOfPoints", NumberOfPoints));
            retVal.Add(("SMC_IFBandwidth", IFBandwidth));

            retVal.Add(("SMC_EnablePhase", IsEnablePhase));
            retVal.Add(("SMC_PhasePointValue", PhasePointValue));

            retVal.Add(("PhaseReferencePoint", PhasePoint));

            return retVal;
        }

    }
}
