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


    public enum VNAReferenceOscillatorEnumtype
    {
        [Display("Internal")]
        [Scpi("INT")]
        Internal,
        [Display("External")]
        [Scpi("EXT")]
        External,
        [Display("PXI Backplane")]
        [Scpi("PXI")]
        PXI
    }


    public enum VNAReferenceFreqEnumtype
    {
        [Display("10 MHz")]
        [Scpi("1E7")]
        Ten,
        [Display("100 MHz")]
        [Scpi("1E8")]
        Hundred,
    }


    public enum VNAReferenceInEnumtype
    {
        [Display("Internal")]
        Internal,
        [Display("External 10 MHz")]
        External10,
        [Display("External 100 MHz")]
        External100,
        [Display("PXI Backplane")]
        PXIBackplane
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

        [Display("Enable Reference Oscillator Settings", Group: "Reference", Order: 10)]
        public bool EnableReferenceOscillatorSettings { get; set; }

        [EnabledIf("EnableReferenceOscillatorSettings", true, HideIfDisabled = true)]
        [Display("Reference In", "Set the frequency reference of the instrument", Group: "Reference", Order: 11)]
        public VNAReferenceInEnumtype VNAReferenceIn { get; set; }

        [EnabledIf("EnableReferenceOscillatorSettings", true, HideIfDisabled = true)]
        [Display("Reference Out", "Set Reference out frequency", Group: "Reference", Order: 12)]
        public VNAReferenceFreqEnumtype VNAReferenceOut { get; set; }

        [Display("Store SNP Block List", "Measurement Classes for which SNP can't be stored", Group: "Store SNP Settings", Order: 31, Collapsed: true)]
        public List<String> StoreSnpBlockList { get; set; }
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

        public bool OptionS93088;

        public PNAX()
        {
            Name = "PNA-X";
            isAlwaysPreset = true;
            ExternalDevices = true;
            IsQueryForErrors = true;
            IsWaitForOpc = true;

            EnableReferenceOscillatorSettings = false;
            VNAReferenceIn = VNAReferenceInEnumtype.Internal;
            VNAReferenceOut = VNAReferenceFreqEnumtype.Ten;

            StoreSnpBlockList = new List<string>() { "Differential I/Q", "Differential IQ" };

            OptionS93088 = true;
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

            string[] OPTValues = ScpiQuery("*OPT?").Split(',');
            OptionS93088 = OPTValues.Any(s => s.Equals("088"));
            if (!OptionS93088)
            {
                Log.Warning("Option S93088A/B not available, SOURce:PHASe commands will be skipped");
            }

            if (EnableReferenceOscillatorSettings)
            {
                SetReferenceIn(VNAReferenceIn);
                SetVNAOutputReferenceOscillatorFrequency(VNAReferenceOut);
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
            int deftimeout = IoTimeout;
            IoTimeout = 60000;

            string scpi = Scpi.Format("{0}", sweepMode);
            ScpiCommand($"SENSe{Channel}:SWEep:MODE {scpi}");

            IoTimeout = deftimeout;
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

        public void SetVNAReferenceOscillator(VNAReferenceOscillatorEnumtype referenceOscillator)
        {
            string state = Scpi.Format("{0}", referenceOscillator);
            ScpiCommand($"SENSe:ROSCillator:SOURce {state}");
        }

        public void SetVNAReferenceOscillatorFrequency(VNAReferenceFreqEnumtype referenceFreq)
        {
            string freq = Scpi.Format("{0}", referenceFreq);
            ScpiCommand($"SENSe:ROSCillator:EXTernal:FREQuency {freq}");
        }

        public void SetVNAOutputReferenceOscillatorFrequency(VNAReferenceFreqEnumtype referenceFreq)
        {
            string freq = Scpi.Format("{0}", referenceFreq);
            ScpiCommand($"SENSe:ROSCillator:OUTPut:FREQuency {freq}");
        }

        public void SetReferenceIn(VNAReferenceInEnumtype refIn)
        {
            switch (refIn)
            {
                case VNAReferenceInEnumtype.Internal:
                    SetVNAReferenceOscillator(VNAReferenceOscillatorEnumtype.Internal);
                    break;
                case VNAReferenceInEnumtype.External10:
                    SetVNAReferenceOscillator(VNAReferenceOscillatorEnumtype.External);
                    SetVNAReferenceOscillatorFrequency(VNAReferenceFreqEnumtype.Ten);
                    break;
                case VNAReferenceInEnumtype.External100:
                    SetVNAReferenceOscillator(VNAReferenceOscillatorEnumtype.External);
                    SetVNAReferenceOscillatorFrequency(VNAReferenceFreqEnumtype.Hundred);
                    break;
                case VNAReferenceInEnumtype.PXIBackplane:
                    SetVNAReferenceOscillator(VNAReferenceOscillatorEnumtype.PXI);
                    break;
            }
        }

        public bool GetDriveAccess()
        {
            return ScpiQuery<bool>("SYSTem:COMMunicate:DRIVe:ENABle?");
        }
    }
}
