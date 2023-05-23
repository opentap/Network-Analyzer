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

namespace OpenTap.Plugins.PNAX
{


    [Display("Cal All", Groups: new[] { "PNA-X", "Calibration" }, Description: "Insert a description here")]
    public class CalAll : TestStep
    {
        #region Settings
        [Display("PNA", Order: 0.1)]
        public PNAX PNAX { get; set; }

        private List<CalibrateAllSelectedChannels> _CalAllSelectedChannels = new List<CalibrateAllSelectedChannels>();
        [Display("Calibrate All Selected Channels", Group: "Cal All", Order: 1)]
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

        [EnabledIf("IsSplitCalEnabled", true, HideIfDisabled = true)]
        [Display("Split Cal", Groups: new[] { "Measurement Class Calibration", "Miscellaneous" }, Order: 23)]
        public bool SplitCal { get; set; }

        [EnabledIf("IsPhaseEnabled", true, HideIfDisabled = true)]
        [Display("Enable Phase Correction", Groups: new[] { "Measurement Class Calibration", "Phase" }, Order: 30)]
        public bool EnablePhaseCorrection { get; set; }

        [EnabledIf("IsPowerCalEnabled", true, HideIfDisabled = true)]
        [Display("Include Power Calibration", Groups: new[] {"Measurement Class Calibration", "Power Cal" }, Order: 40)]
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
                if (_Port1 == DUTConnectorsEnum.Notused)
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

        [EnabledIf("IsPort2CalKitEnabled", true, HideIfDisabled = true)]
        [Display("Port 2", Groups: new[] { "DUT Connectors and Cal Kits", "Cal Kits" }, Order: 111)]
        public String Port2CalKit { get; set; }

        [EnabledIf("IsPort3CalKitEnabled", true, HideIfDisabled = true)]
        [Display("Port 3", Groups: new[] { "DUT Connectors and Cal Kits", "Cal Kits" }, Order: 112)]
        public String Port3CalKit { get; set; }

        [EnabledIf("IsPort4CalKitEnabled", true, HideIfDisabled = true)]
        [Display("Port 4", Groups: new[] { "DUT Connectors and Cal Kits", "Cal Kits" }, Order: 113)]
        public String Port4CalKit { get; set; }


        #endregion

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

                if ((cal.ChannelType == ChannelTypeEnum.SMC) ||
                    (cal.ChannelType == ChannelTypeEnum.GainCompressionConverters))
                {
                    IsSplitCalEnabled = true;
                }
                else if (cal.ChannelType == ChannelTypeEnum.SMC)
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



        public CalAll()
        {
            UserSmartCalOrder = false;
            IndependentCalibrationChannels = new List<int>();
            ExtraPowerCals = ExtraPowerCalsEnum.NoIndependentSourceCal;
            SplitCal = false;
            IncludePowerCalibration = false;

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

        public override void Run()
        {
            PNAX.CalAllReset();

            // Set Channels and Ports for each channel
            foreach (CalibrateAllSelectedChannels cal in _CalAllSelectedChannels)
            {
                PNAX.CalAllSelectPorts(cal.Channel, cal.Ports);
            }

            // Set Properties according to the selected Channel types
            if (IsAnyCalEnabled)
            {
                // Misc
                PNAX.CalAllSetProperty("Use Smart Cal Order", UserSmartCalOrder.ToString());
                String ExtraPowerCalsString = ExtraPowerCalsToString(ExtraPowerCals);
                if (ExtraPowerCalsString != "")
                {
                    PNAX.CalAllSetProperty("Enable Extra Power Cals", ExtraPowerCalsString);
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
            if (IsPowerCalEnabled)
            {
                PNAX.CalAllSetProperty("Include Power Calibration", IncludePowerCalibration.ToString());
            }
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
            PNAX.CalAllSelectDutConnectorType(CalChannel, 3, strDutPort3);
            PNAX.CalAllSelectDutConnectorType(CalChannel, 4, strDutPort4);

            String strPort1CalKit = Scpi.Format("{0}", Port1CalKit);
            String strPort2CalKit = Scpi.Format("{0}", Port2CalKit);
            String strPort3CalKit = Scpi.Format("{0}", Port3CalKit);
            String strPort4CalKit = Scpi.Format("{0}", Port4CalKit);
            if (strDutPort1 != "Not used") PNAX.CalAllSelectCalKit(CalChannel, 1, Port1CalKit);
            if (strDutPort2 != "Not used") PNAX.CalAllSelectCalKit(CalChannel, 2, Port2CalKit);
            if (strDutPort3 != "Not used") PNAX.CalAllSelectCalKit(CalChannel, 3, Port3CalKit);
            if (strDutPort4 != "Not used") PNAX.CalAllSelectCalKit(CalChannel, 4, Port4CalKit);

            PNAX.CalAllInit(CalChannel);

            int CalSteps = PNAX.CalAllNumberOfSteps(CalChannel);

            int deftimeout = PNAX.IoTimeout;
            PNAX.IoTimeout = 200000;

            for(int CalStep = 1; CalStep <= CalSteps; CalStep++)
            {
                String StepDescription = PNAX.CalAllStepDescription(CalChannel, CalStep);

                Log.Info($"Step {CalStep}: {StepDescription}");

                PNAX.CalAllStep(CalChannel, CalStep);
                //PNAX.WaitForOperationComplete();
            }
            PNAX.IoTimeout = deftimeout;

            PNAX.CalAllSave(CalChannel);

            //if (PNAX.SimulatorMode())
            //{
            // Creates a unity Cal Set
            // SENSe<cnum>:CORRection:CSET:CREate:DEFault [<csetname>], [<correctiontype>]
            // SENS:CORR:CSET:CRE:DEF 'My2P','Full 2P(1,2)'
            //}

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
