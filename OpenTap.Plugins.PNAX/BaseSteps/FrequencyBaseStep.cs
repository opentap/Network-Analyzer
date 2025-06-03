using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTap.Plugins.PNAX
{

    public enum SweepTypeEnum
    {
        [Scpi("LIN")]
        [Display("Linear Sweep")]
        LinearSweep,
        [Scpi("CW")]
        [Display("CW Frequency")]
        CWFrequency
    }

    public enum DataAcquisitionModeEnum
    {
        [Scpi("SMAR")]
        [Display("SMART Sweep")]
        SMARTSweep,
        [Scpi("PFREQ")]
        [Display("Sweep Power Per Frequency 2D")]
        SweepPowerPerFrequency2D,
        [Scpi("FPOW")]
        [Display("Sweep Frequency Per Power 2D")]
        SweepFrequencyPerPower2D
    }

    [Browsable(false)]
    public class FrequencyBaseStep : PNABaseStep
    {
        #region Settings
        [Display("Number Of Points", Group: "Sweep Settings", Order: 10)]
        public virtual int SweepSettingsNumberOfPoints { get; set; }

        [Display("IF Bandwidth", Group: "Sweep Settings", Order: 11)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double SweepSettingsIFBandwidth { get; set; }

        [Browsable(false)]
        public bool LinearSweepEnabled { get; set; }
        [EnabledIf("LinearSweepEnabled", true, HideIfDisabled = true)]
        [Display("Type", Group: "Sweep Settings", Order: 11.9)]
        public SweepSSCSTypeEnum IsStartStopCenterSpan { get; set; }

        [EnabledIf("LinearSweepEnabled", true, HideIfDisabled = true)]
        [EnabledIf("IsStartStopCenterSpan", SweepSSCSTypeEnum.StartStop, HideIfDisabled = true)]
        [Display("Start", Group: "Sweep Settings", Order: 12)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double SweepSettingsStart { get; set; }

        [EnabledIf("LinearSweepEnabled", true, HideIfDisabled = true)]
        [EnabledIf("IsStartStopCenterSpan", SweepSSCSTypeEnum.StartStop, HideIfDisabled = true)]
        [Display("Stop", Group: "Sweep Settings", Order: 13)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsStop { get; set; }

        [EnabledIf("LinearSweepEnabled", true, HideIfDisabled = true)]
        [EnabledIf("IsStartStopCenterSpan", SweepSSCSTypeEnum.CenterSpan, HideIfDisabled = true)]
        [Display("Center", Group: "Sweep Settings", Order: 14)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsCenter { get; set; }

        [EnabledIf("LinearSweepEnabled", true, HideIfDisabled = true)]
        [EnabledIf("IsStartStopCenterSpan", SweepSSCSTypeEnum.CenterSpan, HideIfDisabled = true)]
        [Display("Span", Group: "Sweep Settings", Order: 15)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsSpan { get; set; }

        [Browsable(false)]
        public bool CWFrequencyEnabled { get; set; }
        [EnabledIf("CWFrequencyEnabled", true, HideIfDisabled = true)]
        [Display("Fixed", Group: "Sweep Settings", Order: 16)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsFixed { get; set; }

        [Browsable(false)]
        public bool EnableSegmentSweepSettings { get; set; } = false;

        [EnabledIf("EnableSegmentSweepSettings", true, HideIfDisabled = true)]
        [Display("Segment Definition Type", Group: "Sweep Properties", Order: 30)]
        public SegmentDefinitionTypeEnum SegmentDefinitionType { get; set; }

        [EnabledIf("EnableSegmentSweepSettings", true, HideIfDisabled = true)]
        [EnabledIf("SegmentDefinitionType", SegmentDefinitionTypeEnum.File, HideIfDisabled = false)]
        [Display("Segment Table File Name", Group: "Sweep Properties", Order: 31)]
        [FilePath]
        public string SegmentTable { get; set; }

        [EnabledIf("EnableSegmentSweepSettings", true, HideIfDisabled = true)]
        [EnabledIf("SegmentDefinitionType", SegmentDefinitionTypeEnum.List, HideIfDisabled = false)]
        [Display("Segment Table", Group: "Sweep Properties", Order: 32)]
        public List<SegmentDefinition> SegmentDefinitions { get; set; }

        [EnabledIf("EnableSegmentSweepSettings", true, HideIfDisabled = true)]
        [EnabledIf("SegmentDefinitionType", SegmentDefinitionTypeEnum.List, HideIfDisabled = false)]
        [Display("Show Table", Group: "Sweep Properties", Order: 33)]
        public bool ShowTable { get; set; }

        [Browsable(false)]
        public int Window { get; set; } = 1;
        #endregion

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.
            SetSweepType();
            SetMode();
            SetSweepPoints();
            UpgradeVerdict(Verdict.Pass);
        }

        protected virtual void SetSweepPoints()
        {
            PNAX.SetIFBandwidth(Channel, SweepSettingsIFBandwidth);

            if (LinearSweepEnabled)
            {
                PNAX.SetPoints(Channel, SweepSettingsNumberOfPoints);
                if (IsStartStopCenterSpan == SweepSSCSTypeEnum.StartStop)
                {
                    PNAX.SetStart(Channel, SweepSettingsStart);
                    PNAX.SetStop(Channel, SweepSettingsStop);
                }
                else
                {
                    PNAX.SetCenter(Channel, SweepSettingsCenter);
                    PNAX.SetSpan(Channel, SweepSettingsSpan);
                }
            }
            else if (CWFrequencyEnabled)
            {
                PNAX.SetPoints(Channel, SweepSettingsNumberOfPoints);
                PNAX.SetCWFreq(Channel, SweepSettingsFixed);
            }
            else if (EnableSegmentSweepSettings)
            {
                PNAX.SetSegmentValues(SegmentDefinitionType, Channel, SegmentDefinitions, ShowTable);
            }
            else
            {
                throw new Exception("Undefined sweep type!");
            }
        }

        protected virtual void SetMode()
        {
            throw new NotImplementedException();
        }

        protected virtual void SetSweepType()
        {
            throw new NotImplementedException();
        }

        public FrequencyBaseStep()
        {
            UpdateDefaultValues();
        }

        private void UpdateDefaultValues()
        {
            UpdateSweepSettings();
            UpdateSegmentSweepValues();
        }

        private void UpdateSegmentSweepValues()
        {
            SegmentDefinitionType = SegmentDefinitionTypeEnum.List;
            SegmentDefinitions = new List<SegmentDefinition>
            {
                new SegmentDefinition { state = true, NumberOfPoints = 21, StartFrequency = 10.5e6, StopFrequency = 1e9 }
            };
            ShowTable = false;
            Window = 1;
        }

        protected virtual void UpdateSweepSettings()
        {
            var DefaultValues = PNAX.GetConverterFrequencyDefaultValues();
            SweepSettingsNumberOfPoints = DefaultValues.SweepSettingsNumberOfPoints;
            SweepSettingsIFBandwidth = DefaultValues.SweepSettingsIFBandwidth;
            SweepSettingsStart = DefaultValues.SweepSettingsStart;
            SweepSettingsStop = DefaultValues.SweepSettingsStop;
            SweepSettingsCenter = DefaultValues.SweepSettingsCenter;
            SweepSettingsSpan = DefaultValues.SweepSettingsSpan;
            SweepSettingsFixed = DefaultValues.SweepSettingsFixed;
            IsStartStopCenterSpan = SweepSSCSTypeEnum.StartStop;
        }

    }
}
