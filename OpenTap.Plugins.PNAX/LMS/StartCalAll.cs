using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OpenTap; // Use OpenTAP infrastructure/core components (log,TestStep definition, etc)

namespace OpenTap.Plugins.PNAX
{
    [Display(
        "Start Cal All",
        Groups: new[] { "Network Analyzer", "Load/Measure/Store" },
        Description: "Starts Cal All on the instrument"
    )]
    public class StartCalAll : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        #endregion

        public StartCalAll() { }

        public override void Run()
        {
            UpgradeVerdict(Verdict.NotSet);

            PNAX.CalAllStartRemotely();

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
