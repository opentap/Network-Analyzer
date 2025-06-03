using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OpenTap.Plugins.PNAX
{
    [Display("Window Size", Groups: new[] { "Network Analyzer" }, Description: "Set and read the display size. The settings are minimum (minimizes the display), maximum, and normal.")]
    public class WindowSize : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display("Window", Description: "Choose which window to set the display size.", Order: 2)]
        public int wnum { get; set; }

        [Display("Window Size", Order: 3)]
        public VNAWindowSize windowSize { get; set; }


        #endregion

        public WindowSize()
        {
            wnum = 1;
            windowSize = VNAWindowSize.NORM;
        }

        public override void Run()
        {
            PNAX.DisplayWindowSize(wnum, windowSize);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
