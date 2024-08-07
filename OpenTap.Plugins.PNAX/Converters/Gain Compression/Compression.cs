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


    //[AllowAsChildIn(typeof(GainCompressionChannel))]
    [Display("Compression", Groups: new[] { "Network Analyzer", "Converters", "Gain Compression Converters" }, Description: "Insert a description here")]
    public class Compression : CompressionBaseStep
    {

        #region Settings
        #endregion

        public Compression()
        {
            IsConverter = true;
        }

    }
}
