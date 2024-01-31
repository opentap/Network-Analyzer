// Author: MyName
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

namespace OpenTap.Plugins.PNAX
{
    
    [AllowAsChildIn(typeof(TestPlan))]
    [AllowAsChildIn(typeof(MODChannel))]
    [Display("Modulation", Groups: new[] { "Network Analyzer", "General", "Modulation Distortion" }, Description: "Insert a description here")]
    public class MODModulate : PNABaseStep
    {
        #region Settings

        [Display("Source", Group: "Settings", Order: 10)]
        public String Source { get; set; }

        [Display("Modulation File", Group: "Settings", Order: 11)]
        public String ModulationFile { get; set; }

        [Display("Enable Modulation", Group: "Settings", Order: 12)]
        public bool EnableModulation { get; set; }

        [Display("Source Correction", Group: "Settings", Order: 13)]
        public MODSourceCorrectionEnum ModSourceCorrection { get; set; }

        [Display("Enable Cal", Groups: new[] { "Modulation Cal", "Power" }, Order: 21)]
        public bool PowerEnableCal { get; set; }

        [EnabledIf("PowerEnableCal", true, HideIfDisabled = true)]
        [Display("Cal Port", Groups: new[] { "Modulation Cal", "Power" }, Order: 22)]
        public MODCalPortEnum PowerCalPort { get; set; }

