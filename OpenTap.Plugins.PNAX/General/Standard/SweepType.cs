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
        PhaseSweep
    }

    [AllowAsChildIn(typeof(StandardChannel))]
    [Display("Sweep Type", Groups: new[] { "PNA-X", "General",  "Standard" }, Description: "Insert a description here")]
    public class SweepType : GeneralChannelBaseStep
    {
        #region Settings

        [Display("Data Acquisition Mode", Order: 10)]
        public StandardSweepTypeEnum StandardSweepType { get; set; }


        [EnabledIf("StandardSweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [Display("Start", Group: "Sweep Properties", Order: 20)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double SweepPropertiesStart { get; set; }

        [EnabledIf("StandardSweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
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

        [EnabledIf("StandardSweepType", StandardSweepTypeEnum.PhaseSweep, StandardSweepTypeEnum.CWTime, StandardSweepTypeEnum.PowerSweep, HideIfDisabled = true)]
        [EnabledIf("EnableCWFreq", true, HideIfDisabled = true)]
        [Display("CW Freq", Group: "Sweep Properties", Order: 22)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepPropertiesCWFreq { get; set; }

        [EnabledIf("StandardSweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, StandardSweepTypeEnum.CWTime, HideIfDisabled = true)]
        [Display("Power", Group: "Sweep Properties", Order: 22)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double SweepPropertiesPower { get; set; }

        [EnabledIf("StandardSweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, StandardSweepTypeEnum.CWTime, StandardSweepTypeEnum.PhaseSweep, StandardSweepTypeEnum.PowerSweep, HideIfDisabled = true)]
        [Display("Points", Group: "Sweep Properties", Order: 23)]
        public int SweepPropertiesPoints { get; set; }

        [EnabledIf("StandardSweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, StandardSweepTypeEnum.CWTime, StandardSweepTypeEnum.PhaseSweep, StandardSweepTypeEnum.PowerSweep, HideIfDisabled = true)]
        [Display("IF Bandwidth", Group: "Sweep Properties", Order: 24)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double SweepPropertiesIFBandwidth { get; set; }


        [EnabledIf("StandardSweepType", StandardSweepTypeEnum.SegmentSweep, HideIfDisabled = true)]
        [Display("Segment Table", Group: "Sweep Properties", Order: 25)]
        public string SegmentTable { get; set; }

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
        }

        public override void Run()
        {
            // ToDo: Add test case code.
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
