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
    public class ConverterNewTraceBaseStep : ConverterBaseStep
    {
        #region Settings
        [Browsable(false)]
        public bool EnableButton { get; set; }

        [Browsable(true)]
        [EnabledIf("EnableButton", true, HideIfDisabled = false)]
        [Display("Add New Trace", Groups: new[] { "Trace" }, Order: 20)]
        [Layout(LayoutMode.FullRow)]
        public virtual void AddNewTraceButton() 
        {
            AddNewTrace();
        }
        #endregion

        public ConverterNewTraceBaseStep()
        {
            EnableButton = true;
            IsControlledByParent = true;
        }

        protected virtual void AddNewTrace()
        {

        }
        public override void Run()
        {
        }
    }
}
