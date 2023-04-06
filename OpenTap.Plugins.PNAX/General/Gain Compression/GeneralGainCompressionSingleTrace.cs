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
    public enum GeneralGainCompressionTraceEnum
    {
        S21,
        S11,
        S12,
        S22,
        AI1,
        AI2,
        CompIn21,
        CompOut21,
        DeltaGain21,
        CompGain21,
        CompS11,
        RefS21,
        CompAI1,
        CompAI2,
    }

    [AllowAsChildIn(typeof(GeneralGainCompressionChannel))]
    [AllowAsChildIn(typeof(GeneralGainCompressionNewTrace))]
    [Display("Compression Single Trace", Groups: new[] { "PNA-X", "General", "Compression" }, Description: "Insert a description here")]
    public class GeneralGainCompressionSingleTrace : GeneralNewTraceBaseStep
    {
        #region Settings
        private String _Trace;
        [Display("Trace", Groups: new[] { "Trace" }, Order: 10)]
        public String Trace
        {
            get
            {
                return _Trace;
            }
            set
            {
                _Trace = value;
                //UpdateTestName();
            }
        }

        private GeneralGainCompressionTraceEnum _Meas;
        [Display("Meas", Groups: new[] { "Trace" }, Order: 11)]
        public GeneralGainCompressionTraceEnum Meas
        {
            get
            {
                return _Meas;
            }
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
            get
            {
                return _Class;
            }
            set
            {
                _Class = value;
                UpdateTestName();
            }
        }


        private int _Channel;
        [Display("Channel", Groups: new[] { "Trace" }, Order: 13)]
        public override int Channel
        {
            get
            {
                return _Channel;
            }
            set
            {
                _Channel = value;
                UpdateTestName();
            }
        }


        private int _Window;
        [Display("Window", Groups: new[] { "Trace" }, Order: 14)]
        public int Window
        {
            get
            {
                return _Window;
            }
            set
            {
                _Window = value;
                UpdateTestName();
            }
        }


        private int _Sheet;
        [Display("Sheet", Groups: new[] { "Trace" }, Order: 15)]
        public int Sheet
        {
            get
            {
                return _Sheet;
            }
            set
            {
                _Sheet = value;
                UpdateTestName();
            }
        }
        #endregion

        public GeneralGainCompressionSingleTrace()
        {
            Trace = "1";
            Meas = GeneralGainCompressionTraceEnum.CompIn21;
            Channel = 1;
            Window = 1;
            Sheet = 1;
        }

        protected void UpdateTestName()
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

            PNAX.ScpiCommand($"CALCulate{Channel.ToString()}:CUST:DEFine \'{Trace}\',\'Gain Compression\',\'{Meas.ToString()}\'");

            // Create a window if it doesn't exist already
            PNAX.ScpiCommand($"DISPlay:WINDow{Window.ToString()}:STATe ON");

            // Display the measurement
            PNAX.ScpiCommand($"DISPlay:WINDow{Window.ToString()}:TRACe{traceid.ToString()}:FEED \'{Trace}\'");

            // Select the measurement

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
