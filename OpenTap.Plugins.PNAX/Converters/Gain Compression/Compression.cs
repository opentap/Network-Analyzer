// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OpenTap.Plugins.PNAX
{
    public enum CompressionMethodEnum
    {
        [Scpi("CFLG")]
        [Display("Compression From Linear Gain")]
        CompressionFromLinearGain,
        [Scpi("CFMG")]
        [Display("Compression From Max Gain")]
        CompressionFromMaxGain,
        [Scpi("BACKoff")]
        [Display("Compression From Back Off")]
        CompressionFromBackOff,
        [Scpi("XYCOM")]
        [Display("X/Y Compression")]
        XYCompression,
        [Scpi("SAT")]
        [Display("Compression From Saturation")]
        CompressionFromSaturation
    }

    public enum EndOfSweepConditionEnum
    {
        [Scpi("STAN")]
        Default,
        [Scpi("POFF")]
        RFOff,
        [Scpi("PSTA")]
        StartPower,
        [Scpi("PSTO")]
        StopPower
    }

    [AllowAsChildIn(typeof(GainCompressionChannel))]
    [Display("Compression", Groups: new[] { "PNA-X", "Converters", "Compression" }, Description: "Insert a description here")]
    public class Compression : ConverterCompressionBaseStep
    {
        [Browsable(false)]
        public bool IsCompressionFromGain { get; set; } = false;
        [Browsable(false)]
        public bool IsCompressionFromBackOff { get; set; } = false;
        [Browsable(false)]
        public bool IsCompressionXY { get; set; } = false;
        [Browsable(false)]
        public bool IsCompressionFromSaturation { get; set; } = false;

        #region Settings
        private CompressionMethodEnum _CompressionMethodEnum;
        [Display("Compression Method", Group: "Compression Method", Order: 10)]
        public CompressionMethodEnum CompressionMethod 
        {
            get
            {
                return _CompressionMethodEnum;
            }
            set 
            {
                _CompressionMethodEnum = value;
                IsCompressionFromBackOff = false;
                IsCompressionFromGain = false;
                IsCompressionXY = false;
                IsCompressionFromSaturation = false;
                if ((_CompressionMethodEnum == CompressionMethodEnum.CompressionFromMaxGain) || 
                    (_CompressionMethodEnum == CompressionMethodEnum.CompressionFromLinearGain))
                {
                    IsCompressionFromGain = true;
                }
                else if (_CompressionMethodEnum == CompressionMethodEnum.CompressionFromBackOff)
                {
                    IsCompressionFromGain = true;
                    IsCompressionFromBackOff = true;
                }
                else if (_CompressionMethodEnum == CompressionMethodEnum.XYCompression)
                {
                    IsCompressionXY = true;
                }
                else if (_CompressionMethodEnum == CompressionMethodEnum.CompressionFromSaturation)
                {
                    IsCompressionFromSaturation = true;
                }
            }
        }

        [EnabledIf("IsCompressionFromGain", true, HideIfDisabled = true)]
        [Display("Level", Group: "Compression Method", Order: 11)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double CompressionLevel { get; set; }

        [EnabledIf("IsCompressionFromBackOff", true, HideIfDisabled =true)]
        [Display("Back Off", Group: "Compression Method", Order: 12)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double CompressionBackOff { get; set; }

        [EnabledIf("IsCompressionXY", true, HideIfDisabled = true)]
        [Display("Delta X", Group: "Compression Method", Order: 13)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double CompressionDeltaX { get; set; }

        [EnabledIf("IsCompressionXY", true, HideIfDisabled = true)]
        [Display("Delta Y", Group: "Compression Method", Order: 14)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double CompressionDeltaY { get; set; }

        [EnabledIf("IsCompressionFromSaturation", true, HideIfDisabled = true)]
        [Display("From Max Pout", Group: "Compression Method", Order: 15)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double CompressionFromMaxPout { get; set; }



        [Display("Tolerance", Group: "SMART Sweep", Order: 20)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double SMARTSweepTolerance { get; set; }

        [Display("Maximum Iterations", Group: "SMART Sweep", Order: 21)]
        public int SMARTSweepIterations { get; set; }

        [Display("Show Iterations", Group: "SMART Sweep", Order: 22)]
        public bool SMARTSweepShowIterations { get; set; }

        [Display("Read DC at Compression Point", Group: "SMART Sweep", Order: 23)]
        public bool SMARTSweepReadDC { get; set; }

        [Display("Safe Mode", Group: "SMART Sweep", Order: 24)]
        public bool SMARTSweepSafeMode { get; set; }

        [EnabledIf("SMARTSweepSafeMode", true, HideIfDisabled = true)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0.000")]
        [Display("Coarse Increment", Group: "SMART Sweep", Order: 25)]
        public int SMARTSweepCoarseIncrement { get; set; }

        [EnabledIf("SMARTSweepSafeMode", true, HideIfDisabled = true)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0.000")]
        [Display("Fine Increment", Group: "SMART Sweep", Order: 26)]
        public double SMARTSweepFineIncrement { get; set; }

        [EnabledIf("SMARTSweepSafeMode", true, HideIfDisabled = true)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0.000")]
        [Display("Fine Threshold", Group: "SMART Sweep", Order: 27)]
        public double SMARTSweepFineThreshold { get; set; }

        [EnabledIf("SMARTSweepSafeMode", true, HideIfDisabled = true)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.000")]
        [Display("Max Output Power", Group: "SMART Sweep", Order: 28)]
        public double SMARTSweepMaxOutputPower { get; set; }

        [Display("Compression Point Interpolation", Group: "2D Sweep", Order: 29)]
        public bool CompressionPointInterpolation { get; set; }


        [Display("End of Sweep Condition", Group: "Sweep", Order: 30)]
        public EndOfSweepConditionEnum EndOfSweepCondition { get; set; }

        [Unit("S", UseEngineeringPrefix: true, PreScaling: 1e3, StringFormat: "0.000")]
        [Display("Settling Time", Group: "Sweep", Order: 31)]
        public double SettlingTime { get; set; }

        #endregion

        public Compression()
        {
            UpdateDefaultValues();
        }

        private void UpdateDefaultValues()
        {
            var DefaultValues = PNAX.GetConverterCompressionDefaultValues();
            CompressionMethod = DefaultValues.CompressionMethod;
            CompressionLevel = DefaultValues.CompressionLevel;
            CompressionBackOff = DefaultValues.CompressionBackOff;
            CompressionDeltaX = DefaultValues.CompressionDeltaX;
            CompressionDeltaY = DefaultValues.CompressionDeltaY;
            CompressionFromMaxPout = DefaultValues.CompressionFromMaxPout;
            SMARTSweepTolerance = DefaultValues.SMARTSweepTolerance;
            SMARTSweepIterations = DefaultValues.SMARTSweepIterations;
            SMARTSweepShowIterations = DefaultValues.SMARTSweepShowIterations;
            SMARTSweepReadDC = DefaultValues.SMARTSweepReadDC;
            SMARTSweepSafeMode = DefaultValues.SMARTSweepSafeMode;
            SMARTSweepCoarseIncrement = DefaultValues.SMARTSweepCoarseIncrement;
            SMARTSweepFineIncrement = DefaultValues.SMARTSweepFineIncrement;
            SMARTSweepFineThreshold = DefaultValues.SMARTSweepFineThreshold;
            SMARTSweepMaxOutputPower = DefaultValues.SMARTSweepMaxOutputPower;
            CompressionPointInterpolation = DefaultValues.CompressionPointInterpolation;
            EndOfSweepCondition = DefaultValues.EndOfSweepCondition;
            SettlingTime = DefaultValues.SettlingTime;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetCompressionMethod(Channel, CompressionMethod);
            switch (CompressionMethod)
            {
                case CompressionMethodEnum.CompressionFromLinearGain:
                case CompressionMethodEnum.CompressionFromMaxGain:
                    PNAX.SetCompressionLevel(Channel, CompressionLevel);
                    break;
                case CompressionMethodEnum.CompressionFromBackOff:
                    PNAX.SetCompressionLevel(Channel, CompressionLevel);
                    PNAX.SetCompressionBackoffLevel(Channel, CompressionBackOff);
                    break;
                case CompressionMethodEnum.XYCompression:
                    PNAX.SetCompressionDeltaX(Channel, CompressionDeltaX);
                    PNAX.SetCompressionDeltaY(Channel, CompressionDeltaY);
                    break;
                case CompressionMethodEnum.CompressionFromSaturation:
                    PNAX.SetCompressionSaturation(Channel, CompressionFromMaxPout);
                    break;
            }

            PNAX.SetSMARTSweepTolerance(Channel, SMARTSweepTolerance);
            PNAX.SetSMARTSweepMaximumIterations(Channel, SMARTSweepIterations);
            PNAX.SetSMARTSweepShowIterations(Channel, SMARTSweepShowIterations);
            PNAX.SetSMARTSweepReadDCAtCompression(Channel, SMARTSweepReadDC);

            PNAX.SetSMARTSweepSafeMode(Channel, SMARTSweepSafeMode);
            PNAX.SetSMARTSweepSafeModeCoarseIncrement(Channel, SMARTSweepCoarseIncrement);
            PNAX.SetSMARTSweepSafeModeFineIncrement(Channel, SMARTSweepFineIncrement);
            PNAX.SetSMARTSweepSafeModeFineThreshold(Channel, SMARTSweepFineThreshold);
            PNAX.SetSMARTSweepSafeModeMaxOutputPower(Channel, SMARTSweepMaxOutputPower);

            PNAX.Set2DSweepCompressionPointInterpolation(Channel, CompressionPointInterpolation);

            PNAX.SetCompressionEndOfSweepCondition(Channel, EndOfSweepCondition);
            PNAX.SetCompressionSettlingTime(Channel, SettlingTime);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
