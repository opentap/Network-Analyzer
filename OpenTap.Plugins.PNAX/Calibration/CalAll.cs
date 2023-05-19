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
    [Flags]
    public enum ExtraPowerCalsEnum
    {
        [Display("No Independent Source Cal")]
        NoIndependentSourceCal = 0,
        [Display("Port 1")]
        Port1 = 1,
        [Display("Port 2")]
        Port2 = 2,
        [Display("Port 3")]
        Port3 = 4,
        [Display("Port 4")]
        Port4 = 8,
        [Display("Port 1 Src2")]
        Port1Src2 = 16,
        [Display("Source3")]
        Source3 = 32
    }

    public enum NoiseCalMethodEnum
    {
        Scalar,
        Vector
    }

    public enum NoiseTunerEnum
    {
        [Display("internal")]
        _internal
    }

    public enum ReceiverCharacterizationMethodEnum
    {
        UsePowerMeter,
        UseNoiseSource
    }

    [Display("Cal All", Groups: new[] { "PNA-X", "Calibration" }, Description: "Insert a description here")]
    public class CalAll : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }


        // Cal Properties
        private bool _CalibrateStandardChannel;
        [Display("Cal Standard Channel", Group: "Measurement Class Calibration", Order: 10)]
        public bool CalibrateStandardChannel
        {
            get
            {
                return _CalibrateStandardChannel;
            }
            set
            {
                _CalibrateStandardChannel = value;
                UpdateCalProperties();
            }
        }

        private bool _CalibrateSMCChannel;
        [Display("Cal SMC + Phase Channel", Group: "Measurement Class Calibration", Order: 11)]
        public bool CalibrateSMCChannel
        {
            get
            {
                return _CalibrateSMCChannel;
            }
            set
            {
                _CalibrateSMCChannel = value;
                UpdateCalProperties();
            }
        }


        private bool _CalibrateGainCompressionChannel;
        [Display("Cal Gain Compression Channel", Group: "Measurement Class Calibration", Order: 12)]
        public bool CalibrateGainCompressionChannel
        {
            get
            {
                return _CalibrateGainCompressionChannel;
            }
            set
            {
                _CalibrateGainCompressionChannel = value;
                UpdateCalProperties();
            }
        }

        private bool _CalibrateGainCompressionConvertersChannel;
        [Display("Cal Gain Compression Converters Channel", Group: "Measurement Class Calibration", Order: 13)]
        public bool CalibrateGainCompressionConvertersChannel
        {
            get
            {
                return _CalibrateGainCompressionConvertersChannel;
            }
            set
            {
                _CalibrateGainCompressionConvertersChannel = value;
                UpdateCalProperties();
            }
        }


        private bool _CalibrateSweptIMDChannel;
        [Display("Cal Swept IMD Channel", Group: "Measurement Class Calibration", Order: 14)]
        public bool CalibrateSweptIMDChannel
        {
            get
            {
                return _CalibrateSweptIMDChannel;
            }
            set
            {
                _CalibrateSweptIMDChannel = value;
                UpdateCalProperties();
            }
        }


        private bool _CalibrateSweptIMDConvertersChannel;
        [Display("Cal Swept IMD Converters Channel", Group: "Measurement Class Calibration", Order: 15)]
        public bool CalibrateSweptIMDConvertersChannel
        {
            get
            {
                return _CalibrateSweptIMDConvertersChannel;
            }
            set
            {
                _CalibrateSweptIMDConvertersChannel = value;
                UpdateCalProperties();
            }
        }


        private bool _CalibrateNoiseFigureColdSourceChannel;
        [Display("Cal Noise Figure Cold Source Channel", Group: "Measurement Class Calibration", Order: 16)]
        public bool CalibrateNoiseFigureColdSourceChannel
        {
            get
            {
                return _CalibrateNoiseFigureColdSourceChannel;
            }
            set
            {
                _CalibrateNoiseFigureColdSourceChannel = value;
                UpdateCalProperties();
            }
        }


        private bool _CalibrateNoiseFigureConvertersChannel;
        [Display("Cal Noise Figure Converters Channel", Group: "Measurement Class Calibration", Order: 17)]
        public bool CalibrateNoiseFigureConvertersChannel
        {
            get
            {
                return _CalibrateNoiseFigureConvertersChannel;
            }
            set
            {
                _CalibrateNoiseFigureConvertersChannel = value;
                UpdateCalProperties();
            }
        }




        // Only enabled if:
        // Scalar Mixer/Converters + Phase
        // Gain Compression Converters
        [Browsable(false)]
        public bool IsSplitCalEnabled { get; set; }

        // Only enabled if:
        // Scalar Mixer/Converters + Phase
        [Browsable(false)]
        public bool IsPhaseEnabled { get; set; }


        // Only enabled if:
        // Standard Channel is the only channel enabled
        [Browsable(false)]
        public bool IsPowerCalEnabled { get; set; }

        // Only enabled if:
        // Swept IMD
        // Swept IMD Converters
        [Browsable(false)]
        public bool IsIMDEnabled { get; set; }

        // Only enabled if:
        // Noise Figure Cold Source
        // Noise Figure Converters
        [Browsable(false)]
        public bool IsNoiseEnabled { get; set; }







        [Display("Use Smart Cal Order", Groups: new[] { "Measurement Class Calibration", "Miscellaneous" }, Order: 20)]
        public bool UserSmartCalOrder { get; set; }

        [Display("Enable Extra Power Cals", Groups: new[] { "Measurement Class Calibration", "Miscellaneous" }, Order: 21)]
        public ExtraPowerCalsEnum ExtraPowerCals { get; set; }

        [Display("Independent Calibration Channels", Groups: new[] { "Measurement Class Calibration", "Miscellaneous" }, Order: 22)]
        public List<int> IndependentCalibrationChannels { get; set; }

        [EnabledIf("IsSplitCalEnabled", true, HideIfDisabled = true)]
        [Display("Split Cal", Groups: new[] { "Measurement Class Calibration", "Miscellaneous" }, Order: 23)]
        public bool SplitCal { get; set; }

        [EnabledIf("IsPhaseEnabled", true, HideIfDisabled = true)]
        [Display("Enable Phase Correction", Groups: new[] { "Measurement Class Calibration", "Phase" }, Order: 30)]
        public bool EnablePhaseCorrection { get; set; }

        [EnabledIf("IsPowerCalEnabled", true, HideIfDisabled = true)]
        [Display("Include Power Calibration", Groups: new[] {"Measurement Class Calibration", "Power Cal" }, Order: 40)]
        public bool IncludePowerCalibration { get; set; }

        [EnabledIf("IsIMDEnabled", true, HideIfDisabled = true)]
        [Display("Max Product Order", Groups: new[] { "Measurement Class Calibration", "IMD" }, Order: 50)]
        public int MaxProductOrder { get; set; }

        [EnabledIf("IsIMDEnabled", true, HideIfDisabled = true)]
        [Display("Include 2nd Order", Groups: new[] { "Measurement Class Calibration", "IMD" }, Order: 51)]
        public bool Include2ndOrder { get; set; }

        [EnabledIf("IsNoiseEnabled", true, HideIfDisabled = true)]
        [Display("Include 2nd Order", Groups: new[] { "Measurement Class Calibration", "Noise" }, Order: 60)]
        public NoiseCalMethodEnum NoiseCalMethod { get; set; }

        [EnabledIf("IsNoiseEnabled", true, HideIfDisabled = true)]
        [EnabledIf("NoiseCalMethod", NoiseCalMethodEnum.Vector, HideIfDisabled = true)]
        [Display("Noise Tuner", Groups: new[] { "Measurement Class Calibration", "Noise" }, Order: 61)]
        public NoiseTunerEnum NoiseTuner { get; set; }

        [EnabledIf("IsNoiseEnabled", true, HideIfDisabled = true)]
        [Display("Receiver Characterization Method", Groups: new[] { "Measurement Class Calibration", "Noise" }, Order: 62)]
        public ReceiverCharacterizationMethodEnum ReceiverCharacterizationMethod { get; set; }

        [EnabledIf("IsNoiseEnabled", true, HideIfDisabled = true)]
        [Display("Force Thru Adapter De-embed", Groups: new[] { "Measurement Class Calibration", "Noise" }, Order: 63)]
        public bool ForceThruAdapter { get; set; }

        [EnabledIf("IsNoiseEnabled", true, HideIfDisabled = true)]
        [Display("Force Power Sensor Adapter De-embed", Groups: new[] { "Measurement Class Calibration", "Noise" }, Order: 64)]
        public bool ForcePowerSensor { get; set; }
        #endregion

        private void UpdateCalProperties()
        {
            IsSplitCalEnabled = false;
            if (CalibrateSMCChannel || CalibrateGainCompressionConvertersChannel)
            {
                IsSplitCalEnabled = true;
            }

            IsPhaseEnabled = false;
            if (CalibrateSMCChannel)
            {
                IsPhaseEnabled = true;
            }

            IsPowerCalEnabled = false;
            if (CalibrateStandardChannel) 
            {
                if (!CalibrateSMCChannel &&
                    !CalibrateGainCompressionChannel &&
                    !CalibrateGainCompressionConvertersChannel &&
                    !CalibrateSweptIMDChannel &&
                    !CalibrateSweptIMDConvertersChannel &&
                    !CalibrateNoiseFigureColdSourceChannel &&
                    !CalibrateNoiseFigureConvertersChannel)
                {
                    IsPowerCalEnabled = true;
                }
            }

            IsIMDEnabled = false;
            if (CalibrateSweptIMDChannel || CalibrateSweptIMDConvertersChannel)
            {
                IsIMDEnabled = true;
            }

            IsNoiseEnabled = false;
            if (CalibrateNoiseFigureColdSourceChannel || CalibrateNoiseFigureConvertersChannel)
            {
                IsNoiseEnabled = true;
            }
        }

        public CalAll()
        {
            UserSmartCalOrder = false;
            IndependentCalibrationChannels = new List<int>();
            ExtraPowerCals = ExtraPowerCalsEnum.NoIndependentSourceCal;
            SplitCal = false;
            IncludePowerCalibration = false;

            EnablePhaseCorrection = false;

            MaxProductOrder = 3;
            Include2ndOrder = false;

            NoiseCalMethod = NoiseCalMethodEnum.Scalar;
            NoiseTuner = NoiseTunerEnum._internal;
            ReceiverCharacterizationMethod = ReceiverCharacterizationMethodEnum.UsePowerMeter;
            ForceThruAdapter = false;
            ForcePowerSensor = false;

        }

        public override void Run()
        {
            PNAX.CalAllReset();

            // TODO
            // Query all measurement classes
            // validate that all the channels have the settings enabled
            // i.e. for Standard the CalibrateStandardChannel should be true
            // if not, let the user know and fail the test

            List<int> AllChannels = PNAX.CalAllSelectChannels();

            PNAX.CalAllSelectPorts(AllChannels, new List<int> { 1, 2 });

            PNAX.CalAllSetProperty("Include Power Calibration", IncludePowerCalibration.ToString());

            int CalChannel = PNAX.CalAllGuidedChannelNumber();

            PNAX.CalAllSelectDutConnectorType(CalChannel, 1, "Type N (50) male");
            PNAX.CalAllSelectDutConnectorType(CalChannel, 2, "Type N (50) male");

            PNAX.CalAllSelectCalKit(CalChannel, 1, "85032F");
            PNAX.CalAllSelectCalKit(CalChannel, 2, "85032F");

            PNAX.CalAllInit(CalChannel);

            int CalSteps = PNAX.CalAllNumberOfSteps(CalChannel);

            for(int CalStep = 1; CalStep <= CalSteps; CalStep++)
            {
                String StepDescription = PNAX.CalAllStepDescription(CalChannel, CalStep);

                Log.Info($"Step {CalStep}: {StepDescription}");

                PNAX.CalAllStep(CalChannel, CalStep);
                PNAX.WaitForOperationComplete(20000);
            }

            PNAX.CalAllSave(CalChannel);

            //if (PNAX.SimulatorMode())
            //{
            // Creates a unity Cal Set
            // SENSe<cnum>:CORRection:CSET:CREate:DEFault [<csetname>], [<correctiontype>]
            // SENS:CORR:CSET:CRE:DEF 'My2P','Full 2P(1,2)'
            //}

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
