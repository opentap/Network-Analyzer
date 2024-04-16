using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

//Note this template assumes that you have a SCPI based instrument, and accordingly
//extends the ScpiInstrument base class.

//If you do NOT have a SCPI based instrument, you should modify this instance to extend
//the (less powerful) Instrument base class.

namespace OpenTap.Plugins.PNAX
{
    public enum CalModeEnum
    {
        SYNC,
        ASYN
    }

    public enum ChannelTypeEnum
    {
        [Display("Standard")]
        Standard,
        [Display("Gain Compression")]
        GainCompression,
        [Display("Swept IMD")]
        SweptIMD,
        [Display("Noise Figure Cold Source")]
        NoiseFigureColdSource,
        [Display("Scalar Mixer/Converter")]
        ScalarMixerConverter,
        [Display("Gain Compression Converters")]
        GainCompressionConverters,
        [Display("Swept IMD Converters")]
        SweptIMDConverters,
        [Display("Noise Figure Converters")]
        NoiseFigureConverters
    }


    public enum PNAPowerMeterEnumtype
    {
        [Display("GPIB")]
        [Scpi("GPIB")]
        Gpib,
        [Display("USB")]
        [Scpi("USB")]
        Usb,
        [Display("LAN")]
        [Scpi("LAN")]
        Lan,
        [Display("ANY")]
        [Scpi("ANY")]
        Any
    }


    public enum FSimCalsetOverwriteCreateEnumtype
    {
        [Display("Over-write selected Calsets")]
        Overwrite,
        [Display("Create New Calsets")]
        New
    }

    public enum FSimEmbedTypeEnumtype
    {
        [Display("Embed")]
        [Scpi("EMB")]
        Embed,
        [Display("De-embed")]
        [Scpi("DEEM")]
        Deembed,
    }


    public class CalibrateAllSelectedChannels
    {
        public int Channel { get; set; }
        public ChannelTypeEnum ChannelType { get; set; }
        public List<int> Ports { get; set; }

        public CalibrateAllSelectedChannels()
        {
            Ports = new List<int>();
        }
    }

    [Flags]
    public enum ExtraPowerCalsEnum
    {
        [Display("No Independent Source Cal")]
        NoIndependentSourceCal = 0,
        [Display("Port 1")]
        Port1 = 1,
        [Display("Port 2")]
        Port2 = 2,
        [Display("Port 3")]
        Port3 = 4,
        [Display("Port 4")]
        Port4 = 8,
        [Display("Port 1 Src2")]
        Port1Src2 = 16,
        [Display("Source3")]
        Source3 = 32
    }

    public enum NoiseCalMethodEnum
    {
        Scalar,
        Vector
    }

    public enum NoiseTunerEnum
    {
        [Display("internal")]
        [Scpi("internal")]
        _internal
    }

    public enum ReceiverCharacterizationMethodEnum
    {
        [Display("Use Power Meter")]
        [Scpi("Use Power Meter")]
        UsePowerMeter,
        [Display("Use Noise Source")]
        [Scpi("Use Noise Source")]
        UseNoiseSource
    }

