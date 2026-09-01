// Author: CMontes
// Copyright:   Copyright 2024 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using System.ComponentModel;

namespace OpenTap.Plugins.PNAX
{
    // Shared setup/run logic for Modulation Distortion channels (General and Converters variants).
    [Browsable(false)]
    public class MODChannelBaseStep : PNABaseStep
    {
        #region Settings
        [Display("Sweep Mode", Group: "Settings", Order: 10)]
        public SweepModeEnumType sweepMode { get; set; }
        #endregion

        protected virtual string DummyMeasurementClass => "Modulation Distortion";

        public MODChannelBaseStep()
        {
            IsControlledByParent = false;
            Channel = 1;
            sweepMode = SweepModeEnumType.SING;
        }

        protected void AddModChildSteps(PNABaseStep newTraceStep, PNABaseStep mixerStep = null)
        {
            ChildTestSteps.Add(ConfigureChildStep(newTraceStep));
            ChildTestSteps.Add(ConfigureChildStep(new MODSweep()));
            ChildTestSteps.Add(ConfigureChildStep(new MODRFPath()));
            ChildTestSteps.Add(ConfigureChildStep(new MODModulate()));
            if (mixerStep != null)
            {
                ChildTestSteps.Add(ConfigureChildStep(mixerStep));
            }
            ChildTestSteps.Add(ConfigureChildStep(new MODSourceCorrection()));
            ChildTestSteps.Add(ConfigureChildStep(new MODMeasure()));
        }

        public override void Run()
        {
            DefineDummyTrace(DummyMeasurementClass, "PIn1", string.Empty);

            RunChildSteps(); //If the step supports child steps.

            PNAX.SetSweepMode(Channel, SweepModeEnumType.SING);

            UpgradeVerdict(Verdict.Pass);
            UpdateMetaData();
        }
    }
}
