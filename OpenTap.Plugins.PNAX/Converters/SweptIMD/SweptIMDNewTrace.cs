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
    public enum IMDTraceTypeEnum
    {
        TonePower,
        IMDRelativeToCarrier,
        InputReferredIntcptPt,
        OutputReferredIntcptPt,
        CTBMidBandDistortion,
        CTBEBandEdgeDistortion,
        CSODistortion,
        XMOD3rdOrderCrossmod,
        ToneGain,
        AI1,
        AI2,
        AIG,
        AOS1,
        AOS2
    }

    public enum IMDToneSelectEnum
    {
        Avg,
        Low,
        High,
        Max,
        Min
    }

    public enum IMDMeasureAtEnum
    {
        [Display("DUT IN")]
        DUTIN,
        [Display("DUT OUT")]
        DUTOUT
    }

    [AllowAsChildIn(typeof(SweptIMDChannel))]
    [AllowChildrenOfType(typeof(SweptIMDSingleTrace))]
    [Display("Swept IMD Traces", Groups: new[] { "PNA-X", "Converters", "Swept IMD Converters" }, Description: "Insert a description here")]
    public class SweptIMDNewTrace : ConverterNewTraceBaseStep
    {
        #region Settings

        private IMDTraceTypeEnum _IMDTraceType;
        [Display("Type", Groups: new[] { "Trace" }, Order: 1)]
        public IMDTraceTypeEnum IMDTraceType
        {
            get { return _IMDTraceType; }
            set
            {
                _IMDTraceType = value;
                if (_IMDTraceType == IMDTraceTypeEnum.TonePower)
                {
                    IMDOrderOptions = new List<int> { 1, 2, 3, 5, 7, 9 };
                    IMDOrder = 1;
                }
                else
                {
                    IMDOrderOptions = new List<int> { 2, 3, 5, 7, 9 };
                    IMDOrder = 2;
                }
                UpdateTestName();
            }
        }


        private IMDToneSelectEnum _MDToneSelect;
        [Display("Tone Select", Groups: new[] { "Trace" }, Order: 2)]
        public IMDToneSelectEnum IMDToneSelect
        {
            get
            {
                return _MDToneSelect;
            }
            set
            {
                _MDToneSelect = value;
                UpdateTestName();
            }
        }

        private List<int> _IMDOrderOptions = new List<int> { 1, 2, 3, 5, 7, 9 };
        [Browsable(false)]
        [Display("Order Options", Groups: new[] { "Trace" }, Order: 0.1)]
        public List<int> IMDOrderOptions
        {
            get { return _IMDOrderOptions; }
            set
            {
                _IMDOrderOptions = value;
                OnPropertyChanged("IMDOrderOptions");
            }
        }

        private int _IMDOrder;
        [AvailableValues(nameof(IMDOrderOptions))]
        [Display("Order", Groups: new[] { "Trace" }, Order: 3)]
        public int IMDOrder
        {
            get
            {
                return _IMDOrder;
            }
            set
            {
                _IMDOrder = value;
                UpdateTestName();
            }
        }



        private IMDMeasureAtEnum _IMDMeasureAt;
        [Display("Measure At", Groups: new[] { "Trace" }, Order: 4)]
        public IMDMeasureAtEnum IMDMeasureAt
        {
            get
            {
                return _IMDMeasureAt;
            }
            set
            {
                _IMDMeasureAt = value;
                UpdateTestName();
            }
        }

        public string ParamName { get; set; }

        #endregion

        private void UpdateTestName()
        {
            string TypeString = "";
            string OrderString = "";
            string MeasAtString = "";
            string ToneString = "";

            if (IMDMeasureAt == IMDMeasureAtEnum.DUTIN)
            {
                MeasAtString = "In";
            }

            switch (IMDOrder)
            {
                case 1:
                    OrderString = "Main";
                    break;
                case 2:
                    OrderString = "2";
                    break;
                case 3:
                    OrderString = "3";
                    break;
                case 5:
                    OrderString = "5";
                    break;
                case 7:
                    OrderString = "7";
                    break;
                case 9:
                    OrderString = "9";
                    break;
            }

            switch (IMDToneSelect)
            {
                case IMDToneSelectEnum.Avg:
                    ToneString = "";
                    break;
                case IMDToneSelectEnum.Low:
                    ToneString = "Lo";
                    break;
                case IMDToneSelectEnum.High:
                    ToneString = "Hi";
                    break;
                case IMDToneSelectEnum.Min:
                    ToneString = "Min";
                    break;
                case IMDToneSelectEnum.Max:
                    ToneString = "Max";
                    break;
            }

            switch (IMDTraceType)
            {
                case IMDTraceTypeEnum.TonePower:
                    TypeString = "Pwr";
                    ParamName = $"{TypeString}{OrderString}{ToneString}{MeasAtString}";
                    break;
                case IMDTraceTypeEnum.IMDRelativeToCarrier:
                    TypeString = "IM";
                    ParamName = $"{TypeString}{OrderString}{ToneString}{MeasAtString}";
                    break;
                case IMDTraceTypeEnum.InputReferredIntcptPt:
                    TypeString = "IIP";
                    ParamName = $"{TypeString}{OrderString}{ToneString}{MeasAtString}";
                    break;
                case IMDTraceTypeEnum.OutputReferredIntcptPt:
                    TypeString = "OIP";
                    ParamName = $"{TypeString}{OrderString}{ToneString}{MeasAtString}";
                    break;
                case IMDTraceTypeEnum.CTBMidBandDistortion:
                    TypeString = "CTB";
                    ParamName = $"{TypeString}{ToneString}";
                    break;
                case IMDTraceTypeEnum.CTBEBandEdgeDistortion:
                    TypeString = "CTBE";
                    ParamName = $"{TypeString}{ToneString}";
                    break;
                case IMDTraceTypeEnum.CSODistortion:
                    TypeString = "CSO";
                    ParamName = $"{TypeString}{ToneString}";
                    break;
                case IMDTraceTypeEnum.XMOD3rdOrderCrossmod:
                    TypeString = "XMOD";
                    ParamName = $"{TypeString}{ToneString}";
                    break;
                case IMDTraceTypeEnum.ToneGain:
                    TypeString = "ToneGain";
                    ParamName = $"{TypeString}{ToneString}";
                    break;
                case IMDTraceTypeEnum.AI1:
                    ParamName = "AI1";
                    return;
                case IMDTraceTypeEnum.AI2:
                    ParamName = "AI2";
                    return;
                case IMDTraceTypeEnum.AIG:
                    ParamName = "AIG";
                    return;
                case IMDTraceTypeEnum.AOS1:
                    ParamName = "AOS1";
                    return;
                case IMDTraceTypeEnum.AOS2:
                    ParamName = "AOS2";
                    return;
            }

            if (Enum.IsDefined(typeof(SweptIMDTraceEnum), ParamName))
            {
                // enable button
                EnableButton = true;
            }
            else
            {
                //disable button
                EnableButton = false;
            }
        }

        public SweptIMDNewTrace()
        {
            IMDTraceType = IMDTraceTypeEnum.TonePower;
            IMDOrder = 1;
            IMDMeasureAt = IMDMeasureAtEnum.DUTOUT;
            EnableButton = false;
            UpdateTestName();
            ChildTestSteps.Add(new SweptIMDSingleTrace() { Meas = SweptIMDTraceEnum.PwrMain});
        }

        protected override void AddNewTrace()
        {
            SweptIMDTraceEnum sweptIMD;
            if (Enum.TryParse<SweptIMDTraceEnum>(ParamName, out sweptIMD))
            {
                this.ChildTestSteps.Add(new SweptIMDSingleTrace() { Meas = sweptIMD, Channel = this.Channel });
            }
        }

        public override void Run()
        {
            // Delete dummy trace defined during channel setup
            // DISPlay:MEASure<mnum>:DELete?
            // CALCulate<cnum>:PARameter:DELete[:NAME] <Mname>
            PNAX.ScpiCommand($"CALCulate{Channel}:PARameter:DELete \'CH{Channel}_DUMMY_PwrMain_1\'");

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
