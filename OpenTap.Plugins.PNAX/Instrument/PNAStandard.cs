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
        #region Settings

        #endregion

        #region General|Standard|Sweep Type
        public String GetStandardSweepType(int Channel)
        {
            return ScpiQuery($"SENSe{ Channel }:SWEep:TYPE?"); ;
        }

        public void SetStandardSweepType(int Channel, StandardSweepTypeEnum standardSweepType)
        {
            string scpi = Scpi.Format("{0}", standardSweepType);
            ScpiCommand($"SENSe{ Channel }:SWEep:TYPE {scpi}");
        }

        // ScalerMixerSweepType
        public void SetStandardSweepType(int Channel, ScalerMixerSweepType standardSweepType)
        {
            string scpi = Scpi.Format("{0}", standardSweepType);
            ScpiCommand($"SENSe{ Channel }:SWEep:TYPE {scpi}");
        }


        public double GetStart(int Channel)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:FREQuency:STARt?");
        }

        public void SetStart(int Channel, double freq)
        {
            ScpiCommand($"SENSe{ Channel }:FREQuency:STARt { freq }");
        }

        public double GetStop(int Channel)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:FREQuency:STOP?"); ;
        }

        public void SetStop(int Channel, double freq)
        {
            ScpiCommand($"SENSe{ Channel }:FREQuency:STOP { freq }");
        }

        public double GetCenter(int Channel)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:FREQuency:CENTer?");
        }

        public void SetCenter(int Channel, double freq)
        {
            ScpiCommand($"SENSe{ Channel }:FREQuency:CENTer { freq }");
        }

        public double GetSpan(int Channel)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:FREQuency:SPAN?");
        }

        public void SetSpan(int Channel, double freq)
        {
            ScpiCommand($"SENSe{ Channel }:FREQuency:SPAN { freq }");
        }

        public double GetPower(int Channel)
        {
            return ScpiQuery<double>($"SOURce{ Channel }:POWer?");
        }

        public void SetPower(int Channel, double power)
        {
            ScpiCommand($"SOURce{ Channel }:POWer { power }");
        }

        public int GetPoints(int Channel)
        {
            return ScpiQuery<int>($"SENSe{ Channel }:SWEep:POINts?"); ;
        }

        public void SetPoints(int Channel, int points)
        {
            ScpiCommand($"SENSe{ Channel }:SWEep:POINts { points }");
        }

        public double GetIFBandwidth(int Channel)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:BANDwidth?"); ;
        }

        public void SetIFBandwidth(int Channel, double bw)
        {
            ScpiCommand($"SENSe{ Channel }:BANDwidth { bw }");
        }

        public double GetStartPower(int Channel)
        {
            return ScpiQuery<double>($"SOURce{ Channel }:POWer:STARt?"); ;
        }

        public void SetStartPower(int Channel, double power)
        {
            ScpiCommand($"SOURce{ Channel }:POWer:STARt { power }");
        }

        public double GetStopPower(int Channel)
        {
            return ScpiQuery<double>($"SOURce{ Channel }:POWer:STOP?"); ;
        }

        public void SetStopPower(int Channel, double power)
        {
            ScpiCommand($"SOURce{ Channel }:POWer:STOP {power}");
        }

        public double GetCWFreq(int Channel)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:FREQuency:CW?"); ;
        }

        public void SetCWFreq(int Channel, double freq)
        {
            ScpiCommand($"SENSe{ Channel }:FREQuency:CW { freq }");
        }

        public double GetPhaseStart(int Channel)
        {
            return ScpiQuery<double>($"SOURce{ Channel }:PHASe:STARt?"); ;
        }

        public void SetPhaseStart(int Channel, double phase)
        {
            ScpiCommand($"SOURce{ Channel }:PHASe:STARt { phase }");
        }

        public double GetPhaseStop(int Channel)
        {
            return ScpiQuery<double>($"SOURce{ Channel }:PHASe:STOP?"); ;
        }

        public void SetPhaseStop(int Channel, double phase)
        {
            ScpiCommand($"SOURce{ Channel }:PHASe:STOP { phase }");
        }
        #endregion

        #region General|Standard|Timing

        public bool GetAutoSweepTime(int Channel)
        {
            return ScpiQuery<bool>($"SENSe{ Channel }:SWEep:TIME:AUTO?"); ;
        }

        public void SetAutoSweepTime(int Channel, bool auto)
        {
            if (auto == true)
            {
                ScpiCommand($"SENSe{ Channel }:SWEep:TIME:AUTO 1");
            }
            else
            {
                ScpiCommand($"SENSe{ Channel }:SWEep:TIME:AUTO 0");
            }
        }

        public double GetSweepTime(int Channel)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:SWEep:TIME?"); ;
        }

        public void SetSweepTime(int Channel, double time)
        {
            ScpiCommand($"SENSe{ Channel }:SWEep:TIME { time }");
        }

        public double GetDwellTime(int Channel)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:SWEep:DWELl?"); ;
        }

        public void SetDwellTime(int Channel, double time)
        {
            ScpiCommand($"SENSe{ Channel }:SWEep:DWELl { time }");
        }

        public double GetSweepDelay(int Channel)
        {
            return ScpiQuery<double>($"SENSe{ Channel }:SWEep:DWELl:SDELay?"); ;
        }

        public void SetSweepDelay(int Channel, double time)
        {
            ScpiCommand($"SENSe{ Channel }:SWEep:DWELl:SDELay { time }");
        }

        public bool GetFastSweepMode(int Channel)
        {
            try
            {
                return ScpiQuery($"SENSe{ Channel }:SWEep:SPEed?").Equals("FAST");
            }
            catch (Exception)
            {
                Log.Error("No Connection to Instrument, Fast Sweep Mode");
                return false;
            }
        }

        public void SetFastSweepMode(int Channel, bool mode)
        {
            if (mode)
            {
                ScpiCommand($"SENSe{ Channel }:SWEep:SPEed FAST");
            }
            else
            {
                ScpiCommand($"SENSe{ Channel }:SWEep:SPEed NORM");
            }
        }

        public StandardChannelSweepModeEnum GetSweepMode(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{ Channel }:SWEep:GENeration?");
            if (retStr.Equals("STEP"))
            {
                return StandardChannelSweepModeEnum.Stepped;
            }
            else if (retStr.Equals("ANAL"))
            {
                return StandardChannelSweepModeEnum.Auto;
            }
            else
            {
                throw new Exception("unknown sweep mode");
            }
        }

        public void SetSweepMode(int Channel, StandardChannelSweepModeEnum mode)
        {
            if (mode == StandardChannelSweepModeEnum.Stepped)
            {
                ScpiCommand($"SENSe{ Channel }:SWEep:GENeration STEP");
            }
            else if (mode == StandardChannelSweepModeEnum.Auto)
            {
                ScpiCommand($"SENSe{ Channel }:SWEep:GENeration ANAL");
            }
        }

        public StandardChannelSweepSequenceEnum GetSweepSequence(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{ Channel }:SWEep:GENeration:POINtsweep?");
            if (retStr.Equals("OFF"))
            {
                return StandardChannelSweepSequenceEnum.Standard;
            }
            else 
            {
                return StandardChannelSweepSequenceEnum.PointSweep;
            }
        }

        public void SetSweepSequence(int Channel, StandardChannelSweepSequenceEnum mode)
        {
            if (mode == StandardChannelSweepSequenceEnum.Standard)
            {
                ScpiCommand($"SENSe{ Channel }:SWEep:GENeration:POINtsweep OFF");
            }
            else
            {
                ScpiCommand($"SENSe{ Channel }:SWEep:GENeration:POINtsweep ON");
            }
        }
        #endregion
    }
}
