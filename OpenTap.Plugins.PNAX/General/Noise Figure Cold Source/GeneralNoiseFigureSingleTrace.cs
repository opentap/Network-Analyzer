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
    public enum GeneralNoiseFigureTraceEnum
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
        [Scpi("S21")]
        S21,
        [Scpi("S12")]
        S12,
        [Scpi("S22")]
        S22,
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

    [AllowAsChildIn(typeof(GeneralNoiseFigureNewTrace))]
    [AllowAsChildIn(typeof(GeneralNoiseFigureChannel))]
    [Display("Noise Figure Single Trace", Groups: new[] { "PNA-X", "Converters", "Noise Figure Converters" }, Description: "Insert a description here")]
    public class GeneralNoiseFigureSingleTrace : SingleTraceBaseStep
    {
        #region Settings
        private GeneralNoiseFigureTraceEnum _Meas;
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public GeneralNoiseFigureTraceEnum Meas
        {
            get { return _Meas; }
            set
            {
                _Meas = value;
                measEnumName = value.ToString();
                UpdateTestName();
            }
        }
        #endregion

        public GeneralNoiseFigureSingleTrace()
        {
            Meas = GeneralNoiseFigureTraceEnum.NF;
            measClass = "Noise Figure Cold Source";
        }

        protected override void UpdateTestName()
        {
            string m = Scpi.Format("{0}", Meas);
            this.Trace = $"CH{Channel}_{m}";
            this.Name = $"CH{Channel}_{m}";
        }

    }
}
