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
        public ConverterStagesEnum GetConverterStages(int Channel)
        {
            ConverterStagesEnum retVal;
            int retInt = ScpiQuery<int>($"SENSe{Channel.ToString()}:MIXer:STAGe?");
            if (retInt == 1)
            {
                retVal = ConverterStagesEnum._1;
            }
            else
            {
                retVal = ConverterStagesEnum._2;
            }
            return retVal;
        }

        public void SetConverterStages(int Channel, ConverterStagesEnum stages)
        {
            if (stages == ConverterStagesEnum._1)
            {
                ScpiCommand($"SENSe{Channel.ToString()}:MIXer:STAGe 1");
            }
            else
            {
                ScpiCommand($"SENSe{Channel.ToString()}:MIXer:STAGe 2");
            }
        }

        public int GetPortInput(int Channel)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENS{Channel.ToString()}:MIXer:PMAP:INP?");
            return retVal;
        }

        public int GetPortOutput(int Channel)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENS{Channel.ToString()}:MIXer:PMAP:OUTP?");
            return retVal;
        }

        public void SetPortInputOutput(int Channel, PortsEnum inport, PortsEnum outport)
        {
            String inp = Scpi.Format("{0}", inport);
            String outp = Scpi.Format("{0}", outport);
            ScpiCommand($"SENS{Channel.ToString()}:MIXer:PMAP {inp},{outp}");
        }

        public int GetInputFractionalMultiplierNumerator(int Channel)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENSe{Channel.ToString()}:MIXer:INPut:FREQ:NUMerator?");
            return retVal;
        }

        public void SetInputFractionalMultiplierNumerator(int Channel, int value)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:MIXer:INPut:FREQ:NUMerator {value.ToString()}");
        }

        public int GetInputFractionalMultiplierDenominator(int Channel)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENSe{Channel.ToString()}:MIXer:INPut:FREQ:DENominator?");
            return retVal;
        }

        public void SetInputFractionalMultiplierDenominator(int Channel, int value)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:MIXer:INPut:FREQ:DENominator {value.ToString()}");
        }

        public int GetLOFractionalMultiplierNumerator(int Channel, int Stage)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENSe{Channel.ToString()}:MIXer:LO{Stage.ToString()}:FREQuency:NUMerator?");
            return retVal;
        }

        public void SetLOFractionalMultiplierNumerator(int Channel, int Stage, int value)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:MIXer:LO{Stage.ToString()}:FREQuency:NUMerator {value.ToString()}");
        }

        public int GetLOFractionalMultiplierDenominator(int Channel, int Stage)
        {
            int retVal;
            retVal = ScpiQuery<int>($"SENSe{Channel.ToString()}:MIXer:LO{Stage.ToString()}:FREQuency:DENominator?");
            return retVal;
        }

        public void SetLOFractionalMultiplierDenominator(int Channel, int Stage, int value)
        {
            ScpiCommand($"SENSe{Channel.ToString()}:MIXer:LO{Stage.ToString()}:FREQuency:DENominator {value.ToString()}");
        }

        // SENSe<ch>:MIXer:LO<n>:NAME?
        public String GetPortLO(int Channel, int Stage)
        {
            String retVal;
            retVal = ScpiQuery($"SENSe{Channel.ToString()}:MIXer:LO{Stage.ToString()}:NAME?");
            return retVal;
        }

        public void SetPortLO(int Channel, int Stage, LOEnum value)
        {
            String str = Scpi.Format("{0}", value);
            ScpiCommand($"SENSe{Channel.ToString()}:MIXer:LO{Stage.ToString()}:NAME \"{str}\"");
        }

        public bool GetEnableEmbeddedLO(int Channel)
        {
            bool retVal;
            retVal = ScpiQuery<bool>($"SENS{Channel.ToString()}:MIXer:ELO:STATe?");
            return retVal;
        }

        public void SetEnableEmbeddedLO(int Channel, bool value)
        {
            if (value == true)
            {.
                ScpiCommand($"SENS{Channel.ToString()}:MIXer:ELO:STATe 1");
            }
            else
            {
                ScpiCommand($"SENS{Channel.ToString()}:MIXer:ELO:STATe 0");
            }
        }

        // SENSe<ch>:MIXer:ELO:TUNing:MODE?
        public TuningMethodEnum GetTuningMethod(int Channel)
        {
            TuningMethodEnum retVal = TuningMethodEnum.BroadbandAndPrecise;
            string retStr = ScpiQuery($"SENS{Channel.ToString()}:MIXer:ELO:TUNing:MODE?");
            if (retStr.Equals("BRO"))
            {
                retVal = TuningMethodEnum.BroadbandAndPrecise;
            }
            else if (retStr.Equals("PREC"))
            {
                retVal = TuningMethodEnum.PreciseOnly;
            }
            else if (retStr.Equals("NON"))
            {
                retVal = TuningMethodEnum.DisableTuning;
            }
            else
            {
                throw new Exception("unknown tuning method");
            }
            return retVal;
        }

        public void SetTuningMethod(int Channel, TuningMethodEnum tuning)
        {
            if (tuning == TuningMethodEnum.BroadbandAndPrecise)
            {
                ScpiCommand($"SENS{Channel.ToString()}:MIXer:ELO:TUNing:MODE BROadband");
            }
            else if (tuning == TuningMethodEnum.PreciseOnly)
            {
                ScpiCommand($"SENS{Channel.ToString()}:MIXer:ELO:TUNing:MODE PRECise");
            }
            else if (tuning == TuningMethodEnum.DisableTuning)
            {
                ScpiCommand($"SENS{Channel.ToString()}:MIXer:ELO:TUNing:MODE NONe");
            }
            else
            {
                throw new Exception("unknown tuning method");
            }
        }

        // SENSe<ch>:MIXer:ELO:NORMalize:POINt?
        public int GetTuningPoint(int Channel)
        {
            int retVal = 0;
            retVal = ScpiQuery<int>($"SENS{Channel.ToString()}:MIXer:ELO:NORMalize:POINt?");
            return retVal;
        }

        public void SetTuningPoint(int Channel, int point)
        {
            ScpiCommand($"SENS{Channel.ToString()}:MIXer:ELO:NORMalize:POINt {point.ToString()}");
        }

        // PNA UI shows "Tune every" for this parameter
        public int GetTuningInterval(int Channel)
        {
            int retVal = 0;
            retVal = ScpiQuery<int>($"SENS{Channel.ToString()}:MIXer:ELO:TUNing:INTerval?");
            return retVal;
        }

        public void SetTuningInterval(int Channel, int interval)
        {
            ScpiCommand($"SENS{Channel.ToString()}:MIXer:ELO:TUNing:INTerval {interval.ToString()}");
        }

        // PNA UI shows "Broadband Search" for this parameter
        public int GetTuningSpan(int Channel)
        {
            int retVal = 0;
            retVal = ScpiQuery<int>($"SENS{Channel.ToString()}:MIXer:ELO:TUNing:SPAN?");
            return retVal;
        }

        public void SetTuningSpan(int Channel, int span)
        {
            ScpiCommand($"SENS{Channel.ToString()}:MIXer:ELO:TUNing:SPAN {span.ToString()}");
        }

        // PNA UI shows "IFBW" for this parameter
        public int GetTuningIFBW(int Channel)
        {
            int retVal = 0;
            retVal = ScpiQuery<int>($"SENS{Channel.ToString()}:MIXer:ELO:TUNing:IFBW?");
            return retVal;
        }

        public void SetTuningIFBW(int Channel, int value)
        {
            ScpiCommand($"SENS{Channel.ToString()}:MIXer:ELO:TUNing:IFBW {value.ToString()}");
        }

        // PNA UI shows "Max Iterations" for this parameter
        public int GetTuningMaxIterations(int Channel)
        {
            int retVal = 0;
            retVal = ScpiQuery<int>($"SENS{Channel.ToString()}:MIXer:ELO:TUNing:ITERations?");
            return retVal;
        }

        public void SetTuningMaxIterations(int Channel, int value)
        {
            ScpiCommand($"SENS{Channel.ToString()}:MIXer:ELO:TUNing:ITERations {value.ToString()}");
        }

        // PNA UI shows "Tolerance" for this parameter
        public int GetTuningTolerance(int Channel)
        {
            int retVal = 0;
            retVal = ScpiQuery<int>($"SENS{Channel.ToString()}:MIXer:ELO:TUNing:TOLerance?");
            return retVal;
        }

        public void SetTuningTolerance(int Channel, int value)
        {
            ScpiCommand($"SENS{Channel.ToString()}:MIXer:ELO:TUNing:TOLerance {value.ToString()}");
        }

        // PNA UI shows "LO Frequency Delta" for this parameter
        public double GetLOFrequencyDelta(int Channel)
        {
            int retVal = 0;
            retVal = ScpiQuery<int>($"SENS{Channel.ToString()}:MIXer:ELO:LO:DELTa?");
            return retVal;
        }

        public void SetLOFrequencyDelta(int Channel, double value)
        {
            ScpiCommand($"SENS{Channel.ToString()}:MIXer:ELO:LO:DELTa {value.ToString()}");
        }
    }
}
