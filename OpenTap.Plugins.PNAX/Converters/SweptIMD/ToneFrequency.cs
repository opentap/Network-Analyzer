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
    [Display("Tone Frequency", Groups: new[] { "PNA-X", "Converters", "Swept IMD Converters" }, Description: "Insert a description here", Order: 4)]
    public class ToneFrequency : ConverterBaseStep
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


        [EnabledIf("ToneFrequencySweepType", ToneFrequencySweepTypeEnum.SweepFc, HideIfDisabled = true)]
        [Display("Type", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 9.9)]
        public SweepSSCSTypeEnum IsStartStopCenterSpan { get; set; }

        [EnabledIf("ToneFrequencySweepType", ToneFrequencySweepTypeEnum.SweepFc, HideIfDisabled = true)]
        [EnabledIf("IsStartStopCenterSpan", SweepSSCSTypeEnum.StartStop, HideIfDisabled = true)]
        [Display("Start fc", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 10)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepFcStartFc { get; set; }

        [EnabledIf("ToneFrequencySweepType", ToneFrequencySweepTypeEnum.SweepFc, HideIfDisabled = true)]
        [EnabledIf("IsStartStopCenterSpan", SweepSSCSTypeEnum.StartStop, HideIfDisabled = true)]
        [Display("Stop fc", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 11)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepFcStopFc { get; set; }

        [EnabledIf("ToneFrequencySweepType", ToneFrequencySweepTypeEnum.SweepFc, HideIfDisabled = true)]
        [EnabledIf("IsStartStopCenterSpan", SweepSSCSTypeEnum.CenterSpan, HideIfDisabled = true)]
        [Display("Center fc", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 12)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepFcCenterFc { get; set; }

        [EnabledIf("ToneFrequencySweepType", ToneFrequencySweepTypeEnum.SweepFc, HideIfDisabled = true)]
        [EnabledIf("IsStartStopCenterSpan", SweepSSCSTypeEnum.CenterSpan, HideIfDisabled = true)]
        [Display("Span fc", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 13)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepFcSpanFc { get; set; }

        [EnabledIf("ToneFrequencySweepType", ToneFrequencySweepTypeEnum.SweepFc, HideIfDisabled = true)]
        [Display("Fixed DeltaF", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 14)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepFcFixedDeltaF { get; set; }

        [EnabledIf("ToneFrequencySweepType", ToneFrequencySweepTypeEnum.SweepDeltaF, HideIfDisabled = true)]
        [Display("Start DeltaF", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 20)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepDeltaFStartDeltaF { get; set; }

        [EnabledIf("ToneFrequencySweepType", ToneFrequencySweepTypeEnum.SweepDeltaF, HideIfDisabled = true)]
        [Display("Stop DeltaF", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 21)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepDeltaFStopDeltaF { get; set; }

        [EnabledIf("ToneFrequencySweepType", ToneFrequencySweepTypeEnum.SweepDeltaF, HideIfDisabled = true)]
        [Display("Fixed fc", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 22)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepDeltaFFixedFc { get; set; }

        [EnabledIf("ToneFrequencySweepType", ToneFrequencySweepTypeEnum.PowerSweep, ToneFrequencySweepTypeEnum.CW, ToneFrequencySweepTypeEnum.LOPowerSweep, HideIfDisabled = true)]
        [Display("CW f1", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 30)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double PowerSweepCWF1 { get; set; }

        [EnabledIf("ToneFrequencySweepType", ToneFrequencySweepTypeEnum.PowerSweep, ToneFrequencySweepTypeEnum.CW, ToneFrequencySweepTypeEnum.LOPowerSweep, HideIfDisabled = true)]
        [Display("CW f2", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 31)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double PowerSweepCWF2 { get; set; }

        [EnabledIf("ToneFrequencySweepType", ToneFrequencySweepTypeEnum.PowerSweep, ToneFrequencySweepTypeEnum.CW, ToneFrequencySweepTypeEnum.LOPowerSweep, HideIfDisabled = true)]
        [Display("CW fc", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 32)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double PowerSweepCWFc { get; set; }

        [EnabledIf("ToneFrequencySweepType", ToneFrequencySweepTypeEnum.PowerSweep, ToneFrequencySweepTypeEnum.CW, ToneFrequencySweepTypeEnum.LOPowerSweep, HideIfDisabled = true)]
        [Display("CW DeltaF", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 33)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double PowerSweepCWDeltaF { get; set; }

        private int _SweepFcNumberOfPoints;
        [Display("Number of Points", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 40)]
        public int SweepFcNumberOfPoints
        {
            get
            {
                return _SweepFcNumberOfPoints;
            }
            set
            {
                _SweepFcNumberOfPoints = value;
                // Update Points on Parent step
                try
                {
                    var a = GetParent<ConverterChannelBase>();
                    // only if there is a parent of type ScalarMixerChannel
                    if (a != null)
                    {
                        a.SweepPoints = _SweepFcNumberOfPoints;
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("can't find parent yet! ex: " + ex.Message);
                }

            }
        }

        [Display("Main Tone IFBW", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 41)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepFcMixedToneIFBW { get; set; }

        [Display("IM Tone IFBW", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 42)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepFcIMToneIFBW { get; set; }

        [Display("Reduce IF BW at low frequencies", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 43)]
        public bool SweepFcReduceIFBW { get; set; }


        [Display("Annotation", Groups: new[] { "Tone Frequency", "X-Axis Display" }, Order: 50)]
        public XAxisDisplayAnnotationEnum XAxisDisplayAnnotation { get; set; }
        #endregion

        public ToneFrequency()
        {
            UpdateDefaultValues();
        }

        private void UpdateDefaultValues()
        {
            var DefaultValues = PNAX.GetToneFrequencyDefaultValues();
            ToneFrequencySweepType = DefaultValues.ToneFrequencySweepType;
            SweepFcStartFc = DefaultValues.SweepFcStartFc;
            SweepFcStopFc = DefaultValues.SweepFcStopFc;
            SweepFcCenterFc = DefaultValues.SweepFcCenterFc;
            SweepFcSpanFc = DefaultValues.SweepFcSpanFc;
            SweepFcFixedDeltaF = DefaultValues.SweepFcFixedDeltaF;
            SweepFcNumberOfPoints = DefaultValues.SweepFcNumberOfPoints;
            SweepFcMixedToneIFBW = DefaultValues.SweepFcMixedToneIFBW;
            SweepFcIMToneIFBW = DefaultValues.SweepFcIMToneIFBW;
            SweepFcReduceIFBW = DefaultValues.SweepFcReduceIFBW;
            SweepDeltaFStartDeltaF = DefaultValues.SweepDeltaFStartDeltaF;
            SweepDeltaFStopDeltaF = DefaultValues.SweepDeltaFStopDeltaF;
            SweepDeltaFFixedFc = DefaultValues.SweepDeltaFFixedFc;
            PowerSweepCWF1 = DefaultValues.PowerSweepCWF1;
            PowerSweepCWF2 = DefaultValues.PowerSweepCWF2;
            PowerSweepCWFc = DefaultValues.PowerSweepCWFc;
            PowerSweepCWDeltaF = DefaultValues.PowerSweepCWDeltaF;
            XAxisDisplayAnnotation = DefaultValues.XAxisDisplayAnnotation;
            IsStartStopCenterSpan = SweepSSCSTypeEnum.StartStop;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetIMDSweepType(Channel, ToneFrequencySweepType);

            switch (ToneFrequencySweepType)
            {
                case ToneFrequencySweepTypeEnum.SweepFc:
                    if (IsStartStopCenterSpan == SweepSSCSTypeEnum.StartStop)
                    {
                        PNAX.SetIMDSweepSettingsStartfc(Channel, SweepFcStartFc);
                        PNAX.SetIMDSweepSettingsStopfc(Channel, SweepFcStopFc);
                    }
                    else
                    {
                        PNAX.SetIMDSweepSettingsCenterfc(Channel, SweepFcCenterFc);
                        PNAX.SetIMDSweepSettingsSpanfc(Channel, SweepFcSpanFc);
                    }
                    PNAX.SetIMDSweepSettingsFixedDeltaF(Channel, SweepFcFixedDeltaF);
                    break;
                case ToneFrequencySweepTypeEnum.SweepDeltaF:
                    PNAX.SetIMDSweepSettingsStartDeltaF(Channel, SweepDeltaFStartDeltaF);
                    PNAX.SetIMDSweepSettingsStopDeltaF(Channel, SweepDeltaFStopDeltaF);
                    PNAX.SetIMDSweepSettingsCenterfcFixed(Channel, SweepDeltaFFixedFc);
                    break;
                case ToneFrequencySweepTypeEnum.PowerSweep:
                case ToneFrequencySweepTypeEnum.CW:
                    PNAX.SetIMDSweepSettingsFixedf1(Channel, PowerSweepCWF1);
                    PNAX.SetIMDSweepSettingsFixedf2(Channel, PowerSweepCWF2);
                    PNAX.SetIMDSweepSettingsCenterfcFixed(Channel, PowerSweepCWFc);
                    PNAX.SetIMDSweepSettingsFixedDeltaF(Channel, PowerSweepCWDeltaF);
                    break;
                case ToneFrequencySweepTypeEnum.LOPowerSweep:
                    // LO - These values are set on the MixerFrequencyTestStep
                    // so no need to set these values here
                    //PNAX.SetLOSweptPowerStart(Channel, 1, LO1SweptPowerStart);
                    //PNAX.SetLOSweptPowerStop(Channel, 1, LO1SweptPowerStop);

                    PNAX.SetIMDSweepSettingsFixedf1(Channel, PowerSweepCWF1);
                    PNAX.SetIMDSweepSettingsFixedf2(Channel, PowerSweepCWF2);
                    PNAX.SetIMDSweepSettingsCenterfcFixed(Channel, PowerSweepCWFc);
                    PNAX.SetIMDSweepSettingsFixedDeltaF(Channel, PowerSweepCWDeltaF);
                    break;
            }

            PNAX.SetPoints(Channel, SweepFcNumberOfPoints);
            PNAX.SetIMDSweepSettingsMainToneIFBW(Channel, SweepFcMixedToneIFBW);
            PNAX.SetIMDSweepSettingsIMToneIFBW(Channel, SweepFcIMToneIFBW);
            PNAX.SetLFAutoBW(Channel, SweepFcReduceIFBW);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
