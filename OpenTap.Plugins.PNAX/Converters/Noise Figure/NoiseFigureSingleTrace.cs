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

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Trace Format", Groups: new[] { "Trace" }, Order: 30)]
        public override void AddTraceFormat()
        {
            this.ChildTestSteps.Add(new TraceFormat() { PNAX = this.PNAX, Channel = this.Channel });
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Trace Title", Groups: new[] { "Trace" }, Order: 40)]
        public override void AddTraceTitle()
        {
            this.ChildTestSteps.Add(new TraceTitle() { PNAX = this.PNAX, Channel = this.Channel });
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Marker", Groups: new[] { "Trace" }, Order: 50)]
        public override void AddMarker()
        {
            this.ChildTestSteps.Add(new Marker() { PNAX = this.PNAX, Channel = this.Channel, mkr = NextMarker() });
        }

        [Browsable(true)]
        [EnabledIf("EnableTraceSettings", true, HideIfDisabled = true)]
        [Display("Add Trace Limits", Groups: new[] { "Trace" }, Order: 60)]
        public override void AddTraceLimits()
        {
            this.ChildTestSteps.Add(new TraceLimits() { PNAX = this.PNAX, Channel = this.Channel });
        }

        public override void Run()
        {
            int _tnum = 0;
            int _mnum = 0;
            String _MeasName = "";
            String m = Scpi.Format("{0}", Meas);
            PNAX.AddNewTrace(Channel, Window, Trace, "Noise Figure Converters", m, ref _tnum, ref _mnum, ref _MeasName);
            tnum = _tnum;
            mnum = _mnum;
            MeasName = _MeasName;

            RunChildSteps(); //If the step supports child steps.

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
