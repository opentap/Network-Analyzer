// Author: CMontes
// Copyright:   Copyright 2024 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files..

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using OpenTap;

namespace OpenTap.Plugins.PNAX
{
    [Display("GenericDUT", Group: "OpenTap.Plugins.PNAX", Description: "Add a description here")]
    public class GenericDUT : Dut
    {
        #region Settings
        [MetaData(true)]
        [Display("Wafer ID", Order: 1)]
        public String WaferID { get; set; }

        [MetaData(true)]
        [Display("Chip ID", Order: 1)]
        public String ChipID { get; set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of this DUT class.
        /// </summary>
        public GenericDUT()
        {
            Name = "Simple DUT";

            WaferID = "WAF0001";
            ChipID = "CHIP0001";
        }

        /// <summary>
        /// Opens a connection to the DUT represented by this class
        /// </summary>
        public override void Open()
        {
            base.Open();
            // TODO: establish connection to DUT here
        }

        /// <summary>
        /// Closes the connection made to the DUT represented by this class
        /// </summary>
        public override void Close()
        {
            // TODO: close connection to DUT
            base.Close();
        }
    }
}
