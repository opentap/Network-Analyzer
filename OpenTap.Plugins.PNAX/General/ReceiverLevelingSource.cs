// Author: CMontes
// Copyright:   Copyright 2024 Keysight Technologies
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

namespace OpenTap.Plugins.PNAX.General
{
    [AllowAsChildIn(typeof(ReceiverLeveling))]
    [Display("Controlled Source", Groups: new[] { "Network Analyzer", "General" },
        Description: "Controlled Source\nCan be added as a child to the following Channels:\n\tReceiver Leveling")]
    public class ReceiverLevelingSource : PNABaseStep
    {
        #region Settings
        [Display("Controlled Source", Group: "Receiver Leveling Setup", Order: 20)]
        public string ControlledSource
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }

        [Display("Enabling Leveling", Group: "Receiver Leveling Setup", Order: 21)]
        public bool EnableLeveling { get; set; }

        [Display("Leveling Receiver", Group: "Receiver Leveling Setup", Order: 22)]
        public string LevelingReceiver { get; set; }

        [Display("Leveling Type", Group: "Receiver Leveling Setup", Order: 23)]
        public ReceiverLevelingTypeEnum receiverLevelingType { get; set; }



        [Display("Max Power", Group: "Controlled Source", Order: 30)]
        public double MaxPower { get; set; }

        [Display("Min Power", Group: "Controlled Source", Order: 31)]
        public double MinPower { get; set; }

        [Display("Enable Safe Mode Leveling", Group: "Controlled Source", Order: 32)]
        public bool EnableSafeModeLeveling { get; set; }

        [EnabledIf("EnableSafeModeLeveling", true, HideIfDisabled = false)]
        [Display("Max Step Size", Group: "Controlled Source", Order: 33)]
        public double SafeMaxStepSize { get; set; }

        [Display("Update Source Power Calibration with Leveling Data", Group: "Controlled Source", Order: 34)]
        public bool EnableUpdateSourcePowerCalibration { get; set; }

        [Display("Source ALC Hardware", Group: "Controlled Source", Order: 35)]
        public SourceLevelingModeType sourceLevelingMode { get; set; }


        [Display("Leveling Tolerance", Group: "Leveling Receiver", Order: 40)]
        public double LevelingTolerance { get; set; }

        [Display("Leveling Max Iterations", Group: "Leveling Receiver", Order: 41)]
        public int LevelingMaxIterations { get; set; }

        [Display("Leveling Receiver Frequency", Group: "Leveling Receiver", Order: 42)]
        public ReceiverLevelingFTypeEnum receiverLevelingFType { get; set; }

        [Display("Leveling IFBW", Group: "Leveling Receiver", Order: 43)]
        public bool LevelingIFBW { get; set; }

        [Display("IFBW", Group: "Leveling Receiver", Order: 44)]
        public ReceiverLevelingIFBWEnum receiverLevelingIFBW { get; set; }


        #endregion

        public ReceiverLevelingSource()
        {
            EnableLeveling = false;
            receiverLevelingType = ReceiverLevelingTypeEnum.Presweep;
            MaxPower = 30.0;
            MinPower = -120;
            EnableSafeModeLeveling = false;
            SafeMaxStepSize = 1;
            EnableUpdateSourcePowerCalibration = false;
            sourceLevelingMode = SourceLevelingModeType.INTernal;

            LevelingTolerance = 0.1;
            LevelingMaxIterations = 10;
            receiverLevelingFType = ReceiverLevelingFTypeEnum.Auto;
            LevelingIFBW = false;
            receiverLevelingIFBW = ReceiverLevelingIFBWEnum.IFBW_100k;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetReferenceReceiver(Channel, ControlledSource, LevelingReceiver);
            PNAX.ReceiverLevelingType(Channel, ControlledSource, receiverLevelingType);

            PNAX.ReceiverLevelingMaxPower(Channel, ControlledSource, MaxPower);
            PNAX.ReceiverLevelingMinPower(Channel, ControlledSource, MinPower);
            PNAX.ReceiverLevelingEnableSafeMode(Channel, ControlledSource, EnableSafeModeLeveling);
            PNAX.ReceiverLevelingSafeModeStepPowerLevel(Channel, ControlledSource, SafeMaxStepSize);
            PNAX.ReceiverLevelingUpdateSourcePowerCal(Channel, ControlledSource, EnableUpdateSourcePowerCalibration);
            PNAX.ReceiverLevelingSourceALC(Channel, ControlledSource, sourceLevelingMode);

            PNAX.ReceiverLevelingTolerance(Channel, ControlledSource, LevelingTolerance);
            PNAX.ReceiverLevelingMaxIterations(Channel, ControlledSource, LevelingMaxIterations);
            //PNAX.ReceiverLevelingMaxIterationsEnable(Channel, ControlledSource, true);    
            PNAX.ReceiverLevelingFrequency(Channel, ControlledSource, receiverLevelingFType);
            PNAX.ReceiverLevelingIFBW(Channel, ControlledSource, LevelingIFBW);
            PNAX.ReceiverIFBW(Channel, ControlledSource, receiverLevelingIFBW);

            // Last enable receiver leveling
            PNAX.ReceiverLevelingState(Channel, ControlledSource, EnableLeveling);

            UpgradeVerdict(Verdict.Pass);
        }

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            List<(string, object)> retVal = new List<(string, object)>();

            retVal.Add(($"ControlledSource", ControlledSource));

            retVal.Add(($"{ControlledSource} EnableLeveling", EnableLeveling));
            retVal.Add(($"{ControlledSource} LevelingReceiver", LevelingReceiver));
            retVal.Add(($"{ControlledSource} LevelingType", receiverLevelingType));

            retVal.Add(($"{ControlledSource} MaxPower", MaxPower));
            retVal.Add(($"{ControlledSource} MinPower", MinPower));
            retVal.Add(($"{ControlledSource} EnableSafeModeLeveling", EnableSafeModeLeveling));
            retVal.Add(($"{ControlledSource} SafeMaxStepSize", SafeMaxStepSize));
            retVal.Add(($"{ControlledSource} UpdateSourcePowerCalibration", EnableUpdateSourcePowerCalibration));
            retVal.Add(($"{ControlledSource} SourceALCHardware", sourceLevelingMode));

            retVal.Add(($"{ControlledSource} {LevelingReceiver} LevelingTolerance", LevelingTolerance));
            retVal.Add(($"{ControlledSource} {LevelingReceiver} LevelingMaxIterations", LevelingMaxIterations));
            retVal.Add(($"{ControlledSource} {LevelingReceiver} receiverLevelingFrequency", receiverLevelingFType));
            retVal.Add(($"{ControlledSource} {LevelingReceiver} LevelingIFBW", LevelingIFBW));
            retVal.Add(($"{ControlledSource} {LevelingReceiver} ReceiverLevelingIFBW", receiverLevelingIFBW));

            return retVal;
        }

    }
}
