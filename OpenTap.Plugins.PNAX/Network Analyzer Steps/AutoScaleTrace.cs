using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OpenTap.Plugins.PNAX
{
    [Display("Auto Scale Trace", Groups: new[] { "Network Analyzer" }, Description: "Auto scale window")]
    public class AutoScaleTrace : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display("Use Trace Output", Description: "Select a trace to obtain the values of: Channel, Window, TNUM and MNUM", Groups: new[] { "Measurements" }, Order: 10)]
        public bool UseTraceOutput { get; set; }

        [EnabledIf("UseTraceOutput", true, HideIfDisabled = true)]
        [Display("MNum", Groups: new[] { "Trace" }, Order: 20)]
        public Input<int> mnum { get; set; }

        [EnabledIf("UseTraceOutput", false, HideIfDisabled = true)]
        [Display("MNum Value", Groups: new[] { "Trace" }, Order: 23)]
        public int mnumValue { get; set; }
        #endregion

        public AutoScaleTrace()
        {
            UseTraceOutput = true;

            mnum = new Input<int>();
            mnumValue = 1;
        }

        public override void Run()
        {
            UpgradeVerdict(Verdict.NotSet);

            if (UseTraceOutput)
            {
                if (mnum == null)
                {
                    Log.Error("Make sure to select a trace");
                    UpgradeVerdict(Verdict.Error);
                }

                // Get the values from the input
                SingleTraceBaseStep x = (mnum.Step as SingleTraceBaseStep);

                Log.Info("trace Window: ");
                Log.Info("trace Window: " + x.Window);
                Log.Info("trace Sheet: " + x.Sheet);
                Log.Info("trace tnum: " + x.tnum);
                Log.Info("trace mnum: " + x.mnum);
                Log.Info("trace MeasName: " + x.MeasName);

                mnumValue = x.mnum;
            }

            PNAX.AutoScaleTrace(mnumValue);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
