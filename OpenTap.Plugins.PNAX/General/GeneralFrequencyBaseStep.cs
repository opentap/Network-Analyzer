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

    [Browsable(false)]
    public class GeneralFrequencyBaseStep : GeneralBaseStep
    {
        #region Settings


        private int _SweepSettingsNumberOfPoints;
        [Display("Number Of Points", Group: "Sweep Settings", Order: 10)]
        public int SweepSettingsNumberOfPoints
        {
            get
            {
                return _SweepSettingsNumberOfPoints;
            }
            set
            {
                _SweepSettingsNumberOfPoints = value;
                // Update Points on Parent step
                try
                {
                    var a = GetParent<ConverterChannelBase>();
                    // only if there is a parent of type ScalarMixerChannel
                    if (a != null)
                    {
                        a.SweepPoints = _SweepSettingsNumberOfPoints;
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("can't find parent yet! ex: " + ex.Message);
                }

            }
        }

        [Display("IF Bandwidth", Group: "Sweep Settings", Order: 11)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double SweepSettingsIFBandwidth { get; set; }

        [EnabledIf("SweepType", SweepTypeEnum.LinearSweep, HideIfDisabled =true)]
        [Display("Start", Group: "Sweep Settings", Order: 12)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double SweepSettingsStart { get; set; }

        [EnabledIf("SweepType", SweepTypeEnum.LinearSweep, HideIfDisabled = true)]
        [Display("Stop", Group: "Sweep Settings", Order: 13)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsStop { get; set; }

        [EnabledIf("SweepType", SweepTypeEnum.LinearSweep, HideIfDisabled = true)]
        [Display("Center", Group: "Sweep Settings", Order: 14)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsCenter { get; set; }

        [EnabledIf("SweepType", SweepTypeEnum.LinearSweep, HideIfDisabled = true)]
        [Display("Span", Group: "Sweep Settings", Order: 15)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsSpan { get; set; }

        [EnabledIf("SweepType", SweepTypeEnum.CWFrequency, HideIfDisabled = true)]
        [Display("Fixed", Group: "Sweep Settings", Order: 16)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double SweepSettingsFixed { get; set; }

        #endregion

        public GeneralFrequencyBaseStep()
        {
        }



        public override void Run()
        {
        }
    }
}
