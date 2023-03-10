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
    public enum InputSourceLevelingModeEnum
    {
        Internal,
        OpenLoop,
        ReceiverR1
    }

    public enum OutputSourceLevelingModeEnum
    {
        Internal,
        ReceiverR2
    }

    [AllowAsChildIn(typeof(GainCompressionChannel))]
    [Display("Power", Groups: new[] { "PNA-X", "Converters", "Compression" }, Description: "Insert a description here")]
    public class Power : ConverterCompressionBaseStep
    {
        #region Settings
        [Display("Power On (All Channels)", Order: 10)]
        public bool PowerOnAllChannels { get; set; }

        [Display("Input Port", Group: "DUT Input Port", Order: 20)]
        public PortsEnum PortInput { get; set; }

        [Display("Linear Input Power", Group: "DUT Input Port", Order: 21)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double InputPortLinearInputPower { get; set; }

        [Display("Source Attenuator", Group: "DUT Input Port", Order: 22)]
        [Unit("dB", UseEngineeringPrefix: true)]
        public double InputPortSourceAttenuator { get; set; }

        [Display("Receiver Attenuator (A)", Group: "DUT Input Port", Order: 23)]
        [Unit("dB", UseEngineeringPrefix: true)]
        public double InputPortReceiverAttenuator { get; set; }

        [Display("Source Leveling Mode", Group: "DUT Input Port", Order: 24)]
        public InputSourceLevelingModeEnum InputSourceLevelingMode { get; set; }



        [Display("Output Port", Group: "DUT Output Port", Order: 30)]
        public PortsEnum PortOutput { get; set; }

        [Display("Reverse Power", Group: "DUT Output Port", Order: 31)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double OutputPortReversePower { get; set; }

        [Display("Auto", Group: "DUT Output Port", Order: 32)]
        public bool AutoOutputPortSourceAttenuator { get; set;}

        [EnabledIf("AutoOutputPortSourceAttenuator", false)]
        [Display("Source Attenuator", Group: "DUT Output Port", Order: 33)]
        [Unit("dB", UseEngineeringPrefix: true)]
        public double OutputPortSourceAttenuator { get; set; }

        [Display("Receiver Attenuator (A)", Group: "DUT Output Port", Order: 34)]
        [Unit("dB", UseEngineeringPrefix: true)]
        public double OutputPortReceiverAttenuator { get; set; }

        [Display("Source Leveling Mode", Group: "DUT Output Port", Order: 35)]
        public OutputSourceLevelingModeEnum OutputSourceLevelingMode { get; set; }


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

        public Power()
        {
            PowerOnAllChannels = GeneralStandardSettings.Current.PowerOnAllChannels;
            PortInput = GeneralStandardSettings.Current.PortInput;
            InputPortLinearInputPower = -GeneralStandardSettings.Current.InputPortLinearInputPower;
            InputPortSourceAttenuator = GeneralStandardSettings.Current.InputPortSourceAttenuator;
            InputPortReceiverAttenuator = GeneralStandardSettings.Current.InputPortReceiverAttenuator;
            InputSourceLevelingMode = GeneralStandardSettings.Current.InputSourceLevelingMode;

            PortOutput = GeneralStandardSettings.Current.PortOutput;
            OutputPortReversePower = GeneralStandardSettings.Current.OutputPortReversePower;
            AutoOutputPortSourceAttenuator = GeneralStandardSettings.Current.AutoOutputPortSourceAttenuator;
            OutputPortSourceAttenuator = GeneralStandardSettings.Current.OutputPortSourceAttenuator;
            OutputPortReceiverAttenuator = GeneralStandardSettings.Current.OutputPortReceiverAttenuator;
            OutputSourceLevelingMode = GeneralStandardSettings.Current.OutputSourceLevelingMode;

            PowerSweepStartPower = GeneralStandardSettings.Current.PowerSweepStartPower;
            PowerSweepStopPower = GeneralStandardSettings.Current.PowerSweepStopPower;
            PowerSweepPowerPoints = GeneralStandardSettings.Current.PowerSweepPowerPoints;
            PowerSweepPowerStep = GeneralStandardSettings.Current.PowerSweepPowerStep;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetPowerOnAllChannels(PowerOnAllChannels);

            PNAX.SetGCPortInputOutput(Channel, PortInput, PortOutput);
            PNAX.SetPowerLinearInputPower(Channel, InputPortLinearInputPower);
            PNAX.SetSourceAttenuator(Channel, PortInput, InputPortSourceAttenuator);
            PNAX.SetReceiverAttenuator(Channel, PortInput, InputPortReceiverAttenuator);
            PNAX.SetSourceLevelingMode(Channel, PortInput, InputSourceLevelingMode.ToString());

            PNAX.SetPowerReversePower(Channel, OutputPortReversePower);
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
