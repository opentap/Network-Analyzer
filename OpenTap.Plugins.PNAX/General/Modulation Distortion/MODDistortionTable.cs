// Author: CMontes
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
    [Display("MOD Distortion Table", Groups: new[] { "Network Analyzer", "General", "Modulation Distortion" }, Description: "Insert a description here")]
    public class MODDistortionTable : PNABaseStep
    {
        #region Settings
        [Display("Window", Group: "Settings", Order: 20)]
        public int Window { get; set; }

        [Display("Show Table", Group: "Settings", Order: 21)]
        public bool ShowTable { get; set; }

        [Display("Carrier", Group: "Settings", Order: 22)]
        public MODTableSetupCarrierEnum MODTableSetupCarrier { get; set; }

        [Display("EVM", Group: "Settings", Order: 23)]
        public MODTableSetupEVMEnum MODTableSetupEVM { get; set; }

        [Display("NPR", Group: "Settings", Order: 24)]
        public MODTableSetupNPREnum MODTableSetupNPR { get; set; }

        [Display("ACP", Group: "Settings", Order: 25)]
        public MODTableSetupACPEnum MODTableSetupACP { get; set; }

        [Display("ACP Avg", Group: "Settings", Order: 26)]
        public MODTableSetupACPAvgEnum MODTableSetupAvg { get; set; }
        #endregion

        public MODDistortionTable()
        {
            Window = 1;
            ShowTable = true;
            MODTableSetupCarrier = MODTableSetupCarrierEnum.CarrierIn1dBm | MODTableSetupCarrierEnum.CarrierOut2dBm | MODTableSetupCarrierEnum.CarrierGain21dB | MODTableSetupCarrierEnum.CarrierIBW;
            MODTableSetupEVM = MODTableSetupEVMEnum.EVMDistEq21;
            MODTableSetupNPR = MODTableSetupNPREnum.NPROut2dBc;
            MODTableSetupACP = MODTableSetupACPEnum.ACPUpIn1dBc | MODTableSetupACPEnum.ACPUpOut2dBc;
            MODTableSetupAvg = 0;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.MODShowTable(Channel, ShowTable);
            PNAX.MODTableAddParameter(Channel, MODTableSetupCarrier);
            PNAX.MODTableAddParameter(Channel, MODTableSetupEVM);
            PNAX.MODTableAddParameter(Channel, MODTableSetupNPR);
            PNAX.MODTableAddParameter(Channel, MODTableSetupACP);
            PNAX.MODTableAddParameter(Channel, MODTableSetupAvg);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
