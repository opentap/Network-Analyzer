using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OpenTap.Plugins.PNAX
{
    [Display("Auto Scale", Groups: new[] { "Network Analyzer" }, Description: "Auto scale window")]
    public class AutoScale : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display("Auto Select All Windows", Group: "Measurements", Order: 1)]
        public bool AutoSelectWindows { get; set; }

        [EnabledIf("AutoSelectWindows", false, HideIfDisabled = true)]
        [Display("Windows", Description: "Choose which channels to trigger.", "Measurements", Order: 2)]
        public List<int> windows { get; set; }
        #endregion

        public AutoScale()
        {
            windows = new List<int>() { 1 };
            AutoSelectWindows = true;
        }

        public void AutoSelectChannelsAvailableOnInstrument()
        {
            if (AutoSelectWindows)
            {
                windows = PNAX.GetActiveWindows();
            }
        }

        public override void Run()
        {
            UpgradeVerdict(Verdict.NotSet);
            AutoSelectChannelsAvailableOnInstrument();

            try
            {
                foreach (int window in windows)
                {
                    PNAX.AutoScaleWindow(window);
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
