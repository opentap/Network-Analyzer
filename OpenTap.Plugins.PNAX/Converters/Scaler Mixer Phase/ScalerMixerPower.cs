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
    [AllowAsChildIn(typeof(ScalerMixerChannel))]
    [Display("Scaler Mixer Power", Groups: new[] { "PNA-X", "Converters", "Scaler Mixer Converter + Phase" }, Description: "Insert a description here")]
    public class ScalerMixerPower : PowerBaseStep
    {
        #region Settings
        [Browsable(false)]
        public bool EnablePowerSweepOutputEdit { get; set; } = false;

        [EnabledIf("EnablePowerSweepOutputEdit", true)]
        [Display("Power Level", Group: "DUT Input Port", Order: 21)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public override double InputPower { get; set; }

        [EnabledIf("EnablePowerSweepOutputEdit", true)]
        [Display("Power Level", Group: "DUT Output Port", Order: 31)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public override double OutputPower { get; set; }

        [EnabledIf("EnablePowerSweepOutputEdit", true)]
        [Display("Start Power", Group: "Dut Input Port Power Sweep", Order: 40)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double InputPowerSweepStartPower { get; set; }

        [EnabledIf("EnablePowerSweepOutputEdit", true)]
        [Display("Stop Power", Group: "Dut Input Port Power Sweep", Order: 41)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double InputPowerSweepStopPower { get; set; }

        [EnabledIf("EnablePowerSweepOutputEdit", true)]
        [Display("Points", Group: "Dut Input Port Power Sweep", Order: 42)]
        public int InputPowerSweepPowerPoints { get; set; }

        [EnabledIf("EnablePowerSweepOutputEdit", true)]
        [Display("Power Step", Group: "Dut Input Port Power Sweep", Order: 43)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double InputPowerSweepPowerStep { get; set; }

        [EnabledIf("EnablePowerSweepOutputEdit", true)]
        [Display("Start Power", Group: "Dut Output Port Power Sweep", Order: 50)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double OutputPowerSweepStartPower { get; set; }

        [EnabledIf("EnablePowerSweepOutputEdit", true)]
        [Display("Stop Power", Group: "Dut Output Port Power Sweep", Order: 51)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double OutputPowerSweepStopPower { get; set; }
        #endregion

        public ScalerMixerPower()
        {
            // ToDo: Set default values for properties / settings.
        }

        public override void Run()
        {
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
