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
    public class Power : TestStep
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
            PowerOnAllChannels = true;
            PortInput = PortsEnum.Port1;
            InputPortLinearInputPower = -25;
            InputPortSourceAttenuator = 0;
            InputPortReceiverAttenuator = 0;
            InputSourceLevelingMode = InputSourceLevelingModeEnum.Internal;

            PortOutput = PortsEnum.Port2;
            OutputPortReversePower = -5;
            AutoOutputPortSourceAttenuator = false;
            OutputPortSourceAttenuator = 0;
            OutputPortReceiverAttenuator = 0;
            OutputSourceLevelingMode = OutputSourceLevelingModeEnum.Internal;

            PowerSweepStartPower = -25;
            PowerSweepStopPower = -5;
            PowerSweepPowerPoints = 21;
            PowerSweepPowerStep = 1;
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
