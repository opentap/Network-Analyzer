// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using OpenTap;

namespace OpenTap.Plugins.PNAX
{
    public enum SweptIMDTraceEnum
    {
        PwrMain,
        PwrMainLo,
        PwrMainHi,
        PwrMainMax,
        PwrMainMin,
        PwrMainIn,
        PwrMainLoIn,
        PwrMainHiIn,
        PwrMainMaxIn,
        PwrMainMinIn,
        Pwr2Lo,
        Pwr2Hi,
        Pwr2Max,
        Pwr2Min,
        Pwr2LoIn,
        Pwr2HiIn,
        Pwr2MaxIn,
        Pwr2MinIn,
        Pwr3,
        Pwr3Lo,
        Pwr3Hi,
        Pwr3Max,
        Pwr3Min,
        Pwr3In,
        Pwr3LoIn,
        Pwr3HiIn,
        Pwr3MaxIn,
        Pwr3MinIn,
        Pwr5,
        Pwr5Lo,
        Pwr5Hi,
        Pwr5Max,
        Pwr5Min,
        Pwr5In,
        Pwr5LoIn,
        Pwr5HiIn,
        Pwr5MaxIn,
        Pwr5MinIn,
        Pwr7,
        Pwr7Lo,
        Pwr7Hi,
        Pwr7Max,
        Pwr7Min,
        Pwr7In,
        Pwr7LoIn,
        Pwr7HiIn,
        Pwr7MaxIn,
        Pwr7MinIn,
        Pwr9,
        Pwr9Lo,
        Pwr9Hi,
        Pwr9Max,
        Pwr9Min,
        Pwr9In,
        Pwr9LoIn,
        Pwr9HiIn,
        Pwr9MaxIn,
        Pwr9MinIn,
        IM2Lo,
        IM2Hi,
        IM2Max,
        IM2Min,
        IM2LoIn,
        IM2HiIn,
        IM2MaxIn,
        IM2MinIn,
        IM3,
        IM3Lo,
        IM3Hi,
        IM3Max,
        IM3Min,
        IM3In,
        IM3LoIn,
        IM3HiIn,
        IM3MaxIn,
        IM3MinIn,
        IM5,
        IM5Lo,
        IM5Hi,
        IM5Max,
        IM5Min,
        IM5In,
        IM5LoIn,
        IM5HiIn,
        IM5MaxIn,
        IM5MinIn,
        IM7,
        IM7Lo,
        IM7Hi,
        IM7Max,
        IM7Min,
        IM7In,
        IM7LoIn,
        IM7HiIn,
        IM7MaxIn,
        IM7MinIn,
        IM9,
        IM9Lo,
        IM9Hi,
        IM9Max,
        IM9Min,
        IM9In,
        IM9LoIn,
        IM9HiIn,
        IM9MaxIn,
        IM9MinIn,
        IIP2Lo,
        IIP2Hi,
        IIP2Max,
        IIP2Min,
        IIP2LoIn,
        IIP2HiIn,
        IIP2MaxIn,
        IIP2MinIn,
        IIP3,
        IIP3Lo,
        IIP3Hi,
        IIP3Max,
        IIP3Min,
        IIP3In,
        IIP3LoIn,
        IIP3HiIn,
        IIP3MaxIn,
        IIP3MinIn,
        CTBLo,
        CTBHi,
        CTB,
        CTBMax,
        CTBMin,
        CTBELo,
        CTBEHi,
        CTBE,
        CTBEMax,
        CTBEMin,
        XMODLo,
        XMODHi,
        XMOD,
        XMODMax,
        XMODMin,
        IIP5,
        IIP5Lo,
        IIP5Hi,
        IIP5Max,
        IIP5Min,
        IIP5In,
        IIP5LoIn,
        IIP5HiIn,
        IIP5MaxIn,
        IIP5MinIn,
        IIP7,
        IIP7Lo,
        IIP7Hi,
        IIP7Max,
        IIP7Min,
        IIP7In,
        IIP7LoIn,
        IIP7HiIn,
        IIP7MaxIn,
        IIP7MinIn,
        IIP9,
        IIP9Lo,
        IIP9Hi,
        IIP9Max,
        IIP9Min,
        IIP9In,
        IIP9LoIn,
        IIP9HiIn,
        IIP9MaxIn,
        IIP9MinIn,
        OIP2Lo,
        OIP2Hi,
        OIP2Max,
        OIP2Min,
        OIP2LoIn,
        OIP2HiIn,
        OIP2MaxIn,
        OIP2MinIn,
        CSOHi,
        CSOLo,
        CSOMax,
        CSOMin,
        OIP3,
        OIP3Lo,
        OIP3Hi,
        OIP3Max,
        OIP3Min,
        OIP3In,
        OIP3LoIn,
        OIP3HiIn,
        OIP3MaxIn,
        OIP3MinIn,
        OIP5,
        OIP5Lo,
        OIP5Hi,
        OIP5Max,
        OIP5Min,
        OIP5In,
        OIP5LoIn,
        OIP5HiIn,
        OIP5MaxIn,
        OIP5MinIn,
        OIP7,
        OIP7Lo,
        OIP7Hi,
        OIP7Max,
        OIP7Min,
        OIP7In,
        OIP7LoIn,
        OIP7HiIn,
        OIP7MaxIn,
        OIP7MinIn,
        OIP9,
        OIP9Lo,
        OIP9Hi,
        OIP9Max,
        OIP9Min,
        OIP9In,
        OIP9LoIn,
        OIP9HiIn,
        OIP9MaxIn,
        OIP9MinIn,
        ToneGainHi,
        ToneGain,
        ToneGainLo,
        ToneGainMax,
        ToneGainMin,
        AI1,
        AI2,
        AIG,
        AOS1,
        AOS2,
    }

    public enum TraceManagerChannelClassEnum
    {
        GCX,
        STD,
        IMDX,
    }

    //[AllowAsChildIn(typeof(SweptIMDChannel))]
    //[AllowAsChildIn(typeof(SweptIMDNewTrace))]
    [Display(
        "Swept IMD Single Trace",
        Groups: new[] { "Network Analyzer", "Converters", "Swept IMD Converters" },
        Description: "Insert a description here"
    )]
    public class SweptIMDSingleTrace : SingleTraceBaseStep
    {
        #region Settings
        private SweptIMDTraceEnum _Meas;

        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public SweptIMDTraceEnum Meas
        {
            get { return _Meas; }
            set
            {
                _Meas = value;
                measEnumName = value.ToString();
                IsConverter = true;
                UpdateTestStepName();
            }
        }

        [Display("Class", Groups: new[] { "Trace" }, Order: 12)]
        public TraceManagerChannelClassEnum Class { get; set; }

        #endregion

        public SweptIMDSingleTrace()
        {
            Meas = SweptIMDTraceEnum.Pwr2Hi;
            Class = TraceManagerChannelClassEnum.IMDX;
            measClass = "Swept IMD Converters";
        }
    }
}
