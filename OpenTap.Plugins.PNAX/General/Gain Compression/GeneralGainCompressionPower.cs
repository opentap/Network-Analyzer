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
    [Display("Power", Groups: new[] { "PNA-X", "General", "Gain Compression" }, Description: "Insert a description here")]
    public class GeneralGainCompressionPower : GeneralBaseStep
    {
        #region Settings
        [Display("Power On (All Channels)", Order: 10)]
        public bool PowerOnAllChannels { get; set; }

        [Display("Input Port", Group: "DUT Input Port", Order: 20)]
        public PortsEnum PortInput { get; set; }

        //[Display("Input Power", Group: "DUT Input Port", Order: 21)]
        //[Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        //public virtual double InputPower { get; set; }

        [Browsable(false)]
        public bool HasAutoInputPort { get; set; } = false;
        private bool _autoInputPortSourceAttenuator;
        [EnabledIf("HasAutoInputPort", true, HideIfDisabled = true)]
        [Display("Auto", Group: "DUT Input Port", Order: 22)]
        public bool AutoInputPortSourceAttenuator
        {
            get { return _autoInputPortSourceAttenuator; }
            set
            {
                _autoInputPortSourceAttenuator = value;
                //if (PortPowersCoupled)
                //    AutoOutputPortSourceAttenuator = value;
            }
        }

        [Browsable(false)]
        public double InputPortSourceAttenuatorAutoValue { get; set; }
        private double _inputPortSourceAttenuator;
        [EnabledIf("AutoInputPortSourceAttenuator", false)]
        [Display("Source Attenuator", Group: "DUT Input Port", Order: 22)]
        [Unit("dB", UseEngineeringPrefix: true)]
        public double InputPortSourceAttenuator
        {
            get
            {
                return AutoInputPortSourceAttenuator ? InputPortSourceAttenuatorAutoValue : _inputPortSourceAttenuator;
            }
            set
            {
                _inputPortSourceAttenuator = value;
                //if (PortPowersCoupled)
                //    OutputPortSourceAttenuator = value;
            }
        }

        [Display("Receiver Attenuator (A)", Group: "DUT Input Port", Order: 23)]
        [Unit("dB", UseEngineeringPrefix: true)]
        public double InputPortReceiverAttenuator { get; set; }

        [Display("Source Leveling Mode", Group: "DUT Input Port", Order: 24)]
        public InputSourceLevelingModeEnum InputSourceLevelingMode { get; set; }

        [Display("Output Port", Group: "DUT Output Port", Order: 30)]
        public virtual PortsEnum PortOutput { get; set; }

        [Display("Auto", Group: "DUT Output Port", Order: 32)]
        public bool AutoOutputPortSourceAttenuator { get; set; }

        [Browsable(false)]
        public double OutputPortSourceAttenuatorAutoValue { get; set; }
        private double _outputPortSourceAttenuator;
        [EnabledIf("AutoOutputPortSourceAttenuator", false)]
        //[EnabledIf("PortPowersCoupled", false)]
        [Display("Source Attenuator", Group: "DUT Output Port", Order: 33)]
        [Unit("dB", UseEngineeringPrefix: true)]
        public double OutputPortSourceAttenuator
        {
            get
            {
                return AutoOutputPortSourceAttenuator ? OutputPortSourceAttenuatorAutoValue : _outputPortSourceAttenuator;
            }
            set
            {
                _outputPortSourceAttenuator = value;
            }
        }

        [Display("Receiver Attenuator (A)", Group: "DUT Output Port", Order: 34)]
        [Unit("dB", UseEngineeringPrefix: true)]
        public double OutputPortReceiverAttenuator { get; set; }

        [Display("Source Leveling Mode", Group: "DUT Output Port", Order: 35)]
        public OutputSourceLevelingModeEnum OutputSourceLevelingMode { get; set; }




        private double _inputPower;
        [Display("Linear Input Power", Group: "DUT Input Port", Order: 21)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double InputPower
        {
            get { return _inputPower; }
            set
            {
                _inputPower = value;
            }
        }
        [Display("Reverse Power", Group: "DUT Output Port", Order: 31)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double OutputPower { get; set; }

        [Display("Start (Min) Power", Group: "Power Sweep", Order: 40)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double PowerSweepStartPower { get; set; }

        [Display("Stop (Max) Power", Group: "Power Sweep", Order: 41)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double PowerSweepStopPower { get; set; }

        [Display("Power Points", Group: "Power Sweep", Order: 42)]
        public int PowerSweepPowerPoints { get; set; }

        [Display("Power Step", Group: "Power Sweep", Order: 43)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double PowerSweepPowerStep { get; set; }

        #endregion

        public GeneralGainCompressionPower()
        {
            AutoInputPortSourceAttenuator = false;
            UpdateDefaultValues();
        }

        private void UpdateDefaultValues()
        {
            var defaultValuesSetup = PNAX.GetMixerSetupDefaultValues();
            PortInput = defaultValuesSetup.PortInput;
            PortOutput = defaultValuesSetup.PortOutput;

            var DefaultValues = PNAX.GetGeneralGainCompressionPowerDefaultValues();

            PowerOnAllChannels             = DefaultValues.PowerOnAllChannels;
            InputPower                     = DefaultValues.InputPortLinearInputPower;
            InputPortSourceAttenuator      = DefaultValues.InputPortSourceAttenuator;
            InputPortReceiverAttenuator    = DefaultValues.InputPortReceiverAttenuator;
            InputSourceLevelingMode        = DefaultValues.InputSourceLevelingMode;

            OutputPower                    = DefaultValues.OutputPortReversePower;
            AutoOutputPortSourceAttenuator = DefaultValues.AutoOutputPortSourceAttenuator;
            OutputPortSourceAttenuator     = DefaultValues.OutputPortSourceAttenuator;
            OutputPortReceiverAttenuator   = DefaultValues.OutputPortReceiverAttenuator;
            OutputSourceLevelingMode       = DefaultValues.OutputSourceLevelingMode;
            PowerSweepStartPower           = DefaultValues.PowerSweepStartPower;
            PowerSweepStopPower            = DefaultValues.PowerSweepStopPower;
            PowerSweepPowerPoints          = DefaultValues.PowerSweepPowerPoints;
            PowerSweepPowerStep            = DefaultValues.PowerSweepPowerStep;

        }

        public override void Run()
        {
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetPowerOnAllChannels(PowerOnAllChannels);

            PNAX.SetGCPortInputOutput(Channel, PortInput, PortOutput);
            PNAX.SetPowerLinearInputPower(Channel, InputPower);
            PNAX.SetSourceAttenuator(Channel, PortInput, InputPortSourceAttenuator);
            PNAX.SetReceiverAttenuator(Channel, PortInput, InputPortReceiverAttenuator);
            PNAX.SetSourceLevelingMode(Channel, PortInput, InputSourceLevelingMode.ToString());

            PNAX.SetPowerReversePower(Channel, OutputPower);
            PNAX.SetSourceAttenuatorAutoMode(Channel, PortOutput, AutoOutputPortSourceAttenuator);
            PNAX.SetSourceAttenuator(Channel, PortOutput, OutputPortSourceAttenuator);
            PNAX.SetReceiverAttenuator(Channel, PortOutput, OutputPortReceiverAttenuator);
            PNAX.SetSourceLevelingMode(Channel, PortOutput, OutputSourceLevelingMode.ToString());

            PNAX.SetPowerSweepStartPower(Channel, PowerSweepStartPower);
            PNAX.SetPowerSweepStopPower(Channel, PowerSweepStopPower);
            PNAX.SetPowerSweepPowerPoints(Channel, PowerSweepPowerPoints);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
