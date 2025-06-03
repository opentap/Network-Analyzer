// Author: CMontes
// Copyright:   Copyright 2024 Keysight Technologies
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
    //[AllowAsChildIn(typeof(TestPlan))]
    //[AllowAsChildIn(typeof(MODChannel))]
    //[AllowAsChildIn(typeof(MODXChannel))]
    [Display(
        "MOD Measure",
        Groups: new[] { "Network Analyzer", "General", "Modulation Distortion" },
        Description: "Insert a description here"
    )]
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
        [EnabledIf(
            "MODMeasurementType",
            MODMeasurementTypeEnum.ACP,
            MODMeasurementTypeEnum.ACPEVM,
            HideIfDisabled = true
        )]
        [Display("ACP Lower Offset Freq", Group: "Settings", Order: 24)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double MODACPLowOffset { get; set; }

        [EnabledIf("Autofill", false, HideIfDisabled = true)]
        [EnabledIf(
            "MODMeasurementType",
            MODMeasurementTypeEnum.ACP,
            MODMeasurementTypeEnum.ACPEVM,
            HideIfDisabled = true
        )]
        [Display("ACP Lower Integration BW", Group: "Settings", Order: 25)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double MODACPLowIBW { get; set; }

        [EnabledIf("Autofill", false, HideIfDisabled = true)]
        [EnabledIf(
            "MODMeasurementType",
            MODMeasurementTypeEnum.ACP,
            MODMeasurementTypeEnum.ACPEVM,
            HideIfDisabled = true
        )]
        [Display("ACP Upper Offset Freq", Group: "Settings", Order: 26)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double MODACPUpperOffset { get; set; }

        [EnabledIf("Autofill", false, HideIfDisabled = true)]
        [EnabledIf(
            "MODMeasurementType",
            MODMeasurementTypeEnum.ACP,
            MODMeasurementTypeEnum.ACPEVM,
            HideIfDisabled = true
        )]
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

        [Display("Measurement Details", Group: "Measurement Details", Order: 30)]
        public bool EnableMeasurementDetails { get; set; }

        [EnabledIf("EnableMeasurementDetails", true, HideIfDisabled = true)]
        [Display("Equalization Aperture Auto", Group: "Measurement Details", Order: 31)]
        public bool EqualizationApertureAuto { get; set; }

        [EnabledIf("EnableMeasurementDetails", true, HideIfDisabled = true)]
        [EnabledIf("EqualizationApertureAuto", false, HideIfDisabled = true)]
        [Display("Equalization Aperture", Group: "Measurement Details", Order: 32)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double EqualizationAperture { get; set; }

        [EnabledIf("EnableMeasurementDetails", true, HideIfDisabled = true)]
        [Display("ADC Anti-alias Filter", Group: "Measurement Details", Order: 33)]
        public MODAntialiasFilterEnum MODAntialiasFilter { get; set; }

        [EnabledIf("EnableMeasurementDetails", true, HideIfDisabled = true)]
        [Display("EVM Normalization", Group: "Measurement Details", Order: 34)]
        public double EVMNormalization { get; set; }

        [EnabledIf("EnableMeasurementDetails", true, HideIfDisabled = true)]
        [Display("Modulation Filter", Group: "Measurement Details", Order: 35)]
        public MODFilterEnum MODFilter { get; set; }

        [EnabledIf("EnableMeasurementDetails", true, HideIfDisabled = true)]
        [EnabledIf("MODFilter", MODFilterEnum.RRC, HideIfDisabled = true)]
        [Display("Symbol Rate Auto", Group: "Measurement Details", Order: 36)]
        public bool SymbolRateAuto { get; set; }

        [EnabledIf("EnableMeasurementDetails", true, HideIfDisabled = true)]
        [EnabledIf("MODFilter", MODFilterEnum.RRC, HideIfDisabled = true)]
        [EnabledIf("SymbolRateAuto", false, HideIfDisabled = true)]
        [Display("Alpha", Group: "Measurement Details", Order: 37)]
        public int MODAlpha { get; set; }

        [EnabledIf("EnableMeasurementDetails", true, HideIfDisabled = true)]
        [EnabledIf("MODFilter", MODFilterEnum.RRC, HideIfDisabled = true)]
        [EnabledIf("SymbolRateAuto", false, HideIfDisabled = true)]
        [Display("Symbol Rate", Group: "Measurement Details", Order: 38)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double SymbolRate { get; set; }

        [EnabledIf("EnableMeasurementDetails", true, HideIfDisabled = true)]
        [Display("DUT NF", Group: "Measurement Details", Order: 39)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0")]
        public double DutNF { get; set; }

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

            Rules.Add(
                () => ((EVMNormalization >= 0.1) && (EVMNormalization <= 1)),
                "EVM normalization must be between 0.1 and 1",
                nameof(EVMNormalization)
            );
            Rules.Add(
                () => ((MODAlpha >= 0) && (MODAlpha <= 1)),
                "Alpha must be between 0 and 1",
                nameof(MODAlpha)
            );
            EnableMeasurementDetails = false;
            EqualizationAperture = 10.24e6;
            EqualizationApertureAuto = true;
            MODAntialiasFilter = MODAntialiasFilterEnum.Auto;
            EVMNormalization = 1;
            MODFilter = MODFilterEnum.None;
            MODAlpha = 0;
            SymbolRate = 10e6;
            SymbolRateAuto = true;
            DutNF = 0;
        }

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            List<(string, object)> retVal = new List<(string, object)>();
            retVal.Add(("MODMeasurementType", MODMeasurementType));
            if (Autofill)
            {
                retVal.Add(("Autofill", Autofill));
            }
            else
            {
                retVal.Add(("MODCarrierOffset", MODCarrierOffset));
                retVal.Add(("MODCarrierIBW", MODCarrierIBW));
                if (
                    (MODMeasurementType == MODMeasurementTypeEnum.ACP)
                    || (MODMeasurementType == MODMeasurementTypeEnum.ACPEVM)
                )
                {
                    retVal.Add(("MODACPLowOffset", MODACPLowOffset));
                    retVal.Add(("MODACPLowIBW", MODACPLowIBW));
                    retVal.Add(("MODACPUpperOffset", MODACPUpperOffset));
                    retVal.Add(("MODACPUpperIBW", MODACPUpperIBW));
                }
                else if ((MODMeasurementType == MODMeasurementTypeEnum.NPR))
                {
                    retVal.Add(("MODNPROffset", MODNPROffset));
                    retVal.Add(("MODNPRIBW", MODNPRIBW));
                }
                if (EnableMeasurementDetails)
                {
                    retVal.Add(("EnableMeasurementDetails", EnableMeasurementDetails));
                    retVal.Add(("EqualizationAperture", EqualizationAperture));
                    retVal.Add(("EqualizationApertureAuto", EqualizationApertureAuto));
                    retVal.Add(("MODAntialiasFilter", MODAntialiasFilter));
                    retVal.Add(("EVMNormalization", EVMNormalization));
                    retVal.Add(("MODFilter", MODFilter));
                    retVal.Add(("MODAlpha", MODAlpha));
                    retVal.Add(("SymbolRate", SymbolRate));
                    retVal.Add(("SymbolRateAuto", SymbolRateAuto));
                    retVal.Add(("DutNF", DutNF));
                }
            }
            return retVal;
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

                if (
                    (MODMeasurementType == MODMeasurementTypeEnum.ACP)
                    || (MODMeasurementType == MODMeasurementTypeEnum.ACPEVM)
                )
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

            if (EnableMeasurementDetails)
            {
                PNAX.MODCorrelationApertureAuto(Channel, EqualizationApertureAuto);
                if (!EqualizationApertureAuto)
                {
                    PNAX.MODCorrelationAperture(Channel, EqualizationAperture);
                }
                PNAX.MODAntialiasFilter(Channel, MODAntialiasFilter);
                PNAX.MODEVMNormalization(Channel, EVMNormalization);
                PNAX.MODFilter(Channel, MODFilter);
                PNAX.MODFilterSymbolRateAuto(Channel, SymbolRateAuto);
                if (!SymbolRateAuto)
                {
                    PNAX.MODFilterAlpha(Channel, MODAlpha);
                    PNAX.MODFilterSymbolRate(Channel, SymbolRate);
                }
                PNAX.MODDutNF(Channel, DutNF);
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
