//Copyright 2012-2019 Keysight Technologies
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using OpenTap;
using Keysight.OpenTap.Wpf;

// Classes that implement IMenuItem can be used to add menus to the TAP GUI.
// Only the Invoke method is required.

// When using the Display Attributes for menu choices, note the following behavior.
// The first parameter is the visible name of the sub menu. It does not need to match the class name.
// Group is the name of the top menu. An underscore in the group name indicates the shortcut key for this menu.
// Order is the left to right order of the top menus. Top menu choices are ordered low to high.
// Description is not used.
// Collapsed is not used.

// Top level custom menus are added after the last standard TAP menu.
// Sub menus are ordered alphabetically by display name.
// It is not possible to create groups under the top menus.
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.IO;


namespace TestPlanGenerator
{
    public abstract class ThirdPartyAppAbstract : IMenuItem, IInitializedMenuItem
    {

        protected Process process;
        protected GuiContext uiContext;
        protected OpenTap.TraceSource traceSource;

        protected ThirdPartyAppAbstract()
        {
            Application.Current.Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
        }

        // TAP closing
        private void Dispatcher_ShutdownStarted(object sender, EventArgs e)
        {
            if (this.process != null)
            {
                if (!this.process.HasExited)
                {
                    this.process.Kill();
                }
                this.process.Dispose();
            }
        }

        public void Initialize(GuiContext context)
        {
            this.uiContext = context;
        }

        public abstract void Invoke();


        protected string GetTheme()
        {
            if (Keysight.OpenTap.Wpf.EditorSettings.Current.ColorTheme == Keysight.OpenTap.Wpf.Themes.TapSkins.Theme.Light)
            {
                return "Light";
            }
            else
            {
                return "Dark";
            }
        }


    }




    // The underscore is an keyboard short cut.
    [Display("NA Test Generator", Group: "_NA Menu", Order: 1, Description: "Test Plan Generator for Network Analyzer.")]
    public class ToolMenuA : ThirdPartyAppAbstract
    {
        private string configAppPath = @"Packages\TestPlanGenerator\TestPlanGenerator.dll";

        public ToolMenuA() : base()
        {
            this.traceSource = Log.CreateSource("PNATool");
        }

        public override void Invoke()
        {
            this.traceSource.Info("Invocation for ToolMenu: ");
            try
            {
                TestGen mainWindow = new TestGen();
                mainWindow.SetUIContext(this.uiContext);
                mainWindow.SetTraceSource(this.traceSource);
                mainWindow.Show();
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                this.traceSource.Error($"{e.Message} - {configAppPath}");
            }
            catch (Exception e)
            {
                this.traceSource.Error(e);
            }

        }

    }



}

