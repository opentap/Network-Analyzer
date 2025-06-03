using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;

//Note this template assumes that you have a SCPI based instrument, and accordingly
//extends the ScpiInstrument base class.

//If you do NOT have a SCPI based instrument, you should modify this instance to extend
//the (less powerful) Instrument base class.

namespace OpenTap.Plugins.PNAX
{
    #region enums
    static class EnumExtensions
    {
        public static List<string> MODGetFlagsScpi(this Enum flags)
        {
            List<string> retString = new List<string>();
            foreach (var value in Enum.GetValues(flags.GetType()).Cast<Enum>())
            {
                ulong mask = Convert.ToUInt64(flags);
                ulong bits = Convert.ToUInt64(value);
                if (bits == 0L)
                    continue; // skip the zero value
                if ((bits & mask) == bits)
                {
                    string paramName = Scpi.Format("{0}", value);
                    retString.Add(paramName);
                }
            }
            return retString;
        }

        public static List<string> MODGetFlagsDisplay(this Enum flags)
        {
            List<string> retString = new List<string>();
            foreach (var value in Enum.GetValues(flags.GetType()).Cast<Enum>())
            {
                ulong mask = Convert.ToUInt64(flags);
                ulong bits = Convert.ToUInt64(value);
                if (bits == 0L)
                    continue; // skip the zero value
                if ((bits & mask) == bits)
                {
                    string paramName = value.ToString();
                    retString.Add(paramName);
                }
            }
            return retString;
        }
    }

    public enum MODSourceCorrectionEnum
    {
        [Display("Off")]
        [Scpi("OFF")]
        OFF,
        [Display("Modulation")]
        [Scpi("MODulation")]
        MODulation,
        [Display("Power")]
        [Scpi("POWer")]
        POWer,
        [Display("Mod & Pwr")]
        [Scpi("MODPwr")]
        MODPwr
    }

    public enum MODCalPortEnum
    {
        DUTIn1,
        DUTOut2,
        DUTOut3,
        DUTOut4,
        DUTOut5
    }

    public enum MODCalTypeEnum
    {
        [Scpi("POWer")]
        POWer,
        [Scpi("EQUalization")]
        EQUalization,
        [Scpi("LO:FTHRu")]
        LO,
        [Scpi("DISTortion")]
        DISTortion,
        [Scpi("NOTch")]
        NOTch,
        [Scpi("ACP:LOWer")]
        ACPLower,
        [Scpi("ACP:UPPer")]
        ACPUpper
    }

    public enum MODCalSynchTypeEnum
    {
        [Display("Synchronous")]
        [Scpi("SYNC")]
        SYNChronous,
        [Display("Asynchrounous")]
        [Scpi("ASYN")]
        ASYNchrounous
    }

    public enum MODMeasurementTypeEnum
    {
        [Scpi("ACP")]
        [Display("ACP")]
        ACP,
        [Scpi("ACPEVM")]
        [Display("ACP+EVM")]
        ACPEVM,
        [Scpi("BPWR")]
        [Display("Band Power")]
        BPWR,
        [Scpi("EVM")]
        [Display("EVM")]
        EVM,
        [Scpi("NPR")]
        [Display("NPR")]
        NPR
    }

    public enum MODMeasConfigTypeEnum
    {
        [Scpi("CARRier")]
        CARRier,
        [Scpi("ACP:LOWer")]
        ACPLower,
        [Scpi("ACP:UPPer")]
        ACPUpper,
        [Scpi("NOTCh")]
        Notch
    }

