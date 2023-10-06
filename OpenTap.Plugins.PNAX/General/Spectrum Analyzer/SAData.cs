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
    [Display("SA Data", Groups: new[] { "PNA-X", "General", "Spectrum Analyzer" }, Description: "Insert a description here")]
    public class SAData : GeneralBaseStep
    {
        #region Settings

        [Display("Data Format", Groups: new[] { "Data" }, Order: 20)]
        public SADataTypeEnum DataFormat { get; set; }
        [Display("Export Receivers", Groups: new[] { "Data" }, Order: 21)]
        public String ExportReceivers { get; set; }
        [Display("Don't save data below threshold", Groups: new[] { "Data" }, Order: 22)]
        public bool ThresholdState { get; set; }
        [Display("Value", Groups: new[] { "Data" }, Order: 23)]
        public double ThresholdValue { get; set; }

        [Display("Export to binary file", Groups: new[] { "Export" }, Order: 30)]
        public bool ExportToBinaryState { get; set; }

        [Display("Export to text file", Groups: new[] { "Export" }, Order: 31)]
        public bool ExportToTextState { get; set; }
        [Display("Verbose Mode", Groups: new[] { "Export" }, Order: 32)]
        public bool VerboseModeState { get; set; }
        [Display("Export markers with data files", Groups: new[] { "Export" }, Order: 33)]
        public bool ExportMarkersState { get; set; }
        [Display("Export all markers to a single file", Groups: new[] { "Export" }, Order: 34)]
        public bool ExportAllMarkersState { get; set; }
        [Display("Erase files each new sweep", Groups: new[] { "Export" }, Order: 35)]
        public bool EraseFilesState { get; set; }
        [Display("File name prefix", Groups: new[] { "Export" }, Order: 36)]
        public String FileName { get; set; }

        [Display("Export to FIFO buffer", Groups: new[] { "Export" }, Order: 40)]
        public bool ExportFifoState { get; set; }
        [Display("Export to shared memory", Groups: new[] { "Export" }, Order: 41)]
        public bool ExportToSharedMemoryState { get; set; }
        [Display("Share name", Groups: new[] { "Export" }, Order: 42)]
        public String ShareName { get; set; }

        [Display("Export IQ data to binary file", Groups: new[] { "Export IQ data" }, Order: 50)]
        public bool ExportIQToBinaryState { get; set; }
        [Display("Export IQ to text file", Groups: new[] { "Export IQ data" }, Order: 51)]
        public bool ExportIQToTextState { get; set; }

        #endregion

        public SAData()
        {
            DataFormat = SADataTypeEnum.FloatLogMagdBm;
            ExportReceivers = "All";
            ThresholdState = false;
            ThresholdValue = -60;

            ExportToBinaryState = false;
            ExportToTextState = false;
            VerboseModeState = false;
            ExportMarkersState = false;
            ExportAllMarkersState = false;
            EraseFilesState = true;
            FileName = @"C:\TEMP\SA_DATA_OUT";

            ExportFifoState = false;
            ExportToSharedMemoryState = false;
            ShareName = "PNA_Share";
            ExportIQToBinaryState = false;
            ExportIQToTextState = false;
        }

        [Browsable(false)]
        public override List<(string, object)> GetMetaData()
        {
            List<(String, object)> retVal = new List<(string, object)>();

            return retVal;
        }

        private SAOnOffTypeEnum ConvertBoolToOnOff(bool state)
        {
            SAOnOffTypeEnum retVal = SAOnOffTypeEnum.Off;
            if (state == true)
            {
                retVal = SAOnOffTypeEnum.On;
            }
            return retVal;
        }
        public override void Run()
        {
            PNAX.SADataType(Channel, DataFormat);
            PNAX.SADataReceiversList(Channel, ExportReceivers);
            PNAX.SADataThresholdState(Channel, ConvertBoolToOnOff(ThresholdState));
            PNAX.SADataThresholdValue(Channel, ThresholdValue);

            PNAX.SADataFileBinaryState(Channel, ConvertBoolToOnOff(ExportToBinaryState));
            PNAX.SADataFileTextState(Channel, ConvertBoolToOnOff(ExportToTextState));
            PNAX.SADataFileTextVerboseState(Channel, ConvertBoolToOnOff(VerboseModeState));
            PNAX.SADataFileTextMarkersState(Channel, ConvertBoolToOnOff(ExportMarkersState));
            PNAX.SADataFileTextMarkersAllState(Channel, ConvertBoolToOnOff(ExportAllMarkersState));
            PNAX.SADataFileEraseState(Channel, ConvertBoolToOnOff(EraseFilesState));
            PNAX.SADataFilePrefix(Channel, FileName);

            PNAX.SADataFifoState(Channel, ConvertBoolToOnOff(ExportFifoState));
            PNAX.SADataSharedState(Channel, ConvertBoolToOnOff(ExportToSharedMemoryState));
            PNAX.SADataSharedName(Channel, ShareName);

            PNAX.SADataIQFileBinaryState(Channel, ConvertBoolToOnOff(ExportIQToBinaryState));
            PNAX.SADataIQFileTextState(Channel, ConvertBoolToOnOff(ExportIQToTextState));

            UpgradeVerdict(Verdict.Pass);
        }
    }
}
