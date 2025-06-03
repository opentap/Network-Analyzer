using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

//Note this template assumes that you have a SCPI based instrument, and accordingly
//extends the ScpiInstrument base class.

//If you do NOT have a SCPI based instrument, you should modify this instance to extend
//the (less powerful) Instrument base class.

namespace OpenTap.Plugins.PNAX
{
    public enum PulseModeEnumtype
    {
        [Display("Off")]
        [Scpi("OFF")]
        Off,
        [Display("Standard Pulse")]
        [Scpi("STD")]
        Standard,
        [Display("Pulse Profile")]
        [Scpi("PROFILE")]
        Profile
    }

    public enum PulseModeBasicEnumtype
    {
        [Display("Off")]
        [Scpi("OFF")]
        Off,
        [Display("Standard Pulse")]
        [Scpi("STD")]
        Standard,
    }

    public enum PulseDetectionMethodEnumtype
    {
        [Display("Narrowband")]
        [Scpi("OFF")]
        Narrowband,
        [Display("Wideband")]
        [Scpi("ON")]
        Wideband
    }

    public enum PulsePrimaryClockEnumtype
    {
        [Display("Internal")]
        [Scpi("Internal")]
        Internal,
        [Display("External")]
        [Scpi("External")]
        External
    }

    public enum PulseTriggerEnumtype
    {
        [Display("Internal")]
        [Scpi("Internal")]
        Internal,
        [Display("External")]
        [Scpi("External")]
        External
    }

    public enum Pulse4ModeEnumtype
    {
        [Display("All ADC Activity")]
        [Scpi("ALL")]
        All,
        [Display("Trace ADC Activity")]
        [Scpi("TRAC")]
        Trace
    }


    public enum PulseTriggerTypeEnumtype
    {
        [Display("Edge")]
        [Scpi("EDGE")]
        Edge,
        [Display("Level")]
        [Scpi("LEV")]
        Level
    }

    public enum PulseTriggerPolarityEnumtype
    {
        [Display("Positive/High")]
        [Scpi("POS")]
        Positive,
        [Display("Negative/Low")]
        [Scpi("NEG")]
        Negative
    }

    public enum PulseTriggerLevelEdgeEnumtype
    {
        [Display("High Level")]
        HighLevel,
        [Display("Low Level")]
        LowLevel,
        [Display("Positive Edge")]
        PositiveEdge,
        [Display("Negative Edge")]
        NegativeEdge
    }

    public enum PulseALCModeEnumType
    {
        [Scpi("INT")]
        Internal,
        [Scpi("OPEN")]
        OpenLoop
    }


    public partial class PNAX : ScpiInstrument
    {
        public void PulseConfigElement(int Channel, string element, string value)
        {
            ScpiCommand($"SENSe{Channel}:PATH:CONFig:ELEMent:STATe \"{element}\",\"{value}\"");
        }

        #region Pulse Measurement
        public void PulseMode(int Channel, PulseModeEnumtype mode)
        {
            string modeValue = Scpi.Format("{0}", mode);
            ScpiCommand($"SENSe{Channel}:SWEep:PULSe:MODE {modeValue}");
        }

        public void PulseMode(int Channel, PulseModeBasicEnumtype mode)
        {
            string modeValue = Scpi.Format("{0}", mode);
            ScpiCommand($"SENSe{Channel}:SWEep:PULSe:MODE {modeValue}");
        }
        #endregion

        #region Pulse Timing
        public void PulsePrimaryWidth(int Channel, double value)
        {
            ScpiCommand($"SENSe{Channel}:SWEep:PULSe:PRIMary:WIDTh {value}");
        }

        public void PulsePrimaryPeriod(int Channel, double value)
        {
            ScpiCommand($"SENSe{Channel}:SWEep:PULSe:PRIMary:PERiod {value}");
        }

        public void PulsePrimaryFrequency(int Channel, double value)
        {
            ScpiCommand($"SENSe{Channel}:SWEep:PULSe:PRIMary:FREQuency {value}");
        }
        #endregion

        #region Properties
        public void PulseDetectionMethodAuto(int Channel, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:SWEep:PULSe:DETectmode:AUTO {stateValue}");
        }

        public void PulseDetectionMethod(int Channel, PulseDetectionMethodEnumtype state)
        {
            string stateValue = Scpi.Format("{0}", state);
            ScpiCommand($"SENSe{Channel}:SWEep:PULSe:WIDeband:STATe {stateValue}");
        }

        public void PulseDetectionSWGating(int Channel, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:SWEep:PULSe:SWGate:STATe {stateValue}");
        }

        public void PulseIFGainAuto(int Channel, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:SWEep:PULSe:IFGain:AUTO {stateValue}");
        }

        public void PulseIFPath(int Channel, string receiver, string value)
        {
            PulseConfigElement(Channel, $"IFGAIN{receiver}", value);
        }

        public void PulseOptimizePRF(int Channel, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:SWEep:PULSe:PRF:AUTO {stateValue}");
        }

        public void PulseProfileSweepTimeAuto(int Channel, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:SWEep:PULSe:CWTime:AUTO {stateValue}");
        }

