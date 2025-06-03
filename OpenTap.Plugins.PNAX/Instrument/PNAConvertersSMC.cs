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
        #region Power
        public int GetSMCPortInput(int Channel)
        {
            return ScpiQuery<int>($"SENS{Channel}:MIXer:PMAP:INP?");
        }

        public int GetSMCPortOutput(int Channel)
        {
            return ScpiQuery<int>($"SENS{Channel}:MIXer:PMAP:OUTP?");
        }

        public void SetSMCPortInputOutput(int Channel, PortsEnum inport, PortsEnum outport)
        {
            string inp = Scpi.Format("{0}", inport);
            string outp = Scpi.Format("{0}", outport);
            ScpiCommand($"SENS{Channel}:MIXer:PMAP {inp},{outp}");
        }

        // SOURce<cnum>:POWer<port>:PORT:STARt?
        public double GetSMCPowerSweepStartPower(int Channel, PortsEnum port)
        {
            string strPort = Scpi.Format("{0}", port);
            return ScpiQuery<double>($"SOURce{Channel}:POWer{strPort}:PORT:STARt?");
        }

        public void SetSMCPowerSweepStartPower(int Channel, PortsEnum port, double power)
        {
            string strPort = Scpi.Format("{0}", port);
            ScpiCommand($"SOURce{Channel}:POWer{strPort}:PORT:STARt {power}");
        }

        public double GetSMCPowerSweepStopPower(int Channel, PortsEnum port)
        {
            string strPort = Scpi.Format("{0}", port);
            return ScpiQuery<double>($"SOURce{Channel}:POWer{strPort}:PORT:STOP?");
        }

        public void SetSMCPowerSweepStopPower(int Channel, PortsEnum port, double power)
        {
            string strPort = Scpi.Format("{0}", port);
            ScpiCommand($"SOURce{Channel}:POWer{strPort}:PORT:STOP {power}");
        }

        #endregion

        #region Sweep
        // SENSe<cnum>:SEGMent:X:SPACing <char>
        public bool GetXAxisPointSpacing(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel}:SEGMent:X:SPACing?");
            return !retStr.Equals("LIN");
        }

        public void SetXAxisPointSpacing(int Channel, bool mode)
        {
            string stateValue = mode ? "OBAS" : "LIN";
            ScpiCommand($"SENSe{Channel}:SEGMent:X:SPACing {stateValue}");
        }

        // SENSe<ch>:MIXer:REVerse?
        public bool GetReversedPort2Coupler(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel}:PATH:CONF:ELEM:STAT? \"Port2Coupler\"");
            return !retStr.Equals("Normal");
        }

        public void SetReversedPort2Coupler(int Channel, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SENSe{Channel}:PATH:CONF:ELEM:STAT \"Port2Coupler\",\"Reversed\"");
            }
            else
            {
                ScpiCommand($"SENSe{Channel}:PATH:CONF:ELEM:STAT \"Port2Coupler\",\"Normal\"");
            }
        }

        public bool GetAvoidSpurs(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel}:MIXer:AVOidspurs?");
            return !retStr.Equals("0");
        }

        public void SetAvoidSpurs(int Channel, bool mode)
        {
            string stateValue = mode ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:MIXer:AVOidspurs {stateValue}");
        }

        public bool GetMixerPhase(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel}:MIXer:PHASe?");
            return !retStr.Equals("0");
        }

        public void SetMixerPhase(int Channel, bool mode)
        {
            string stateValue = mode ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:MIXer:PHASe {stateValue}");
        }

        // SENSe<ch>:MIXer:NORMalize:POINt?
        public int GetNormalizingDataPoint(int Channel)
        {
            return ScpiQuery<int>($"SENS{Channel}:MIXer:NORMalize:POINt?");
        }

        public void SetNormalizingDataPoint(int Channel, int datapoint)
        {
            ScpiCommand($"SENS{Channel}:MIXer:NORMalize:POINt {datapoint}");
        }

        // SENSe<ch>:MIXer:PHASe:ABSolute[:STATe]?
        public bool GetMixerUseAbsolutePhase(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel}:MIXer:PHASe:ABSolute?");
            return !retStr.Equals("0");
        }

        public void SetMixerUseAbsolutePhase(int Channel, bool mode)
        {
            string stateValue = mode ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:MIXer:PHASe:ABSolute {stateValue}");
        }

        #endregion
    }
}
