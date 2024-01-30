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
    public enum TriggerSourceEnumType
    {
        [Scpi("EXT")]
        [Display("External")]
        EXT,
        [Scpi("IMM")]
        [Display("Internal")]
        IMM,
        [Scpi("MAN")]
        [Display("Manual")]
        MAN
    }

    public enum TriggerModeEnumType
    {
        [Scpi("CHAN")]
        [Display("Channel")]
        CHAN,
        [Scpi("SWE")]
        [Display("Sweep")]
        SWE,
        [Scpi("POIN")]
        [Display("Point")]
        POIN,
        [Scpi("TRAC")]
        [Display("Trace")]
        TRAC
    }

    public enum SweepModeEnumType
    {
        [Scpi("HOLD")]
        [Display("Hold")]
        HOLD,
        [Scpi("CONT")]
        [Display("Continuous")]
        CONT,
        [Scpi("GRO")]
        [Display("Groups")]
        GRO,
        [Scpi("SING")]
        [Display("Single")]
        SING
    }

    [Display("PNA-X", Group: "Network Analyzer", Description: "Insert a description here")]
    public partial class PNAX : ScpiInstrument
    {
        #region Settings
        [Display("Always Preset VNA", "When enbaled, the instrument driver will send SYST:FPR at the start of every test run.", Group: "Instrument Settings", Order: 3)]
        public bool isAlwaysPreset { get; set; }

        [Display("External Devices Policy", "Set and return whether External Devices remain activated or are de-activated when the VNA is Preset or when a Instrument State is recalled.\nOFF (0)  External devices remain active when the VNA is Preset or when a Instrument State is recalled.\nON (1)  External devices are de-activated (SYST:CONF:EDEV:STAT to OFF) when the VNA is Preset or when a Instrument State is recalled.", Group: "Instrument Settings", Order: 2)]
        public bool ExternalDevices { get; set; }

        [Display("Query for Errors", "Send SYST:ERR after every command. Useful for debugging", Group: "Instrument Settings", Order: 3)]
        public bool IsQueryForErrors { get; set; }

        [Display("Query for OPC", "Send OPC? after every command. Useful for debugging", Group: "Instrument Settings", Order: 4)]
        public bool IsWaitForOpc { get; set; }
        
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

        private bool IsModelA = false;

        public PNAX()
        {
            Name = "PNA-X";
            isAlwaysPreset = true;
            ExternalDevices = true;
            IsQueryForErrors = true;
            IsWaitForOpc = true;
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

            //if (this.ExternalDevices)
            //{
            //    ScpiCommand("SYSTem:PREFerences:ITEM:EDEV:DPOLicy ON");
            //    WaitForOperationComplete();
            //}
            //else
            //{
            //    ScpiCommand("SYSTem:PREFerences:ITEM:EDEV:DPOLicy OFF");
            //    WaitForOperationComplete();
            //}
            string[] IDNValues = IdnString.Split(',');
            if (IDNValues[1].StartsWith("N") && IDNValues[1].EndsWith("A"))
            {
                // We have a PNA instrument from the A family i.e. N5235A
                IsModelA = true;
                isAlwaysPreset = false;
            }

            if (isAlwaysPreset)
            {
                Preset();
            }

            
        }

        public void Preset()
        {
            ScpiCommand("SYST:FPR");
            if (!IsModelA)
            {
                ScpiCommand("DISP:WIND OFF");
            }
            WaitForOperationComplete();
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

        [Browsable(false)]
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
            string[] list = ListOfMeas.Split(',');

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
                string[] listTraces = ListOfTracesInWindow.Split(',');

                if (listTraces[0].Equals("\"NO CATALOG\"") || listTraces[0].Equals("\"EMPTY\""))
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

            if (IsWaitForOpc)
            {
                WaitForOperationComplete();
            }

            if (IsQueryForErrors)
            {
                List<ScpiError> errors = base.QueryErrors();

                if (errors.Count > 0)
                {
                    string errorString = string.Join(",", errors.ToArray());
                    throw new Exception($"Error: {errorString} while sending command: {command}");
                }
            }
        }

        public void ScpiCommand(string command, int timeOut)
        {
            base.ScpiCommand(command);

            if (IsWaitForOpc)
            {
                WaitForOperationComplete(timeOut);
            }

            if (IsQueryForErrors)
            {
                List<ScpiError> errors = base.QueryErrors();

                if (errors.Count > 0)
                {
                    string errorString = string.Join(",", errors.ToArray());
                    throw new Exception($"Error: {errorString} while sending command: {command}");
                }
            }
        }

        public override string ScpiQuery(string query, bool isSilent = false)
        {
            string strRet = base.ScpiQuery(query, isSilent);
            strRet = strRet.Replace("\n", "");
            return strRet;
        }

        public void SetTriggerSource(TriggerSourceEnumType trigerSource)
        {
            string scpi = Scpi.Format("{0}", trigerSource);
            ScpiCommand($"TRIGger:SOURce {scpi}");
        }

        public void SetTriggerMode(int Channel, TriggerModeEnumType triggerMode)
        {
            string scpi = Scpi.Format("{0}", triggerMode);
            ScpiCommand($"SENSe{Channel}:SWEep:TRIGger:MODE {scpi}");
        }

        public void SetSweepMode(int Channel, SweepModeEnumType sweepMode)
        {
            string scpi = Scpi.Format("{0}", sweepMode);
            ScpiCommand($"SENSe{Channel}:SWEep:MODE {scpi}");
        }

        public void SendTrigger(int Channel)
        {
            ScpiCommand($"INITiate{Channel}:IMMediate");
        }

        public List<string> SourceCatalog(int Channel)
        {
            string retString = ScpiQuery($"SOURce{Channel}:CATalog?");
            retString = retString.Replace("\"", "");
            List<string> retVal = retString.Split(',').ToList<string>();
            return retVal;
        }

        public List<string> CalsetCatalog()
        {
            string retString = ScpiQuery($"CSET:CATalog?");
            retString = retString.Replace("\"", "");
            List<string> retVal = retString.Split(',').ToList<string>();
            return retVal;
        }

        public void LoadCalset(int Channel, string calset, bool UseCalSetStimulus = true)
        {
            String StateStr = "OFF";
            if (UseCalSetStimulus)
            {
                StateStr = "ON";
            }

            List<String> calsets = CalsetCatalog();

            // Find if any of the calsets on the instruments matches the desired calset
            bool calset_exists = calsets.Any(s => s.Equals(calset));

            if (!calset_exists)
            {
                throw new Exception("selected Cal set does not exist!");
            }

            ScpiCommand($"SENSe{Channel}:CORRection:CSET:ACTivate \"{calset}, {StateStr}");
        }
    }
}
