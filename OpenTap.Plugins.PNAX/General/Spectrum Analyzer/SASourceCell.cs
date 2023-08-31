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

namespace OpenTap.Plugins.PNAX.General.Spectrum_Analyzer
{
    [AllowAsChildIn(typeof(SASource))]
    [Display("SA Source Cell", Groups: new[] { "PNA-X", "General", "Spectrum Analyzer" }, Description: "Insert a description here")]
    public class SASourceCell : GeneralBaseStep
    {
        #region Settings
        [Display("Name", Group: "Sweep Properties", Order: 20)]
        public String CellName
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }

        [Display("State", Group: "Sweep Properties", Order: 21)]
        public SASourceStateTypeEnum State { get; set; }

        [Display("Type", Group: "Sweep Properties", Order: 22)]
        public SASourceSweepTypeEnum SASourceSweepType { get; set; }



        [EnabledIf("SASourceSweepType", SASourceSweepTypeEnum.CWTime, SASourceSweepTypeEnum.PowerSweep, HideIfDisabled = true)]
        [Display("CW Freq", Groups: new[] { "Sweep Properties", "Frequency" }, Order: 30)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepPropertiesCWFreq { get; set; }

        [EnabledIf("SASourceSweepType", SASourceSweepTypeEnum.CWTime, SASourceSweepTypeEnum.LinearFrequency, HideIfDisabled = true)]
        [Display("Power Level", Groups: new[] { "Sweep Properties", "Power" }, Order: 40)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "00.00")]
        public double SweepPropertiesPowerLevel { get; set; }

        [EnabledIf("SASourceSweepType", SASourceSweepTypeEnum.CWTime, SASourceSweepTypeEnum.LinearFrequency, SASourceSweepTypeEnum.PowerSweep, SASourceSweepTypeEnum.LinFPower, HideIfDisabled = true)]
        [Display("Phase Level", Groups: new[] { "Sweep Properties", "Phase" }, Order: 50)]
        [Unit("°", UseEngineeringPrefix: true, StringFormat: "0.0")]
        public double SweepPropertiesPhaseLevel { get; set; }


        [EnabledIf("SASourceSweepType", SASourceSweepTypeEnum.LinearFrequency, SASourceSweepTypeEnum.LinFPower, HideIfDisabled = true)]
        [Display("Start Frequency", Groups: new[] { "Sweep Properties", "Frequency" }, Order: 31)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double SweepPropertiesFreqStart { get; set; }

        [EnabledIf("SASourceSweepType", SASourceSweepTypeEnum.LinearFrequency, SASourceSweepTypeEnum.LinFPower, HideIfDisabled = true)]
        [Display("Stop Frequency", Groups: new[] { "Sweep Properties", "Frequency" }, Order: 32)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepPropertiesFreqStop { get; set; }

        [EnabledIf("SASourceSweepType", SASourceSweepTypeEnum.LinearFrequency, SASourceSweepTypeEnum.LinFPower, HideIfDisabled = true)]
        [Display("Source Number of Steps", Groups: new[] { "Sweep Properties", "Frequency" }, Order: 33)]
        public int SweepPropertiesFreqNumberOfSteps { get; set; }

        [EnabledIf("SASourceSweepType", SASourceSweepTypeEnum.LinearFrequency, SASourceSweepTypeEnum.LinFPower, HideIfDisabled = true)]
        [Display("SA Sweeps per Source Steps", Groups: new[] { "Sweep Properties", "Frequency" }, Order: 34)]
        public int SweepPropertiesFreqSweepsPerSourceSteps { get; set; }


        [EnabledIf("SASourceSweepType", SASourceSweepTypeEnum.PowerSweep, SASourceSweepTypeEnum.LinFPower, HideIfDisabled = true)]
        [Display("Start Power", Groups: new[] { "Sweep Properties", "Power" }, Order: 41)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "00.00")]
        public double SweepPropertiesPowerStart { get; set; }

        [EnabledIf("SASourceSweepType", SASourceSweepTypeEnum.PowerSweep, SASourceSweepTypeEnum.LinFPower, HideIfDisabled = true)]
        [Display("Stop Power", Groups: new[] { "Sweep Properties", "Power" }, Order: 42)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "00.00")]
        public double SweepPropertiesPowerStop { get; set; }

        [EnabledIf("SASourceSweepType", SASourceSweepTypeEnum.PowerSweep, SASourceSweepTypeEnum.LinFPower, HideIfDisabled = true)]
        [Display("Source Number of Steps", Groups: new[] { "Sweep Properties", "Power" }, Order: 43)]
        public int SweepPropertiesPowerNumberOfSteps { get; set; }

        [EnabledIf("SASourceSweepType", SASourceSweepTypeEnum.PowerSweep, SASourceSweepTypeEnum.LinFPower, HideIfDisabled = true)]
        [Display("SA Sweeps per Source Steps", Groups: new[] { "Sweep Properties", "Power" }, Order: 44)]
        public int SweepPropertiesPowerSweepsPerSourceSteps { get; set; }

        #endregion

        public SASourceCell()
        {
            State = SASourceStateTypeEnum.Off;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.
            PNAX.SetSASourcePowerMode(Channel, CellName, State);
            PNAX.SetSASweepType(Channel, CellName, SASourceSweepType);


            UpgradeVerdict(Verdict.Pass);
        }
    }
}
