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
    [AllowAsChildIn(typeof(NoiseFigureChannel))]
    [Display("Noise Figure Power", Groups: new[] { "PNA-X", "Converters", "Noise Figure Converters" }, Description: "Insert a description here")]
    public class NoiseFigurePower : PowerBaseStep
    {
        #region Settings
        private double _inputPower;
        [Display("Power Level", Group: "DUT Input Port", Order: 21)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public override double InputPower
        {
            get { return _inputPower; }
            set
            {
                _inputPower = value;
                if (PortPowersCoupled)
                    OutputPower = value;
            }
        }

        [Browsable(false)]
        public bool OutputPortEnabled { get; set; } = false;
        [EnabledIf("OutputPortEnabled", true)]
        [Display("Output Port", Group: "DUT Output Port", Order: 30)]
        public override PortsEnum PortOutput { get; set; }

        [EnabledIf("PortPowersCoupled", false)]
        [Display("Power Level", Group: "DUT Output Port", Order: 31)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public override double OutputPower { get; set; }
        #endregion

        public NoiseFigurePower()
        {
            HasPortPowersCoupled = true;
            PortPowersCoupled = false;
            HasAutoInputPort = true;
            UpdateDefaultValues();
        }
        private void UpdateDefaultValues()
        {
            var defaultValuesSetup = PNAX.GetMixerSetupDefaultValues();
            PortInput = defaultValuesSetup.PortInput;
            PortOutput = defaultValuesSetup.PortOutput;

            var DefaultValues = PNAX.GetNoiseFigureConverterPowerDefaultValues();

            PowerOnAllChannels = DefaultValues.PowerOnAllChannels;
            PortPowersCoupled  = DefaultValues.PortPowersCoupled;

            InputPower                    = DefaultValues.InputPortLinearInputPower;
            AutoInputPortSourceAttenuator = DefaultValues.AutoOutputPortSourceAttenuator;
            InputPortSourceAttenuator     = DefaultValues.InputPortSourceAttenuator;
            InputPortReceiverAttenuator   = DefaultValues.InputPortReceiverAttenuator;
            InputSourceLevelingMode       = DefaultValues.InputSourceLevelingMode;

            OutputPower                    = DefaultValues.OutputPortReversePower;
            AutoOutputPortSourceAttenuator = DefaultValues.AutoOutputPortSourceAttenuator;
            OutputPortSourceAttenuator     = DefaultValues.OutputPortSourceAttenuator;
            OutputPortReceiverAttenuator   = DefaultValues.OutputPortReceiverAttenuator;
            OutputSourceLevelingMode       = DefaultValues.OutputSourceLevelingMode;

            InputPortSourceAttenuatorAutoValue  = InputPortSourceAttenuator;
            OutputPortSourceAttenuatorAutoValue = OutputPortSourceAttenuator;

        }
        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetPowerOnAllChannels(PowerOnAllChannels);
            PNAX.SetNFCoupledTonePowers(Channel, PortPowersCoupled);

            PNAX.SetNFPortInputOutput(Channel, PortInput, PortOutput);

            PNAX.SetNFPowerLevel(Channel, PortInput, InputPower);
            PNAX.SetSourceAttenuator(Channel, (int)PortInput, InputPortSourceAttenuator);
            PNAX.SetReceiverAttenuator(Channel, (int)PortInput, InputPortReceiverAttenuator);
            PNAX.SetSourceLevelingMode(Channel, PortInput, InputSourceLevelingMode);
            PNAX.SetSourceAttenuatorAutoMode(Channel, PortInput, AutoInputPortSourceAttenuator);


            PNAX.SetNFPowerLevel(Channel, PortOutput, OutputPower);
            PNAX.SetSourceAttenuator(Channel, (int)PortOutput, OutputPortSourceAttenuator);
            PNAX.SetReceiverAttenuator(Channel, (int)PortOutput, OutputPortReceiverAttenuator);
            PNAX.SetSourceLevelingMode(Channel, PortOutput, OutputSourceLevelingMode);
            PNAX.SetSourceAttenuatorAutoMode(Channel, PortOutput, AutoOutputPortSourceAttenuator);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
