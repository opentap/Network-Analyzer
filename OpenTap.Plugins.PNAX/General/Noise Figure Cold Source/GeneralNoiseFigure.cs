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
    //[AllowAsChildIn(typeof(GeneralNoiseFigureChannel))]
    [Display(
        "Noise Figure",
        Groups: new[] { "Network Analyzer", "General", "Noise Figure Cold Source" },
        Description: "Insert a description here"
    )]
    public class GeneralNoiseFigure : NoiseFigureBaseStep { }
}
