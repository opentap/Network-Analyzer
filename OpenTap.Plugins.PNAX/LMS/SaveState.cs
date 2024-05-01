// Author: Ivan Diep
// Copyright:   Copyright 2021 Keysight Technologies
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
using System.IO;

namespace OpenTap.Plugins.PNAX
{
    [Display("Save State", Groups: new[] { "Network Analyzer", "Load/Measure/Store" }, Description: "Save state as a .csa file")]
    public class SaveState : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display("State Filename", "Specfiy path and filename for csa file to be saved", "Save File")]
        [FilePath(FilePathAttribute.BehaviorChoice.Save, "csa")]
        public string StateFileName { get; set; }
        #endregion

        public SaveState()
        {
            // ToDo: Set default values for properties / settings.
            StateFileName = "";
            Rules.Add(() => ((StateFileName.Equals("") == false)), "Must be a valid file", "StateFileName");
        }

        public override void Run()
        {
            UpgradeVerdict(Verdict.NotSet);

            Log.Info("---Save State File---");
            // ToDo: Add test case code.
            RunChildSteps(); //If the step supports child steps.

            try
            {
                PNAX.SaveState(StateFileName);
                Log.Info("---Save State Completed---");
                PNAX.WaitForOperationComplete();
            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
