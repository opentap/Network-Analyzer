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
    public enum SMCTraceEnum
    {
        SC21,
        SC12,
        S11,
        S22,
        IPwr,
        OPwr,
        RevIPwr,
        RevOPwr,
        AI1_1,
        AI2_1,
        AI1_2,
        AI2_2,
    }


    [AllowAsChildIn(typeof(ScalarMixerChannel))]
    [AllowAsChildIn(typeof(ScalarMixerNewTrace))]
    [Display("Scalar Mixer Single Trace", Groups: new[] { "PNA-X", "Converters", "Scalar Mixer Converter + Phase" }, Description: "Insert a description here")]
    public class ScalarMixerSingleTrace : ConverterSingleTraceBaseStep
    {
        #region Settings
        private SMCTraceEnum _Meas;
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public SMCTraceEnum Meas
        {
            get { return _Meas; }
            set
            {
                _Meas = value;
                UpdateTestName();
            }
        }
        #endregion

        public ScalarMixerSingleTrace()
        {
            Trace = "1";
            Meas = SMCTraceEnum.SC21;
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

            PNAX.ScpiCommand($"CALCulate{Channel.ToString()}:CUST:DEFine \'{Trace}\',\'Scalar Mixer/Converter\',\'{Meas.ToString()}\'");

            // Create a window if it doesn't exist already
            PNAX.ScpiCommand($"DISPlay:WINDow{Window.ToString()}:STATe ON");

            // Display the measurement
            PNAX.ScpiCommand($"DISPlay:WINDow{Window.ToString()}:TRACe{traceid.ToString()}:FEED \'{Trace}\'");

            // Select the measurement

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
