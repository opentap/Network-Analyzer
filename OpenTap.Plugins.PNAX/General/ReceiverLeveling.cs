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
    // not available in channels: DIQ, NF, NFX, SA
    //[AllowAsChildIn(typeof(StandardChannel))]
    //[AllowAsChildIn(typeof(GeneralGainCompressionChannel))]
    //[AllowAsChildIn(typeof(GeneralSweptIMDChannel))]
    //[AllowAsChildIn(typeof(ScalarMixerChannel))]
    //[AllowAsChildIn(typeof(GainCompressionChannel))]
    //[AllowAsChildIn(typeof(SweptIMDChannel))]
    [Display("Receiver Leveling", Groups: new[] { "Network Analyzer", "General" },
        Description: "Receiver Leveling\nCan be added as a child to the following Channels:\n\tStandard\n\tGain Compression\n\tSwept IMD\n\t\n\tSMC\n\tGCX\n\tIMDX")]
    public class ReceiverLeveling : PNABaseStep
    {
        #region Settings
        #endregion

        public ReceiverLeveling()
        {
            MetaData = new List<(string, object)>();

            ReceiverLevelingSource p1 = new ReceiverLevelingSource { IsControlledByParent = true, Channel = this.Channel, ControlledSource = "Port 1", LevelingReceiver = "R1" };
            this.ChildTestSteps.Add(p1);
            ReceiverLevelingSource p2 = new ReceiverLevelingSource { IsControlledByParent = true, Channel = this.Channel, ControlledSource = "Port 2", LevelingReceiver = "R2" };
            this.ChildTestSteps.Add(p2);
            ReceiverLevelingSource p3 = new ReceiverLevelingSource { IsControlledByParent = true, Channel = this.Channel, ControlledSource = "Port 3", LevelingReceiver = "R3" };
            this.ChildTestSteps.Add(p3);
            ReceiverLevelingSource p4 = new ReceiverLevelingSource { IsControlledByParent = true, Channel = this.Channel, ControlledSource = "Port 4", LevelingReceiver = "R4" };
            this.ChildTestSteps.Add(p4);
            ReceiverLevelingSource p1src2 = new ReceiverLevelingSource { IsControlledByParent = true, Channel = this.Channel, ControlledSource = "Port 1 Src2", LevelingReceiver = "R1" };
            this.ChildTestSteps.Add(p1src2);
            ReceiverLevelingSource src3 = new ReceiverLevelingSource { IsControlledByParent = true, Channel = this.Channel, ControlledSource = "Source 3", LevelingReceiver = "R1" };
            this.ChildTestSteps.Add(src3);

        }

        [Browsable(true)]
        [Display("Add Controlled Source", Group: "Controlled Sources", Order: 40)]
        public void AddControlledSource()
        {
            ReceiverLevelingSource newsrc1 = new ReceiverLevelingSource { IsControlledByParent = true, Channel = this.Channel, ControlledSource = "Port 1", LevelingReceiver = "R1" };
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
