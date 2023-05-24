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
    public enum FOMModeEnum
    {
        [Display("Coupled")]
        Coupled,
        [Display("Un-Coupled")]
        UnCoupled
    }

    public enum FOMRangeEnum
    {
        Primary,
        Source,
        Receivers,
        Source2,
        Source3
    }

    [AllowAsChildIn(typeof(StandardChannel))]
    [AllowAsChildIn(typeof(GeneralGainCompressionChannel))]
    [AllowAsChildIn(typeof(GeneralNoiseFigureChannel))]
    [Display("Frequency Offset", Groups: new[] { "PNA-X", "General" }, 
        Description: "Frequency Offset Mode\nCan be added as a child to the following Channels:\n\tStandard\n\tGain Compression\n\tNoise Figure Cold Source")]
    public class FrequencyOffset : GeneralBaseStep
    {
        #region Settings
        [Display("Enable Frequency Offset", Order: 10)]
        public bool EnableFOM { get; set; }

        //[Display("X-Axis Annotation", Order: 11)]
        //public FOMRangeEnum FOMXAxisAnnotation { get; set; }

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

        [EnabledIf("PrimarySweepType", StandardSweepTypeEnum.CWTime, StandardSweepTypeEnum.PhaseSweep, HideIfDisabled = true)]
        [Display("Sweep Time", Groups: new[] { "Primary", "Settings" }, Order: 23.1)]
        [Unit("mSec", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double PrimarySweepTime { get; set; }

        [EnabledIf("PrimarySweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, StandardSweepTypeEnum.CWTime, StandardSweepTypeEnum.PhaseSweep, StandardSweepTypeEnum.PowerSweep, HideIfDisabled = true)]
        [Display("Points", Groups: new[] { "Primary", "Settings" }, Order: 24)]
        public int PrimaryPoints { get; set; }

        [EnabledIf("PrimarySweepType", StandardSweepTypeEnum.SegmentSweep, HideIfDisabled = true)]
        [Display("Segment Table", Groups: new[] { "Primary", "Settings" }, Order: 25)]
        public List<SegmentDefinition> PrimarySegmentDefinition { get; set; }
        #endregion

        #region Source
        private FOMModeEnum _SourceMode;
        [Display("Source Mode", Groups: new[] { "Source" }, Order: 30)]
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
                    SourceSweepType = PrimarySweepType;
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
        [Display("Data Acquisition Mode", Groups: new[] { "Source" }, Order: 31)]
        public StandardSweepTypeEnum SourceSweepType { get; set; }

        [EnabledIf("SourceMode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("SourceSweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [Display("Start", Groups: new[] { "Source", "Settings" }, Order: 32)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double SourceStart { get; set; }

        [EnabledIf("SourceMode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("SourceSweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [Display("Stop", Groups: new[] { "Source", "Settings" }, Order: 33)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SourceStop { get; set; }

        [EnabledIf("SourceMode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("SourceSweepType", StandardSweepTypeEnum.PowerSweep, StandardSweepTypeEnum.CWTime, StandardSweepTypeEnum.PhaseSweep, HideIfDisabled = true)]
        [Display("CW Freq", Groups: new[] { "Source", "Settings" }, Order: 34)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double SourceCW { get; set; }

        [EnabledIf("SourceSweepType", StandardSweepTypeEnum.CWTime, StandardSweepTypeEnum.PhaseSweep, HideIfDisabled = true)]
        [Display("Sweep Time", Groups: new[] { "Source", "Settings" }, Order: 34.1)]
        [Unit("mSec", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double SourceSweepTime { get; set; }

        [EnabledIf("SourceMode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("SourceSweepType", StandardSweepTypeEnum.SegmentSweep, HideIfDisabled = true)]
        [Display("Segment Table", Groups: new[] { "Source", "Settings" }, Order: 36)]
        public List<SegmentDefinition> SourceSegmentDefinition { get; set; }

        [EnabledIf("SourceMode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Offset", Groups: new[] { "Source", "Settings" }, Order: 37)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double SourceOffset { get; set; }

        [EnabledIf("SourceMode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Multiplier", Groups: new[] { "Source", "Settings" }, Order: 38)]
        [Unit("", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SourceMultiplier { get; set; }

        [EnabledIf("SourceMode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Divisor", Groups: new[] { "Source", "Settings" }, Order: 39)]
        [Unit("", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SourceDivisor { get; set; }
        #endregion

        #region Receivers
        private FOMModeEnum _ReceiversMode;
        [Display("Receivers Mode", Groups: new[] { "Receivers" }, Order: 40)]
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
        [Display("Data Acquisition Mode", Groups: new[] { "Receivers" }, Order: 41)]
        public StandardSweepTypeEnum ReceiversSweepType { get; set; }

        [EnabledIf("ReceiversMode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("ReceiversSweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [Display("Start", Groups: new[] { "Receivers", "Settings" }, Order: 42)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double ReceiversStart { get; set; }

        [EnabledIf("ReceiversMode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("ReceiversSweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [Display("Stop", Groups: new[] { "Receivers", "Settings" }, Order: 43)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double ReceiversStop { get; set; }

        [EnabledIf("ReceiversMode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("ReceiversSweepType", StandardSweepTypeEnum.PowerSweep, StandardSweepTypeEnum.CWTime, StandardSweepTypeEnum.PhaseSweep, HideIfDisabled = true)]
        [Display("CW Freq", Groups: new[] { "Receivers", "Settings" }, Order: 44)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double ReceiversCW { get; set; }

        [EnabledIf("ReceiversSweepType", StandardSweepTypeEnum.CWTime, StandardSweepTypeEnum.PhaseSweep, HideIfDisabled = true)]
        [Display("Sweep Time", Groups: new[] { "Receivers", "Settings" }, Order: 44.1)]
        [Unit("mSec", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double ReceiversSweepTime { get; set; }

        [EnabledIf("ReceiversMode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("ReceiversSweepType", StandardSweepTypeEnum.SegmentSweep, HideIfDisabled = true)]
        [Display("Segment Table", Groups: new[] { "Receivers", "Settings" }, Order: 46)]
        public List<SegmentDefinition> ReceiversSegmentDefinition { get; set; }

        [EnabledIf("ReceiversMode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Offset", Groups: new[] { "Receivers", "Settings" }, Order: 47)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double ReceiversOffset { get; set; }

        [EnabledIf("ReceiversMode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Multiplier", Groups: new[] { "Receivers", "Settings" }, Order: 48)]
        [Unit("", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double ReceiversMultiplier { get; set; }

        [EnabledIf("ReceiversMode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Divisor", Groups: new[] { "Receivers", "Settings" }, Order: 49)]
        [Unit("", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double ReceiversDivisor { get; set; }
        #endregion

        #region Source2
        private FOMModeEnum _Source2Mode;
        [Display("Source2 Mode", Groups: new[] { "Source2" }, Order: 50)]
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
        [Display("Data Acquisition Mode", Groups: new[] { "Source2" }, Order: 51)]
        public StandardSweepTypeEnum Source2SweepType { get; set; }

        [EnabledIf("Source2Mode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("Source2SweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [Display("Start", Groups: new[] { "Source2", "Settings" }, Order: 52)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double Source2Start { get; set; }

        [EnabledIf("Source2Mode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("Source2SweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [Display("Stop", Groups: new[] { "Source2", "Settings" }, Order: 53)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double Source2Stop { get; set; }

        [EnabledIf("Source2Mode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("Source2SweepType", StandardSweepTypeEnum.PowerSweep, StandardSweepTypeEnum.CWTime, StandardSweepTypeEnum.PhaseSweep, HideIfDisabled = true)]
        [Display("CW Freq", Groups: new[] { "Source2", "Settings" }, Order: 54)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double Source2CW { get; set; }

        [EnabledIf("Source2SweepType", StandardSweepTypeEnum.CWTime, StandardSweepTypeEnum.PhaseSweep, HideIfDisabled = true)]
        [Display("Sweep Time", Groups: new[] { "Source2", "Settings" }, Order: 54.1)]
        [Unit("mSec", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double Source2SweepTime { get; set; }

        [EnabledIf("Source2Mode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("Source2SweepType", StandardSweepTypeEnum.SegmentSweep, HideIfDisabled = true)]
        [Display("Segment Table", Groups: new[] { "Source2", "Settings" }, Order: 56)]
        public List<SegmentDefinition> Source2SegmentDefinition { get; set; }

        [EnabledIf("Source2Mode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Offset", Groups: new[] { "Source2", "Settings" }, Order: 57)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double Source2Offset { get; set; }

        [EnabledIf("Source2Mode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Multiplier", Groups: new[] { "Source2", "Settings" }, Order: 58)]
        [Unit("", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double Source2Multiplier { get; set; }

        [EnabledIf("Source2Mode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Divisor", Groups: new[] { "Source2", "Settings" }, Order: 59)]
        [Unit("", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double Source2Divisor { get; set; }
        #endregion

        #region Source3
        private FOMModeEnum _Source3Mode;
        [Display("Source3 Mode", Groups: new[] { "Source3" }, Order: 60)]
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
        [Display("Data Acquisition Mode", Groups: new[] { "Source3" }, Order: 61)]
        public StandardSweepTypeEnum Source3SweepType { get; set; }

        [EnabledIf("Source3Mode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("Source3SweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [Display("Start", Groups: new[] { "Source3", "Settings" }, Order: 62)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double Source3Start { get; set; }

        [EnabledIf("Source3Mode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("Source3SweepType", StandardSweepTypeEnum.LinearFrequency, StandardSweepTypeEnum.LogFrequency, HideIfDisabled = true)]
        [Display("Stop", Groups: new[] { "Source3", "Settings" }, Order: 63)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double Source3Stop { get; set; }

        [EnabledIf("Source3Mode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("Source3SweepType", StandardSweepTypeEnum.PowerSweep, StandardSweepTypeEnum.CWTime, StandardSweepTypeEnum.PhaseSweep, HideIfDisabled = true)]
        [Display("CW Freq", Groups: new[] { "Source3", "Settings" }, Order: 64)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double Source3CW { get; set; }

        [EnabledIf("Source3SweepType", StandardSweepTypeEnum.CWTime, StandardSweepTypeEnum.PhaseSweep, HideIfDisabled = true)]
        [Display("Sweep Time", Groups: new[] { "Source3", "Settings" }, Order: 64.1)]
        [Unit("mSec", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double Source3SweepTime { get; set; }

        [EnabledIf("Source3Mode", FOMModeEnum.UnCoupled, HideIfDisabled = true)]
        [EnabledIf("Source3SweepType", StandardSweepTypeEnum.SegmentSweep, HideIfDisabled = true)]
        [Display("Segment Table", Groups: new[] { "Source3", "Settings" }, Order: 66)]
        public List<SegmentDefinition> Source3SegmentDefinition { get; set; }

        [EnabledIf("Source3Mode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Offset", Groups: new[] { "Source3", "Settings" }, Order: 67)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double Source3Offset { get; set; }

        [EnabledIf("Source3Mode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Multiplier", Groups: new[] { "Source3", "Settings" }, Order: 68)]
        [Unit("", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double Source3Multiplier { get; set; }

        [EnabledIf("Source3Mode", FOMModeEnum.Coupled, HideIfDisabled = true)]
        [Display("Divisor", Groups: new[] { "Source3", "Settings" }, Order: 69)]
        [Unit("", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double Source3Divisor { get; set; }
        #endregion

        #endregion

        public FrequencyOffset()
        {
            EnableFOM = false;
            //FOMXAxisAnnotation = FOMRangeEnum.Receivers;

            PrimarySweepType = StandardSweepTypeEnum.LinearFrequency;
            PrimaryStart = 10e6;
            PrimaryStop = 50e9;
            PrimaryPoints = 201;
            PrimaryCW = 1e9;
            PrimarySweepTime = 0.016884;
            PrimarySegmentDefinition = new List<SegmentDefinition>();
            PrimarySegmentDefinition.Add(new SegmentDefinition { state = true, NumberOfPoints = 21, StartFrequency = 10.5e6, StopFrequency = 1e9 });

            SourceMode = FOMModeEnum.Coupled;
            SourceSweepType = StandardSweepTypeEnum.LinearFrequency;
            SourceStart = 10e6;
            SourceStop = 50e9;
            SourceCW = 1e9;
            SourceSweepTime = 0.016884;
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
            ReceiversSweepTime = 0.016884;
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
            Source2SweepTime = 0.016884;
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
            Source3SweepTime = 0.016884;
            Source3SegmentDefinition = new List<SegmentDefinition>();
            Source3SegmentDefinition.Add(new SegmentDefinition { state = true, NumberOfPoints = 21, StartFrequency = 10.5e6, StopFrequency = 1e9 });
            Source3Offset = 0;
            Source3Multiplier = 1;
            Source3Divisor = 1;

        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            // Primary
            PNAX.SetFOMSweepType(Channel, 1, PrimarySweepType);
            switch (PrimarySweepType)
            {
                case StandardSweepTypeEnum.LinearFrequency:
                case StandardSweepTypeEnum.LogFrequency:
                    PNAX.SetFOMStart(Channel, 1, PrimaryStart);
                    PNAX.SetFOMStop(Channel, 1, PrimaryStop);
                    PNAX.SetPoints(Channel, PrimaryPoints);
                    break;
                case StandardSweepTypeEnum.PowerSweep:
                    PNAX.SetFOMCW(Channel, 1, PrimaryCW);
                    PNAX.SetPoints(Channel, PrimaryPoints);
                    break;
                case StandardSweepTypeEnum.CWTime:
                case StandardSweepTypeEnum.PhaseSweep:
                    PNAX.SetFOMCW(Channel, 1, PrimaryCW);
                    PNAX.SetSweepTime(Channel, PrimarySweepTime);
                    PNAX.SetPoints(Channel, PrimaryPoints);
                    break;
                case StandardSweepTypeEnum.SegmentSweep:
                    SetFOMSegmentValues(1, PrimarySegmentDefinition);
                    break;
            }

            // Source
            if(SourceMode == FOMModeEnum.Coupled)
            {
                PNAX.SetFOMOffset(Channel, 2, SourceOffset);
                PNAX.SetFOMMultiplier(Channel, 2, SourceMultiplier);
                PNAX.SetFOMDivisor(Channel, 2, SourceDivisor);
            }
            else
            {
                PNAX.SetFOMSweepType(Channel, 2, SourceSweepType);
                switch (SourceSweepType)
                {
                    case StandardSweepTypeEnum.LinearFrequency:
                    case StandardSweepTypeEnum.LogFrequency:
                        PNAX.SetFOMStart(Channel, 2, SourceStart);
                        PNAX.SetFOMStop(Channel, 2, SourceStop);
                        //PNAX.SetPoints(Channel, SourcePoints);
                        break;
                    case StandardSweepTypeEnum.PowerSweep:
                        PNAX.SetFOMCW(Channel, 2, SourceCW);
                        //PNAX.SetPoints(Channel, SourcePoints);
                        break;
                    case StandardSweepTypeEnum.CWTime:
                    case StandardSweepTypeEnum.PhaseSweep:
                        PNAX.SetFOMCW(Channel, 2, SourceCW);
                        PNAX.SetSweepTime(Channel, SourceSweepTime);
                        //PNAX.SetPoints(Channel, SourcePoints);
                        break;
                    case StandardSweepTypeEnum.SegmentSweep:
                        SetFOMSegmentValues(2, SourceSegmentDefinition);
                        break;
                }
            }

            // Receivers
            if (ReceiversMode == FOMModeEnum.Coupled)
            {
                PNAX.SetFOMOffset(Channel, 3, ReceiversOffset);
                PNAX.SetFOMMultiplier(Channel, 3, ReceiversMultiplier);
                PNAX.SetFOMDivisor(Channel, 3, ReceiversDivisor);
            }
            else
            {
                PNAX.SetFOMSweepType(Channel, 3, ReceiversSweepType);
                switch (ReceiversSweepType)
                {
                    case StandardSweepTypeEnum.LinearFrequency:
                    case StandardSweepTypeEnum.LogFrequency:
                        PNAX.SetFOMStart(Channel, 3, ReceiversStart);
                        PNAX.SetFOMStop(Channel, 3, ReceiversStop);
                        //PNAX.SetPoints(Channel, ReceiversPoints);
                        break;
                    case StandardSweepTypeEnum.PowerSweep:
                        PNAX.SetFOMCW(Channel, 3, ReceiversCW);
                        //PNAX.SetPoints(Channel, ReceiversPoints);
                        break;
                    case StandardSweepTypeEnum.CWTime:
                    case StandardSweepTypeEnum.PhaseSweep:
                        PNAX.SetFOMCW(Channel, 3, ReceiversCW);
                        PNAX.SetSweepTime(Channel, ReceiversSweepTime);
                        //PNAX.SetPoints(Channel, ReceiversPoints);
                        break;
                    case StandardSweepTypeEnum.SegmentSweep:
                        SetFOMSegmentValues(3, ReceiversSegmentDefinition);
                        break;
                }
            }


            // Source2
            if (Source2Mode == FOMModeEnum.Coupled)
            {
                PNAX.SetFOMOffset(Channel, 4, Source2Offset);
                PNAX.SetFOMMultiplier(Channel, 4, Source2Multiplier);
                PNAX.SetFOMDivisor(Channel, 4, Source2Divisor);
            }
            else
            {
                PNAX.SetFOMSweepType(Channel, 4, Source2SweepType);
                switch (Source2SweepType)
                {
                    case StandardSweepTypeEnum.LinearFrequency:
                    case StandardSweepTypeEnum.LogFrequency:
                        PNAX.SetFOMStart(Channel, 4, Source2Start);
                        PNAX.SetFOMStop(Channel, 4, Source2Stop);
                        //PNAX.SetPoints(Channel, Source2Points);
                        break;
                    case StandardSweepTypeEnum.PowerSweep:
                        PNAX.SetFOMCW(Channel, 4, Source2CW);
                        //PNAX.SetPoints(Channel, Source2Points);
                        break;
                    case StandardSweepTypeEnum.CWTime:
                    case StandardSweepTypeEnum.PhaseSweep:
                        PNAX.SetFOMCW(Channel, 4, Source2CW);
                        PNAX.SetSweepTime(Channel, Source2SweepTime);
                        //PNAX.SetPoints(Channel, Source2Points);
                        break;
                    case StandardSweepTypeEnum.SegmentSweep:
                        SetFOMSegmentValues(4, Source2SegmentDefinition);
                        break;
                }
            }


            // Source3
            if (Source3Mode == FOMModeEnum.Coupled)
            {
                PNAX.SetFOMOffset(Channel, 5, Source3Offset);
                PNAX.SetFOMMultiplier(Channel, 5, Source3Multiplier);
                PNAX.SetFOMDivisor(Channel, 5, Source3Divisor);
            }
            else
            {
                PNAX.SetFOMSweepType(Channel, 5, Source3SweepType);
                switch (Source3SweepType)
                {
                    case StandardSweepTypeEnum.LinearFrequency:
                    case StandardSweepTypeEnum.LogFrequency:
                        PNAX.SetFOMStart(Channel, 5, Source3Start);
                        PNAX.SetFOMStop(Channel, 5, Source3Stop);
                        //PNAX.SetPoints(Channel, Source3Points);
                        break;
                    case StandardSweepTypeEnum.PowerSweep:
                        PNAX.SetFOMCW(Channel, 5, Source3CW);
                        //PNAX.SetPoints(Channel, Source3Points);
                        break;
                    case StandardSweepTypeEnum.CWTime:
                    case StandardSweepTypeEnum.PhaseSweep:
                        PNAX.SetFOMCW(Channel, 5, Source3CW);
                        PNAX.SetSweepTime(Channel, Source3SweepTime);
                        //PNAX.SetPoints(Channel, Source3Points);
                        break;
                    case StandardSweepTypeEnum.SegmentSweep:
                        SetFOMSegmentValues(5, Source3SegmentDefinition);
                        break;
                }
            }


            PNAX.SetFOMState(Channel, EnableFOM);

            UpgradeVerdict(Verdict.Pass);
        }

        public void SetFOMSegmentValues(int Range, List<SegmentDefinition> SegmentDefinitions)
        {
            PNAX.FOMSegmentDeleteAllSegments(Channel, Range);
            int segment = 0;
            foreach (SegmentDefinition a in SegmentDefinitions)
            {
                segment = PNAX.FOMSegmentAdd(Channel, Range);
                PNAX.FOMSetSegmentState(Channel, Range, segment, a.state);
                PNAX.FOMSetSegmentNumberOfPoints(Channel, Range, segment, a.NumberOfPoints);
                PNAX.FOMSetSegmentStartFrequency(Channel, Range, segment, a.StartFrequency);
                PNAX.FOMSetSegmentStopFrequency(Channel, Range, segment, a.StopFrequency);
            }
            PNAX.SetFOMSweepType(Channel, Range, StandardSweepTypeEnum.SegmentSweep);
        }
    }
}
