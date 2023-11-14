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
    [AllowAsChildIn(typeof(GeneralGainCompressionChannel))]
    [Display("Compression", Groups: new[] { "PNA-X", "General", "Gain Compression" }, Description: "Insert a description here")]
    public class GeneralGainCompression : CompressionBaseStep
    {

        #region Settings

        [EnabledIf("SMARTSweepSafeMode", true, HideIfDisabled = true)]
        [Display("DC Parameters", Group: "SMART Sweep", Order: 29)]
        public string SMARTSweepDCParameters { get; set; }

        [EnabledIf("SMARTSweepSafeMode", true, HideIfDisabled = true)]
        [Unit("V", UseEngineeringPrefix: true, StringFormat: "0.000")]
        [Display("Max DC Power", Group: "SMART Sweep", Order: 30)]
        public double SMARTSweepMaxDCPower { get; set; }

        #endregion

        public GeneralGainCompression()
        {
            SMARTSweepDCParameters = "None";
            SMARTSweepMaxDCPower = 0.0;
        }

        protected override void SetDC()
        {
            // DC Parameters
            PNAX.SetSMARTSweepSafeModeDCParameters(Channel, SMARTSweepDCParameters);
            // Max DC Power
            PNAX.SetSMARTSweepSafeModeMaxDCPower(Channel, SMARTSweepMaxDCPower);
        }
    }
}
