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
        [Scpi("INT")]
        Internal,
        [Scpi("OPEN")]
        OpenLoop,
        ReceiverR1
    }

    public enum OutputSourceLevelingModeEnum
    {
        Internal,
        ReceiverR2
    }

    [Browsable(false)]
    public class PowerBaseStep : ConverterBaseStep
    {
        #region Settings
        [Display("Power On (All Channels)", Order: 10)]
        public bool PowerOnAllChannels { get; set; }

        [Browsable(false)]
        public bool HasPortPowersCoupled { get; set; } = false;
        private bool _portPowersCoupled;
        [EnabledIf("HasPortPowersCoupled", true, HideIfDisabled = false)]
        [Display("Port Powers Coupled", Order: 11)]
        public bool PortPowersCoupled
        {
            get { return _portPowersCoupled; }
            set
            {
                _portPowersCoupled = value;
                if (value)
                {
                    OutputPower = InputPower;
                    AutoOutputPortSourceAttenuator = AutoInputPortSourceAttenuator;
                    OutputPortSourceAttenuator = InputPortSourceAttenuator;
                }
            }
        }

        [Display("Input Port", Group: "DUT Input Port", Order: 20)]
        public PortsEnum PortInput { get; set; }

        [Display("Input Power", Group: "DUT Input Port", Order: 21)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public virtual double InputPower { get; set; }

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
                if (PortPowersCoupled)
                    AutoOutputPortSourceAttenuator = value;
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
                if (PortPowersCoupled)
                    OutputPortSourceAttenuator = value;
            }
        }

        [Display("Receiver Attenuator (A)", Group: "DUT Input Port", Order: 23)]
        [Unit("dB", UseEngineeringPrefix: true)]
        public double InputPortReceiverAttenuator { get; set; }

        [Display("Source Leveling Mode", Group: "DUT Input Port", Order: 24)]
        public InputSourceLevelingModeEnum InputSourceLevelingMode { get; set; }

        [Display("Output Port", Group: "DUT Output Port", Order: 30)]
        public virtual PortsEnum PortOutput { get; set; }

        [Display("Output Power", Group: "DUT Output Port", Order: 31)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public virtual double OutputPower { get; set; }

        [EnabledIf("PortPowersCoupled", false)]
        [Display("Auto", Group: "DUT Output Port", Order: 32)]
        public bool AutoOutputPortSourceAttenuator { get; set;}

        [Browsable(false)]
        public double OutputPortSourceAttenuatorAutoValue { get; set; }
        private double _outputPortSourceAttenuator;
        [EnabledIf("AutoOutputPortSourceAttenuator", false)]
        [EnabledIf("PortPowersCoupled", false)]
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


        #endregion

        public PowerBaseStep()
        {
        }

        public override void Run()
        {

        }
    }
}