    [Flags]
    public enum MODTableSetupCarrierEnum
    {
        [Display("Carrier In1 dBm")]
        [Scpi("Carrier In1 dBm")]
        CarrierIn1dBm = 1,
        [Display("Carrier In1 dBm/Hz")]
        [Scpi("Carrier In1 dBm/Hz")]
        CarrierIn1dBmHz = 2,
        [Display("Carrier PDlvrIn1 dBm")]
        [Scpi("Carrier PDlvrIn1 dBm")]
        CarrierPDlvrIn1dBm = 4,
        [Display("Carrier PDlvrIn1 dBm/Hz")]
        [Scpi("Carrier PDlvrIn1 dBm/Hz")]
        CarrierPDlvrIn1dBmHz = 8,
        [Display("Carrier Out2 dBm")]
        [Scpi("Carrier Out2 dBm")]
        CarrierOut2dBm = 16,
        [Display("Carrier Out2 dBm/Hz")]
        [Scpi("Carrier Out2 dBm/Hz")]
        CarrierOut2dBmHz = 32,
        [Display("Carrier PDlvrOut2 dBm")]
        [Scpi("Carrier PDlvrOut2 dBm")]
        CarrierPDlvrOut2dBm = 64,
        [Display("Carrier PDlvrOut2 dBm/Hz")]
        [Scpi("Carrier PDlvrOut2 dBm/Hz")]
        CarrierPDlvrOut2dBmHz = 128,
        [Display("Carrier Gain21 dB")]
        [Scpi("Carrier Gain21 dB")]
        CarrierGain21dB = 256,
        [Display("Carrier IBW")]
        [Scpi("Carrier IBW")]
        CarrierIBW = 512,
        [Display("Carrier OffsFreq")]
        [Scpi("Carrier OffsFreq")]
        CarrierOffsFreq = 1024,
        [Display("Carrier Filter")]
        [Scpi("Carrier Filter")]
        CarrierFilter = 2048
    }

    [Flags]
    public enum MODTableSetupEVMEnum
    {
        [Display("EVM DistEq21 dBc")]
        [Scpi("EVM DistEq21 dBc")]
        EVMDistEq21dBc = 1,
        [Display("EVM DistEq21 %")]
        [Scpi("EVM DistEq21 %")]
        EVMDistEq21 = 2,
        [Display("EVM DistUn21 dBc")]
        [Scpi("EVM DistUn21 dBc")]
        EVMDistUn21dBc = 4,
        [Display("EVM DistUn21 %")]
        [Scpi("EVM DistUn21 %")]
        EVMDistUn21 = 8,
        [Display("EVM Norm")]
        [Scpi("EVM Norm")]
        EVMNorm = 16,
        [Display("Equalized EVM at Input")]
        [Scpi("EvmInEq1")]
        EvmInEq1 = 32,
        [Display("Unequalized EVM at Input")]
        [Scpi("EvmInUn1")]
        EvmInUn1 = 64,
        [Display("Equalized EVM at Output")]
        [Scpi("EvmOutEq2")]
        EvmOutEq2 = 128,
        [Display("Unequalized EVM at Output")]
        [Scpi("EvmOutUn2")]
        EvmOutUn2 = 256
    }

    [Flags]
    public enum MODTableSetupNPREnum
    {
        [Display("NPR In1 dBc")]
        [Scpi("NPR In1 dBc")]
        NPRIn1dBc = 1,
        [Display("NPR In1 dBm")]
        [Scpi("NPR In1 dBm")]
        NPRIn1dBm = 2,
        [Display("NPR In1 dBm/Hz")]
        [Scpi("NPR In1 dBm/Hz")]
        NPRIn1dBmHz = 4,
        [Display("NPR Out2 dBc")]
        [Scpi("NPR Out2 dBc")]
        NPROut2dBc = 8,
        [Display("NPR Out2 dBm")]
        [Scpi("NPR Out2 dBm")]
        NPROut2dBm = 16,
        [Display("NPR Out2 dBm/Hz")]
        [Scpi("NPR Out2 dBm/Hz")]
        NPROut2dBmHz = 32,
        [Display("NPR Dist21 dBc")]
        [Scpi("NPR Dist21 dBc")]
        NPRDist21dBc = 64,
        [Display("NPR NtchIBW")]
        [Scpi("NPR NtchIBW")]
        NPRNtchIBW = 128,
        [Display("NPR NtchOffsFreq")]
        [Scpi("NPR NtchOffsFreq")]
        NPRNtchOffsFreq = 256
    }

