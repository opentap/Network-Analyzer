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
    [AllowAsChildIn(typeof(ScalarMixerChannel))]
    [Display("Scalar Mixer Power", Groups: new[] { "PNA-X", "Converters", "Scalar Mixer Converter + Phase" }, Description: "Insert a description here")]
    public class ScalarMixerPower : PowerBaseStep
    {
        #region Settings
        private ScalerMixerSweepType _SweepType;
        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
        [Display("Sweep Type", Order: 0.5)]
        public ScalerMixerSweepType SweepType
        {
            get
            {
                return _SweepType;
            }
            set
            {
                _SweepType = value;
                if (_SweepType == ScalerMixerSweepType.SegmentSweep)
                {
                    PortPowersCoupled = false;
                    // Port Powers coupling disabled in segment sweep mode
                    HasPortPowersCoupled = false;
                }
                else
                {
                    HasPortPowersCoupled = true;
                }
            }
        }


        [Browsable(false)]
        public bool EnablePowerSweepOutputEdit { get; set; } = false;

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

        [EnabledIf("AutoOutputPortSourceAttenuator", false)]
        [Display("Power Level", Group: "DUT Output Port", Order: 31)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public override double OutputPower { get; set; }

        [EnabledIf("EnablePowerSweepOutputEdit", true)]
        [Display("Start Power", Group: "Dut Input Port Power Sweep", Order: 40)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double InputPowerSweepStartPower { get; set; }

        [EnabledIf("EnablePowerSweepOutputEdit", true)]
        [Display("Stop Power", Group: "Dut Input Port Power Sweep", Order: 41)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double InputPowerSweepStopPower { get; set; }

        [EnabledIf("EnablePowerSweepOutputEdit", true)]
        [Display("Points", Group: "Dut Input Port Power Sweep", Order: 42)]
        public int InputPowerSweepPowerPoints { get; set; }

        [EnabledIf("EnablePowerSweepOutputEdit", true)]
        [Display("Power Step", Group: "Dut Input Port Power Sweep", Order: 43)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double InputPowerSweepPowerStep { get; set; }

        [EnabledIf("EnablePowerSweepOutputEdit", true)]
        [Display("Start Power", Group: "Dut Output Port Power Sweep", Order: 50)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double OutputPowerSweepStartPower { get; set; }

        [EnabledIf("EnablePowerSweepOutputEdit", true)]
        [Display("Stop Power", Group: "Dut Output Port Power Sweep", Order: 51)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double OutputPowerSweepStopPower { get; set; }
        #endregion

        public ScalarMixerPower()
        {
            HasPortPowersCoupled = true;
            HasAutoInputPort = true;
            UpdateDefaultValues();
        }

        private void UpdateDefaultValues()
        {
            var defaultValuesSetup = PNAX.GetMixerSetupDefaultValues();
            PortInput  = defaultValuesSetup.PortInput;
            PortOutput = defaultValuesSetup.PortOutput;

            var DefaultValues = PNAX.GetScalarMixerConverterPowerDefaultValues();
            PowerOnAllChannels = DefaultValues.PowerOnAllChannels;
            PortPowersCoupled = DefaultValues.PortPowersCoupled;

            InputPower                    = DefaultValues.InputPortLinearInputPower;
            InputPortSourceAttenuator     = DefaultValues.InputPortSourceAttenuator;
            AutoInputPortSourceAttenuator = DefaultValues.AutoInputPortSourceAttenuator;
            InputPortReceiverAttenuator   = DefaultValues.InputPortReceiverAttenuator;
            InputSourceLevelingMode       = DefaultValues.InputSourceLevelingMode;

            OutputPower                    = DefaultValues.OutputPortReversePower;
            AutoOutputPortSourceAttenuator = DefaultValues.AutoOutputPortSourceAttenuator;
            OutputPortSourceAttenuator     = DefaultValues.OutputPortSourceAttenuator;
            OutputPortReceiverAttenuator   = DefaultValues.OutputPortReceiverAttenuator;
            OutputSourceLevelingMode       = DefaultValues.OutputSourceLevelingMode;

            InputPortSourceAttenuatorAutoValue  = InputPortSourceAttenuator;
            OutputPortSourceAttenuatorAutoValue = OutputPortSourceAttenuator;

            InputPowerSweepStartPower = DefaultValues.InputPowerSweepStartPower;
            InputPowerSweepStopPower = DefaultValues.InputPowerSweepStopPower;
            InputPowerSweepPowerPoints = DefaultValues.InputPowerSweepPowerPoints;
            InputPowerSweepPowerStep = DefaultValues.InputPowerSweepPowerStep;
            OutputPowerSweepStartPower = DefaultValues.OutputPowerSweepStartPower;
            OutputPowerSweepStopPower = DefaultValues.OutputPowerSweepStopPower;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetPowerOnAllChannels(PowerOnAllChannels);
            PNAX.SetCoupledTonePowers(Channel, PortPowersCoupled);

            PNAX.SetSMCPortInputOutput(Channel, PortInput, PortOutput);

            PNAX.SetPowerLevel(Channel, PortInput, InputPower);
            PNAX.SetSourceAttenuator(Channel, (int)PortInput, InputPortSourceAttenuator);
            PNAX.SetReceiverAttenuator(Channel, (int)PortInput, InputPortReceiverAttenuator);
            PNAX.SetSourceLevelingMode(Channel, PortInput, InputSourceLevelingMode);
            PNAX.SetSourceAttenuatorAutoMode(Channel, PortInput, AutoInputPortSourceAttenuator);


            PNAX.SetPowerLevel(Channel, PortOutput, OutputPower);
            PNAX.SetSourceAttenuator(Channel, (int)PortOutput, OutputPortSourceAttenuator);
            PNAX.SetReceiverAttenuator(Channel, (int)PortOutput, OutputPortReceiverAttenuator);
            PNAX.SetSourceLevelingMode(Channel, PortOutput, OutputSourceLevelingMode);
            PNAX.SetSourceAttenuatorAutoMode(Channel, PortOutput, AutoOutputPortSourceAttenuator);

            if (EnablePowerSweepOutputEdit)
            {
                PNAX.SetSMCPowerSweepStartPower(Channel, PortInput, InputPowerSweepStartPower);
                PNAX.SetSMCPowerSweepStopPower(Channel, PortInput, InputPowerSweepStopPower);
            }

            if (PortPowersCoupled)
            {
                // Use the same values for input port
                PNAX.SetSMCPowerSweepStartPower(Channel, PortOutput, InputPowerSweepStartPower);
                PNAX.SetSMCPowerSweepStopPower(Channel, PortOutput, InputPowerSweepStopPower);
            }
            else
            {
                // use the values for output port
                PNAX.SetSMCPowerSweepStartPower(Channel, PortOutput, OutputPowerSweepStartPower);
                PNAX.SetSMCPowerSweepStopPower(Channel, PortOutput, OutputPowerSweepStopPower);
            }

            UpgradeVerdict(Verdict.Pass);
        }

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            List<(String, object)> retVal = new List<(string, object)>
            {
                ("PowerOnAllChannels", PowerOnAllChannels),
                ("PortPowersCoupled", PortPowersCoupled),

                ("SMC PortInput", PortInput),
                ("SMC PortOutput", PortOutput),

                ("SMC_InputPower", InputPower),
                ("SMC_InputPortSourceAttenuator", InputPortSourceAttenuator),
                ("SMC_InputPortReceiverAttenuator", InputPortReceiverAttenuator),
                ("SMC_InputSourceLevelingMode", InputSourceLevelingMode),
                ("SMC_AutoInputPortSourceAttenuator", AutoInputPortSourceAttenuator),

                ("SMC_OutputPower", OutputPower),
                ("SMC_OutputPortSourceAttenuator", OutputPortSourceAttenuator),
                ("SMC_OutputPortReceiverAttenuator", OutputPortReceiverAttenuator),
                ("SMC_OutputSourceLevelingMode", OutputSourceLevelingMode),
                ("SMC_AutoOutputPortSourceAttenuator", AutoOutputPortSourceAttenuator)
            };

            if (EnablePowerSweepOutputEdit)
            {
                retVal.Add(("SMC_InputPowerSweepStartPower", InputPowerSweepStartPower));
                retVal.Add(("SMC_InputPowerSweepStopPower", InputPowerSweepStopPower));
            }

            if (PortPowersCoupled)
            {
                retVal.Add(("SMC_OutputPowerSweepStartPower", InputPowerSweepStartPower));
                retVal.Add(("SMC_OutputPowerSweepStopPower", InputPowerSweepStopPower));
            }
            else
            {
                retVal.Add(("SMC_OutputPowerSweepStartPower", OutputPowerSweepStartPower));
                retVal.Add(("SMC_OutputPowerSweepStopPower", OutputPowerSweepStopPower));
            }

            return retVal;
        }

    }
}
