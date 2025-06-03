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
        "Apply Adapter/Fixture",
        Groups: new[] { "Network Analyzer", "Calibration", "Fixtures" },
        Description: "Choose an existing characterized adapter to be de-embedded from any existing channels or existing calsets that share the same ports"
    )]
    public class ApplyAdapterFixtureStep : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1, Group: "Instrument")]
        public PNAX PNAX { get; set; }

        [Display("VNA Port", Group: "Define Fixtures", Order: 20)]
        public int Port { get; set; }

        [FilePath(FilePathAttribute.BehaviorChoice.Open, "s*p")]
        [Display("Fixture/Adapter SnP File", Group: "Define Fixtures", Order: 21)]
        public string FixtureAdapterSnP { get; set; }

        [FilePath(FilePathAttribute.BehaviorChoice.Open)]
        [Display("Calset Name", Group: "Properties", Order: 30)]
        public string CalsetName { get; set; }

        [Display("Calset", Group: "Properties", Order: 31)]
        public FSimCalsetOverwriteCreateEnumtype OverwriteCreateCalset { get; set; }

        [FilePath(FilePathAttribute.BehaviorChoice.Save)]
        [EnabledIf(
            "OverwriteCreateCalset",
            FSimCalsetOverwriteCreateEnumtype.New,
            HideIfDisabled = false
        )]
        [Display("New Calset Name", Group: "Properties", Order: 32)]
        public string NewCalsetName { get; set; }

        [Display(
            "Power Compensation",
            Group: "Properties",
            Order: 33,
            Description: "When the Cal Set contains a power correction array for the fixture port, that array will be compensated for the fixture loss."
        )]
        public bool CompPwr { get; set; }

        [Display(
            "Extrapolation",
            Group: "Properties",
            Order: 34,
            Description: "Applies a simple extrapolation when the S2P file has a narrower frequency range than the Cal Set. The values for the first and last data points are extended in either direction to cover the frequency range of the Cal Set."
        )]
        public bool Extrapolation { get; set; }
        #endregion

        public ApplyAdapterFixtureStep()
        {
            Port = 1;
            FixtureAdapterSnP = "";
            CalsetName = "MyCalset";
            OverwriteCreateCalset = FSimCalsetOverwriteCreateEnumtype.New;
            NewCalsetName = "CPM_" + CalsetName;
            CompPwr = true;
            Extrapolation = false;

            Rules.Add(IsFileValid, "Must be a valid snp file", "FixtureAdapterSnP");
            Rules.Add(
                () => ((CalsetName.Equals("") == false)),
                "Must be a valid state file",
                "CalsetName"
            );
            Rules.Add(
                () => ((NewCalsetName.Equals("") == false)),
                "Must be a valid state file",
                "NewCalsetName"
            );
        }

        private bool IsFileValid()
        {
            if (string.IsNullOrEmpty(FixtureAdapterSnP))
                return false;

            return File.Exists(FixtureAdapterSnP);
        }

        public override void Run()
        {
            PNAX.CalPlaneManagerApplyAdapterFixtureDeembed(
                CalsetName,
                NewCalsetName,
                FixtureAdapterSnP,
                Port,
                CompPwr,
                Extrapolation
            );

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
