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
    [AllowAnyChild]
    [Display("Converter Channel", Groups: new[] { "Converters" }, Description: "Insert a description here")]
    public class ConverterChannelBase : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        private int _channel;
        [Display("Channel", Order: 1)]
        public int Channel
        {
            get { return _channel; }
            set
            {
                _channel = value;
                foreach (var a in this.ChildTestSteps)
                {
                    if (a is ConverterBaseStep)
                    {
                        (a as ConverterBaseStep).Channel = value;
                    }
                }
            }
        }
        [Browsable(false)]
        public bool DisabledInChannelParentStep { get; set; } = false;

        private ConverterStagesEnum _ConverterStagesEnum;
        [Display("Converter Stages", Order: 10, Description:"Stage is defined in Mixer Setup Test Step")]
        [EnabledIf("DisabledInChannelParentStep", HideIfDisabled =false)]
        public ConverterStagesEnum ConverterStages
        {
            get
            {
                return _ConverterStagesEnum;
            }
            set
            {
                _ConverterStagesEnum = value;

                if (this.ChildTestSteps != null)
                {
                    // Update children
                    foreach (var a in this.ChildTestSteps)
                    {
                        if (a is ConverterBaseStep && !(a is MixerSetupTestStep) )
                        {
                            (a as ConverterBaseStep).ConverterStages = _ConverterStagesEnum;
                        }
                    }
                }

            }
        }

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

        private int _SweepPoints;
        [EnabledIf("DisabledInChannelParentStep", HideIfDisabled = false)]
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

        public ConverterChannelBase()
        {
            Channel = 1;
            var defaultValues = PNAX.GetMixerSetupDefaultValues();
            ConverterStages = defaultValues.ConverterStages;
        }

        public override void Run()
        {

        }
    }
}
