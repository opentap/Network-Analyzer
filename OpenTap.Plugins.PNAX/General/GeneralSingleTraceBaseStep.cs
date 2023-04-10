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
    public class GeneralSingleTraceBaseStep : GeneralBaseStep
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

        protected virtual void UpdateTestName()
        {
        }


    }
}
