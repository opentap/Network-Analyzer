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

        [Display("Input", Groups: new[] { "Mixer Frequency", "Input" }, Order: 10)]
        public MixerFrequencyTypeEnum InputMixerFrequencyType { get; set; }

        [EnabledIf("InputMixerFrequencyType", MixerFrequencyTypeEnum.StartStop, HideIfDisabled = true)]
        [Display("Start", Groups: new[] { "Mixer Frequency", "Input" }, Order: 11)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double InputMixerFrequencyStart { get; set; }

        [EnabledIf("InputMixerFrequencyType", MixerFrequencyTypeEnum.StartStop, HideIfDisabled = true)]
        [Display("Stop", Groups: new[] { "Mixer Frequency", "Input" }, Order: 12)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double InputMixerFrequencyStop { get; set; }

        [EnabledIf("InputMixerFrequencyType", MixerFrequencyTypeEnum.CenterSpan, HideIfDisabled = true)]
        [Display("Center", Groups: new[] { "Mixer Frequency", "Input" }, Order: 13)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double InputMixerFrequencyCenter { get; set; }

        [EnabledIf("InputMixerFrequencyType", MixerFrequencyTypeEnum.CenterSpan, HideIfDisabled = true)]
        [Display("Span", Groups: new[] { "Mixer Frequency", "Input" }, Order: 14)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double InputMixerFrequencySpan { get; set; }

        [EnabledIf("InputMixerFrequencyType", MixerFrequencyTypeEnum.Fixed, HideIfDisabled = true)]
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

        [Display("LO1", Groups: new[] { "Mixer Frequency", "LO1" }, Order: 20)]
        public MixerFrequencyTypeEnum LO1MixerFrequencyType { get; set; }

        [EnabledIf("LO1MixerFrequencyType", MixerFrequencyTypeEnum.StartStop, HideIfDisabled = true)]
        [Display("Start", Groups: new[] { "Mixer Frequency", "LO1" }, Order: 21)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LO1MixerFrequencyStart { get; set; }

        [EnabledIf("LO1MixerFrequencyType", MixerFrequencyTypeEnum.StartStop, HideIfDisabled = true)]
        [Display("Stop", Groups: new[] { "Mixer Frequency", "LO1" }, Order: 22)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LO1MixerFrequencyStop { get; set; }

        [EnabledIf("LO1MixerFrequencyType", MixerFrequencyTypeEnum.CenterSpan, HideIfDisabled = true)]
        [Display("Center", Groups: new[] { "Mixer Frequency", "LO1" }, Order: 23)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LO1MixerFrequencyCenter { get; set; }

        [EnabledIf("LO1MixerFrequencyType", MixerFrequencyTypeEnum.CenterSpan, HideIfDisabled = true)]
        [Display("Span", Groups: new[] { "Mixer Frequency", "LO1" }, Order: 24)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LO1MixerFrequencySpan { get; set; }

        [EnabledIf("LO1MixerFrequencyType", MixerFrequencyTypeEnum.Fixed, HideIfDisabled = true)]
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


        [Display("IF", Groups: new[] { "Mixer Frequency", "IF" }, Order: 30)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        public MixerFrequencyTypeEnum IFMixerFrequencyType { get; set; }

        [Display("Sideband", Groups: new[] { "Mixer Frequency", "IF" }, Order: 30.5)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        public SidebandTypeEnum IFSidebandType { get; set; }

        [EnabledIf("IFMixerFrequencyType", MixerFrequencyTypeEnum.StartStop, HideIfDisabled = true)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("Start", Groups: new[] { "Mixer Frequency", "IF" }, Order: 31)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double IFMixerFrequencyStart { get; set; }

        [EnabledIf("IFMixerFrequencyType", MixerFrequencyTypeEnum.StartStop, HideIfDisabled = true)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("Stop", Groups: new[] { "Mixer Frequency", "IF" }, Order: 32)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double IFMixerFrequencyStop { get; set; }

        [EnabledIf("IFMixerFrequencyType", MixerFrequencyTypeEnum.CenterSpan, HideIfDisabled = true)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("Center", Groups: new[] { "Mixer Frequency", "IF" }, Order: 33)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double IFMixerFrequencyCenter { get; set; }

        [EnabledIf("IFMixerFrequencyType", MixerFrequencyTypeEnum.CenterSpan, HideIfDisabled = true)]
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

        [Display("LO2", Groups: new[] { "Mixer Frequency", "LO2" }, Order: 40)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        public MixerFrequencyTypeEnum LO2MixerFrequencyType { get; set; }

        [EnabledIf("LO2MixerFrequencyType", MixerFrequencyTypeEnum.StartStop, HideIfDisabled = true)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("Start", Groups: new[] { "Mixer Frequency", "LO2" }, Order: 41)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LO2MixerFrequencyStart { get; set; }

        [EnabledIf("LO2MixerFrequencyType", MixerFrequencyTypeEnum.StartStop, HideIfDisabled = true)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("Stop", Groups: new[] { "Mixer Frequency", "LO2" }, Order: 42)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LO2MixerFrequencyStop { get; set; }

        [EnabledIf("LO2MixerFrequencyType", MixerFrequencyTypeEnum.CenterSpan, HideIfDisabled = true)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("Center", Groups: new[] { "Mixer Frequency", "LO2" }, Order: 43)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LO2MixerFrequencyCenter { get; set; }

        [EnabledIf("LO2MixerFrequencyType", MixerFrequencyTypeEnum.CenterSpan, HideIfDisabled = true)]
        [EnabledIf("DoubleStage", true, HideIfDisabled = true)]
        [Display("Span", Groups: new[] { "Mixer Frequency", "LO2" }, Order: 44)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double LO2MixerFrequencySpan { get; set; }

        [EnabledIf("LO2MixerFrequencyType", MixerFrequencyTypeEnum.Fixed, HideIfDisabled = true)]
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


        [Display("Output", Groups: new[] { "Mixer Frequency", "Output" }, Order: 50)]
        public MixerFrequencyTypeEnum OutputMixerFrequencyType { set; get; }

        [Display("Sideband", Groups: new[] { "Mixer Frequency", "Output" }, Order: 50.5)]
        public SidebandTypeEnum OutputSidebandType { get; set; }

        [EnabledIf("OutputMixerFrequencyType", MixerFrequencyTypeEnum.StartStop, HideIfDisabled = true)]
        [Display("Start", Groups: new[] { "Mixer Frequency", "Output" }, Order: 51)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double OutputMixerFrequencyStart { get; set; }

        [EnabledIf("OutputMixerFrequencyType", MixerFrequencyTypeEnum.StartStop, HideIfDisabled = true)]
        [Display("Stop", Groups: new[] { "Mixer Frequency", "Output" }, Order: 52)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double OutputMixerFrequencyStop { get; set; }

        [EnabledIf("OutputMixerFrequencyType", MixerFrequencyTypeEnum.CenterSpan, HideIfDisabled = true)]
        [Display("Center", Groups: new[] { "Mixer Frequency", "Output" }, Order: 53)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double OutputMixerFrequencyCenter { get; set; }

        [EnabledIf("OutputMixerFrequencyType", MixerFrequencyTypeEnum.CenterSpan, HideIfDisabled = true)]
        [Display("Span", Groups: new[] { "Mixer Frequency", "Output" }, Order: 54)]
        [Unit("Hz", UseEngineeringPrefix: true)]
        public double OutputMixerFrequencySpan { get; set; }

        [EnabledIf("OutputMixerFrequencyType", MixerFrequencyTypeEnum.Fixed, HideIfDisabled = true)]
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


        public MixerFrequencyTestStep()
        {
            IsConverter = true;
            UpdateDefaultValues();
            retVal = new List<(string, object)>();
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

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

            // Now read back and validate the values were not changed by the Calculate command
            ValidateInput(Channel);
            ValidateLO1(Channel);
            ValidateIF(Channel);
            ValidateLO2(Channel);
            ValidateOutput(Channel);

            UpgradeVerdict(Verdict.Pass);
        }

        /// <summary>
        /// A generic method to execute a calculation on a dummy channel, handling setup and cleanup.
        /// </summary>
        private void ExecuteCalculation(string calcCommand, Action<int> setPrerequisites, Action<int> readbackAction)
        {
            const int DummyChannel = 234;
            string dummyTraceName = $"CH{DummyChannel}_DUMMY_SC21_1";
            try
            {
                PNAX.Open();
                Log.Info($"Calculating values for command: {calcCommand}");
                PNAX.ScpiCommand($"CALCulate{DummyChannel}:CUST:DEFine '{dummyTraceName}','Gain Compression Converters','SC21'");

                setPrerequisites(DummyChannel);

                PNAX.MixerCalc(DummyChannel, calcCommand);
                PNAX.WaitForOperationComplete();

                readbackAction(DummyChannel);
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to calculate values for {calcCommand}. {ex.Message}");
            }
            finally
            {
                if (PNAX.IsConnected)
                {
                    PNAX.ScpiCommand($"CALCulate{DummyChannel}:PARameter:DELete '{dummyTraceName}'");
                    PNAX.Close();
                }
            }
        }

        private void CalcInputValues()
        {
            ExecuteCalculation("INP",
                ch => { SetLO1(ch); SetIF(ch); SetLO2(ch); SetOutput(ch); SetMultiplier(ch); },
                ch =>
                {
                    string inpMode = PNAX.GetMixerFrequencyInputMode(ch);
                    if (inpMode.Equals("SWEPT"))
                    {
                        InputMixerFrequencyType = MixerFrequencyTypeEnum.StartStop;
                        InputMixerFrequencyStart = PNAX.GetFrequencyInputStart(ch);
                        InputMixerFrequencyStop = PNAX.GetFrequencyInputStop(ch);
                    }
                    else if (inpMode.Equals("FIXED"))
                    {
                        InputMixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
                        InputMixerFrequencyFixed = PNAX.GetFrequencyInputFixed(ch);
                    }
                });
        }

        private void CalcLO1Values()
        {
            ExecuteCalculation("LO_1",
                ch => { SetInput(ch); SetIF(ch); SetLO2(ch); SetOutput(ch); SetMultiplier(ch); },
                ch =>
                {
                    string inpMode = PNAX.GetMixerFrequencyLOMode(ch, 1);
                    if (inpMode.Equals("SWEPT"))
                    {
                        LO1MixerFrequencyType = MixerFrequencyTypeEnum.StartStop;
                        LO1MixerFrequencyStart = PNAX.GetFrequencyLOStart(ch, 1);
                        LO1MixerFrequencyStop = PNAX.GetFrequencyLOStop(ch, 1);
                    }
                    else if (inpMode.Equals("FIXED"))
                    {
                        LO1MixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
                        LO1MixerFrequencyFixed = PNAX.GetFrequencyLOFixed(ch, 1);
                    }
                    InputGTLO1 = PNAX.GetLOILTI(ch, 1);
                });
        }

        private void CalcLO2Values()
        {
            ExecuteCalculation("LO_2",
                ch => { SetInput(ch); SetIF(ch); SetLO1(ch); SetOutput(ch); SetMultiplier(ch); },
                ch =>
                {
                    string inpMode = PNAX.GetMixerFrequencyLOMode(ch, 2);
                    if (inpMode.Equals("SWEPT"))
                    {
                        LO2MixerFrequencyType = MixerFrequencyTypeEnum.StartStop;
                        LO2MixerFrequencyStart = PNAX.GetFrequencyLOStart(ch, 2);
                        LO2MixerFrequencyStop = PNAX.GetFrequencyLOStop(ch, 2);
                    }
                    else if (inpMode.Equals("FIXED"))
                    {
                        LO2MixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
                        LO2MixerFrequencyFixed = PNAX.GetFrequencyLOFixed(ch, 2);
                    }
                    IF1GTLO2 = PNAX.GetLOILTI(ch, 2);
                });
        }

        private void CalcOutputValues()
        {
            ExecuteCalculation("OUTP",
                ch => { SetInput(ch); SetLO1(ch); SetIF(ch); SetLO2(ch); SetMultiplier(ch); },
                ch =>
                {
                    string inpMode = PNAX.GetMixerFrequencyOutputMode(ch);
                    if (inpMode.Equals("SWEPT"))
                    {
                        OutputMixerFrequencyType = MixerFrequencyTypeEnum.StartStop;
                        OutputMixerFrequencyStart = PNAX.GetFrequencyOutputStart(ch);
                        OutputMixerFrequencyStop = PNAX.GetFrequencyOutputStop(ch);
                    }
                    else if (inpMode.Equals("FIXED"))
                    {
                        OutputMixerFrequencyType = MixerFrequencyTypeEnum.Fixed;
                        OutputMixerFrequencyFixed = PNAX.GetFrequencyOutputFixed(ch);
                    }
                    OutputSidebandType = PNAX.GetFrequencyOutputSideband(ch);
                });
        }

        /// <summary>
        /// A generic method to apply frequency settings for any stage.
        /// It centralizes the Start/Stop vs Center/Span vs Fixed logic.
        /// </summary>
        private void ApplyStageSettings(int channel, string stageName, MixerFrequencyTypeEnum freqType, double start, double stop, double center, double span, double fixedFreq, 
            Action<int, MixerFrequencyTypeEnum> setMode,
            Action<int, double> setStart, 
            Action<int, double> setStop, 
            Action<int, double> setFixed)
        {
            setMode?.Invoke(channel, freqType);
            retVal.Add(($"{stageName} Mode", freqType));

            if (freqType == MixerFrequencyTypeEnum.CenterSpan)
            {
                double calculatedStart = center - (span / 2);
                double calculatedStop = center + (span / 2);
                setStart(channel, calculatedStart);
                setStop(channel, calculatedStop);
                retVal.Add(($"{stageName} Center", center));
                retVal.Add(($"{stageName} Span", span));
            }
            else if (freqType == MixerFrequencyTypeEnum.StartStop)
            {
                setStart(channel, start);
                setStop(channel, stop);
                retVal.Add(($"{stageName} Start", start));
                retVal.Add(($"{stageName} Stop", stop));
            }
            else // Fixed
            {
                setFixed(channel, fixedFreq);
                retVal.Add(($"{stageName} Fixed", fixedFreq));
            }
        }

        /// <summary>
        /// A generic method to validate settings for any stage after they have been applied.
        /// </summary>
        private void ValidateStageSettings(int channel, MixerFrequencyTypeEnum freqType, double start, double stop, double center, double span, double fixedFreq, 
            Action<int, string> validateMode,
            Action<int, double> validateStart, 
            Action<int, double> validateStop, 
            Action<int, double> validateFixed)
        {
            if (freqType == MixerFrequencyTypeEnum.Fixed)
            {
                validateMode?.Invoke(channel, "FIXED");
                validateFixed(channel, fixedFreq);
            }
            else // StartStop or CenterSpan are both "SWEPT" mode
            {
                validateMode?.Invoke(channel, "SWEPT");
                double expectedStart = (freqType == MixerFrequencyTypeEnum.CenterSpan) ? center - (span / 2) : start;
                double expectedStop = (freqType == MixerFrequencyTypeEnum.CenterSpan) ? center + (span / 2) : stop;
                validateStart(channel, expectedStart);
                validateStop(channel, expectedStop);
            }
        }

        private void SetInput(int channel)
        {
            ApplyStageSettings(channel, "Input", InputMixerFrequencyType, InputMixerFrequencyStart, InputMixerFrequencyStop, InputMixerFrequencyCenter, InputMixerFrequencySpan, InputMixerFrequencyFixed,
            (ch, type) => PNAX.SetMixerFrequencyInputMode(ch, type),
            PNAX.SetFrequencyInputStart,
            PNAX.SetFrequencyInputStop,
            PNAX.SetFrequencyInputFixed);
        }

        private void ValidateInput(int channel)
        {
            ValidateStageSettings(channel, InputMixerFrequencyType, InputMixerFrequencyStart, InputMixerFrequencyStop, InputMixerFrequencyCenter, InputMixerFrequencySpan, InputMixerFrequencyFixed,
                (ch, mode) => PNAX.ValidateMixerFrequencyInputMode(ch, mode), 
                PNAX.ValidateFrequencyInputStart, 
                PNAX.ValidateFrequencyInputStop, 
                PNAX.ValidateFrequencyInputFixed);
        }

        private void SetLO1(int channel)
        {
            ApplyStageSettings(channel, "LO1", LO1MixerFrequencyType, LO1MixerFrequencyStart, LO1MixerFrequencyStop, LO1MixerFrequencyCenter, LO1MixerFrequencySpan, LO1MixerFrequencyFixed,
                (ch, type) => PNAX.SetMixerFrequencyLOMode(ch, 1, type),
                (ch, val) => PNAX.SetFrequencyLOStart(ch, 1, val),
                (ch, val) => PNAX.SetFrequencyLOStop(ch, 1, val),
                (ch, val) => PNAX.SetFrequencyLOFixed(ch, 1, val));
            PNAX.SetLOILTI(channel, 1, InputGTLO1);
            retVal.Add(("Input Greater Than LO1", InputGTLO1));
        }

        private void ValidateLO1(int channel)
        {
            ValidateStageSettings(channel, LO1MixerFrequencyType, LO1MixerFrequencyStart, LO1MixerFrequencyStop, LO1MixerFrequencyCenter, LO1MixerFrequencySpan, LO1MixerFrequencyFixed,
                (ch, mode) => PNAX.ValidateMixerFrequencyLOMode(ch, 1, mode), 
                (ch, val) => PNAX.ValidateFrequencyLOStart(ch, 1, val),
                (ch, val) => PNAX.ValidateFrequencyLOStop(ch, 1, val), 
                (ch, val) => PNAX.ValidateFrequencyLOFixed(ch, 1, val));
            PNAX.ValidateLOILTI(channel, 1, InputGTLO1);
        }

        private void SetIF(int channel)
        {
            if (ConverterStages == ConverterStagesEnum._2)
            {
                ApplyStageSettings(channel, "IF", IFMixerFrequencyType, IFMixerFrequencyStart, IFMixerFrequencyStop, IFMixerFrequencyCenter, IFMixerFrequencySpan, IFMixerFrequencyFixed,
                    null, // No "mode" for IF
                    (ch, val) => PNAX.SetFrequencyIFStart(ch, val),
                    (ch, val) => PNAX.SetFrequencyIFStop(ch, val),
                    (ch, val) => {/* TODO */});
                PNAX.SetFrequencyIFSideband(channel, IFSidebandType);
                retVal.Add(("IF Sideband", IFSidebandType));
            }
        }

        private void ValidateIF(int channel)
        {
            if (ConverterStages == ConverterStagesEnum._2)
            {
                ValidateStageSettings(channel, IFMixerFrequencyType, IFMixerFrequencyStart, IFMixerFrequencyStop, IFMixerFrequencyCenter, IFMixerFrequencySpan, IFMixerFrequencyFixed, 
                    null,
                    (ch, val) => PNAX.ValidateFrequencyIFStart(ch, val), 
                    (ch, val) => PNAX.ValidateFrequencyIFStop(ch, val), 
                    (ch, val) => {/* TODO */});
                PNAX.ValidateFrequencyIFSideband(channel, IFSidebandType);
            }
        }

        private void SetLO2(int channel)
        {
            if (ConverterStages == ConverterStagesEnum._2)
            {
                ApplyStageSettings(channel, "LO2", LO2MixerFrequencyType, LO2MixerFrequencyStart, LO2MixerFrequencyStop, LO2MixerFrequencyCenter, LO2MixerFrequencySpan, LO2MixerFrequencyFixed,
                    (ch, type) => PNAX.SetMixerFrequencyLOMode(ch, 2, type),
                    (ch, val) => PNAX.SetFrequencyLOStart(ch, 2, val),
                    (ch, val) => PNAX.SetFrequencyLOStop(ch, 2, val),
                    (ch, val) => PNAX.SetFrequencyLOFixed(ch, 2, val));
                PNAX.SetLOILTI(channel, 2, IF1GTLO2);
                retVal.Add(("IF1 Greater Than LO2", IF1GTLO2));
            }
        }

        private void ValidateLO2(int channel)
        {
            if (ConverterStages == ConverterStagesEnum._2)
            {
                ValidateStageSettings(channel, LO2MixerFrequencyType, LO2MixerFrequencyStart, LO2MixerFrequencyStop, LO2MixerFrequencyCenter, LO2MixerFrequencySpan, LO2MixerFrequencyFixed,
                    (ch, mode) => PNAX.ValidateMixerFrequencyLOMode(ch, 2, mode),
                    (ch, val) => PNAX.ValidateFrequencyLOStart(ch, 2, val),
                    (ch, val) => PNAX.ValidateFrequencyLOStop(ch, 2, val),
                    (ch, val) => PNAX.ValidateFrequencyLOFixed(ch, 2, val));
                PNAX.ValidateLOILTI(channel, 2, IF1GTLO2);
            }
        }

        private void SetOutput(int channel)
        {
            ApplyStageSettings(channel, "Output", OutputMixerFrequencyType, OutputMixerFrequencyStart, OutputMixerFrequencyStop, OutputMixerFrequencyCenter, OutputMixerFrequencySpan, OutputMixerFrequencyFixed,
                (ch, type) => PNAX.SetMixerFrequencyOutputMode(ch, type),
                PNAX.SetFrequencyOutputStart,
                PNAX.SetFrequencyOutputStop,
                PNAX.SetFrequencyOutputFixed);
            PNAX.SetFrequencyOutputSideband(channel, OutputSidebandType);
            retVal.Add(("Output Sideband", OutputSidebandType));
        }

        private void ValidateOutput(int channel)
        {
            ValidateStageSettings(channel, OutputMixerFrequencyType, OutputMixerFrequencyStart, OutputMixerFrequencyStop, OutputMixerFrequencyCenter, OutputMixerFrequencySpan, OutputMixerFrequencyFixed, 
                null, // No mode validation for output
                (ch, val) => PNAX.ValidateFrequencyOutputStart(ch, val),
                (ch, val) => PNAX.ValidateFrequencyOutputStop(ch, val),
                (ch, val) => PNAX.ValidateFrequencyOutputFixed(ch, val));
            PNAX.ValidateFrequencyOutputSideband(channel, OutputSidebandType);
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
                retVal.Add(("LO2 Fractional Multiplier Numerator", LO2FractionalMultiplierNumerator));
                retVal.Add(("LO2 Fractional Multiplier Denominator", LO2FractionalMultiplierDenominator));
            }
        }

        private List<(string, object)> retVal;

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            return retVal;
        }

    }
}