        [EnabledIf("PowerEnableCal", true, HideIfDisabled = true)]
        [Display("Cal Span", Groups: new[] { "Modulation Cal", "Power" }, Order: 23)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double PowerCalSpan { get; set; }

        [EnabledIf("PowerEnableCal", true, HideIfDisabled = true)]
        [Display("Max Iterations", Groups: new[] { "Modulation Cal", "Power" }, Order: 24)]
        public int PowerMaxIterations { get; set; }

        [EnabledIf("PowerEnableCal", true, HideIfDisabled = true)]
        [Display("Desired Tolerance", Groups: new[] { "Modulation Cal", "Power" }, Order: 25)]
        [Unit("dB", UseEngineeringPrefix: false, StringFormat: "0.00")]
        public double PowerDesiredTolerance { get; set; }


        [Display("Enable Cal", Groups: new[] { "Modulation Cal", "Equalization" }, Order: 31)]
        public bool EqualizationEnableCal { get; set; }

        [EnabledIf("EqualizationEnableCal", true, HideIfDisabled = true)]
        [Display("Cal Port", Groups: new[] { "Modulation Cal", "Equalization" }, Order: 32)]
        public MODCalPortEnum EqualizationCalPort { get; set; }

        [EnabledIf("EqualizationEnableCal", true, HideIfDisabled = true)]
        [Display("Cal Span", Groups: new[] { "Modulation Cal", "Equalization" }, Order: 33)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double EqualizationCalSpan { get; set; }

        [EnabledIf("EqualizationEnableCal", true, HideIfDisabled = true)]
        [Display("Max Iterations", Groups: new[] { "Modulation Cal", "Equalization" }, Order: 34)]
        public int EqualizationMaxIterations { get; set; }

        [EnabledIf("EqualizationEnableCal", true, HideIfDisabled = true)]
        [Display("Desired Tolerance", Groups: new[] { "Modulation Cal", "Equalization" }, Order: 35)]
        [Unit("dB-pk", UseEngineeringPrefix: false, StringFormat: "0.00")]
        public double EqualizationDesiredTolerance { get; set; }



        [Display("Enable Cal", Groups: new[] { "Modulation Cal", "Distortion" }, Order: 41)]
        public bool DistortionEnableCal { get; set; }

        [EnabledIf("DistortionEnableCal", true, HideIfDisabled = true)]
        [Display("Cal Port", Groups: new[] { "Modulation Cal", "Distortion" }, Order: 42)]
        public MODCalPortEnum DistortionCalPort { get; set; }

        [EnabledIf("DistortionEnableCal", true, HideIfDisabled = true)]
        [Display("Cal Span", Groups: new[] { "Modulation Cal", "Distortion" }, Order: 43)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double DistortionCalSpan { get; set; }

        [EnabledIf("DistortionEnableCal", true, HideIfDisabled = true)]
        [Display("Max Iterations", Groups: new[] { "Modulation Cal", "Distortion" }, Order: 44)]
        public int DistortionMaxIterations { get; set; }

        [EnabledIf("DistortionEnableCal", true, HideIfDisabled = true)]
        [Display("Desired Tolerance", Groups: new[] { "Modulation Cal", "Distortion" }, Order: 45)]
        [Unit("dBc", UseEngineeringPrefix: false, StringFormat: "0.00")]
        public double DistortionDesiredTolerance { get; set; }



        [Display("Enable Cal", Groups: new[] { "Modulation Cal", "ACP Lower" }, Order: 51)]
        public bool ACPLowerEnableCal { get; set; }

        [EnabledIf("ACPLowerEnableCal", true, HideIfDisabled = true)]
        [Display("Cal Port", Groups: new[] { "Modulation Cal", "ACP Lower" }, Order: 52)]
        public MODCalPortEnum ACPLowerCalPort { get; set; }

        [EnabledIf("ACPLowerEnableCal", true, HideIfDisabled = true)]
        [Display("Cal Span", Groups: new[] { "Modulation Cal", "ACP Lower" }, Order: 53)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double ACPLowerCalSpan { get; set; }

        [EnabledIf("ACPLowerEnableCal", true, HideIfDisabled = true)]
        [Display("Guard Band", Groups: new[] { "Modulation Cal", "ACP Lower" }, Order: 53.5)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double ACPLowerGuardBand { get; set; }

        [EnabledIf("ACPLowerEnableCal", true, HideIfDisabled = true)]
        [Display("Max Iterations", Groups: new[] { "Modulation Cal", "ACP Lower" }, Order: 54)]
        public int ACPLowerMaxIterations { get; set; }

        [EnabledIf("ACPLowerEnableCal", true, HideIfDisabled = true)]
        [Display("Desired Tolerance", Groups: new[] { "Modulation Cal", "ACP Lower" }, Order: 55)]
        [Unit("dBc", UseEngineeringPrefix: false, StringFormat: "0.00")]
        public double ACPLowerDesiredTolerance { get; set; }



        [Display("Enable Cal", Groups: new[] { "Modulation Cal", "ACP Upper" }, Order: 61)]
        public bool ACPUpperEnableCal { get; set; }

        [EnabledIf("ACPUpperEnableCal", true, HideIfDisabled = true)]
        [Display("Cal Port", Groups: new[] { "Modulation Cal", "ACP Upper" }, Order: 62)]
        public MODCalPortEnum ACPUpperCalPort { get; set; }

        [EnabledIf("ACPUpperEnableCal", true, HideIfDisabled = true)]
        [Display("Cal Span", Groups: new[] { "Modulation Cal", "ACP Upper" }, Order: 63)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double ACPUpperCalSpan { get; set; }

        [EnabledIf("ACPUpperEnableCal", true, HideIfDisabled = true)]
        [Display("Guard Band", Groups: new[] { "Modulation Cal", "ACP Upper" }, Order: 53.5)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double ACPUpperGuardBand { get; set; }

        [EnabledIf("ACPUpperEnableCal", true, HideIfDisabled = true)]
        [Display("Max Iterations", Groups: new[] { "Modulation Cal", "ACP Upper" }, Order: 64)]
        public int ACPUpperMaxIterations { get; set; }

        [EnabledIf("ACPUpperEnableCal", true, HideIfDisabled = true)]
        [Display("Desired Tolerance", Groups: new[] { "Modulation Cal", "ACP Upper" }, Order: 65)]
        [Unit("dBc", UseEngineeringPrefix: false, StringFormat: "0.00")]
        public double ACPUpperDesiredTolerance { get; set; }
        #endregion

        public MODModulate()
        {
            Source = "Port 1";
            ModulationFile = @"D:\data\d.mdx";
            EnableModulation = false;
            ModSourceCorrection = MODSourceCorrectionEnum.OFF;

            PowerEnableCal = true;
            PowerCalPort = MODCalPortEnum.DUTIn1;
            PowerCalSpan = 135.166667e6;
            PowerMaxIterations = 3;
            PowerDesiredTolerance = 0.1;

            EqualizationEnableCal = true;
            EqualizationCalPort = MODCalPortEnum.DUTIn1;
            EqualizationCalSpan = 135.166667e6;
            EqualizationMaxIterations = 3;
            EqualizationDesiredTolerance = 0.3;

            DistortionEnableCal = false;
            DistortionCalPort = MODCalPortEnum.DUTIn1;
            DistortionCalSpan = 135.166667e6;
            DistortionMaxIterations = 3;
            DistortionDesiredTolerance = -40;

            ACPLowerEnableCal = false;
            ACPLowerCalPort = MODCalPortEnum.DUTIn1;
            ACPLowerCalSpan = 16.916667e6;
            ACPLowerGuardBand = 0;
            ACPLowerMaxIterations = 2;
            ACPLowerDesiredTolerance = -40;

            ACPUpperEnableCal = false;
            ACPUpperCalPort = MODCalPortEnum.DUTIn1;
            ACPUpperCalSpan = 16.916667e6;
            ACPUpperGuardBand = 0;
            ACPUpperMaxIterations = 2;
            ACPUpperDesiredTolerance = -40;



        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetMODSource(Channel, Source);
            PNAX.MODLoadFile(Channel, Source, ModulationFile);
            PNAX.MODEnableModulation(Channel, Source, EnableModulation);
            PNAX.WaitForOperationComplete();


            PNAX.WaitForOperationComplete();

            PNAX.MODCalEnable(Channel, Source, PowerEnableCal, MODCalTypeEnum.POWer);
            if (PowerEnableCal)
            {
                PNAX.MODCalPort(Channel, Source, PowerCalPort, MODCalTypeEnum.POWer);
                PNAX.MODCalSpan(Channel, Source, PowerCalSpan, MODCalTypeEnum.POWer);
                PNAX.MODCalMaxIterations(Channel, Source, PowerMaxIterations, MODCalTypeEnum.POWer);
                PNAX.MODCalDesiredTolearance(Channel, Source, PowerDesiredTolerance, MODCalTypeEnum.POWer);
            }

            PNAX.MODCalEnable(Channel, Source, EqualizationEnableCal, MODCalTypeEnum.EQUalization);
            if (EqualizationEnableCal)
            {
                PNAX.MODCalPort(Channel, Source, EqualizationCalPort, MODCalTypeEnum.EQUalization);
                PNAX.MODCalSpan(Channel, Source, EqualizationCalSpan, MODCalTypeEnum.EQUalization);
                PNAX.MODCalMaxIterations(Channel, Source, EqualizationMaxIterations, MODCalTypeEnum.EQUalization);
                PNAX.MODCalDesiredTolearance(Channel, Source, EqualizationDesiredTolerance, MODCalTypeEnum.EQUalization);
            }

            PNAX.MODCalEnable(Channel, Source, DistortionEnableCal, MODCalTypeEnum.DISTortion);
            if (DistortionEnableCal)
            {
                PNAX.MODCalPort(Channel, Source, DistortionCalPort, MODCalTypeEnum.DISTortion);
                PNAX.MODCalSpan(Channel, Source, DistortionCalSpan, MODCalTypeEnum.DISTortion);
                PNAX.MODCalMaxIterations(Channel, Source, DistortionMaxIterations, MODCalTypeEnum.DISTortion);
                PNAX.MODCalDesiredTolearance(Channel, Source, DistortionDesiredTolerance, MODCalTypeEnum.DISTortion);
            }

            PNAX.MODCalEnable(Channel, Source, ACPLowerEnableCal, MODCalTypeEnum.ACPLower);
            if (ACPLowerEnableCal)
            {
                PNAX.MODCalPort(Channel, Source, ACPLowerCalPort, MODCalTypeEnum.ACPLower);
                PNAX.MODCalSpan(Channel, Source, ACPLowerCalSpan, MODCalTypeEnum.ACPLower);
                PNAX.MODCalGuardBand(Channel, Source, ACPLowerGuardBand, MODCalTypeEnum.ACPLower);
                PNAX.MODCalMaxIterations(Channel, Source, ACPLowerMaxIterations, MODCalTypeEnum.ACPLower);
                PNAX.MODCalDesiredTolearance(Channel, Source, ACPLowerDesiredTolerance, MODCalTypeEnum.ACPLower);
            }

            PNAX.MODCalEnable(Channel, Source, ACPUpperEnableCal, MODCalTypeEnum.ACPUpper);
            if (ACPUpperEnableCal)
            {
                PNAX.MODCalPort(Channel, Source, ACPUpperCalPort, MODCalTypeEnum.ACPUpper);
                PNAX.MODCalSpan(Channel, Source, ACPUpperCalSpan, MODCalTypeEnum.ACPUpper);
                PNAX.MODCalGuardBand(Channel, Source, ACPUpperGuardBand, MODCalTypeEnum.ACPUpper);
                PNAX.MODCalMaxIterations(Channel, Source, ACPUpperMaxIterations, MODCalTypeEnum.ACPUpper);
                PNAX.MODCalDesiredTolearance(Channel, Source, ACPUpperDesiredTolerance, MODCalTypeEnum.ACPUpper);
            }


            PNAX.WaitForOperationComplete();

            // Execute Calibration
            PNAX.MODCalibrationMeasure(Channel, Source);
            //PNAX.WaitForOperationComplete(60000);
            // Calibration details returns multiple single responses, using SCPIQuery leaves multiple lines on the queue
            // Better not to use Calibration Details for now
            //Log.Info("MOD Calibration Details: " + PNAX.MODGetCalibrationDetails(Channel, Source)); 
            Log.Info("MOD Calibration Status: " + PNAX.MODGetCalibrationStatus(Channel, Source));

            // Source Correction
            PNAX.MODSourceCorrection(Channel, Source, ModSourceCorrection);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
