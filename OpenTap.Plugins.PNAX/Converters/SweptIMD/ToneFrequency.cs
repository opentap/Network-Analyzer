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
        SweepFc,
        SweepDeltaF,
        PowerSweep,
        CW,
        LOPowerSweep
    }

    [AllowAsChildIn(typeof(SweptIMDChannel))]
    [Display("Tone Frequency", Groups: new[] { "PNA-X", "Converters", "Swept IMD Converters" }, Description: "Insert a description here", Order: 4)]
    public class ToneFrequency : TestStep
    {
        #region Settings

        [Browsable(false)]
        public bool IsToneFrequencySweepFc { get; set; }
        [Browsable(false)]
        public bool IsToneFrequencySweepDeltaF { get; set; }
        [Browsable(false)]
        public bool IsToneFrequencyPowerSweep { get; set; }
        [Browsable(false)]
        public bool IsToneFrequencyCW { get; set; }
        [Browsable(false)]
        public bool IsToneFrequencyLOPowerSweep { get; set; }


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

                IsToneFrequencySweepFc = false;
                IsToneFrequencySweepDeltaF = false;
                IsToneFrequencyPowerSweep = false;
                IsToneFrequencyCW = false;
                IsToneFrequencyLOPowerSweep = false;
                switch (_ToneFrequencySweepType)
                {
                    case ToneFrequencySweepTypeEnum.SweepFc:
                        IsToneFrequencySweepFc = true;
                        break;
                    case ToneFrequencySweepTypeEnum.SweepDeltaF:
                        IsToneFrequencySweepDeltaF = true;
                        break;
                    case ToneFrequencySweepTypeEnum.PowerSweep:
                        IsToneFrequencyPowerSweep = true;
                        break;
                    case ToneFrequencySweepTypeEnum.CW:
                        IsToneFrequencyCW = true;
                        break;
                    case ToneFrequencySweepTypeEnum.LOPowerSweep:
                        IsToneFrequencyLOPowerSweep = true;
                        break;
                }
            }
        }


        [EnabledIf("IsToneFrequencySweepFc", true, HideIfDisabled = true)]
        [Display("Start fc", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 10)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepFcStartFc { get; set; }

        [EnabledIf("IsToneFrequencySweepFc", true, HideIfDisabled = true)]
        [Display("Stop fc", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 11)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepFcStopFc { get; set; }

        [EnabledIf("IsToneFrequencySweepFc", true, HideIfDisabled = true)]
        [Display("Center fc", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 12)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepFcCenterFc { get; set; }

        [EnabledIf("IsToneFrequencySweepFc", true, HideIfDisabled = true)]
        [Display("Span fc", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 13)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepFcSpanFc { get; set; }

        [EnabledIf("IsToneFrequencySweepFc", true, HideIfDisabled = true)]
        [Display("Fixed DeltaF", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 14)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepFcFixedDeltaF { get; set; }




        [EnabledIf("IsToneFrequencySweepDeltaF", true, HideIfDisabled = true)]
        [Display("Start DeltaF", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 20)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepDeltaFStartDeltaF { get; set; }

        [EnabledIf("IsToneFrequencySweepDeltaF", true, HideIfDisabled = true)]
        [Display("Stop DeltaF", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 21)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepDeltaFStopDeltaF { get; set; }

        [EnabledIf("IsToneFrequencySweepDeltaF", true, HideIfDisabled = true)]
        [Display("Fixed fc", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 22)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepDeltaFFixedFc { get; set; }




        [EnabledIf("IsToneFrequencyPowerSweep", true, HideIfDisabled = true)]
        [EnabledIf("IsToneFrequencyCW", true, HideIfDisabled = true)]
        [EnabledIf("IsToneFrequencyLOPowerSweep", true, HideIfDisabled = true)]
        [Display("CW f1", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 30)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double PowerSweepCWF1 { get; set; }

        [EnabledIf("IsToneFrequencyPowerSweep", true, HideIfDisabled = true)]
        [EnabledIf("IsToneFrequencyCW", true, HideIfDisabled = true)]
        [EnabledIf("IsToneFrequencyLOPowerSweep", true, HideIfDisabled = true)]
        [Display("CW f2", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 31)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double PowerSweepCWF2 { get; set; }

        [EnabledIf("IsToneFrequencyPowerSweep", true, HideIfDisabled = true)]
        [EnabledIf("IsToneFrequencyCW", true, HideIfDisabled = true)]
        [EnabledIf("IsToneFrequencyLOPowerSweep", true, HideIfDisabled = true)]
        [Display("CW fc", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 32)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double PowerSweepCWFc { get; set; }

        [EnabledIf("IsToneFrequencyPowerSweep", true, HideIfDisabled = true)]
        [EnabledIf("IsToneFrequencyCW", true, HideIfDisabled = true)]
        [EnabledIf("IsToneFrequencyLOPowerSweep", true, HideIfDisabled = true)]
        [Display("CW DeltaF", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 33)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double PowerSweepCWDeltaF { get; set; }



        [Display("Number of Points", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 40)]
        public int SweepFcNumberOfPoints { get; set; }

        [Display("Main Tone IFBW", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 41)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepFcMixedToneIFBW { get; set; }

        [Display("IM Tone IFBW", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 42)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepFcIMToneIFBW { get; set; }

        [Display("Reduce IF BW at low frequencies", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 43)]
        public bool SweepFcReduceIFBW { get; set; }


        #endregion

        public ToneFrequency()
        {
            ToneFrequencySweepType = ToneFrequencySweepTypeEnum.SweepFc;
            SweepFcStartFc = 10.5e6;
            SweepFcStopFc = 66.9995e9;
            SweepFcCenterFc = 33.505e9;
            SweepFcSpanFc = 66.9995e9;
            SweepFcFixedDeltaF = 1e6;
            SweepFcNumberOfPoints = 201;
            SweepFcMixedToneIFBW = 1e3;
            SweepFcIMToneIFBW = 1e3;
            SweepFcReduceIFBW = true;

            SweepDeltaFStartDeltaF = 1e6;
            SweepDeltaFStopDeltaF = 10e6;
            SweepDeltaFFixedFc = 1e9;

            PowerSweepCWF1 = 999.5e6;
            PowerSweepCWF2 = 1.0005e9;
            PowerSweepCWFc = 1e9;
            PowerSweepCWDeltaF = 1e6;
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
