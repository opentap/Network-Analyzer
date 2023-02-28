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
        [Display("Compression From Linear Gain")]
        CompressionFromLinearGain,
        [Display("Compression From Max Gain")]
        CompressionFromMaxGain,
        [Display("Compression From Back Off")]
        CompressionFromBackOff,
        [Display("X/Y Compression")]
        XYCompression,
        [Display("Compression From Saturation")]
        CompressionFromSaturation
    }

    public enum EndOfSweepConditionEnum
    {
        Default,
        RFOff,
        StartPower,
        StopPower
    }

    [AllowAsChildIn(typeof(GainCompressionChannel))]
    [Display("Compression", Groups: new[] { "PNA-X", "Converters", "Compression" }, Description: "Insert a description here")]
    public class Compression : TestStep
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

        [Display("End of Sweep Condition", Group: "Sweep", Order: 30)]
        public EndOfSweepConditionEnum EndOfSweepCondition { get; set; }

        [Unit("S", UseEngineeringPrefix: true, PreScaling: 1e3, StringFormat: "0.000")]
        [Display("Settling Time", Group: "Sweep", Order: 31)]
        public double SettlingTime { get; set; }

        #endregion

        public Compression()
        {
            CompressionMethod = CompressionMethodEnum.CompressionFromLinearGain;
            CompressionLevel = 1;
            CompressionBackOff = 10;
            CompressionDeltaX = 10;
            CompressionDeltaY = 9;
            CompressionFromMaxPout = 0.1;

            SMARTSweepSafeMode = false;
            SMARTSweepCoarseIncrement = 3;
            SMARTSweepFineIncrement = 1;
            SMARTSweepFineThreshold = 0.5;
            SMARTSweepMaxOutputPower = 30;

            EndOfSweepCondition = EndOfSweepConditionEnum.Default;
            SettlingTime = 0.000;
        }

        public override void Run()
        {
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
