// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using OpenTap;
using System;
using System.Collections;
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

    //[AllowAsChildIn(typeof(GainCompressionChannel))]
    //[AllowAsChildIn(typeof(SweptIMDChannel))]
    //[AllowAsChildIn(typeof(NoiseFigureChannel))]
    //[AllowAsChildIn(typeof(ScalarMixerChannel))]
    [Display("Mixer Frequency", Groups: new[] { "Network Analyzer", "Converters" }, Description: "Insert a description here", Order: 3)]
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

        [Display("Fractional Multiplier Numerator", Groups: new[] { "Mixer Frequency", "Input" }, Order: 16)]
        public int InputFractionalMultiplierNumerator { get; set; }
        [Display("Fractional Multiplier Denominator", Groups: new[] { "Mixer Frequency", "Input" }, Order: 17)]
        public int InputFractionalMultiplierDenominator { get; set; }

        [Browsable(true)]
        [Display("Calc Input", Groups: new[] { "Mixer Frequency", "Input" }, Order: 18)]
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
                IsLO1MixerFrequencyTypeStartStop = _LO1MixerFrequencyType == MixerFrequencyTypeEnum.StartStop;
                IsLO1MixerFrequencyTypeCenterSpan = _LO1MixerFrequencyType == MixerFrequencyTypeEnum.CenterSpan;
                IsLO1MixerFrequencyTypeFixed = _LO1MixerFrequencyType == MixerFrequencyTypeEnum.Fixed;
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

        [Display("Fractional Multiplier Numerator", Groups: new[] { "Mixer Frequency", "LO1" }, Order: 27)]
        public int LO1FractionalMultiplierNumerator { get; set; }
        [Display("Fractional Multiplier Denominator", Groups: new[] { "Mixer Frequency", "LO1" }, Order: 28)]
        public int LO1FractionalMultiplierDenominator { get; set; }

        [Browsable(true)]
        [Display("Calc LO", Groups: new[] { "Mixer Frequency", "LO1" }, Order: 29)]
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
                IsIFMixerFrequencyTypeStartStop = _IFMixerFrequencyType == MixerFrequencyTypeEnum.StartStop;
                IsIFMixerFrequencyTypeCenterSpan = _IFMixerFrequencyType == MixerFrequencyTypeEnum.CenterSpan;
                IsIFMixerFrequencyTypeFixed = _IFMixerFrequencyType == MixerFrequencyTypeEnum.Fixed;
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
                IsLO2MixerFrequencyTypeStartStop = _LO2MixerFrequencyType == MixerFrequencyTypeEnum.StartStop;
                IsLO2MixerFrequencyTypeCenterSpan = _LO2MixerFrequencyType == MixerFrequencyTypeEnum.CenterSpan;
                IsLO2MixerFrequencyTypeFixed = _LO2MixerFrequencyType == MixerFrequencyTypeEnum.Fixed;
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


        [Display("Fractional Multiplier Numerator", Groups: new[] { "Mixer Frequency", "LO2" }, Order: 47)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        public int LO2FractionalMultiplierNumerator { get; set; }
        [Display("Fractional Multiplier Denominator", Groups: new[] { "Mixer Frequency", "LO2" }, Order: 48)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        public int LO2FractionalMultiplierDenominator { get; set; }

        [Browsable(true)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("Calc LO2", Groups: new[] { "Mixer Frequency", "LO2" }, Order: 49)]
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
                IsOutputMixerFrequencyTypeStartStop = _OutputMixerFrequencyType == MixerFrequencyTypeEnum.StartStop;
                IsOutputMixerFrequencyTypeCenterSpan = _OutputMixerFrequencyType == MixerFrequencyTypeEnum.CenterSpan;
                IsOutputMixerFrequencyTypeFixed = _OutputMixerFrequencyType == MixerFrequencyTypeEnum.Fixed;
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

        /// <summary>
        /// Defines the target for the mixer calculation.
        /// </summary>
        private enum CalculationTarget { Input, LO1, LO2, Output }

        private void CalcInputValues() => CalculateValues(CalculationTarget.Input);
        private void CalcLO1Values() => CalculateValues(CalculationTarget.LO1);
        private void CalcLO2Values() => CalculateValues(CalculationTarget.LO2);
        private void CalcOutputValues() => CalculateValues(CalculationTarget.Output);

        /// <summary>
        /// A centralized method to perform mixer calculations using a temporary PNA channel.
        /// This avoids code duplication across the different Calc... methods.
        /// </summary>
        /// <param name="target">The specific part of the mixer to calculate.</param>
        private void CalculateValues(CalculationTarget target)
        {
            const int DummyChannel = 234;
            string dummyTraceName = $"CH{DummyChannel}_DUMMY_SC21_1";
            string logName = target.ToString();

            try
            {
                PNAX.Open();
                Log.Info($"Calculating {logName} values");

                // Create Dummy channel and measurement
                int traceid = PNAX.GetNewTraceID(DummyChannel);
                PNAX.ScpiCommand($"CALCulate{DummyChannel}:CUST:DEFine '{dummyTraceName}','Gain Compression Converters','SC21'");

                // Set all known parameters except for the one being calculated
                PNAX.SetConverterStages(DummyChannel, ConverterStages);
                PNAX.SetFrequencyOutputSideband(DummyChannel, OutputSidebandType);
                SetMultiplier(DummyChannel);
                SetIF(DummyChannel);

                if (target != CalculationTarget.Input) SetInput(DummyChannel);
                if (target != CalculationTarget.LO1) SetLO1(DummyChannel);
                if (target != CalculationTarget.LO2) SetLO2(DummyChannel);
                if (target != CalculationTarget.Output) SetOutput(DummyChannel);

                // Execute the calculation and read back the results
                switch (target)
                {
                    case CalculationTarget.Input:
                        PNAX.MixerCalc(DummyChannel, "INP");
                        PNAX.WaitForOperationComplete();
                        string inpMode = PNAX.GetMixerFrequencyInputMode(DummyChannel);
                        if (inpMode.Equals("SWEPT"))
                        {
                            InputMixerFrequencyType = MixerFrequencyTypeEnum.StartStop;
                            InputMixerFrequencyStart = PNAX.GetFrequencyLOStart(DummyChannel, 1);
                            InputMixerFrequencyStop = PNAX.GetFrequencyLOStop(DummyChannel, 1);
                        }
                        else if (inpMode.Equals("FIXED"))
                        {
                            InputMixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
                            InputMixerFrequencyFixed = PNAX.GetFrequencyInputFixed(DummyChannel);
                        }
                        break;
                    case CalculationTarget.LO1:
                        PNAX.MixerCalc(DummyChannel, "LO_1");
                        PNAX.WaitForOperationComplete();
                        string lo1Mode = PNAX.GetMixerFrequencyLOMode(DummyChannel, 1);
                        if (lo1Mode.Equals("SWEPT"))
                        {
                            LO1MixerFrequencyType = MixerFrequencyTypeEnum.StartStop;
                            LO1MixerFrequencyStart = PNAX.GetFrequencyLOStart(DummyChannel, 1);
                            LO1MixerFrequencyStop = PNAX.GetFrequencyLOStop(DummyChannel, 1);
                        }
                        else if (lo1Mode.Equals("FIXED"))
                        {
                            LO1MixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
                            LO1MixerFrequencyFixed = PNAX.GetFrequencyLOFixed(DummyChannel, 1);
                        }
                        InputGTLO1 = PNAX.GetLOILTI(DummyChannel, 1);
                        break;
                    case CalculationTarget.LO2:
                        PNAX.MixerCalc(DummyChannel, "LO_2");
                        PNAX.WaitForOperationComplete();
                        string lo2Mode = PNAX.GetMixerFrequencyLOMode(DummyChannel, 2);
                        if (lo2Mode.Equals("SWEPT"))
                        {
                            LO2MixerFrequencyType = MixerFrequencyTypeEnum.StartStop;
                            LO2MixerFrequencyStart = PNAX.GetFrequencyLOStart(DummyChannel, 2);
                            LO2MixerFrequencyStop = PNAX.GetFrequencyLOStop(DummyChannel, 2);
                        }
                        else if (lo2Mode.Equals("FIXED"))
                        {
                            LO2MixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
                            LO2MixerFrequencyFixed = PNAX.GetFrequencyLOFixed(DummyChannel, 2);
                        }
                        IF1GTLO2 = PNAX.GetLOILTI(DummyChannel, 2);
                        break;
                    case CalculationTarget.Output:
                        PNAX.MixerCalc(DummyChannel, "OUTP");
                        PNAX.WaitForOperationComplete();
                        string outMode = PNAX.GetMixerFrequencyOutputMode(DummyChannel);
                        if (outMode.Equals("SWEPT"))
                        {
                            OutputMixerFrequencyType = MixerFrequencyTypeEnum.StartStop;
                            OutputMixerFrequencyStart = PNAX.GetFrequencyOutputStart(DummyChannel);
                            OutputMixerFrequencyStop = PNAX.GetFrequencyOutputStop(DummyChannel);
                        }
                        else if (outMode.Equals("FIXED"))
                        {
                            OutputMixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
                            OutputMixerFrequencyFixed = PNAX.GetFrequencyOutputFixed(DummyChannel);
                        }
                        OutputSidebandType = PNAX.GetFrequencyOutputSideband(DummyChannel);
                        break;
                }
            }
            catch (Exception)
            {
                Log.Error($"Cannot calculate {logName} values!");
            }
            finally
            {
                if (PNAX.IsConnected)
                {
                    try
                    {
                        // Cleanup dummy channel
                        PNAX.ScpiCommand($"CALCulate{DummyChannel}:PARameter:DELete '{dummyTraceName}'");
                    }
                    catch (Exception ex)
                    {
                        Log.Warning($"Failed to delete dummy channel during cleanup: {ex.Message}");
                    }
                    PNAX.Close();
                }
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
            var defaultSettings = PNAX.GetMixerSetupDefaultValues();
            if (defaultValues == null || defaultSettings == null)
                return;

            InputMixerFrequencyType = defaultValues.InputMixerFrequencyType;
            InputMixerFrequencyStart = defaultValues.InputMixerFrequencyStart;
            InputMixerFrequencyStop = defaultValues.InputMixerFrequencyStop;
            InputMixerFrequencyCenter = defaultValues.InputMixerFrequencyCenter;
            InputMixerFrequencySpan = defaultValues.InputMixerFrequencySpan;
            InputMixerFrequencyFixed = defaultValues.InputMixerFrequencyFixed;
            InputFractionalMultiplierNumerator = defaultSettings.InputFractionalMultiplierNumerator;
            InputFractionalMultiplierDenominator = defaultSettings.InputFractionalMultiplierDenominator;

            LO1MixerFrequencyType = defaultValues.LO1MixerFrequencyType;
            LO1MixerFrequencyStart = defaultValues.LO1MixerFrequencyStart;
            LO1MixerFrequencyStop = defaultValues.LO1MixerFrequencyStop;
            LO1MixerFrequencyCenter = defaultValues.LO1MixerFrequencyCenter;
            LO1MixerFrequencySpan = defaultValues.LO1MixerFrequencySpan;
            LO1MixerFrequencyFixed = defaultValues.LO1MixerFrequencyFixed;
            InputGTLO1 = defaultValues.InputGTLO1;
            LO1FractionalMultiplierNumerator = defaultSettings.LO1FractionalMultiplierNumerator;
            LO1FractionalMultiplierDenominator = defaultSettings.LO1FractionalMultiplierDenominator;

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
            LO2FractionalMultiplierNumerator = defaultSettings.LO2FractionalMultiplierNumerator;
            LO2FractionalMultiplierDenominator = defaultSettings.LO2FractionalMultiplierDenominator;

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
            SetMultiplier(Channel);
            SetLO1(Channel);
            SetIF(Channel);
            SetLO2(Channel);
            SetOutput(Channel);

            // Apply changes to instrument
            PNAX.MixerCalc(Channel);
            PNAX.MixerApply(Channel);

            ValidateAllSettings();
            UpgradeVerdict(Verdict.Pass);
        }

        private void ValidateAllSettings()
        {
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
        }

        private void SetMultiplier(int Channel)
        {
            PNAX.SetInputFractionalMultiplierNumerator(Channel, InputFractionalMultiplierNumerator);
            PNAX.SetInputFractionalMultiplierDenominator(Channel, InputFractionalMultiplierDenominator);
            PNAX.SetLOFractionalMultiplierNumerator(Channel, 1, LO1FractionalMultiplierNumerator);
            PNAX.SetLOFractionalMultiplierDenominator(Channel, 1, LO1FractionalMultiplierDenominator);
            retVal.Add(("Fractional Multiplier Numerator", InputFractionalMultiplierNumerator));
            retVal.Add(("Fractional Multiplier Denominator", InputFractionalMultiplierDenominator));
            retVal.Add(("LO1 Fractional Multiplier Numerator", LO1FractionalMultiplierNumerator));
            retVal.Add(("LO1 Fractional Multiplier Denominator", LO1FractionalMultiplierDenominator));
            if (ConverterStages == ConverterStagesEnum._2)
            {
                PNAX.SetLOFractionalMultiplierNumerator(Channel, 2, LO2FractionalMultiplierNumerator);
                PNAX.SetLOFractionalMultiplierDenominator(Channel, 2, LO2FractionalMultiplierDenominator);
                retVal.Add(("LO1 Fractional Multiplier Numerator", LO2FractionalMultiplierNumerator));
                retVal.Add(("LO1 Fractional Multiplier Denominator", LO2FractionalMultiplierDenominator));
            }

        }

        private (double Start, double Stop) CalculateStartStop(double inputStart, double inputStop, double inputCenter, double inputSpan, MixerFrequencyTypeEnum type)
        {
            if (type == MixerFrequencyTypeEnum.StartStop)
                return (inputStart, inputStop);
            else
                return (inputCenter - (inputSpan / 2.0), inputCenter + (inputSpan / 2.0));
        }

        private void LogStartStop(string prefix, double start, double stop)
        {
            retVal.Add(($"{prefix} Mode", MixerFrequencyTypeEnum.StartStop));
            retVal.Add(($"{prefix} Start", start));
            retVal.Add(($"{prefix} Stop", stop));
        }

        private void LogCenterSpan(string prefix, double center, double span)
        {
            retVal.Add(($"{prefix} Mode", MixerFrequencyTypeEnum.CenterSpan));
            retVal.Add(($"{prefix} Center", center));
            retVal.Add(($"{prefix} Span", span));
        }

        private void LogFixed(string prefix, double @fixed)
        {
            retVal.Add(($"{prefix} Mode", MixerFrequencyTypeEnum.Fixed));
            retVal.Add(($"{prefix} Fixed", @fixed));
        }

        private void SetInput(int Channel)
        {
            var (start, stop) = CalculateStartStop(InputMixerFrequencyStart, InputMixerFrequencyStop, InputMixerFrequencyCenter, InputMixerFrequencySpan, InputMixerFrequencyType);
            switch (InputMixerFrequencyType)
            { 
             case MixerFrequencyTypeEnum.StartStop:
                    PNAX.SetMixerFrequencyInputMode(Channel, MixerFrequencyTypeEnum.StartStop);
                    PNAX.SetFrequencyInputStart(Channel, start);
                    PNAX.SetFrequencyInputStop(Channel, stop);
                    LogStartStop("Mixer Frequency Input", start, stop);
                    break;
                case MixerFrequencyTypeEnum.CenterSpan:
                    PNAX.SetMixerFrequencyInputMode(Channel, MixerFrequencyTypeEnum.CenterSpan);
                    PNAX.SetFrequencyInputStart(Channel, start);
                    PNAX.SetFrequencyInputStop(Channel, stop);
                    LogCenterSpan("Mixer Frequency Input", InputMixerFrequencyCenter, InputMixerFrequencySpan);
                    break;
                case MixerFrequencyTypeEnum.Fixed:
                    PNAX.SetMixerFrequencyInputMode(Channel, MixerFrequencyTypeEnum.Fixed);
                    PNAX.SetFrequencyInputFixed(Channel, InputMixerFrequencyFixed);
                    LogFixed("Mixer Frequency Input", InputMixerFrequencyFixed);
                    break;
            }
        }

        private void SetLO1(int Channel)
        {
            var (start, stop) = CalculateStartStop(LO1MixerFrequencyStart, LO1MixerFrequencyStop, LO1MixerFrequencyCenter, LO1MixerFrequencySpan, LO1MixerFrequencyType);
            switch(LO1MixerFrequencyType)
            { 
                case MixerFrequencyTypeEnum.StartStop:
                    PNAX.SetMixerFrequencyLOMode(Channel, 1, MixerFrequencyTypeEnum.StartStop);
                    PNAX.SetFrequencyLOStart(Channel, 1, start);
                    PNAX.SetFrequencyLOStop(Channel, 1, stop);
                    LogStartStop("Mixer Frequency LO1", start, stop);
                    break;
                case MixerFrequencyTypeEnum.CenterSpan:
                    PNAX.SetMixerFrequencyLOMode(Channel, 1, MixerFrequencyTypeEnum.CenterSpan);
                    PNAX.SetFrequencyLOStart(Channel, 1, start);
                    PNAX.SetFrequencyLOStop(Channel, 1, stop);
                    LogCenterSpan("Mixer Frequency LO1", LO1MixerFrequencyCenter, LO1MixerFrequencySpan);
                    break;
                case MixerFrequencyTypeEnum.Fixed:
                    PNAX.SetMixerFrequencyLOMode(Channel, 1, MixerFrequencyTypeEnum.Fixed);
                    PNAX.SetFrequencyLOFixed(Channel, 1, LO1MixerFrequencyFixed);
                    LogFixed("Mixer Frequency LO1", LO1MixerFrequencyFixed);
                    break;
            }
            PNAX.SetLOILTI(Channel, 1, InputGTLO1);
            retVal.Add(("Input Greater Than LO", InputGTLO1));
        }

        private void SetIF(int Channel)
        {
            if (ConverterStages == ConverterStagesEnum._2)
            {
                var (start, stop) = CalculateStartStop(LO2MixerFrequencyStart, LO2MixerFrequencyStop, LO2MixerFrequencyCenter, LO2MixerFrequencySpan, LO2MixerFrequencyType);
                switch(IFMixerFrequencyType)
                {
                    case MixerFrequencyTypeEnum.StartStop:
                        PNAX.SetFrequencyIFStart(Channel, start);
                        PNAX.SetFrequencyIFStop(Channel, stop);
                        LogStartStop("Mixer Frequency IF", start, stop);
                        break;
                    case MixerFrequencyTypeEnum.CenterSpan:
                        PNAX.SetFrequencyIFStart(Channel, start);
                        PNAX.SetFrequencyIFStop(Channel, stop);
                        LogCenterSpan("Mixer Frequency IF", IFMixerFrequencyCenter, IFMixerFrequencySpan);
                        break;
                    case MixerFrequencyTypeEnum.Fixed:
                        // TODO find command for IF Fixed
                        // PNAX.SetFrequencyIFFixed(Channel, IFMixerFrequencyFixed);
                        break;
                }
                PNAX.SetFrequencyIFSideband(Channel, IFSidebandType);
                retVal.Add(("Mixer Frequency IF Sideband", IFSidebandType));
            }
        }

        private void SetLO2(int Channel)
        {
            if (ConverterStages == ConverterStagesEnum._2)
            {
                var (start, stop) = CalculateStartStop(LO2MixerFrequencyStart, LO2MixerFrequencyStop, LO2MixerFrequencyCenter, LO2MixerFrequencySpan, LO2MixerFrequencyType);
                switch(LO2MixerFrequencyType)
                {
                    case MixerFrequencyTypeEnum.StartStop:
                        PNAX.SetMixerFrequencyLOMode(Channel, 2, MixerFrequencyTypeEnum.StartStop);
                        PNAX.SetFrequencyLOStart(Channel, 2, start);
                        PNAX.SetFrequencyLOStop(Channel, 2, stop);
                        LogStartStop("Mixer Frequency LO2", start, stop);
                        break;
                    case MixerFrequencyTypeEnum.CenterSpan:
                        PNAX.SetMixerFrequencyLOMode(Channel, 2, MixerFrequencyTypeEnum.CenterSpan);
                        PNAX.SetFrequencyLOStart(Channel, 2, start);
                        PNAX.SetFrequencyLOStop(Channel, 2, stop);
                        LogCenterSpan("Mixer Frequency LO2", LO2MixerFrequencyCenter, LO2MixerFrequencySpan);
                        break;
                    case MixerFrequencyTypeEnum.Fixed:
                        PNAX.SetMixerFrequencyLOMode(Channel, 2, MixerFrequencyTypeEnum.Fixed);
                        PNAX.SetFrequencyLOFixed(Channel, 2, LO2MixerFrequencyFixed);
                        LogFixed("Mixer Frequency LO2", LO2MixerFrequencyFixed);
                        break;
                }
                PNAX.SetLOILTI(Channel, 2, IF1GTLO2);
                retVal.Add(("IF1 Greater Than LO2", IF1GTLO2));
            }
        }

        private void SetOutput(int Channel)
        {
            PNAX.SetMixerFrequencyOutputMode(Channel, OutputMixerFrequencyType);
            retVal.Add(("Mixer Frequency LO1 Mode", MixerFrequencyTypeEnum.Fixed));
            var (start, stop) = CalculateStartStop(OutputMixerFrequencyStart, OutputMixerFrequencyStop, OutputMixerFrequencyCenter, OutputMixerFrequencySpan, OutputMixerFrequencyType);
            switch(OutputMixerFrequencyType)
            {
                case MixerFrequencyTypeEnum.StartStop:
                    PNAX.SetFrequencyOutputStart(Channel, start);
                    PNAX.SetFrequencyOutputStop(Channel, stop);
                    LogStartStop("Mixer Frequency Output", start, stop);
                    break;
                case MixerFrequencyTypeEnum.CenterSpan:
                    PNAX.SetFrequencyOutputStart(Channel, start);
                    PNAX.SetFrequencyOutputStop(Channel, stop);
                    LogCenterSpan("Mixer Frequency Output", OutputMixerFrequencyCenter, OutputMixerFrequencySpan);
                    break;
                case MixerFrequencyTypeEnum.Fixed:
                    PNAX.SetFrequencyOutputFixed(Channel, OutputMixerFrequencyFixed);
                    LogFixed("Mixer Frequency Output", OutputMixerFrequencyFixed);
                    break;
            }
            PNAX.SetFrequencyOutputSideband(Channel, OutputSidebandType);
            retVal.Add(("Mixer Frequency Output Sideband", OutputSidebandType));
        }

        private List<(string, object)> retVal = new List<(string, object)>();

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            return retVal;
        }

    }
}
