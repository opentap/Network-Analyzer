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
        [Display("Internal")]
        INTernal,
        [Display("Open Loop")]
        OPENloop
    }

    [AllowAsChildIn(typeof(GainCompressionChannel))]
    [AllowAsChildIn(typeof(SweptIMDChannel))]
    [AllowAsChildIn(typeof(NoiseFigureChannel))]
    [AllowAsChildIn(typeof(ScalarMixerChannel))]
    [Display("Mixer Power", Groups: new[] { "PNA-X", "Converters" }, Description: "Insert a description here", Order: 2)]
    public class MixerPowerTestStep : ConverterBaseStep
    {
        #region Settings
        [Display("Power On (All Channels)", Order: 10)]
        public bool PowerOnAllChannels { get; set; }

        [Browsable(false)]
        public bool EnableLO1 { get; set; }

        private LOEnum _PortLO1;
        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
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
        [EnabledIf("ConverterStages", ConverterStagesEnum._2, HideIfDisabled = true)]
        [EnabledIf("EnableLO2", true, HideIfDisabled = true)]
        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
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
                EnableLO2 = _PortLO2 != LOEnum.NotControlled;
            }
        }

        [EnabledIf("ConverterStages", ConverterStagesEnum._2, HideIfDisabled = true)]
        [EnabledIf("EnableLO2", true, HideIfDisabled = true)]
        [Display("LO2 Power", Group: "LO2", Order: 31)]
        [Unit("dBm", UseEngineeringPrefix: true)]
        public double LO2Power { get; set; }

        [EnabledIf("ConverterStages", ConverterStagesEnum._2, HideIfDisabled = true)]
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

        // TODO
        // Hide these group only for Converters Gain Compression
        // For all aother Converters, these are available
        // TODO
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
            UpdateDefaultValues();
        }

        private void UpdateDefaultValues()
        {
            var defaultValues = PNAX.GetMixerPowerDefaultValues();
            var defaultMixerSetupValues = PNAX.GetMixerSetupDefaultValues();

            PortLO1 = defaultMixerSetupValues.PortLO1;  // get this value from TestStep MixerSetupTestStep.cs
            PortLO2 = defaultMixerSetupValues.PortLO2;  // get this value from TestStep MixerSetupTestStep.cs

            PowerOnAllChannels = defaultValues.PowerOnAllChannels;

            SourceLevelingModeLO1 = defaultValues.SourceLevelingModeLO1;
            SourceLevelingModeLO2 = defaultValues.SourceLevelingModeLO2;
            LO1Power              = defaultValues.Lo1Power;
            LO2Power              = defaultValues.Lo2Power;

            SourceAttenuatorPowerPort3   = defaultValues.SourceAttenuatorPowerPort3;
            ReceiverAttenuatorPowerPort3 = defaultValues.ReceiverAttenuatorPowerPort3;
            SourceAttenuatorPowerPort4   = defaultValues.SourceAttenuatorPowerPort4;
            ReceiverAttenuatorPowerPort4 = defaultValues.ReceiverAttenuatorPowerPort4;
            LO1SweptPowerStart = defaultValues.LO1SweptPowerStart;
            LO1SweptPowerStop  = defaultValues.LO1SweptPowerStop ;
            LO1SweptPowerStep  = defaultValues.LO1SweptPowerStep ;
            LO2SweptPowerStart = defaultValues.LO2SweptPowerStart;
            LO2SweptPowerStop  = defaultValues.LO2SweptPowerStop ;
            LO2SweptPowerStep  = defaultValues.LO2SweptPowerStep ;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetPowerOnAllChannels(PowerOnAllChannels);
            PNAX.SetLOPower(Channel, 1, LO1Power);
            // We are assuming Port 3, but we need to get this value from MixerSetupTestStep:PortLO1 "PortLO1"
            PNAX.SetSourceLevelingMode(Channel, 3, SourceLevelingModeLO1.ToString());

            if(ConverterStages == ConverterStagesEnum._2)
            {
                PNAX.SetLOPower(Channel, 2, LO2Power);
                // We are assuming Port 4, but we need to get this value from MixerSetupTestStep:PortLO2 "PortLO2"
                PNAX.SetSourceLevelingMode(Channel, 4, SourceLevelingModeLO2.ToString());
            }

            PNAX.SetSourceAttenuator(Channel, 3, SourceAttenuatorPowerPort3);
            PNAX.SetReceiverAttenuator(Channel, 3, ReceiverAttenuatorPowerPort3);

            PNAX.SetSourceAttenuator(Channel, 4, SourceAttenuatorPowerPort4);
            PNAX.SetReceiverAttenuator(Channel, 4, ReceiverAttenuatorPowerPort4);

            PNAX.SetLOSweptPowerStart(Channel, 1, LO1SweptPowerStart);
            PNAX.SetLOSweptPowerStop(Channel, 1, LO1SweptPowerStop);

            if (ConverterStages == ConverterStagesEnum._2)
            {
                PNAX.SetLOSweptPowerStart(Channel, 2, LO2SweptPowerStart);
                PNAX.SetLOSweptPowerStop(Channel, 2, LO2SweptPowerStop);
            }
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
