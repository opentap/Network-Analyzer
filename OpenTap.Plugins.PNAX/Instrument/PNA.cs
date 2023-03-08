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

        public PNAX()
        {
            Name = "PNA-X";
            // ToDo: Set default values for properties / settings.
        }

        /// <summary>
        /// Open procedure for the instrument.
        /// </summary>
        public override void Open()
        {

            base.Open();
            // TODO:  Open the connection to the instrument here

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
            }
            else
            {
                Open();
                Log.Info("Getting default values for Standard Channel");

                this.ScpiCommand("SYSTem:PRESet");
                this.WaitForOperationComplete();

                // Channel 1 is Standard
                String sweepType = GetStandardSweepType(1);
                double start = GetStart(1);
                double stop = GetStop(1);
                double power = GetPower(1);
                int points = GetPoints(1);
                double ifbw = GetIFBandwidth(1);
                double startpower = GetStartPower(1);
                double stoppower = GetStopPower(1);
                double cwfreq = GetCWFreq(1);
                double startphase = GetPhaseStart(1);
                double stopphase = GetPhaseStop(1);

                Log.Info($"Sweep Type: {sweepType}");
                Log.Info($"Start: {start}");
                Log.Info($"Stop: {stop}");
                Log.Info($"Power: {power}");
                Log.Info($"Number of Points: {points}");
                Log.Info($"IF Bandwidth: {ifbw}");
                Log.Info($"Start Power: {startpower}");
                Log.Info($"Stop Power: {stoppower}");
                Log.Info($"CW Freq: {cwfreq}");
                Log.Info($"Start Phase: {startphase}");
                Log.Info($"Stop Phase: {stopphase}");




                Log.Info("Getting default values for Gain Compression Converters Channel");
                // Lets create Channel 2 - Gain Compression Converters
                ScpiCommand("CALC2:MEAS2:DEF \"SC21:Gain Compression Converters\"");
                WaitForOperationComplete();

                int converterstages = GetConverterStages(2);
                int portinput = GetPortInput(2);
                int portoutput = GetPortOutput(2);
                int inputfractionalmultipliernumerator = GetInputFractionalMultiplierNumerator(2);
                int inputfractionalmultiplierdenominator = GetInputFractionalMultiplierDenominator(2);
                int lo1fractionalmultipliernumerator = GetLOFractionalMultiplierNumerator(2, 1);
                int lo1fractionalmultiplierdenominator = GetLOFractionalMultiplierDenominator(2, 1);
                //int lo2fractionalmultipliernumerator = GetLOFractionalMultiplierNumerator(2, 2);
                //int lo2fractionalmultiplierdenominator = GetLOFractionalMultiplierDenominator(2, 2);
                String portlo1 = GetPortLO(2, 1);
                //String portlo2 = GetPortLO(2, 2);
                Log.Info($"Converter Stages: {converterstages}");
                Log.Info($"Port Input: {portinput}");
                Log.Info($"Port Output: {portoutput}");
                Log.Info($"Input Fractional Multiplier Numerator: {inputfractionalmultipliernumerator}");
                Log.Info($"Input Fractional Multiplier Denominator: {inputfractionalmultiplierdenominator}");
                Log.Info($"LO1 Fractional Multiplier Numerator: {lo1fractionalmultipliernumerator}");
                Log.Info($"LO1 Fractional Multiplier Denominator: {lo1fractionalmultiplierdenominator}");
                //Log.Info($"LO2 Fractional Multiplier Numerator: {lo2fractionalmultipliernumerator}");
                //Log.Info($"LO2 Fractional Multiplier Denominator: {lo2fractionalmultiplierdenominator}");
                Log.Info($"Port LO1: {portlo1}");

                Close();
            }
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
