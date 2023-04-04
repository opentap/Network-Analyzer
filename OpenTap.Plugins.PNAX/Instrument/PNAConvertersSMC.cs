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
        #region Power
        public int GetSMCPortInput(int Channel)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENS{Channel.ToString()}:MIXer:PMAP:INP?");
            return retVal;
        }

        public int GetSMCPortOutput(int Channel)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENS{Channel.ToString()}:MIXer:PMAP:OUTP?");
            return retVal;
        }

        public void SetSMCPortInputOutput(int Channel, PortsEnum inport, PortsEnum outport)
        {
            String inp = Scpi.Format("{0}", inport);
            String outp = Scpi.Format("{0}", outport);
            ScpiCommand($"SENS{Channel.ToString()}:MIXer:PMAP {inp},{outp}");
        }

        // SOURce<cnum>:POWer<port>:PORT:STARt?
        public double GetSMCPowerSweepStartPower(int Channel, PortsEnum port)
        {
            double retVal = double.NaN;
            String strPort = Scpi.Format("{0}", port);
            retVal = ScpiQuery<double>($"SOURce{Channel.ToString()}:POWer{strPort}:PORT:STARt?");
            return retVal;
        }

        public void SetSMCPowerSweepStartPower(int Channel, PortsEnum port, double power)
        {
            String strPort = Scpi.Format("{0}", port);
            ScpiCommand($"SOURce{Channel.ToString()}:POWer{strPort}:PORT:STARt {power.ToString()}");
        }

        public double GetSMCPowerSweepStopPower(int Channel, PortsEnum port)
        {
            double retVal = double.NaN;
            String strPort = Scpi.Format("{0}", port);
            retVal = ScpiQuery<double>($"SOURce{Channel.ToString()}:POWer{strPort}:PORT:STOP?");
            return retVal;
        }

        public void SetSMCPowerSweepStopPower(int Channel, PortsEnum port, double power)
        {
            String strPort = Scpi.Format("{0}", port);
            ScpiCommand($"SOURce{Channel.ToString()}:POWer{strPort}:PORT:STOP {power.ToString()}");
        }


        #endregion

        #region Sweep
        // SENSe<cnum>:SEGMent:X:SPACing <char>
        public bool GetXAxisPointSpacing(int Channel)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:SEGMent:X:SPACing?");
            if (retStr.Equals("LIN"))
            {
                retVal = false;
            }
            else
            {
                retVal = true;
            }
            return retVal;
        }

        public void SetXAxisPointSpacing(int Channel, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:SEGMent:X:SPACing OBAS");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:SEGMent:X:SPACing LIN");
            }
        }

        // SENSe<ch>:MIXer:REVerse?
        public bool GetReversedPort2Coupler(int Channel)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:MIXer:REVerse?");
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

        public void SetReversedPort2Coupler(int Channel, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:MIXer:REVerse ON");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:MIXer:REVerse OFF");
            }
        }

        public bool GetAvoidSpurs(int Channel)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:MIXer:AVOidspurs?");
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

        public void SetAvoidSpurs(int Channel, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:MIXer:AVOidspurs ON");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:MIXer:AVOidspurs OFF");
            }
        }

        // SENSe<ch>:MIXer:PHASe[:STATe]?
        public bool GetMixerPhase(int Channel)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:MIXer:PHASe?");
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

        public void SetMixerPhase(int Channel, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:MIXer:PHASe ON");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:MIXer:PHASe OFF");
            }
        }

        // SENSe<ch>:MIXer:NORMalize:POINt?
        public int GetNormalizingDataPoint(int Channel)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENS{Channel.ToString()}:MIXer:NORMalize:POINt?");
            return retVal;
        }

        public void SetNormalizingDataPoint(int Channel, int datapoint)
        {
            ScpiCommand($"SENS{Channel.ToString()}:MIXer:NORMalize:POINt {datapoint.ToString()}");
        }

        // SENSe<ch>:MIXer:PHASe:ABSolute[:STATe]?
        public bool GetMixerUseAbsolutePhase(int Channel)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:MIXer:PHASe:ABSolute?");
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

        public void SetMixerUseAbsolutePhase(int Channel, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:MIXer:PHASe:ABSolute ON");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:MIXer:PHASe:ABSolute OFF");
            }
        }


        #endregion
    }
}