    [Flags]
    public enum MODTableSetupACPEnum
    {
        [Display("ACP LoIn1 dBc")]
        [Scpi("ACP LoIn1 dBc")]
        ACPLoIn1dBc = 1,
        [Display("ACP LoIn1 dBm")]
        [Scpi("ACP LoIn1 dBm")]
        ACPLoIn1dBm = 2,
        [Display("ACP LoIn1 dBm/Hz")]
        [Scpi("ACP LoIn1 dBm/Hz")]
        ACPLoIn1dBmHz = 4,
        [Display("ACP LoOut2 dBc")]
        [Scpi("ACP LoOut2 dBc")]
        ACPLoOut2dBc = 8,
        [Display("ACP LoOut2 dBm")]
        [Scpi("ACP LoOut2 dBm")]
        ACPLoOut2dBm = 16,
        [Display("ACP LoOut2 dBm/Hz")]
        [Scpi("ACP LoOut2 dBm/Hz")]
        ACPLoOut2dBmHz = 32,
        [Display("ACP LoDist21 dBc")]
        [Scpi("ACP LoDist21 dBc")]
        ACPLoDist21dBc = 64,
        [Display("ACP LoIBW")]
        [Scpi("ACP LoIBW")]
        ACPLoIBW = 128,
        [Display("ACP LoOffsFreq")]
        [Scpi("ACP LoOffsFreq")]
        ACPLoOffsFreq = 256,
        [Display("ACP UpIn1 dBc")]
        [Scpi("ACP UpIn1 dBc")]
        ACPUpIn1dBc = 512,
        [Display("ACP UpIn1 dBm")]
        [Scpi("ACP UpIn1 dBm")]
        ACPUpIn1dBm = 1024,
        [Display("ACP UpIn1 dBm/Hz")]
        [Scpi("ACP UpIn1 dBm/Hz")]
        ACPUpIn1dBmHz = 2048,
        [Display("ACP UpOut2 dBc")]
        [Scpi("ACP UpOut2 dBc")]
        ACPUpOut2dBc = 4096,
        [Display("ACP UpOut2 dBm")]
        [Scpi("ACP UpOut2 dBm")]
        ACPUpOut2dBm = 8192,
        [Display("ACP UpOut2 dBm/Hz")]
        [Scpi("ACP UpOut2 dBm/Hz")]
        ACPUpOut2dBmHz = 16384,
        [Display("ACP UpDist21 dBc")]
        [Scpi("ACP UpDist21 dBc")]
        ACPUpDist21dBc = 32768,
        [Display("ACP UpIBW")]
        [Scpi("ACP UpIBW")]
        ACPUpIBW = 65536,
        [Display("ACP UpOffsFreq")]
        [Scpi("ACP UpOffsFreq")]
        ACPUpOffsFreq = 131072
    }

    [Flags]
    public enum MODTableSetupACPAvgEnum
    {
        [Display("ACP AvgIn1 dBc")]
        [Scpi("ACP AvgIn1 dBc")]
        ACPAvgIn1dBc = 1,
        [Display("ACP AvgIn1 dBm")]
        [Scpi("ACP AvgIn1 dBm")]
        ACPAvgIn1dBm = 2,
        [Display("ACP AvgIn1 dBm/Hz")]
        [Scpi("ACP AvgIn1 dBm/Hz")]
        ACPAvgIn1dBmHz = 4,
        [Display("ACP AvgOut2 dBc")]
        [Scpi("ACP AvgOut2 dBc")]
        ACPAvgOut2dBc = 8,
        [Display("ACP AvgOut2 dBm")]
        [Scpi("ACP AvgOut2 dBm")]
        ACPAvgOut2dBm = 16,
        [Display("ACP AvgOut2 dBm/Hz")]
        [Scpi("ACP AvgOut2 dBm/Hz")]
        ACPAvgOut2dBmHz = 32,
        [Display("ACP AvgDist21 dBc")]
        [Scpi("ACP AvgDist21 dBc")]
        ACPAvgDist21dBc = 64
    }

    public enum MODAntialiasFilterEnum
    {
        [Scpi("NARR")]
        Narrow,
        [Scpi("WIDE")]
        Wide,
        [Scpi("AUTO")]
        Auto
    }

    public enum MODFilterEnum
    {
        [Scpi("NONE")]
        None,
        [Scpi("RRC")]
        RRC
    }

    public enum MODSweepTypeEnum
    {
        [Scpi("FIX")]
        Fixed,
        [Scpi("POW")]
        Power
    }

    public enum MODPowerPortEnum
    {
        [Scpi("DIN1")]
        [Display("DUT input")]
        Din,
        [Scpi("DOUT2")]
        [Display("DUT output")]
        Dout
    }

    public enum MODSweepPowerTypeEnum
    {
        [Scpi("RAMP")]
        Ramp,
        [Scpi("LIST")]
        List
    }

    public enum MODDutPortEnum
    {
        [Scpi("Port 1")]
        Port1 = 1,
        [Scpi("Port 2")]
        Port2 = 2,
        [Scpi("Port 3")]
        Port3 = 3,
        [Scpi("Port 4")]
        Port4 = 4
    }
    #endregion

