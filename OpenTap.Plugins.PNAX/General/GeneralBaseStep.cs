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
    public class GeneralBaseStep : SingleTraceBaseStep
    {
        #region Settings
        #endregion

        #region Segment Sweep
        [Browsable(false)]
        public bool EnableSegmentSweepSettings { get; set; } = false;

        //[EnabledIf("StandardSweepType", StandardSweepTypeEnum.SegmentSweep, HideIfDisabled = true)]
        [EnabledIf("EnableSegmentSweepSettings", true, HideIfDisabled = true)]
        [Display("Segment Definition Type", Group: "Sweep Properties", Order: 30)]
        public SegmentDefinitionTypeEnum SegmentDefinitionType { get; set; }

        //[EnabledIf("StandardSweepType", StandardSweepTypeEnum.SegmentSweep, HideIfDisabled = true)]
        [EnabledIf("EnableSegmentSweepSettings", true, HideIfDisabled = true)]
        [EnabledIf("SegmentDefinitionType", SegmentDefinitionTypeEnum.File, HideIfDisabled = false)]
        [Display("Segment Table File Name", Group: "Sweep Properties", Order: 31)]
        [FilePath]
        public string SegmentTable { get; set; }

        //[EnabledIf("StandardSweepType", StandardSweepTypeEnum.SegmentSweep, HideIfDisabled = true)]
        [EnabledIf("EnableSegmentSweepSettings", true, HideIfDisabled = true)]
        [EnabledIf("SegmentDefinitionType", SegmentDefinitionTypeEnum.List, HideIfDisabled = false)]
        [Display("Segment Table", Group: "Sweep Properties", Order: 32)]
        public List<SegmentDefinition> segmentDefinitions { get; set; }

        //[EnabledIf("StandardSweepType", StandardSweepTypeEnum.SegmentSweep, HideIfDisabled = true)]
        [EnabledIf("EnableSegmentSweepSettings", true, HideIfDisabled = true)]
        [EnabledIf("SegmentDefinitionType", SegmentDefinitionTypeEnum.List, HideIfDisabled = false)]
        [Display("Show Table", Group: "Sweep Properties", Order: 33)]
        public bool ShowTable { get; set; }


        #endregion

        public GeneralBaseStep()
        {
            SegmentDefinitionType = SegmentDefinitionTypeEnum.List;
            segmentDefinitions = new List<SegmentDefinition>
            {
                new SegmentDefinition { state = true, NumberOfPoints = 21, StartFrequency = 10.5e6, StopFrequency = 1e9 }
            };
            ShowTable = false;
            Window = 1;
        }

        public override void Run()
        {
        }
    }
}
