using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

//Note this template assumes that you have a SCPI based instrument, and accordingly
//extends the ScpiInstrument base class.

//If you do NOT have a SCPI based instrument, you should modify this instance to extend
//the (less powerful) Instrument base class.

namespace OpenTap.Plugins.PNAX
{
    [Display("PNA-X", Group: "PNA-X", Description: "Insert a description here")]
    public partial class PNAX : ScpiInstrument
    {
        #region Settings

        #endregion

        private int TraceCount = 0;
        public StandardChannelValues DefaultStandardChannelValues;
        public MixerPowerValues DefaultMixerPowerValues;
        public MixerFrequencyValues DefaultMixerFrequencyValues;
        public MixerSetupValues DefaultMixerSetupValues;
        public ToneFrequencyValues DefaultToneFrequencyValues;
        public TonePowerValues DefaultTonePowerValues;
        public ConverterCompressionValues DefaultConverterCompressionValues;
        public ConverterFrequencyValues DefaultConverterFrequencyValues;
        public MixerConverterPowerValue DefaultMixerConverterPowerValues;
        public ScalarMixerConverterPowerValue DefaultScalarMixerConverterPowerValues;
        public NoiseFigureConverterPowerValue DefaultNoiseFigureConverterPowerValues;
        public MixerSweepValue DefaultMixerSweepValue;


        public PNAX()
        {
            Name = "PNA-X";
        }

        /// <summary>
        /// Open procedure for the instrument.
        /// </summary>
        public override void Open()
        {

            base.Open();

            this.QueryErrorAfterCommand = true;
            // TODO:  Open the connection to the instrument here

            ScpiCommand("SYST:FPR");
            WaitForOperationComplete();
            TraceCount = 1;

            //if (!IdnString.Contains("Instrument ID"))
            //{
            //    Log.Error("This instrument driver does not support the connected instrument.");
            //    throw new ArgumentException("Wrong instrument type.");
            // }

        }

        /// <summary>
        /// Close procedure for the instrument.
        /// </summary>
        public override void Close()
        {
            // TODO:  Shut down the connection to the instrument here.
            base.Close();
        }

        [Browsable(true)]
        [Display("Update Default Values", Group: "Instrument Settings", Order: 3)]
        public void UpdateDefaultValues()
        {
            if (IsConnected)
            {
                Log.Info("Disconnect before Updating ports!");
                return;
            }
            UpdateStandardValues();
            UpdateMixerSetupValues();
            UpdateMixerPowerValues();
            UpdateMixerFrequencyValues();
            UpdateToneFrequencyValues();
            UpdateTonePowerValues();
            UpdateCompressionValues();
            UpdateConverterFrequencyValues();
            UpdateConverterPowerValues();
            UpdateMixerSweepDefaultValues();
        }

        public int GetNewTraceID()
        {
            return TraceCount++;
        }
    }
}