    public partial class PNAX : ScpiInstrument
    {
        #region Sweep
        public void MODSweepType(int Channel, MODSweepTypeEnum swepTypeEnum)
        {
            string SweepType = Scpi.Format("{0}", swepTypeEnum);
            ScpiCommand($"SENSe{Channel}:DISTortion:SWEep:TYPE {SweepType}");
        }

        public void MODCarrierFreq(int Channel, double value)
        {
            ScpiCommand($"SENSe{Channel}:DISTortion:SWEep:CARRier:FREQuency {value}");
        }

        public void MODSpan(int Channel, double value)
        {
            ScpiCommand($"SENSe{Channel}:FREQuency:SPAN {value}");
        }

        public void MODNoiseBW(int Channel, double value)
        {
            ScpiCommand($"SENSe{Channel}:SA:BANDwidth:NOISe {value}");
        }

        public void MODSetPowerPort(int Channel, MODPowerPortEnum powerPort)
        {
            string MODPowerPort = Scpi.Format("{0}", powerPort);
            ScpiCommand($"SENSe{Channel}:DISTortion:SWEep:POWer:CARRier:LEVel:PORT {MODPowerPort}");
        }

        public void MODSetPower(int Channel, double value)
        {
            ScpiCommand($"SENSe{Channel}:DISTortion:SWEep:POWer:CARRier:LEVel {value}");
        }

        public void MODPowerSweepType(int Channel, MODSweepPowerTypeEnum powerType, int index = 1)
        {
            string MODPowerType = Scpi.Format("{0}", powerType);
            ScpiCommand($"SENSe{Channel}:DISTortion:SWEep:POWer:CARRier:LEVel{index}:TYPE {MODPowerType}");
        }

        public void MODPowerSweepStart(int Channel, double value, int index = 1)
        {
            ScpiCommand($"SENSe{Channel}:DISTortion:SWEep:POWer:CARRier:RAMP:LEVel{index}:STARt {value}");
        }

        public void MODPowerSweepStop(int Channel, double value, int index = 1)
        {
            ScpiCommand($"SENSe{Channel}:DISTortion:SWEep:POWer:CARRier:RAMP:LEVel{index}:STOP {value}");
        }

        public void MODPowerSweepNumberOfPoints(int Channel, int value)
        {
            ScpiCommand($"SENSe{Channel}:DISTortion:SWEep:POWer:CARRier:RAMP:POINts {value}");
        }

        public void MODPowerSweepNoiseBW(int Channel, int value, int index = 1)
        {
            ScpiCommand($"SENSe{Channel}:DISTortion:SWEep:POWer:CARRier:LIST{index}:NBW {value}");
        }

        public void MODPowerSweepAutoIncreaseBW(int Channel, bool state)
        {
            String StateStr = "OFF";
            if (state)
            {
                StateStr = "ON";
            }
            ScpiCommand($"SENSe{Channel}:DISTortion:SWEep:POWer:CARRier:RAMP:NBW:AUTO {StateStr}");
        }

        #endregion

        #region RF Path
        public void MODSourceAttenuatorInclude(int Channel, bool state)
        {
            String StateStr = "OFF";
            if (state)
            {
                StateStr = "ON";
            }
            ScpiCommand($"SENSe{Channel}:DISTortion:PATH:SOURce:ATTenuation:INCLude {StateStr}");
        }

        public void MODSourceAttenuator(int Channel, double value, MODDutPortEnum source)
        {
            string dutport = Scpi.Format("{0}", source);
            ScpiCommand($"SOURce{Channel}:POWer:ATTenuation {value}, \"{dutport}\"");
        }

        public void MODNominalSource(int Channel, double value)
        {
            ScpiCommand($"SENSe{Channel}:DISTortion:PATH:SOURce:NOMinal:AMPLifier {value}");
        }

        public void MODDutInput(int Channel, MODDutPortEnum dutinput)
        {
            int port = (int)dutinput;
            ScpiCommand($"SENSe{Channel}:DISTortion:PATH:DUT:INPut {port}");
        }

        public void MODNominalDUTGain(int Channel, double value)
        {
            ScpiCommand($"SENSe{Channel}:DISTortion:PATH:DUT:NOMinal:GAIN {value}");
        }

