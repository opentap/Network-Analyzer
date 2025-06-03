// Author: CMontes
// Copyright:   Copyright 2023-2024 Keysight Technologies
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

//using OpenTap.Plugins.BasicSteps;

namespace OpenTap.Plugins.PNAX
{
    //[AllowAsChildIn(typeof(TestPlan))]
    //[AllowAsChildIn(typeof(SequenceStep))]
    [AllowAnyChild]
    [Browsable(false)]
    public class PNABaseStep : TestStep
    {
        #region Settings

        [Browsable(false)]
        public bool IsControlledByParent { get; set; } = false;
        private PNAX _PNAX;

        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
        [Display("PNA", Order: 0.1)]
        public virtual PNAX PNAX
        {
            get { return _PNAX; }
            set
            {
                _PNAX = value;
                // Update traces
                foreach (var a in ChildTestSteps)
                {
                    if (a.GetType().IsSubclassOf(typeof(PNABaseStep)))
                    {
                        (a as PNABaseStep).PNAX = value;
                    }
                }
            }
        }

        private int _Channel;

        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
        [Display("Channel", Order: 0.11)]
        public virtual int Channel
        {
            get { return _Channel; }
            set
            {
                _Channel = value;

                // Update traces
                foreach (var a in ChildTestSteps)
                {
                    if (a.GetType().IsSubclassOf(typeof(PNABaseStep)))
                    {
                        (a as PNABaseStep).Channel = value;
                    }
                    if (a is SingleTraceBaseStep)
                    {
                        (a as SingleTraceBaseStep).UpdateTestStepName();
                    }
                }
            }
        }

        [Browsable(false)]
        public bool IsConverter { get; set; } = false;

        [Browsable(false)]
        public bool IsConverterEditable { get; set; } = false;
        private ConverterStagesEnum _ConverterStagesEnum;

        [EnabledIf("IsConverter", true, HideIfDisabled = true)]
        [EnabledIf("IsConverterEditable", true, HideIfDisabled = false)]
        [Display("Converter Stages", Order: 0.12)]
        public ConverterStagesEnum ConverterStages
        {
            get { return _ConverterStagesEnum; }
            set
            {
                _ConverterStagesEnum = value;
                DoubleStage = _ConverterStagesEnum == ConverterStagesEnum._2;
                UpdateChanelConverterStage();
                UpdateChildStepConverterStage();
            }
        }

        [Browsable(false)]
        public bool DoubleStage { get; set; }

        [Output]
        [Browsable(false)]
        [Display("MetaData", Groups: new[] { "MetaData" }, Order: 1000.0)]
        public List<(string, object)> MetaData { get; set; }
        #endregion

        public PNABaseStep()
        {
            // ToDo: Set default values for properties / settings.
            Channel = 1;
            ConverterStages = ConverterStagesEnum._1;
        }

        public override void Run()
        {
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            // UpgradeVerdict(Verdict.Pass);
        }

        [Browsable(false)]
        public virtual List<(string, object)> GetMetaData()
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateMetaData()
        {
            MetaData = new List<(string, object)> { ("Channel", Channel) };

            foreach (var ch in this.ChildTestSteps)
            {
                List<(string, object)> ret = (ch as PNABaseStep).GetMetaData();
                foreach (var it in ret)
                {
                    MetaData.Add(it);
                }
            }
        }

        protected virtual void UpdateChanelConverterStage() { }

        private void UpdateChildStepConverterStage()
        {
            foreach (var step in this.ChildTestSteps)
            {
                if (step.GetType().IsSubclassOf(typeof(PNABaseStep)))
                {
                    if (step.GetType().Equals(typeof(MixerSetupTestStep)))
                        continue;

                    (step as PNABaseStep).ConverterStages = _ConverterStagesEnum;
                }
            }
        }
    }
}
