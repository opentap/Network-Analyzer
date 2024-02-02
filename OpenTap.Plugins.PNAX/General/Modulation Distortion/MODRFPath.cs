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
    [AllowAsChildIn(typeof(MODXChannel))]
    [Display("MOD RF Path", Groups: new[] { "Network Analyzer", "General", "Modulation Distortion" }, Description: "Insert a description here")]
    public class MODRFPath : PNABaseStep
    {
        #region Settings
        [Display("Include Source Attenuator", Group: "Settings", Order: 10)]
        public bool IncludeSourceAtt { get; set; }

        [EnabledIf("IncludeSourceAtt", true, HideIfDisabled = true)]
        [Display("Source Attenuator", Group: "Settings", Order: 11)]
        [Unit("dB", UseEngineeringPrefix: false, StringFormat: "0")]
        public double SourceAttenuator { get; set; }

        [Display("Nominal Source Amplifier", Group: "Settings", Order: 12)]
        [Unit("dB", UseEngineeringPrefix: false, StringFormat: "0.00")]
        public double NominalSourceAmp { get; set; }

        [Display("DUT Input", Group: "Settings", Order: 13)]
        public MODDutPortEnum DUTInput { get; set; }

        [Display("Nominal DUT Gain", Group: "Settings", Order: 14)]
        [Unit("dB", UseEngineeringPrefix: false, StringFormat: "0.00")]
        public double NominalDUTGain { get; set; }

        [Display("DUT Output", Group: "Settings", Order: 15)]
        public MODDutPortEnum DUTOutput { get; set; }

        [Display("Receiver Attenuator", Group: "Settings", Order: 16)]
        [Unit("dB", UseEngineeringPrefix: false, StringFormat: "0")]
        public double ReceiverAttenuator { get; set; }

        [Display("Include Nominal DUT NF", Group: "Settings", Order: 17)]
        public bool IncludeNominalDUTNF { get; set; }

        [EnabledIf("IncludeNominalDUTNF", true, HideIfDisabled = true)]
        [Display("Nominal DUT NF", Group: "Settings", Order: 18)]
        [Unit("dB", UseEngineeringPrefix: false, StringFormat: "0.00")]
        public double NominalDUTNF { get; set; }

        #endregion

        public MODRFPath()
        {
            IncludeSourceAtt = true;
            SourceAttenuator = 0;
            NominalSourceAmp = 0;
            DUTInput = MODDutPortEnum.Port1;
            NominalDUTGain = 0;
            DUTOutput = MODDutPortEnum.Port2;
            ReceiverAttenuator = 18;
            IncludeNominalDUTNF = false;
            NominalDUTNF = 0;
        }

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            List<(string, object)> retVal = new List<(string, object)>();
            retVal.Add(("IncludeSourceAtt", IncludeSourceAtt));
            retVal.Add(("SourceAttenuator", SourceAttenuator));
            retVal.Add(("NominalSourceAmp", NominalSourceAmp));
            retVal.Add(("DUTInput", DUTInput));
            retVal.Add(("NominalDUTGain", NominalDUTGain));
            retVal.Add(("DUTOutput", DUTOutput));
            retVal.Add(("ReceiverAttenuator", ReceiverAttenuator));
            retVal.Add(("IncludeNominalDUTNF", IncludeNominalDUTNF));
            retVal.Add(("NominalDUTNF", NominalDUTNF));

            return retVal;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.MODSourceAttenuatorInclude(Channel, IncludeSourceAtt);
            if (IncludeSourceAtt) PNAX.MODSourceAttenuator(Channel, SourceAttenuator, DUTInput);
            PNAX.MODNominalSource(Channel, NominalSourceAmp);
            PNAX.MODDutInput(Channel, DUTInput);
            PNAX.MODNominalDUTGain(Channel, NominalDUTGain);
            PNAX.MODDutOutput(Channel, DUTOutput);
            PNAX.MODReceiverAttenuator(Channel, ReceiverAttenuator, DUTOutput);
            PNAX.MODNominalDUTNFInclude(Channel, IncludeNominalDUTNF);
            if (IncludeNominalDUTNF) PNAX.MODNominalDUTNF(Channel, NominalDUTNF);


            UpgradeVerdict(Verdict.Pass);
        }
    }
}
