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
    public class ConverterCompressionBaseStep : TestStep
    {
        #region Settings
        [Browsable(false)]
        public bool IsControlledByParent { get; set; } = false;
        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
        [Display("Channel", Order: 1)]
        public virtual int Channel { get; set; }

        [Browsable(false)]
        public bool DoubleStage { get; set; }


        private void UpdateConverterStages()
        {
            if (_ConverterStagesEnum == ConverterStagesEnum._2)
            {
                DoubleStage = true;
            }
            else
            {
                DoubleStage = false;
            }
        }

        protected ConverterStagesEnum _ConverterStagesEnum;
        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
        [Display("Converter Stages", Order: 10)]
        public ConverterStagesEnum ConverterStages
        {
            get
            {
                try
                {
                    _ConverterStagesEnum = GetParent<ConverterChannelBase>().ConverterStages;
                }
                catch (Exception ex)
                {
                    Log.Info(ex.Message);
                }
                UpdateConverterStages();
                return _ConverterStagesEnum;
            }
            set
            {
                _ConverterStagesEnum = value;
            }
        }

        #endregion



        public ConverterCompressionBaseStep()
        {
        }

        public override void Run()
        {
        }
    }
}