    public enum DUTConnectorsEnum
    {
        [Scpi("Type N (50) male")]
        [Display("Type N (50) male")]
        TypeN50male,
        [Scpi("Type N (50) female")]
        [Display("Type N (50) female")]
        TypeN50female,
        [Scpi("Type N (75) male")]
        [Display("Type N (75) male")]
        TypeN75male,
        [Scpi("Type N (75) female")]
        [Display("Type N (75) female")]
        TypeN75female,
        [Scpi("Type F (75) male")]
        [Display("Type F (75) male")]
        TypeF75male,
        [Scpi("Type F (75) female")]
        [Display("Type F (75) female")]
        TypeF75female,
        [Scpi("APC 3.5 male")]
        [Display("APC 3.5 male")]
        APC35male,
        [Scpi("APC 3.5 female")]
        [Display("APC 3.5 female")]
        APC35female,
        [Scpi("APC 2.4 male")]
        [Display("APC 2.4 male")]
        APC24male,
        [Scpi("APC 2.4 female")]
        [Display("APC 2.4 female")]
        APC24female,
        [Scpi("2.92 mm male")]
        [Display("2.92 mm male")]
        _292mmmale,
        [Scpi("2.92 mm female")]
        [Display("2.92 mm female")]
        _292mmfemale,
        [Scpi("1.85 mm female")]
        [Display("1.85 mm female")]
        _185mmfemale,
        [Scpi("1.85 mm male")]
        [Display("1.85 mm male")]
        _185mmmale,
        [Scpi("1.00 mm female")]
        [Display("1.00 mm female")]
        _100mmfemale,
        [Scpi("1.00 mm male")]
        [Display("1.00 mm male")]
        _100mmmale,
        [Scpi("7-16 male")]
        [Display("7-16 male")]
        _716male,
        [Scpi("7-16 female")]
        [Display("7-16 female")]
        _716female,
        [Scpi("APC 7")]
        [Display("APC 7")]
        APC7,
        [Scpi("X-band waveguide")]
        [Display("X-band waveguide")]
        Xbandwaveguide,
        [Scpi("P-band waveguide")]
        [Display("P-band waveguide")]
        Pbandwaveguide,
        [Scpi("K-band waveguide")]
        [Display("K-band waveguide")]
        Kbandwaveguide,
        [Scpi("Q-band waveguide")]
        [Display("Q-band waveguide")]
        Qbandwaveguide,
        [Scpi("R-band waveguide")]
        [Display("R-band waveguide")]
        Rbandwaveguide,
        [Scpi("U-band waveguide")]
        [Display("U-band waveguide")]
        Ubandwaveguide,
        [Scpi("V-band waveguide")]
        [Display("V-band waveguide")]
        Vbandwaveguide,
        [Scpi("W-band waveguide")]
        [Display("W-band waveguide")]
        Wbandwaveguide,
        [Scpi("Type A (50) male")]
        [Display("Type A (50) male")]
        TypeA50male,
        [Scpi("Type A (50) female")]
        [Display("Type A (50) female")]
        TypeA50female,
        [Scpi("Type B")]
        [Display("Type B")]
        TypeB,
        [Scpi("Not used")]
        [Display("Not used")]
        Notused
    }

    public enum CalKitEnum
    {
        [Scpi("85032F")]
        [Display("85032F")]
        _85032F,
        [Scpi("85032B/E")]
        [Display("85032B/E")]
        _85032BE,
        [Scpi("85054B")]
        [Display("85054B")]
        _85054B,
        [Scpi("85054D")]
        [Display("85054D")]
        _85054D
    }

    public partial class PNAX : ScpiInstrument
    {
        public static T ConvertStringToEnum<T>(string value)
        {
            string valueWOSpaces = value.Replace(" ", "");
            return (T)Enum.Parse(typeof(T), valueWOSpaces, ignoreCase: true);
        }

        public List<string> GetAllMeasClasses()
        {
            List<string> AllMeasClasses = new List<string>();
            List<int> Channels = GetActiveChannels();

            foreach(int ch in Channels)
            {
                ScpiCommand($"SYST:ACT:CHAN {ch}");
                WaitForOperationComplete();
                string mclass = ScpiQuery("SYSTem:ACTive:MCLass?");
                AllMeasClasses.Add(mclass);
            }
            return AllMeasClasses;
        }

        public string GetChannelType(int Channel)
        {
            string retString = ScpiQuery<string>($"SENSe{Channel}:CLASs:NAME?");
            retString = retString.Replace("\"", "");
            retString = retString.Replace("/", "");

            return retString;
        }

        public void CalAllStartRemotely()
        {
            if (IsModelA)
            {
                CalAllStartRemotelyWithMacro();
            }
            else
            {
                ScpiCommand("SYST:CORR:WIZ CALL");
            }
        }

