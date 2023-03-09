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
    public enum MixerFrequencyTypeEnum
    {
        StartStop,
        CenterSpan,
        Fixed
    }

    public enum SidebandTypeEnum
    {
        High,
        Low
    }

    [AllowAsChildIn(typeof(GainCompressionChannel))]
    [AllowAsChildIn(typeof(SweptIMDChannel))]
    [Display("Mixer Frequency", Groups: new[] { "PNA-X", "Converters" }, Description: "Insert a description here", Order: 3)]
    public class MixerFrequencyTestStep : TestStep
    {
        #region Settings

        [Browsable(false)]
        public bool IsControlledByParent { get; set; } = false;
        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
        [Display("PNA", Group: "Instrument Settings", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Browsable(false)]
        public bool DoubleStage { get; set; }

        private ConverterStagesEnum _ConverterStagesEnum;
        [Display("Converter Stages", Order: 1)]
        public ConverterStagesEnum ConverterStages
        {
            get
            {
                try
                {
                    _ConverterStagesEnum = GetParent<ConverterChannelBase>().ConverterStages;
                    UpdateConverterStages();
                }
                catch (Exception ex)
                {
                    Log.Info(ex.Message);
                }
                return _ConverterStagesEnum;
            }
            set
            {
                _ConverterStagesEnum = value;
                UpdateConverterStages();
            }
        }

        private void UpdateConverterStages()
        {
            if (_ConverterStagesEnum == ConverterStagesEnum._2)
            {
                DoubleStage = true;
            }
            else
            {
                DoubleStage = false;
            }
        }


        #region Input
        [Browsable(false)]
        public bool IsInputMixerFrequencyTypeStartStop { get; set; }
        [Browsable(false)]
        public bool IsInputMixerFrequencyTypeCenterSpan { get; set; }
        [Browsable(false)]
        public bool IsInputMixerFrequencyTypeFixed { get; set; }


        private MixerFrequencyTypeEnum _InputMixerFrequencyType;
        [Display("Input", Groups: new[] { "Mixer Frequency", "Input" }, Order: 10)]
        public MixerFrequencyTypeEnum InputMixerFrequencyType 
        {
            get
            {
                return _InputMixerFrequencyType;
            }
            set
            {
                _InputMixerFrequencyType = value;
                if (_InputMixerFrequencyType== MixerFrequencyTypeEnum.StartStop)
                {
                    IsInputMixerFrequencyTypeStartStop = true;
                    IsInputMixerFrequencyTypeCenterSpan = false;
                    IsInputMixerFrequencyTypeFixed = false;
                }
                else if (_InputMixerFrequencyType == MixerFrequencyTypeEnum.CenterSpan)
                {
                    IsInputMixerFrequencyTypeStartStop = false;
                    IsInputMixerFrequencyTypeCenterSpan = true;
                    IsInputMixerFrequencyTypeFixed = false;
                }
                else if (_InputMixerFrequencyType == MixerFrequencyTypeEnum.Fixed)
                {
                    IsInputMixerFrequencyTypeStartStop = false;
                    IsInputMixerFrequencyTypeCenterSpan = false;
                    IsInputMixerFrequencyTypeFixed = true;
                }
            }
        }

        [EnabledIf("IsInputMixerFrequencyTypeStartStop", true, HideIfDisabled = true)]
        [Display("Start", Groups: new[] { "Mixer Frequency", "Input" }, Order: 11)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double InputMixerFrequencyStart { get; set; }

        [EnabledIf("IsInputMixerFrequencyTypeStartStop", true, HideIfDisabled = true)]
        [Display("Stop", Groups: new[] { "Mixer Frequency", "Input" }, Order: 12)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double InputMixerFrequencyStop { get; set; }

        [EnabledIf("IsInputMixerFrequencyTypeCenterSpan", true, HideIfDisabled = true)]
        [Display("Center", Groups: new[] { "Mixer Frequency", "Input" }, Order: 13)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double InputMixerFrequencyCenter { get; set; }

        [EnabledIf("IsInputMixerFrequencyTypeCenterSpan", true, HideIfDisabled = true)]
        [Display("Span", Groups: new[] { "Mixer Frequency", "Input" }, Order: 14)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double InputMixerFrequencySpan { get; set; }

        [EnabledIf("IsInputMixerFrequencyTypeFixed", true, HideIfDisabled = true)]
        [Display("Fixed", Groups: new[] { "Mixer Frequency", "Input" }, Order: 15)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double InputMixerFrequencyFixed { get; set; }
        #endregion

        #region LO1
        [Browsable(false)]
        public bool IsLO1MixerFrequencyTypeStartStop { get; set; }
        [Browsable(false)]
        public bool IsLO1MixerFrequencyTypeCenterSpan { get; set; }
        [Browsable(false)]
        public bool IsLO1MixerFrequencyTypeFixed { get; set; }

        private MixerFrequencyTypeEnum _LO1MixerFrequencyType;
        [Display("LO1", Groups: new[] { "Mixer Frequency", "LO1" }, Order: 20)]
        public MixerFrequencyTypeEnum LO1MixerFrequencyType
        {
            get
            {
                return _LO1MixerFrequencyType;
            }
            set
            {
                _LO1MixerFrequencyType = value;
                if (_LO1MixerFrequencyType == MixerFrequencyTypeEnum.StartStop)
                {
                    IsLO1MixerFrequencyTypeStartStop = true;
                    IsLO1MixerFrequencyTypeCenterSpan = false;
                    IsLO1MixerFrequencyTypeFixed = false;
                }
                else if (_LO1MixerFrequencyType == MixerFrequencyTypeEnum.CenterSpan)
                {
                    IsLO1MixerFrequencyTypeStartStop = false;
                    IsLO1MixerFrequencyTypeCenterSpan = true;
                    IsLO1MixerFrequencyTypeFixed = false;
                }
                else if (_LO1MixerFrequencyType == MixerFrequencyTypeEnum.Fixed)
                {
                    IsLO1MixerFrequencyTypeStartStop = false;
                    IsLO1MixerFrequencyTypeCenterSpan = false;
                    IsLO1MixerFrequencyTypeFixed = true;
                }
            }
        }

        [EnabledIf("IsLO1MixerFrequencyTypeStartStop", true, HideIfDisabled = true)]
        [Display("Start", Groups: new[] { "Mixer Frequency", "LO1" }, Order: 21)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LO1MixerFrequencyStart { get; set; }

        [EnabledIf("IsLO1MixerFrequencyTypeStartStop", true, HideIfDisabled = true)]
        [Display("Stop", Groups: new[] { "Mixer Frequency", "LO1" }, Order: 22)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LO1MixerFrequencyStop { get; set; }

        [EnabledIf("IsLO1MixerFrequencyTypeCenterSpan", true, HideIfDisabled = true)]
        [Display("Center", Groups: new[] { "Mixer Frequency", "LO1" }, Order: 23)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LO1MixerFrequencyCenter { get; set; }

        [EnabledIf("IsLO1MixerFrequencyTypeCenterSpan", true, HideIfDisabled = true)]
        [Display("Span", Groups: new[] { "Mixer Frequency", "LO1" }, Order: 24)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LO1MixerFrequencySpan { get; set; }

        [EnabledIf("IsLO1MixerFrequencyTypeFixed", true, HideIfDisabled = true)]
        [Display("Fixed", Groups: new[] { "Mixer Frequency", "LO1" }, Order: 25)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LO1MixerFrequencyFixed { get; set; }

        [Display("Input > LO", Groups: new[] { "Mixer Frequency", "LO1" }, Order: 26)]
        public bool InputGTLO1 { get; set; }
        #endregion

        #region IF
        [Browsable(false)]
        public bool IsIFMixerFrequencyTypeStartStop { get; set; }
        [Browsable(false)]
        public bool IsIFMixerFrequencyTypeCenterSpan { get; set; }
        [Browsable(false)]
        public bool IsIFMixerFrequencyTypeFixed { get; set; }


        private MixerFrequencyTypeEnum _IFMixerFrequencyType;
        [Display("IF", Groups: new[] { "Mixer Frequency", "IF" }, Order: 30)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        public MixerFrequencyTypeEnum IFMixerFrequencyType
        {
            get
            {
                return _IFMixerFrequencyType;
            }
            set
            {
                _IFMixerFrequencyType = value;
                if (_IFMixerFrequencyType == MixerFrequencyTypeEnum.StartStop)
                {
                    IsIFMixerFrequencyTypeStartStop = true;
                    IsIFMixerFrequencyTypeCenterSpan = false;
                    IsIFMixerFrequencyTypeFixed = false;
                }
                else if (_IFMixerFrequencyType == MixerFrequencyTypeEnum.CenterSpan)
                {
                    IsIFMixerFrequencyTypeStartStop = false;
                    IsIFMixerFrequencyTypeCenterSpan = true;
                    IsIFMixerFrequencyTypeFixed = false;
                }
                else if (_IFMixerFrequencyType == MixerFrequencyTypeEnum.Fixed)
                {
                    IsIFMixerFrequencyTypeStartStop = false;
                    IsIFMixerFrequencyTypeCenterSpan = false;
                    IsIFMixerFrequencyTypeFixed = true;
                }
            }
        }

        [Display("Sideband", Groups: new[] { "Mixer Frequency", "IF" }, Order: 30.5)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        public SidebandTypeEnum IFSidebandType { get; set; }

        [EnabledIf("IsIFMixerFrequencyTypeStartStop", true, HideIfDisabled = true)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("Start", Groups: new[] { "Mixer Frequency", "IF" }, Order: 31)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double IFMixerFrequencyStart { get; set; }

        [EnabledIf("IsIFMixerFrequencyTypeStartStop", true, HideIfDisabled = true)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("Stop", Groups: new[] { "Mixer Frequency", "IF" }, Order: 32)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double IFMixerFrequencyStop { get; set; }

        [EnabledIf("IsIFMixerFrequencyTypeCenterSpan", true, HideIfDisabled = true)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("Center", Groups: new[] { "Mixer Frequency", "IF" }, Order: 33)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double IFMixerFrequencyCenter { get; set; }

        [EnabledIf("IsIFMixerFrequencyTypeCenterSpan", true, HideIfDisabled = true)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("Span", Groups: new[] { "Mixer Frequency", "IF" }, Order: 34)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double IFMixerFrequencySpan { get; set; }

        [EnabledIf("IsIFMixerFrequencyTypeFixed", true, HideIfDisabled = true)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("Fixed", Groups: new[] { "Mixer Frequency", "IF" }, Order: 35)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double IFMixerFrequencyFixed { get; set; }
        #endregion

        #region LO2
        [Browsable(false)]
        public bool IsLO2MixerFrequencyTypeStartStop { get; set; }
        [Browsable(false)]
        public bool IsLO2MixerFrequencyTypeCenterSpan { get; set; }
        [Browsable(false)]
        public bool IsLO2MixerFrequencyTypeFixed { get; set; }

        private MixerFrequencyTypeEnum _LO2MixerFrequencyType;
        [Display("LO2", Groups: new[] { "Mixer Frequency", "LO2" }, Order: 40)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        public MixerFrequencyTypeEnum LO2MixerFrequencyType
        {
            get
            {
                return _LO2MixerFrequencyType;
            }
            set
            {
                _LO2MixerFrequencyType = value;
                if (_LO2MixerFrequencyType == MixerFrequencyTypeEnum.StartStop)
                {
                    IsLO2MixerFrequencyTypeStartStop = true;
                    IsLO2MixerFrequencyTypeCenterSpan = false;
                    IsLO2MixerFrequencyTypeFixed = false;
                }
                else if (_LO2MixerFrequencyType == MixerFrequencyTypeEnum.CenterSpan)
                {
                    IsLO2MixerFrequencyTypeStartStop = false;
                    IsLO2MixerFrequencyTypeCenterSpan = true;
                    IsLO2MixerFrequencyTypeFixed = false;
                }
                else if (_LO2MixerFrequencyType == MixerFrequencyTypeEnum.Fixed)
                {
                    IsLO2MixerFrequencyTypeStartStop = false;
                    IsLO2MixerFrequencyTypeCenterSpan = false;
                    IsLO2MixerFrequencyTypeFixed = true;
                }
            }
        }

        [EnabledIf("IsLO2MixerFrequencyTypeStartStop", true, HideIfDisabled = true)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("Start", Groups: new[] { "Mixer Frequency", "LO2" }, Order: 41)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LO2MixerFrequencyStart { get; set; }

        [EnabledIf("IsLO2MixerFrequencyTypeStartStop", true, HideIfDisabled = true)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("Stop", Groups: new[] { "Mixer Frequency", "LO2" }, Order: 42)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LO2MixerFrequencyStop { get; set; }

        [EnabledIf("IsLO2MixerFrequencyTypeCenterSpan", true, HideIfDisabled = true)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("Center", Groups: new[] { "Mixer Frequency", "LO2" }, Order: 43)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LO2MixerFrequencyCenter { get; set; }

        [EnabledIf("IsLO2MixerFrequencyTypeCenterSpan", true, HideIfDisabled = true)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("Span", Groups: new[] { "Mixer Frequency", "LO2" }, Order: 44)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LO2MixerFrequencySpan { get; set; }

        [EnabledIf("IsLO2MixerFrequencyTypeFixed", true, HideIfDisabled = true)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("Fixed", Groups: new[] { "Mixer Frequency", "LO2" }, Order: 45)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LO2MixerFrequencyFixed { get; set; }

        [Display("IF1 > LO2", Groups: new[] { "Mixer Frequency", "LO2" }, Order: 46)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        public bool IF1GTLO2 { get; set; }
        #endregion

        #region Output
        [Browsable(false)]
        public bool IsOutputMixerFrequencyTypeStartStop { get; set; }
        [Browsable(false)]
        public bool IsOutputMixerFrequencyTypeCenterSpan { get; set; }
        [Browsable(false)]
        public bool IsOutputMixerFrequencyTypeFixed { get; set; }


        private MixerFrequencyTypeEnum _OutputMixerFrequencyType;
        [Display("Output", Groups: new[] { "Mixer Frequency", "Output" }, Order: 50)]
        public MixerFrequencyTypeEnum OutputMixerFrequencyType
        {
            get
            {
                return _OutputMixerFrequencyType;
            }
            set
            {
                _OutputMixerFrequencyType = value;
                if (_OutputMixerFrequencyType == MixerFrequencyTypeEnum.StartStop)
                {
                    IsOutputMixerFrequencyTypeStartStop = true;
                    IsOutputMixerFrequencyTypeCenterSpan = false;
                    IsOutputMixerFrequencyTypeFixed = false;
                }
                else if (_OutputMixerFrequencyType == MixerFrequencyTypeEnum.CenterSpan)
                {
                    IsOutputMixerFrequencyTypeStartStop = false;
                    IsOutputMixerFrequencyTypeCenterSpan = true;
                    IsOutputMixerFrequencyTypeFixed = false;
                }
                else if (_OutputMixerFrequencyType == MixerFrequencyTypeEnum.Fixed)
                {
                    IsOutputMixerFrequencyTypeStartStop = false;
                    IsOutputMixerFrequencyTypeCenterSpan = false;
                    IsOutputMixerFrequencyTypeFixed = true;
                }
            }
        }

        [Display("Sideband", Groups: new[] { "Mixer Frequency", "Output" }, Order: 50.5)]
        public SidebandTypeEnum OutputSidebandType { get; set; }

        [EnabledIf("IsOutputMixerFrequencyTypeStartStop", true, HideIfDisabled = true)]
        [Display("Start", Groups: new[] { "Mixer Frequency", "Output" }, Order: 51)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double OutputMixerFrequencyStart { get; set; }

        [EnabledIf("IsOutputMixerFrequencyTypeStartStop", true, HideIfDisabled = true)]
        [Display("Stop", Groups: new[] { "Mixer Frequency", "Output" }, Order: 52)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double OutputMixerFrequencyStop { get; set; }

        [EnabledIf("IsOutputMixerFrequencyTypeCenterSpan", true, HideIfDisabled = true)]
        [Display("Center", Groups: new[] { "Mixer Frequency", "Output" }, Order: 53)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double OutputMixerFrequencyCenter { get; set; }

        [EnabledIf("IsOutputMixerFrequencyTypeCenterSpan", true, HideIfDisabled = true)]
        [Display("Span", Groups: new[] { "Mixer Frequency", "Output" }, Order: 54)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double OutputMixerFrequencySpan { get; set; }

        [EnabledIf("IsOutputMixerFrequencyTypeFixed", true, HideIfDisabled = true)]
        [Display("Fixed", Groups: new[] { "Mixer Frequency", "Output" }, Order: 55)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double OutputMixerFrequencyFixed { get; set; }
        #endregion



        #endregion

        public MixerFrequencyTestStep()
        {
            ConverterStages = ConverterStagesEnum._1;

            IsInputMixerFrequencyTypeStartStop = true;
            IsInputMixerFrequencyTypeCenterSpan = false;
            IsInputMixerFrequencyTypeFixed = false;

            InputMixerFrequencyType = MixerFrequencyTypeEnum.StartStop;
            InputMixerFrequencyStart = 10.5e6;
            InputMixerFrequencyStop = 66.9995e9;
            InputMixerFrequencyCenter = 33.505e9;
            InputMixerFrequencySpan = 66.989e9;
            InputMixerFrequencyFixed = 1e9;

            IsLO1MixerFrequencyTypeStartStop = false;
            IsLO1MixerFrequencyTypeCenterSpan = false;
            IsLO1MixerFrequencyTypeFixed = true;

            LO1MixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
            LO1MixerFrequencyStart = 0;
            LO1MixerFrequencyStop = 0;
            LO1MixerFrequencyCenter = 0;
            LO1MixerFrequencySpan = 0;
            LO1MixerFrequencyFixed = 0;
            InputGTLO1 = true;

            IsIFMixerFrequencyTypeStartStop = false;
            IsIFMixerFrequencyTypeCenterSpan = true;
            IsIFMixerFrequencyTypeFixed = false;

            IFSidebandType = SidebandTypeEnum.Low;
            IFMixerFrequencyType = MixerFrequencyTypeEnum.CenterSpan;
            IFMixerFrequencyStart = 10.5e6;
            IFMixerFrequencyStop = 66.9995e9;
            IFMixerFrequencyCenter = 33.505e9;
            IFMixerFrequencySpan = 66.989e9;
            IFMixerFrequencyFixed = 10e6;

            IsLO2MixerFrequencyTypeStartStop = false;
            IsLO2MixerFrequencyTypeCenterSpan = false;
            IsLO2MixerFrequencyTypeFixed = true;

            LO2MixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
            LO2MixerFrequencyStart = 0;
            LO2MixerFrequencyStop = 0;
            LO2MixerFrequencyCenter = 0;
            LO2MixerFrequencySpan = 0;
            LO2MixerFrequencyFixed = 0;
            IF1GTLO2 = true;


            IsOutputMixerFrequencyTypeStartStop = false;
            IsOutputMixerFrequencyTypeCenterSpan = true;
            IsOutputMixerFrequencyTypeFixed = false;

            OutputSidebandType = SidebandTypeEnum.Low;
            OutputMixerFrequencyType = MixerFrequencyTypeEnum.CenterSpan;
            OutputMixerFrequencyStart = 10.5e6;
            OutputMixerFrequencyStop = 66.9995e9;
            OutputMixerFrequencyCenter = 33.505e9;
            OutputMixerFrequencySpan = 66.989e9;
            OutputMixerFrequencyFixed = 10e6;

        }

        public override void Run()
        {
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
