using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OpenTap.Plugins.PNAX
{
    [Display("General Standard Settings", Description: "Add a description here")]
    public class GeneralStandardSettings : ComponentSettings<GeneralStandardSettings>
    {
        // TODO: Add settings here as properties, use DisplayAttribute to indicate to the GUI
        //       how the setting should be displayed.
        //       Example:
        [Display("Category\\Example Setting", Description:" Just an example")]
        public bool ExampleSetting { get; set; }

        public StandardSweepTypeEnum StandardSweepType { get; set; } = StandardSweepTypeEnum.LinearFrequency;
        public double SweepPropertiesStart { get; set; } = 10e6;
        public double SweepPropertiesStop { get; set; } = 50e9;         // for N5247B: 67e9;
        public double SweepPropertiesStartPower { get; set; } = -10;
        public double SweepPropertiesStopPower { get; set; } = 0;
        public double SweepPropertiesStartPhase { get; set; } = 0;
        public double SweepPropertiesStopPhase { get; set; } = 0;
        public double SweepPropertiesCWFreq { get; set; } = 1e9;
        public double SweepPropertiesPower { get; set; } = -15;         // for N5247B: -5;
        public int SweepPropertiesPoints { get; set; } = 201;
        public double SweepPropertiesIFBandwidth { get; set; } = 100e3;


        public double SweepTimeAuto { get; set; } = 0.016884;
        public double SweepTimeStepped { get; set; } = 0.002010;        // for N5247B: 190.520e-3;
        public double DwellTime { get; set; } = 0;
        public double SweepDelay { get; set; } = 0;
        public bool AutoSweepTime { get; set; } = true;
        public bool FastSweep { get; set; } = false;
        public StandardChannelSweepModeEnum StandardChannelSweepMode { get; set; } = StandardChannelSweepModeEnum.Auto;
        public StandardChannelSweepSequenceEnum StandardChannelSweepSequence { get; set; } = StandardChannelSweepSequenceEnum.Standard;

    }
}
