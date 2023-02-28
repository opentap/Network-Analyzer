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
    public enum SourceLevelingModeType
    {
        Internal,
        OpenLoop
    }

    [AllowAsChildIn(typeof(GainCompressionChannel))]
    [AllowAsChildIn(typeof(SweptIMDChannel))]
    [Display("Mixer Power", Groups: new[] { "PNA-X", "Converters" }, Description: "Insert a description here", Order: 2)]
    public class MixerPowerTestStep : TestStep
    {
        #region Settings
        [Display("Power On (All Channels)", Order: 10)]
        public bool PowerOnAllChannels { get; set; }

        [Browsable(false)]
        public bool EnableLO1 { get; set; }

        private LOEnum _PortLO1;
        [Display("LO1", Group: "LO1", Order: 20)]
        public LOEnum PortLO1
        {
            get
            {
                return _PortLO1;
            }
            set 
            {
                _PortLO1 = value;
                if (_PortLO1 == LOEnum.NotControlled)
                {
                    EnableLO1 = false;
                }
                else
                {
                    EnableLO1 = true;
                }
            }
        }

        [EnabledIf("EnableLO1", true, HideIfDisabled = true)]
        [Display("LO1 Power", Group: "LO1", Order: 21)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double LO1Power { get; set; }

        [EnabledIf("EnableLO1", true, HideIfDisabled = true)]
        [Display("Source Leveling Mode", Group: "LO1", Order: 22)]
        public SourceLevelingModeType SourceLevelingModeLO1 { get; set; }


        [Browsable(false)]
        public bool EnableLO2 { get; set; }

        private LOEnum _PortLO2;
        [Display("LO2", Group: "LO2", Order: 30)]
        public LOEnum PortLO2
        {
            get
            {
                return _PortLO2;
            }
            set
            {
                _PortLO2 = value;
                if (_PortLO2 == LOEnum.NotControlled)
                {
                    EnableLO2 = false;
                }
                else
                {
                    EnableLO2 = true;
                }
            }
        }

        [EnabledIf("EnableLO2", true, HideIfDisabled = true)]
        [Display("LO2 Power", Group: "LO2", Order: 31)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double LO2Power { get; set; }

        [EnabledIf("EnableLO2", true, HideIfDisabled = true)]
        [Display("Source Leveling Mode", Group: "LO2", Order: 32)]
        public SourceLevelingModeType SourceLevelingModeLO2 { get; set; }


        [Display("Source Attenuator", Groups: new[] { "Port Settings" , "Port 3" }, Order: 40)]
        [Unit("dB", UseEngineeringPrefix: true)]
        public double SourceAttenuatorPowerPort3 { get; set; }

        [Display("Receiver Attenuator", Groups: new[] { "Port Settings", "Port 3" }, Order: 41)]
        [Unit("dB", UseEngineeringPrefix: true)]
        public double ReceiverAttenuatorPowerPort3 { get; set; }

        [Display("Source Attenuator", Groups: new[] { "Port Settings", "Port 4" }, Order: 50)]
        [Unit("dB", UseEngineeringPrefix: true)]
        public double SourceAttenuatorPowerPort4 { get; set; }

        [Display("Receiver Attenuator", Groups: new[] { "Port Settings", "Port 4" }, Order: 51)]
        [Unit("dB", UseEngineeringPrefix: true)]
        public double ReceiverAttenuatorPowerPort4 { get; set; }


        [Display("Start", Groups: new[] { "Swept Power Settings", "LO1 Swept Power" }, Order: 60)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double LO1SweptPowerStart { get; set; }

        [Display("Stop", Groups: new[] { "Swept Power Settings", "LO1 Swept Power" }, Order: 61)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double LO1SweptPowerStop { get; set; }

        [Display("Step", Groups: new[] { "Swept Power Settings", "LO1 Swept Power" }, Order: 62)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double LO1SweptPowerStep { get; set; }

        [Display("Start", Groups: new[] { "Swept Power Settings", "LO2 Swept Power" }, Order: 70)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double LO2SweptPowerStart { get; set; }

        [Display("Stop", Groups: new[] { "Swept Power Settings", "LO2 Swept Power" }, Order: 71)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double LO2SweptPowerStop { get; set; }

        [Display("Step", Groups: new[] { "Swept Power Settings", "LO2 Swept Power" }, Order: 72)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double LO2SweptPowerStep { get; set; }
        #endregion

        public MixerPowerTestStep()
        {
            PortLO1 = LOEnum.NotControlled; // new Input<LOEnum>();
            LO1Power = -15;
            SourceLevelingModeLO1 = SourceLevelingModeType.Internal;

            PortLO2 = LOEnum.NotControlled; // new Input<LOEnum>();
            LO2Power = -15;
            SourceLevelingModeLO2 = SourceLevelingModeType.Internal;

            SourceAttenuatorPowerPort3 = 0;
            ReceiverAttenuatorPowerPort3 = 0;
            SourceAttenuatorPowerPort4 = 0;
            ReceiverAttenuatorPowerPort4 = 0;

            LO1SweptPowerStart = -20;
            LO1SweptPowerStop = -10;
            LO1SweptPowerStep = 0.05;

            LO2SweptPowerStart = -10;
            LO2SweptPowerStop = -10;
            LO2SweptPowerStep = 0.0;
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