        private int calAllMacro = -1;
        public void CalAllStartRemotelyWithMacro()
        {
            // setup macro the first time
            if (calAllMacro == -1)
            {
                // create macro file
                const string DEFAULT_FOLDER = @"C:\Users\Public\Documents\Network Analyzer\";
                const string CAL_ALL_SCRIPT_NAME = "CalibrateAll.vbs";
                const string CAL_ALL_TITLE = "CalibrateAll";
                ScpiCommand($@"MMEM:CDIR ""{DEFAULT_FOLDER}""");
                ScpiIEEEBlockCommand(
                    @"MMEM:TRAN ""CalibrateAll.vbs"",",
                    ASCIIEncoding.ASCII.GetBytes(@"Call CreateObject(""AgilentPNA835x.Application"").LaunchDialog(""CalibrateAll"")"));

                // find the macro slot
                int emptySlot = -1;
                for (int macroSlot = 24; macroSlot > 0; macroSlot--)
                {
                    string title = ScpiQuery($"SYST:SHOR{macroSlot}:TITLE?", true).Trim('\n', '"');
                    string path = ScpiQuery($"SYST:SHOR{macroSlot}:PATH?", true).Trim('\n', '"');

                    if (title == CAL_ALL_TITLE && path != string.Empty)
                    {
                        calAllMacro = macroSlot;
                        break;
                    }
                    else if (emptySlot == -1 && title == string.Empty && path == string.Empty)
                    {
                        emptySlot = macroSlot;
                    }
                }

                // create the macro
                if (calAllMacro == -1)
                {
                    if (emptySlot != -1)
                    {
                        ScpiCommand($@"SYST:SHOR{emptySlot}:TITLE ""{CAL_ALL_TITLE}""");
                        ScpiCommand($@"SYST:SHOR{emptySlot}:PATH ""{DEFAULT_FOLDER + CAL_ALL_SCRIPT_NAME}""");
                        calAllMacro = emptySlot;
                    }
                    else
                    {
                        throw new Exception("No empty slots for CalAll macro! Clear a slot and try again.");
                    }
                }
            }

            // call the macro
            ScpiCommand($"SYST:SHOR{calAllMacro}:EXEC");
        }

        public void CalAllReset()
        {
            ScpiCommand("SYST:CAL:ALL:RESet");
        }

        public List<int> CalAllSelectChannels()
        {
            // Get active channels using GetActiveChannels()
            List<int> Channels = GetActiveChannels();
            CalAllSelectChannels(Channels);
            return Channels;
        }

        public void CalAllSelectChannels(List<int> Channels)
        {
            string AllChannelsString = string.Join(",", Channels.ToArray());
            CalAllSelectChannels(AllChannelsString);
        }

        public void CalAllSelectChannels(string Channels)
        {
            ScpiCommand($"SYSTem:CALibrate:ALL:SELect {Channels}");
        }

        public void CalAllSelectPorts(List<int> Ports)
        {
            List<int> Channels = GetActiveChannels();
            foreach(int ch in Channels)
            {
                string AllPortsString = string.Join(",", Ports.ToArray());
                CalAllSelectPorts(ch, AllPortsString);
            }
        }

        public void CalAllSelectPorts(List<int> Channels, List<int> Ports)
        {
            foreach (int ch in Channels)
            {
                string AllPortsString = string.Join(",", Ports.ToArray());
                CalAllSelectPorts(ch, AllPortsString);
            }
        }

        public void CalAllSelectPorts(int Channel, List<int> Ports)
        {
            string AllPortsString = string.Join(",", Ports.ToArray());
            CalAllSelectPorts(Channel, AllPortsString);
        }

        public void CalAllSelectPorts(int Channel, string Ports)
        {
            ScpiCommand($"SYSTem:CALibrate:ALL:CHANnel{Channel}:PORTs:SELect {Ports}");
        }

        public void CalAllSetProperty(string prop, string value)
        {
            try
            {
                ScpiErrorsLogLevelOverrides[342] = LogEventType.Debug;
                ScpiCommand($"SYSTem:CALibrate:ALL:MCLass:PROPerty:VALue \"{prop}\",\"{value}\"");
            }
            catch (Exception ex)
            {
                // Error 342, may need error 341 also
                if (!ex.Message.StartsWith("Error: 342"))
                {
                    throw ex;
                }

            }
            //if (prop.Equals("Use Smart Cal Order") && IsModelA)
            //{
            //    // A model does not support this property
            //    return;
            //}
        }

        public void CalAllSelectDutConnectorType(int Channel, int Port, string ConnectorType)
        {
            ScpiCommand($"SENSe{Channel}:CORRection:COLLect:GUIDed:CONNector:PORT{Port}:SELect \"{ConnectorType}\"");
        }

        public void CalAllSelectCalKit(int Channel, int Port, string CalKit)
        {
            if (IsModelA)
            {
                ScpiCommand($"SENSe{Channel}:CORRection:COLLect:GUIDed:CKIT:PORT{Port} \"{CalKit}\"");
            }
            else
            {
                ScpiCommand($"SENSe{Channel}:CORRection:COLLect:GUIDed:CKIT:PORT{Port}:SELect \"{CalKit}\"");
            }
        }

