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
    public enum StandardSweepTypeEnum
    {
        [Display("Linear Frequency")]
        LinearFrequency,
        [Display("Log Frequency")]
        LogFrequency,
        [Display("Power Sweep")]
        PowerSweep,
        [Display("CW Time")]
        CWTime,
        [Display("Segment Sweep")]
        SegmentSweep,
        [Display("Phase Sweep")]
        PhaseSweep
    }

    [AllowAsChildIn(typeof(StandardChannel))]
    [Display("Sweep Type", Groups: new[] { "PNA-X", "General",  "Standard" }, Description: "Insert a description here")]
    public class SweepType : TestStep
    {
        #region Settings
        [Browsable(false)]
        public bool EnableStartStop { get; set; }
        [Browsable(false)]
        public bool EnablePower { get; set; }
        [Browsable(false)]
        public bool EnablePoints { get; set; }
        [Browsable(false)]
        public bool EnableIFBandwidth { get; set; }
        [Browsable(false)]
        public bool EnableStartStopPower { get; set; }
        [Browsable(false)]
        public bool EnableCWFreq { get; set; }
        [Browsable(false)]
        public bool EnableSegmentTable { get; set; }
        [Browsable(false)]
        public bool EnablePhaseSweep { get; set; }


        private StandardSweepTypeEnum _StandardSweepType;
        [Display("Data Acquisition Mode", Order: 2)]
        public StandardSweepTypeEnum StandardSweepType
        {
            get
            {
                return _StandardSweepType;
            }
            set
            {
                _StandardSweepType = value;

                EnableStartStop = false;
                EnablePower = false;
                EnablePoints = false;
                EnableIFBandwidth = false;
                EnableStartStopPower = false;
                EnableCWFreq = false;
                EnableSegmentTable = false;
                EnablePhaseSweep = false;
                switch (_StandardSweepType)
                {
                    case StandardSweepTypeEnum.LinearFrequency:
                        EnableStartStop = true;
                        EnablePower = true;
                        EnablePoints = true;
                        EnableIFBandwidth = true;
                        break;
                    case StandardSweepTypeEnum.LogFrequency:
                        EnableStartStop = true;
                        EnablePower = true;
                        EnablePoints = true;
                        EnableIFBandwidth = true;
                        break;
                    case StandardSweepTypeEnum.PowerSweep:
                        EnableStartStopPower = true;
                        EnableCWFreq = true;
                        EnablePoints = true;
                        EnableIFBandwidth = true;
                        break;
                    case StandardSweepTypeEnum.CWTime:
                        EnableCWFreq = true;
                        EnablePower = true;
                        EnablePoints = true;
                        EnableIFBandwidth = true;
                        break;
                    case StandardSweepTypeEnum.SegmentSweep:
                        EnableSegmentTable = true;
                        break;
                    case StandardSweepTypeEnum.PhaseSweep:
                        EnablePhaseSweep = true;
                        EnableCWFreq = true;
                        EnablePoints = true;
                        EnableIFBandwidth = true;
                        break;
                }
            }
        }








        [EnabledIf("EnableStartStop", true, HideIfDisabled = true)]
        [Display("Start", Group: "Sweep Properties", Order: 20)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double SweepPropertiesStart { get; set; }

        [EnabledIf("EnableStartStop", true, HideIfDisabled = true)]
        [Display("Stop", Group: "Sweep Properties", Order: 21)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepPropertiesStop { get; set; }

        [EnabledIf("EnableStartStopPower", true, HideIfDisabled = true)]
        [Display("Start Power", Group: "Sweep Properties", Order: 20)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double SweepPropertiesStartPower { get; set; }

        [EnabledIf("EnableStartStopPower", true, HideIfDisabled = true)]
        [Display("Stop Power", Group: "Sweep Properties", Order: 21)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double SweepPropertiesStopPower { get; set; }

        [EnabledIf("EnablePhaseSweep", true, HideIfDisabled = true)]
        [Display("Start Phase", Group: "Sweep Properties", Order: 20)]
        [Unit("°", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double SweepPropertiesStartPhase { get; set; }

        [EnabledIf("EnablePhaseSweep", true, HideIfDisabled = true)]
        [Display("Stop Phase", Group: "Sweep Properties", Order: 21)]
        [Unit("°", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double SweepPropertiesStopPhase { get; set; }

        [EnabledIf("EnableCWFreq", true, HideIfDisabled = true)]
        [Display("CW Freq", Group: "Sweep Properties", Order: 22)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepPropertiesCWFreq { get; set; }

        [EnabledIf("EnablePower", true, HideIfDisabled = true)]
        [Display("Power", Group: "Sweep Properties", Order: 22)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double SweepPropertiesPower { get; set; }

        [EnabledIf("EnablePoints", true, HideIfDisabled = true)]
        [Display("Points", Group: "Sweep Properties", Order: 23)]
        public int SweepPropertiesPoints { get; set; }

        [EnabledIf("EnableIFBandwidth", true, HideIfDisabled = true)]
        [Display("IF Bandwidth", Group: "Sweep Properties", Order: 24)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double SweepPropertiesIFBandwidth { get; set; }


        [EnabledIf("EnableSegmentTable", true, HideIfDisabled = true)]
        [Display("Segment Table", Group: "Sweep Properties", Order: 25)]
        public String SegmentTable { get; set; }






        #endregion

        public SweepType()
        {
            StandardSweepType = StandardSweepTypeEnum.LinearFrequency;
            SweepPropertiesStart = 10e6;
            SweepPropertiesStop = 67e9;
            SweepPropertiesStartPower = -10;
            SweepPropertiesStopPower = 0;
            SweepPropertiesStartPhase = 0;
            SweepPropertiesStopPhase = 0;
            SweepPropertiesCWFreq = 1e9;
            SweepPropertiesPower = -5;
            SweepPropertiesPoints = 201;
            SweepPropertiesIFBandwidth = 100e3;
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
