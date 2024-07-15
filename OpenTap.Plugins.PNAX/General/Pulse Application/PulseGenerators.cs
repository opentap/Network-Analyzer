// Author: CMontes
// Copyright:   Copyright 2024 Keysight Technologies
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
using System.Xml.Serialization;

namespace OpenTap.Plugins.PNAX
{
    //[AllowAsChildIn(typeof(PulseSetup))]
    [Display("Pulse Generators Setup", Groups: new[] { "Network Analyzer", "General" }, Description: "Insert a description here")]
    public class PulseGenerators : PNABaseStep
    {
        #region Settings
        [Browsable(false)]
        public bool IsSettingReadOnly { get; set; } = false;

        // Update parent step Pulse Timing section with these values
        [EnabledIf("IsSettingReadOnly", true, HideIfDisabled = false)]
        [Display("Trigger", Groups: new[] { "Pulse Generators" }, Order: 21, Description: "Set this value on parent step")]
        public PulseTriggerEnumtype PulseTriggerType { get; set; }

        // Update parent step Pulse Timing section with these values
        [EnabledIf("IsSettingReadOnly", true, HideIfDisabled = false)]
        [Display("Frequency", Groups: new[] { "Pulse Generators" }, Order: 22, Description: "Set this value on parent step")]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double Frequency { get; set; }


