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
    public enum SASweepTypeEnum
    {
        [Scpi("LINear")]
        [Display("Linear Frequency")]
        LinearFrequency,
        [Scpi("SEGMent")]
        [Display("Segment Sweep")]
        SegmentSweep
    }

    public enum SASourceSweepTypeEnum
    {
        [Scpi("CW")]
        [Display("CW Time")]
        CWTime,
        [Scpi("LIN")]
        [Display("Linear Frequency")]
        LinearFrequency,
        [Scpi("POW")]
        [Display("Power Sweep")]
        PowerSweep,
        [Scpi("LFP")]
        [Display("LinF+Power")]
        LinFPower,
    }

    public enum SADetectorTypeEnum
    {
        [Scpi("PEAK")]
        [Display("Peak")]
        Peak,
        [Scpi("AVERage")]
        [Display("Average")]
        Average,
        [Scpi("SAMPle")]
        [Display("Sample")]
        Sample,
        [Scpi("NORMal")]
        [Display("Normal")]
        Normal,
        [Scpi("NEGPeak")]
        [Display("NegPeak")]
        NegPeak,
        [Scpi("PSAMple")]
        [Display("Peak Sample")]
        PeakSample,
        [Scpi("PAVerage")]
        [Display("Peak Average")]
        PeakAverage,
        [Scpi("FASPeak")]
        [Display("Fast Peak")]
        FastPeak
    }

    public enum SAVideoAverageTypeEnum
    {
        [Scpi("VOLT")]
        [Display("Voltage")]
        Voltage,
        [Scpi("POW")]
        [Display("Power")]
        Power,
        [Scpi("LOG")]
        [Display("Log")]
        Log,
        [Scpi("VMAX")]
        [Display("Voltage Max")]
        VoltageMax,
        [Scpi("VMIN")]
        [Display("Voltage Min")]
        VoltageMin
    }

    public enum SAReceiverAttenuatorEnum
    {
        [Scpi("AREC")]
        [Display("A Receiver")]
        AREC,
        [Scpi("BREC")]
        [Display("B Receiver")]
        BREC,
        [Scpi("CREC")]
        [Display("C Receiver")]
        CREC,
        [Scpi("DREC")]
        [Display("D Receiver")]
        DREC
    }

    public enum SAOnOffTypeEnum
    {
        [Scpi("OFF")]
        [Display("OFF")]
        Off,
        [Scpi("ON")]
        [Display("ON")]
        On
    }

    public enum SASourceSweepOrderTypeEnum
    {
        [Scpi("FREQ")]
        FrequencyPower,
        [Scpi("POW")]
        PowerFrequency
    }

    public partial class PNAX : ScpiInstrument
    {
        #region SA Setup
        public double GetSAResolutionBandwidth(int Channel)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:SA:BANDwidth:RESolution?"); ;
        }

        public void SetSAResolutionBandwidth(int Channel, double resbw)
        {
            ScpiCommand($"SENSe{ Channel }:SA:BANDwidth:RESolution { resbw }");
        }

        public bool GetSAResolutionBandwidthAuto(int Channel)
        {
            bool retVal = false;
            int retInt = ScpiQuery<int>($"SENSe{ Channel }:SA:BANDwidth:RESolution:AUTO?");
            if (retInt == 1)
            {
                retVal = true;
            }

            return retVal;
        }

        public void SetSAResolutionBandwidthAuto(int Channel, bool auto)
        {
            if (auto == true)
            {
                ScpiCommand($"SENSe{ Channel }:SA:BANDwidth:RESolution:AUTO 1");
            }
            else
            {
                ScpiCommand($"SENSe{ Channel }:SA:BANDwidth:RESolution:AUTO 0");
            }
        }

        public double GetSAVideoBandwidth(int Channel)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:SA:BANDwidth:VIDeo?"); ;
        }

        public void SetSAVideoBandwidth(int Channel, double vidbw)
        {
            ScpiCommand($"SENSe{ Channel }:SA:BANDwidth:VIDeo { vidbw }");
        }

        public bool GetSAVideoBandwidthAuto(int Channel)
        {
            bool retVal = false;
            int retInt = ScpiQuery<int>($"SENSe{ Channel }:SA:BANDwidth:VIDeo:AUTO?");
            if (retInt == 1)
            {
                retVal = true;
            }

            return retVal;
        }

        public void SetSAVideoBandwidthAuto(int Channel, bool auto)
        {
            if (auto == true)
            {
                ScpiCommand($"SENSe{ Channel }:SA:BANDwidth:VIDeo:AUTO 1");
            }
            else
            {
                ScpiCommand($"SENSe{ Channel }:SA:BANDwidth:VIDeo:AUTO 0");
            }
        }

        public SADetectorTypeEnum GetSADetectorType(int Channel)
        {
            SADetectorTypeEnum retVal = SADetectorTypeEnum.Peak;

            String retStr = ScpiQuery($"SENSe{ Channel }:SA:DETector:FUNCtion?");

            if (retStr.Equals("PEAK"))
            {
                retVal = SADetectorTypeEnum.Peak;
            }
            else if (retStr.Equals("AVER"))
            {
                retVal = SADetectorTypeEnum.Average;
            }
            else if (retStr.Equals("SAMP"))
            {
                retVal = SADetectorTypeEnum.Sample;
            }
            else if (retStr.Equals("NORM"))
            {
                retVal = SADetectorTypeEnum.Normal;
            }
            else if (retStr.Equals("NEGP"))
            {
                retVal = SADetectorTypeEnum.NegPeak;
            }
            else if (retStr.Equals("PSAM"))
            {
                retVal = SADetectorTypeEnum.PeakSample;
            }
            else if (retStr.Equals("PAV"))
            {
                retVal = SADetectorTypeEnum.PeakAverage;
            }
            else if (retStr.Equals("FASP"))
            {
                retVal = SADetectorTypeEnum.FastPeak;
            }

            return retVal;
        }

        public void SetSADetectorType(int Channel, SADetectorTypeEnum detector)
        {
            string scpi = Scpi.Format("{0}", detector);
            ScpiCommand($"SENSe{ Channel }:SA:DETector:FUNCtion {scpi}");
        }

        public bool GetSADetectorBypass(int Channel)
        {
            bool retVal = false;
            int retInt = ScpiQuery<int>($"SENSe{ Channel }:SA:DETector:BYPass:STATe?");
            if (retInt == 1)
            {
                retVal = true;
            }

            return retVal;
        }

        public void SetSADetectorBypass(int Channel, bool bypass)
        {
            if (bypass == true)
            {
                ScpiCommand($"SENSe{ Channel }:SA:DETector:BYPass:STATe 1");
            }
            else
            {
                ScpiCommand($"SENSe{ Channel }:SA:DETector:BYPass:STATe 0");
            }
        }

        //
        public SAVideoAverageTypeEnum GetSAVideoAverageType(int Channel)
        {
            SAVideoAverageTypeEnum retVal = SAVideoAverageTypeEnum.Power;

            String retStr = ScpiQuery($"SENSe{ Channel }:SA:BANDwidth:VIDeo:AVER:TYPE?");

            if (retStr.Equals("VOLT"))
            {
                retVal = SAVideoAverageTypeEnum.Voltage;
            }
            else if (retStr.Equals("POW"))
            {
                retVal = SAVideoAverageTypeEnum.Power;
            }
            else if (retStr.Equals("LOG"))
            {
                retVal = SAVideoAverageTypeEnum.Log;
            }
            else if (retStr.Equals("VMAX"))
            {
                retVal = SAVideoAverageTypeEnum.VoltageMax;
            }
            else if (retStr.Equals("VMIN"))
            {
                retVal = SAVideoAverageTypeEnum.VoltageMin;
            }

            return retVal;
        }

        public void SetSAVideoAverageType(int Channel, SAVideoAverageTypeEnum videoaverage)
        {
            string scpi = Scpi.Format("{0}", videoaverage);
            ScpiCommand($"SENSe{ Channel }:SA:BANDwidth:VIDeo:AVER:TYPE {scpi}");
        }

        public int GetSAVideoAverageCount(int Channel)
        {
            return ScpiQuery<int>($"SENSe{ Channel }:SA:BANDwidth:VIDeo:AVER:COUNt?");
        }

        public double GetSAReceiverAttenuation(int Channel, SAReceiverAttenuatorEnum recvr)
        {
            string scpi = Scpi.Format("{0}", recvr);
            return ScpiQuery<double>($"SENSe{ Channel }:POWer:ATTenuator? {scpi}");
        }

        public void SetSAReceiverAttenuation(int Channel, SAReceiverAttenuatorEnum recvr, double att)
        {
            string scpi = Scpi.Format("{0}", recvr);
            ScpiCommand($"SENSe{ Channel }:POWer:ATTenuator {scpi},{att}");
        }

        public void SetSASweepOrder(int Channel, SASourceSweepOrderTypeEnum sweepOrder)
        {
            string scpi = Scpi.Format("{0}", sweepOrder);
            ScpiCommand($"SENSe{Channel}:SA:SOURce:SWEep:FIRst:DIMension {scpi}");
        }
        #endregion

        #region SA Source

        public SAOnOffTypeEnum GetSASourcePowerMode(int Channel, String port)
        {
            SAOnOffTypeEnum retVal = ScpiQuery<SAOnOffTypeEnum>($"SOURce{Channel}:POWer:MODE? \"{port}\"");
            return retVal;
        }

        public void SetSASourcePowerMode(int Channel, String port, SAOnOffTypeEnum state)
        {
            string scpi = Scpi.Format("{0}", state);
            SetSourcePowerMode(Channel, port, scpi);
        }

        public SASourceSweepTypeEnum GetSASweepType(int Channel, String src)
        {
            SASourceSweepTypeEnum retVal = ScpiQuery<SASourceSweepTypeEnum>($"SENSe{ Channel }:SA:SOURce:SWEep:TYPE? \"{src}\"");
            return retVal;
        }

        public void SetSASweepType(int Channel, String src, SASourceSweepTypeEnum saSourceSweepType)
        {
            string scpi = Scpi.Format("{0}", saSourceSweepType);
            ScpiCommand($"SENSe{ Channel }:SA:SOURce:SWEep:TYPE {scpi},\"{src}\"");
        }

        public double GetSAFrequencyStart(int Channel, String src)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:SA:SOURce:FREQuency:STARt? \"{src}\"");
        }

        public void SetSAFrequencyStart(int Channel, String src, double freq)
        {
            ScpiCommand($"SENSe{ Channel }:SA:SOURce:FREQuency:STARt { freq },\"{src}\"");
        }

        public double GetSAFrequencyStop(int Channel, String src)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:SA:SOURce:FREQuency:STOP? \"{src}\""); ;
        }

        public void SetSAFrequencyStop(int Channel, String src, double freq)
        {
            ScpiCommand($"SENSe{ Channel }:SA:SOURce:FREQuency:STOP { freq },\"{src}\"");
        }

        public double GetSAFrequencyCW(int Channel, String src)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:SA:SOURce:FREQuency:CW? \"{src}\"");
        }

        public void SetSAFrequencyCW(int Channel, String src, double freq)
        {
            ScpiCommand($"SENSe{ Channel }:SA:SOURce:FREQuency:CW { freq },\"{src}\"");
        }

        public int GetSAFrequencySteps(int Channel)
        {
            return ScpiQuery<int>($"SENSe{ Channel }:SA:SOURce:SWEep:POINt:COUNt?");
        }

        public void SetSAFrequencySteps(int Channel, int steps)
        {
            ScpiCommand($"SENSe{ Channel }:SA:SOURce:SWEep:POINt:COUNt { steps }");
        }

        public int GetSAFrequencyRepeat(int Channel)
        {
            return ScpiQuery<int>($"SENSe{ Channel }:SA:SOURce:SWEep:REPeat:COUNt?");
        }

        public void SetSAFrequencyRepeat(int Channel, int repeats)
        {
            ScpiCommand($"SENSe{ Channel }:SA:SOURce:SWEep:REPeat:COUNt { repeats }");
        }




        public double GetSAPowerStart(int Channel, String src)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:SA:SOURce:POWer:STARt? \"{src}\"");
        }

        public void SetSAPowerStart(int Channel, String src, double freq)
        {
            ScpiCommand($"SENSe{ Channel }:SA:SOURce:POWer:STARt { freq },\"{src}\"");
        }

        public double GetSAPowerStop(int Channel, String src)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:SA:SOURce:POWer:STOP? \"{src}\""); ;
        }

        public void SetSAPowerStop(int Channel, String src, double freq)
        {
            ScpiCommand($"SENSe{ Channel }:SA:SOURce:POWer:STOP { freq },\"{src}\"");
        }

        public double GetSAPowerLevel(int Channel, String src)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:SA:SOURce:POWer:VALue? \"{src}\"");
        }

        public void SetSAPowerLevel(int Channel, String src, double freq)
        {
            ScpiCommand($"SENSe{ Channel }:SA:SOURce:POWer:VALue { freq },\"{src}\"");
        }

        public int GetSAPowerSteps(int Channel)
        {
            return ScpiQuery<int>($"SENSe{ Channel }:SA:SOURce:POW:SWEep:POINt:COUNt?");
        }

        public void SetSAPowerSteps(int Channel, int steps)
        {
            ScpiCommand($"SENSe{ Channel }:SA:SOURce:POW:SWEep:POINt:COUNt { steps }");
        }

        public int GetSAPowerRepeat(int Channel)
        {
            return ScpiQuery<int>($"SENSe{ Channel }:SA:SOURce:POW:SWEep:REPeat:COUNt?");
        }

        public void SetSAPowerRepeat(int Channel, int repeats)
        {
            ScpiCommand($"SENSe{ Channel }:SA:SOURce:POW:SWEep:REPeat:COUNt { repeats }");
        }




        public double GetSAPhaseStart(int Channel, String src)
        {
            return ScpiQuery<double>($"SOURce{ Channel }:PHASe:STARt? \"{src}\"");
        }

        public void SetSAPhaseStart(int Channel, String src, double freq)
        {
            ScpiCommand($"SOURce{ Channel }:PHASe:STARt { freq },\"{src}\"");
        }

        public double GetSAPhaseStop(int Channel, String src)
        {
            return ScpiQuery<double>($"SOURce{ Channel }:PHASe:STOP? \"{src}\""); ;
        }

        public void SetSAPhaseStop(int Channel, String src, double freq)
        {
            ScpiCommand($"SOURce{ Channel }:PHASe:STOP { freq },\"{src}\"");
        }

        public double GetSAPhaseLevel(int Channel, String src)
        {
            return ScpiQuery<double>($"SOURce{ Channel }:PHASe:FIXed? \"{src}\"");
        }

        public void SetSAPhaseLevel(int Channel, String src, double freq)
        {
            ScpiCommand($"SOURce{ Channel }:PHASe:FIXed { freq },\"{src}\"");
        }


        // TODO
        // Phase Number of Steps
        // Phase Sweeps per source step
        // TODO


        #endregion
    }
}
