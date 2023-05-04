using OpenTap;   // Use OpenTAP infrastructure/core components (log,TestStep definition, etc)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OpenTap.Plugins.PNAX
{
    [Display("Load File", Groups: new[] { "PNA-X", "L / M / S" }, Description: "Load State File")]
    public class LoadFile : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display("State Filename", "Specfiy path and filename for csa data to be loaded", "Load File")]
        [FilePath(FilePathAttribute.BehaviorChoice.Open, "csa")]
        public string StateFile { get; set; }
        #endregion

        public LoadFile()
        {
            StateFile = "";
            Rules.Add(IsFileValid, "Must be a valid file", "StateFile");
        }

        public override void Run()
        {
            UpgradeVerdict(Verdict.NotSet);

            try 
            {
                PNAX.LoadState(StateFile);
                PNAX.WaitForOperationComplete();
            }
            catch(FileNotFoundException ex)
            {
                throw ex;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            UpgradeVerdict(Verdict.Pass);
        }

        private bool IsFileValid()
        {
            if (string.IsNullOrEmpty(StateFile)) return false;

            return File.Exists(StateFile);
        }
    }
}
