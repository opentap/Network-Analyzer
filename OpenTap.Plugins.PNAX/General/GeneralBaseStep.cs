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
    public class GeneralBaseStep : SingleTraceBaseStep
    {
        #region Settings
        [Browsable(false)]
        public bool IsControlledByParent { get; set; } = false;
        private PNAX _PNAX;
        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
        [Display("PNA", Order: 0.1)]
        public virtual PNAX PNAX
        {
            get
            {
                return _PNAX;
            }
            set
            {
                _PNAX = value;

                // Update traces
                foreach (var a in this.ChildTestSteps)
                {
                    if (a is GeneralSingleTraceBaseStep)
                    {
                        (a as GeneralSingleTraceBaseStep).PNAX = value;
                    }
                    if (a is GeneralBaseStep)
                    {
                        (a as GeneralBaseStep).PNAX = value;
                    }
                }
            }
        }

        private int _Channel;
        [EnabledIf("IsControlledByParent", false, HideIfDisabled = false)]
        [Display("Channel", Order: 1)]
        public virtual int Channel
        {
            get
            {
                return _Channel;
            }
            set
            {
                _Channel = value;

                // Update traces
                foreach (var a in this.ChildTestSteps)
                {
                    if (a is GeneralSingleTraceBaseStep)
                    {
                        (a as GeneralSingleTraceBaseStep).Channel = value;
                    }
                    if (a is GeneralBaseStep)
                    {
                        (a as GeneralBaseStep).Channel = value;
                    }
                }
            }
        }
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

        //[EnabledIf("StandardSweepType", StandardSweepTypeEnum.SegmentSweep, HideIfDisabled = true)]
        //[EnabledIf("EnableSegmentSweepSettings", true, HideIfDisabled = true)]
        //[EnabledIf("SegmentDefinitionType", SegmentDefinitionTypeEnum.List, HideIfDisabled = false)]
        //[EnabledIf("ShowTable", true, HideIfDisabled = true)]
        //[Display("Window", Group: "Sweep Properties", Order: 34)]
        //public new int Window { get; set; }

        #endregion

        public void SetSegmentValues()
        {
            if (SegmentDefinitionType == SegmentDefinitionTypeEnum.File)
            {
                Log.Error("Load file Not implemented!");
            }
            else
            {
                PNAX.SegmentDeleteAllSegments(Channel);
                int segment = 0;
                foreach (SegmentDefinition a in segmentDefinitions)
                {
                    segment = PNAX.SegmentAdd(Channel);
                    PNAX.SetSegmentState(Channel, segment, a.state);
                    PNAX.SetSegmentNumberOfPoints(Channel, segment, a.NumberOfPoints);
                    PNAX.SetSegmentStartFrequency(Channel, segment, a.StartFrequency);
                    PNAX.SetSegmentStopFrequency(Channel, segment, a.StopFrequency);
                }
                PNAX.SetStandardSweepType(Channel, StandardSweepTypeEnum.SegmentSweep);
                if (ShowTable)
                {
                    PNAX.SetSegmentTableShow(Channel, true, Window);
                }
            }

        }

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