        public string CalAllGetCalKits(int Channel, DUTConnectorsEnum connector)
        {
            string conn = Scpi.Format("{0}", connector);
            return ScpiQuery($"SENSe{Channel}:CORRection:COLLect:GUIDed:CKIT:CATalog? \"{conn}\"");
        }

        public int CalAllGuidedChannelNumber()
        {
            if (IsModelA)
            {
                return ScpiQuery<int>($"SYSTem:CALibrate:ALL:GUIDed:CHANnel?");
            }
            return ScpiQuery<int>($"SYSTem:CALibrate:ALL:GUIDed:CHANnel:VALue?");

        }

        public void CalAllInit(int CalChannel, string CalSetName = "", bool MatchCalSettings = false, CalModeEnum calMode = CalModeEnum.ASYN)
        {
            int MatchCalSettingsInt = 0;
            if (MatchCalSettings)
            {
                MatchCalSettingsInt = 1;
            }

            if (CalSetName != "")
            {
                // Make sure it exists
                var csetCatalogStr = ScpiQuery("CSET:CATalog?");
                List<string> csetCat = csetCatalogStr.Split(',').ToList();
                bool found = false;
                foreach(string cset in csetCat)
                {
                    if (cset.Equals(CalSetName))
                    {
                        // found it
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    ScpiCommand($"SENS:CORR:CSET:CRE '{CalSetName}'");
                }
            }

            ScpiCommand($"SENSe{CalChannel}:CORRection:COLLect:GUIDed:INITiate:IMMediate \"{CalSetName}\", {MatchCalSettingsInt}, {calMode}");
            //ScpiCommand($"SENSe{CalChannel}:CORRection:COLLect:GUIDed:INITiate:IMMediate");
        }

        public int CalAllNumberOfSteps(int CalChannel)
        {
            return ScpiQuery<int>($"SENSe{CalChannel}:CORRection:COLLect:GUIDed:STEPs?");
        }

        public string CalAllStepDescription(int CalChannel, int CalStep)
        {
            return ScpiQuery($"SENSe{CalChannel}:CORRection:COLLect:GUIDed:DESCription? {CalStep}");
        }

        public void CalAllStep(int CalChannel, int CalStep, CalModeEnum calMode = CalModeEnum.ASYN)
        {
            if (IsModelA)
            {
                ScpiCommand($"SENSe{CalChannel}:CORRection:COLLect:GUIDed:ACQuire STAN{CalStep}");
            }
            else
            {
                ScpiCommand($"SENSe{CalChannel}:CORRection:COLLect:GUIDed:ACQuire STAN{CalStep},{calMode}");
            }
        }

        public void CalAllSave(int CalChannel)
        {
            ScpiCommand($"SENSe{CalChannel}:CORRection:COLLect:GUIDed:SAVE:IMMediate");
        }

        public int SimulatorMode()
        {
            if (IsModelA) return 0;

            int mode = ScpiQuery<int>("SYST:ACT:SIMulator?");
            return mode;
        }

        public void SetPowerSensor(PNAPowerMeterEnumtype powerMeterType, string powerSensorString)
        {
            string pmType = Scpi.Format("{0}", powerMeterType);
            ScpiCommand($"SYSTem:COMMunicate:PSENsor {pmType}, \"{powerSensorString}\"");
        }

        public string ReadConnectedSensor()
        {
            String str = ScpiQuery("SYSTem:COMMunicate:USB:PMETer:CATalog? OFF");
            string selectedPowerSensor = str.Split(';').FirstOrDefault().Trim(' ', '"');
            return selectedPowerSensor;
        }

        public void PowerMeterSettlingTolerance(double tolerance)
        {
            ScpiCommand($"SOURce:POWer:CORRection:COLLect:AVERage:NTOLerance {tolerance}");
        }

        public void PowerMeterSettlingMaxReadings(int readings)
        {
            ScpiCommand($"SOURce:POWer:CORRection:COLLect:AVERage:COUNt {readings}");
        }

        public void PowerMeterPowerLevel(int Channel, int port, double level)
        {
            ScpiCommand($"SENSe{Channel}:CORRection:COLLect:GUIDed:PSENsor{port}:POWer:LEVel {level}");
        }

        public void PowerMeterSensorEnable(int Channel, int port, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:CORRection:COLLect:GUIDed:PSENsor{port}:STATe {stateValue}");
        }

        #region Fixtures

        public void CalPlaneManagerApplyAdapterFixtureDeembed(string cs1, string cs2, string s2p, int port, bool powerCorrection, bool extrapolation = false)
        {
            string compPwr = powerCorrection ? "ON" : "OFF";
            string extrap = extrapolation ? "ON" : "OFF";
            ScpiCommand($"CSET:FIXTure:DEEMbed \"{cs1}\",\"{cs2}\",\"{s2p}\",{port},{compPwr},{extrap}");
        }

        public void CalPlaneManagerApplyAdapterFixtureEmbed(string cs1, string cs2, string s2p, int port, bool powerCorrection, bool extrapolation = false)
        {
            string compPwr = powerCorrection ? "ON" : "OFF";
            string extrap = extrapolation ? "ON" : "OFF";
            ScpiCommand($"CSET:FIXTure:EMBed \"{cs1}\",\"{cs2}\",\"{s2p}\",{port},{compPwr},{extrap}");
        }

        public void CalSetActivate(int cnum, string cset, bool UseCalsetStim)
        {
            string stim = UseCalsetStim ? "ON" : "OFF";
            ScpiCommand($"SENSe{cnum}:CORRection:CSET:ACTivate {cset}, {stim}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnum"></param>
        /// <param name="order">Order of operations, where:
        /// 0 - Port Extension operation
        /// 1 - 2-Port DeEmbedding operation
        /// 2 - Port Matching operation
        /// 3 - Arbitrary Impedance operation
        /// </param>
        public void FSimSingleEndedOrder(int cnum, string order = "0,1,2,3")
        {
            ScpiCommand($"CALCulate{cnum}:FSIMulator:SENDed:OORDer {order}");
        }

        public void FSimCircuitReset(int cnum)
        {
            ScpiCommand($"CALCulate{cnum}:FSIMulator:DRAFt:CIRCuit:RESet");
        }

        public int FSimCircuitNext(int cnum)
        {
            return ScpiQuery<int>($"CALCulate{cnum}:FSIMulator:DRAFt:CIRCuit:NEXT?");
        }

        public void FSimCircuitAddFile(int cnum, int circN, int fixtportcount = 2)
        {
            ScpiCommand($"CALCulate{cnum}:FSIMulator:DRAFt:CIRCuit{circN}:ADD FILE, {fixtportcount}");
        }

        public void FSimCircuitSetPort(int cnum, int circN, int port)
        {
            ScpiCommand($"CALCulate{cnum}:FSIMulator:DRAFt:CIRCuit{circN}:VNA:PORTs {port}");
        }

        public void FSimCircuitSetFileName(int cnum, int circN, string filename)
        {
            ScpiCommand($"CALCulate{cnum}:FSIMulator:DRAFt:CIRCuit{circN}:FILE \"{filename}\"");
        }

        public void FSimCircuitState(int cnum, int circN, bool state)
        {
            string strState = state ? "ON" : "OFF";
            ScpiCommand($"CALCulate{cnum}:FSIMulator:DRAFt:CIRCuit{circN}:STATe {strState}");
        }

        public void FSimCircuitEmbedType(int cnum, int circN, FSimEmbedTypeEnumtype type)
        {
            string strType = Scpi.Format("{0}", type);
            ScpiCommand($"CALCulate{cnum}:FSIMulator:DRAFt:CIRCuit{circN}:EMBED:TYPE {strType}");
        }

        public void FSimCircuitExtrapolation(int cnum, int circN, bool state)
        {
            string strState = state ? "ON" : "OFF";
            ScpiCommand($"CALCulate{cnum}:FSIMulator:DRAFt:CIRCuit{circN}:FILE:EXTRapolate {strState}");
        }

        public void FSimCircuitPowerCompensate(int cnum, int pnum, bool state)
        {
            string strState = state ? "ON" : "OFF";
            ScpiCommand($"CALCulate{cnum}:FSIMulator:POWer:PORT{pnum}:COMPensate:STATe {strState}");
        }


        public void FSimApply(int cnum)
        {
            ScpiCommand($"CALCulate{cnum}:FSIMulator:APPLy");
        }

        public void FSimState(int cnum, bool state)
        {
            string strState = state ? "ON" : "OFF";
            ScpiCommand($"CALCulate{cnum}:FSIMulator:STATe {strState}");
        }
        #endregion

    }
}
