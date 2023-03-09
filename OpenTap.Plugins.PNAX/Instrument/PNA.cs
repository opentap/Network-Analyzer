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
        private StandardChannelValues standardChannelValues;
        private MixerPowerValues mixerPowerValues;
        private MixerFrequencyValues mixerFrequencyValues;

        public PNAX()
        {
            Name = "PNA-X";
            standardChannelValues = new StandardChannelValues();
            // ToDo: Set default values for properties / settings.
        }

        /// <summary>
        /// Open procedure for the instrument.
        /// </summary>
        public override void Open()
        {

            base.Open();
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
            UpdateMixerPowerValues();
            UpdateMixerFrequencyValues();
        }

        public int GetNewTraceID()
        {
            return TraceCount++;
        }

        #region Mixer Setup
        public int GetConverterStages(int Channel)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENSe{Channel.ToString()}:MIXer:STAGe?");
            return retVal;
        }

        public int GetPortInput(int Channel)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENS{Channel.ToString()}:MIX:PMAP:INP?");
            return retVal;
        }

        public int GetPortOutput(int Channel)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENS{Channel.ToString()}:MIX:PMAP:OUTP?");
            return retVal;
        }

        public int GetInputFractionalMultiplierNumerator(int Channel)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENSe{Channel.ToString()}:MIXer:INPut:FREQ:NUMerator?");
            return retVal;
        }

        public int GetInputFractionalMultiplierDenominator(int Channel)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENSe{Channel.ToString()}:MIXer:INPut:FREQ:DENominator?");
            return retVal;
        }

        public int GetLOFractionalMultiplierNumerator(int Channel, int Stage)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENSe{Channel.ToString()}:MIXer:LO{Stage.ToString()}:FREQuency:NUMerator?");
            return retVal;
        }

        public int GetLOFractionalMultiplierDenominator(int Channel, int Stage)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENSe{Channel.ToString()}:MIXer:LO{Stage.ToString()}:FREQuency:DENominator?");
            return retVal;
        }

        // SENSe<ch>:MIXer:LO<n>:NAME?
        public String GetPortLO(int Channel, int Stage)
        {
            String retVal;
            retVal = ScpiQuery($"SENSe{Channel.ToString()}:MIXer:LO{Stage.ToString()}:NAME?");
            return retVal;
        }
        #endregion

    }
}
