using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;

//Note this template assumes that you have a SCPI based instrument, and accordingly
//extends the ScpiInstrument base class.

//If you do NOT have a SCPI based instrument, you should modify this instance to extend
//the (less powerful) Instrument base class.

namespace OpenTap.Plugins.PNAX
{
    public enum ReceiverLevelingTypeEnum
    {
        [Scpi("PRESweep")]
        [Display("PRESweep")]
        Presweep = 0,
        [Scpi("POINt")]
        [Display("POINt")]
        Point = 1
    }

    public enum ReceiverLevelingFTypeEnum
    {
        [Scpi("AUTO")]
        [Display("Auto")]
        Auto = 0,
        [Scpi("INPut")]
        [Display("Input")]
        Input = 0,
        [Scpi("OUTPut")]
        [Display("Output")]
        Output = 0,
        [Scpi("RECeiver")]
        [Display("Receiver")]
        Receiver = 0,
        [Scpi("SOURce")]
        [Display("Source")]
        Source = 0
    }


    public partial class PNAX : ScpiInstrument
    {
        private void SetReferenceReceiver(int Channel, String port, String refRec)
        {
            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:REFerence \"{refRec}\", \"{port}\"");
        }
        
        private void ReceiverLevelingState(int Channel, String port, bool state)
        {
            String StateStr = "OFF";
            if (state)
            {
                StateStr = "ON";
            }

            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:STATe {StateStr}, \"{port}\"");
        }

        private void ReceiverLevelingType(int Channel, string port, ReceiverLevelingTypeEnum receiverLevelingType)
        {
            string LevelingType = Scpi.Format("{0}", receiverLevelingType);

            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:ACQuisition:MODE {LevelingType}, \"{port}\"");
        }

        private void ReceiverLevelingMaxPower(int Channel, string port, double maxpower)
        {
            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:SAFE:MAX {maxpower}, \"{port}\"");
        }

        private void ReceiverLevelingMinPower(int Channel, string port, double minpower)
        {
            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:SAFE:MIN {minpower}, \"{port}\"");
        }

        private void ReceiverLevelingEnableSafeMode(int Channel, String port, bool state)
        {
            String StateStr = "OFF";
            if (state)
            {
                StateStr = "ON";
            }

            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:SAFE:STATe {StateStr}, \"{port}\"");
        }

        private void ReceiverLevelingSafeModeStepPowerLevel(int Channel, string port, double value)
        {
            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:SAFE:STEP {value}, \"{port}\"");
        }

        private void ReceiverLevelingUpdateSourcePowerCal(int Channel, string port, bool state)
        {
            String StateStr = "OFF";
            if (state)
            {
                StateStr = "ON";
            }

            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:LSPC {StateStr}, \"{port}\"");
        }

        private void ReceiverLevelingSourceALC(int Channel, string port, SourceLevelingModeType sourceLeveling)
        {
            string LevelingType = Scpi.Format("{0}", sourceLeveling);

            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE {LevelingType}, \"{port}\"");
        }

        private void ReceiverLevelingTolerance(int Channel, string port, double value)
        {
            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:TOLerance {value}, \"{port}\"");
        }

        private void ReceiverLevelingMaxIterations(int Channel, string port, int value)
        {
            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:ITERation:VALue {value}, \"{port}\"");
        }

        private void ReceiverLevelingMaxIterationsEnable(int Channel, string port, bool state)
        {
            String StateStr = "OFF";
            if (state)
            {
                StateStr = "ON";
            }

            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:ITERation:ENABle {StateStr}, \"{port}\"");
        }
        
        private void ReceiverLevelingFrequency(int Channel, string port, ReceiverLevelingFTypeEnum ftype)
        {
            string FType = Scpi.Format("{0}", ftype);

            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:FTYPe {FType}, \"{port}\"");
        }

        private void ReceiverLevelingIFBW(int Channel, string port, double value)
        {
            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:IFBW {value}, \"{port}\"");
        }
    }
}
