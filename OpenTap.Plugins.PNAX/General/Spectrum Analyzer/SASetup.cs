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

namespace OpenTap.Plugins.PNAX.General.Spectrum_Analyzer
{
    public class SASegmentDefinition
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

        [Display("MT Ref", Order: 5)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double MTRef { get; set; }

        [Display("Vect Avg", Order: 6)]
        public int VectAvg { get; set; }

        [Display("D. Threshold", Order: 7)]
        [Unit("dBm")]
        public double DThreshold { get; set; }

        [Display("Video BW", Order: 8)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "000.000000")]
        public double VBW { get; set; }
    }

    [Flags]
    public enum SASegmentAttributes
    {
        None = 0,
        [Display("MT Ref")]
        MultiToneRef = 1,
        [Display("Vect Avg")]
        VectorAverage = 2,
        [Display("D. Threshold")]
        DataThreshold = 4,
        [Display("Video BW")]
        VideoBandwidth = 8
    }

    [AllowAsChildIn(typeof(SpectrumAnalyzerChannel))]
    [Display("SA Setup", Groups: new[] { "PNA-X", "General", "Spectrum Analyzer" }, Description: "Insert a description here")]
    public class SASetup : PNABaseStep
    {
        #region Settings
        private SASweepTypeEnum _SASweepType;
        [Display("Data Acquisition Mode", Order: 10)]
        public SASweepTypeEnum SASweepType
        {
            get
            {
                return _SASweepType;
            }
            set
            {
                _SASweepType = value;
                SAEnableSegmentSweepSettings = false;
                if (_SASweepType == SASweepTypeEnum.SegmentSweep)
                {
                    SAEnableSegmentSweepSettings = true;
                }
            }
        }

        [EnabledIf("SASweepType", SASweepTypeEnum.LinearFrequency, HideIfDisabled = true)]
        [Display("Start", Group: "Sweep Properties", Order: 20)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double SweepPropertiesStart { get; set; }

        [EnabledIf("SASweepType", SASweepTypeEnum.LinearFrequency, HideIfDisabled = true)]
        [Display("Stop", Group: "Sweep Properties", Order: 21)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepPropertiesStop { get; set; }

        [EnabledIf("SASweepType", SASweepTypeEnum.LinearFrequency, HideIfDisabled = true)]
        [Display("Center", Group: "Sweep Properties", Order: 22)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double SweepPropertiesCenter { get; set; }

        [EnabledIf("SASweepType", SASweepTypeEnum.LinearFrequency, HideIfDisabled = true)]
        [Display("Span", Group: "Sweep Properties", Order: 23)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double SweepPropertiesSpan { get; set; }

        [EnabledIf("SASweepType", SASweepTypeEnum.LinearFrequency, HideIfDisabled = true)]
        [Display("Points", Group: "Sweep Properties", Order: 24)]
        public int SweepPropertiesPoints { get; set; }

        [EnabledIf("ResolutionBandwithAuto", false, HideIfDisabled = false)]
        [Display("Resolution Bandwidth", Group: "Processing", Order: 30)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "000.000000")]
        public double ResolutionBandwidth { get; set; }

        [Display("Resolution Bandwidth Auto", Group: "Processing", Order: 31)]
        public bool ResolutionBandwithAuto { get; set; }

        [EnabledIf("VideoBandwithAuto", false, HideIfDisabled = false)]
        [Display("Video Bandwidth", Group: "Processing", Order: 32)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "000.000000")]
        public double VideoBandwidth { get; set; }

        [Display("Video Bandwidth Auto", Group: "Processing", Order: 33)]
        public bool VideoBandwithAuto { get; set; }

        [Display("Detector Type", Group: "Processing", Order: 34)]
        public SADetectorTypeEnum DetectorType { get; set; }

        [Display("Bypass", Group: "Processing", Order: 34.1)]
        public bool DetectorTypeBypass { get; set; }

        [Display("Video Average Type", Group: "Processing", Order: 35)]
        public SAVideoAverageTypeEnum VideoAverageType { get; set; }


        [Display("A", Group: "Attenuators", Order: 40)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0")]
        public double RcvrAAttenuator { get; set; }

        [Display("B", Group: "Attenuators", Order: 41)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0")]
        public double RcvrBAttenuator { get; set; }

        [Display("C", Group: "Attenuators", Order: 42)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0")]
        public double RcvrCAttenuator { get; set; }

        [Display("D", Group: "Attenuators", Order: 43)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0")]
        public double RcvrDAttenuator { get; set; }

        [Browsable(false)]
        public bool SAEnableSegmentSweepSettings { get; set; } = false;

        [EnabledIf("SAEnableSegmentSweepSettings", true, HideIfDisabled = true)]
        [Display("Segment Definition Type", Group: "Sweep Properties", Order: 30)]
        public SegmentDefinitionTypeEnum SASegmentDefinitionType { get; set; }

        [EnabledIf("SAEnableSegmentSweepSettings", true, HideIfDisabled = true)]
        [EnabledIf("SASegmentDefinitionType", SegmentDefinitionTypeEnum.File, HideIfDisabled = false)]
        [Display("Segment Table File Name", Group: "Sweep Properties", Order: 31)]
        [FilePath]
        public string SASegmentTable { get; set; }

        [EnabledIf("SAEnableSegmentSweepSettings", true, HideIfDisabled = true)]
        [EnabledIf("SASegmentDefinitionType", SegmentDefinitionTypeEnum.List, HideIfDisabled = false)]
        [Display("Segment Table", Group: "Sweep Properties", Order: 32)]
        public List<SASegmentDefinition> SAsegmentDefinitions { get; set; }

        [EnabledIf("SAEnableSegmentSweepSettings", true, HideIfDisabled = true)]
        [EnabledIf("SASegmentDefinitionType", SegmentDefinitionTypeEnum.List, HideIfDisabled = false)]
        [Display("Show Table", Group: "Sweep Properties", Order: 33)]
        public bool SAShowTable { get; set; }

        [EnabledIf("SAEnableSegmentSweepSettings", true, HideIfDisabled = true)]
        [EnabledIf("SASegmentDefinitionType", SegmentDefinitionTypeEnum.List, HideIfDisabled = false)]
        [Display("Segment Attributes", Group: "Sweep Properties", Order: 34)]
        public SASegmentAttributes SASegmentAttributes { get; set; }

        #endregion

        public SASetup()
        {
            SweepPropertiesStart = 10e6;
            SweepPropertiesStop = 50e9;
            SweepPropertiesCenter = 25.05e9;
            SweepPropertiesSpan = 49.99e9;
            SweepPropertiesPoints = 1001;

            ResolutionBandwidth = 100e3;
            ResolutionBandwithAuto = true;
            VideoBandwidth = 100e3;
            VideoBandwithAuto = true;

            DetectorType = SADetectorTypeEnum.Peak;
            VideoAverageType = SAVideoAverageTypeEnum.Power;

            SASegmentDefinitionType = SegmentDefinitionTypeEnum.List;
            SAsegmentDefinitions = new List<SASegmentDefinition>
            {
                new SASegmentDefinition { state = true, NumberOfPoints = 21, StartFrequency = 10.5e6, StopFrequency = 1e9, MTRef = 0.0, VectAvg = 1, VBW = 1e6, DThreshold = -60.0 }
            };
            SAShowTable = false;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            switch (SASweepType)
            {
                case SASweepTypeEnum.LinearFrequency:
                    PNAX.SetSASweepType(Channel, SASweepType);
                    PNAX.SetStart(Channel, SweepPropertiesStart);
                    PNAX.SetStop(Channel, SweepPropertiesStop);
                    PNAX.SetPoints(Channel, SweepPropertiesPoints);
                    break;
                case SASweepTypeEnum.SegmentSweep:
                    SASetSegmentValues();
                    PNAX.SetSASweepType(Channel, SASweepType);
                    break;
            }

            PNAX.SetSAResolutionBandwidth(Channel, ResolutionBandwidth);
            PNAX.SetSAResolutionBandwidthAuto(Channel, ResolutionBandwithAuto);
            PNAX.SetSAVideoBandwidth(Channel, VideoBandwidth);
            PNAX.SetSAVideoBandwidthAuto(Channel, VideoBandwithAuto);
            PNAX.SetSADetectorType(Channel, DetectorType);
            PNAX.SetSADetectorBypass(Channel, DetectorTypeBypass);
            PNAX.SetSAVideoAverageType(Channel, VideoAverageType);

            PNAX.SetSAReceiverAttenuation(Channel, SAReceiverAttenuatorEnum.AREC, RcvrAAttenuator);
            PNAX.SetSAReceiverAttenuation(Channel, SAReceiverAttenuatorEnum.BREC, RcvrBAttenuator);
            PNAX.SetSAReceiverAttenuation(Channel, SAReceiverAttenuatorEnum.CREC, RcvrCAttenuator);
            PNAX.SetSAReceiverAttenuation(Channel, SAReceiverAttenuatorEnum.DREC, RcvrDAttenuator);

            UpgradeVerdict(Verdict.Pass);
        }

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            List<(string, object)> retVal = new List<(string, object)>
            {
                ("SA Data Acquisition Mode", SASweepType)
            };

            switch (SASweepType)
            {
                case SASweepTypeEnum.LinearFrequency:
                    retVal.Add(("SA Start", SweepPropertiesStart));
                    retVal.Add(("SA Stop", SweepPropertiesStop));
                    retVal.Add(("SA Points", SweepPropertiesPoints));
                    break;
                case SASweepTypeEnum.SegmentSweep:
                    break;
            }
            retVal.Add(("SA Resolution Bandwidth", ResolutionBandwidth));
            retVal.Add(("SA Resolution Bandwith Auto", ResolutionBandwithAuto));
            retVal.Add(("SA Video Bandwidth", VideoBandwidth));
            retVal.Add(("SA Video Bandwith Auto", VideoBandwithAuto));
            retVal.Add(("SA Detector Type", DetectorType));
            retVal.Add(("SA Detector Type Bypass", DetectorTypeBypass));
            retVal.Add(("SA Video Average Type", VideoAverageType));
            retVal.Add(("SA Rcvr A Attenuator", RcvrAAttenuator));
            retVal.Add(("SA Rcvr B Attenuator", RcvrBAttenuator));
            retVal.Add(("SA Rcvr C Attenuator", RcvrCAttenuator));
            retVal.Add(("SA Rcvr D Attenuator", RcvrDAttenuator));

            return retVal;
        }

        public void SASetSegmentValues()
        {
            if (SASegmentDefinitionType == SegmentDefinitionTypeEnum.File)
            {
                Log.Error("Load file Not implemented!");
            }
            else
            {
                PNAX.SegmentDeleteAllSegments(Channel);
                int segment = 0;
                foreach (SASegmentDefinition a in SAsegmentDefinitions)
                {
                    segment = PNAX.SegmentAdd(Channel);
                    PNAX.SetSegmentState(Channel, segment, a.state);
                    PNAX.SetSegmentNumberOfPoints(Channel, segment, a.NumberOfPoints);
                    PNAX.SetSegmentStartFrequency(Channel, segment, a.StartFrequency);
                    PNAX.SetSegmentStopFrequency(Channel, segment, a.StopFrequency);

                    if (SASegmentAttributes.HasFlag(SASegmentAttributes.MultiToneRef))
                    {
                        PNAX.SetSegmentSAMTReferenceControl(Channel, segment, SAOnOffTypeEnum.On);
                        PNAX.SetSegmentSAMTReference(Channel, segment, a.MTRef);
                    }
                    if (SASegmentAttributes.HasFlag(SASegmentAttributes.DataThreshold))
                    {
                        PNAX.SetSegmentSADataThresholdControl(Channel, segment, SAOnOffTypeEnum.On);
                        PNAX.SetSegmentSADataThreshold(Channel, segment, a.DThreshold);
                    }
                    if (SASegmentAttributes.HasFlag(SASegmentAttributes.VectorAverage))
                    {
                        PNAX.SetSegmentSAVectorAverageControl(Channel, segment, SAOnOffTypeEnum.On);
                        PNAX.SetSegmentSAVectorAverage(Channel, segment, a.VectAvg);
                    }
                    if (SASegmentAttributes.HasFlag(SASegmentAttributes.VideoBandwidth))
                    {
                        PNAX.SetSegmentSAVideoBWControl(Channel, segment, SAOnOffTypeEnum.On);
                        PNAX.SetSegmentSAVideoBW(Channel, segment, a.VBW);
                    }
                }
                if (SAShowTable)
                {
                    PNAX.SetSegmentTableShow(Channel, true, 1);
                }
            }

        }

    }
}
