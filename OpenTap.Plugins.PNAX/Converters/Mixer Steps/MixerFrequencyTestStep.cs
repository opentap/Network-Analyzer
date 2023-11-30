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
    public enum MixerFrequencyTypeEnum
    {
        [Scpi("SWEPT")]
        StartStop,
        [Scpi("SWEPT")]
        CenterSpan,
        [Scpi("FIXED")]
        Fixed
    }

    public enum SidebandTypeEnum
    {
        High,
        Low
    }

    [AllowAsChildIn(typeof(GainCompressionChannel))]
    [AllowAsChildIn(typeof(SweptIMDChannel))]
    [AllowAsChildIn(typeof(NoiseFigureChannel))]
    [AllowAsChildIn(typeof(ScalarMixerChannel))]
    [Display("Mixer Frequency", Groups: new[] { "PNA-X", "Converters" }, Description: "Insert a description here", Order: 3)]
    public class MixerFrequencyTestStep : PNABaseStep
    {
        #region Settings

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

        [Browsable(true)]
        [Display("Calc Input", Groups: new[] { "Mixer Frequency", "Input" }, Order: 16)]
        public void CalcInput()
        {
            if (PNAX.IsConnected)
            {
                Log.Info("Disconnect before using CALC!");
                return;
            }
            CalcInputValues();
        }

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

        [Browsable(true)]
        [Display("Calc LO", Groups: new[] { "Mixer Frequency", "LO1" }, Order: 27)]
        public void CalcLO1()
        {
            if (PNAX.IsConnected)
            {
                Log.Info("Disconnect before using CALC!");
                return;
            }
            CalcLO1Values();
        }
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

        [Browsable(true)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("Calc LO2", Groups: new[] { "Mixer Frequency", "LO2" }, Order: 47)]
        public void CalcLO2()
        {
            if (PNAX.IsConnected)
            {
                Log.Info("Disconnect before using CALC!");
                return;
            }
            CalcLO2Values();
        }
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

        [Browsable(true)]
        [Display("Calc Output", Groups: new[] { "Mixer Frequency", "Output" }, Order: 56)]
        public void CalcOutput()
        {
            if (PNAX.IsConnected)
            {
                Log.Info("Disconnect before using CALC!");
                return;
            }
            CalcOutputValues();
        }

        #endregion



        #endregion


        private void CalcInputValues()
        {
            int DummyChannel = 234;
            try
            {
                PNAX.Open();
                Log.Info("Calculating Input values");

                // Create Dummy channel
                //PNAX.MixerDiscard(DummyChannel);
                int traceid = PNAX.GetNewTraceID(DummyChannel);
                // Define a dummy measurement so we can setup all channel parameters
                // we will add the traces during the StandardSingleTrace or StandardNewTrace test steps
                PNAX.ScpiCommand($"CALCulate{DummyChannel.ToString()}:CUST:DEFine \'CH{DummyChannel.ToString()}_DUMMY_SC21_1\',\'Gain Compression Converters\',\'SC21\'");

                // Set requirements
                PNAX.SetConverterStages(DummyChannel, ConverterStages);
                //SetInput();
                SetLO1(DummyChannel);
                SetIF(DummyChannel);
                SetLO2(DummyChannel);
                SetOutput(DummyChannel);

                PNAX.MixerCalc(DummyChannel, "INP");
                PNAX.WaitForOperationComplete();

                // Read Input and Update settings
                string inpMode = PNAX.GetMixerFrequencyInputMode(DummyChannel);

                if (inpMode.Equals("SWEPT"))
                {
                    InputMixerFrequencyType = MixerFrequencyTypeEnum.StartStop;
                    double ReadStart = PNAX.GetFrequencyLOStart(DummyChannel, 1);
                    double ReadStop = PNAX.GetFrequencyLOStop(DummyChannel, 1);
                    InputMixerFrequencyStart = ReadStart;
                    InputMixerFrequencyStop = ReadStop;
                }
                else if (inpMode.Equals("FIXED"))
                {
                    InputMixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
                    double ReadInputMixerFrequencyFixed = PNAX.GetFrequencyInputFixed(DummyChannel);
                    InputMixerFrequencyFixed = ReadInputMixerFrequencyFixed;
                }

                // Delete Dummy Channel
                PNAX.ScpiCommand($"CALCulate{DummyChannel}:PARameter:DELete \'CH{DummyChannel}_DUMMY_SC21_1\'");

                PNAX.Close();
            }
            catch (Exception)
            {
                if (PNAX.IsConnected)
                {
                    PNAX.ScpiCommand($"CALCulate{DummyChannel}:PARameter:DELete \'CH{DummyChannel}_DUMMY_SC21_1\'");
                    PNAX.Close();
                }
                Log.Error("Cannot calcluate Input values!");
                return;
            }

        }

        private void CalcLO1Values()
        {
            int DummyChannel = 234;
            try
            {
                PNAX.Open();
                Log.Info("Calculating LO1 values");

                // Create Dummy channel
                //PNAX.MixerDiscard(DummyChannel);
                int traceid = PNAX.GetNewTraceID(DummyChannel);
                // Define a dummy measurement so we can setup all channel parameters
                // we will add the traces during the StandardSingleTrace or StandardNewTrace test steps
                PNAX.ScpiCommand($"CALCulate{DummyChannel.ToString()}:CUST:DEFine \'CH{DummyChannel.ToString()}_DUMMY_SC21_1\',\'Gain Compression Converters\',\'SC21\'");

                // Set requirements
                PNAX.SetConverterStages(DummyChannel, ConverterStages);
                SetInput(DummyChannel);
                //SetLO1();
                SetIF(DummyChannel);
                SetLO2(DummyChannel);
                SetOutput(DummyChannel);

                //PNAX.MixerApply(DummyChannel);
                PNAX.MixerCalc(DummyChannel, "LO_1");
                PNAX.WaitForOperationComplete();

                // Read LO1 and Update settings
                String inpMode = PNAX.GetMixerFrequencyLOMode(DummyChannel, 1);

                if (inpMode.Equals("SWEPT"))
                {
                    LO1MixerFrequencyType = MixerFrequencyTypeEnum.StartStop;
                    double ReadStart = PNAX.GetFrequencyLOStart(DummyChannel, 1);
                    double ReadStop = PNAX.GetFrequencyLOStop(DummyChannel, 1);
                    LO1MixerFrequencyStart = ReadStart;
                    LO1MixerFrequencyStop = ReadStop;
                }
                else if (inpMode.Equals("FIXED"))
                {
                    LO1MixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
                    double ReadLO1MixerFrequencyFixed = PNAX.GetFrequencyLOFixed(DummyChannel, 1);
                    LO1MixerFrequencyFixed = ReadLO1MixerFrequencyFixed;
                }

                bool ReadInputGTLO1 = PNAX.GetLOILTI(DummyChannel, 1);
                InputGTLO1 = ReadInputGTLO1;

                // Delete Dummy Channel
                PNAX.ScpiCommand($"CALCulate{DummyChannel}:PARameter:DELete \'CH{DummyChannel}_DUMMY_SC21_1\'");

                PNAX.Close();
            }
            catch (Exception)
            {
                if (PNAX.IsConnected)
                {
                    PNAX.ScpiCommand($"CALCulate{DummyChannel}:PARameter:DELete \'CH{DummyChannel}_DUMMY_SC21_1\'");
                    PNAX.Close();
                }
                Log.Error("Cannot calcluate LO1 values!");
                return;
            }

        }

        private void CalcLO2Values()
        {
            int DummyChannel = 234;
            try
            {
                PNAX.Open();
                Log.Info("Calculating LO2 values");

                // Create Dummy channel
                //PNAX.MixerDiscard(DummyChannel);
                int traceid = PNAX.GetNewTraceID(DummyChannel);
                // Define a dummy measurement so we can setup all channel parameters
                // we will add the traces during the StandardSingleTrace or StandardNewTrace test steps
                PNAX.ScpiCommand($"CALCulate{DummyChannel.ToString()}:CUST:DEFine \'CH{DummyChannel.ToString()}_DUMMY_SC21_1\',\'Gain Compression Converters\',\'SC21\'");

                // Set requirements
                PNAX.SetConverterStages(DummyChannel, ConverterStages);
                SetInput(DummyChannel);
                SetLO1(DummyChannel);
                SetIF(DummyChannel);
                //SetLO2(DummyChannel);
                SetOutput(DummyChannel);

                PNAX.MixerCalc(DummyChannel, "LO_2");
                PNAX.WaitForOperationComplete();

                // Read LO1 and Update settings
                string inpMode = PNAX.GetMixerFrequencyLOMode(DummyChannel, 2);

                if (inpMode.Equals("SWEPT"))
                {
                    LO2MixerFrequencyType = MixerFrequencyTypeEnum.StartStop;
                    double ReadStart = PNAX.GetFrequencyLOStart(DummyChannel, 2);
                    double ReadStop = PNAX.GetFrequencyLOStop(DummyChannel, 2);
                    LO2MixerFrequencyStart = ReadStart;
                    LO2MixerFrequencyStop = ReadStop;
                }
                else if (inpMode.Equals("FIXED"))
                {
                    LO2MixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
                    double ReadLO2MixerFrequencyFixed = PNAX.GetFrequencyLOFixed(DummyChannel, 2);
                    LO2MixerFrequencyFixed = ReadLO2MixerFrequencyFixed;
                }

                bool ReadInputGTLO2 = PNAX.GetLOILTI(DummyChannel, 2);
                IF1GTLO2 = ReadInputGTLO2;

                // Delete Dummy Channel
                PNAX.ScpiCommand($"CALCulate{DummyChannel}:PARameter:DELete \'CH{DummyChannel}_DUMMY_SC21_1\'");

                PNAX.Close();
            }
            catch (Exception)
            {
                if (PNAX.IsConnected)
                {
                    PNAX.ScpiCommand($"CALCulate{DummyChannel}:PARameter:DELete \'CH{DummyChannel}_DUMMY_SC21_1\'");
                    PNAX.Close();
                }
                Log.Error("Cannot calcluate LO2 values!");
                return;
            }

        }

        private void CalcOutputValues()
        {
            int DummyChannel = 234;
            try
            {
                PNAX.Open();
                Log.Info("Calculating Input values");

                // Create Dummy channel
                //PNAX.MixerDiscard(DummyChannel);
                int traceid = PNAX.GetNewTraceID(DummyChannel);
                // Define a dummy measurement so we can setup all channel parameters
                // we will add the traces during the StandardSingleTrace or StandardNewTrace test steps
                PNAX.ScpiCommand($"CALCulate{DummyChannel.ToString()}:CUST:DEFine \'CH{DummyChannel.ToString()}_DUMMY_SC21_1\',\'Gain Compression Converters\',\'SC21\'");

                // Set requirements
                PNAX.SetConverterStages(DummyChannel, ConverterStages);
                SetInput(DummyChannel);
                SetLO1(DummyChannel);
                SetIF(DummyChannel);
                SetLO2(DummyChannel);
                //SetOutput(DummyChannel);

                PNAX.MixerCalc(DummyChannel, "OUTP");
                PNAX.WaitForOperationComplete();

                // Read output and Update settings
                String inpMode = PNAX.GetMixerFrequencyOutputMode(DummyChannel);

                if (inpMode.Equals("SWEPT"))
                {
                    double ReadStart = PNAX.GetFrequencyOutputStart(DummyChannel);
                    double ReadStop = PNAX.GetFrequencyOutputStop(DummyChannel);
                    OutputMixerFrequencyType = MixerFrequencyTypeEnum.StartStop;
                    OutputMixerFrequencyStart = ReadStart;
                    OutputMixerFrequencyStop = ReadStop;
                }
                else if (inpMode.Equals("FIXED"))
                {
                    OutputMixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
                    double ReadOutputMixerFrequencyFixed = PNAX.GetFrequencyOutputFixed(DummyChannel);
                    OutputMixerFrequencyFixed = ReadOutputMixerFrequencyFixed;
                }

                SidebandTypeEnum ReadOutputSidebandType = PNAX.GetFrequencyOutputSideband(DummyChannel);
                OutputSidebandType = ReadOutputSidebandType;

                // Delete Dummy Channel
                PNAX.ScpiCommand($"CALCulate{DummyChannel}:PARameter:DELete \'CH{DummyChannel}_DUMMY_SC21_1\'");

                PNAX.Close();
            }
            catch (Exception)
            {
                if (PNAX.IsConnected)
                {
                    PNAX.ScpiCommand($"CALCulate{DummyChannel}:PARameter:DELete \'CH{DummyChannel}_DUMMY_SC21_1\'");
                    PNAX.Close();
                }
                Log.Error("Cannot calcluate Input values!");
                return;
            }

        }

        public MixerFrequencyTestStep()
        {
            IsConverter = true;
            UpdateDefaultValues();
        }

        private void UpdateDefaultValues()
        {
            var defaultValues = PNAX.GetMixerFrequencyDefaultValues();
            if (defaultValues == null)
                return;

            InputMixerFrequencyType = defaultValues.InputMixerFrequencyType;
            InputMixerFrequencyStart = defaultValues.InputMixerFrequencyStart;
            InputMixerFrequencyStop = defaultValues.InputMixerFrequencyStop;
            InputMixerFrequencyCenter = defaultValues.InputMixerFrequencyCenter;
            InputMixerFrequencySpan = defaultValues.InputMixerFrequencySpan;
            InputMixerFrequencyFixed = defaultValues.InputMixerFrequencyFixed;

            LO1MixerFrequencyType = defaultValues.LO1MixerFrequencyType;
            LO1MixerFrequencyStart = defaultValues.LO1MixerFrequencyStart;
            LO1MixerFrequencyStop = defaultValues.LO1MixerFrequencyStop;
            LO1MixerFrequencyCenter = defaultValues.LO1MixerFrequencyCenter;
            LO1MixerFrequencySpan = defaultValues.LO1MixerFrequencySpan;
            LO1MixerFrequencyFixed = defaultValues.LO1MixerFrequencyFixed;
            InputGTLO1 = defaultValues.InputGTLO1;

            IFSidebandType = defaultValues.IFSidebandType;
            IFMixerFrequencyType = defaultValues.IFMixerFrequencyType;
            IFMixerFrequencyStart = defaultValues.IFMixerFrequencyStart;
            IFMixerFrequencyStop = defaultValues.IFMixerFrequencyStop;
            IFMixerFrequencyCenter = defaultValues.IFMixerFrequencyCenter;
            IFMixerFrequencySpan = defaultValues.IFMixerFrequencySpan;
            IFMixerFrequencyFixed = defaultValues.IFMixerFrequencyFixed;

            LO2MixerFrequencyType = defaultValues.LO2MixerFrequencyType;
            LO2MixerFrequencyStart = defaultValues.LO2MixerFrequencyStart;
            LO2MixerFrequencyStop = defaultValues.LO2MixerFrequencyStop;
            LO2MixerFrequencyCenter = defaultValues.LO2MixerFrequencyCenter;
            LO2MixerFrequencySpan = defaultValues.LO2MixerFrequencySpan;
            LO2MixerFrequencyFixed = defaultValues.LO2MixerFrequencyFixed;
            IF1GTLO2 = defaultValues.IF1GTLO2;

            OutputSidebandType = defaultValues.OutputSidebandType;
            OutputMixerFrequencyType = defaultValues.OutputMixerFrequencyType;
            OutputMixerFrequencyStart = defaultValues.OutputMixerFrequencyStart;
            OutputMixerFrequencyStop = defaultValues.OutputMixerFrequencyStop;
            OutputMixerFrequencyCenter = defaultValues.OutputMixerFrequencyCenter;
            OutputMixerFrequencySpan = defaultValues.OutputMixerFrequencySpan;
            OutputMixerFrequencyFixed = defaultValues.OutputMixerFrequencyFixed;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            // Initialize MetaData list
            retVal = new List<(string, object)>();

            // Start from scratch
            PNAX.MixerDiscard(Channel);

            PNAX.SetConverterStages(Channel, ConverterStages);

            SetInput(Channel);
            SetLO1(Channel);
            SetIF(Channel);
            SetLO2(Channel);
            SetOutput(Channel);

            // Apply changes to instrument
            PNAX.MixerCalc(Channel);
            PNAX.MixerApply(Channel);

            // Now read back and validate the values were not changed by the Calculate command
            #region Input
            if (InputMixerFrequencyType == MixerFrequencyTypeEnum.StartStop)
            {
                PNAX.ValidateMixerFrequencyInputMode(Channel, "SWEPT");
                PNAX.ValidateFrequencyInputStart(Channel, InputMixerFrequencyStart);
                PNAX.ValidateFrequencyInputStop(Channel, InputMixerFrequencyStop);
            }
            else if (InputMixerFrequencyType == MixerFrequencyTypeEnum.CenterSpan)
            {
                // Calculate Start/Stop from Center/Span
                double start = InputMixerFrequencyCenter - (InputMixerFrequencySpan / 2);
                double stop = InputMixerFrequencyCenter + (InputMixerFrequencySpan / 2);

                PNAX.ValidateMixerFrequencyInputMode(Channel, "SWEPT");
                PNAX.ValidateFrequencyInputStart(Channel, start);
                PNAX.ValidateFrequencyInputStop(Channel, stop);
            }
            else
            {
                // Fixed
                PNAX.ValidateMixerFrequencyInputMode(Channel, "FIXED");
                PNAX.ValidateFrequencyInputFixed(Channel, InputMixerFrequencyFixed);
            }
            #endregion

            #region LO1
            if (LO1MixerFrequencyType == MixerFrequencyTypeEnum.StartStop)
            {
                PNAX.ValidateMixerFrequencyLOMode(Channel, 1, "SWEPT");
                PNAX.ValidateFrequencyLOStart(Channel, 1, LO1MixerFrequencyStart);
                PNAX.ValidateFrequencyLOStop(Channel, 1, LO1MixerFrequencyStop);
            }
            else if (LO1MixerFrequencyType == MixerFrequencyTypeEnum.CenterSpan)
            {
                // Calculate Start/Stop from Center/Span
                double start = LO1MixerFrequencyCenter - (LO1MixerFrequencySpan / 2);
                double stop = LO1MixerFrequencyCenter + (LO1MixerFrequencySpan / 2);
                PNAX.ValidateMixerFrequencyLOMode(Channel, 1, "SWEPT");
                PNAX.ValidateFrequencyLOStart(Channel, 1, start);
                PNAX.ValidateFrequencyLOStop(Channel, 1, stop);
            }
            else
            {
                // Fixed
                PNAX.ValidateMixerFrequencyLOMode(Channel, 1, "FIXED");
                PNAX.ValidateFrequencyLOFixed(Channel, 1, LO1MixerFrequencyFixed);
            }
            PNAX.ValidateLOILTI(Channel, 1, InputGTLO1);
            #endregion

            #region IF
            if (ConverterStages == ConverterStagesEnum._2)
            {
                if (IFMixerFrequencyType == MixerFrequencyTypeEnum.StartStop)
                {
                    PNAX.ValidateFrequencyIFStart(Channel, IFMixerFrequencyStart);
                    PNAX.ValidateFrequencyIFStop(Channel, IFMixerFrequencyStop);
                }
                else if (IFMixerFrequencyType == MixerFrequencyTypeEnum.CenterSpan)
                {
                    // Calculate Start/Stop from Center/Span
                    double start = IFMixerFrequencyCenter - (IFMixerFrequencySpan / 2);
                    double stop = IFMixerFrequencyCenter + (IFMixerFrequencySpan / 2);
                    PNAX.ValidateFrequencyIFStart(Channel, start);
                    PNAX.ValidateFrequencyIFStop(Channel, stop);
                }
                else
                {
                    // Fixed
                    // TODO find command for IF Fixed
                    // PNAX.SetFrequencyIFFixed(Channel, IFMixerFrequencyFixed);
                }
                PNAX.ValidateFrequencyIFSideband(Channel, IFSidebandType);
            }
            #endregion

            #region LO2
            if (ConverterStages == ConverterStagesEnum._2)
            {
                if (LO2MixerFrequencyType == MixerFrequencyTypeEnum.StartStop)
                {
                    PNAX.ValidateMixerFrequencyLOMode(Channel, 1, "SWEPT");
                    PNAX.ValidateFrequencyLOStart(Channel, 2, LO2MixerFrequencyStart);
                    PNAX.ValidateFrequencyLOStop(Channel, 2, LO2MixerFrequencyStop);
                }
                else if (LO2MixerFrequencyType == MixerFrequencyTypeEnum.CenterSpan)
                {
                    // Calculate Start/Stop from Center/Span
                    double start = LO2MixerFrequencyCenter - (LO2MixerFrequencySpan / 2);
                    double stop = LO2MixerFrequencyCenter + (LO2MixerFrequencySpan / 2);
                    PNAX.ValidateMixerFrequencyLOMode(Channel, 1, "SWEPT");
                    PNAX.ValidateFrequencyLOStart(Channel, 2, start);
                    PNAX.ValidateFrequencyLOStop(Channel, 2, stop);
                }
                else
                {
                    // Fixed
                    PNAX.ValidateMixerFrequencyLOMode(Channel, 1, "FIXED");
                    PNAX.ValidateFrequencyLOFixed(Channel, 2, LO2MixerFrequencyFixed);
                }
                PNAX.ValidateLOILTI(Channel, 2, IF1GTLO2);
            }
            #endregion

            #region Output
            if (OutputMixerFrequencyType == MixerFrequencyTypeEnum.StartStop)
            {
                PNAX.ValidateFrequencyOutputStart(Channel, OutputMixerFrequencyStart);
                PNAX.ValidateFrequencyOutputStop(Channel, OutputMixerFrequencyStop);
            }
            else if (OutputMixerFrequencyType == MixerFrequencyTypeEnum.CenterSpan)
            {
                // Calculate Start/Stop from Center/Span
                double start = OutputMixerFrequencyCenter - (OutputMixerFrequencySpan / 2);
                double stop = OutputMixerFrequencyCenter + (OutputMixerFrequencySpan / 2);
                PNAX.ValidateFrequencyOutputStart(Channel, start);
                PNAX.ValidateFrequencyOutputStop(Channel, stop);
            }
            else
            {
                // Fixed
                PNAX.ValidateFrequencyOutputFixed(Channel, OutputMixerFrequencyFixed);
            }
            PNAX.ValidateFrequencyOutputSideband(Channel, OutputSidebandType);
            #endregion

            UpgradeVerdict(Verdict.Pass);
        }

        private void SetInput(int Channel)
        {
            #region Input
            if (InputMixerFrequencyType == MixerFrequencyTypeEnum.StartStop)
            {
                PNAX.SetMixerFrequencyInputMode(Channel, MixerFrequencyTypeEnum.StartStop);
                PNAX.SetFrequencyInputStart(Channel, InputMixerFrequencyStart);
                PNAX.SetFrequencyInputStop(Channel, InputMixerFrequencyStop);

                retVal.Add(("Mixer Frequency Input Mode", MixerFrequencyTypeEnum.StartStop));
                retVal.Add(("Mixer Frequency Input Start", InputMixerFrequencyStart));
                retVal.Add(("Mixer Frequency Input Stop", InputMixerFrequencyStop));
            }
            else if (InputMixerFrequencyType == MixerFrequencyTypeEnum.CenterSpan)
            {
                // Calculate Start/Stop from Center/Span
                double start = InputMixerFrequencyCenter - (InputMixerFrequencySpan / 2);
                double stop = InputMixerFrequencyCenter + (InputMixerFrequencySpan / 2);
                PNAX.SetMixerFrequencyInputMode(Channel, MixerFrequencyTypeEnum.CenterSpan);
                PNAX.SetFrequencyInputStart(Channel, start);
                PNAX.SetFrequencyInputStop(Channel, stop);

                retVal.Add(("Mixer Frequency Input Mode", MixerFrequencyTypeEnum.CenterSpan));
                retVal.Add(("Mixer Frequency Input Center", InputMixerFrequencyCenter));
                retVal.Add(("Mixer Frequency Input Span", InputMixerFrequencySpan));
            }
            else
            {
                // Fixed
                PNAX.SetMixerFrequencyInputMode(Channel, MixerFrequencyTypeEnum.Fixed);
                PNAX.SetFrequencyInputFixed(Channel, InputMixerFrequencyFixed);

                retVal.Add(("Mixer Frequency Input Mode", MixerFrequencyTypeEnum.Fixed));
                retVal.Add(("Mixer Frequency Input Fixed", InputMixerFrequencyFixed));
            }
            #endregion
        }

        private void SetLO1(int Channel)
        {
            #region LO1
            if (LO1MixerFrequencyType == MixerFrequencyTypeEnum.StartStop)
            {
                PNAX.SetMixerFrequencyLOMode(Channel, 1, MixerFrequencyTypeEnum.StartStop);
                PNAX.SetFrequencyLOStart(Channel, 1, LO1MixerFrequencyStart);
                PNAX.SetFrequencyLOStop(Channel, 1, LO1MixerFrequencyStop);

                retVal.Add(("Mixer Frequency LO1 Mode", MixerFrequencyTypeEnum.StartStop));
                retVal.Add(("Mixer Frequency LO1 Start", LO1MixerFrequencyStart));
                retVal.Add(("Mixer Frequency LO1 Stop", LO1MixerFrequencyStop));
            }
            else if (LO1MixerFrequencyType == MixerFrequencyTypeEnum.CenterSpan)
            {
                // Calculate Start/Stop from Center/Span
                double start = LO1MixerFrequencyCenter - (LO1MixerFrequencySpan / 2);
                double stop = LO1MixerFrequencyCenter + (LO1MixerFrequencySpan / 2);
                PNAX.SetMixerFrequencyLOMode(Channel, 1, MixerFrequencyTypeEnum.CenterSpan);
                PNAX.SetFrequencyLOStart(Channel, 1, start);
                PNAX.SetFrequencyLOStop(Channel, 1, stop);

                retVal.Add(("Mixer Frequency LO1 Mode", MixerFrequencyTypeEnum.CenterSpan));
                retVal.Add(("Mixer Frequency LO1 Center", LO1MixerFrequencyCenter));
                retVal.Add(("Mixer Frequency LO1 Span", LO1MixerFrequencySpan));
            }
            else
            {
                // Fixed
                PNAX.SetMixerFrequencyLOMode(Channel, 1, MixerFrequencyTypeEnum.Fixed);
                PNAX.SetFrequencyLOFixed(Channel, 1, LO1MixerFrequencyFixed);

                retVal.Add(("Mixer Frequency LO1 Mode", MixerFrequencyTypeEnum.Fixed));
                retVal.Add(("Mixer Frequency LO1 Fixed", LO1MixerFrequencyFixed));
            }
            PNAX.SetLOILTI(Channel, 1, InputGTLO1);
            retVal.Add(("Input Greater Than LO", InputGTLO1));
            #endregion
        }

        private void SetIF(int Channel)
        {
            #region IF
            if (ConverterStages == ConverterStagesEnum._2)
            {
                if (IFMixerFrequencyType == MixerFrequencyTypeEnum.StartStop)
                {
                    PNAX.SetFrequencyIFStart(Channel, IFMixerFrequencyStart);
                    PNAX.SetFrequencyIFStop(Channel, IFMixerFrequencyStop);

                    retVal.Add(("Mixer Frequency IF Start", IFMixerFrequencyStart));
                    retVal.Add(("Mixer Frequency IF Stop", IFMixerFrequencyStop));
                }
                else if (IFMixerFrequencyType == MixerFrequencyTypeEnum.CenterSpan)
                {
                    // Calculate Start/Stop from Center/Span
                    double start = IFMixerFrequencyCenter - (IFMixerFrequencySpan / 2);
                    double stop = IFMixerFrequencyCenter + (IFMixerFrequencySpan / 2);
                    PNAX.SetFrequencyIFStart(Channel, start);
                    PNAX.SetFrequencyIFStop(Channel, stop);

                    retVal.Add(("Mixer Frequency IF Center", IFMixerFrequencyCenter));
                    retVal.Add(("Mixer Frequency IF Span", IFMixerFrequencySpan));
                }
                else
                {
                    // Fixed
                    // TODO find command for IF Fixed
                    // PNAX.SetFrequencyIFFixed(Channel, IFMixerFrequencyFixed);
                }
                PNAX.SetFrequencyIFSideband(Channel, IFSidebandType);
                retVal.Add(("Mixer Frequency IF Sideband", IFSidebandType));
            }
            #endregion
        }

        private void SetLO2(int Channel)
        {
            #region LO2
            if (ConverterStages == ConverterStagesEnum._2)
            {
                if (LO2MixerFrequencyType == MixerFrequencyTypeEnum.StartStop)
                {
                    PNAX.SetMixerFrequencyLOMode(Channel, 1, MixerFrequencyTypeEnum.StartStop);
                    PNAX.SetFrequencyLOStart(Channel, 2, LO2MixerFrequencyStart);
                    PNAX.SetFrequencyLOStop(Channel, 2, LO2MixerFrequencyStop);

                    retVal.Add(("Mixer Frequency LO2 Mode", MixerFrequencyTypeEnum.StartStop));
                    retVal.Add(("Mixer Frequency LO2 Start", LO2MixerFrequencyStart));
                    retVal.Add(("Mixer Frequency LO2 Stop", LO2MixerFrequencyStop));
                }
                else if (LO2MixerFrequencyType == MixerFrequencyTypeEnum.CenterSpan)
                {
                    // Calculate Start/Stop from Center/Span
                    double start = LO2MixerFrequencyCenter - (LO2MixerFrequencySpan / 2);
                    double stop = LO2MixerFrequencyCenter + (LO2MixerFrequencySpan / 2);
                    PNAX.SetMixerFrequencyLOMode(Channel, 1, MixerFrequencyTypeEnum.CenterSpan);
                    PNAX.SetFrequencyLOStart(Channel, 2, start);
                    PNAX.SetFrequencyLOStop(Channel, 2, stop);

                    retVal.Add(("Mixer Frequency LO2 Mode", MixerFrequencyTypeEnum.CenterSpan));
                    retVal.Add(("Mixer Frequency LO2 Center", LO2MixerFrequencyCenter));
                    retVal.Add(("Mixer Frequency LO2 Span", LO2MixerFrequencySpan));
                }
                else
                {
                    // Fixed
                    PNAX.SetMixerFrequencyLOMode(Channel, 1, MixerFrequencyTypeEnum.Fixed);
                    PNAX.SetFrequencyLOFixed(Channel, 2, LO2MixerFrequencyFixed);

                    retVal.Add(("Mixer Frequency LO2 Mode", MixerFrequencyTypeEnum.Fixed));
                    retVal.Add(("Mixer Frequency LO2 Fixed", LO2MixerFrequencyFixed));
                }
                PNAX.SetLOILTI(Channel, 2, IF1GTLO2);
                retVal.Add(("IF1 Greater Than LO2", IF1GTLO2));
            }
            #endregion
        }

        private void SetOutput(int Channel)
        {
            #region Output
            PNAX.SetMixerFrequencyOutputMode(Channel, OutputMixerFrequencyType);
            retVal.Add(("Mixer Frequency LO1 Mode", MixerFrequencyTypeEnum.Fixed));
            if (OutputMixerFrequencyType == MixerFrequencyTypeEnum.StartStop)
            {
                PNAX.SetFrequencyOutputStart(Channel, OutputMixerFrequencyStart);
                PNAX.SetFrequencyOutputStop(Channel, OutputMixerFrequencyStop);

                retVal.Add(("Mixer Frequency Output Start", OutputMixerFrequencyStart));
                retVal.Add(("Mixer Frequency Output Stop", OutputMixerFrequencyStop));
            }
            else if (OutputMixerFrequencyType == MixerFrequencyTypeEnum.CenterSpan)
            {
                // Calculate Start/Stop from Center/Span
                double start = OutputMixerFrequencyCenter - (OutputMixerFrequencySpan / 2);
                double stop = OutputMixerFrequencyCenter + (OutputMixerFrequencySpan / 2);
                PNAX.SetFrequencyOutputStart(Channel, start);
                PNAX.SetFrequencyOutputStop(Channel, stop);

                retVal.Add(("Mixer Frequency Output Center", OutputMixerFrequencyCenter));
                retVal.Add(("Mixer Frequency Output Span", OutputMixerFrequencySpan));
            }
            else
            {
                // Fixed
                PNAX.SetFrequencyOutputFixed(Channel, OutputMixerFrequencyFixed);

                retVal.Add(("Mixer Frequency Output Fixed", OutputMixerFrequencyFixed));
            }
            PNAX.SetFrequencyOutputSideband(Channel, OutputSidebandType);
            retVal.Add(("Mixer Frequency Output Sideband", OutputSidebandType));
            #endregion
        }

        private List<(string, object)> retVal = new List<(string, object)>();

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            return retVal;
        }

    }
}
