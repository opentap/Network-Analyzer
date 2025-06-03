using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using OpenTap;

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
            return ScpiQuery<double>($"SENSe{Channel}:NOISe:BWIDth?");
        }

        public void SetNFBandwidth(int Channel, NoiseBandwidthNoise bw)
        {
            string noise = Scpi.Format("{0}", bw);
            double dblNoise = double.Parse(noise);
            SetNFBandwidth(Channel, dblNoise);
        }

        public void SetNFBandwidth(int Channel, NoiseBandwidthNormal bw)
        {
            string noise = Scpi.Format("{0}", bw);
            double dblNoise = double.Parse(noise);
            SetNFBandwidth(Channel, dblNoise);
        }

        public void SetNFBandwidth(int Channel, double bw)
        {
            ScpiCommand($"SENSe{Channel}:NOISe:BWIDth {bw}");
        }

        public int GetNFAverage(int Channel)
        {
            return ScpiQuery<int>($"SENSe{Channel}:NOISe:AVERage?");
        }

        public void SetNFAverage(int Channel, int avg)
        {
            ScpiCommand($"SENSe{Channel}:NOISe:AVERage {avg}");
        }

        public bool GetNFNoiseAverage(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel}:NOISe:AVERage:STATe?");
            return !retStr.Equals("0");
        }

        public void SetNFNoiseAverage(int Channel, bool mode)
        {
            string stateValue = mode ? "1" : "0";
            ScpiCommand($"SENSe{Channel}:NOISe:AVERage:STATe {stateValue}");
        }

        public bool GetNFNarrowbandNoiseFigureCompensation(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel}:NOISe:NARRowband:STATe?");
            return !retStr.Equals("0");
        }

        public void SetNFNarrowbandNoiseFigureCompensation(int Channel, bool mode)
        {
            string stateValue = mode ? "1" : "0";
            ScpiCommand($"SENSe{Channel}:NOISe:NARRowband:STATe {stateValue}");
        }

        public NoiseReceiver GetNFNoiseReceiver(int Channel)
        {
            string retString = ScpiQuery($"SENSe{Channel}:NOISe:RECeiver?");
            if (retString.Equals("NORM"))
            {
                return NoiseReceiver.NAReceiver;
            }
            else if (retString.Equals("NOIS"))
            {
                return NoiseReceiver.NoiseReceiver;
            }
            else
            {
                throw new Exception("Unknown Noise Receiver!");
            }
        }

        public void SetNFNoiseReceiver(int Channel, NoiseReceiver rec)
        {
            string nr = Scpi.Format("{0}", rec);
            ScpiCommand($"SENSe{Channel}:NOISe:RECeiver {nr}");
        }

        public ReceiverGain GetNFReceiverGain(int Channel)
        {
            int retInt = ScpiQuery<int>($"SENSe{Channel}:NOISe:GAIN?");
            if (retInt == 0)
            {
                return ReceiverGain.Low;
            }
            else if (retInt == 15)
            {
                return ReceiverGain.Medium;
            }
            else if (retInt == 30)
            {
                return ReceiverGain.High;
            }
            else
            {
                throw new Exception("Unknown Noise Receiver!");
            }
        }

        public void SetNFReceiverGain(int Channel, ReceiverGain rec)
        {
            string nr = Scpi.Format("{0}", rec);
            ScpiCommand($"SENSe{Channel}:NOISe:GAIN {nr}");
        }

        public double GetNFTemperature(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:NOISe:TEMPerature:SOURce?");
        }

        public void SetNFTemperature(int Channel, double temp)
        {
            ScpiCommand($"SENSe{Channel}:NOISe:TEMPerature:SOURce {temp}");
        }

        public bool GetNFUse302K(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel}:NOISe:TEMPerature:SOURce:AUTO?");
            return !retStr.Equals("0");
        }

        public void SetNFUse302K(int Channel, bool mode)
        {
            string stateValue = mode ? "1" : "0";
            ScpiCommand($"SENSe{Channel}:NOISe:TEMPerature:SOURce:AUTO {stateValue}");
        }

        public int GetNFMaxImpedanceStates(int Channel)
        {
            return ScpiQuery<int>($"SENSe{Channel}:NOISe:IMPedance:COUNt?");
        }

        public void SetNFMaxImpedanceStates(int Channel, int states)
        {
            ScpiCommand($"SENSe{Channel}:NOISe:IMPedance:COUNt {states}");
        }

        public bool GetNFEnableSourcePulling(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel}:NOISe:PULL?");
            return !retStr.Equals("0");
        }

        public void SetNFEnableSourcePulling(int Channel, bool mode)
        {
            string stateValue = mode ? "1" : "0";
            ScpiCommand($"SENSe{Channel}:NOISe:PULL {stateValue}");
        }

        public bool GetNFEnableCustomNoiseTuner(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel}:NOISe:TUNer:FILE:STATe?");
            return !retStr.Equals("0");
        }

        public void SetNFEnableCustomNoiseTuner(int Channel, bool mode)
        {
            string stateValue = mode ? "1" : "0";
            ScpiCommand($"SENSe{Channel}:NOISe:TUNer:FILE:STATe {stateValue}");
        }

        public string GetNFCustomNoiseTunerFile(int Channel)
        {
            return ScpiQuery($"SENSe{Channel}:NOISe:TUNer:FILE:NAME?");
        }

        public void SetNFCustomNoiseTunerFile(int Channel, string tunerfile)
        {
            ScpiCommand($"SENSe{Channel}:NOISe:TUNer:FILE:NAME \"{tunerfile}\"");
        }

        #endregion

        #region Power
        public int GetNFPortInput(int Channel)
        {
            return ScpiQuery<int>($"SENS{Channel}:NOISe:PMAP:INP?");
        }

        public int GetNFPortOutput(int Channel)
        {
            return ScpiQuery<int>($"SENS{Channel}:NOISe:PMAP:OUTP?");
        }

        public void SetNFPortInputOutput(int Channel, PortsEnum inport, PortsEnum outport)
        {
            string inp = Scpi.Format("{0}", inport);
            string outp = Scpi.Format("{0}", outport);
            ScpiCommand($"SENS{Channel}:NOISe:PMAP {inp},{outp}");
        }

        public bool GetCoupledTonePowers(int Channel)
        {
            string retStr = ScpiQuery($"SOURce{Channel}:POWer:COUPle?");
            return !retStr.Equals("0");
        }

        public void SetCoupledTonePowers(int Channel, bool mode)
        {
            string stateValue = mode ? "1" : "0";
            ScpiCommand($"SOURce{Channel}:POWer:COUPle {stateValue}");
        }

        // SOURce<cnum>:POWer<port>[:LEVel][:IMMediate][:AMPLitude]? [src]
        public double GetPowerLevel(int Channel, PortsEnum port)
        {
            string p = Scpi.Format("{0}", port);
            return ScpiQuery<double>($"SOURce{Channel}:POWer{p}?");
        }

        public void SetPowerLevel(int Channel, PortsEnum port, double power)
        {
            string p = Scpi.Format("{0}", port);
            ScpiCommand($"SOURce{Channel}:POWer{p} {power}");
        }

        #endregion

        #region Frequency
        // Same commands used in Gain Compression Converters
        // PNAConvertersCompression.cs

        #endregion
    }
}
