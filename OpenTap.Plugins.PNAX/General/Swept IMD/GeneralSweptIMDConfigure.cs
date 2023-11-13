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
    public enum ReceiverConfigurationEnumType
    {
        [Scpi("INT")]
        [Display("Internal Combiner Using One RefReceiver")]
        INT,
        [Scpi("EXT")]
        [Display("External Combiner Using One RefReceiver")]
        EXT
    }

    [AllowAsChildIn(typeof(GeneralSweptIMDChannel))]
    [Display("Configure", Groups: new[] { "PNA-X", "General", "Swept IMD" }, Description: "Insert a description here", Order: 4)]
    public class GeneralSweptIMDConfigure : PNABaseStep
    {
        #region Settings
        [Display("Receiver Configuration", Groups: new[] { "Receiver Configuration"}, Order: 10)]
        public ReceiverConfigurationEnumType ReceiverConfiguration { get; set; }
        #endregion

        public GeneralSweptIMDConfigure()
        {
            ReceiverConfiguration = ReceiverConfigurationEnumType.INT;
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetReceiverConfiguration(Channel, ReceiverConfiguration);
            
            UpgradeVerdict(Verdict.Pass);
        }
    }
}
