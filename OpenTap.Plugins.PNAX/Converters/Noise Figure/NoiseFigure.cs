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

    public enum NoiseBandwidthNoise
    {
        [Scpi("24e6")]
        [Display("24 MHz")]
        TwentyFour,
        [Scpi("8e6")]
        [Display("8.0 MHz")]
        Eight,
        [Scpi("4e6")]
        [Display("4.0 MHz")]
        Four,
        [Scpi("2e6")]
        [Display("2.0 MHz")]
        Two,
        [Scpi("8e3")]
        [Display("0.8 MHz")]
        PointEight
    }
    public enum NoiseBandwidthNormal
    {
        [Scpi("1.2e6")]
        [Display("1.2 MHz")]
        OnePointTwo,
        [Scpi("720e3")]
        [Display("720 kHz")]
        PointSevenTwo
    }

    public enum NoiseReceiver
    {
        [Scpi("NORM")]
        [Display("NA Receiver (Port 2)")]
        NAReceiver,
        [Scpi("NOIS")]
        [Display("Noise Receiver")]
        NoiseReceiver
    }

    public enum ReceiverGain
    {
        [Scpi("30")]
        [Display("High")]
        High,
        [Scpi("15")]
        [Display("Medium")]
        Medium,
        [Scpi("0")]
        [Display("Low")]
        Low
    }

    [AllowAsChildIn(typeof(NoiseFigureChannel))]
    [Display("Noise Figure", Groups: new[] { "PNA-X", "Converters", "Noise Figure Converters" }, Description: "Insert a description here")]
    public class NoiseFigure : ConverterBaseStep
    {
        #region Settings
        [Browsable(false)]
        public bool IsUseNarrowbandCompensationEnabled { get; set; }


        private NoiseBandwidthNoise _NoiseBandwidthNoise;
        [EnabledIf("NoiseReceiver", NoiseReceiver.NoiseReceiver, HideIfDisabled = true)]
        [Display("Noise Bandwidth", Group: "Bandwidth/Average", Order: 21)]
        public NoiseBandwidthNoise NoiseBandwidthNoise
        {
            get
            {
                return _NoiseBandwidthNoise;
            }
            set
            {
                _NoiseBandwidthNoise = value;
                UpdateIsUseNarrowbandCompensationEnabled();
            }
        }
        private NoiseBandwidthNormal _NoiseBandwidthNormal;
        [EnabledIf("NoiseReceiver", NoiseReceiver.NAReceiver, HideIfDisabled = true)]
        [Display("Noise Bandwidth", Group: "Bandwidth/Average", Order: 21)]
        public NoiseBandwidthNormal NoiseBandwidthNormal
        {
            get
            {
                return _NoiseBandwidthNormal;
            }
            set
            {
                _NoiseBandwidthNormal = value;
                UpdateIsUseNarrowbandCompensationEnabled();
            }
        }

        private int AverageNumberNoise;
        private int AverageNumberNormal;
        private bool IsAverageOnNoise;
        private bool IsAverageOnNormal;

        [Display("Average Number", Group: "Bandwidth/Average", Order: 22)]
        public int AverageNumber { get; set; }


        [Display("Average On", Group: "Bandwidth/Average", Order: 23)]
        public bool IsAverageOn { get; set; }

        [EnabledIf("IsUseNarrowbandCompensationEnabled", true, HideIfDisabled = false)]
        [Display("Use Narrowband Compensation", Group: "Bandwidth/Average", Order: 23)]
        public bool UseNarrowbandCompensation { get; set; }

        private NoiseReceiver _NoiseReceiver;
        [Display("Noise Receiver", Group: "Noise Receiver", Order: 31)]
        public NoiseReceiver NoiseReceiver
        {
            get
            {
                return _NoiseReceiver;
            }
            set
            {
                _NoiseReceiver = value;
                UpdateIsUseNarrowbandCompensationEnabled();

                if (_NoiseReceiver == NoiseReceiver.NoiseReceiver)
                {
                    AverageNumber = AverageNumberNoise;
                    IsAverageOn = IsAverageOnNoise;
                }
                else if (_NoiseReceiver == NoiseReceiver.NAReceiver)
                {
                    AverageNumber = AverageNumberNormal;
                    IsAverageOn = IsAverageOnNormal;
                }
            }
        }


        [EnabledIf("NoiseReceiver", NoiseReceiver.NoiseReceiver, HideIfDisabled = false)]
        [Display("Receiver Gain", Group: "Noise Receiver", Order: 32)]
        public ReceiverGain ReceiverGain { get; set; }

        [Display("Source Temperature", Group: "Source Temperature", Order: 41)]
        public double SourceTemperature { get; set; }
        [Display("Use 302K for Vector Noise", Group: "Source Temperature", Order: 41)]
        public bool Use302K { get; set; }

        [Display("Max Acquired Impedance States", Group: "Impedance States", Order: 50)]
        public int MaxImpedanceStates { get; set; }

        [Display("Enable Source Pulling for SParameters", Group: "Impedance States", Order: 51)]
        public bool EnableSourcePulling { get; set; }

        [FilePath]
        [Display("Noise Tuner File", Group: "Impedance States", Order: 52)]
        public String NoiseTunerFile { get; set; }

        #endregion

        public void UpdateIsUseNarrowbandCompensationEnabled()
        {
            IsUseNarrowbandCompensationEnabled = false;
            if (_NoiseReceiver == NoiseReceiver.NoiseReceiver)
            {
                IsUseNarrowbandCompensationEnabled = true;
            }
            else if ((_NoiseBandwidthNoise == NoiseBandwidthNoise.Four) ||
                (_NoiseBandwidthNoise == NoiseBandwidthNoise.Two) ||
                (_NoiseBandwidthNoise == NoiseBandwidthNoise.PointEight))
            {
                IsUseNarrowbandCompensationEnabled = true;
            }

            if (IsUseNarrowbandCompensationEnabled == false)
            {
                // uncheck it
                UseNarrowbandCompensation = false;
            }
        }

        public NoiseFigure()
        {
            var NFDefault = PNAX.GetNoiseFigureConverterDefaultValues();

            NoiseBandwidthNoise = NFDefault.NoiseBandwidthNoise;
            NoiseBandwidthNormal = NFDefault.NoiseBandwidthNormal;
            AverageNumberNoise = NFDefault.AverageNumberNoise;
            AverageNumberNormal = NFDefault.AverageNumberNormal;
            NoiseReceiver = NFDefault.NoiseReceiver;    // After AverageNumberNoise and AverageNumberNormal have been set
            IsAverageOnNoise = NFDefault.IsAverageOnNoise;
            IsAverageOnNormal = NFDefault.IsAverageOnNormal;
            UseNarrowbandCompensation = NFDefault.UseNarrowbandCompensation;
            ReceiverGain = NFDefault.ReceiverGain;
            SourceTemperature = NFDefault.SourceTemperature;
            Use302K = NFDefault.Use302K;
            MaxImpedanceStates = NFDefault.MaxImpedanceStates;
            EnableSourcePulling = NFDefault.EnableSourcePulling;
            NoiseTunerFile = NFDefault.NoiseTunerFile;

            Rules.Add(() => ((MaxImpedanceStates >= 4) && (MaxImpedanceStates <= 7)), "Max Acquired Impedance States must be between 4 and 7", nameof(MaxImpedanceStates));

        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetNFNoiseReceiver(Channel, NoiseReceiver);
            PNAX.SetNFAverage(Channel, AverageNumber);
            PNAX.SetNFNoiseAverage(Channel, IsAverageOn);
            PNAX.SetNFNarrowbandNoiseFigureCompensation(Channel, UseNarrowbandCompensation);

            if (NoiseReceiver == NoiseReceiver.NoiseReceiver)
            {
                PNAX.SetNFBandwidth(Channel, NoiseBandwidthNoise);
                PNAX.SetNFReceiverGain(Channel, ReceiverGain);
            }
            else if (NoiseReceiver == NoiseReceiver.NAReceiver)
            {
                PNAX.SetNFBandwidth(Channel, NoiseBandwidthNormal);
            }
            else
            {
                throw new Exception("Unknown Noise Receiver mode!");
            }

            PNAX.SetNFTemperature(Channel, SourceTemperature);
            PNAX.SetNFUse302K(Channel, Use302K);

            PNAX.SetNFMaxImpedanceStates(Channel, MaxImpedanceStates);
            PNAX.SetNFEnableSourcePulling(Channel, EnableSourcePulling);

            if (!NoiseTunerFile.Equals(""))
            {
                PNAX.SetNFEnableCustomNoiseTuner(Channel, true);
                PNAX.SetNFCustomNoiseTunerFile(Channel, NoiseTunerFile);
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