        public void MODDutOutput(int Channel, MODDutPortEnum dutoutput)
        {
            int port = (int)dutoutput;
            ScpiCommand($"SENSe{Channel}:DISTortion:PATH:DUT:OUTPut {port}");
        }

        public void MODReceiverAttenuator(int Channel, double value, MODDutPortEnum dutport)
        {
            int port = (int)dutport;
            ScpiCommand($"SOURce{Channel}:POWer{port}:ATTenuation:RECeiver:TEST {value}");
        }

        public void MODNominalDUTNFInclude(int Channel, bool state)
        {
            String StateStr = "OFF";
            if (state)
            {
                StateStr = "ON";
            }
            ScpiCommand($"SENSe{Channel}:DISTortion:PATH:DUT:NOMinal:NF:INCLude {StateStr}");
        }

        public void MODNominalDUTNF(int Channel, double value)
        {
            ScpiCommand($"SENSe{Channel}:DISTortion:PATH:DUT:NOMinal:NF {value}");
        }

        #endregion

        #region Modulate and Measure
        public void SetMODSource(int Channel, String source)
        {
            ScpiCommand($"SENSe{Channel}:DISTortion:MODulate:SOURce \"{source}\"");
        }

        public void MODLoadFile(int Channel, string port, string filename)
        {
            //ScpiCommand($"SOURce{Channel}:MODulation:FILE:LOAD \"{filename}\",\"{port}\"");
            ScpiCommand($"SOURce{Channel}:MODulation:LOAD \"{filename}\",\"{port}\"");
        }

        public void MODEnableModulation(int Channel, string port, bool state)
        {
            String StateStr = "OFF";
            if (state)
            {
                StateStr = "ON";
            }
            ScpiCommand($"SOURce{Channel}:MODulation:STATe {StateStr}, \"{port}\"");
        }

        public void MODSourceCorrection(int Channel, string port, MODSourceCorrectionEnum sourceCorrectionType)
        {
            string sourceCorrection = Scpi.Format("{0}", sourceCorrectionType);

            ScpiCommand($"SOURce{Channel}:CORRection:SELect {sourceCorrection},\"{port}\"");
        }

        public void MODCalEnable(int Channel, string port, bool state, MODCalTypeEnum MODCalType)
        {
            String StateStr = "OFF";
            if (state)
            {
                StateStr = "ON";
            }
            string calType = Scpi.Format("{0}", MODCalType);
            ScpiCommand($"SOURce{Channel}:MODulation:CORRection:COLLection:{calType}:ENABle {StateStr}, \"{port}\"");
        }

        public void MODCalPort(int Channel, string port, MODCalPortEnum MODCalPort, MODCalTypeEnum MODCalType)
        {
            string calType = Scpi.Format("{0}", MODCalType);
            string calPort = Scpi.Format("{0}", MODCalPort);
            ScpiCommand($"SOURce{Channel}:MODulation:CORRection:COLLection:{calType}:RECeiver \"{calPort}\", \"{port}\"");
        }

        public void MODCalSpan(int Channel, string port, double CalSpan, MODCalTypeEnum MODCalType)
        {
            string calType = Scpi.Format("{0}", MODCalType);
            ScpiCommand($"SOURce{Channel}:MODulation:CORRection:COLLection:{calType}:SPAN {CalSpan}, \"{port}\"");
        }

        public void MODCalGuardBand(int Channel, string port, double GuardBand, MODCalTypeEnum MODCalType)
        {
            string calType = Scpi.Format("{0}", MODCalType);
            ScpiCommand($"SOURce{Channel}:MODulation:CORRection:COLLection:{calType}:GBANd {GuardBand}, \"{port}\"");
        }

        public void MODCalMaxIterations(int Channel, string port, int MaxIterations, MODCalTypeEnum MODCalType)
        {
            string calType = Scpi.Format("{0}", MODCalType);
            ScpiCommand($"SOURce{Channel}:MODulation:CORRection:COLLection:{calType}:ITERations {MaxIterations}, \"{port}\"");
        }

        public void MODCalDesiredTolearance(int Channel, string port, double DesiredTolerance, MODCalTypeEnum MODCalType)
        {
            string calType = Scpi.Format("{0}", MODCalType);
            ScpiCommand($"SOURce{Channel}:MODulation:CORRection:COLLection:{calType}:TOLerance {DesiredTolerance}, \"{port}\"");
        }