        // Update parent step Pulse Timing section with these values
        [EnabledIf("IsSettingReadOnly", true, HideIfDisabled = false)]
        [Display("Period", Groups: new[] { "Pulse Generators" }, Order: 23, Description: "Set this value on parent step")]
        [Unit("sec", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double Period { get; set; }


        [Display("Source 1 Enable Modulator", Groups: new[] { "Pulsed Sources" }, Order: 31)]
        public bool Source1EnableModulator { get; set; }

        [Display("ALC Open Loop", Groups: new[] { "Pulsed Sources" }, Order: 32)]
        public bool ALCOpenLoop { get; set; }

        [Display("Source 2 Enable Modulator", Groups: new[] { "Pulsed Sources" }, Order: 33)]
        public bool Source2EnableModulator { get; set; }

        [Display("Modulator Drive", Groups: new[] { "Pulsed Sources" }, Order: 34)]
        [AvailableValues(nameof(PulseGenListOfAvailableValues))]
        public String ModulatorDrive { get; set; }



        private bool _SynchADCUsingPulseTrigger;
        [EnabledIf("IsSettingReadOnly", true, HideIfDisabled = false)]
        [Display("Synchronize ADCs Using Pulse Trigger", Groups: new[] { "Pulsed Receivers" }, Order: 41, Description:"Set this value on parent step")]
        public bool SynchADCUsingPulseTrigger
        {
            set
            {
                _SynchADCUsingPulseTrigger = value;

                UpdatePulse0Enable();
            }
            get
            {
                return _SynchADCUsingPulseTrigger;
            }
        }

        [Display("Pulse4 Output Indicates", Groups: new[] { "Pulsed Receivers" }, Order: 42)]
        public bool Pulse4OutputEnabled { get; set; }

        [Display("Pulse4 Output Indicates", Groups: new[] { "Pulsed Receivers" }, Order: 42)]
        public Pulse4ModeEnumtype Pulse4Mode { get; set; }

        [Display("Offset Pulses using ADC Delay", Groups: new[] { "Pulsed Generators" }, Order: 51)]
        public bool OffsetPulsesUsingADCDelay { get; set; }

        [XmlIgnore]
        [EnabledIf("IsSettingReadOnly", true, HideIfDisabled = false)]
        [Browsable(true)]
        [Unit("sec", UseEngineeringPrefix: true, StringFormat: "0.0")]
        [Display("ADC Delay", Groups: new[] { "Pulsed Generators" }, Order: 52)]
        public double OffsetPulseADCDelay { get; set; }

        [Display("RF Modulator Delay", Groups: new[] { "Pulsed Generators" }, Order: 53)]
        [Unit("sec", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double RFModulatorDelay { get; set; }



        private List<string> _PulseGenListOfAvailableValues;
        [Display("Pulse Gen Values", "Editable list for Pulse Gen values", Groups: new[] { "Available Values Setup" }, Order: 101, Collapsed: true)]
        public List<string> PulseGenListOfAvailableValues
        {
            get { return _PulseGenListOfAvailableValues; }
            set
            {
                _PulseGenListOfAvailableValues = value;
                OnPropertyChanged("PulseGenListOfAvailableValues");
            }
        }

        #endregion


        private void UpdatePulse0Enable()
        {
            foreach (var a in this.ChildTestSteps)
            {
                if (a is Generator)
                {
                    if ((a as Generator).PulseName.Equals("Pulse0"))
                    {
                        (a as Generator).PulseEnable = _SynchADCUsingPulseTrigger;
                        return;
                    }
                }
            }
        }


        public PulseGenerators()
        {
            // ON >> CW
            // RearPanel >> External
            _PulseGenListOfAvailableValues = new List<string> { "On", "Off", "Pulse1", "Pulse2", "Pulse3", "Pulse4", "RearPanel" };

            Generator pulse0 = new Generator { IsControlledByParent = true, Channel = this.Channel, PulseName = "Pulse0", PulseWidth = 10e-9, PulseDelay = 250e-9 };
            this.ChildTestSteps.Add(pulse0);
            Generator pulse1 = new Generator { IsControlledByParent = true, Channel = this.Channel, PulseName = "Pulse1" };
            this.ChildTestSteps.Add(pulse1);
            Generator pulse2 = new Generator { IsControlledByParent = true, Channel = this.Channel, PulseName = "Pulse2" };
            this.ChildTestSteps.Add(pulse2);
            Generator pulse3 = new Generator { IsControlledByParent = true, Channel = this.Channel, PulseName = "Pulse3" };
            this.ChildTestSteps.Add(pulse3);
            Generator pulse4 = new Generator { IsControlledByParent = true, Channel = this.Channel, PulseName = "Pulse4" };
            this.ChildTestSteps.Add(pulse4);


            Source1EnableModulator = false;
            ALCOpenLoop = false;
            Source2EnableModulator = false;
            ModulatorDrive = "Pulse1";

            SynchADCUsingPulseTrigger = false;
            Pulse4OutputEnabled = false;
            Pulse4Mode = Pulse4ModeEnumtype.All;

            OffsetPulsesUsingADCDelay = false;
            OffsetPulseADCDelay = 210e-9;
            RFModulatorDelay = 40e-9;
        }

        public override void Run()
        {
            // Pulsed Sources
            PNAX.PulseGeneratorSource1EnableModulator(Channel, Source1EnableModulator);
            PNAX.PulseGeneratorALCOpenLoop(Channel, ALCOpenLoop);
            PNAX.PulseGeneratorSource2EnableModulator(Channel, Source2EnableModulator);
            PNAX.PulseGeneratorPulseGen(Channel, ModulatorDrive);

            // Pulsed Receivers
            PNAX.PulseGeneratorSyncADCs(Channel, SynchADCUsingPulseTrigger);
            PNAX.Pulse4Option(Channel, Pulse4OutputEnabled);
            PNAX.Pulse4Mode(Channel, Pulse4Mode);

            // Offset
            PNAX.PulseGeneratorADCDelay(Channel, OffsetPulsesUsingADCDelay);
            PNAX.PulseGeneratorModulatorDelay(Channel, RFModulatorDelay);
            OffsetPulseADCDelay = PNAX.PulseGeneratorFixedADCDelay(Channel);


            RunChildSteps(); // Pulse Generators

            UpgradeVerdict(Verdict.Pass);
        }

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            UpdateMetaData();
            List<(string, object)> retVal = new List<(string, object)>();

            retVal.Add(($"Source1EnableModulator", Source1EnableModulator));
            retVal.Add(($"ALCOpenLoop", ALCOpenLoop));
            retVal.Add(($"Source2EnableModulator", Source2EnableModulator));
            retVal.Add(($"ModulatorDrive", ModulatorDrive));
            retVal.Add(($"SynchADCUsingPulseTrigger", SynchADCUsingPulseTrigger));
            retVal.Add(($"Pulse4OutputEnabled", Pulse4OutputEnabled));
            retVal.Add(($"Pulse4Mode", Pulse4Mode));
            retVal.Add(($"OffsetPulsesUsingADCDelay", OffsetPulsesUsingADCDelay));
            retVal.Add(($"RFModulatorDelay", RFModulatorDelay));
            retVal.Add(($"OffsetPulseADCDelay", OffsetPulseADCDelay));

            foreach (var a in MetaData)
            {
                retVal.Add(a);
            }

            return retVal;
        }

        [Browsable(true)]
        [Display("Update MetaData", Groups: new[] { "MetaData" }, Order: 1000.2)]
        public override void UpdateMetaData()
        {
            MetaData = new List<(string, object)>();

            foreach (var ch in this.ChildTestSteps)
            {
                List<(string, object)> ret = (ch as Generator).GetMetaData();
                foreach (var it in ret)
                {
                    MetaData.Add(it);
                }
            }
        }

    }
}
