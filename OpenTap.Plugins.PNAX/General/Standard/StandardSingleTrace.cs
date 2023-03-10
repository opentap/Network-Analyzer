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
    public enum StandardTraceEnum
    {
        S11,
        S12,
        S13,
        S14,
        S21,
        S22,
        S23,
        S24,
        S31,
        S32,
        S33,
        S34,
        S41,
        S42,
        S43,
        S44,
        Sss11,
        Ssd12,
        Ssc12,
        Sds21,
        Sdd22,
        Sdc22,
        Scs21,
        Scd22,
        Scc22,
        [Display("Sds21/Scs21")]
        Sds21Scs21,
        [Display("Ssd12/Ssc12")]
        Ssd12Ssc12,
        A1,
        A2,
        A3,
        A4,
        B1,
        B2,
        B3,
        B4,
        C1,
        C2,
        C3,
        C4,
        D1,
        D2,
        D3,
        D4,
        R11,
        R22,
        R33,
        R44,
        a11,
        a22,
        a33,
        a44,
        b11,
        b12,
        b13,
        b14,
        b21,
        b22,
        b23,
        b24,
        b31,
        b32,
        b33,
        b34,
        b41,
        b42,
        b43,
        b44,
        AuxLn11,
        AuxLn21
    }

    [AllowAsChildIn(typeof(StandardNewTrace))]
    [Display("Standard Single Trace", Groups: new[] { "PNA-X", "General",  "Standard" }, Description: "Insert a description here")]
    public class StandardSingleTrace : GeneralChannelBaseStep
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
        public int Channel
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

        public StandardSingleTrace()
        {
            Trace = "1";
            Meas = StandardTraceEnum.S11;
            Class = TraceManagerChannelClassEnum.STD;
            Window = 1;
            Sheet = 1;
            Channel = 1;
        }

        private void UpdateTestName()
        {
            this.Trace = $"CH{Channel.ToString()}_{Meas}";
            this.Name = $"CH{Channel.ToString()}_{Meas}";
        }

        public override void Run()
        {
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            int traceid = PNAX.GetNewTraceID();

            // Define the measurement
            PNAX.ScpiCommand($"CALCulate{Channel.ToString()}:PARameter:DEFine:EXT \'{Trace}\',\'{Meas.ToString()}\'");
            PNAX.ScpiCommand($"CALCulate{Channel.ToString()}:PARameter:SELect \'{Trace}\'");

            // Create a window if it doesn't exist already
            PNAX.ScpiCommand($"DISPlay:WINDow{Window.ToString()}:STATe ON");

            // Display the measurement
            PNAX.ScpiCommand($"DISPlay:WINDow{Window.ToString()}:TRACe{traceid.ToString()}:FEED \'{Trace}\'");

            // Select the measurement

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
