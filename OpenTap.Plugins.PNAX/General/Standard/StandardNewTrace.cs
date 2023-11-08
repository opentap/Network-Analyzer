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
    public class StandardTrace
    {
        public int Channel { get; set; }
        public StandardTraceEnum Meas { get; set; }
        public int Window { get; set; }
        public int Sheet { get; set; }
        public PNAX.MeasurementFormatEnum MeasurementFormat { get; set; }
        public String Title { get; set; }
    }

    [AllowAsChildIn(typeof(StandardChannel))]
    [AllowChildrenOfType(typeof(StandardSingleTrace))]
    [Display("Standard New Trace", Groups: new[] { "PNA-X", "General",  "Standard" }, Description: "Insert a description here")]
    public class StandardNewTrace : GeneralNewTraceBaseStep
    {
        #region Settings
        private StandardTraceEnum _Meas;
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public StandardTraceEnum Meas
        {
            get
            {
                return _Meas;
            }
            set
            {
                _Meas = value;
            }
        }
        #endregion

        public StandardNewTrace()
        {
            ChildTestSteps.Add(new StandardSingleTrace() { PNAX = this.PNAX, Meas = StandardTraceEnum.S11});
        }

        // overloaded constructor for window and sheet
        public StandardNewTrace(List<StandardTrace> standardTraces)
        {
            if ((standardTraces is null) || (standardTraces.Count == 0))
            {
                // add default trace
                ChildTestSteps.Add(new StandardSingleTrace() { PNAX = this.PNAX, Meas = StandardTraceEnum.S11 });
                return;
            }

            foreach (StandardTrace tr in standardTraces)
            {
                StandardSingleTrace sstr = new StandardSingleTrace() { PNAX = this.PNAX, Channel = tr.Channel, Meas = tr.Meas, Window = tr.Window, Sheet = tr.Sheet };

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


        public override void Run()
        {
            // Delete dummy trace defined during channel setup
            // DISPlay:MEASure<mnum>:DELete?
            // CALCulate<cnum>:PARameter:DELete[:NAME] <Mname>
            PNAX.ScpiCommand($"CALCulate{Channel}:PARameter:DELete \'CH{Channel}_DUMMY_1\'");


            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }

        protected override void AddNewTrace()
        {
            StandardTraceEnum standardTrace;
            if (Enum.TryParse<StandardTraceEnum>(Meas.ToString(), out standardTrace))
            {
                this.ChildTestSteps.Add(new StandardSingleTrace() { PNAX = this.PNAX, Meas = standardTrace, Channel = this.Channel });
            }
        }

    }
}
