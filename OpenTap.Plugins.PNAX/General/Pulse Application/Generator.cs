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
    //[AllowAsChildIn(typeof(PulseGenerators))]
    [Display("Generator Setup", Groups: new[] { "Network Analyzer", "General" }, Description: "Insert a description here")]
    public class Generator : PNABaseStep
    {
        #region Settings
        [Browsable(false)]
        public bool IsInvertEnabled { get; set; }

        [Display("Pulse Name", Group: "Generator Setup", Order: 20)]
        public string PulseName
        {
            get
            {
                return this.Name;
            }
            set
            {
                IsInvertEnabled = true;
                if (value.Equals("Pulse0"))
                {
                    IsInvertEnabled = false;
                }
                this.Name = value;
            }
        }

        [Display("Width", Group: "Generator Setup", Order: 21)]
        [Unit("sec", UseEngineeringPrefix: true, StringFormat: "0.0000")]
        public double PulseWidth { get; set; }

        [Display("Delay", Group: "Generator Setup", Order: 22)]
        [Unit("sec", UseEngineeringPrefix: true, StringFormat: "0.0000")]
        public double PulseDelay { get; set; }

        [EnabledIf("IsInvertEnabled", true, HideIfDisabled = true)]
        [Display("Invert", Group: "Generator Setup", Order: 23)]
        public bool PulseInvert { get; set; }

        [Display("Enable", Group: "Generator Setup", Order: 24)]
        public bool PulseEnable { get; set; }
        #endregion

        public Generator()
        {
            PulseName = "Pulse1";
            PulseWidth = 100e-6;
            PulseDelay = 0;
            PulseInvert = false;
            PulseEnable = false;
        }

        public override void Run()
        {
            PNAX.PulseGeneratorWidth(Channel, PulseName, PulseWidth);
            PNAX.PulseGeneratorDelay(Channel, PulseName, PulseDelay);
            if (IsInvertEnabled) PNAX.PulseGeneratorInvert(Channel, PulseName, PulseInvert);
            PNAX.PulseGeneratorEnable(Channel, PulseName, PulseEnable);

            UpgradeVerdict(Verdict.Pass);
        }

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            List<(string, object)> retVal = new List<(string, object)>();

            retVal.Add(($"PulseName", PulseName));

            retVal.Add(($"{PulseName} PulseWidth", PulseWidth));
            retVal.Add(($"{PulseName} PulseDelay", PulseDelay));
            retVal.Add(($"{PulseName} PulseInvert", PulseInvert));
            retVal.Add(($"{PulseName} PulseEnable", PulseEnable));
            return retVal;
        }

    }
}
