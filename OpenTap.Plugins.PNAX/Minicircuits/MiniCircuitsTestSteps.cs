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

namespace OpenTap.Plugins.PNAX.Minicircuits
{
    [Display("1. Converter Stages", Groups: new[] { "PNA-X", "Converters", "Swept IMD Converters", "Steps" }, Description: "Insert a description here")]
    public class MiniCircuitsTestSteps : TestStep
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public MiniCircuitsTestSteps()
        {
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.
        }
    }

    [Display("2. Input Port", Groups: new[] { "PNA-X", "Converters", "Swept IMD Converters", "Steps" }, Description: "Insert a description here")]
    public class InputPortMiniCircuitsTestSteps : TestStep
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public InputPortMiniCircuitsTestSteps()
        {
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.
        }
    }

    [Display("3. LO1 Port", Groups: new[] { "PNA-X", "Converters", "Swept IMD Converters", "Steps" }, Description: "Insert a description here")]
    public class LO1PortMiniCircuitsTestSteps : TestStep
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public LO1PortMiniCircuitsTestSteps()
        {
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.
        }
    }

    [Display("4. LO2 Port", Groups: new[] { "PNA-X", "Converters", "Swept IMD Converters", "Steps" }, Description: "Insert a description here")]
    public class LO2MiniCircuitsTestSteps : TestStep
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public LO2MiniCircuitsTestSteps()
        {
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.
        }
    }

    [Display("5. Output Port", Groups: new[] { "PNA-X", "Converters", "Swept IMD Converters", "Steps" }, Description: "Insert a description here")]
    public class OutputPortMiniCircuitsTestSteps : TestStep
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public OutputPortMiniCircuitsTestSteps()
        {
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.
        }
    }

    [Display("6. Embedded LO", Groups: new[] { "PNA-X", "Converters", "Swept IMD Converters", "Steps" }, Description: "Insert a description here")]
    public class EmbeddedLOMiniCircuitsTestSteps : TestStep
    {
        #region Settings
        // ToDo: Add property here for each parameter the end user should be able to change
        #endregion

        public EmbeddedLOMiniCircuitsTestSteps()
        {
        }

        public override void Run()
        {
            RunChildSteps(); //If the step supports child steps.
        }
    }

    

}
