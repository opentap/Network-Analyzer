using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using OpenTap;

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
            string retStr = ScpiQuery($"SENSe{Channel}:FOM:STATe?");
            return !retStr.Equals("0");
        }

        public void SetFOMState(int Channel, bool mode)
        {
            string stateValue = mode ? "1" : "0";
            ScpiCommand($"SENSe{Channel}:FOM:STATe {stateValue}");
        }

        public bool GetFOMMode(int Channel, int Range)
        {
            string retStr = ScpiQuery($"SENSe{Channel}:FOM:RANGe{Range}:COUPled?");
            return !retStr.Equals("0");
        }

        public void SetFOMMode(int Channel, int Range, bool mode)
        {
            string stateValue = mode ? "1" : "0";
            ScpiCommand($"SENSe{Channel}:FOM:RANGe{Range}:COUPled {stateValue}");
        }

        public void SetFOMSweepType(int Channel, int Range, StandardSweepTypeEnum standardSweepType)
        {
            string scpi = Scpi.Format("{0}", standardSweepType);
            ScpiCommand($"SENSe{Channel}:FOM:RANGe{Range}:SWEep:TYPE {scpi}");
        }

        public double GetFOMStart(int Channel, int Range)
        {
            return ScpiQuery<double>($"SENSe{Channel}:FOM:RANGe{Range}:FREQuency:STARt?");
        }

        public void SetFOMStart(int Channel, int Range, double freq)
        {
            ScpiCommand($"SENSe{Channel}:FOM:RANGe{Range}:FREQuency:STARt {freq}");
        }

        public double GetFOMStop(int Channel, int Range)
        {
            return ScpiQuery<double>($"SENSe{Channel}:FOM:RANGe{Range}:FREQuency:STOP?");
            ;
        }

        public void SetFOMStop(int Channel, int Range, double freq)
        {
            ScpiCommand($"SENSe{Channel}:FOM:RANGe{Range}:FREQuency:STOP {freq}");
        }

        public double GetFOMCW(int Channel, int Range)
        {
            return ScpiQuery<double>($"SENSe{Channel}:FOM:RANGe{Range}:FREQuency:CW?");
        }

        public void SetFOMCW(int Channel, int Range, double freq)
        {
            ScpiCommand($"SENSe{Channel}:FOM:RANGe{Range}:FREQuency:CW {freq}");
        }

        public double GetFOMOffset(int Channel, int Range)
        {
            return ScpiQuery<double>($"SENSe{Channel}:FOM:RANGe{Range}:FREQuency:OFFSet?");
        }

        public void SetFOMOffset(int Channel, int Range, double freq)
        {
            ScpiCommand($"SENSe{Channel}:FOM:RANGe{Range}:FREQuency:OFFSet {freq}");
        }

        public double GetFOMDivisor(int Channel, int Range)
        {
            return ScpiQuery<double>($"SENSe{Channel}:FOM:RANGe{Range}:FREQuency:DIVisor?");
        }

        public void SetFOMDivisor(int Channel, int Range, double freq)
        {
            ScpiCommand($"SENSe{Channel}:FOM:RANGe{Range}:FREQuency:DIVisor {freq}");
        }

        public double GetFOMMultiplier(int Channel, int Range)
        {
            return ScpiQuery<double>($"SENSe{Channel}:FOM:RANGe{Range}:FREQuency:MULTiplier?");
        }

        public void SetFOMMultiplier(int Channel, int Range, double freq)
        {
            ScpiCommand($"SENSe{Channel}:FOM:RANGe{Range}:FREQuency:MULTiplier {freq}");
        }

        public double GetFOMXAxis(int Channel)
        {
            return ScpiQuery<double>($"SENSe{Channel}:FOM:DISPlay:SELect?");
        }

        public void SetFOMXAxis(int Channel, string value)
        {
            ScpiCommand($"SENSe{Channel}:FOM:DISPlay:SELect {value}");
        }

        #region Segment Sweep
        public void FOMSegmentDeleteAllSegments(int Channel, int Range)
        {
            ScpiCommand($"SENSe{Channel}:FOM:RANGe{Range}:SEGMent:DELete:ALL");
        }

        public int FOMSegmentAdd(int Channel, int Range)
        {
            int NumberOfSegments = ScpiQuery<int>(
                $"SENSe{Channel}:FOM:RANGe{Range}:SEGMent:COUNt?"
            );
            NumberOfSegments++;
            ScpiCommand($"SENSe{Channel}:FOM:RANGe{Range}:SEGMent{NumberOfSegments}:ADD");
            return NumberOfSegments;
        }

        public void FOMSetSegmentState(int Channel, int Range, int segment, bool state)
        {
            string stateValue = state ? "ON" : "OFF";
            ScpiCommand($"SENSe{Channel}:FOM:RANGe{Range}:SEGMent{segment}:STATE {stateValue}");
        }

        public void FOMSetSegmentNumberOfPoints(int Channel, int Range, int segment, int points)
        {
            ScpiCommand($"SENSe{Channel}:FOM:RANGe{Range}:SEGMent{segment}:SWEep:POINts {points}");
        }

        public void FOMSetSegmentStartFrequency(int Channel, int Range, int segment, double freq)
        {
            ScpiCommand($"SENSe{Channel}:FOM:RANGe{Range}:SEGMent{segment}:FREQuency:STARt {freq}");
        }

        public void FOMSetSegmentStopFrequency(int Channel, int Range, int segment, double freq)
        {
            ScpiCommand($"SENSe{Channel}:FOM:RANGe{Range}:SEGMent{segment}:FREQuency:STOP {freq}");
        }
        #endregion
    }
}
