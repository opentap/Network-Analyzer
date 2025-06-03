using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using OpenTap;

namespace OpenTap.Plugins.PNAX
{
    [Display(
        "External Source State",
        Groups: new[] { "Network Analyzer" },
        Description: "Set and return the state of activation of the device. When SYST:CONF:EDEV:IOEN = ON, and this command is set to ON, the VNA will attempt communication with the external device."
            + "Send this command AFTER sending other external device settings (especially SYST:CONF:EDEV:DTYP) to avoid communicating with the device before it has been fully configured."
            + "See Also: SYST:PREF:ITEM:EDEV:DPOL - Determines whether external devices remain activated or are de-activated when the VNA is Preset or when a Instrument State is recalled."
    )]
    public class ExternalSourceState : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display("State", Group: "External source", Order: 10)]
        public bool ExtSourceState { get; set; }

        [Display("External Source Name", Group: "External source", Order: 11)]
        public string ExtSourceName { get; set; }
        #endregion

        public ExternalSourceState()
        {
            ExtSourceState = true;
            ExtSourceName = "Device0";
        }

        public override void Run()
        {
            PNAX.EnableExternalSource(ExtSourceName, ExtSourceState);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
