using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using OpenTap;

//Note this template assumes that you have a SCPI based instrument, and accordingly
//extends the ScpiInstrument base class.

//If you do NOT have a SCPI based instrument, you should modify this instance to extend
//the (less powerful) Instrument base class.

namespace OpenTap.Plugins.PNAX
{
    public enum ReceiverLevelingTypeEnum
    {
        [Scpi("PRESweep")]
        [Display("Pre-Sweep")]
        Presweep = 0,

        [Scpi("POINt")]
        [Display("Point")]
        Point = 1,
    }

    public enum ReceiverLevelingFTypeEnum
    {
        [Scpi("AUTO")]
        [Display("Auto")]
        Auto = 0,

        [Scpi("INPut")]
        [Display("Input")]
        Input = 1,

        [Scpi("OUTPut")]
        [Display("Output")]
        Output = 2,

        [Scpi("RECeiver")]
        [Display("Receiver")]
        Receiver = 3,

        [Scpi("SOURce")]
        [Display("Source")]
        Source = 4,
    }

    public enum ReceiverLevelingIFBWEnum
    {
        [Scpi("1")]
        [Display("1 Hz")]
        IFBW_1 = 1,

        [Scpi("2")]
        [Display("2 Hz")]
        IFBW_2 = 2,

        [Scpi("5")]
        [Display("5 Hz")]
        IFBW_5 = 5,

        [Scpi("10")]
        [Display("10 Hz")]
        IFBW_10 = 10,

        [Scpi("20")]
        [Display("20 Hz")]
        IFBW_20 = 20,

        [Scpi("50")]
        [Display("50 Hz")]
        IFBW_50 = 50,

        [Scpi("100")]
        [Display("100 Hz")]
        IFBW_100 = 100,

        [Scpi("200")]
        [Display("200 Hz")]
        IFBW_200 = 200,

        [Scpi("500")]
        [Display("500 Hz")]
        IFBW_500 = 500,

        [Scpi("1e3")]
        [Display("1 kHz")]
        IFBW_1k = 1000,

        [Scpi("2e3")]
        [Display("2 kHz")]
        IFBW_2k = 2000,

        [Scpi("5e3")]
        [Display("5 kHz")]
        IFBW_5k = 5000,

        [Scpi("10e3")]
        [Display("10 kHz")]
        IFBW_10k = 10000,

        [Scpi("20e3")]
        [Display("20 kHz")]
        IFBW_20k = 20000,

        [Scpi("50e3")]
        [Display("50 kHz")]
        IFBW_50k = 50000,

        [Scpi("100e3")]
        [Display("100 kHz")]
        IFBW_100k = 100000,

        [Scpi("200e3")]
        [Display("200 kHz")]
        IFBW_200k = 200000,

        [Scpi("600e3")]
        [Display("600 kHz")]
        IFBW_600k = 600000,

        [Scpi("1000e3")]
        [Display("1000 kHz")]
        IFBW_1000k = 1000000,

        [Scpi("2000e3")]
        [Display("2000 kHz")]
        IFBW_2000k = 2000000,

        [Scpi("5000e3")]
        [Display("5000 kHz")]
        IFBW_5000k = 5000000,
    }

    public partial class PNAX : ScpiInstrument
    {
        public void SetReferenceReceiver(int Channel, String port, String refRec)
        {
            ScpiCommand(
                $"SOURce{Channel}:POWer:ALC:MODE:RECeiver:REFerence \"{refRec}\", \"{port}\""
            );
        }

        public void ReceiverLevelingState(int Channel, String port, bool state)
        {
            String StateStr = "OFF";
            if (state)
            {
                StateStr = "ON";
            }

            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:STATe {StateStr}, \"{port}\"");
        }

        public void ReceiverLevelingType(
            int Channel,
            string port,
            ReceiverLevelingTypeEnum receiverLevelingType
        )
        {
            string LevelingType = Scpi.Format("{0}", receiverLevelingType);

            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:ACQuisition:MODE {LevelingType}");
        }

        public void ReceiverLevelingMaxPower(int Channel, string port, double maxpower)
        {
            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:SAFE:MAX {maxpower}, \"{port}\"");
        }

        public void ReceiverLevelingMinPower(int Channel, string port, double minpower)
        {
            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:SAFE:MIN {minpower}, \"{port}\"");
        }

        public void ReceiverLevelingEnableSafeMode(int Channel, String port, bool state)
        {
            String StateStr = "OFF";
            if (state)
            {
                StateStr = "ON";
            }

            ScpiCommand(
                $"SOURce{Channel}:POWer:ALC:MODE:RECeiver:SAFE:STATe {StateStr}, \"{port}\""
            );
        }

        public void ReceiverLevelingSafeModeStepPowerLevel(int Channel, string port, double value)
        {
            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:SAFE:STEP {value}, \"{port}\"");
        }

        public void ReceiverLevelingUpdateSourcePowerCal(int Channel, string port, bool state)
        {
            String StateStr = "OFF";
            if (state)
            {
                StateStr = "ON";
            }

            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:LSPC {StateStr}, \"{port}\"");
        }

        public void ReceiverLevelingSourceALC(
            int Channel,
            string port,
            SourceLevelingModeType sourceLeveling
        )
        {
            string LevelingType = Scpi.Format("{0}", sourceLeveling);

            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE {LevelingType}, \"{port}\"");
        }

        public void ReceiverLevelingTolerance(int Channel, string port, double value)
        {
            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:TOLerance {value}, \"{port}\"");
        }

        public void ReceiverLevelingMaxIterations(int Channel, string port, int value)
        {
            ScpiCommand(
                $"SOURce{Channel}:POWer:ALC:MODE:RECeiver:ITERation:VALue {value}, \"{port}\""
            );
        }

        public void ReceiverLevelingMaxIterationsEnable(int Channel, string port, bool state)
        {
            String StateStr = "OFF";
            if (state)
            {
                StateStr = "ON";
            }

            ScpiCommand(
                $"SOURce{Channel}:POWer:ALC:MODE:RECeiver:ITERation:ENABle {StateStr}, \"{port}\""
            );
        }

        public void ReceiverLevelingFrequency(
            int Channel,
            string port,
            ReceiverLevelingFTypeEnum ftype
        )
        {
            string FType = Scpi.Format("{0}", ftype);

            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:FTYPe {FType}, \"{port}\"");
        }

        public void ReceiverLevelingIFBW(int Channel, string port, bool state)
        {
            String StateStr = "OFF";
            if (state)
            {
                StateStr = "ON";
            }

            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:FAST {StateStr}, \"{port}\"");
        }

        public void ReceiverIFBW(int Channel, string port, double value)
        {
            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:IFBW {value}, \"{port}\"");
        }

        public void ReceiverIFBW(int Channel, string port, ReceiverLevelingIFBWEnum ifbw)
        {
            string value = Scpi.Format("{0}", ifbw);

            ScpiCommand($"SOURce{Channel}:POWer:ALC:MODE:RECeiver:IFBW {value}, \"{port}\"");
        }
    }
}
