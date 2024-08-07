﻿// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using OpenTap;
using OpenTap.Plugins.PNAX.General.Spectrum_Analyzer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OpenTap.Plugins.PNAX
{
    //[AllowAsChildIn(typeof(GeneralSweptIMDChannel))]
    //[AllowChildrenOfType(typeof(GeneralSweptIMDSingleTrace))]
    [Display("Swept IMD Traces", Groups: new[] { "Network Analyzer", "General", "Swept IMD" }, Description: "Insert a description here")]
    public class GeneralSweptIMDNewTrace : AddNewTraceBaseStep
    {
        #region Settings

        private SweptIMDTraceEnum Meas;

        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
        [Display("Meas", Groups: new[] { "Trace" }, Order: 0.2)]
        public string ParamName { get; set; }


        private IMDTraceTypeEnum _IMDTraceType;
        [Display("Type", Groups: new[] { "Trace" }, Order: 1)]
        public IMDTraceTypeEnum IMDTraceType
        {
            get { return _IMDTraceType; }
            set
            {
                _IMDTraceType = value;
                if ((_IMDTraceType == IMDTraceTypeEnum.TonePower) ||
                    (_IMDTraceType == IMDTraceTypeEnum.ToneGain) )
                {
                    IMDOrderOptions = new List<int> { 1, 2, 3, 5, 7, 9 };
                    IMDOrder = 1;
                }
                else
                {
                    IMDOrderOptions = new List<int> { 2, 3, 5, 7, 9 };
                    IMDOrder = 2;
                }
                UpdateSweptIMDTestName();
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
                UpdateSweptIMDTestName();
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
                UpdateSweptIMDTestName();
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
                UpdateSweptIMDTestName();
            }
        }


        #endregion

        private void UpdateSweptIMDTestName()
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
                Meas = (SweptIMDTraceEnum) Enum.Parse(typeof(SweptIMDTraceEnum), ParamName);
            }
            else
            {
                //disable button
                EnableButton = false;
            }
        }

        public GeneralSweptIMDNewTrace()
        {
            IMDTraceType = IMDTraceTypeEnum.TonePower;
            IMDOrder = 1;
            IMDMeasureAt = IMDMeasureAtEnum.DUTOUT;
            EnableButton = false;
            UpdateSweptIMDTestName();
            ChildTestSteps.Add(new GeneralSweptIMDSingleTrace() { PNAX = this.PNAX, Meas = this.Meas, Channel = this.Channel, IsControlledByParent = true, EnableTraceSettings = true });
        }

        protected override void AddNewTrace()
        {
            ChildTestSteps.Add(new GeneralSweptIMDSingleTrace() { PNAX = this.PNAX, Meas = this.Meas, Channel = this.Channel, IsControlledByParent = true, EnableTraceSettings = true });
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
