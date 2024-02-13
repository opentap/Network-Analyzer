// Author: MyName
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

namespace OpenTap.Plugins.PNAX
{
    [AllowAsChildIn(typeof(TestPlan))]
    [AllowAsChildIn(typeof(DIQSources))]
    [Display("DIQ Source", Groups: new[] { "Network Analyzer", "General", "Differential I/Q" }, Description: "Insert a description here")]
    public class DIQSource : PNABaseStep
    {
        #region Settings
        [Display("Name", Group: "Source", Order: 20)]
        public string SourceName
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }

        [Display("Source State", Groups: new[] { "Source" }, Order: 21)]
        public DIQPortStateEnumtype SourceState { get; set; }

        // TODO
        // Add options according to Ranges in DIQFrequencyRange
        // i.e. F1
        [Display("Frequency Range", Groups: new[] { "Source" }, Order: 22)]
        public int FreqRange { get; set; }

        // TODO
        // Enable only for external sources
        [Display("External Source Port", Groups: new[] { "Source" }, Order: 23)]
        public int ExtSourcePort { get; set; }


        private bool _SweepPower;
        [Display("Sweep Power", Groups: new[] { "Power" }, Order: 30)]
        public bool SweepPower 
        {
            get
            {
                return _SweepPower;
            }
            set
            {
                _SweepPower = value;
                if (_SweepPower)
                {
                    Autorange = false;
                }
                else
                {
                    StopPower = StartPower;
                }
            }
        }

