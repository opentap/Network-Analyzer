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
    public enum XAxisAnnotation
    {
        [Display("Output")]
        Output,
        [Display("Input")]
        Input
    }

    [AllowAsChildIn(typeof(NoiseFigureChannel))]
    [Display("Noise Figure Frequency", Groups: new[] { "PNA-X", "Converters", "Noise Figure Converters" }, Description: "Insert a description here")]
    public class NoiseFigureFrequency : FrequencyBaseStep
    {
        #region Settings
        [Display("X-Axis Annotation", Order: 1)]
        public XAxisAnnotation XAxisAnnotation { get; set; }
        #endregion

        public NoiseFigureFrequency()
        {
            UpdateDefaultValues();
        }

        public void UpdateDefaultValues()
        {
            var DefaultValues = PNAX.GetNoiseFigureConverterFrequencyDefaultValues();
            SweepType                   = DefaultValues.SweepType;

            SweepSettingsNumberOfPoints = DefaultValues.SweepSettingsNumberOfPoints;
            SweepSettingsIFBandwidth    = DefaultValues.SweepSettingsIFBandwidth;
            SweepSettingsStart          = DefaultValues.SweepSettingsStart;
            SweepSettingsStop           = DefaultValues.SweepSettingsStop;
            SweepSettingsCenter         = DefaultValues.SweepSettingsCenter;
            SweepSettingsSpan           = DefaultValues.SweepSettingsSpan;
            SweepSettingsFixed          = DefaultValues.SweepSettingsFixed;

        }
        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetSweepType(Channel, SweepType);

            PNAX.SetPoints(Channel, SweepSettingsNumberOfPoints);
            PNAX.SetIFBandwidth(Channel, SweepSettingsIFBandwidth);

            if (SweepType == SweepTypeEnum.LinearSweep)
            {
                PNAX.SetStart(Channel, SweepSettingsStart);
                PNAX.SetStop(Channel, SweepSettingsStop);
                PNAX.SetCenter(Channel, SweepSettingsCenter);
                PNAX.SetSpan(Channel, SweepSettingsSpan);
            }
            else if (SweepType == SweepTypeEnum.CWFrequency)
            {
                PNAX.SetCWFreq(Channel, SweepSettingsFixed);
            }


            UpgradeVerdict(Verdict.Pass);
        }
    }
}
