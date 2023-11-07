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
    public partial class PNAX : ScpiInstrument
    {
        #region Compression
        public CompressionMethodEnum GetCompressionMethod(int Channel)
        {
            string retString = ScpiQuery($"SENSe{Channel}:GCSetup:COMPression:ALGorithm?");
            if (retString.Equals("CFMG"))
            {
                return CompressionMethodEnum.CompressionFromMaxGain;
            }
            else if (retString.StartsWith("BACK"))
            {
                return CompressionMethodEnum.CompressionFromBackOff;
            }
            else if (retString.Equals("XYCOM"))
            {
                return CompressionMethodEnum.XYCompression;
            }
            else if (retString.Equals("SAT"))
            {
                return CompressionMethodEnum.CompressionFromSaturation;
            }
            else
            {
                return CompressionMethodEnum.CompressionFromLinearGain;
            }
        }

        public void SetCompressionMethod(int Channel, CompressionMethodEnum method)
        {
            string alg = Scpi.Format("{0}", method);
            ScpiCommand($"SENSe{Channel}:GCSetup:COMPression:ALGorithm {alg}");
        }

        public double GetCompressionLevel(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:GCSetup:COMPression:LEVel?");
        }

        public void SetCompressionLevel(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel}:GCSetup:COMPression:LEVel {level}");
        }

        public double GetCompressionBackoffLevel(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:GCSetup:COMPression:BACKoff:LEVel?");
        }

        public void SetCompressionBackoffLevel(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel}:GCSetup:COMPression:BACKoff:LEVel {level}");
        }

        public double GetCompressionDeltaX(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:GCSetup:COMPression:DELTa:X?");
        }

        public void SetCompressionDeltaX(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel}:GCSetup:COMPression:DELTa:X {level}");
        }

        public double GetCompressionDeltaY(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:GCSetup:COMPression:DELTa:Y?");
        }

        public void SetCompressionDeltaY(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel}:GCSetup:COMPression:DELTa:Y {level}");
        }

        public double GetCompressionSaturation(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:GCSetup:COMPression:SATuration:LEVel?");
        }

        public void SetCompressionSaturation(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel}:GCSetup:COMPression:SATuration:LEVel {level}");
        }

        public double GetSMARTSweepTolerance(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:GCSetup:SMARt:TOLerance?");
        }

        public void SetSMARTSweepTolerance(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel}:GCSetup:SMARt:TOLerance {level}");
        }

        public double GetSMARTSweepMaximumIterations(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:GCSetup:SMARt:MITerations?");
        }

        public void SetSMARTSweepMaximumIterations(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel}:GCSetup:SMARt:MITerations {level}");
        }

        public bool GetSMARTSweepShowIterations(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel}:GCSetup:SMARt:SITerations?");
            return !retStr.Equals("0");
        }

        public void SetSMARTSweepShowIterations(int Channel, bool mode)
        {
            string stateValue = mode ? "1" : "0";
            ScpiCommand($"SENSe{Channel}:GCSetup:SMARt:SITerations {stateValue}");
        }

        public bool GetSMARTSweepReadDCAtCompression(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel}:GCSetup:SMARt:CDC?");
            return !retStr.Equals("0");
        }

        public void SetSMARTSweepReadDCAtCompression(int Channel, bool mode)
        {
            string stateValue = mode ? "1" : "0";
            ScpiCommand($"SENSe{Channel}:GCSetup:SMARt:CDC {stateValue}");
        }

        public bool GetSMARTSweepSafeMode(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel}:GCSetup:SAFE:ENABle?");
            return !retStr.Equals("0");
        }

        public void SetSMARTSweepSafeMode(int Channel, bool mode)
        {
            string stateValue = mode ? "1" : "0";
            ScpiCommand($"SENSe{Channel}:GCSetup:SAFE:ENABle {stateValue}");
        }

        public double GetSMARTSweepSafeModeCoarseIncrement(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:GCSetup:SAFE:CPADjustment?");
        }

        public void SetSMARTSweepSafeModeCoarseIncrement(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel}:GCSetup:SAFE:CPADjustment {level}");
        }

        public double GetSMARTSweepSafeModeFineIncrement(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:GCSetup:SAFE:FPADjustment?");
        }

        public void SetSMARTSweepSafeModeFineIncrement(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel}:GCSetup:SAFE:FPADjustment {level}");
        }

        public double GetSMARTSweepSafeModeFineThreshold(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:GCSetup:SAFE:FTHReshold?");
        }

        public void SetSMARTSweepSafeModeFineThreshold(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel}:GCSetup:SAFE:FTHReshold {level}");
        }

        public double GetSMARTSweepSafeModeMaxOutputPower(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:GCSetup:SAFE:MLIMit?");
        }

        public void SetSMARTSweepSafeModeMaxOutputPower(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel}:GCSetup:SAFE:MLIMit {level}");
        }

        public string GetSMARTSweepSafeModeDCParameters(int Channel)
        {
            return ScpiQuery($"SENSe{Channel}:GCSetup:SAFE:DC:PARameter?");
        }

        public void SetSMARTSweepSafeModeDCParameters(int Channel, string device)
        {
            ScpiCommand($"SENSe{Channel}:GCSetup:SAFE:DC:PARameter \"{device}\"");
        }

        public double GetSMARTSweepSafeModeMaxDCPower(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:GCSetup:SAFE:DC:MLIMit?");
        }

        public void SetSMARTSweepSafeModeMaxDCPower(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel}:GCSetup:SAFE:DC:MLIMit {level}");
        }



        public bool Get2DSweepCompressionPointInterpolation(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel}:GCSetup:COMPression:INTerpolate?");
            return !retStr.Equals("0");
        }

        public void Set2DSweepCompressionPointInterpolation(int Channel, bool mode)
        {
            string stateValue = mode ? "1" : "0";
            ScpiCommand($"SENSe{Channel}:GCSetup:COMPression:INTerpolate {stateValue}");
        }

        public EndOfSweepConditionEnum GetCompressionEndOfSweepCondition(int Channel)
        {
            string retString = ScpiQuery($"SENSe{Channel}:GCSetup:EOSoperation?");
            if (retString.Equals("POFF"))
            {
                return EndOfSweepConditionEnum.RFOff;
            }
            else if (retString.StartsWith("PSTA"))
            {
                return EndOfSweepConditionEnum.StartPower;
            }
            else if (retString.Equals("PSTO"))
            {
                return EndOfSweepConditionEnum.StopPower;
            }
            else
            {
                return EndOfSweepConditionEnum.Default;
            }
        }

        public void SetCompressionEndOfSweepCondition(int Channel, EndOfSweepConditionEnum condition)
        {
            string cond = Scpi.Format("{0}", condition);
            ScpiCommand($"SENSe{Channel}:GCSetup:EOSoperation {cond}");
        }

        public double GetCompressionSettlingTime(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:GCSetup:SMARt:STIMe?");
        }

        public void SetCompressionSettlingTime(int Channel, double time)
        {
            ScpiCommand($"SENSe{Channel}:GCSetup:SMARt:STIMe {time}");
        }
        #endregion

        #region Power
        public int GetGCPortInput(int Channel)
        {
            return ScpiQuery<int>($"SENS{Channel}:GCSetup:PMAP:INP?");
        }

        public int GetGCPortOutput(int Channel)
        {
            return ScpiQuery<int>($"SENS{Channel}:GCSetup:PMAP:OUTP?");
        }

        public void SetGCPortInputOutput(int Channel, PortsEnum inport, PortsEnum outport)
        {
            string inp = Scpi.Format("{0}", inport);
            string outp = Scpi.Format("{0}", outport);
            ScpiCommand($"SENS{Channel}:GCSetup:PMAP {inp},{outp}");
        }

        public double GetPowerLinearInputPower(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:GCSetup:POWer:LINear:INPut:LEVel?");
        }

        public void SetPowerLinearInputPower(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:GCSetup:POWer:LINear:INPut:LEVel {power}");
        }

        public double GetPowerReversePower(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:GCSetup:POWer:REVerse:LEVel?");
        }

        public void SetPowerReversePower(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:GCSetup:POWer:REVerse:LEVel {power}");
        }

        public bool GetSourceAttenuatorAutoMode(int Channel, int port)
        {
            string retStr = ScpiQuery($"SOURce{Channel}:POWer{port}:ATTenuation:AUTO?");
            return !retStr.Equals("0");
        }

        public void SetSourceAttenuatorAutoMode(int Channel, int port, bool mode)
        {
            string stateValue = mode ? "1" : "0";
            ScpiCommand($"SOURce{Channel}:POWer{port}:ATTenuation:AUTO {stateValue}");
        }

        public void SetSourceAttenuatorAutoMode(int Channel, PortsEnum port, bool mode)
        {
            string strPort = Scpi.Format("{0}", port);
            string stateValue = mode ? "1" : "0";
            ScpiCommand($"SOURce{Channel}:POWer{strPort}:ATTenuation:AUTO {stateValue}");
        }

        public double GetPowerSweepStartPower(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:GCSetup:POWer:STARt:LEVel?");
        }

        public void SetPowerSweepStartPower(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:GCSetup:POWer:STARt:LEVel {power}");
        }

        public double GetPowerSweepStopPower(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:GCSetup:POWer:STOP:LEVel?");
        }

        public void SetPowerSweepStopPower(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:GCSetup:POWer:STOP:LEVel {power}");
        }

        public double GetPowerSweepPowerPoints(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:GCSetup:SWEep:POWer:POINts?");
        }

        public void SetPowerSweepPowerPoints(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel}:GCSetup:SWEep:POWer:POINts {power}");
        }


        #endregion

        #region Frequency
        public SweepTypeEnum GetSweepType(int Channel)
        {
            SweepTypeEnum retVal = SweepTypeEnum.LinearSweep;
            string retString = ScpiQuery($"SENSe{Channel}:SWEep:TYPE?");
            if (retString.Equals("LIN"))
            {
                retVal = SweepTypeEnum.LinearSweep;
            }
            //else if (retString.Equals("LOG"))
            //{
            //    retVal = SweepTypeEnum.CompressionFromMaxGain;
            //}
            //else if (retString.StartsWith("POW"))
            //{
            //    retVal = SweepTypeEnum.CompressionFromBackOff;
            //}
            else if (retString.Equals("CW"))
            {
                retVal = SweepTypeEnum.CWFrequency;
            }
            //else if (retString.Equals("SEGM"))
            //{
            //    retVal = SweepTypeEnum.CompressionFromSaturation;
            //}
            return retVal;
        }

        public void SetSweepType(int Channel, SweepTypeEnum sweeptype)
        {
            string sweep = Scpi.Format("{0}", sweeptype);
            ScpiCommand($"SENSe{Channel}:SWEep:TYPE {sweep}");
        }

        public void SetSweepType(int Channel, GeneralGainCompressionSweepTypeEnum sweeptype)
        {
            string sweep = Scpi.Format("{0}", sweeptype);
            ScpiCommand($"SENSe{Channel}:SWEep:TYPE {sweep}");
        }

        public void SetSweepType(int Channel, GeneralNFSweepTypeEnum sweeptype)
        {
            string sweep = Scpi.Format("{0}", sweeptype);
            ScpiCommand($"SENSe{Channel}:SWEep:TYPE {sweep}");
        }

        public DataAcquisitionModeEnum GetDataAcquisitionMode(int Channel)
        {
            string retString = ScpiQuery($"SENSe{Channel}:GCSetup:AMODe?");
            if (retString.Equals("PFREQ"))
            {
                return DataAcquisitionModeEnum.SweepPowerPerFrequency2D;
            }
            else if (retString.StartsWith("FPOW"))
            {
                return DataAcquisitionModeEnum.SweepFrequencyPerPower2D;
            }
            else
            {
                return DataAcquisitionModeEnum.SMARTSweep;
            }
        }

        public void SetDataAcquisitionMode(int Channel, DataAcquisitionModeEnum sweeptype)
        {
            string sweep = Scpi.Format("{0}", sweeptype);
            ScpiCommand($"SENSe{Channel}:GCSetup:AMODe {sweep}");
        }
        #endregion
    }
}
