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
    public enum DutInputPortsEnum
    {
        Port1,
        Port3,
    }

    public enum DutOutputPortsEnum
    {
        Port2,
        Port4
    }

    public enum PowerLevelingEnum
    {
        [Display("Set Input Power")]
        SetInputPower,
        [Display("Set Input Power, receiver leveling")]
        SetInputPowerReceiverLeveling,
        [Display("Set Input Power, equal tones at output")]
        SetInputPowerEqualTonesAtOutput,
        [Display("Set Output Power, receiver leveling")]
        SetOutputPowerReceiverLeveling
    }

    [AllowAsChildIn(typeof(SweptIMDChannel))]
    [Display("Tone Power", Groups: new[] { "PNA-X", "Converters", "Swept IMD Converters" }, Description: "Insert a description here")]
    public class TonePower : TestStep
    {
        #region Settings

        [Browsable(false)]
        public bool IsControlledByParent { get; set; } = false;
        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display("Power On (All Channels)", Order: 10)]
        public bool PowerOnAllChannels { get; set; }

        [Display("Input Port", Group: "DUT Input", Order: 30)]
        public DutInputPortsEnum PortInput { get; set; }

        [Display("Source Attenuator", Group: "DUT Input", Order: 31)]
        [Unit("dB", UseEngineeringPrefix: true)]
        public double SourceAttenuatorDutInput { get; set; }

        [Display("Receiver Attenuator", Group: "DUT Input", Order: 32)]
        [Unit("dB", UseEngineeringPrefix: true)]
        public double ReceiverAttenuatorDutInput { get; set; }



        [Display("Output Port", Group: "DUT Output", Order: 40)]
        public DutOutputPortsEnum PortOutput { get; set; }

        [Display("Source Attenuator", Group: "DUT Output", Order: 41)]
        [Unit("dB", UseEngineeringPrefix: true)]
        public double SourceAttenuatorDutOutput { get; set; }

        [Display("Receiver Attenuator", Group: "DUT Output", Order: 42)]
        [Unit("dB", UseEngineeringPrefix: true)]
        public double ReceiverAttenuatorDutOutput { get; set; }




        [Display("Coupled Tone Powers", Group: "Tone Powers", Order: 50)]
        public bool CoupleTonePowers { get; set; }

        [Display("ALC On", Group: "Tone Powers", Order: 51)]
        public bool ALCOn { get; set; }

        [Display("Power Leveling", Group: "Tone Powers", Order: 52)]
        public PowerLevelingEnum PowerLeveling { get; set; }

        [Display("Fixed f1 Power", Group: "Tone Powers", Order: 53)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double FixedF1Power { get; set; }

        [Display("Fixed f2 Power", Group: "Tone Powers", Order: 54)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double FixedF2Power { get; set; }

        [Display("Start f1 Power", Group: "Tone Powers", Order: 55)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double StartF1Power { get; set; }

        [Display("Start f2 Power", Group: "Tone Powers", Order: 56)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double StartF2Power { get; set; }

        [Display("Stop f1 Power", Group: "Tone Powers", Order: 57)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double StopF1Power { get; set; }

        [Display("Stop f2 Power", Group: "Tone Powers", Order: 58)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double StopF2Power { get; set; }

        [Display("Step f1 Power", Group: "Tone Powers", Order: 59)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double StepF1Power { get; set; }

        [Display("Step f2 Power", Group: "Tone Powers", Order: 60)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double StepF2Power { get; set; }

        #endregion

        public TonePower()
        {
            PowerOnAllChannels = true;
            CoupleTonePowers = true;
            ALCOn = true;
            UpdateDefaultValues();
        }

        private void UpdateDefaultValues()
        {
            var DefaultValues = PNAX.GetTonePowerDefaultValues();
            FixedF1Power = DefaultValues.FixedF1Power;
            FixedF2Power = DefaultValues.FixedF2Power;
            StartF1Power = DefaultValues.StartF1Power;
            StartF2Power = DefaultValues.StartF2Power;
            StopF1Power  = DefaultValues.StopF1Power;
            StopF2Power  = DefaultValues.StopF2Power;
            StepF1Power  = DefaultValues.StepF1Power;
            StepF2Power  = DefaultValues.StepF2Power;
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
