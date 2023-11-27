using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTap.Plugins.PNAX
{
    [Browsable(false)]
    public class ToneFrequencyBaseStep : PNABaseStep
    {
        #region Settings

        [Browsable(false)]
        public bool IsSweepFCEnabled { get; set; }
        [EnabledIf("IsSweepFCEnabled", true, HideIfDisabled = true)]
        [Display("Type", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 9.9)]
        public SweepSSCSTypeEnum IsStartStopCenterSpan { get; set; }

        [EnabledIf("IsSweepFCEnabled", true, HideIfDisabled = true)]
        [EnabledIf("IsStartStopCenterSpan", SweepSSCSTypeEnum.StartStop, HideIfDisabled = true)]
        [Display("Start fc", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 10)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepFcStartFc { get; set; }

        [EnabledIf("IsSweepFCEnabled", true, HideIfDisabled = true)]
        [EnabledIf("IsStartStopCenterSpan", SweepSSCSTypeEnum.StartStop, HideIfDisabled = true)]
        [Display("Stop fc", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 11)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepFcStopFc { get; set; }

        [EnabledIf("IsSweepFCEnabled", true, HideIfDisabled = true)]
        [EnabledIf("IsStartStopCenterSpan", SweepSSCSTypeEnum.CenterSpan, HideIfDisabled = true)]
        [Display("Center fc", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 12)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepFcCenterFc { get; set; }


        [EnabledIf("IsSweepFCEnabled", true, HideIfDisabled = true)]
        [EnabledIf("IsStartStopCenterSpan", SweepSSCSTypeEnum.CenterSpan, HideIfDisabled = true)]
        [Display("Span fc", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 13)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepFcSpanFc { get; set; }

        [Browsable(false)]
        public bool IsFixedDeltaFEnabled { get; set; }
        [EnabledIf("IsFixedDeltaFEnabled", true, HideIfDisabled = true)]
        [Display("Fixed DeltaF", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 14)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepFcFixedDeltaF { get; set; }

        [Browsable(false)]
        public bool IsSweepDeltaFEnabled { get; set; }
        [EnabledIf("IsSweepDeltaFEnabled", true, HideIfDisabled = true)]
        [Display("Start DeltaF", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 20)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepDeltaFStartDeltaF { get; set; }

        [EnabledIf("IsSweepDeltaFEnabled", true, HideIfDisabled = true)]
        [Display("Stop DeltaF", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 21)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepDeltaFStopDeltaF { get; set; }

        [EnabledIf("IsSweepDeltaFEnabled", true, HideIfDisabled = true)]
        [Display("Fixed fc", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 22)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double SweepDeltaFFixedFc { get; set; }

        [Browsable(false)]
        public bool IsPowerSweepEnabled { get; set; }
        [EnabledIf("IsPowerSweepEnabled", true, HideIfDisabled = true)]
        [Display("CW f1", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 30)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double PowerSweepCWF1 { get; set; }

        [EnabledIf("IsPowerSweepEnabled", true, HideIfDisabled = true)]
        [Display("CW f2", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 31)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double PowerSweepCWF2 { get; set; }

        [EnabledIf("IsPowerSweepEnabled", true, HideIfDisabled = true)]
        [Display("CW fc", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 32)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double PowerSweepCWFc { get; set; }

        [EnabledIf("IsPowerSweepEnabled", true, HideIfDisabled = true)]
        [Display("CW DeltaF", Groups: new[] { "Tone Frequency", "Sweep Settings" }, Order: 33)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double PowerSweepCWDeltaF { get; set; }

        private int _SweepFcNumberOfPoints;
        [Browsable(false)]
        public bool IsSweepPointsEnabled { get; set; }
        [EnabledIf("IsSweepPointsEnabled", true, HideIfDisabled = true)]
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
                UpdateMixerSweepPoints();
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

        [Browsable(false)]
        public bool IsSegmentEnabled { get; set; } = false;

        #endregion

        public ToneFrequencyBaseStep()
        {
            UpdateDefaultValues();
            UpdateTypeAndNotation();
        }

        protected virtual void UpdateTypeAndNotation()
        {
        }

        private void UpdateDefaultValues()
        {
            var DefaultValues = PNAX.GetToneFrequencyDefaultValues();
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
            IsStartStopCenterSpan = SweepSSCSTypeEnum.StartStop;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.
            SetSweepType();
            if (IsSweepFCEnabled)
            {
                SetSweepFC();
            }
            else if (IsSweepDeltaFEnabled)
            {
                SetSweepDeltaF();
            }
            else if (IsPowerSweepEnabled)   // Power Sweep, CW, LoPowerSweep
            {
                SetPowerSweep();
            }
            else if (IsSegmentEnabled)
            {
                SetSegmentValues();
            }

            SetSweepTone();

            UpgradeVerdict(Verdict.Pass);
        }

        protected virtual void SetSweepTone()
        {
            PNAX.SetIMDSweepSettingsMainToneIFBW(Channel, SweepFcMixedToneIFBW);
            PNAX.SetIMDSweepSettingsIMToneIFBW(Channel, SweepFcIMToneIFBW);
            PNAX.SetLFAutoBW(Channel, SweepFcReduceIFBW);
        }

        protected virtual void SetPowerSweep()
        {
            PNAX.SetIMDSweepSettingsFixedf1(Channel, PowerSweepCWF1);
            PNAX.SetIMDSweepSettingsFixedf2(Channel, PowerSweepCWF2);
            PNAX.SetIMDSweepSettingsCenterfcFixed(Channel, PowerSweepCWFc);
            PNAX.SetIMDSweepSettingsFixedDeltaF(Channel, PowerSweepCWDeltaF);
            PNAX.SetPoints(Channel, SweepFcNumberOfPoints);
        }

        protected virtual void SetSweepDeltaF()
        {
            PNAX.SetIMDSweepSettingsStartDeltaF(Channel, SweepDeltaFStartDeltaF);
            PNAX.SetIMDSweepSettingsStopDeltaF(Channel, SweepDeltaFStopDeltaF);
            PNAX.SetIMDSweepSettingsCenterfcFixed(Channel, SweepDeltaFFixedFc);
            PNAX.SetPoints(Channel, SweepFcNumberOfPoints);
        }

        protected virtual void SetSweepFC()
        {
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
            PNAX.SetPoints(Channel, SweepFcNumberOfPoints);
        }

        protected virtual void SetSweepType()
        {
            throw new NotImplementedException();
        }

        protected virtual void SetSegmentValues()
        {
        }

        protected virtual void UpdateMixerSweepPoints()
        {
        }

    }
}
