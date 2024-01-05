// Author: CMontes
// Copyright:   Copyright 2024 Keysight Technologies
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

namespace OpenTap.Plugins.PNAX.General
{
    [AllowAsChildIn(typeof(StandardChannel))]
    [AllowAsChildIn(typeof(GeneralGainCompressionChannel))]
    [AllowAsChildIn(typeof(GeneralSweptIMDChannel))]
    [AllowAsChildIn(typeof(ScalarMixerChannel))]
    [AllowAsChildIn(typeof(GainCompressionChannel))]
    [AllowAsChildIn(typeof(SweptIMDChannel))]
    [Display("Receiver Leveling", Groups: new[] { "Network Analyzer", "General" },
        Description: "Receiver Leveling\nCan be added as a child to the following Channels:\n\tStandard\n\tGain Compression\n\tSwept IMD\n\t\n\tSMC\n\tGCX\n\tIMDX")]
    public class ReceiverLeveling : PNABaseStep
    {
        #region Settings
        #endregion

        public ReceiverLeveling()
        {
            MetaData = new List<(string, object)>();

            ReceiverLevelingSource src1 = new ReceiverLevelingSource { IsControlledByParent = true, Channel = this.Channel, ControlledSource = "Port 1" };
            this.ChildTestSteps.Add(src1);

        }

        [Browsable(true)]
        [Display("Add Controlled Source", Group: "Controlled Sources", Order: 40)]
        public void AddControlledSource()
        {
            ReceiverLevelingSource newsrc1 = new ReceiverLevelingSource { IsControlledByParent = true, Channel = this.Channel, ControlledSource = "Port 1" };
            this.ChildTestSteps.Add(newsrc1);
        }

        public override void Run()
        {
            RunChildSteps();

            UpgradeVerdict(Verdict.Pass);
        }

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            UpdateMetaData();
            List<(string, object)> retVal = new List<(string, object)>();

            foreach (var a in MetaData)
            {
                retVal.Add(a);
            }

            return retVal;
        }

        [Browsable(true)]
        [Display("Update MetaData", Groups: new[] { "MetaData" }, Order: 1000.2)]
        public override void UpdateMetaData()
        {
            MetaData = new List<(string, object)>();

            foreach (var ch in this.ChildTestSteps)
            {
                List<(string, object)> ret = (ch as ReceiverLevelingSource).GetMetaData();
                foreach (var it in ret)
                {
                    MetaData.Add(it);
                }
            }
        }

    }
}
