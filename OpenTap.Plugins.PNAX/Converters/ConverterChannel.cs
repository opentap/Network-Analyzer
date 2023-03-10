﻿// Author: MyName
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

        private int _Channel;
        [Display("Channel", Order: 1)]
        public int Channel
        {
            set
            {
                _Channel = value;
            }
            get
            {
                return _Channel;
            }
        }

        private ConverterStagesEnum _ConverterStagesEnum;
        [Display("Converter Stages", Order: 10)]
        public ConverterStagesEnum ConverterStages
        {
            get
            {
                return _ConverterStagesEnum;
            }
            set
            {
                _ConverterStagesEnum = value;
            }
        }

        #endregion



        public ConverterChannelBase()
        {
            Channel = 1;
            ConverterStages = GeneralStandardSettings.Current.ConverterStages;
        }

        public override void Run()
        {

        }
    }
}
