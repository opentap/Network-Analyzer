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
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:FREQuency:STARt?");
            return retVal;
        }

        public void SetStart(int Channel, double freq)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:FREQuency:STARt {freq.ToString()}");
        }

        public double GetStop(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:FREQuency:STOP?");
            return retVal;
        }

        public void SetStop(int Channel, double freq)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:FREQuency:STOP {freq.ToString()}");
        }

        public double GetPower(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SOURce{Channel.ToString()}:POWer?");
            return retVal;
        }

        public void SetPower(int Channel, double power)
        {
            ScpiCommand($"SOURce{Channel.ToString()}:POWer {power.ToString()}");
        }

        public int GetPoints(int Channel)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENSe{Channel.ToString()}:SWEep:POINts?");
            return retVal;
        }

        public void SetPoints(int Channel, int points)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:SWEep:POINts {points.ToString()}");
        }

        public double GetIFBandwidth(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:BANDwidth?");
            return retVal;
        }

        public void SetIFBandwidth(int Channel, double bw)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:BANDwidth {bw.ToString()}");
        }

        public double GetStartPower(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SOURce{Channel.ToString()}:POWer:STARt?");
            return retVal;
        }

        public void SetStartPower(int Channel, double power)
        {
            ScpiCommand($"SOURce{Channel.ToString()}:POWer:STARt {power.ToString()}");
        }

        public double GetStopPower(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SOURce{Channel.ToString()}:POWer:STOP?");
            return retVal;
        }

        public void SetStopPower(int Channel, double power)
        {
            ScpiCommand($"SOURce{Channel.ToString()}:POWer:STOP {power.ToString()}");
        }

        public double GetCWFreq(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:FREQuency:CW?");
            return retVal;
        }

        public void SetCWFreq(int Channel, double freq)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:FREQuency:CW {freq.ToString()}");
        }

        public double GetPhaseStart(int Channel)
        {
            // 
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SOURce{Channel.ToString()}:PHASe:STARt?");
            return retVal;
        }

        public void SetPhaseStart(int Channel, double phase)
        {
            ScpiCommand($"SOURce{Channel.ToString()}:PHASe:STARt {phase.ToString()}");
        }

        public double GetPhaseStop(int Channel)
        {
            // 
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SOURce{Channel.ToString()}:PHASe:STOP?");
            return retVal;
        }

        public void SetPhaseStop(int Channel, double phase)
        {
            ScpiCommand($"SOURce{Channel.ToString()}:PHASe:STOP {phase.ToString()}");
        }
        #endregion

        #region General|Standard|Timing

        public bool GetAutoSweepTime(int Channel)
        {
            bool retVal = false;
            retVal = ScpiQuery<bool>($"SENSe{Channel.ToString()}:SWEep:TIME:AUTO?");
            return retVal;
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
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:SWEep:TIME?");
            return retVal;
        }

        public void SetSweepTime(int Channel, double time)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:SWEep:TIME {time.ToString()}");
        }

        public double GetDwellTime(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:SWEep:DWELl?");
            return retVal;
        }

        public void SetDwellTime(int Channel, double time)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:SWEep:DWELl {time.ToString()}");
        }

        public double GetSweepDelay(int Channel)
        {
            double retVal = double.NaN;
            retVal = ScpiQuery<double>($"SENSe{Channel.ToString()}:SWEep:DWELl:SDELay?");
            return retVal;
        }

        public void SetSweepDelay(int Channel, double time)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:SWEep:DWELl:SDELay {time.ToString()}");
        }

        public bool GetFastSweepMode(int Channel)
        {
            bool retVal = false;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:SWEep:SPEed?");
            if (retStr.Equals("FAST"))
            {
                retVal = true;
            }
            else
            {
                retVal = false;
            }
            return retVal;
        }

        public void SetFastSweepMode(int Channel, bool mode)
        {
            if (mode == true)
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
            StandardChannelSweepModeEnum retVal = StandardChannelSweepModeEnum.Auto;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:SWEep:GENeration?");
            if (retStr.Equals("STEP"))
            {
                retVal = StandardChannelSweepModeEnum.Stepped;
            }
            else if (retStr.Equals("ANAL"))
            {
                retVal = StandardChannelSweepModeEnum.Auto;
            }
            else
            {
                throw new Exception("unknown sweep mode");
            }
            return retVal;
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
            else
            {
                throw new Exception("unknown sweep mode");
            }
        }

        public StandardChannelSweepSequenceEnum GetSweepSequence(int Channel)
        {
            StandardChannelSweepSequenceEnum retVal = StandardChannelSweepSequenceEnum.Standard;
            String retStr = ScpiQuery($"SENSe{Channel.ToString()}:SWEep:GENeration:POINtsweep?");
            if (retStr.Equals("OFF"))
            {
                retVal = StandardChannelSweepSequenceEnum.Standard;
            }
            else if (retStr.Equals("ON"))
            {
                retVal = StandardChannelSweepSequenceEnum.PointSweep;
            }
            else
            {
                throw new Exception("unknown sweep sequence");
            }
            return retVal;
        }

        public void SetSweepSequence(int Channel, StandardChannelSweepSequenceEnum mode)
        {
            if (mode == StandardChannelSweepSequenceEnum.Standard)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:SWEep:GENeration:POINtsweep OFF");
            }
            else if (mode == StandardChannelSweepSequenceEnum.PointSweep)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:SWEep:GENeration:POINtsweep ON");
            }
            else
            {
                throw new Exception("unknown sweep sequence");
            }
        }
        #endregion
    }
}
