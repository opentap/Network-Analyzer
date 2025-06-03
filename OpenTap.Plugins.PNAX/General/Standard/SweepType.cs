// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using OpenTap;

namespace OpenTap.Plugins.PNAX
{
    public class SegmentDefinition
    {
        [Display("State", Order: 1)]
        public bool state { get; set; }

        [Display("Number of Points", Order: 2)]
        public int NumberOfPoints { get; set; }

        [Display("Start Frequency", Order: 3)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double StartFrequency { get; set; }

        [Display("Stop Frequency", Order: 4)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double StopFrequency { get; set; }
    }

    public enum SegmentDefinitionTypeEnum
    {
        File,
        List,
    }

    public enum StandardSweepTypeEnum
    {
        [Scpi("LINear")]
        [Display("Linear Frequency")]
        LinearFrequency,

        [Scpi("LOGarithmic")]
        [Display("Log Frequency")]
        LogFrequency,

        [Scpi("POWer")]
        [Display("Power Sweep")]
        PowerSweep,

        [Scpi("CW")]
        [Display("CW Time")]
        CWTime,

        [Scpi("SEGMent")]
        [Display("Segment Sweep")]
        SegmentSweep,

        [Scpi("PHASe")]
        [Display("Phase Sweep")]
        PhaseSweep,
    }

    //[AllowAsChildIn(typeof(StandardChannel))]
    [Display(
        "Sweep Type",
        Groups: new[] { "Network Analyzer", "General", "Standard" },
        Description: "Insert a description here"
    )]
    public class SweepType : PNABaseStep
    {
        #region Settings

        private StandardSweepTypeEnum _StandardSweepType;

        [Display("Data Acquisition Mode", Order: 10)]
        public StandardSweepTypeEnum StandardSweepType
        {
            get { return _StandardSweepType; }
            set
            {
                _StandardSweepType = value;
                EnableSegmentSweepSettings = value == StandardSweepTypeEnum.SegmentSweep;
            }
        }

        [EnabledIf(
            "StandardSweepType",
            StandardSweepTypeEnum.LinearFrequency,
            StandardSweepTypeEnum.LogFrequency,
            HideIfDisabled = true
        )]
        [Display("Start", Group: "Sweep Properties", Order: 20)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double SweepPropertiesStart { get; set; }

        [EnabledIf(
            "StandardSweepType",
            StandardSweepTypeEnum.LinearFrequency,
            StandardSweepTypeEnum.LogFrequency,
            HideIfDisabled = true
        )]
        [Display("Stop", Group: "Sweep Properties", Order: 21)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepPropertiesStop { get; set; }

        [EnabledIf("StandardSweepType", StandardSweepTypeEnum.PowerSweep, HideIfDisabled = true)]
        [Display("Start Power", Group: "Sweep Properties", Order: 20)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double SweepPropertiesStartPower { get; set; }

        [EnabledIf("StandardSweepType", StandardSweepTypeEnum.PowerSweep, HideIfDisabled = true)]
        [Display("Stop Power", Group: "Sweep Properties", Order: 21)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double SweepPropertiesStopPower { get; set; }

        [EnabledIf("StandardSweepType", StandardSweepTypeEnum.PhaseSweep, HideIfDisabled = true)]
        [Display("Start Phase", Group: "Sweep Properties", Order: 20)]
        [Unit("°", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double SweepPropertiesStartPhase { get; set; }

        [EnabledIf("StandardSweepType", StandardSweepTypeEnum.PhaseSweep, HideIfDisabled = true)]
        [Display("Stop Phase", Group: "Sweep Properties", Order: 21)]
        [Unit("°", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double SweepPropertiesStopPhase { get; set; }

        [EnabledIf(
            "StandardSweepType",
            StandardSweepTypeEnum.PhaseSweep,
            StandardSweepTypeEnum.CWTime,
            StandardSweepTypeEnum.PowerSweep,
            HideIfDisabled = true
        )]
        [Display("CW Freq", Group: "Sweep Properties", Order: 22)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepPropertiesCWFreq { get; set; }

        [EnabledIf(
            "StandardSweepType",
            StandardSweepTypeEnum.LinearFrequency,
            StandardSweepTypeEnum.LogFrequency,
            StandardSweepTypeEnum.CWTime,
            HideIfDisabled = true
        )]
        [Display("Power", Group: "Sweep Properties", Order: 22)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double SweepPropertiesPower { get; set; }

        [EnabledIf(
            "StandardSweepType",
            StandardSweepTypeEnum.LinearFrequency,
            StandardSweepTypeEnum.LogFrequency,
            StandardSweepTypeEnum.CWTime,
            StandardSweepTypeEnum.PhaseSweep,
            StandardSweepTypeEnum.PowerSweep,
            HideIfDisabled = true
        )]
        [Display("Points", Group: "Sweep Properties", Order: 23)]
        public int SweepPropertiesPoints { get; set; }

        [EnabledIf(
            "StandardSweepType",
            StandardSweepTypeEnum.LinearFrequency,
            StandardSweepTypeEnum.LogFrequency,
            StandardSweepTypeEnum.CWTime,
            StandardSweepTypeEnum.PhaseSweep,
            StandardSweepTypeEnum.PowerSweep,
            HideIfDisabled = true
        )]
        [Display("IF Bandwidth", Group: "Sweep Properties", Order: 24)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double SweepPropertiesIFBandwidth { get; set; }

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

        public SweepType()
        {
            UpdateDefaultValues();
        }

        private void UpdateDefaultValues()
        {
            var defaultValues = PNAX.GetStandardChannelDefaultValues();
            StandardSweepType = defaultValues.SweepType;
            SweepPropertiesStart = defaultValues.Start;
            SweepPropertiesStop = defaultValues.Stop;
            SweepPropertiesStartPower = defaultValues.StartPower;
            SweepPropertiesStopPower = defaultValues.StopPower;
            SweepPropertiesStartPhase = defaultValues.StartPhase;
            SweepPropertiesStopPhase = defaultValues.StopPhase;
            SweepPropertiesCWFreq = defaultValues.CWFrequency;
            SweepPropertiesPower = defaultValues.Power;
            SweepPropertiesPoints = defaultValues.Points;
            SweepPropertiesIFBandwidth = defaultValues.IFBandWidth;

            SegmentDefinitionType = SegmentDefinitionTypeEnum.List;
            SegmentDefinitions = new List<SegmentDefinition>();
            SegmentDefinitions.Add(
                new SegmentDefinition
                {
                    state = true,
                    NumberOfPoints = 21,
                    StartFrequency = 10.5e6,
                    StopFrequency = 1e9,
                }
            );
            ShowTable = false;
        }

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            List<(string, object)> retVal = new List<(string, object)>
            {
                ("Data Acquisition Mode", StandardSweepType),
            };

            switch (StandardSweepType)
            {
                case StandardSweepTypeEnum.LinearFrequency:
                case StandardSweepTypeEnum.LogFrequency:
                    retVal.Add(("Start", SweepPropertiesStart));
                    retVal.Add(("Stop", SweepPropertiesStop));
                    retVal.Add(("Power", SweepPropertiesPower));
                    retVal.Add(("Points", SweepPropertiesPoints));
                    retVal.Add(("IF Bandwidth", SweepPropertiesIFBandwidth));
                    break;
                case StandardSweepTypeEnum.PowerSweep:
                    retVal.Add(("Start Power", SweepPropertiesStartPower));
                    retVal.Add(("Stop Power", SweepPropertiesStopPower));
                    retVal.Add(("CW Freq", SweepPropertiesCWFreq));
                    retVal.Add(("Points", SweepPropertiesPoints));
                    retVal.Add(("IF Bandwidth", SweepPropertiesIFBandwidth));
                    break;
                case StandardSweepTypeEnum.CWTime:
                    retVal.Add(("Power", SweepPropertiesPower));
                    retVal.Add(("CW Freq", SweepPropertiesCWFreq));
                    retVal.Add(("Points", SweepPropertiesPoints));
                    retVal.Add(("IF Bandwidth", SweepPropertiesIFBandwidth));
                    break;
                case StandardSweepTypeEnum.SegmentSweep:
                case StandardSweepTypeEnum.PhaseSweep:
                    retVal.Add(("Start Phase", SweepPropertiesStartPhase));
                    retVal.Add(("Stop Phase", SweepPropertiesStopPhase));
                    retVal.Add(("CW Freq", SweepPropertiesCWFreq));
                    retVal.Add(("Points", SweepPropertiesPoints));
                    retVal.Add(("IF Bandwidth", SweepPropertiesIFBandwidth));
                    break;
            }

            return retVal;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetStandardSweepType(Channel, StandardSweepType);
            switch (StandardSweepType)
            {
                case StandardSweepTypeEnum.LinearFrequency:
                case StandardSweepTypeEnum.LogFrequency:
                    PNAX.SetStart(Channel, SweepPropertiesStart);
                    PNAX.SetStop(Channel, SweepPropertiesStop);
                    PNAX.SetPower(Channel, SweepPropertiesPower);
                    PNAX.SetPoints(Channel, SweepPropertiesPoints);
                    PNAX.SetIFBandwidth(Channel, SweepPropertiesIFBandwidth);
                    break;
                case StandardSweepTypeEnum.PowerSweep:
                    PNAX.SetStartPower(Channel, SweepPropertiesStartPower);
                    PNAX.SetStopPower(Channel, SweepPropertiesStopPower);
                    PNAX.SetCWFreq(Channel, SweepPropertiesCWFreq);
                    PNAX.SetPoints(Channel, SweepPropertiesPoints);
                    PNAX.SetIFBandwidth(Channel, SweepPropertiesIFBandwidth);
                    break;
                case StandardSweepTypeEnum.CWTime:
                    PNAX.SetPower(Channel, SweepPropertiesPower);
                    PNAX.SetCWFreq(Channel, SweepPropertiesCWFreq);
                    PNAX.SetPoints(Channel, SweepPropertiesPoints);
                    PNAX.SetIFBandwidth(Channel, SweepPropertiesIFBandwidth);
                    break;
                case StandardSweepTypeEnum.SegmentSweep:
                    PNAX.SetSegmentValues(
                        SegmentDefinitionType,
                        Channel,
                        SegmentDefinitions,
                        ShowTable
                    );
                    break;
                case StandardSweepTypeEnum.PhaseSweep:
                    PNAX.SetPhaseStart(Channel, SweepPropertiesStartPhase);
                    PNAX.SetPhaseStop(Channel, SweepPropertiesStopPhase);
                    PNAX.SetCWFreq(Channel, SweepPropertiesCWFreq);
                    PNAX.SetPoints(Channel, SweepPropertiesPoints);
                    PNAX.SetIFBandwidth(Channel, SweepPropertiesIFBandwidth);
                    break;
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
