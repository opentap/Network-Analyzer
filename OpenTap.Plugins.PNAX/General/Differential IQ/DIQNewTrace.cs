// Author: CMontes
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
    public enum DIQTraceEnum
    {
        IPwrF1,
        OPwrF1,
        GainF1,
    }

    [AllowAsChildIn(typeof(DIQChannel))]
    [AllowChildrenOfType(typeof(DIQSingleTrace))]
    [Display("DIQ New Trace", Groups: new[] { "Network Analyzer", "General", "Differential I/Q" }, Description: "Insert a description here")]
    public class DIQNewTrace : AddNewTraceBaseStep
    {
        #region Settings
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public DIQTraceEnum Meas { get; set; }

        [Display("Meas", Groups: new[] { "New Trace" }, Order: 21, Description: "Do not use underscores in the parameter name. For example, b2_f1 cannot be used as a parameter name. However, b2f1 is a valid parameter name.")]
        public string NewMeas { get; set; }

        [Display("Expression", Groups: new[] { "New Trace" }, Order: 22)]
        public new string Expression { get; set; }
        #endregion

        public DIQNewTrace()
        {
            Meas = DIQTraceEnum.IPwrF1;
            ChildTestSteps.Add(new DIQSingleTrace() { PNAX = this.PNAX, Meas = this.Meas, Channel = this.Channel, IsControlledByParent = true, EnableTraceSettings = true });
            NewMeas = "NewDIQTrace";
            Expression = "(a1_F1*b1_F1)/(a2_F1*b2_F1)";
            Rules.Add(() => ((NewMeas.Contains("_") == false)), "Parameter name can not include underscore", nameof(NewMeas));
        }


        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            List<(string, object)> retVal = new List<(string, object)>();

            return retVal;
        }

        protected override void DeleteDummyTrace()
        {
            PNAX.ScpiCommand($"CALCulate{Channel}:PARameter:DELete \'CH{Channel}_DUMMY_1\'");
        }

        [Display("Add New Trace", Groups: new[] { "Trace" }, Order: 12)]
        protected override void AddNewTrace()
        {
            ChildTestSteps.Add(new DIQSingleTrace() { PNAX = this.PNAX, Meas = this.Meas, Channel = this.Channel, IsControlledByParent = true, EnableTraceSettings = true });
        }

        [Browsable(true)]
        [Display("Define New Trace", Groups: new[] { "New Trace" }, Order: 23)]
        public void AddNewCustomTrace()
        {
            ChildTestSteps.Add(new DIQSingleTrace() { PNAX = this.PNAX, CustomMeas = NewMeas, Channel = this.Channel, IsControlledByParent = true, EnableTraceSettings = true, Expression = this.Expression, CustomTraceMeas = true });
        }
    }
}
