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

namespace OpenTap.Plugins.PNAX.General.Spectrum_Analyzer
{
    [AllowAsChildIn(typeof(SpectrumAnalyzerChannel))]
    [Display("SA Source", Groups: new[] { "PNA-X", "General", "Spectrum Analyzer" }, Description: "Insert a description here")]
    public class SASource : GeneralBaseStep
    {
        #region Settings
        [Display("Power On (All Channels)", Group: "Power", Order: 20)]
        public bool PowerOnAllChannels { get; set; }

        [Browsable(false)]
        private bool _portPowersCoupled;
        [Display("Port Powers Coupled", Group: "Power", Order: 21)]
        public bool PortPowersCoupled
        {
            get { return _portPowersCoupled; }
            set
            {
                _portPowersCoupled = value;
                if (value)
                {
                    // TODO
                    // Update all power from child steps
                    // to be the same as the first port
                    // TODO
                }
            }
        }

        [Display("RF source sweep order", Group: "Sweep", Order: 30)]
        public SASourceSweepOrderTypeEnum SourceSweepOrder { get; set; }

        #endregion

        public SASource()
        {
            PowerOnAllChannels = true;
            PortPowersCoupled = false;

            SourceSweepOrder = SASourceSweepOrderTypeEnum.FrequencyPower;

            SASourceCell port1 = new SASourceCell { IsControlledByParent = true, Channel = this.Channel, CellName = "Port 1" };
            SASourceCell port2 = new SASourceCell { IsControlledByParent = true, Channel = this.Channel, CellName = "Port 2" };
            SASourceCell port3 = new SASourceCell { IsControlledByParent = true, Channel = this.Channel, CellName = "Port 3" };
            SASourceCell port4 = new SASourceCell { IsControlledByParent = true, Channel = this.Channel, CellName = "Port 4" };
            SASourceCell port1src2 = new SASourceCell { IsControlledByParent = true, Channel = this.Channel, CellName = "Port 1 Src2" };
            SASourceCell source3 = new SASourceCell { IsControlledByParent = true, Channel = this.Channel, CellName = "Source3" };

            this.ChildTestSteps.Add(port1);
            this.ChildTestSteps.Add(port2);
            this.ChildTestSteps.Add(port3);
            this.ChildTestSteps.Add(port4);
            this.ChildTestSteps.Add(port1src2);
            this.ChildTestSteps.Add(source3);

        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.

            PNAX.SetPowerOnAllChannels(PowerOnAllChannels);
            PNAX.SetCoupledTonePowers(Channel, PortPowersCoupled);

            PNAX.SetSASweepOrder(Channel, SourceSweepOrder);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
