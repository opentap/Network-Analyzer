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
    public enum FOMModeEnum
    {
        [Display("Coupled")]
        Coupled,
        [Display("Un-Coupled")]
        UnCoupled
    }

    [Display("Frequency Offset", Groups: new[] { "PNA-X", "General" }, Description: "Insert a description here")]
    public class FrequencyOffset : GeneralBaseStep
    {
        #region Settings
        [Display("Enable Frequency Offset", Order: 10)]
        public bool EnableFOM { get; set; }

        #region Primary
        private StandardSweepTypeEnum _PrimarySweepType;
        [Display("Data Acquisition Mode", Groups: new[] { "Primary" }, Order: 20)]
        public StandardSweepTypeEnum PrimarySweepType 
        {
            get
            {
                return _PrimarySweepType;
            }
            set
            {
                _PrimarySweepType = value;
                if (SourceMode == FOMModeEnum.Coupled) SourceSweepType = _PrimarySweepType;
                if (ReceiversMode == FOMModeEnum.Coupled) ReceiversSweepType = _PrimarySweepType;
                if (Source2Mode == FOMModeEnum.Coupled) Source2SweepType = _PrimarySweepType;
                if (Source3Mode == FOMModeEnum.Coupled) Source3SweepType = _PrimarySweepType;
            }
        }

        [EnabledIf("PrimarySweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [Display("Start", Groups: new[] { "Primary" , "Settings"}, Order: 21)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double PrimaryStart { get; set; }

        [EnabledIf("PrimarySweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [Display("Stop", Groups: new[] { "Primary" , "Settings"}, Order: 22)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double PrimaryStop { get; set; }

        [EnabledIf("PrimarySweepType", StandardSweepTypeEnum.PowerSweep, StandardSweepTypeEnum.CWTime, StandardSweepTypeEnum.PhaseSweep, HideIfDisabled = true)]
        [Display("CW Freq", Groups: new[] { "Primary", "Settings" }, Order: 23)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double PrimaryCW { get; set; }

        [EnabledIf("PrimarySweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, StandardSweepTypeEnum.CWTime, StandardSweepTypeEnum.PhaseSweep, StandardSweepTypeEnum.PowerSweep, HideIfDisabled = true)]
        [Display("Points", Groups: new[] { "Primary", "Settings" }, Order: 24)]
        public int PrimaryPoints { get; set; }

        [EnabledIf("PrimarySweepType", StandardSweepTypeEnum.SegmentSweep, HideIfDisabled = true)]
        [Display("Segment Table", Groups: new[] { "Primary", "Settings" }, Order: 25)]
        public List<SegmentDefinition> PrimarySegmentDefinition { get; set; }
        #endregion

        #region Source
        private FOMModeEnum _SourceMode;
        [Display("Source Mode", Groups: new[] { "Source" }, Order: 20)]
        public FOMModeEnum SourceMode 
        {
            get 
            { 
                return _SourceMode;
            }
            set
            {
                _SourceMode = value;
                if (value == FOMModeEnum.Coupled)
                {
                    // Set all values equal to primary
                    SourceSweepType = PrimarySweepType;
                    //SourceOffset = 0;
                    //SourceMultiplier = 1;
                    //SourceDivisor = 1;
                }
                else
                {
                    SourceStart = (PrimaryStart * SourceMultiplier / SourceDivisor) + SourceOffset;
                    SourceStop = (PrimaryStop * SourceMultiplier / SourceDivisor) + SourceOffset;
                    SourceCW = (PrimaryCW * SourceMultiplier / SourceDivisor) + SourceOffset;
                }
            }
        }

        [EnabledIf("SourceMode", FOMModeEnum.UnCoupled, HideIfDisabled = false)]
        [Display("Data Acquisition Mode", Groups: new[] { "Source" }, Order: 21)]
        public StandardSweepTypeEnum SourceSweepType { get; set; }

        [EnabledIf("SourceMode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("SourceSweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [Display("Start", Groups: new[] { "Source", "Settings" }, Order: 22)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double SourceStart { get; set; }

        [EnabledIf("SourceMode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("SourceSweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [Display("Stop", Groups: new[] { "Source", "Settings" }, Order: 23)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SourceStop { get; set; }

        [EnabledIf("SourceMode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("SourceSweepType", StandardSweepTypeEnum.PowerSweep, StandardSweepTypeEnum.CWTime, StandardSweepTypeEnum.PhaseSweep, HideIfDisabled = true)]
        [Display("CW Freq", Groups: new[] { "Source", "Settings" }, Order: 24)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double SourceCW { get; set; }

        [EnabledIf("SourceMode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("SourceSweepType", StandardSweepTypeEnum.SegmentSweep, HideIfDisabled = true)]
        [Display("Segment Table", Groups: new[] { "Source", "Settings" }, Order: 26)]
        public List<SegmentDefinition> SourceSegmentDefinition { get; set; }

        [EnabledIf("SourceMode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Offset", Groups: new[] { "Source", "Settings" }, Order: 27)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double SourceOffset { get; set; }

        [EnabledIf("SourceMode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Multiplier", Groups: new[] { "Source", "Settings" }, Order: 28)]
        [Unit("", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SourceMultiplier { get; set; }

        [EnabledIf("SourceMode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Divisor", Groups: new[] { "Source", "Settings" }, Order: 29)]
        [Unit("", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SourceDivisor { get; set; }
        #endregion

        #region Receivers
        private FOMModeEnum _ReceiversMode;
        [Display("Receivers Mode", Groups: new[] { "Receivers" }, Order: 20)]
        public FOMModeEnum ReceiversMode
        {
            get
            {
                return _ReceiversMode;
            }
            set
            {
                _ReceiversMode = value;
                if (value == FOMModeEnum.Coupled)
                {
                    // Set all values equal to primary
                    ReceiversSweepType = PrimarySweepType;
                    //ReceiversOffset = 0;
                    //ReceiversMultiplier = 1;
                    //ReceiversDivisor = 1;
                }
                else
                {
                    ReceiversStart = (PrimaryStart * ReceiversMultiplier / ReceiversDivisor) + ReceiversOffset;
                    ReceiversStop = (PrimaryStop * ReceiversMultiplier / ReceiversDivisor) + ReceiversOffset;
                    ReceiversCW = (PrimaryCW * ReceiversMultiplier / ReceiversDivisor) + ReceiversOffset;
                }
            }
        }

        [EnabledIf("ReceiversMode", FOMModeEnum.UnCoupled, HideIfDisabled = false)]
        [Display("Data Acquisition Mode", Groups: new[] { "Receivers" }, Order: 21)]
        public StandardSweepTypeEnum ReceiversSweepType { get; set; }

        [EnabledIf("ReceiversMode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("ReceiversSweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [Display("Start", Groups: new[] { "Receivers", "Settings" }, Order: 22)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double ReceiversStart { get; set; }

        [EnabledIf("ReceiversMode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("ReceiversSweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [Display("Stop", Groups: new[] { "Receivers", "Settings" }, Order: 23)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double ReceiversStop { get; set; }

        [EnabledIf("ReceiversMode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("ReceiversSweepType", StandardSweepTypeEnum.PowerSweep, StandardSweepTypeEnum.CWTime, StandardSweepTypeEnum.PhaseSweep, HideIfDisabled = true)]
        [Display("CW Freq", Groups: new[] { "Receivers", "Settings" }, Order: 24)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double ReceiversCW { get; set; }

        [EnabledIf("ReceiversMode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("ReceiversSweepType", StandardSweepTypeEnum.SegmentSweep, HideIfDisabled = true)]
        [Display("Segment Table", Groups: new[] { "Receivers", "Settings" }, Order: 26)]
        public List<SegmentDefinition> ReceiversSegmentDefinition { get; set; }

        [EnabledIf("ReceiversMode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Offset", Groups: new[] { "Receivers", "Settings" }, Order: 27)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double ReceiversOffset { get; set; }

        [EnabledIf("ReceiversMode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Multiplier", Groups: new[] { "Receivers", "Settings" }, Order: 28)]
        [Unit("", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double ReceiversMultiplier { get; set; }

        [EnabledIf("ReceiversMode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Divisor", Groups: new[] { "Receivers", "Settings" }, Order: 29)]
        [Unit("", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double ReceiversDivisor { get; set; }
        #endregion

        #region Source2
        private FOMModeEnum _Source2Mode;
        [Display("Source2 Mode", Groups: new[] { "Source2" }, Order: 20)]
        public FOMModeEnum Source2Mode
        {
            get
            {
                return _Source2Mode;
            }
            set
            {
                _Source2Mode = value;
                if (value == FOMModeEnum.Coupled)
                {
                    // Set all values equal to primary
                    Source2SweepType = PrimarySweepType;
                    //Source2Offset = 0;
                    //Source2Multiplier = 1;
                    //Source2Divisor = 1;
                }
                else
                {
                    Source2Start = (PrimaryStart * Source2Multiplier / Source2Divisor) + Source2Offset;
                    Source2Stop = (PrimaryStop * Source2Multiplier / Source2Divisor) + Source2Offset;
                    Source2CW = (PrimaryCW * Source2Multiplier / Source2Divisor) + Source2Offset;
                }
            }
        }

        [EnabledIf("Source2Mode", FOMModeEnum.UnCoupled, HideIfDisabled = false)]
        [Display("Data Acquisition Mode", Groups: new[] { "Source2" }, Order: 21)]
        public StandardSweepTypeEnum Source2SweepType { get; set; }

        [EnabledIf("Source2Mode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("Source2SweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [Display("Start", Groups: new[] { "Source2", "Settings" }, Order: 22)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double Source2Start { get; set; }

        [EnabledIf("Source2Mode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("Source2SweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [Display("Stop", Groups: new[] { "Source2", "Settings" }, Order: 23)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double Source2Stop { get; set; }

        [EnabledIf("Source2Mode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("Source2SweepType", StandardSweepTypeEnum.PowerSweep, StandardSweepTypeEnum.CWTime, StandardSweepTypeEnum.PhaseSweep, HideIfDisabled = true)]
        [Display("CW Freq", Groups: new[] { "Source2", "Settings" }, Order: 24)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double Source2CW { get; set; }

        [EnabledIf("Source2Mode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("Source2SweepType", StandardSweepTypeEnum.SegmentSweep, HideIfDisabled = true)]
        [Display("Segment Table", Groups: new[] { "Source2", "Settings" }, Order: 26)]
        public List<SegmentDefinition> Source2SegmentDefinition { get; set; }

        [EnabledIf("Source2Mode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Offset", Groups: new[] { "Source2", "Settings" }, Order: 27)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double Source2Offset { get; set; }

        [EnabledIf("Source2Mode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Multiplier", Groups: new[] { "Source2", "Settings" }, Order: 28)]
        [Unit("", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double Source2Multiplier { get; set; }

        [EnabledIf("Source2Mode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Divisor", Groups: new[] { "Source2", "Settings" }, Order: 29)]
        [Unit("", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double Source2Divisor { get; set; }
        #endregion

        #region Source3
        private FOMModeEnum _Source3Mode;
        [Display("Source3 Mode", Groups: new[] { "Source3" }, Order: 20)]
        public FOMModeEnum Source3Mode
        {
            get
            {
                return _Source3Mode;
            }
            set
            {
                _Source3Mode = value;
                if (value == FOMModeEnum.Coupled)
                {
                    // Set all values equal to primary
                    Source3SweepType = PrimarySweepType;
                    //Source3Offset = 0;
                    //Source3Multiplier = 1;
                    //Source3Divisor = 1;
                }
                else
                {
                    Source3Start = (PrimaryStart * Source3Multiplier / Source3Divisor) + Source3Offset;
                    Source3Stop = (PrimaryStop * Source3Multiplier / Source3Divisor) + Source3Offset;
                    Source3CW = (PrimaryCW * Source3Multiplier / Source3Divisor) + Source3Offset;
                }
            }
        }

        [EnabledIf("Source3Mode", FOMModeEnum.UnCoupled, HideIfDisabled = false)]
        [Display("Data Acquisition Mode", Groups: new[] { "Source3" }, Order: 21)]
        public StandardSweepTypeEnum Source3SweepType { get; set; }

        [EnabledIf("Source3Mode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("Source3SweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [Display("Start", Groups: new[] { "Source3", "Settings" }, Order: 22)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double Source3Start { get; set; }

        [EnabledIf("Source3Mode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("Source3SweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [Display("Stop", Groups: new[] { "Source3", "Settings" }, Order: 23)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double Source3Stop { get; set; }

        [EnabledIf("Source3Mode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("Source3SweepType", StandardSweepTypeEnum.PowerSweep, StandardSweepTypeEnum.CWTime, StandardSweepTypeEnum.PhaseSweep, HideIfDisabled = true)]
        [Display("CW Freq", Groups: new[] { "Source3", "Settings" }, Order: 24)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double Source3CW { get; set; }

        [EnabledIf("Source3Mode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("Source3SweepType", StandardSweepTypeEnum.SegmentSweep, HideIfDisabled = true)]
        [Display("Segment Table", Groups: new[] { "Source3", "Settings" }, Order: 26)]
        public List<SegmentDefinition> Source3SegmentDefinition { get; set; }

        [EnabledIf("Source3Mode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Offset", Groups: new[] { "Source3", "Settings" }, Order: 27)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double Source3Offset { get; set; }

        [EnabledIf("Source3Mode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Multiplier", Groups: new[] { "Source3", "Settings" }, Order: 28)]
        [Unit("", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double Source3Multiplier { get; set; }

        [EnabledIf("Source3Mode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Divisor", Groups: new[] { "Source3", "Settings" }, Order: 29)]
        [Unit("", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double Source3Divisor { get; set; }
        #endregion





        #endregion

        public FrequencyOffset()
        {
            PrimarySweepType = StandardSweepTypeEnum.LinearFrequency;
            PrimaryStart = 10e6;
            PrimaryStop = 50e9;
            PrimaryPoints = 201;
            PrimaryCW = 1e9; 
            PrimarySegmentDefinition = new List<SegmentDefinition>();
            PrimarySegmentDefinition.Add(new SegmentDefinition { state = true, NumberOfPoints = 21, StartFrequency = 10.5e6, StopFrequency = 1e9 });

            SourceMode = FOMModeEnum.Coupled;
            SourceSweepType = StandardSweepTypeEnum.LinearFrequency;
            SourceStart = 10e6;
            SourceStop = 50e9;
            SourceCW = 1e9;
            SourceSegmentDefinition = new List<SegmentDefinition>();
            SourceSegmentDefinition.Add(new SegmentDefinition { state = true, NumberOfPoints = 21, StartFrequency = 10.5e6, StopFrequency = 1e9 });
            SourceOffset = 0;
            SourceMultiplier = 1;
            SourceDivisor = 1;

            ReceiversMode = FOMModeEnum.Coupled;
            ReceiversSweepType = StandardSweepTypeEnum.LinearFrequency;
            ReceiversStart = 10e6;
            ReceiversStop = 50e9;
            ReceiversCW = 1e9;
            ReceiversSegmentDefinition = new List<SegmentDefinition>();
            ReceiversSegmentDefinition.Add(new SegmentDefinition { state = true, NumberOfPoints = 21, StartFrequency = 10.5e6, StopFrequency = 1e9 });
            ReceiversOffset = 0;
            ReceiversMultiplier = 1;
            ReceiversDivisor = 1;

            Source2Mode = FOMModeEnum.Coupled;
            Source2SweepType = StandardSweepTypeEnum.LinearFrequency;
            Source2Start = 10e6;
            Source2Stop = 50e9;
            Source2CW = 1e9;
            Source2SegmentDefinition = new List<SegmentDefinition>();
            Source2SegmentDefinition.Add(new SegmentDefinition { state = true, NumberOfPoints = 21, StartFrequency = 10.5e6, StopFrequency = 1e9 });
            Source2Offset = 0;
            Source2Multiplier = 1;
            Source2Divisor = 1;

            Source3Mode = FOMModeEnum.UnCoupled;
            Source3SweepType = StandardSweepTypeEnum.LinearFrequency;
            Source3Start = 10e6;
            Source3Stop = 13.51e9;
            Source3CW = 1e9;
            Source3SegmentDefinition = new List<SegmentDefinition>();
            Source3SegmentDefinition.Add(new SegmentDefinition { state = true, NumberOfPoints = 21, StartFrequency = 10.5e6, StopFrequency = 1e9 });
            Source3Offset = 0;
            Source3Multiplier = 1;
            Source3Divisor = 1;

        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.



            UpgradeVerdict(Verdict.Pass);
        }
    }
}
