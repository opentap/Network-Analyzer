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
        [Browsable(false)]
        public bool IsRangesVisible { get; set; } = false;

        private int _NumberOfRanges;
        [EnabledIf("IsRangesVisible", true, HideIfDisabled = true)]
        public int NumberOfRanges
        {
            set
            {
                _NumberOfRanges = value;
                UpdateAvailableRanges();
            }
            get
            {
                return _NumberOfRanges;
            }
        }

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
                if (this.Name.Contains("Port"))
                {
                    IsExternalPort = false;
                }
                else
                {
                    IsExternalPort = true;
                    ExtSourcePort = 1;
                }
            }
        }

        [Display("Source State", Groups: new[] { "Source" }, Order: 21)]
        public DIQPortStateEnumtype SourceState { get; set; }

        [Display("Frequency Range", Groups: new[] { "Source" }, Order: 22)]
        [AvailableValues(nameof(FreqRangesListOfAvailableValues))]
        public string FreqRange { get; set; }

        [Browsable(false)]
        public bool IsExternalPort { get; set; } = false;

        [EnabledIf("IsExternalPort", true, HideIfDisabled = true)]
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
        [AvailableValues(nameof(LevelingModeListOfAvailableValues))]
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
        [AvailableValues(nameof(ReferencedToListOfAvailableValues))]
        public string ReferencedTo { get; set; }

        [Display("Receiver to measure the controlled source", Groups: new[] { "Phase" }, Order: 45)]
        [AvailableValues(nameof(ReceiverListOfAvailableValues))]
        public string rCont { get; set; }

        [Display("Receiver to measure the reference source", Groups: new[] { "Phase" }, Order: 46)]
        [AvailableValues(nameof(ReceiverListOfAvailableValues))]
        public string rRef { get; set; }

        [Display("Tolerance", Groups: new[] { "Phase" }, Order: 47)]
        [Unit("°", UseEngineeringPrefix: false, StringFormat: "0.000")]
        public double Tolerance { get; set; }

        [Display("Max Iterations", Groups: new[] { "Phase" }, Order: 48)]
        public int MaxIterations { get; set; }


        [Display("Match Correction On", Groups: new[] { "Match Correction" }, Order: 50)]
        public bool MatchCorrection { get; set; }

        [Display("Test Receiver", Groups: new[] { "Match Correction" }, Order: 51)]
        [AvailableValues(nameof(TRecListOfAvailableValues))]
        public string TRec { get; set; }

        [Display("Reference Receiver", Groups: new[] { "Match Correction" }, Order: 52)]
        [AvailableValues(nameof(RRecListOfAvailableValues))]
        public string RRec { get; set; }

        private List<string> _SelectMatchFreqRange;
        [Display("Select Frequency Range", Groups: new[] { "Match Correction" }, Order: 53)]
        [AvailableValues(nameof(FreqRangesListOfAvailableValues))]
        public List<string> SelectMatchFreqRange
        {
            get
            {
                MatchFreqRange = string.Join(",", _SelectMatchFreqRange);
                return _SelectMatchFreqRange;
            }
            set
            {
                _SelectMatchFreqRange = value;
                //MatchFreqRange = string.Join(",", _SelectMatchFreqRange);
            }
        }

        [Browsable(false)]
        public bool IsMatchFreqRangeEditable { get; set; } = false;

        [EnabledIf("IsMatchFreqRangeEditable", true, HideIfDisabled = false)]
        [Display("Match Frequency Range", Groups: new[] { "Match Correction" }, Order: 54)]
        public string MatchFreqRange { get; set; }


        private List<string> _FreqRangesListOfAvailableValues;
        [Display("Frequency Range Values", "Editable list for Frequency Range values", Groups: new[] { "Available Values Setup" }, Order: 101, Collapsed: true)]
        public List<string> FreqRangesListOfAvailableValues
        {
            get { return _FreqRangesListOfAvailableValues; }
            set
            {
                _FreqRangesListOfAvailableValues = value;
                OnPropertyChanged("FreqRangesListOfAvailableValues");
            }
        }

        private List<string> _LevelingModeListOfAvailableValues;
        [Display("Leveling Mode Values", "Editable list for Leveling Mode values", Groups: new[] { "Available Values Setup" }, Order: 102, Collapsed: true)]
        public List<string> LevelingModeListOfAvailableValues
        {
            get { return _LevelingModeListOfAvailableValues; }
            set
            {
                _LevelingModeListOfAvailableValues = value;
                OnPropertyChanged("LevelingModeListOfAvailableValues");
            }
        }

        private List<string> _ReferencedToListOfAvailableValues;
        [Display("Referenced To Values", "Editable list for Referenced To values", Groups: new[] { "Available Values Setup" }, Order: 103, Collapsed: true)]
        public List<string> ReferencedToListOfAvailableValues
        {
            get { return _ReferencedToListOfAvailableValues; }
            set
            {
                _ReferencedToListOfAvailableValues = value;
                OnPropertyChanged("ReferencedToListOfAvailableValues");
            }
        }

        private List<string> _ReceiverListOfAvailableValues;
        [Display("Receiver Values", "Editable list for Receiver values", Groups: new[] { "Available Values Setup" }, Order: 104, Collapsed: true)]
        public List<string> ReceiverListOfAvailableValues
        {
            get { return _ReceiverListOfAvailableValues; }
            set
            {
                _ReceiverListOfAvailableValues = value;
                OnPropertyChanged("ReceiverListOfAvailableValues");
            }
        }

        private List<string> _TRecListOfAvailableValues;
        [Display("Test Receiver Values", "Editable list for Test Receiver values", Groups: new[] { "Available Values Setup" }, Order: 105, Collapsed: true)]
        public List<string> TRecListOfAvailableValues
        {
            get { return _TRecListOfAvailableValues; }
            set
            {
                _TRecListOfAvailableValues = value;
                OnPropertyChanged("TRecListOfAvailableValues");
            }
        }

        private List<string> _RRecListOfAvailableValues;
        [Display("Reference Receiver Values", "Editable list for Reference Receiver values", Groups: new[] { "Available Values Setup" }, Order: 106, Collapsed: true)]
        public List<string> RRecListOfAvailableValues
        {
            get { return _RRecListOfAvailableValues; }
            set
            {
                _RRecListOfAvailableValues = value;
                OnPropertyChanged("RRecListOfAvailableValues");
            }
        }


        private List<string> _listOfAvailableValues;
        [Display("Available Values", "An editable list of values.")]
        public List<string> ListOfAvailableValues
        {
            get { return _listOfAvailableValues; }
            set
            {
                _listOfAvailableValues = value;
                OnPropertyChanged("ListOfAvailableValues");
            }
        }

        #endregion
        protected void UpdateAvailableRanges()
        {
            // Start a new list
            _FreqRangesListOfAvailableValues = new List<string>();
            // Fill it with the number of available ranges
            for (int i = 0; i < NumberOfRanges; i++)
            {
                _FreqRangesListOfAvailableValues.Add($"F{i + 1}");
            }
        }

        public DIQSource()
        {
            _FreqRangesListOfAvailableValues = new List<string> { "F1" };
            _LevelingModeListOfAvailableValues = new List<string> { "Internal", "Internal-R1,1", "Open Loop", "Open Loop-R1,1" };
            _ReferencedToListOfAvailableValues = new List<string> { "Port 3", "Port 4", "Source3" };
            _ReceiverListOfAvailableValues = new List<string> { "a1", "a2", "a3", "a4", "b1", "b2", "b3", "b4" };
            _TRecListOfAvailableValues = new List<string> { "b1", "b2", "b3", "b4" };
            _RRecListOfAvailableValues = new List<string> { "a1", "a2", "a3", "a4" };

            SourceName = "Source";
            SourceState = DIQPortStateEnumtype.Auto;
            FreqRange = "F1";
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
            SelectMatchFreqRange = new List<string>() { "F1" };
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
