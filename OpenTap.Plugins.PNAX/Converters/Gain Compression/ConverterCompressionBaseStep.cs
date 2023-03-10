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
    [Display("ConverterCompressionBaseStep", Group: "OpenTap.Plugins.PNAX.Converters.Gain_Compression", Description: "Insert a description here")]
    public class ConverterCompressionBaseStep : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        private int _Channel;
        [Display("Channel", Order: 1)]
        public int Channel
        {
            //set
            //{
            //    _Channel = value;
            //}
            get
            {
                try
                {
                    _Channel = GetParent<ConverterChannelBase>().Channel;
                }
                catch (Exception ex)
                {
                    Log.Info(ex.Message);
                }

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



        public ConverterCompressionBaseStep()
        {
        }

        public override void Run()
        {
        }
    }
}