        public void MODCalibrationMeasure(int Channel, string port, MODCalSynchTypeEnum CalSynchType = MODCalSynchTypeEnum.SYNChronous)
        {
            string synchType = Scpi.Format("{0}", CalSynchType);
            ScpiCommand($"SOURce{Channel}:MODulation:CORRection:COLLection:ACQuire {synchType}, \"{port}\"", 60000);
        }

        public string MODGetCalibrationDetails(int Channel, string port)
        {
            // Calibration details returns multiple single responses, using SCPIQuery leaves multiple lines on the queue
            // Better not to use Calibration Details for now

            String retString = "";
            //String line = "";
            //retString = ScpiQuery($"SOURce{Channel}:MODulation:CORRection:COLLection:ACQuire:DETails? \"{port}\"");
            //do
            //{
            //    line = ScpiQuery($"");
            //    retString += line;
            //} while (!line.Equals("\""));
            return retString;
        }

        public void MODSaveDistortionTable(int Channel, string FullFileName)
        {
            string FileName = Path.GetFileName(FullFileName);
            string FilePath = Path.GetDirectoryName(FullFileName);
            string InstrumentFileName = "";
            string InstrumentFolderName = "";

            if (GetDriveAccess() == false)
            {
                throw new Exception("Make Sure to Enable Remote Drive Access on the VNA.\nGo to Instrument | Setup | System Setup | Remote Interface… and Enable Remote Drive Access");
            }

            InstrumentFileName = @"C:\Users\Public\Documents\Network Analyzer\" + FileName;
            InstrumentFolderName = @"C:\Users\Public\Documents\Network Analyzer\";

            ChangeFolder(InstrumentFolderName);

            // Save csv data to local folder on instrument: <documents>\<mode>\screen
            ScpiCommand($"SENSe{Channel}:DISTortion:TABLe:DISPlay:SAVE \"{InstrumentFileName}\"");

            // Make sure folder exists on local PC
            bool exists = System.IO.Directory.Exists(FilePath);
            if (!exists)
                System.IO.Directory.CreateDirectory(FilePath);

            // Copy from instrument to local PC
            byte[] filedata = ScpiQueryBlock(string.Format(":MMEM:TRAN? \"{0}\"", InstrumentFileName));

            // Write file at local PC
            File.WriteAllBytes(FullFileName, filedata);

            // Delete File from instrument
            ScpiCommand(string.Format("MMEM:DEL \"{0}\"", InstrumentFileName));

            QueryErrors();
        }

        public string MODGetCalibrationStatus(int Channel, string port)
        {
            return ScpiQuery($"SOURce{Channel}:MODulation:CORRection:COLLection:ACQuire:STATus? \"{port}\"");
        }

        public void MODSetMeasType(int Channel, MODMeasurementTypeEnum measType, int bnum = 1)
        {
            string modMeasType = Scpi.Format("{0}", measType);
            ScpiCommand($"SENSe{Channel}:DISTortion:MEASure:BAND{bnum}:TYPE {modMeasType}");
        }

        public void MODSetIBW(int Channel, double ibw, MODMeasConfigTypeEnum measConfType, int bnum = 1)
        {
            string MeasConf = Scpi.Format("{0}", measConfType);
            ScpiCommand($"SENSe{Channel}:DISTortion:MEASure:BAND{bnum}:{MeasConf}:IBW {ibw}");
        }

        public void MODSetOffset(int Channel, double offset, MODMeasConfigTypeEnum measConfType, int bnum = 1)
        {
            string MeasConf = Scpi.Format("{0}", measConfType);
            ScpiCommand($"SENSe{Channel}:DISTortion:MEASure:BAND{bnum}:{MeasConf}:OFFSet {offset}");
        }

        public void MODAutofill(int Channel, int bnum = 1)
        {
            ScpiCommand($"SENSe{Channel}:DISTortion:MEASure:BAND{bnum}:AUTofill");
        }

        public double MODGetDataValue(int Channel, string paramName, int bnum = 1)
        {
            return ScpiQuery<double>($"SENSe{Channel}:DISTortion:TABLe:DATA:VALue? {bnum},\"{paramName}\"");
        }

        public List<string> MODGetAllColumnNames(int Channel)
        {
            string retString = ScpiQuery($"SENSe{Channel}:DISTortion:TABLe:DISPlay:CATalog?");
            retString = retString.Replace("\"", "");
            List<string> retVal = retString.Split(',').ToList<string>();
            return retVal;
        }


