// Author: CMontes
// Copyright:   Copyright 2024 Keysight Technologies
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
using System.Xml.Serialization;

namespace OpenTap.Plugins.PNAX
{
    [AllowAsChildIn(typeof(StandardChannel))]
    [Display("Pulse Setup", Groups: new[] { "Network Analyzer", "General" }, Description: "Insert a description here")]
    public class PulseSetup : PNABaseStep
    {
        #region Settings
        [Browsable(false)]
        public bool IsSettingReadOnly { get; set; } = false;


        [Display("Pulse Mode", Group: "Pulse Measurement", Order: 21)]
        public PulseModeEnumtype PulseMode { get; set; }

        [Display("Pulse Width", Group: "Pulse Timing", Order: 30)]
        [Unit("sec", UseEngineeringPrefix: true, StringFormat: "000.000")]
        public double PulseWidthPrimary { get; set; }

        private double _PulsePeriodPrimary;
        [Display("Pulse Period", Group: "Pulse Timing", Order: 31)]
        [Unit("sec", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double PulsePeriodPrimary
        {
            get
            {
                return _PulsePeriodPrimary;
            }
            set
            {
                _PulsePeriodPrimary = value;
                foreach (var a in this.ChildTestSteps)
                {
                    if (a is PulseGenerators)
                    {
                        (a as PulseGenerators).Period = _PulsePeriodPrimary;
                    }
                }
            }
        }


        private double _PulseFrequencyPrimary;
        [Display("Pulse Frequency", Group: "Pulse Timing", Order: 32)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double PulseFrequencyPrimary
        {
            get
            {
                return _PulseFrequencyPrimary;
            }
            set
            {
                _PulseFrequencyPrimary = value;
                foreach (var a in this.ChildTestSteps)
                {
                    if (a is PulseGenerators)
                    {
                        (a as PulseGenerators).Frequency = _PulseFrequencyPrimary;
                    }
                }
            }
        }



        [Display("Autoselect Pulse Detection Method", Groups: new[] { "Properties", "Pulse Detection Method" }, Order: 41)]
        public bool PulseDetectionMethodAuto { get; set; }

        [EnabledIf("PulseDetectionMethodAuto", false, HideIfDisabled = false)]
        [Display("Autoselect Pulse Detection Method", Groups: new[] { "Properties", "Pulse Detection Method" }, Order: 42)]
        public PulseDetectionMethodEnumtype PulseDetectionMethod { get; set; }

        [EnabledIf("PulseDetectionMethodAuto", false, HideIfDisabled = false)]
        [EnabledIf("PulseDetectionMethod", PulseDetectionMethodEnumtype.Narrowband, HideIfDisabled = false)]
        [Display("SW Gating", Groups: new[] { "Properties", "Pulse Detection Method" }, Order: 43)]
        public bool PulseDetectionMethodSWGating { get; set; }

        [Display("Autoselect IF Path Gain and Loss", Groups: new[] { "Properties"}, Order: 44)]
        public bool IfPathGainAndLossAuto { get; set; }

        // TODO implement IFPath dialog
        [EnabledIf("IsSettingReadOnly", true, HideIfDisabled = false)]
        [Display("IF Path...", Groups: new[] { "Properties" }, Order: 44.1)]
        public List<IFPathDefinition> IFPathList { get; set; }


        [EnabledIf("PulseDetectionMethodAuto", false, HideIfDisabled = false)]
        [EnabledIf("PulseDetectionMethod", PulseDetectionMethodEnumtype.Narrowband, HideIfDisabled = false)]
        [Display("Optimize Pulse Frequency", Groups: new[] { "Properties" }, Order: 45)]
        public bool OptimizePulseFrequency { get; set; }

        [Display("Autoselect Profile Sweep Time", Groups: new[] { "Properties" }, Order: 46)]
        public bool ProfileSweepTimeAuto { get; set; }

        [XmlIgnore]
        [EnabledIf("IsSettingReadOnly", true, HideIfDisabled = false)]
        [Browsable(true)]
        [Display("Sweep Time", Groups: new[] { "Properties" }, Order: 47)]
        [Unit("sec", UseEngineeringPrefix: true, StringFormat: "00.000")]
        public double SweepTime { get; set; }

        [Display("Number of Points", Groups: new[] { "Properties" }, Order: 48)]
        public int NumberOfPoints { get; set; }

        [Display("IFBW", Groups: new[] { "Properties" }, Order: 49)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "00")]
        public double IFBW { get; set; }

        [Display("Primary Clock", Groups: new[] { "Measurement Timing" }, Order: 50)]
        public PulsePrimaryClockEnumtype PulseClockPrimary { get; set; }

        [Display("Autoselect Width & Delay", Groups: new[] { "Measurement Timing" }, Order: 51)]
        public bool WidthAndDelayAuto { get; set; }

        [Display("Autoselect Pulse Generators", Groups: new[] { "Measurement Timing" }, Order: 52)]
        public bool PulseGeneratorsAuto { get; set; }

        private PulseTriggerEnumtype _PulseTriggerType;
        [Display("Trigger Source", Groups: new[] { "Pulse Trigger" }, Order: 60)]
        public PulseTriggerEnumtype PulseTriggerType
        {
            get
            {
                return _PulseTriggerType;
            }
            set
            {
                _PulseTriggerType = value;
                foreach (var a in this.ChildTestSteps)
                {
                    if (a is PulseGenerators)
                    {
                        (a as PulseGenerators).PulseTriggerType = value;
                    }
                }
            }
        }


        [EnabledIf("PulseTriggerType", PulseTriggerEnumtype.External, HideIfDisabled = false)]
        [Display("Trigger Level/Edge", Groups: new[] { "Pulse Trigger" }, Order: 61)]
        public PulseTriggerLevelEdgeEnumtype pulseTriggerLevelEdge { get; set; }

        private bool _SynchADCUsingPulseTrigger;
        [Display("Synchronize ADCs Using Pulse Trigger", Groups: new[] { "Pulse Trigger" }, Order: 62)]
        public bool SynchADCUsingPulseTrigger
        {
            get
            {
                return _SynchADCUsingPulseTrigger;
            }
            set
            {
                _SynchADCUsingPulseTrigger = value;
                foreach (var a in this.ChildTestSteps)
                {
                    if (a is PulseGenerators)
                    {
                        (a as PulseGenerators).SynchADCUsingPulseTrigger = value;
                    }
                }
            }
        }

        [Display("ADC trigger delay", Groups: new[] { "Pulse Trigger" }, Order: 63)]
        public double ADCTriggerDelay { get; set; }
        #endregion

        private List<string> _IFPathListOfAvailableValues;
        [Display("IF Path Values", "Editable list for Pulse Gen values", Groups: new[] { "Available Values Setup" }, Order: 101, Collapsed: true)]
        public List<string> IFPathListOfAvailableValues
        {
            get { return _IFPathListOfAvailableValues; }
            set
            {
                _IFPathListOfAvailableValues = value;
                OnPropertyChanged("IFPathListOfAvailableValues");
            }
        }

        private static List<string> _IFInputListOfAvailableValues;
        [Display("IF Input Values", "Editable list for Pulse Gen values", Groups: new[] { "Available Values Setup" }, Order: 101, Collapsed: true)]
        public static List<string> IFInputListOfAvailableValues
        {
            get { return _IFInputListOfAvailableValues; }
            set
            {
                _IFInputListOfAvailableValues = value;
                //PulseSetup.OnPropertyChanged("IFInputListOfAvailableValues");
            }
        }


        public PulseSetup()
        {
            _IFPathListOfAvailableValues = new List<string> { "A", "B", "C", "D", "R1", "R2", "R3", "R4" };
            _IFInputListOfAvailableValues = new List<string> { "Internal", "External" };

            PulseGenerators pulseGenerators = new PulseGenerators { IsControlledByParent = true, Channel = this.Channel };
            this.ChildTestSteps.Add(pulseGenerators);

            PulseMode = PulseModeEnumtype.Off;

            PulseWidthPrimary = 100e-6;
            PulsePeriodPrimary = 1e-3;
            PulseFrequencyPrimary = 1e3;

            PulseDetectionMethodAuto = true;
            PulseDetectionMethod = PulseDetectionMethodEnumtype.Wideband;
            PulseDetectionMethodSWGating = true;
            IfPathGainAndLossAuto = true;
            OptimizePulseFrequency = true;
            ProfileSweepTimeAuto = true;
            //SweepTime = 16.884e-3;
            NumberOfPoints = 201;
            IFBW = 15e3;

            PulseClockPrimary = PulsePrimaryClockEnumtype.Internal;
            WidthAndDelayAuto = true;
            PulseGeneratorsAuto = true;

            PulseTriggerType = PulseTriggerEnumtype.Internal;
            pulseTriggerLevelEdge = PulseTriggerLevelEdgeEnumtype.HighLevel;
            SynchADCUsingPulseTrigger = false;
            ADCTriggerDelay = 250e-3;

        }

        public override void Run()
        {
            RunChildSteps(); // Pulse Generators

            // Pulse Measurement
            PNAX.PulseMode(Channel, PulseMode);

            // Pulse Timing
            PNAX.PulsePrimaryWidth(Channel, PulseWidthPrimary);
            PNAX.PulsePrimaryPeriod(Channel, PulsePeriodPrimary);
            PNAX.PulsePrimaryFrequency(Channel, PulseFrequencyPrimary);

            // Pulse Properties
            PNAX.PulseDetectionMethodAuto(Channel, PulseDetectionMethodAuto);
            if (PulseDetectionMethodAuto == false)
            {
                PNAX.PulseDetectionMethod(Channel, PulseDetectionMethod);
                if (PulseDetectionMethod == PulseDetectionMethodEnumtype.Narrowband)
                {
                    PNAX.PulseDetectionSWGating(Channel, PulseDetectionMethodSWGating);
                }

                PNAX.PulseOptimizePRF(Channel, OptimizePulseFrequency);
            }
            PNAX.PulseIFGainAuto(Channel, IfPathGainAndLossAuto);
            // TODO Add Missing IF Path, IF Input, IF Attenuator, IF Filter, IF Gain, ADC Filter
            PNAX.PulseProfileSweepTimeAuto(Channel, ProfileSweepTimeAuto);
            PNAX.PulseNumberOfPoints(Channel, NumberOfPoints);
            PNAX.PulseIFBW(Channel, IFBW);

            // Measurement Timing
            PNAX.PulsePrimaryClock(Channel, PulseClockPrimary);
            PNAX.PulseWidthAndDelayAuto(Channel, WidthAndDelayAuto);
            PNAX.PulseGeneratorsAutoselect(Channel, PulseGeneratorsAuto);

            // Trigger
            PNAX.PulseGeneratorTrigger(Channel, PulseTriggerType);
            switch (pulseTriggerLevelEdge)
            {
                case PulseTriggerLevelEdgeEnumtype.HighLevel:
                    PNAX.PulseTriggerType(Channel, PulseTriggerTypeEnumtype.Level);
                    PNAX.PulseTriggerPolarity(Channel, PulseTriggerPolarityEnumtype.Positive);
                    break;
                case PulseTriggerLevelEdgeEnumtype.LowLevel:
                    PNAX.PulseTriggerType(Channel, PulseTriggerTypeEnumtype.Level);
                    PNAX.PulseTriggerPolarity(Channel, PulseTriggerPolarityEnumtype.Negative);
                    break;
                case PulseTriggerLevelEdgeEnumtype.PositiveEdge:
                    PNAX.PulseTriggerType(Channel, PulseTriggerTypeEnumtype.Edge);
                    PNAX.PulseTriggerPolarity(Channel, PulseTriggerPolarityEnumtype.Positive);
                    break;
                case PulseTriggerLevelEdgeEnumtype.NegativeEdge:
                    PNAX.PulseTriggerType(Channel, PulseTriggerTypeEnumtype.Edge);
                    PNAX.PulseTriggerPolarity(Channel, PulseTriggerPolarityEnumtype.Negative);
                    break;
            }
            PNAX.PulseGeneratorSyncADCs(Channel, SynchADCUsingPulseTrigger);
            PNAX.PulseGeneratorDelay(Channel, "Pulse0", ADCTriggerDelay);




            // Update Sweep Time on UI
            SweepTime = PNAX.PulseSweepTimeQ(Channel);
            // Update Measurement Timing on UI


            UpgradeVerdict(Verdict.Pass);
        }

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            UpdateMetaData();
            List<(string, object)> retVal = new List<(string, object)>();

            retVal.Add(($"PulseMode", PulseMode));
            retVal.Add(($"PulseWidthPrimary", PulseWidthPrimary));
            retVal.Add(($"PulsePeriodPrimary", PulsePeriodPrimary));
            retVal.Add(($"PulseFrequencyPrimary", PulseFrequencyPrimary));
            retVal.Add(($"PulseDetectionMethodAuto", PulseDetectionMethodAuto));
            if (PulseDetectionMethodAuto == false)
            {
                retVal.Add(($"PulseDetectionMethod", PulseDetectionMethod));
                if (PulseDetectionMethod == PulseDetectionMethodEnumtype.Narrowband)
                {
                    retVal.Add(($"PulseDetectionMethodSWGating", PulseDetectionMethodSWGating));
                }
                retVal.Add(($"OptimizePulseFrequency", OptimizePulseFrequency));

            }
            retVal.Add(($"IfPathGainAndLossAuto", IfPathGainAndLossAuto));
            retVal.Add(($"ProfileSweepTimeAuto", ProfileSweepTimeAuto));
            retVal.Add(($"NumberOfPoints", NumberOfPoints));
            retVal.Add(($"IFBW", IFBW));
            retVal.Add(($"PulseClockPrimary", PulseClockPrimary));
            retVal.Add(($"WidthAndDelayAuto", WidthAndDelayAuto));
            retVal.Add(($"PulseGeneratorsAuto", PulseGeneratorsAuto));
            retVal.Add(($"PulseTriggerType", PulseTriggerType));
            retVal.Add(($"SynchADCUsingPulseTrigger", SynchADCUsingPulseTrigger));
            retVal.Add(($"Pulse0GeneratorDelay", ADCTriggerDelay));
            retVal.Add(($"SweepTime", SweepTime));


            foreach (var a in MetaData)
            {
                retVal.Add(a);
            }

            return retVal;
        }

        [Browsable(true)]
        [Display("Update MetaData", Groups: new[] { "MetaData" }, Order: 1000.2)]
        public override void UpdateMetaData()
        {
            MetaData = new List<(string, object)>();

            foreach (var ch in this.ChildTestSteps)
            {
                List<(string, object)> ret = (ch as PulseGenerators).GetMetaData();
                foreach (var it in ret)
                {
                    MetaData.Add(it);
                }
            }
        }

    }

    public class IFPathDefinition
    {
        [Display("IF Path", Order: 1)]
        [AvailableValues(nameof(PulseSetup.IFPathListOfAvailableValues))]
        public String PathName { get; set; }

        [Display("IF Input", Order: 2)]
        [AvailableValues(nameof(IFInputValues))]
        public String IFInput { get; set; }

        public List<string> IFInputValues = PulseSetup.IFInputListOfAvailableValues;
        public IFPathDefinition()
        {
            PathName = "A";
            IFInput = "Internal";
        }
    }

}
