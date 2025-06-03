// Author: CMontes
// Copyright:   Copyright 2024 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using OpenTap;

namespace OpenTap.Plugins.PNAX
{
    [Display(
        "Fixture Generator",
        Groups: new[] { "Network Analyzer", "Calibration", "Fixtures" },
        Description: "This feature allow you to mathematically add (embed) or remove (de-embed) circuits to, or from, your measurements."
    )]
    public class FixtureGeneratorStep : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1, Group: "Instrument")]
        public PNAX PNAX { get; set; }

        [Display("Auto Select All Channels", Group: "Instrument", Order: 10)]
        public bool AutoSelectChannels { get; set; }

        [EnabledIf("AutoSelectChannels", false, HideIfDisabled = true)]
        [Display(
            "Channel",
            Group: "Instrument",
            Description: "Choose which channel to grab data from.",
            Order: 10
        )]
        public List<int> channels { get; set; }

        [Display("Apply Fixtures", Group: "User Block", Order: 20)]
        public bool ApplyFixtures { get; set; }

        [Display("VNA Port", Group: "User Block", Order: 21)]
        public int Port { get; set; }

        [Display("Enable Block", Group: "User Block", Order: 22)]
        public bool EnableBlock { get; set; }

        [Display("Embed Type", Group: "User Block", Order: 23)]
        public FSimEmbedTypeEnumtype EmbedType { get; set; }

        [FilePath(FilePathAttribute.BehaviorChoice.Open, "s*p")]
        [Display("Fixture/Adapter SnP File", Group: "User Block", Order: 24)]
        public string FixtureAdapterSnP { get; set; }

        [Display(
            "Power Compensation",
            Group: "User Block",
            Order: 25,
            Description: "When the Cal Set contains a power correction array for the fixture port, that array will be compensated for the fixture loss."
        )]
        public bool CompPwr { get; set; }

        [Display("Enable Extrapolation", Group: "User Block", Order: 26)]
        public bool Extrapolation { get; set; }

        #endregion

        public FixtureGeneratorStep()
        {
            channels = new List<int>() { 1 };
            AutoSelectChannels = true;
            ApplyFixtures = true;
            Port = 1;
            EnableBlock = true;
            EmbedType = FSimEmbedTypeEnumtype.Deembed;
            FixtureAdapterSnP = "";
            CompPwr = true;
            Extrapolation = false;

            Rules.Add(IsFileValid, "Must be a valid file", "FixtureAdapterSnP");
        }

        private bool IsFileValid()
        {
            if (string.IsNullOrEmpty(FixtureAdapterSnP))
                return false;

            return File.Exists(FixtureAdapterSnP);
        }

        public override void Run()
        {
            if (AutoSelectChannels)
            {
                channels = PNAX.GetActiveChannels();
            }

            foreach (int Channel in channels)
            {
                PNAX.FSimSingleEndedOrder(Channel, "1,2,3,0");
                PNAX.FSimCircuitReset(Channel);
                int circN = PNAX.FSimCircuitNext(Channel);
                PNAX.FSimCircuitAddFile(Channel, circN);
                PNAX.FSimCircuitSetPort(Channel, circN, Port);
                PNAX.FSimCircuitSetFileName(Channel, circN, FixtureAdapterSnP);
                PNAX.FSimCircuitState(Channel, circN, EnableBlock);
                PNAX.FSimCircuitEmbedType(Channel, circN, EmbedType);
                PNAX.FSimCircuitExtrapolation(Channel, circN, Extrapolation);
                PNAX.FSimCircuitPowerCompensate(Channel, circN, CompPwr);
                PNAX.FSimApply(Channel);
                PNAX.FSimState(Channel, ApplyFixtures);
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
