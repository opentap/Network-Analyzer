// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using OpenTap;
using OpenTap.Plugins.PNAX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OpenTap.Plugins.PNAX
{
    [Browsable(false)]
    public class ConverterBaseStep : SingleTraceBaseStep
    {
        #region Settings

        [Browsable(false)]
        public bool DoubleStage { get; set; }

        [Browsable(false)]
        public bool IsChildEditable { get; set; } = false;
        private ConverterStagesEnum _ConverterStagesEnum;
        [EnabledIf("IsChildEditable", true, HideIfDisabled = false)]
        [Display("Converter Stages", Order: 10)]
        public override ConverterStagesEnum ConverterStages
        {
            get
            {
                return _ConverterStagesEnum;
            }
            set
            {
                _ConverterStagesEnum = value;
                DoubleStage = _ConverterStagesEnum == ConverterStagesEnum._2;
            }
        }
        #endregion


        public ConverterBaseStep()
        {
            IsConverter = true;
        }

        public override void Run()
        {
        }
    }
}
