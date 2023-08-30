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


    public partial class PNAX : ScpiInstrument
    {
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
    }
}
