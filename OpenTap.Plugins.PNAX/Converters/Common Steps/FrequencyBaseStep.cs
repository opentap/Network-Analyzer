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
    public enum SweepTypeEnum
    {
        [Scpi("LIN")]
        [Display("Linear Sweep")]
        LinearSweep,
        [Scpi("CW")]
        [Display("CW Frequency")]
        CWFrequency
    }

    public enum DataAcquisitionModeEnum
    {
        [Scpi("SMAR")]
        [Display("SMART Sweep")]
        SMARTSweep,
        [Scpi("PFREQ")]
        [Display("Sweep Power Per Frequency 2D")]
        SweepPowerPerFrequency2D,
        [Scpi("FPOW")]
        [Display("Sweep Frequency Per Power 2D")]
        SweepFrequencyPerPower2D
    }

    [Browsable(false)]
    public class FrequencyBaseStep : ConverterBaseStep
    {
        #region Settings

        [Display("Sweep Type", Order: 1)]
        public SweepTypeEnum SweepType { get; set; }


        [Display("Number Of Points", Group: "Sweep Settings", Order: 10)]
        public int SweepSettingsNumberOfPoints { get; set; }

        [Display("IF Bandwidth", Group: "Sweep Settings", Order: 11)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double SweepSettingsIFBandwidth { get; set; }

        [EnabledIf("SweepType", SweepTypeEnum.LinearSweep, HideIfDisabled =true)]
        [Display("Start", Group: "Sweep Settings", Order: 12)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double SweepSettingsStart { get; set; }

        [EnabledIf("SweepType", SweepTypeEnum.LinearSweep, HideIfDisabled = true)]
        [Display("Stop", Group: "Sweep Settings", Order: 13)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsStop { get; set; }

        [EnabledIf("SweepType", SweepTypeEnum.LinearSweep, HideIfDisabled = true)]
        [Display("Center", Group: "Sweep Settings", Order: 14)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsCenter { get; set; }

        [EnabledIf("SweepType", SweepTypeEnum.LinearSweep, HideIfDisabled = true)]
        [Display("Span", Group: "Sweep Settings", Order: 15)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsSpan { get; set; }

        [EnabledIf("SweepType", SweepTypeEnum.CWFrequency, HideIfDisabled = true)]
        [Display("Fixed", Group: "Sweep Settings", Order: 16)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsFixed { get; set; }

        #endregion

        public FrequencyBaseStep()
        {
        }



        public override void Run()
        {
        }
    }
}
