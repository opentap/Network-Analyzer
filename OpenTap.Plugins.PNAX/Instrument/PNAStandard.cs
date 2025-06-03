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
    public partial class PNAX : ScpiInstrument
    {
        #region General|Standard|Sweep Type
        public string GetStandardSweepType(int Channel)
        {
            return ScpiQuery($"SENSe{Channel}:SWEep:TYPE?"); ;
        }

        private void SetSweepType(int Channel, string sweepType)
        {
            ScpiCommand($"SENSe{Channel}:SWEep:TYPE {sweepType}");
        }

        public void SetStandardSweepType(int Channel, StandardSweepTypeEnum standardSweepType)
        {
            string scpi = Scpi.Format("{0}", standardSweepType);
            SetSweepType(Channel, scpi);
        }

        // ScalerMixerSweepType
        public void SetStandardSweepType(int Channel, ScalerMixerSweepType SMCSweepType)
        {
            string scpi = Scpi.Format("{0}", SMCSweepType);
            SetSweepType(Channel, scpi);
        }

        public void SetSASweepType(int Channel, SASweepTypeEnum SASweepType)
        {
            string scpi = Scpi.Format("{0}", SASweepType);
            SetSweepType(Channel, scpi);
        }

        public double GetStart(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:FREQuency:STARt?");
        }

        public void SetStart(int Channel, double freq)
        {
            ScpiCommand($"SENSe{Channel}:FREQuency:STARt {freq}");
        }

        public double GetStop(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:FREQuency:STOP?"); ;
        }

        public void SetStop(int Channel, double freq)
        {
            ScpiCommand($"SENSe{Channel}:FREQuency:STOP {freq}");
        }

        public double GetCenter(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:FREQuency:CENTer?");
        }

        public void SetCenter(int Channel, double freq)
        {
            ScpiCommand($"SENSe{Channel}:FREQuency:CENTer {freq}");
        }

        public double GetSpan(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:FREQuency:SPAN?");
        }

        public void SetSpan(int Channel, double freq)
        {
            ScpiCommand($"SENSe{Channel}:FREQuency:SPAN {freq}");
        }

        public double GetPower(int Channel)
        {
            return ScpiQuery<double>($"SOURce{Channel}:POWer?");
        }

        public void SetPower(int Channel, double power)
        {
            ScpiCommand($"SOURce{Channel}:POWer {power}");
        }

        public int GetPoints(int Channel)
        {
            return ScpiQuery<int>($"SENSe{Channel}:SWEep:POINts?"); ;
        }

        public void SetPoints(int Channel, int points)
        {
            ScpiCommand($"SENSe{Channel}:SWEep:POINts {points}");
        }

        public double GetIFBandwidth(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:BANDwidth?"); ;
        }

        public void SetIFBandwidth(int Channel, double bw)
        {
            ScpiCommand($"SENSe{Channel}:BANDwidth {bw}");
        }

        public string GetSourcePowerMode(int Channel, String port)
        {
            return ScpiQuery($"SOURce{Channel}:POWer:MODE? \"{port}\"");
        }

        public void SetSourcePowerMode(int Channel, String port, String mode)
        {
            ScpiCommand($"SOURce{Channel}:POWer:MODE {mode}, \"{port}\"");
        }

        public double GetStartPower(int Channel)
        {
            return ScpiQuery<double>($"SOURce{Channel}:POWer:STARt?"); ;
        }

        public void SetStartPower(int Channel, double power)
        {
            ScpiCommand($"SOURce{Channel}:POWer:STARt {power}");
        }

        public double GetStopPower(int Channel)
        {
            return ScpiQuery<double>($"SOURce{Channel}:POWer:STOP?"); ;
        }

        public void SetStopPower(int Channel, double power)
        {
            ScpiCommand($"SOURce{Channel}:POWer:STOP {power}");
        }

        public double GetCWFreq(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:FREQuency:CW?"); ;
        }

        public void SetCWFreq(int Channel, double freq)
        {
            ScpiCommand($"SENSe{Channel}:FREQuency:CW {freq}");
        }

        public double GetPhaseStart(int Channel)
        {
            if (!OptionS93088)
            {
                Log.Warning("Option S93088A/B not on instrument, skipping command: " + $"SOURce{Channel}:PHASe:STARt?");
                return double.NaN;
            }
            return ScpiQuery<double>($"SOURce{Channel}:PHASe:STARt?"); ;
        }

        public void SetPhaseStart(int Channel, double phase)
        {
            if (!OptionS93088)
            {
                Log.Warning("Option S93088A/B not on instrument, skipping command: " + $"SOURce{Channel}:PHASe:STARt {phase}");
                return;
            }
            ScpiCommand($"SOURce{Channel}:PHASe:STARt {phase}");
        }

        public double GetPhaseStop(int Channel)
        {
            if (!OptionS93088)
            {
                Log.Warning("Option S93088A/B not on instrument, skipping command: " + $"SOURce{Channel}:PHASe:STOP?");
                return double.NaN;
            }
            return ScpiQuery<double>($"SOURce{Channel}:PHASe:STOP?"); ;
        }

        public void SetPhaseStop(int Channel, double phase)
        {
            if (!OptionS93088)
            {
                Log.Warning("Option S93088A/B not on instrument, skipping command: " + $"SOURce{Channel}:PHASe:STOP {phase}");
                return;
            }
            ScpiCommand($"SOURce{Channel}:PHASe:STOP {phase}");
        }
        #endregion

        #region General|Standard|Timing

        public bool GetAutoSweepTime(int Channel)
        {
            return ScpiQuery<bool>($"SENSe{Channel}:SWEep:TIME:AUTO?"); ;
        }

        public void SetAutoSweepTime(int Channel, bool auto)
        {
            string stateValue = auto ? "1" : "0";
            ScpiCommand($"SENSe{Channel}:SWEep:TIME:AUTO {stateValue}");
        }

        public double GetSweepTime(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:SWEep:TIME?"); ;
        }

        public void SetSweepTime(int Channel, double time)
        {
            ScpiCommand($"SENSe{Channel}:SWEep:TIME {time}");
        }

        public double GetDwellTime(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:SWEep:DWELl?"); ;
        }

        public void SetDwellTime(int Channel, double time)
        {
            ScpiCommand($"SENSe{Channel}:SWEep:DWELl {time}");
        }

        public double GetSweepDelay(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:SWEep:DWELl:SDELay?"); ;
        }

        public void SetSweepDelay(int Channel, double time)
        {
            ScpiCommand($"SENSe{Channel}:SWEep:DWELl:SDELay {time}");
        }

        public bool GetFastSweepMode(int Channel)
        {
            try
            {
                return ScpiQuery($"SENSe{Channel}:SWEep:SPEed?").Equals("FAST");
            }
            catch (Exception)
            {
                Log.Error("No Connection to Instrument, Fast Sweep Mode");
                return false;
            }
        }

        public void SetFastSweepMode(int Channel, bool mode)
        {
            string modeValue = mode ? "FAST" : "NORM";
            ScpiCommand($"SENSe{Channel}:SWEep:SPEed {modeValue}");
        }

        public StandardChannelSweepModeEnum GetSweepMode(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel}:SWEep:GENeration?");
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
                ScpiCommand($"SENSe{Channel}:SWEep:GENeration STEP");
            }
            else if (mode == StandardChannelSweepModeEnum.Auto)
            {
                ScpiCommand($"SENSe{Channel}:SWEep:GENeration ANAL");
            }
        }

        public StandardChannelSweepSequenceEnum GetSweepSequence(int Channel)
        {
            string retStr = ScpiQuery($"SENSe{Channel}:SWEep:GENeration:POINtsweep?");
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
            string modeValue = mode == StandardChannelSweepSequenceEnum.Standard ? "OFF" : "ON";
            ScpiCommand($"SENSe{Channel}:SWEep:GENeration:POINtsweep {modeValue}");
        }
        #endregion

        #region Segment Sweep
        public void SegmentDeleteAllSegments(int Channel)
        {
            ScpiCommand($"SENSe{Channel}:SEGMent:DELete:ALL");
        }

        public int SegmentAdd(int Channel)
        {
            int NumberOfSegments = ScpiQuery<int>($"SENSe{Channel}:SEGMent:COUNt?");
            NumberOfSegments++;
            ScpiCommand($"SENSe{Channel}:SEGMent{NumberOfSegments}:ADD");
            return NumberOfSegments;
        }

        public void SetSegmentState(int Channel, int segment, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:SEGMent{segment}:STATE {stateValue}");
        }

        public void SetSegmentNumberOfPoints(int Channel, int segment, int points)
        {
            ScpiCommand($"SENSe{Channel}:SEGMent{segment}:SWEep:POINts {points}");
        }

        public void SetSegmentStartFrequency(int Channel, int segment, double freq)
        {
            ScpiCommand($"SENSe{Channel}:SEGMent{segment}:FREQuency:STARt {freq}");
        }

        public void SetSegmentStopFrequency(int Channel, int segment, double freq)
        {
            ScpiCommand($"SENSe{Channel}:SEGMent{segment}:FREQuency:STOP {freq}");
        }

        public void SetSegmentTableShow(int Channel, bool state, int window)
        {
            // Lets review if the window exists
            int windowState = ScpiQuery<int>($"DISPlay:WINDow{window}?");
            if (windowState == 0)
            {
                // if does not exist lets enable it
                ScpiCommand($"DISPlay:WINDow{window} ON");
            }

            // Set the window state
            if (state)
            {
                ScpiCommand($"DISPlay:WINDow{window}:TABLe SEGM");
            }
            else
            {
                ScpiCommand($"DISPlay:WINDow{window}:TABLe OFF");
            }
        }

        public void SetSegmentSAMTReferenceControl(int Channel, int segment, SAOnOffTypeEnum value)
        {
            string scpi = Scpi.Format("{0}", value);
            ScpiCommand($"SENSe{Channel}:SEGMent{segment}:SA:MTReference:CONTrol {scpi}");
        }

        public void SetSegmentSAMTReference(int Channel, int segment, double value)
        {
            ScpiCommand($"SENSe{Channel}:SEGMent{segment}:SA:MTReference {value}");
        }

        public void SetSegmentSADataThresholdControl(int Channel, int segment, SAOnOffTypeEnum value)
        {
            string scpi = Scpi.Format("{0}", value);
            ScpiCommand($"SENSe{Channel}:SEGMent{segment}:SA:DTHReshold:CONTrol {scpi}");
        }

        public void SetSegmentSADataThreshold(int Channel, int segment, double value)
        {
            ScpiCommand($"SENSe{Channel}:SEGMent{segment}:SA:DTHReshold {value}");
        }

        public void SetSegmentSAVectorAverageControl(int Channel, int segment, SAOnOffTypeEnum value)
        {
            string scpi = Scpi.Format("{0}", value);
            ScpiCommand($"SENSe{Channel}:SEGMent{segment}:SA:VAVerage:CONTrol {scpi}");
        }

        public void SetSegmentSAVectorAverage(int Channel, int segment, int value)
        {
            ScpiCommand($"SENSe{Channel}:SEGMent{segment}:SA:VAVerage {value}");
        }

        public void SetSegmentSAVideoBWControl(int Channel, int segment, SAOnOffTypeEnum value)
        {
            string scpi = Scpi.Format("{0}", value);
            ScpiCommand($"SENSe{Channel}:SEGMent{segment}:SA:VIDeobw:CONTrol {scpi}");
        }

        public void SetSegmentSAVideoBW(int Channel, int segment, double value)
        {
            ScpiCommand($"SENSe{Channel}:SEGMent{segment}:SA:VIDeobw {value}");
        }

        public void SetSegmentValues(SegmentDefinitionTypeEnum SegmentType, int Channel, List<SegmentDefinition> SegmentDefinitions, bool ShowTable)
        {
            if (SegmentType == SegmentDefinitionTypeEnum.File)
            {
                Log.Error("Load file Not implemented!");
            }
            else
            {
                SegmentDeleteAllSegments(Channel);
                foreach (SegmentDefinition a in SegmentDefinitions)
                {
                    int segment = SegmentAdd(Channel);
                    SetSegmentState(Channel, segment, a.state);
                    SetSegmentNumberOfPoints(Channel, segment, a.NumberOfPoints);
                    SetSegmentStartFrequency(Channel, segment, a.StartFrequency);
                    SetSegmentStopFrequency(Channel, segment, a.StopFrequency);
                }
                SetStandardSweepType(Channel, StandardSweepTypeEnum.SegmentSweep);
                if (ShowTable)
                {
                    SetSegmentTableShow(Channel, true, 1);
                }
            }

        }

        #endregion

        #region Configuration
        public ReceiverConfigurationEnumType GetReceiverConfiguration(int Channel)
        {
            ReceiverConfigurationEnumType retVal = ReceiverConfigurationEnumType.INT;
            string retStr = ScpiQuery($"SENSe{Channel}:IMD:TPOWer:LEVel?");
            if (retStr.Equals("INT"))
            {
                retVal = ReceiverConfigurationEnumType.INT;
            }
            else if (retStr.Equals("EXT"))
            {
                retVal = ReceiverConfigurationEnumType.EXT;
            }
            else
            {
                throw new Exception("Undefined Receiver Configuration mode");
            }
            return retVal;
        }


        public void SetReceiverConfiguration(int Channel, ReceiverConfigurationEnumType rec)
        {
            string path = Scpi.Format("{0}", rec);
            ScpiCommand($"SENSe{Channel}:IMD:RECeiver:CONFig:COMBiner:PATH {path}");
        }

        #endregion
    }
}
