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
            String retVal = "";
            retVal = ScpiQuery($"SENSe{Channel.ToString()}:SWEep:TYPE?");
            return retVal;
        }

        public void SetStandardSweepType(int Channel, StandardSweepTypeEnum standardSweepType)
        {
            String scpi = Scpi.Format("{0}", standardSweepType);
            ScpiCommand($"SENSe{Channel.ToString()}:SWEep:TYPE {scpi}");
        }

        public double GetStart(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel.ToString()}:FREQuency:STARt?");
        }

        public void SetStart(int Channel, double freq)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:FREQuency:STARt {freq.ToString()}");
        }

        public double GetStop(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel.ToString()}:FREQuency:STOP?"); ;
        }

        public void SetStop(int Channel, double freq)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:FREQuency:STOP {freq.ToString()}");
        }

        public double GetCenter(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:FREQuency:CENTer?");
            return retVal;
        }

        public void SetCenter(int Channel, double freq)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:FREQuency:CENTer {freq.ToString()}");
        }

        public double GetSpan(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:FREQuency:SPAN?");
            return retVal;
        }

        public void SetSpan(int Channel, double freq)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:FREQuency:SPAN {freq.ToString()}");
        }

        public double GetPower(int Channel)
        {
            return ScpiQuery<double>($"SOURce{Channel.ToString()}:POWer?");
        }

        public void SetPower(int Channel, double power)
        {
            ScpiCommand($"SOURce{Channel.ToString()}:POWer {power.ToString()}");
        }

        public int GetPoints(int Channel)
        {
            return ScpiQuery<int>($"SENSe{Channel.ToString()}:SWEep:POINts?"); ;
        }

        public void SetPoints(int Channel, int points)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:SWEep:POINts {points.ToString()}");
        }

        public double GetIFBandwidth(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel.ToString()}:BANDwidth?"); ;
        }

        public void SetIFBandwidth(int Channel, double bw)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:BANDwidth {bw.ToString()}");
        }

        public double GetStartPower(int Channel)
        {
            return ScpiQuery<double>($"SOURce{Channel.ToString()}:POWer:STARt?"); ;
        }

        public void SetStartPower(int Channel, double power)
        {
            ScpiCommand($"SOURce{Channel.ToString()}:POWer:STARt {power.ToString()}");
        }

        public double GetStopPower(int Channel)
        {
            return ScpiQuery<double>($"SOURce{Channel.ToString()}:POWer:STOP?"); ;
        }

        public void SetStopPower(int Channel, double power)
        {
            ScpiCommand($"SOURce{Channel.ToString()}:POWer:STOP {power}");
        }

        public double GetCWFreq(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel.ToString()}:FREQuency:CW?"); ;
        }

        public void SetCWFreq(int Channel, double freq)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:FREQuency:CW {freq.ToString()}");
        }

        public double GetPhaseStart(int Channel)
        {
            return ScpiQuery<double>($"SOURce{Channel.ToString()}:PHASe:STARt?"); ;
        }

        public void SetPhaseStart(int Channel, double phase)
        {
            ScpiCommand($"SOURce{Channel.ToString()}:PHASe:STARt {phase.ToString()}");
        }

        public double GetPhaseStop(int Channel)
        {
            return ScpiQuery<double>($"SOURce{Channel.ToString()}:PHASe:STOP?"); ;
        }

        public void SetPhaseStop(int Channel, double phase)
        {
            ScpiCommand($"SOURce{Channel.ToString()}:PHASe:STOP {phase.ToString()}");
        }
        #endregion

        #region General|Standard|Timing

        public bool GetAutoSweepTime(int Channel)
        {
            return ScpiQuery<bool>($"SENSe{Channel.ToString()}:SWEep:TIME:AUTO?"); ;
        }

        public void SetAutoSweepTime(int Channel, bool auto)
        {
            if (auto == true)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:SWEep:TIME:AUTO 1");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:SWEep:TIME:AUTO 0");
            }
        }

        public double GetSweepTime(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel.ToString()}:SWEep:TIME?"); ;
        }

        public void SetSweepTime(int Channel, double time)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:SWEep:TIME {time.ToString()}");
        }

        public double GetDwellTime(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel.ToString()}:SWEep:DWELl?"); ;
        }

        public void SetDwellTime(int Channel, double time)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:SWEep:DWELl {time.ToString()}");
        }

        public double GetSweepDelay(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel.ToString()}:SWEep:DWELl:SDELay?"); ;
        }

        public void SetSweepDelay(int Channel, double time)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:SWEep:DWELl:SDELay {time.ToString()}");
        }

        public bool GetFastSweepMode(int Channel)
        {
            try
            {
                return ScpiQuery($"SENSe{Channel.ToString()}:SWEep:SPEed?").Equals("FAST");
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
                ScpiCommand($"SENSe{Channel.ToString()}:SWEep:SPEed FAST");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:SWEep:SPEed NORM");
            }
        }

        public StandardChannelSweepModeEnum GetSweepMode(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel.ToString()}:SWEep:GENeration?");
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
                ScpiCommand($"SENSe{Channel.ToString()}:SWEep:GENeration STEP");
            }
            else if (mode == StandardChannelSweepModeEnum.Auto)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:SWEep:GENeration ANAL");
            }
        }

        public StandardChannelSweepSequenceEnum GetSweepSequence(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel.ToString()}:SWEep:GENeration:POINtsweep?");
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
                ScpiCommand($"SENSe{Channel.ToString()}:SWEep:GENeration:POINtsweep OFF");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:SWEep:GENeration:POINtsweep ON");
            }
        }
        #endregion
    }
}
