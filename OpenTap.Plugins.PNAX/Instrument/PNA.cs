﻿using OpenTap;
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
        public NoiseFigureConverterValues DefaultNoiseFigureConverterValues;
        public NoiseFigureConverterPowerValue DefaultNoiseFigureConverterPowerValues;
        public NoiseFigureConverterFrequencyValues DefaultNoiseFigureConverterFrequencyValues;
        public MixerSweepValue DefaultMixerSweepValue;

        public GeneralGainCompressionPowerValues DefaultGeneralGainCompressionPowerValues;

        public PNAX()
        {
            Name = "PNA-X";
        }

        /// <summary>
        /// MNUM: The Tr# that appears on the VNA screen is the third and most visible way to refer to a trace. 
        /// Since we already have a "Trace Number", we call this the Measurement Number in the VNA Help file. 
        /// This number is issued sequentially by the VNA regardless of channel and window. 
        /// It is therefore unique among all traces
        /// </summary>
        protected int mnum = 1;

        /// <summary>
        /// Open procedure for the instrument.
        /// </summary>
        public override void Open()
        {

            base.Open();

            this.QueryErrorAfterCommand = false;
            // TODO:  Open the connection to the instrument here

            ScpiCommand("SYST:FPR");
            ScpiCommand("DISP:WIND OFF");
            WaitForOperationComplete();

            //if (!IdnString.Contains("Instrument ID"))
            //{
            //    Log.Error("This instrument driver does not support the connected instrument.");
            //    throw new ArgumentException("Wrong instrument type.");
            // }

            mnum = 1;
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

        /// <summary>
        /// Generates a unique trace id aka MNUM or Measurement Number on the VNA help file
        /// </summary>
        /// <returns></returns>
        public int GetUniqueTraceId()
        {
            return mnum++;
        }

        public int GetNewTraceID(int Channel)
        {
            int TraceCount = 0;
            // Query Channel for list of measurements
            var ListOfMeas = ScpiQuery($"CALC{Channel}:PAR:CAT:EXT?");
            ListOfMeas = ListOfMeas.Replace("\n", "");
            String[] list = ListOfMeas.Split(',');

            if (list[0].Equals("\"NO CATALOG\""))
            {
                // channel does not have any traces
                TraceCount = 1;
            }
            else
            {
                int listCount = list.Count();
                TraceCount = (listCount / 2) + 1;
            }

            return TraceCount++;
        }

        public int GetNewWindowTraceID(int Window)
        {
            int TraceCount = 0;

            // first lets see if the window exists
            int IsWindowActive = ScpiQuery<int>($"DISPlay:WINDow{Window}:STATe?");
            if (IsWindowActive == 0)
            {
                // does not exist
                // thus, channel does not have any traces
                TraceCount = 1;
            }
            else
            {
                // exists
                // now lets find trace count in this window
                var ListOfTracesInWindow = ScpiQuery($"DISPlay:WINDow{Window}:CAT?");
                ListOfTracesInWindow = ListOfTracesInWindow.Replace("\n", "");
                String[] listTraces = ListOfTracesInWindow.Split(',');

                if (listTraces[0].Equals("\"NO CATALOG\""))
                {
                    // channel does not have any traces
                    TraceCount = 1;
                }
                else
                {
                    int listCount = listTraces.Count();
                    TraceCount = listCount + 1;
                }
            }




            return TraceCount++;
        }

        public override void ScpiCommand(string command)
        {
            base.ScpiCommand(command);

            WaitForOperationComplete();
            List<ScpiError> errors = base.QueryErrors();

            if (errors.Count > 0)
            {
                String errorString = String.Join(",", errors.ToArray());
                throw new Exception($"Error: {errorString} while sending command: {command}");
            }
        }

        public override string ScpiQuery(string query, bool isSilent = false)
        {
            String strRet = base.ScpiQuery(query, isSilent);
            strRet = strRet.Replace("\n", "");
            return strRet;
        }
    }
}
