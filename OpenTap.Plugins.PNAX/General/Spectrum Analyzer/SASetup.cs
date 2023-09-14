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
    [AllowAsChildIn(typeof(SpectrumAnalyzerChannel))]
    [Display("SA Setup", Groups: new[] { "PNA-X", "General", "Spectrum Analyzer" }, Description: "Insert a description here")]
    public class SASetup : GeneralBaseStep
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
                EnableSegmentSweepSettings = false;
                if (_SASweepType == SASweepTypeEnum.SegmentSweep)
                {
                    EnableSegmentSweepSettings = true;
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

        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetSASweepType(Channel, SASweepType);
            switch (SASweepType)
            {
                case SASweepTypeEnum.LinearFrequency:
                    PNAX.SetStart(Channel, SweepPropertiesStart);
                    PNAX.SetStop(Channel, SweepPropertiesStop);
                    PNAX.SetPoints(Channel, SweepPropertiesPoints);
                    break;
                case SASweepTypeEnum.SegmentSweep:
                    SetSegmentValues();
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
            List<(String, object)> retVal = new List<(string, object)>();

            retVal.Add(("SA Data Acquisition Mode", SASweepType));

            switch (SASweepType)
            {
                case SASweepTypeEnum.LinearFrequency:
                    retVal.Add(("SA Start", SweepPropertiesStart));
                    retVal.Add(("SA Stop", SweepPropertiesStop));
                    retVal.Add(("SA Points", SweepPropertiesPoints));
                    break;
                case SASweepTypeEnum.SegmentSweep:
                    //SetSegmentValues();
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
    }
}
