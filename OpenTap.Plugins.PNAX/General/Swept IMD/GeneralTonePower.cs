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
    [AllowAsChildIn(typeof(GeneralSweptIMDChannel))]
    [Display("Tone Power", Groups: new[] { "PNA-X", "General", "Swept IMD" }, Description: "Insert a description here")]
    public class GeneralTonePower : GeneralBaseStep
    {
        #region Settings
        [Browsable(false)]
        public bool IsPowerSweep { get; set; }

        private GeneralToneFrequencySweepTypeEnum _ToneFrequencySweepType;
        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
        [Display("Sweep Type", Order: 0.5)]
        public GeneralToneFrequencySweepTypeEnum ToneFrequencySweepType 
        {
            get
            {
                return _ToneFrequencySweepType;
            }
            set
            {
                _ToneFrequencySweepType = value;
                IsPowerSweep = _ToneFrequencySweepType == GeneralToneFrequencySweepTypeEnum.PowerSweep;
            } 
        }

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


        private bool _CoupleTonePowers;
        [Display("Coupled Tone Powers", Group: "Tone Powers", Order: 50)]
        public bool CoupleTonePowers 
        {
            get
            {
                return _CoupleTonePowers;
            }
            set
            {
                _CoupleTonePowers = value;

                // Update f2 values as f1 values
                FixedF2Power = FixedF1Power;
                StartF2Power = StartF1Power;
                StopF2Power = StopF1Power;
            }
        }

        [Display("ALC On", Group: "Tone Powers", Order: 51)]
        public bool ALCOn { get; set; }

        [Display("Power Leveling", Group: "Tone Powers", Order: 52)]
        public PowerLevelingEnum PowerLeveling { get; set; }

        private double _fixedF1Power;
        [EnabledIf("IsPowerSweep", false, HideIfDisabled = false)]
        [Display("Fixed f1 Power", Group: "Tone Powers", Order: 53)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double FixedF1Power
        {
            get { return _fixedF1Power; }
            set
            {
                _fixedF1Power = value;
                FixedF2Power = value;
            }
        }

        [EnabledIf("IsPowerSweep", false, HideIfDisabled = false)]
        [Display("Fixed f2 Power", Group: "Tone Powers", Order: 54)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double FixedF2Power { get; set; }

        private double _startF1Power;
        [EnabledIf("IsPowerSweep", true, HideIfDisabled = false)]
        [Display("Start f1 Power", Group: "Tone Powers", Order: 55)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double StartF1Power
        {
            get { return _startF1Power; }
            set
            {
                _startF1Power = value;
                StartF2Power = value;
            }
        }

        [EnabledIf("IsPowerSweep", true, HideIfDisabled = false)]
        [Display("Start f2 Power", Group: "Tone Powers", Order: 56)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double StartF2Power { get; set; }

        private double _stopF1Power;
        [EnabledIf("IsPowerSweep", true, HideIfDisabled = false)]
        [Display("Stop f1 Power", Group: "Tone Powers", Order: 57)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double StopF1Power
        {
            get { return _stopF1Power; }
            set
            {
                _stopF1Power = value;
                StopF2Power = value;
            }
        }

        [EnabledIf("IsPowerSweep", true, HideIfDisabled = false)]
        [Display("Stop f2 Power", Group: "Tone Powers", Order: 58)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double StopF2Power { get; set; }

        #endregion

        public GeneralTonePower()
        {
            PowerOnAllChannels = true;
            UpdateDefaultValues();
        }

        private void UpdateDefaultValues()
        {
            var DefaultValues = PNAX.GetTonePowerDefaultValues();
            PortInput = DefaultValues.PortInput;
            PortOutput = DefaultValues.PortOutput;
            SourceAttenuatorDutInput = DefaultValues.InputPortSourceAttenuator;
            ReceiverAttenuatorDutInput = DefaultValues.InputPortReceiverAttenuator;
            SourceAttenuatorDutOutput = DefaultValues.OutputPortSourceAttenuator;
            ReceiverAttenuatorDutOutput = DefaultValues.OutputPortReceiverAttenuator;

            CoupleTonePowers = DefaultValues.CoupleTonePowers;
            ALCOn = DefaultValues.ALCOn;
            PowerLeveling = DefaultValues.PowerLeveling;
            FixedF1Power = DefaultValues.FixedF1Power;
            FixedF2Power = DefaultValues.FixedF2Power;
            StartF1Power = DefaultValues.StartF1Power;
            StartF2Power = DefaultValues.StartF2Power;
            StopF1Power  = DefaultValues.StopF1Power;
            StopF2Power  = DefaultValues.StopF2Power;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetPowerOnAllChannels(PowerOnAllChannels);

            PNAX.SetIMDPortInputOutput(Channel, PortInput, PortOutput);
            PNAX.SetSourceAttenuator(Channel, (int)PortInput, SourceAttenuatorDutInput);
            PNAX.SetReceiverAttenuator(Channel, (int)PortInput, ReceiverAttenuatorDutInput);
            PNAX.SetSourceAttenuator(Channel, (int)PortOutput, SourceAttenuatorDutOutput);
            PNAX.SetReceiverAttenuator(Channel, (int)PortOutput, ReceiverAttenuatorDutOutput);

            PNAX.SetIMDCoupledTonePowers(Channel, CoupleTonePowers);
            PNAX.SetIMDALCHardware(Channel, (int)PortInput, ALCOn);

            PNAX.SetIMDPowerLevelingMode(Channel, PowerLeveling);
            switch (PowerLeveling)
            {
                case PowerLevelingEnum.SetInputPower:
                    PNAX.SetIMDReceiverLevelingMode(Channel, (int)PortInput, false);
                    break;
                case PowerLevelingEnum.SetInputPowerReceiverLeveling:
                    PNAX.SetIMDReceiverLevelingMode(Channel, (int)PortInput, true);
                    break;
                case PowerLevelingEnum.SetInputPowerEqualTonesAtOutput:
                    PNAX.SetIMDReceiverLevelingMode(Channel, (int)PortInput, false);
                    break;
                case PowerLevelingEnum.SetOutputPowerReceiverLeveling:
                    PNAX.SetIMDReceiverLevelingMode(Channel, (int)PortInput, true);
                    break;
            }

            if (ToneFrequencySweepType != GeneralToneFrequencySweepTypeEnum.PowerSweep)
            {
                // All power sweeps use Fixed f1 and f2
                PNAX.SetIMDFixedPowerF1(Channel, FixedF1Power);
                PNAX.SetIMDFixedPowerF2(Channel, FixedF2Power);
            }
            else
            {
                // except for Power Sweep that uses Start and Stop
                PNAX.SetIMDStartPowerF1(Channel, StartF1Power);
                PNAX.SetIMDStartPowerF2(Channel, StartF2Power);
                PNAX.SetIMDStopPowerF1(Channel, StopF1Power);
                PNAX.SetIMDStopPowerF2(Channel, StopF2Power);

            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
