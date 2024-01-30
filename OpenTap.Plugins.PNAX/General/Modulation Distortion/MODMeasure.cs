// Author: MyName
// Copyright:   Copyright 2024 Keysight Technologies
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
    [AllowAsChildIn(typeof(TestPlan))]
    [AllowAsChildIn(typeof(MODChannel))]
    [Display("MOD Measure", Groups: new[] { "Network Analyzer", "General", "Modulation Distortion" }, Description: "Insert a description here")]
    public class MODMeasure : PNABaseStep
    {
        #region Settings
        [Display("Measurement Type", Group: "Settings", Order: 21)]
        public MODMeasurementTypeEnum MODMeasurementType { get; set; }

        [Display("Autofill", Group: "Settings", Order: 21.1)]
        public bool Autofill { get; set; }

        [EnabledIf("Autofill", false, HideIfDisabled = true)]
        [Display("Carrier Offset Freq", Group: "Settings", Order: 22)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0")]
        public double MODCarrierOffset { get; set; }
        [EnabledIf("Autofill", false, HideIfDisabled = true)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        [Display("Carrier Integration BW", Group: "Settings", Order: 23)]
        public double MODCarrierIBW { get; set; }

        [EnabledIf("Autofill", false, HideIfDisabled = true)]
        [EnabledIf("MODMeasurementType", MODMeasurementTypeEnum.ACP, MODMeasurementTypeEnum.ACPEVM, HideIfDisabled = true)]
        [Display("ACP Lower Offset Freq", Group: "Settings", Order: 24)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double MODACPLowOffset { get; set; }
        [EnabledIf("Autofill", false, HideIfDisabled = true)]
        [EnabledIf("MODMeasurementType", MODMeasurementTypeEnum.ACP, MODMeasurementTypeEnum.ACPEVM, HideIfDisabled = true)]
        [Display("ACP Lower Integration BW", Group: "Settings", Order: 25)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double MODACPLowIBW { get; set; }

        [EnabledIf("Autofill", false, HideIfDisabled = true)]
        [EnabledIf("MODMeasurementType", MODMeasurementTypeEnum.ACP, MODMeasurementTypeEnum.ACPEVM, HideIfDisabled = true)]
        [Display("ACP Upper Offset Freq", Group: "Settings", Order: 26)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double MODACPUpperOffset { get; set; }
        [EnabledIf("Autofill", false, HideIfDisabled = true)]
        [EnabledIf("MODMeasurementType", MODMeasurementTypeEnum.ACP, MODMeasurementTypeEnum.ACPEVM, HideIfDisabled = true)]
        [Display("ACP Upper Integration BW", Group: "Settings", Order: 27)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double MODACPUpperIBW { get; set; }

        [EnabledIf("Autofill", false, HideIfDisabled = true)]
        [EnabledIf("MODMeasurementType", MODMeasurementTypeEnum.NPR, HideIfDisabled = true)]
        [Display("Notch Offset Freq", Group: "Settings", Order: 26)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0")]
        public double MODNPROffset { get; set; }
        [EnabledIf("Autofill", false, HideIfDisabled = true)]
        [EnabledIf("MODMeasurementType", MODMeasurementTypeEnum.NPR, HideIfDisabled = true)]
        [Display("Notch Integration BW", Group: "Settings", Order: 27)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double MODNPRIBW { get; set; }

        #endregion

        public MODMeasure()
        {
            MODMeasurementType = MODMeasurementTypeEnum.BPWR;
            Autofill = false;

            MODCarrierOffset = 0;
            MODCarrierIBW = 100e6;

            MODACPLowOffset = -100e6;
            MODACPLowIBW = 100e6;
            MODACPUpperOffset = 100e6;
            MODACPUpperIBW = 100e6;

            MODNPROffset = 0;
            MODNPRIBW = 10e6;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.MODSetMeasType(Channel, MODMeasurementType);

            if (Autofill)
            {
                PNAX.MODAutofill(Channel);
            }
            else
            {
                PNAX.MODSetOffset(Channel, MODCarrierOffset, MODMeasConfigTypeEnum.CARRier);
                PNAX.MODSetIBW(Channel, MODCarrierIBW, MODMeasConfigTypeEnum.CARRier);

                if ((MODMeasurementType == MODMeasurementTypeEnum.ACP) ||
                    (MODMeasurementType == MODMeasurementTypeEnum.ACPEVM))
                {
                    PNAX.MODSetOffset(Channel, MODACPLowOffset, MODMeasConfigTypeEnum.ACPLower);
                    PNAX.MODSetIBW(Channel, MODACPLowIBW, MODMeasConfigTypeEnum.ACPLower);

                    PNAX.MODSetOffset(Channel, MODACPUpperOffset, MODMeasConfigTypeEnum.ACPUpper);
                    PNAX.MODSetIBW(Channel, MODACPUpperIBW, MODMeasConfigTypeEnum.ACPUpper);
                }
                else if ((MODMeasurementType == MODMeasurementTypeEnum.NPR))
                {
                    PNAX.MODSetOffset(Channel, MODNPROffset, MODMeasConfigTypeEnum.Notch);
                    PNAX.MODSetIBW(Channel, MODNPRIBW, MODMeasConfigTypeEnum.Notch);
                }
            }
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