        private double _StartPower;
        [Display("Start Power", Groups: new[] { "Power" }, Order: 31)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double StartPower
        {
            get
            {
                return _StartPower;
            }
            set
            {
                _StartPower = value;
                if (!SweepPower)
                {
                    StopPower = StartPower;
                }
            }
        }

        [EnabledIf("SweepPower", true, HideIfDisabled = false)]
        [Display("Stop Power", Groups: new[] { "Power" }, Order: 32)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double StopPower { get; set; }

        [Display("Leveling Mode", Groups: new[] { "Power" }, Order: 33)]
        public string LevelingMode { get; set; }

        private bool _Autorange;
        [EnabledIf("SweepPower", false, HideIfDisabled = false)]
        [Display("Auto range source attenuator", Groups: new[] { "Power" }, Order: 34)]
        public bool Autorange
        {
            get
            {
                return _Autorange;
            }
            set
            {
                _Autorange = value;
                if (_Autorange)
                {
                    SourceAttenuator = 0;
                }
            }
        }


        [EnabledIf("Autorange", false, HideIfDisabled = false)]
        [Display("Source Attenuator", Groups: new[] { "Power" }, Order: 35)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0")]
        public int SourceAttenuator { get; set; }

        [Display("Phase State", Groups: new[] { "Phase" }, Order: 40)]
        public DIQPhaseStateEnumtype DIQPhaseState { get; set; }

        private bool _SweepPhase;
        [Display("Sweep Phase", Groups: new[] { "Phase" }, Order: 41)]
        public bool SweepPhase
        {
            get
            {
                return _SweepPhase;
            }
            set
            {
                _SweepPhase = value;
                if (!_SweepPhase)
                {
                    StopPhase = StartPhase;
                }
            }
        }

        private double _StartPhase;
        [Display("Start Phase", Groups: new[] { "Phase" }, Order: 42)]
        [Unit("°", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double StartPhase
        {
            get
            {
                return _StartPhase;
            }
            set
            {
                _StartPhase = value;
                if (!SweepPhase)
                {
                    StopPhase = StartPhase;
                }
            }
        }

        [EnabledIf("SweepPhase", true, HideIfDisabled = false)]
        [Display("Stop Phase", Groups: new[] { "Phase" }, Order: 43)]
        [Unit("°", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double StopPhase { get; set; }


        [Display("Referenced to", Groups: new[] { "Phase" }, Order: 44)]
        public string ReferencedTo { get; set; }

        // TODO
        // Add options 
        // i.e. a1,a2,a3,a4,b1,b2,b3,b4
        [Display("Receiver to measure the controlled source", Groups: new[] { "Phase" }, Order: 45)]
        public string rCont { get; set; }

        // TODO
        // Add options 
        // i.e. a1,a2,a3,a4,b1,b2,b3,b4
        [Display("Receiver to measure the reference source", Groups: new[] { "Phase" }, Order: 46)]
        public string rRef { get; set; }

        [Display("Tolerance", Groups: new[] { "Phase" }, Order: 47)]
        [Unit("°", UseEngineeringPrefix: false, StringFormat: "0.000")]
        public double Tolerance { get; set; }

        [Display("Max Iterations", Groups: new[] { "Phase" }, Order: 48)]
        public int MaxIterations { get; set; }


        [Display("Match Correction On", Groups: new[] { "Match Correction" }, Order: 50)]
        public bool MatchCorrection { get; set; }

        // TODO
        // Add options 
        // i.e. b1,b2,b3,b4
        [Display("Test Receiver", Groups: new[] { "Match Correction" }, Order: 51)]
        public string TRec { get; set; }

        // TODO
        // Add options 
        // i.e. a1,a2,a3,a4
        [Display("Reference Receiver", Groups: new[] { "Match Correction" }, Order: 52)]
        public string RRec { get; set; }

        // TODO
        // Add options according to the Ranges define in DIQFrequencyRange
        // i.e. F1,F2, etc
        [Display("Match Frequency Range", Groups: new[] { "Match Correction" }, Order: 53)]
        public string MatchFreqRange { get; set; }

        #endregion

        public DIQSource()
        {
            SourceName = "Source";
            SourceState = DIQPortStateEnumtype.Auto;
            FreqRange = 1;
            ExtSourcePort = 0;

            SweepPower = false;
            StartPower = -15;
            StopPower = -15;
            LevelingMode = "Internal";
            Autorange = true;
            SourceAttenuator = 0;

            DIQPhaseState = DIQPhaseStateEnumtype.Off;
            SweepPhase = false;
            StartPhase = 0;
            StopPhase = 0;
            ReferencedTo = "Port 3";
            rCont = "a1";
            rRef = "a3";
            Tolerance = 0.3;
            MaxIterations = 10;

            MatchCorrection = false;
            TRec = "b1";
            RRec = "a1";
            MatchFreqRange = "F1";
        }

        public override void Run()
        {
            PNAX.DIQSourceState(Channel, SourceName, SourceState);
            PNAX.DIQSourceFreqRange(Channel, SourceName, FreqRange);
            PNAX.DIQSourceExternalPort(Channel, SourceName, ExtSourcePort); // TODO send only for external sources

            PNAX.DIQSourcePowerState(Channel, SourceName, SweepPower);
            PNAX.DIQSourcePowerStart(Channel, SourceName, StartPower);
            PNAX.DIQSourcePowerStop(Channel, SourceName, StopPower);
            PNAX.DIQSourceLevelingMode(Channel, SourceName, LevelingMode);
            PNAX.DIQSourcePowerAttenuation(Channel, SourceName, SourceAttenuator);
            PNAX.DIQSourcePowerAttenuationAuto(Channel, SourceName, Autorange);

            PNAX.DIQSourcePhaseState(Channel, SourceName, DIQPhaseState);
            PNAX.DIQSourcePhaseSweepState(Channel, SourceName, SweepPhase);
            PNAX.DIQSourcePhaseStart(Channel, SourceName, StartPhase);
            PNAX.DIQSourcePhaseStop(Channel, SourceName, StopPhase);
            PNAX.DIQSourcePhaseReference(Channel, SourceName, ReferencedTo);
            PNAX.DIQSourcePhaseControlParam(Channel, SourceName, $"{rCont}/{rRef}");
            PNAX.DIQSourcePhaseTolerance(Channel, SourceName, Tolerance);
            PNAX.DIQSourcePhaseIterations(Channel, SourceName, MaxIterations);

            PNAX.DIQSourceMatchCorrectionState(Channel, SourceName, MatchCorrection);
            PNAX.DIQSourceMatchCorrectionTestReceiver(Channel, SourceName, TRec);
            PNAX.DIQSourceMatchCorrectionReferenceReceiver(Channel, SourceName, RRec);
            PNAX.DIQSourceMatchCorrectionRange(Channel, SourceName, MatchFreqRange);

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
