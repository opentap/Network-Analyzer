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
    public class GeneralGainCompressionPower : PowerBaseStep
    {
        #region Settings
        [Display("Linear Input Power", Group: "DUT Input Port", Order: 21)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public override double InputPower { get; set; }

        [Display("Reverse Power", Group: "DUT Output Port", Order: 31)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public override double OutputPower { get; set; }

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
        }

        protected override void UpdatePowerValues()
        {
            var DefaultValues = PNAX.GetGeneralGainCompressionPowerDefaultValues();

            PowerOnAllChannels = DefaultValues.PowerOnAllChannels;
            InputPower = DefaultValues.InputPortLinearInputPower;
            InputPortSourceAttenuator = DefaultValues.InputPortSourceAttenuator;
            InputPortReceiverAttenuator = DefaultValues.InputPortReceiverAttenuator;
            InputSourceLevelingMode = DefaultValues.InputSourceLevelingMode;

            OutputPower = DefaultValues.OutputPortReversePower;
            AutoOutputPortSourceAttenuator = DefaultValues.AutoOutputPortSourceAttenuator;
            OutputPortSourceAttenuator = DefaultValues.OutputPortSourceAttenuator;
            OutputPortReceiverAttenuator = DefaultValues.OutputPortReceiverAttenuator;
            OutputSourceLevelingMode = DefaultValues.OutputSourceLevelingMode;
            PowerSweepStartPower = DefaultValues.PowerSweepStartPower;
            PowerSweepStopPower = DefaultValues.PowerSweepStopPower;
            PowerSweepPowerPoints = DefaultValues.PowerSweepPowerPoints;
            PowerSweepPowerStep = DefaultValues.PowerSweepPowerStep;
        }

        protected override void SetPowerFlags()
        {
            PNAX.SetPowerOnAllChannels(PowerOnAllChannels);
        }

        protected override void SetPort()
        {
            PNAX.SetGCPortInputOutput(Channel, PortInput, PortOutput);
        }

        protected override void SetInputPower()
        {
            PNAX.SetPowerLinearInputPower(Channel, InputPower);
            PNAX.SetSourceAttenuator(Channel, PortInput, InputPortSourceAttenuator);
            PNAX.SetReceiverAttenuator(Channel, PortInput, InputPortReceiverAttenuator);
            PNAX.SetSourceLevelingMode(Channel, PortInput, InputSourceLevelingMode.ToString());
        }

        protected override void SetOutputPower()
        {
            PNAX.SetPowerReversePower(Channel, OutputPower);
            PNAX.SetSourceAttenuatorAutoMode(Channel, PortOutput, AutoOutputPortSourceAttenuator);
            PNAX.SetSourceAttenuator(Channel, PortOutput, OutputPortSourceAttenuator);
            PNAX.SetReceiverAttenuator(Channel, PortOutput, OutputPortReceiverAttenuator);
            PNAX.SetSourceLevelingMode(Channel, PortOutput, OutputSourceLevelingMode.ToString());
        }

        protected override void SetSweepPower()
        {
            PNAX.SetPowerSweepStartPower(Channel, PowerSweepStartPower);
            PNAX.SetPowerSweepStopPower(Channel, PowerSweepStopPower);
            PNAX.SetPowerSweepPowerPoints(Channel, PowerSweepPowerPoints);
        }
    }
}