        public void PulseSweepTime(int Channel, double value)
        {
            ScpiCommand($"SENSe{Channel}:SWEep:TIME:STOP {value}");
        }
        public double PulseSweepTimeQ(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:SWEep:TIME:STOP?");
        }

        public double PulseSweepTimeStart(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:SWEep:TIME:STARt?");
        }

        public void PulseNumberOfPoints(int Channel, int value)
        {
            ScpiCommand($"SENSe{Channel}:SWEep:POINts {value}");
        }

        public void PulseIFBW(int Channel, double value)
        {
            SetIFBandwidth(Channel, value);
        }
        #endregion

        #region Measurement Timing
        public void PulsePrimaryClock(int Channel, PulsePrimaryClockEnumtype value)
        {
            string clockValue = Scpi.Format("{0}", value);
            ScpiCommand($"SENSe{Channel}:SWEep:PULSe:PRIMary:CLOCk \"{clockValue}\"");
        }

        public void PulseWidthAndDelayAuto(int Channel, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:SWEep:PULSe:TIMing:AUTO {stateValue}");
        }

        public void PulseGeneratorsAutoselect(int Channel, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:SWEep:PULSe:DRIVe:AUTO {stateValue}");
        }

        // Modulator Drive in Pulse Generators Dialog
        public void PulseGeneratorPulseGen(int Channel, string pulseGen)
        {
            PulseConfigElement(Channel, "PulseModDrive", pulseGen);
        }

        #endregion

        #region Pulse Generators
        public void PulseGeneratorWidth(int Channel, string pulseGen, double value)
        {
            ScpiCommand($"SENSe{Channel}:PULSe:WIDTh {value},\"{pulseGen}\"");
        }

        public void PulseGeneratorDelay(int Channel, string pulseGen, double value)
        {
            ScpiCommand($"SENSe{Channel}:PULSe:DELay {value},\"{pulseGen}\"");
        }

        public void PulseGeneratorInvert(int Channel, string pulseGen, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:PULSe:INVert {stateValue},\"{pulseGen}\"");
        }

        public void PulseGeneratorEnable(int Channel, string pulseGen, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:PULSe:STATe {stateValue},\"{pulseGen}\"");
        }

        public void PulseGeneratorTrigger(int Channel, PulseTriggerEnumtype value)
        {
            string triggerValue = Scpi.Format("{0}", value);
            PulseConfigElement(Channel, "PulseTrigInput", triggerValue);
        }

        #endregion

        #region Pulsed Sources
        public void PulseGeneratorSource1EnableModulator(int Channel, bool state)
        {
            string stateValue = state ? "Enable" : "Disable";
            PulseConfigElement(Channel, "Src1Out1PulseModEnable", stateValue);
        }

        public void PulseGeneratorSource2EnableModulator(int Channel, bool state)
        {
            string stateValue = state ? "Enable" : "Disable";
            PulseConfigElement(Channel, "Src2Out1PulseModEnable", stateValue);
        }

        public void PulseGeneratorALCOpenLoop(int Channel, bool pulseALCMode)
        {
            if (pulseALCMode)
            {
                PulseGeneratorALCOpenLoop(Channel, PulseALCModeEnumType.OpenLoop);
            }
            else
            {
                PulseGeneratorALCOpenLoop(Channel, PulseALCModeEnumType.Internal);
            }
        }

        public void PulseGeneratorALCOpenLoop(int Channel, PulseALCModeEnumType pulseALCMode)
        {
            string ALCMode = Scpi.Format("{0}", pulseALCMode);
            SetSourceLevelingMode(Channel, 1, ALCMode);
        }

        public void PulseGeneratorADCDelay(int Channel, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:PULSe:HDELay:STATe {stateValue}");
        }

        public void PulseGeneratorModulatorDelay(int Channel, double value)
        {
            ScpiCommand($"SENSe{Channel}:PULSe:HDELay:MODulator {value}");
        }

        public double PulseGeneratorFixedADCDelay(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:PULSe:HDELay:ADC?");
        }
        #endregion

        #region Pulsed Receivers
        public void PulseGeneratorSyncADCs(int Channel, bool state)
        {
            PulseGeneratorEnable(Channel, "Pulse0", state);
        }

        public void Pulse4Option(int Channel, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:PULSe4:OPTion {stateValue}");
        }

        public void Pulse4Mode(int Channel, Pulse4ModeEnumtype mode)
        {
            string modeValue = Scpi.Format("{0}", mode);
            ScpiCommand($"SENSe{Channel}:PULSe4:MODE {modeValue}");
        }
        #endregion

        #region Pulse Trigger
        public void PulseTriggerType(int Channel, PulseTriggerTypeEnumtype mode)
        {
            string modeValue = Scpi.Format("{0}", mode);
            ScpiCommand($"SENSe{Channel}:PULSe:TTYPe {modeValue}");
        }

        public void PulseTriggerPolarity(int Channel, PulseTriggerPolarityEnumtype mode)
        {
            string modeValue = Scpi.Format("{0}", mode);
            ScpiCommand($"SENSe{Channel}:PULSe:TPOLarity {modeValue}");
        }
        #endregion
    }
}
