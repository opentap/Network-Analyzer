// Author: MyName
// Copyright:   Copyright 2023 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using OpenTap;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OpenTap.Plugins.PNAX
{
    public class PictureDefinition
    {
        public Picture Picture { get; } = new Picture();

        [FilePath(FilePathAttribute.BehaviorChoice.Open)]
        [Display("Picture Source", "File path for picture to be displayed.")]
        public string PictureSource
        {
            get => Picture.Source;
            set => Picture.Source = value;
        }

        [Display("Picture Description", "Description of the picture to be displayed.")]
        public string PictureDescription
        {
            get => Picture.Description;
            set => Picture.Description = value;
        }

    }

    [Display("Cal All", Groups: new[] { "Network Analyzer", "Calibration" }, Description: "Insert a description here")]
    public class CalAll : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        [Display("Cal Set Name", Order: 0.2)]
        public String CalSetName { get; set; }

        [Display("Auto Select Channels", Group: "Cal All", Order: 1)]
        public bool AutoSelectChannels { get; set; }

        private List<CalibrateAllSelectedChannels> _CalAllSelectedChannels = new List<CalibrateAllSelectedChannels>();
        [EnabledIf("AutoSelectChannels", false, HideIfDisabled = true)]
        [Display("Calibrate All Selected Channels", Group: "Cal All", Order: 1.1)]
        public List<CalibrateAllSelectedChannels> CalAllSelectedChannels
        {
            get
            {
                // TODO - why the Setter never gets called?
                UpdateCalProperties();
                // TODO
                return _CalAllSelectedChannels;
            }
            set
            {
                _CalAllSelectedChannels = value;
                // TODO - why the Setter never gets called?
                UpdateCalProperties();
                // TODO
            }
        }

        [Browsable(true)]
        [EnabledIf("AutoSelectChannels", false, HideIfDisabled = true)]
        [Display("Query Channels", Group: "Cal All", Order: 1.2)]
        public void QueryChannelsButton()
        {
            if (PNAX.IsConnected)
            {
                Log.Info("Disconnect before using this feature!");
                return;
            }
            QueryChannels();
        }

        [EnabledIf("AutoSelectChannels", true, HideIfDisabled = true)]
        [Display("Channel Cal Ports", Group: "Cal All", Order: 1.3)]
        public List<int> CalChannels { get; set; }

        [Browsable(false)]
        public bool IsAnyCalEnabled { get; set; }

        // Only enabled if:
        // Scalar Mixer/Converters + Phase
        // Gain Compression Converters
        [Browsable(false)]
        public bool IsSplitCalEnabled { get; set; }

        // Only enabled if:
        // Scalar Mixer/Converters + Phase
        [Browsable(false)]
        public bool IsPhaseEnabled { get; set; }


        // Only enabled if:
        // Standard Channel is the only channel enabled
        [Browsable(false)]
        public bool IsPowerCalEnabled { get; set; }

        // Only enabled if:
        // Swept IMD
        // Swept IMD Converters
        [Browsable(false)]
        public bool IsIMDEnabled { get; set; }

        // Only enabled if:
        // Noise Figure Cold Source
        // Noise Figure Converters
        [Browsable(false)]
        public bool IsNoiseEnabled { get; set; }

        [EnabledIf("IsAnyCalEnabled", true, HideIfDisabled = true)]
        [Display("Use Smart Cal Order", Groups: new[] { "Measurement Class Calibration", "Miscellaneous" }, Order: 20)]
        public bool UserSmartCalOrder { get; set; }

        [EnabledIf("IsAnyCalEnabled", true, HideIfDisabled = true)]
        [Display("Enable Extra Power Cals", Groups: new[] { "Measurement Class Calibration", "Miscellaneous" }, Order: 21)]
        public ExtraPowerCalsEnum ExtraPowerCals { get; set; }

        [EnabledIf("IsAnyCalEnabled", true, HideIfDisabled = true)]
        [Display("Independent Calibration Channels", Groups: new[] { "Measurement Class Calibration", "Miscellaneous" }, Order: 22)]
        public List<int> IndependentCalibrationChannels { get; set; }

        [EnabledIf("IsAnyCalEnabled", true, HideIfDisabled = true)]
        [Display("Power Meter Settling Tolerance", Groups: new[] { "Measurement Class Calibration", "Miscellaneous" }, Order: 23)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double PowerMeterSettlingTolerance { get; set; }

        [EnabledIf("IsAnyCalEnabled", true, HideIfDisabled = true)]
        [Display("Power Meter Max Readings", Groups: new[] { "Measurement Class Calibration", "Miscellaneous" }, Order: 24)]
        public int PowerMeterSettlingReadings { get; set; }

        [EnabledIf("IsAnyCalEnabled", true, HideIfDisabled = true)]
        [Display("Power Meter Power Level", Groups: new[] { "Measurement Class Calibration", "Miscellaneous" }, Order: 25)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0")]
        public double PowerMeterPowerLevel { get; set; }




        [EnabledIf("IsSplitCalEnabled", true, HideIfDisabled = true)]
        [Display("Split Cal", Groups: new[] { "Measurement Class Calibration", "Miscellaneous" }, Order: 23)]
        public bool SplitCal { get; set; }

        [EnabledIf("IsPhaseEnabled", true, HideIfDisabled = true)]
        [Display("Enable Phase Correction", Groups: new[] { "Measurement Class Calibration", "Phase" }, Order: 30)]
        public bool EnablePhaseCorrection { get; set; }

        [EnabledIf("IsPowerCalEnabled", true, HideIfDisabled = true)]
        [Display("Include Power Calibration", Groups: new[] { "Measurement Class Calibration", "Power Cal" }, Order: 40)]
        public bool IncludePowerCalibration { get; set; }

        [EnabledIf("IsIMDEnabled", true, HideIfDisabled = true)]
        [Display("Max Product Order", Groups: new[] { "Measurement Class Calibration", "IMD" }, Order: 50)]
        public int MaxProductOrder { get; set; }

        [EnabledIf("IsIMDEnabled", true, HideIfDisabled = true)]
        [Display("Include 2nd Order", Groups: new[] { "Measurement Class Calibration", "IMD" }, Order: 51)]
        public bool Include2ndOrder { get; set; }

        [EnabledIf("IsNoiseEnabled", true, HideIfDisabled = true)]
        [Display("Noise Cal Method", Groups: new[] { "Measurement Class Calibration", "Noise" }, Order: 60)]
        public NoiseCalMethodEnum NoiseCalMethod { get; set; }

        // AutoOrient Tuner
        [EnabledIf("IsNoiseEnabled", true, HideIfDisabled = true)]
        [EnabledIf("NoiseCalMethod", NoiseCalMethodEnum.Vector, HideIfDisabled = true)]
        [Display("Noise Tuner", Groups: new[] { "Measurement Class Calibration", "Noise" }, Order: 61)]
        public NoiseTunerEnum NoiseTuner { get; set; }

        [EnabledIf("IsNoiseEnabled", true, HideIfDisabled = true)]
        [Display("Receiver Characterization Method", Groups: new[] { "Measurement Class Calibration", "Noise" }, Order: 62)]
        public ReceiverCharacterizationMethodEnum ReceiverCharacterizationMethod { get; set; }

        [EnabledIf("IsNoiseEnabled", true, HideIfDisabled = true)]
        [Display("Force Thru Adapter De-embed", Groups: new[] { "Measurement Class Calibration", "Noise" }, Order: 63)]
        public bool ForceThruAdapter { get; set; }

        [EnabledIf("IsNoiseEnabled", true, HideIfDisabled = true)]
        [Display("Force Power Sensor Adapter De-embed", Groups: new[] { "Measurement Class Calibration", "Noise" }, Order: 64)]
        public bool ForcePowerSensor { get; set; }


        private DUTConnectorsEnum _Port1;
        [Display("Port 1", Groups: new[] { "DUT Connectors and Cal Kits", "DUT Connectors" }, Order: 100)]
        public DUTConnectorsEnum Port1
        {
            get
            {
                return _Port1;
            }
            set
            {
                _Port1 = value;
                if (_Port1 == DUTConnectorsEnum.Notused)
                {
                    IsPort1CalKitEnabled = false;
                }
                else
                {
                    IsPort1CalKitEnabled = true;
                }
            }
        }

        private DUTConnectorsEnum _Port2;
        [Display("Port 2", Groups: new[] { "DUT Connectors and Cal Kits", "DUT Connectors" }, Order: 101)]
        public DUTConnectorsEnum Port2
        {
            get
            {
                return _Port2;
            }
            set
            {
                _Port2 = value;
                if (_Port2 == DUTConnectorsEnum.Notused)
                {
                    IsPort2CalKitEnabled = false;
                }
                else
                {
                    IsPort2CalKitEnabled = true;
                }
            }
        }


        private DUTConnectorsEnum _Port3;
        [Display("Port 3", Groups: new[] { "DUT Connectors and Cal Kits", "DUT Connectors" }, Order: 102)]
        public DUTConnectorsEnum Port3
        {
            get
            {
                return _Port3;
            }
            set
            {
                _Port3 = value;
                if (_Port3 == DUTConnectorsEnum.Notused)
                {
                    IsPort3CalKitEnabled = false;
                }
                else
                {
                    IsPort3CalKitEnabled = true;
                }
            }
        }

        private DUTConnectorsEnum _Port4;
        [Display("Port 4", Groups: new[] { "DUT Connectors and Cal Kits", "DUT Connectors" }, Order: 103)]
        public DUTConnectorsEnum Port4
        {
            get
            {
                return _Port4;
            }
            set
            {
                _Port4 = value;
                if (_Port4 == DUTConnectorsEnum.Notused)
                {
                    IsPort4CalKitEnabled = false;
                }
                else
                {
                    IsPort4CalKitEnabled = true;
                }
            }
        }

        [Browsable(false)]
        public bool IsPort1CalKitEnabled { get; set; }
        [Browsable(false)]
        public bool IsPort2CalKitEnabled { get; set; }
        [Browsable(false)]
        public bool IsPort3CalKitEnabled { get; set; }
        [Browsable(false)]
        public bool IsPort4CalKitEnabled { get; set; }

        [EnabledIf("IsPort1CalKitEnabled", true, HideIfDisabled = true)]
        [Display("Port 1", Groups: new[] { "DUT Connectors and Cal Kits", "Cal Kits" }, Order: 110)]
        public String Port1CalKit { get; set; }

        [Browsable(true)]
        [EnabledIf("IsPort1CalKitEnabled", true, HideIfDisabled = true)]
        [Display("Query Port 1 Cal Kits", Groups: new[] { "DUT Connectors and Cal Kits", "Cal Kits" }, Order: 110.1)]
        public void QueryCalKitsPort1()
        {
            if (PNAX.IsConnected)
            {
                Log.Info("Disconnect before querying cal kits!");
                return;
            }
            String retString = QueryCalKits(Port1);
            if (retString != "")
            {
                Port1CalKit = retString;
            }
        }


        [EnabledIf("IsPort2CalKitEnabled", true, HideIfDisabled = true)]
        [Display("Port 2", Groups: new[] { "DUT Connectors and Cal Kits", "Cal Kits" }, Order: 111)]
        public String Port2CalKit { get; set; }

        [Browsable(true)]
        [EnabledIf("IsPort2CalKitEnabled", true, HideIfDisabled = true)]
        [Display("Query Port 2 Cal Kits", Groups: new[] { "DUT Connectors and Cal Kits", "Cal Kits" }, Order: 111.1)]
        public void QueryCalKitsPort2()
        {
            if (PNAX.IsConnected)
            {
                Log.Info("Disconnect before querying cal kits!");
                return;
            }
            String retString = QueryCalKits(Port2);
            if (retString != "")
            {
                Port2CalKit = retString;
            }
        }

        [EnabledIf("IsPort3CalKitEnabled", true, HideIfDisabled = true)]
        [Display("Port 3", Groups: new[] { "DUT Connectors and Cal Kits", "Cal Kits" }, Order: 112)]
        public String Port3CalKit { get; set; }

        [Browsable(true)]
        [EnabledIf("IsPort3CalKitEnabled", true, HideIfDisabled = true)]
        [Display("Query Port 3 Cal Kits", Groups: new[] { "DUT Connectors and Cal Kits", "Cal Kits" }, Order: 112.1)]
        public void QueryCalKitsPort3()
        {
            if (PNAX.IsConnected)
            {
                Log.Info("Disconnect before querying cal kits!");
                return;
            }
            String retString = QueryCalKits(Port3);
            if (retString != "")
            {
                Port3CalKit = retString;
            }
        }

        [EnabledIf("IsPort4CalKitEnabled", true, HideIfDisabled = true)]
        [Display("Port 4", Groups: new[] { "DUT Connectors and Cal Kits", "Cal Kits" }, Order: 113)]
        public String Port4CalKit { get; set; }

        [Browsable(true)]
        [EnabledIf("IsPort4CalKitEnabled", true, HideIfDisabled = true)]
        [Display("Query Port 4 Cal Kits", Groups: new[] { "DUT Connectors and Cal Kits", "Cal Kits" }, Order: 113.1)]
        public void QueryCalKitsPort4()
        {
            if (PNAX.IsConnected)
            {
                Log.Info("Disconnect before querying cal kits!");
                return;
            }
            String retString = QueryCalKits(Port4);
            if (retString != "")
            {
                Port4CalKit = retString;
            }
        }


        [Display("Show Pictures", Groups: new[] { "Picture" }, Description: "The dialog will include a picture if the environment supports it.", Order: 120, Collapsed: true)]
        public bool ShowPicture { get; set; }


        [Display("Pictures", Groups: new[] { "Picture" }, Description: "List of Picture Source and Description", Order: 121, Collapsed: false)]
        [EnabledIf(nameof(ShowPicture), HideIfDisabled = true)]
        public List<PictureDefinition> PictureList { get; set; }

        private bool _headlessMode = false;
        [Display("Headless Mode", Groups: new[] { "Headless Mode" }, Description: "Settings to use in headless mode", Order: 122, Collapsed: false)]
        public bool HeadlessMode { get => _headlessMode; set => _headlessMode = value; }

        [Display("Automatically select Cal Kit", Groups: new[] { "Headless Mode" }, Order: 127)]
        [EnabledIf(nameof(HeadlessMode), HideIfDisabled = true)]
        public bool AutomaticallySelectCalKit { get; set; }

        [Display("Include Power Calibration", Groups: new[] { "Headless Mode" }, Order: 128)]
        [EnabledIf(nameof(HeadlessMode), HideIfDisabled = true)]
        [ExternalParameter]
        public bool HeadlessPowerCalibration { get; set; }

        #endregion

        private String QueryCalKits(DUTConnectorsEnum PortConn)
        {
            try
            {
                PNAX.Open();
                Log.Info("Calculating Input values");

                int CalChannel = PNAX.CalAllGuidedChannelNumber();

                String kits = PNAX.CalAllGetCalKits(CalChannel, PortConn);

                var dialog = new SelectCalKitDialog(kits);
                UserInput.Request(dialog);

                // Response from the user.
                if (dialog.Response == WaitForInputResult.Cancel)
                {
                    Log.Info("Select cal kit aborted!");
                    PNAX.Close();
                    return "";
                }
                Log.Info("Selected cal kit: " + dialog.SelectedValue);

                PNAX.Close();
                return dialog.SelectedValue;
            }
            catch (Exception)
            {
                Log.Error("Cannot calcluate Input values!");
                return "";
            }
        }

        private void UpdateCalProperties()
        {
            bool DisablePowerCal = false;
            IsAnyCalEnabled = false;
            IsSplitCalEnabled = false;
            IsPhaseEnabled = false;
            IsPowerCalEnabled = false;
            IsIMDEnabled = false;
            IsNoiseEnabled = false;

            foreach (CalibrateAllSelectedChannels cal in _CalAllSelectedChannels)
            {
                IsAnyCalEnabled = true;

                if ((cal.ChannelType == ChannelTypeEnum.ScalarMixerConverter) ||
                    (cal.ChannelType == ChannelTypeEnum.GainCompressionConverters))
                {
                    IsSplitCalEnabled = true;
                }
                else if (cal.ChannelType == ChannelTypeEnum.ScalarMixerConverter)
                {
                    IsPhaseEnabled = true;
                }
                else if ((cal.ChannelType == ChannelTypeEnum.SweptIMD) ||
                    (cal.ChannelType == ChannelTypeEnum.SweptIMDConverters))
                {
                    IsIMDEnabled = true;
                }
                else if ((cal.ChannelType == ChannelTypeEnum.NoiseFigureColdSource) ||
                    (cal.ChannelType == ChannelTypeEnum.NoiseFigureConverters))
                {
                    IsNoiseEnabled = true;
                }

                // PowerCal is enabled if Standard Channel is the only Channel defined
                if (cal.ChannelType == ChannelTypeEnum.Standard)
                {
                    IsPowerCalEnabled = true;
                }
                else
                {
                    DisablePowerCal = true;
                }
            }

            // Found at least one channel that is not Standard
            if (DisablePowerCal)
            {
                IsPowerCalEnabled = false;
            }
        }

        private void QueryChannels()
        {
            try
            {
                PNAX.Open();
                Log.Info("Querying Channels from Instrument");
                List<CalibrateAllSelectedChannels> channelsFromInstrument = new List<CalibrateAllSelectedChannels>();

                // Query channels
                List<int> Channels = PNAX.GetActiveChannels();

                // for each channel query its type
                foreach (int ch in Channels)
                {
                    String measName = PNAX.GetChannelType(ch);

                    // now build the list
                    CalibrateAllSelectedChannels selectedChannel = new CalibrateAllSelectedChannels();
                    selectedChannel.Channel = ch;
                    selectedChannel.ChannelType = PNAX.ConvertStringToEnum<ChannelTypeEnum>(measName);
                    selectedChannel.Ports = new List<int>() { 1, 2 };
                    channelsFromInstrument.Add(selectedChannel);
                }

                CalAllSelectedChannels = channelsFromInstrument;

                PNAX.Close();
            }
            catch (Exception ex)
            {
                if (PNAX.IsConnected)
                {
                    PNAX.Close();
                }
                Log.Error("Cannot query channels!");
                return;
            }

        }

        public CalAll()
        {
            AutoSelectChannels = true;
            CalChannels = new List<int> { 1, 2 };
            UserSmartCalOrder = false;
            IndependentCalibrationChannels = new List<int>();
            ExtraPowerCals = ExtraPowerCalsEnum.NoIndependentSourceCal;
            SplitCal = false;
            IncludePowerCalibration = false;

            PowerMeterSettlingTolerance = 0.050;
            PowerMeterSettlingReadings = 3;
            PowerMeterPowerLevel = 0;

            EnablePhaseCorrection = false;

            MaxProductOrder = 3;
            Include2ndOrder = false;

            NoiseCalMethod = NoiseCalMethodEnum.Scalar;
            NoiseTuner = NoiseTunerEnum._internal;
            ReceiverCharacterizationMethod = ReceiverCharacterizationMethodEnum.UsePowerMeter;
            ForceThruAdapter = false;
            ForcePowerSensor = false;

            CalAllSelectedChannels = new List<CalibrateAllSelectedChannels>();

            Port1 = DUTConnectorsEnum.Notused;
            Port2 = DUTConnectorsEnum.Notused;
            Port3 = DUTConnectorsEnum.Notused;
            Port4 = DUTConnectorsEnum.Notused;

            Port1CalKit = "85032F";
            Port2CalKit = "85032F";
            Port3CalKit = "85032F";
            Port4CalKit = "85032F";

            CalSetName = "MyCalSet";

            ShowPicture = false;
            PictureList = new List<PictureDefinition>();
        }

        private String ExtraPowerCalsToString(ExtraPowerCalsEnum value)
        {
            String retString = "";
            List<String> values = new List<string>();
            int InValue = (int)value;
            if (InValue == 0)
            {
                // no option is selected
                return retString;
            }

            if ((InValue & (int)ExtraPowerCalsEnum.Port1) > 0)
            {
                values.Add("Port 1");
            }
            if ((InValue & (int)ExtraPowerCalsEnum.Port2) > 0)
            {
                values.Add("Port 2");
            }
            if ((InValue & (int)ExtraPowerCalsEnum.Port3) > 0)
            {
                values.Add("Port 3");
            }
            if ((InValue & (int)ExtraPowerCalsEnum.Port4) > 0)
            {
                values.Add("Port 4");
            }
            if ((InValue & (int)ExtraPowerCalsEnum.Port1Src2) > 0)
            {
                values.Add("Port 1 Src2");
            }
            if ((InValue & (int)ExtraPowerCalsEnum.Source3) > 0)
            {
                values.Add("Source3");
            }

            retString = String.Join(",", values.ToArray());
            return retString;
        }

        private Verdict DoHeadlessModeTasks()
        {
            if (HeadlessMode)
            {
                // query the calkit for this type of connector and add it automatically
                const string defaultKit = "85032F";
                if (IsPort1CalKitEnabled)
                {
                    int CalChannel = PNAX.CalAllGuidedChannelNumber();

                    // Select Connector Type
                    String strDutPort1 = Scpi.Format("{0}", Port1);
                    PNAX.CalAllSelectDutConnectorType(CalChannel, 1, strDutPort1);

                    // Get Kits
                    string kits = PNAX.CalAllGetCalKits(CalChannel, Port1);
                    string selectedKit = kits.Split(',').FirstOrDefault(x => !x.Contains(defaultKit)).Trim(' ', '"');
                    selectedKit = selectedKit == "" ? defaultKit : selectedKit;
                    Port1CalKit = selectedKit;
                    Log.Debug($"Port1 calkit set to: {selectedKit}");

                    if (selectedKit.Contains("ECal"))
                    {
                        // We selecte a valid ECal calkit
                        Log.Info($"Valid ECal found for Port 1: {selectedKit}");
                    }
                    else
                    {
                        // Looks like we don't have a valid ECAL for Port 1 - Fail 
                        Log.Info($"Could not find ECal for Port 1 connector type: {Port1}");
                        return Verdict.Fail;
                    }
                }
                if (IsPort2CalKitEnabled)
                {
                    int CalChannel = PNAX.CalAllGuidedChannelNumber();

                    // Select Connector Type
                    String strDutPort2 = Scpi.Format("{0}", Port2);
                    PNAX.CalAllSelectDutConnectorType(CalChannel, 2, strDutPort2);

                    // Get Kits
                    string kits = PNAX.CalAllGetCalKits(CalChannel, Port2);
                    string selectedKit = kits.Split(',').FirstOrDefault(x => !x.Contains(defaultKit)).Trim(' ', '"');
                    selectedKit = selectedKit == "" ? defaultKit : selectedKit;
                    Port2CalKit = selectedKit;
                    Log.Debug($"Port2 calkit set to: {selectedKit}");

                    if (selectedKit.Contains("ECal"))
                    {
                        // We selecte a valid ECal calkit
                        Log.Info($"Valid ECal found for Port 2: {selectedKit}");
                    }
                    else
                    {
                        // Looks like we don't have a valid ECAL for Port 2 - Fail 
                        Log.Info($"Could not find ECal for Port 2 connector type: {Port2}");
                        return Verdict.Fail;
                    }
                }
                if (IsPort3CalKitEnabled)
                {
                    int CalChannel = PNAX.CalAllGuidedChannelNumber();

                    // Select Connector Type
                    String strDutPort3 = Scpi.Format("{0}", Port3);
                    PNAX.CalAllSelectDutConnectorType(CalChannel, 3, strDutPort3);

                    // Get Kits
                    string kits = PNAX.CalAllGetCalKits(CalChannel, Port3);
                    string selectedKit = kits.Split(',').FirstOrDefault(x => !x.Contains(defaultKit)).Trim(' ', '"');
                    selectedKit = selectedKit == "" ? defaultKit : selectedKit;
                    Port3CalKit = selectedKit;
                    Log.Debug($"Port3 calkit set to: {selectedKit}");

                    if (selectedKit.Contains("ECal"))
                    {
                        // We selecte a valid ECal calkit
                        Log.Info($"Valid ECal found for Port 3: {selectedKit}");
                    }
                    else
                    {
                        // Looks like we don't have a valid ECAL for Port 3 - Fail 
                        Log.Info($"Could not find ECal for Port 3 connector type: {Port3}");
                        return Verdict.Fail;
                    }
                }
                if (IsPort4CalKitEnabled)
                {
                    int CalChannel = PNAX.CalAllGuidedChannelNumber();

                    // Select Connector Type
                    String strDutPort4 = Scpi.Format("{0}", Port4);
                    PNAX.CalAllSelectDutConnectorType(CalChannel, 4, strDutPort4);

                    // Get Kits
                    string kits = PNAX.CalAllGetCalKits(CalChannel, Port4);
                    string selectedKit = kits.Split(',').FirstOrDefault(x => !x.Contains(defaultKit)).Trim(' ', '"');
                    selectedKit = selectedKit == "" ? defaultKit : selectedKit;
                    Port4CalKit = selectedKit;
                    Log.Debug($"Port4 calkit set to: {selectedKit}");

                    if (selectedKit.Contains("ECal"))
                    {
                        // We selecte a valid ECal calkit
                        Log.Info($"Valid ECal found for Port 4: {selectedKit}");
                    }
                    else
                    {
                        // Looks like we don't have a valid ECAL for Port 4 - Fail 
                        Log.Info($"Could not find ECal for Port 4 connector type: {Port4}");
                        return Verdict.Fail;
                    }
                }

                // enable Power Calibration if appropriate
                if (IsPowerCalEnabled)
                {
                    PNAX.CalAllSetProperty("Include Power Calibration", HeadlessPowerCalibration.ToString());
                    Log.Debug($"Power calibration set");
                }
                return Verdict.Pass;
            }
            return Verdict.Pass;
        }

        public override void Run()
        {
            if (DoHeadlessModeTasks() == Verdict.Fail)
            {
                Log.Info("Headless task failed!");
                UpgradeVerdict(Verdict.Fail);
                return;
            }

            PNAX.CalAllReset();

            if (AutoSelectChannels)
            {
                // Set Channels and Ports according to values on the instrument
                // Query channels
                List<int> Channels = PNAX.GetActiveChannels();

                // for each channel query its type
                foreach (int ch in Channels)
                {
                    PNAX.CalAllSelectPorts(ch, CalChannels);
                }
            }
            else
            {
                // Set Channels and Ports for each channel using List provided by user
                foreach (CalibrateAllSelectedChannels cal in _CalAllSelectedChannels)
                {
                    PNAX.CalAllSelectPorts(cal.Channel, cal.Ports);
                }
            }

            // Set Properties according to the selected Channel types
            String ExtraPowerCalsString = "";
            if (IsAnyCalEnabled)
            {
                // Misc
                PNAX.CalAllSetProperty("Use Smart Cal Order", UserSmartCalOrder.ToString());
                ExtraPowerCalsString = ExtraPowerCalsToString(ExtraPowerCals);
                if (ExtraPowerCalsString != "")
                {
                    PNAX.CalAllSetProperty("Enable Extra Power Cals", ExtraPowerCalsString);

                    string psen = PNAX.ReadConnectedSensor();
                    PNAX.SetPowerSensor(PNAPowerMeterEnumtype.Usb, psen);

                    PNAX.PowerMeterSettlingTolerance(PowerMeterSettlingTolerance);
                    PNAX.PowerMeterSettlingMaxReadings(PowerMeterSettlingReadings);

                }
                String IndependentCalibrationChannelsString = String.Join(",", IndependentCalibrationChannels);
                if (IndependentCalibrationChannelsString != "")
                {
                    PNAX.CalAllSetProperty("Independent Calibration Channels", IndependentCalibrationChannelsString);
                }
            }
            if (IsSplitCalEnabled)
            {
                PNAX.CalAllSetProperty("Split Cal", SplitCal.ToString());
            }
            if (IsPhaseEnabled)
            {
                PNAX.CalAllSetProperty("Enable Phase Correction", EnablePhaseCorrection.ToString());
            }
            //if (IsPowerCalEnabled)
            //{
            PNAX.CalAllSetProperty("Include Power Calibration", IncludePowerCalibration.ToString());
            //}
            if (IsIMDEnabled)
            {
                PNAX.CalAllSetProperty("Max Product Order", MaxProductOrder.ToString());
                PNAX.CalAllSetProperty("Include 2nd Order", Include2ndOrder.ToString());
            }
            if (IsNoiseEnabled)
            {
                PNAX.CalAllSetProperty("Noise Cal Method", NoiseCalMethod.ToString());
                if (NoiseCalMethod == NoiseCalMethodEnum.Vector)
                {
                    String noiseTunerString = Scpi.Format("{0}", NoiseTuner);
                    PNAX.CalAllSetProperty("AutoOrient Tuner", noiseTunerString);
                }
                String charMethodScpi = Scpi.Format("{0}", EnablePhaseCorrection);
                PNAX.CalAllSetProperty("Receiver Characterization Method", charMethodScpi);
                PNAX.CalAllSetProperty("Force Thru Adapter De-embed", ForceThruAdapter.ToString());
                PNAX.CalAllSetProperty("Force Power Sensor Adapter De-embed", ForcePowerSensor.ToString());
            }

            int CalChannel = PNAX.CalAllGuidedChannelNumber();

            String strDutPort1 = Scpi.Format("{0}", Port1);
            String strDutPort2 = Scpi.Format("{0}", Port2);
            String strDutPort3 = Scpi.Format("{0}", Port3);
            String strDutPort4 = Scpi.Format("{0}", Port4);
            PNAX.CalAllSelectDutConnectorType(CalChannel, 1, strDutPort1);
            PNAX.CalAllSelectDutConnectorType(CalChannel, 2, strDutPort2);
            if (strDutPort3 != "Not used") PNAX.CalAllSelectDutConnectorType(CalChannel, 3, strDutPort3);
            if (strDutPort4 != "Not used") PNAX.CalAllSelectDutConnectorType(CalChannel, 4, strDutPort4);

            String strPort1CalKit = Scpi.Format("{0}", Port1CalKit);
            String strPort2CalKit = Scpi.Format("{0}", Port2CalKit);
            String strPort3CalKit = Scpi.Format("{0}", Port3CalKit);
            String strPort4CalKit = Scpi.Format("{0}", Port4CalKit);
            if (strDutPort1 != "Not used") PNAX.CalAllSelectCalKit(CalChannel, 1, Port1CalKit);
            if (strDutPort2 != "Not used") PNAX.CalAllSelectCalKit(CalChannel, 2, Port2CalKit);
            if (strDutPort3 != "Not used") PNAX.CalAllSelectCalKit(CalChannel, 3, Port3CalKit);
            if (strDutPort4 != "Not used") PNAX.CalAllSelectCalKit(CalChannel, 4, Port4CalKit);

            if (IsAnyCalEnabled)
            {
                if (ExtraPowerCalsString != "")
                {
                    PNAX.PowerMeterPowerLevel(CalChannel, 1, PowerMeterPowerLevel);
                    PNAX.PowerMeterSensorEnable(CalChannel, 1, true);
                }
            }

            PNAX.CalAllInit(CalChannel, CalSetName);

            int CalSteps = PNAX.CalAllNumberOfSteps(CalChannel);

            int deftimeout = PNAX.IoTimeout;
            PNAX.IoTimeout = 1200000;

            // Make sure the number of steps matches the number of images defined
            if (ShowPicture)
            {
                if (PictureList.Count != CalSteps)
                {
                    Log.Error("The number of defined images do not match the number of steps in calibration, make sure the images match the requested calibration");
                    UpgradeVerdict(Verdict.Error);
                    return;
                }
            }

            for (int CalStep = 1; CalStep <= CalSteps; CalStep++)
            {
                String StepDescription = PNAX.CalAllStepDescription(CalChannel, CalStep);

                Log.Info($"Step {CalStep}: {StepDescription}");

                CalStepDialog dialog = new CalStepDialog(CalStep, CalSteps, StepDescription) { Picture = null };

                if (ShowPicture)
                {
                    PictureDefinition pic = PictureList.ElementAt(CalStep - 1);
                    dialog.Picture = pic.Picture;
                }

                UserInput.Request(dialog);

                // Response from the user.
                if (dialog.Response == WaitForInputResult.Cancel)
                {
                    Log.Info("Cal All aborted!");
                    UpgradeVerdict(Verdict.Aborted);
                    return;
                }

                PNAX.CalAllStep(CalChannel, CalStep);
            }
            PNAX.IoTimeout = deftimeout;

            PNAX.CalAllSave(CalChannel);

            if (PNAX.SimulatorMode() > 0)
            {
                //Creates a unity Cal Set
                //PNAX.ScpiCommand("SENSe<cnum>:CORRection:CSET:CREate:DEFault \"CH1_CALREG\", [<correctiontype>]");
                PNAX.ScpiCommand("sense:correction:cset:deactivate");
                PNAX.ScpiCommand($"SENS:CORR:CSET:DEL '{CalSetName}'");
                PNAX.ScpiCommand($"SENS:CORR:CSET:CRE:DEF '{CalSetName}','Full 2P(1,2)'");
            }

            UpgradeVerdict(Verdict.Pass);
        }
    }

    public enum WaitForInputResult
    {
        // The number assigned, determines the order in which the buttons are shown in the dialog.
        Cancel = 2, Ok = 1
    }

    class CalStepDialog : IDisplayAnnotation
    {
        public CalStepDialog(int stepNumber, int totalSteps, String stepDescription)
        {
            StepNumber = stepNumber;
            TotalSteps = totalSteps;
            StepDescription = stepDescription;
        }

        [Browsable(false)]
        public int StepNumber { get; set; }
        [Browsable(false)]
        public int TotalSteps { get; set; }
        [Browsable(false)]
        public String StepDescription { get; set; }

        [Browsable(false)]
        public bool PictureEnabled => Picture != null;

        [Layout(LayoutMode.FullRow, rowHeight: 2)]
        [Display("Picture", Order: 2)]
        [EnabledIf(nameof(PictureEnabled), HideIfDisabled = true)]
        public Picture Picture { get; set; }

        // Name is handled specially to create the title of the dialog window.
        public string Name { get { return "Step " + StepNumber + " of " + TotalSteps; } }

        [Layout(LayoutMode.FullRow, rowHeight: 2)]
        [Browsable(true)] // Show it event though it is read-only.
        [Display("Message", Order: 1)]
        public string Message { get { return StepDescription; } }

        [Layout(LayoutMode.FloatBottom | LayoutMode.FullRow)] // Show the button selection at the bottom of the window.
        [Submit] // When the button is clicked the result is 'submitted', so the dialog is closed.
        public WaitForInputResult Response { get; set; }

        string IDisplayAnnotation.Description => string.Empty;

        string[] IDisplayAnnotation.Group => Array.Empty<string>();

        double IDisplayAnnotation.Order => -10000;

        bool IDisplayAnnotation.Collapsed => false;
    }

    class SelectCalKitDialog : IDisplayAnnotation
    {
        public SelectCalKitDialog(String AvailableKits)
        {
            _AvailableKits = AvailableKits;
            _AvailableKits = _AvailableKits.Replace("\"", "");
            //_AvailableKits = _AvailableKits.Replace(" ", "");
            List<String> avail = _AvailableKits.Split(',').ToList();
            List<String> TempString = avail;
            CalKits = TempString.AsReadOnly();
        }

        [Browsable(false)]
        public String _AvailableKits { get; set; }

        public string Name { get { return "Select Cal Kit"; } }


        //Collection of ports
        private ReadOnlyCollection<String> _CalKits;
        [Layout(LayoutMode.FullRow)] // Set the layout of the property to fill the entire row.
        [Browsable(false)] // Show it event though it is read-only.
        public ReadOnlyCollection<String> CalKits
        {
            get
            {
                return _CalKits;
            }
            set
            {
                _CalKits = value;
                //OnPropertyChanged("CalKits");
            }
        }

        [Display("Selected Value", "Select the cal kit from the list of available values")]
        [AvailableValues(nameof(CalKits))]
        public string SelectedValue { get; set; }


        [Layout(LayoutMode.FloatBottom | LayoutMode.FullRow)] // Show the button selection at the bottom of the window.
        [Submit] // When the button is clicked the result is 'submitted', so the dialog is closed.
        public WaitForInputResult Response { get; set; }

        string IDisplayAnnotation.Description => string.Empty;

        string[] IDisplayAnnotation.Group => Array.Empty<string>();

        double IDisplayAnnotation.Order => -10000;

        bool IDisplayAnnotation.Collapsed => false;
    }

    class SelectConnectorTypeDialog : IDisplayAnnotation
    {
        public SelectConnectorTypeDialog(DUTConnectorsEnum Port1Input, DUTConnectorsEnum Port2Input, DUTConnectorsEnum Port3Input, DUTConnectorsEnum Port4Input)
        {
            Port1 = Port1Input;
            Port2 = Port2Input;
            Port3 = Port3Input;
            Port4 = Port4Input;
        }


        public string Name { get { return "Select Connector Types"; } }



        [Display("Port 1 Connector", "Select the connector type for Port 1")]
        public DUTConnectorsEnum Port1 { get; set; }

        [Display("Port 2 Connector", "Select the connector type for Port 2")]
        public DUTConnectorsEnum Port2 { get; set; }

        [Display("Port 3 Connector", "Select the connector type for Port 3")]
        public DUTConnectorsEnum Port3 { get; set; }

        [Display("Port 4 Connector", "Select the connector type for Port 4")]
        public DUTConnectorsEnum Port4 { get; set; }


        [Layout(LayoutMode.FloatBottom | LayoutMode.FullRow)] // Show the button selection at the bottom of the window.
        [Submit] // When the button is clicked the result is 'submitted', so the dialog is closed.
        public WaitForInputResult Response { get; set; }

        string IDisplayAnnotation.Description => string.Empty;

        string[] IDisplayAnnotation.Group => Array.Empty<string>();

        double IDisplayAnnotation.Order => -10000;

        bool IDisplayAnnotation.Collapsed => false;
    }

    class CalStepErrorMessageDialog : IDisplayAnnotation
    {
        public CalStepErrorMessageDialog(String stepDescription)
        {
            StepDescription = stepDescription;
        }

        [Browsable(false)]
        public String StepDescription { get; set; }

        // Name is handled specially to create the title of the dialog window.
        public string Name { get { return "Error selecting Connector Type and Calkit"; } }

        [Layout(LayoutMode.FullRow, rowHeight: 2)]
        [Browsable(true)] // Show it event though it is read-only.
        [Display("Message", Order: 1)]
        public string Message { get { return StepDescription; } }

        [Layout(LayoutMode.FloatBottom | LayoutMode.FullRow)] // Show the button selection at the bottom of the window.
        [Submit] // When the button is clicked the result is 'submitted', so the dialog is closed.
        public WaitForInputResult Response { get; set; }

        string IDisplayAnnotation.Description => string.Empty;

        string[] IDisplayAnnotation.Group => Array.Empty<string>();

        double IDisplayAnnotation.Order => -10000;

        bool IDisplayAnnotation.Collapsed => false;
    }
}