        public void MODTableAddParameter(int Channel, MODTableSetupCarrierEnum modParamName)
        {
            List<string> paramNames = modParamName.MODGetFlagsScpi();

            foreach (string paramName in paramNames)
            {
                MODTableAddParameter(Channel, paramName);
            }
        }

        public void MODTableAddParameter(int Channel, MODTableSetupEVMEnum modParamName)
        {
            List<string> paramNames = modParamName.MODGetFlagsScpi();

            foreach (string paramName in paramNames)
            {
                MODTableAddParameter(Channel, paramName);
            }
        }

        public void MODTableAddParameter(int Channel, MODTableSetupNPREnum modParamName)
        {
            List<string> paramNames = modParamName.MODGetFlagsScpi();

            foreach (string paramName in paramNames)
            {
                MODTableAddParameter(Channel, paramName);
            }
        }

        public void MODTableAddParameter(int Channel, MODTableSetupACPEnum modParamName)
        {
            List<string> paramNames = modParamName.MODGetFlagsScpi();

            foreach (string paramName in paramNames)
            {
                MODTableAddParameter(Channel, paramName);
            }
        }

        public void MODTableAddParameter(int Channel, MODTableSetupACPAvgEnum modParamName)
        {
            List<string> paramNames = modParamName.MODGetFlagsScpi();

            foreach (string paramName in paramNames)
            {
                MODTableAddParameter(Channel, paramName);
            }
        }

        public void MODTableAddParameter(int Channel, string paramName)
        {
            ScpiCommand($"SENSe{Channel}:DISTortion:TABLe:DISPlay:FEED \"{paramName}\"");
        }

        public void MODShowTable(int wnum, bool state)
        {
            String StateStr = "OFF";
            if (state)
            {
                StateStr = "DISTortion";
            }
            ScpiCommand($"DISPlay:WINDow{wnum}:TABLe {StateStr}");
        }

        public void MODCorrelationAperture(int Channel, double value)
        {
            ScpiCommand($"SENSe{Channel}:DISTortion:MEASure:CORRelation:APERture {value}");
        }

        public void MODCorrelationApertureAuto(int Channel, bool state)
        {
            String StateStr = "OFF";
            if (state)
            {
                StateStr = "ON";
            }
            ScpiCommand($"SENSe{Channel}:DISTortion:MEASure:CORRelation:APERture:AUTO:STATe {StateStr}");
        }

        public void MODAntialiasFilter(int Channel, MODAntialiasFilterEnum antialiasFilter)
        {
            string modAntialiasFilter = Scpi.Format("{0}", antialiasFilter);
            ScpiCommand($"SENSe{Channel}:DISTortion:ADC:FILTer:TYPE {modAntialiasFilter}");
        }

        public void MODEVMNormalization(int Channel, double value)
        {
            ScpiCommand($"SENSe{Channel}:DISTortion:EVM:NORMalize {value}");
        }

        public void MODDutNF(int Channel, double value)
        {
            ScpiCommand($"SENSe{Channel}:DISTortion:PATH:DUT:NOMinal:NF {value}");
        }

        public void MODFilter(int Channel, MODFilterEnum filter)
        {
            string Filter = Scpi.Format("{0}", filter);
            ScpiCommand($"SENSe{Channel}:DISTortion:MEASure:FILTer {Filter}");
        }

        public void MODFilterAlpha(int Channel, int alpha)
        {
            ScpiCommand($"SENSe{Channel}:DISTortion:MEASure:FILTer:ALPHa {alpha}");
        }

        public void MODFilterSymbolRate(int Channel, double srate)
        {
            ScpiCommand($"SENSe{Channel}:DISTortion:MEASure:FILTer:SRATe {srate}");
        }

        public void MODFilterSymbolRateAuto(int Channel, bool state)
        {
            String StateStr = "OFF";
            if (state)
            {
                StateStr = "ON";
            }
            ScpiCommand($"SENSe{Channel}:DISTortion:MEASure:FILTer:SRATe:AUTO:STATe {StateStr}");
        }

        #endregion

        #region Mixers
        public void MODMixerSourceRole(int Channel, string role, string source)
        {
            ScpiCommand($"SENSe{Channel}:ROLE:DEVice \"{role}\",\"{source}\"");
        }
        #endregion
    }
}
