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
        #region Noise Figure
        public double GetNFBandwidth(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:NOISe:BWIDth?");
            return retVal;
        }

        public void SetNFBandwidth(int Channel, NoiseBandwidthNoise bw)
        {
            String noise = Scpi.Format("{0}", bw);
            double dblNoise = double.Parse(noise);
            SetNFBandwidth(Channel, dblNoise);
        }

        public void SetNFBandwidth(int Channel, NoiseBandwidthNormal bw)
        {
            String noise = Scpi.Format("{0}", bw);
            double dblNoise = double.Parse(noise);
            SetNFBandwidth(Channel, dblNoise);
        }

        public void SetNFBandwidth(int Channel, double bw)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:NOISe:BWIDth {bw.ToString()}");
        }

        public int GetNFAverage(int Channel)
        {
            int retVal = 1;
            retVal = ScpiQuery<int>($"SENSe{Channel.ToString()}:NOISe:AVERage?");
            return retVal;
        }

        public void SetNFAverage(int Channel, int avg)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:NOISe:AVERage {avg.ToString()}");
        }

        public bool GetNFNoiseAverage(int Channel)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:NOISe:AVERage:STATe?");
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

        public void SetNFNoiseAverage(int Channel, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:NOISe:AVERage:STATe 1");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:NOISe:AVERage:STATe 0");
            }
        }

        public bool GetNFNarrowbandNoiseFigureCompensation(int Channel)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:NOISe:NARRowband:STATe?");
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

        public void SetNFNarrowbandNoiseFigureCompensation(int Channel, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:NOISe:NARRowband:STATe 1");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:NOISe:NARRowband:STATe 0");
            }
        }

        public NoiseReceiver GetNFNoiseReceiver(int Channel)
        {
            NoiseReceiver retVal = NoiseReceiver.NoiseReceiver;
            String retString = ScpiQuery($"SENSe{Channel.ToString()}:NOISe:RECeiver?");
            if (retString.Equals("NORM"))
            {
                retVal = NoiseReceiver.NAReceiver;
            }
            else if (retString.Equals("NOIS"))
            {
                retVal = NoiseReceiver.NoiseReceiver;
            }
            else
            {
                throw new Exception("Unknown Noise Receiver!");
            }
            return retVal;
        }

        public void SetNFNoiseReceiver(int Channel, NoiseReceiver rec)
        {
            String nr = Scpi.Format("{0}", rec);
            ScpiCommand($"SENSe{Channel.ToString()}:NOISe:RECeiver {nr}");
        }

        public ReceiverGain GetNFReceiverGain(int Channel)
        {
            ReceiverGain retVal = ReceiverGain.High;
            int retInt = ScpiQuery<int>($"SENSe{Channel.ToString()}:NOISe:GAIN?");
            if (retInt == 0)
            {
                retVal = ReceiverGain.Low;
            }
            else if (retInt == 15)
            {
                retVal = ReceiverGain.Medium;
            }
            else if (retInt == 30)
            {
                retVal = ReceiverGain.High;
            }
            else
            {
                throw new Exception("Unknown Noise Receiver!");
            }
            return retVal;
        }

        public void SetNFReceiverGain(int Channel, ReceiverGain rec)
        {
            String nr = Scpi.Format("{0}", rec);
            ScpiCommand($"SENSe{Channel.ToString()}:NOISe:GAIN {nr}");
        }

        public double GetNFTemperature(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:NOISe:TEMPerature:SOURce?");
            return retVal;
        }

        public void SetNFTemperature(int Channel, double temp)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:NOISe:TEMPerature:SOURce {temp.ToString()}");
        }

        public bool GetNFUse302K(int Channel)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:NOISe:TEMPerature:SOURce:AUTO?");
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

        public void SetNFUse302K(int Channel, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:NOISe:TEMPerature:SOURce:AUTO 1");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:NOISe:TEMPerature:SOURce:AUTO 0");
            }
        }

        public int GetNFMaxImpedanceStates(int Channel)
        {
            int retVal = 4;
            retVal = ScpiQuery<int>($"SENSe{Channel.ToString()}:NOISe:IMPedance:COUNt?");
            return retVal;
        }

        public void SetNFMaxImpedanceStates(int Channel, int states)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:NOISe:IMPedance:COUNt {states.ToString()}");
        }

        public bool GetNFEnableSourcePulling (int Channel)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:NOISe:PULL?");
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

        public void SetNFEnableSourcePulling(int Channel, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:NOISe:PULL 1");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:NOISe:PULL 0");
            }
        }

        public bool GetNFEnableCustomNoiseTuner(int Channel)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:NOISe:TUNer:FILE:STATe?");
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

        public void SetNFEnableCustomNoiseTuner(int Channel, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:NOISe:TUNer:FILE:STATe 1");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:NOISe:TUNer:FILE:STATe 0");
            }
        }

        public String GetNFCustomNoiseTunerFile(int Channel)
        {
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:NOISe:TUNer:FILE:NAME?");
            return retStr;
        }

        public void SetNFCustomNoiseTunerFile(int Channel, String tunerfile)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:NOISe:TUNer:FILE:NAME \"{tunerfile}\"");
        }



        #endregion

        #region Power
        public int GetNFPortInput(int Channel)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENS{Channel.ToString()}:NOISe:PMAP:INP?");
            return retVal;
        }

        public int GetNFPortOutput(int Channel)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENS{Channel.ToString()}:NOISe:PMAP:OUTP?");
            return retVal;
        }

        public void SetNFPortInputOutput(int Channel, PortsEnum inport, PortsEnum outport)
        {
            String inp = Scpi.Format("{0}", inport);
            String outp = Scpi.Format("{0}", outport);
            ScpiCommand($"SENS{Channel.ToString()}:NOISe:PMAP {inp},{outp}");
        }

        public bool GetNFCoupledTonePowers(int Channel)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SOURce{Channel.ToString()}:POWer:COUPle?");
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

        public void SetNFCoupledTonePowers(int Channel, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SOURce{Channel.ToString()}:POWer:COUPle 1");
            }
            else
            {
                ScpiCommand($"SOURce{Channel.ToString()}:POWer:COUPle 0");
            }
        }

        // SOURce<cnum>:POWer<port>[:LEVel][:IMMediate][:AMPLitude]? [src]
        public double GetNFPowerLevel(int Channel, PortsEnum port)
        {
            double retVal = double.NaN;
            String p = Scpi.Format("{0}", port);
            retVal = ScpiQuery<double>($"SOURce{Channel.ToString()}:POWer{p}?");
            return retVal;
        }

        public void SetNFPowerLevel(int Channel, PortsEnum port, double power)
        {
            String p = Scpi.Format("{0}", port);
            ScpiCommand($"SOURce{Channel.ToString()}:POWer{p} {power.ToString()}");
        }






        #endregion

        #region Frequency
        // Same commands used in Gain Compression Converters
        // PNAConvertersCompression.cs



        #endregion
    }
}
