// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using OpenTap;

namespace OpenTap.Plugins.PNAX
{
    [Browsable(false)]
    [AllowAnyChild]
    [Display(
        "Converter Channel",
        Groups: new[] { "Converters" },
        Description: "Insert a description here"
    )]
    public class ConverterChannelBaseStep : PNABaseStep
    {
        #region SettingsIsConverterEditable
        private LOEnum _portLO1;

        [Browsable(false)]
        public LOEnum PortLO1
        {
            get { return _portLO1; }
            set
            {
                _portLO1 = value;
                UpdateMixerPortLO1();
            }
        }
        private LOEnum _portLO2;

        [Browsable(false)]
        public LOEnum PortLO2
        {
            get { return _portLO2; }
            set
            {
                _portLO2 = value;
                UpdateMixerPortLO2();
            }
        }

        [Browsable(false)]
        public bool IsSweepPointEditable { get; set; }
        private int _SweepPoints;

        [EnabledIf("IsSweepPointEditable", HideIfDisabled = false)]
        [Display("Sweep Points", Order: 20)]
        public int SweepPoints
        {
            get { return _SweepPoints; }
            set
            {
                _SweepPoints = value;
                foreach (var a in this.ChildTestSteps)
                {
                    if (a is MixerSetupTestStep)
                    {
                        (a as MixerSetupTestStep).SweepPoints = value;
                    }
                }
            }
        }

        #endregion

        protected void UpdateNumberOfPoints()
        {
            foreach (var a in this.ChildTestSteps)
            {
                if (a is ToneFrequency)
                {
                    SweepPoints = (a as ToneFrequency).SweepFcNumberOfPoints;
                    return;
                }
                else if (a is GainCompressionFrequency)
                {
                    SweepPoints = (a as GainCompressionFrequency).SweepSettingsNumberOfPoints;
                    return;
                }
                else if (a is NoiseFigureFrequency)
                {
                    SweepPoints = (a as NoiseFigureFrequency).SweepSettingsNumberOfPoints;
                    return;
                }
                else if (a is ScalarMixerSweep)
                {
                    SweepPoints = (a as ScalarMixerSweep).NumberOfPoints;
                    return;
                }
            }
        }

        private void UpdateMixerPortLO1()
        {
            foreach (TestStep step in ChildTestSteps)
            {
                if (step is MixerPowerTestStep)
                {
                    (step as MixerPowerTestStep).PortLO1 = PortLO1;
                }
            }
        }

        private void UpdateMixerPortLO2()
        {
            foreach (TestStep step in ChildTestSteps)
            {
                if (step is MixerPowerTestStep)
                {
                    (step as MixerPowerTestStep).PortLO2 = PortLO2;
                }
            }
        }

        public ConverterChannelBaseStep()
        {
            MetaData = new List<(string, object)>();
            Channel = 1;
            var defaultValues = PNAX.GetMixerSetupDefaultValues();
            ConverterStages = defaultValues.ConverterStages;
            IsConverter = true;
            IsControlledByParent = false;
        }

        public override void Run()
        {
            UpdateMetaData();
        }
    }
}
