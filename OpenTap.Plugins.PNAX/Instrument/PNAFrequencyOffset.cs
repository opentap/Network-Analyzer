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
        public bool GetFOMState(int Channel)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:FOM:STATe?");
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

        public void SetFOMState(int Channel, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:FOM:STATe 1");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:FOM:STATe 0");
            }
        }

        public bool GetFOMMode(int Channel, int Range)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:FOM:RANGe{Range}:COUPled?");
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

        public void SetFOMMode(int Channel, int Range, bool mode)
        {
            if (mode == true)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:FOM:RANGe{Range}:COUPled 1");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:FOM:RANGe{Range}:COUPled 0");
            }
        }

        public void SetFOMSweepType(int Channel, int Range, StandardSweepTypeEnum standardSweepType)
        {
            string scpi = Scpi.Format("{0}", standardSweepType);
            ScpiCommand($"SENSe{ Channel }:FOM:RANGe{Range}:SWEep:TYPE {scpi}");
        }

        public double GetFOMStart(int Channel, int Range)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:FOM:RANGe{Range}:FREQuency:STARt?");
        }

        public void SetFOMStart(int Channel, int Range, double freq)
        {
            ScpiCommand($"SENSe{ Channel }:FOM:RANGe{Range}:FREQuency:STARt { freq }");
        }

        public double GetFOMStop(int Channel, int Range)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:FOM:RANGe{Range}:FREQuency:STOP?"); ;
        }

        public void SetFOMStop(int Channel, int Range, double freq)
        {
            ScpiCommand($"SENSe{ Channel }:FOM:RANGe{Range}:FREQuency:STOP { freq }");
        }

        public double GetFOMCW(int Channel, int Range)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:FOM:RANGe{Range}:FREQuency:CENTer?");
        }

        public void SetFOMCW(int Channel, int Range, double freq)
        {
            ScpiCommand($"SENSe{ Channel }:FOM:RANGe{Range}:FREQuency:CENTer { freq }");
        }

        public double GetFOMOffset(int Channel, int Range)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:FOM:RANGe{Range}:FREQuency:OFFSet?");
        }

        public void SetFOMOffset(int Channel, int Range, double freq)
        {
            ScpiCommand($"SENSe{ Channel }:FOM:RANGe{Range}:FREQuency:OFFSet { freq }");
        }

        public double GetFOMDivisor(int Channel, int Range)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:FOM:RANGe{Range}:FREQuency:DIVisor?");
        }

        public void SetFOMDivisor(int Channel, int Range, double freq)
        {
            ScpiCommand($"SENSe{ Channel }:FOM:RANGe{Range}:FREQuency:DIVisor { freq }");
        }

        public double GetFOMMultiplier(int Channel, int Range)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:FOM:RANGe{Range}:FREQuency:MULTiplier?");
        }

        public void SetFOMMultiplier(int Channel, int Range, double freq)
        {
            ScpiCommand($"SENSe{ Channel }:FOM:RANGe{Range}:FREQuency:MULTiplier { freq }");
        }

        public double GetFOMXAxis(int Channel)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:FOM:DISPlay:SELect?");
        }

        public void SetFOMXAxis(int Channel, string value)
        {
            ScpiCommand($"SENSe{ Channel }:FOM:DISPlay:SELect { value }");
        }

    }
}
