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

namespace OpenTap.Plugins.PNAX
{
    public enum NoiseFigureTraceEnum
    {
        [Scpi("NF")]
        NF,
        [Scpi("T-Eff")]
        [Display("T-Eff")]
        TEff,
        [Scpi("ENR")]
        ENR,
        [Scpi("DUTRNP")]
        DUTRNP,
        [Scpi("SYSRNP")]
        SYSRNP,
        [Scpi("DUTNPD")]
        DUTNPD,
        [Scpi("SYSNPD")]
        SYSNPD,
        [Scpi("DUTRNPI")]
        DUTRNPI,
        [Scpi("SYSRNPI")]
        SYSRNPI,
        [Scpi("DUTNPDI")]
        DUTNPDI,
        [Scpi("SYSNPDI")]
        SYSNPDI,
        [Scpi("S11")]
        S11,
        [Scpi("SC21")]
        SC21,
        [Scpi("SC12")]
        SC12,
        [Scpi("S22")]
        S22,
        [Scpi("IPwr")]
        IPwr,
        [Scpi("RevIPwr")]
        RevIPwr,
        [Scpi("RevOPwr")]
        RevOPwr,
        [Scpi("OPwr")]
        OPwr,
        [Scpi("ALO1")]
        ALO1,
        [Scpi("BLO1")]
        BLO1,
        [Scpi("CLO1")]
        CLO1,
        [Scpi("DLO1")]
        DLO1,
        [Scpi("R1LO1")]
        R1LO1,
        [Scpi("R2LO1")]
        R2LO1,
        [Scpi("R3LO1")]
        R3LO1,
        [Scpi("R4LO1")]
        R4LO1,
        [Scpi("R1_1")]
        R1_1,
        [Scpi("R2_2")]
        R2_2,
        [Scpi("A_1")]
        A_1,
        [Scpi("A_2")]
        A_2,
        [Scpi("B_1")]
        B_1,
        [Scpi("B_2")]
        B_2,
        [Scpi("NFmin")]
        NFmin,
        [Scpi("GammaOpt")]
        GammaOpt,
        [Scpi("Rn")]
        Rn,
        [Scpi("NCorr_11")]
        NCorr_11,
        [Scpi("NCorr_12")]
        NCorr_12,
        [Scpi("NCorr_21")]
        NCorr_21,
        [Scpi("NCorr_22")]
        NCorr_22
    }

    [AllowAsChildIn(typeof(NoiseFigureNewTrace))]
    [AllowAsChildIn(typeof(NoiseFigureChannel))]
    [Display("Noise Figure Single Trace", Groups: new[] { "PNA-X", "Converters", "Noise Figure Converters" }, Description: "Insert a description here")]
    public class NoiseFigureSingleTrace : ConverterSingleTraceBaseStep
    {
        #region Settings
        private NoiseFigureTraceEnum _Meas;
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public NoiseFigureTraceEnum Meas
        {
            get { return _Meas; }
            set
            {
                _Meas = value;
                UpdateTestName();
            }
        }
        #endregion

        public NoiseFigureSingleTrace()
        {
            Trace = "1";
            Meas = NoiseFigureTraceEnum.NF;
            Channel = 1;
            Window = 1;
            Sheet = 1;
        }

        protected override void UpdateTestName()
        {
            String m = Scpi.Format("{0}", Meas);
            this.Trace = $"CH{Channel}_{m}";
            this.Name = $"CH{Channel}_{m}";
        }


        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            int traceid = PNAX.GetNewTraceID(Channel);

            // Define the measurement
            String m = Scpi.Format("{0}", Meas);
            PNAX.ScpiCommand($"CALCulate{Channel.ToString()}:CUST:DEFine \'{Trace}\',\'Noise Figure Converters\',\'{m}\'");

            // Create a window if it doesn't exist already
            PNAX.ScpiCommand($"DISPlay:WINDow{Window.ToString()}:STATe ON");

            // Display the measurement
            PNAX.ScpiCommand($"DISPlay:WINDow{Window.ToString()}:TRACe{traceid.ToString()}:FEED \'{Trace}\'");

            // Select the measurement

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
