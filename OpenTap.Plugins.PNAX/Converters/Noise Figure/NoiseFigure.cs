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

    public enum NoiseBandwidth
    {
        [Display("24 MHz")]
        TwentyFour,
        [Display("8.0 MHz")]
        Eight,
        [Display("4.0 MHz")]
        Four,
        [Display("2.0 MHz")]
        Two,
        [Display("0.8 MHz")]
        PointEight
    }
    public enum NoiseReceiver
    {
        [Display("NA Receiver (Port 2)")]
        NAReceiver,
        [Display("Noise Receiver")]
        NoiseReceiver
    }

    public enum ReceiverGain
    {
        [Display("High")]
        High,
        [Display("Medium")]
        Medium,
        [Display("Low")]
        Low
    }

    [AllowAsChildIn(typeof(NoiseFigureChannel))]
    [Display("Noise Figure", Groups: new[] { "PNA-X", "Converters", "Noise Figure Converters" }, Description: "Insert a description here")]
    public class NoiseFigure : ConverterBaseStep
    {
        #region Settings
        [Display("Noise Bandwidth", Group: "Bandwidth/Average", Order: 21)]
        public NoiseBandwidth NoiseBandwidth { get; set; }

        [Display("Average Number", Group: "Bandwidth/Average", Order: 22)]
        public int AverageNumber { get; set; }

        [Display("Average On", Group: "Bandwidth/Average", Order: 23)]
        public bool IsAverageOn { get; set; }

        [EnabledIf("NoiseBandwidth", NoiseBandwidth.Four, NoiseBandwidth.Two, NoiseBandwidth.PointEight, HideIfDisabled = true)]
        [Display("Use Narrowband Compensation", Group: "Bandwidth/Average", Order: 23)]
        public bool UseNarrowbandCompensation { get; set; }

        [Display("Noise Receiver", Group: "Noise Receiver", Order: 31)]
        public NoiseReceiver NoiseReceiver { get; set; }

        [Display("Receiver Gain", Group: "Noise Receiver", Order: 32)]
        public ReceiverGain ReceiverGain { get; set; }

        [Display("Source Temperature", Group: "Source Temperature", Order: 41)]
        public double SourceTemperature { get; set; }
        [Display("Use 302K for Vector Noise", Group: "Source Temperature", Order: 41)]
        public bool Use302K { get; set; }

        #endregion

        public NoiseFigure()
        {
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }
    }
}
