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
            CompressionMethodEnum retVal = CompressionMethodEnum.CompressionFromLinearGain;
            String retString = ScpiQuery($"SENSe{Channel.ToString()}:GCSetup:COMPression:ALGorithm?");
            if (retString.Equals("CFLG"))
            {
                retVal = CompressionMethodEnum.CompressionFromLinearGain;
            }
            else if (retString.Equals("CFMG"))
            {
                retVal = CompressionMethodEnum.CompressionFromMaxGain;
            }
            else if (retString.StartsWith("BACK"))
            {
                retVal = CompressionMethodEnum.CompressionFromBackOff;
            }
            else if (retString.Equals("XYCOM"))
            {
                retVal = CompressionMethodEnum.XYCompression;
            }
            else if (retString.Equals("SAT"))
            {
                retVal = CompressionMethodEnum.CompressionFromSaturation;
            }
            return retVal;
        }

        public void SetCompressionMethod(int Channel, CompressionMethodEnum method)
        {
            String alg = Scpi.Format("{0}", method);
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:COMPression:ALGorithm {alg}");
        }

        public double GetCompressionLevel(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:GCSetup:COMPression:LEVel?");
            return retVal;
        }

        public void SetCompressionLevel(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:COMPression:LEVel {level.ToString()}");
        }

        public double GetCompressionBackoffLevel(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:GCSetup:COMPression:BACKoff:LEVel?");
            return retVal;
        }

        public void SetCompressionBackoffLevel(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:COMPression:BACKoff:LEVel {level.ToString()}");
        }

        public double GetCompressionDeltaX(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:GCSetup:COMPression:DELTa:X?");
            return retVal;
        }

        public void SetCompressionDeltaX(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:COMPression:DELTa:X {level.ToString()}");
        }

        public double GetCompressionDeltaY(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:GCSetup:COMPression:DELTa:Y?");
            return retVal;
        }

        public void SetCompressionDeltaY(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:COMPression:DELTa:Y {level.ToString()}");
        }

        public double GetCompressionSaturation(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:GCSetup:COMPression:SATuration:LEVel?");
            return retVal;
        }

        public void SetCompressionSaturation(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:COMPression:SATuration:LEVel {level.ToString()}");
        }

        public double GetSMARTSweepTolerance(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:GCSetup:SMARt:TOLerance?");
            return retVal;
        }

        public void SetSMARTSweepTolerance(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:SMARt:TOLerance {level.ToString()}");
        }

        public double GetSMARTSweepMaximumIterations(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:GCSetup:SMARt:MITerations?");
            return retVal;
        }

        public void SetSMARTSweepMaximumIterations(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:SMARt:MITerations {level.ToString()}");
        }

        public bool GetSMARTSweepShowIterations(int Channel)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:GCSetup:SMARt:SITerations?");
            if (retStr.Equals("0"))
            {
                retVal = false;
            }
            else
            {
                retVal = true;
            }
            return retVal;
        }

        public void SetSMARTSweepShowIterations(int Channel, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:SMARt:SITerations 1");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:SMARt:SITerations 0");
            }
        }

        public bool GetSMARTSweepReadDCAtCompression(int Channel)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:GCSetup:SMARt:CDC?");
            if (retStr.Equals("0"))
            {
                retVal = false;
            }
            else
            {
                retVal = true;
            }
            return retVal;
        }

        public void SetSMARTSweepReadDCAtCompression(int Channel, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:SMARt:CDC 1");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:SMARt:CDC 0");
            }
        }

        public bool GetSMARTSweepSafeMode(int Channel)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:GCSetup:SAFE:ENABle?");
            if (retStr.Equals("0"))
            {
                retVal = false;
            }
            else
            {
                retVal = true;
            }
            return retVal;
        }

        public void SetSMARTSweepSafeMode(int Channel, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:SAFE:ENABle 1");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:SAFE:ENABle 0");
            }
        }

        public double GetSMARTSweepSafeModeCoarseIncrement(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:GCSetup:SAFE:CPADjustment?");
            return retVal;
        }

        public void SetSMARTSweepSafeModeCoarseIncrement(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:SAFE:CPADjustment {level.ToString()}");
        }

        public double GetSMARTSweepSafeModeFineIncrement(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:GCSetup:SAFE:FPADjustment?");
            return retVal;
        }

        public void SetSMARTSweepSafeModeFineIncrement(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:SAFE:FPADjustment {level.ToString()}");
        }

        public double GetSMARTSweepSafeModeFineThreshold(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:GCSetup:SAFE:FTHReshold?");
            return retVal;
        }

        public void SetSMARTSweepSafeModeFineThreshold(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:SAFE:FTHReshold {level.ToString()}");
        }

        public double GetSMARTSweepSafeModeMaxOutputPower(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:GCSetup:SAFE:MLIMit?");
            return retVal;
        }

        public void SetSMARTSweepSafeModeMaxOutputPower(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:SAFE:MLIMit {level.ToString()}");
        }

        public String GetSMARTSweepSafeModeDCParameters(int Channel)
        {
            String retVal = "";
            retVal = ScpiQuery($"SENSe{Channel.ToString()}:GCSetup:SAFE:DC:PARameter?");
            return retVal;
        }

        public void SetSMARTSweepSafeModeDCParameters(int Channel, String device)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:SAFE:DC:PARameter \"{device}\"");
        }

        public double GetSMARTSweepSafeModeMaxDCPower(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:GCSetup:SAFE:DC:MLIMit?");
            return retVal;
        }

        public void SetSMARTSweepSafeModeMaxDCPower(int Channel, double level)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:SAFE:DC:MLIMit {level.ToString()}");
        }



        public bool Get2DSweepCompressionPointInterpolation(int Channel)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:GCSetup:COMPression:INTerpolate?");
            if (retStr.Equals("0"))
            {
                retVal = false;
            }
            else
            {
                retVal = true;
            }
            return retVal;
        }

        public void Set2DSweepCompressionPointInterpolation(int Channel, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:COMPression:INTerpolate 1");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:COMPression:INTerpolate 0");
            }
        }

        public EndOfSweepConditionEnum GetCompressionEndOfSweepCondition(int Channel)
        {
            EndOfSweepConditionEnum retVal = EndOfSweepConditionEnum.Default;
            String retString = ScpiQuery($"SENSe{Channel.ToString()}:GCSetup:EOSoperation?");
            if (retString.Equals("STAN"))
            {
                retVal = EndOfSweepConditionEnum.Default;
            }
            else if (retString.Equals("POFF"))
            {
                retVal = EndOfSweepConditionEnum.RFOff;
            }
            else if (retString.StartsWith("PSTA"))
            {
                retVal = EndOfSweepConditionEnum.StartPower;
            }
            else if (retString.Equals("PSTO"))
            {
                retVal = EndOfSweepConditionEnum.StopPower;
            }
            return retVal;
        }

        public void SetCompressionEndOfSweepCondition(int Channel, EndOfSweepConditionEnum condition)
        {
            String cond = Scpi.Format("{0}", condition);
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:EOSoperation {cond}");
        }

        public double GetCompressionSettlingTime(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:GCSetup:SMARt:STIMe?");
            return retVal;
        }

        public void SetCompressionSettlingTime(int Channel, double time)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:SMARt:STIMe {time.ToString()}");
        }
        #endregion

        #region Power
        public int GetGCPortInput(int Channel)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENS{Channel.ToString()}:GCSetup:PMAP:INP?");
            return retVal;
        }

        public int GetGCPortOutput(int Channel)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENS{Channel.ToString()}:GCSetup:PMAP:OUTP?");
            return retVal;
        }

        public void SetGCPortInputOutput(int Channel, PortsEnum inport, PortsEnum outport)
        {
            String inp = Scpi.Format("{0}", inport);
            String outp = Scpi.Format("{0}", outport);
            ScpiCommand($"SENS{Channel.ToString()}:GCSetup:PMAP {inp},{outp}");
        }

        public double GetPowerLinearInputPower(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:GCSetup:POWer:LINear:INPut:LEVel?");
            return retVal;
        }

        public void SetPowerLinearInputPower(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:POWer:LINear:INPut:LEVel {power.ToString()}");
        }

        public double GetPowerReversePower(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:GCSetup:POWer:REVerse:LEVel?");
            return retVal;
        }

        public void SetPowerReversePower(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:POWer:REVerse:LEVel {power.ToString()}");
        }

        public bool GetSourceAttenuatorAutoMode(int Channel, int port)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SOURce{Channel.ToString()}:POWer{port.ToString()}:ATTenuation:AUTO?");
            if (retStr.Equals("0"))
            {
                retVal = false;
            }
            else
            {
                retVal = true;
            }
            return retVal;
        }

        public void SetSourceAttenuatorAutoMode(int Channel, int port, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SOURce{Channel.ToString()}:POWer{port.ToString()}:ATTenuation:AUTO 1");
            }
            else
            {
                ScpiCommand($"SOURce{Channel.ToString()}:POWer{port.ToString()}:ATTenuation:AUTO 0");
            }
        }

        public void SetSourceAttenuatorAutoMode(int Channel, PortsEnum port, bool mode)
        {
            String strPort = Scpi.Format("{0}", port);
            if (mode == true)
            {
                ScpiCommand($"SOURce{Channel.ToString()}:POWer{strPort}:ATTenuation:AUTO 1");
            }
            else
            {
                ScpiCommand($"SOURce{Channel.ToString()}:POWer{strPort}:ATTenuation:AUTO 0");
            }
        }

        public double GetPowerSweepStartPower(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:GCSetup:POWer:STARt:LEVel?");
            return retVal;
        }

        public void SetPowerSweepStartPower(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:POWer:STARt:LEVel {power.ToString()}");
        }

        public double GetPowerSweepStopPower(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:GCSetup:POWer:STOP:LEVel?");
            return retVal;
        }

        public void SetPowerSweepStopPower(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:POWer:STOP:LEVel {power.ToString()}");
        }

        public double GetPowerSweepPowerPoints(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:GCSetup:SWEep:POWer:POINts?");
            return retVal;
        }

        public void SetPowerSweepPowerPoints(int Channel, double power)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:SWEep:POWer:POINts {power.ToString()}");
        }


        #endregion

        #region Frequency
        public SweepTypeEnum GetSweepType(int Channel)
        {
            SweepTypeEnum retVal = SweepTypeEnum.LinearSweep;
            String retString = ScpiQuery($"SENSe{Channel.ToString()}:SWEep:TYPE?");
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
            String sweep = Scpi.Format("{0}", sweeptype);
            ScpiCommand($"SENSe{Channel.ToString()}:SWEep:TYPE {sweep}");
        }

        public void SetSweepType(int Channel, GeneralGainCompressionSweepTypeEnum sweeptype)
        {
            String sweep = Scpi.Format("{0}", sweeptype);
            ScpiCommand($"SENSe{Channel.ToString()}:SWEep:TYPE {sweep}");
        }

        public void SetSweepType(int Channel, GeneralNFSweepTypeEnum sweeptype)
        {
            String sweep = Scpi.Format("{0}", sweeptype);
            ScpiCommand($"SENSe{Channel.ToString()}:SWEep:TYPE {sweep}");
        }

        public DataAcquisitionModeEnum GetDataAcquisitionMode(int Channel)
        {
            DataAcquisitionModeEnum retVal = DataAcquisitionModeEnum.SMARTSweep;
            String retString = ScpiQuery($"SENSe{Channel.ToString()}:GCSetup:AMODe?");
            if (retString.Equals("SMAR"))
            {
                retVal = DataAcquisitionModeEnum.SMARTSweep;
            }
            else if (retString.Equals("PFREQ"))
            {
                retVal = DataAcquisitionModeEnum.SweepPowerPerFrequency2D;
            }
            else if (retString.StartsWith("FPOW"))
            {
                retVal = DataAcquisitionModeEnum.SweepFrequencyPerPower2D;
            }
            return retVal;
        }

        public void SetDataAcquisitionMode(int Channel, DataAcquisitionModeEnum sweeptype)
        {
            String sweep = Scpi.Format("{0}", sweeptype);
            ScpiCommand($"SENSe{Channel.ToString()}:GCSetup:AMODe {sweep}");
        }
        #endregion
    }
}
