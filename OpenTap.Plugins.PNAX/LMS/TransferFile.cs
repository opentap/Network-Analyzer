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
using System.IO;
using System.Linq;
using System.Text;

namespace OpenTap.Plugins.PNAX
{
    [Display("Transfer File", Groups: new[] { "Network Analyzer", "Load/Measure/Store" }, Description: "Transfer file from instrument to controlling PC")]
    public class TransferFile : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display("Source Filename", "Specfiy name of the source file on instrument", "Save File", Order: 20)]
        public string SourceFileName { get; set; }

        [Display("Destination Filename", "Specfiy path and filename for csa file to be saved", "Save File", Order: 21)]
        [FilePath(FilePathAttribute.BehaviorChoice.Save, "csa")]
        public string DestinationFileName { get; set; }

        [Display("Delete Source", Order: 22)]
        public bool DeleteSource { get; set; }
        #endregion

        public TransferFile()
        {
            SourceFileName = "";
            DestinationFileName = "";
            DeleteSource = false;

            Rules.Add(() => ((SourceFileName.Equals("") == false)), "Must be a valid state file", "SourceFileName");
            Rules.Add(() => ((DestinationFileName.Equals("") == false)), "Must be a valid state file", "DestinationFileName");
        }

        public override void Run()
        {
            Log.Debug("Source File Name: " + SourceFileName);
            Log.Debug("Destination File Name: " + DestinationFileName);
            string absolutePath = Path.GetFullPath(DestinationFileName);
            Log.Debug("Destination File Name (absolute path): " + absolutePath);

            PNAX.TransferFile(SourceFileName, absolutePath, DeleteSource);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
