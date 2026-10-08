// Author: CMontes
// Copyright:   Copyright 2024 Keysight Technologies
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
    [Display("Modulation Distortion Converters Channel", Groups: new[] { "Network Analyzer", "Converters", "Modulation Distortion" }, Description: "Insert a description here")]
    public class MODXChannel : MODChannelBaseStep
    {
        protected override string DummyMeasurementClass => "Modulation Distortion Converters";

        public MODXChannel()
        {
            AddModChildSteps(new MODXNewTrace(), new MODXMixer());
        }
    }
}
