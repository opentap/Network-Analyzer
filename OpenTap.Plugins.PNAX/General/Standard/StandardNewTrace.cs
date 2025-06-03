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
    public class StandardTrace
    {
        public int Channel { get; set; }
        public StandardTraceEnum Meas { get; set; }
        public int Window { get; set; }
        public int Sheet { get; set; }
        public PNAX.MeasurementFormatEnum MeasurementFormat { get; set; }
        public string Title { get; set; }
    }

    //[AllowAsChildIn(typeof(StandardChannel))]
    //[AllowChildrenOfType(typeof(StandardSingleTrace))]
    [Display(
        "Standard New Trace",
        Groups: new[] { "Network Analyzer", "General", "Standard" },
        Description: "Insert a description here"
    )]
    public class StandardNewTrace : AddNewTraceBaseStep
    {
        #region Settings
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public StandardTraceEnum Meas { get; set; }
        #endregion

        public StandardNewTrace()
        {
            Meas = StandardTraceEnum.S11;
            ChildTestSteps.Add(
                new StandardSingleTrace()
                {
                    PNAX = this.PNAX,
                    Meas = this.Meas,
                    Channel = this.Channel,
                    IsControlledByParent = true,
                    EnableTraceSettings = true,
                }
            );
        }

        // overloaded constructor for window and sheet
        public StandardNewTrace(List<StandardTrace> standardTraces)
        {
            if ((standardTraces is null) || (standardTraces.Count == 0))
            {
                // add default trace
                ChildTestSteps.Add(
                    new StandardSingleTrace() { PNAX = this.PNAX, Meas = StandardTraceEnum.S11 }
                );
                return;
            }

            foreach (StandardTrace tr in standardTraces)
            {
                StandardSingleTrace sstr = new StandardSingleTrace()
                {
                    PNAX = this.PNAX,
                    Channel = tr.Channel,
                    Meas = tr.Meas,
                    Window = tr.Window,
                    Sheet = tr.Sheet,
                };

                sstr.AddTraceFormat(tr.MeasurementFormat);

                if (!tr.Title.Equals(""))
                {
                    sstr.AddTraceTitle(tr.Title);
                }
                ChildTestSteps.Add(sstr);
            }
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

        protected override void AddNewTrace()
        {
            ChildTestSteps.Add(
                new StandardSingleTrace()
                {
                    PNAX = this.PNAX,
                    Meas = this.Meas,
                    Channel = this.Channel,
                    IsControlledByParent = true,
                    EnableTraceSettings = true,
                }
            );
        }
    }
}
