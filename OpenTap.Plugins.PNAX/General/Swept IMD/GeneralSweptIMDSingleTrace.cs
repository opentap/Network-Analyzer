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
    [AllowAsChildIn(typeof(GeneralSweptIMDChannel))]
    [AllowAsChildIn(typeof(GeneralSweptIMDNewTrace))]
    [Display("Swept IMD Single Trace", Groups: new[] { "PNA-X", "General", "Swept IMD" }, Description: "Insert a description here")]
    public class GeneralSweptIMDSingleTrace : GeneralSingleTraceBaseStep
    {
        #region Settings
        private SweptIMDTraceEnum _Meas;
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public SweptIMDTraceEnum Meas
        {
            get { return _Meas; }
            set
            {
                _Meas = value;
                UpdateTestName();
            }
        }

        private TraceManagerChannelClassEnum _Class;
        [Display("Class", Groups: new[] { "Trace" }, Order: 12)]
        public TraceManagerChannelClassEnum Class
        {
            get { return _Class; }
            set
            {
                _Class = value;
                UpdateTestName();
            }
        }


        #endregion

        public GeneralSweptIMDSingleTrace()
        {
            Trace = "1";
            Meas = SweptIMDTraceEnum.Pwr2Hi;
            Class = TraceManagerChannelClassEnum.IMDX;
            Channel = 1;
            Window = 1;
            Sheet = 1;
        }

        protected override void UpdateTestName()
        {
            this.Trace = $"CH{Channel}_{Meas}";
            this.Name = $"CH{Channel}_{Meas}";
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            int traceid = PNAX.GetNewTraceID();

            // Define the measurement
            //PNAX.ScpiCommand($"CALCulate{Channel.ToString()}:PARameter:DEFine:EXT \'{Trace}\',\'{Meas.ToString()}\'");
            //PNAX.ScpiCommand($"CALCulate{Channel.ToString()}:PARameter:SELect \'{Trace}\'");

            PNAX.ScpiCommand($"CALCulate{Channel.ToString()}:CUST:DEFine \'{Trace}\',\'Swept IMD\',\'{Meas.ToString()}\'");

            // Create a window if it doesn't exist already
            PNAX.ScpiCommand($"DISPlay:WINDow{Window.ToString()}:STATe ON");

            // Display the measurement
            PNAX.ScpiCommand($"DISPlay:WINDow{Window.ToString()}:TRACe{traceid.ToString()}:FEED \'{Trace}\'");

            // Select the measurement

            UpgradeVerdict(Verdict.Pass);
        }
    }


}
