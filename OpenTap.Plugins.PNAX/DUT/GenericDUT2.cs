// Author: CMontes
// Copyright:   Copyright 2024 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files..

using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OpenTap.Plugins.PNAX
{
    [Display("Generic DUT2", Group: "OpenTap.Plugins.PNAX", Description: "Generic DUT that includes Serial Number and Temperature for metadata")]
    public class GenericDUT2 : Dut
    {
        #region Settings
        [MetaData(true)]
        [Display("Serial Number", Order: 1)]
        public String SerialNumber { get; set; }

        [MetaData(true)]
        [Display("Temperature", Order: 2)]
        public int Temperature { get; set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of this DUT class.
        /// </summary>
        public GenericDUT2()
        {
            Name = "Generic DUT";

            SerialNumber = "0001";
            Temperature = 25;
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
