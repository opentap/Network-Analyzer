using Keysight.OpenTap.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OpenTap;
using System.IO;
using Microsoft.Win32;

namespace TestPlanGenerator
{
    /// <summary>
    /// Interaction logic for TestGen.xaml
    /// </summary>
    public partial class TestGen : Window, INotifyPropertyChanged
    {
        protected GuiContext uiContext;
        protected OpenTap.TraceSource traceSource;

        public TestGen()
        {
            string finalFileName = System.IO.Directory.GetCurrentDirectory() + "\\" + @"\Input.xlsx";

            InitializeComponent();
            DataContext = new TestGenModel();
            (DataContext as TestGenModel).TestPlanInputFileName = finalFileName;
            (DataContext as TestGenModel).TestPlanOutputFileName = @"C:\Data\Output.TapPlan";
        }

        public void SetUIContext(GuiContext context)
        {
            this.uiContext = context;
        }

        public void SetTraceSource(OpenTap.TraceSource value)
        {
            this.traceSource = value;
            (DataContext as TestGenModel).SetTraceSource(traceSource);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Return) || (e.Key == Key.Enter))
            {
                // Update binding value
                BindingExpression binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
                binding.UpdateSource();

                // Switch focus to indicate user the change was made
                //MainPanel.Focus();
            }

        }

        private void OnLostFocusHandler(object sender, RoutedEventArgs e)
        {

            // Update binding value
            BindingExpression binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();

            // Switch focus to indicate user the change was made
            //MainPanel.Focus();

        }

        private void GeneratePlanButton_Click(object sender, RoutedEventArgs e)
        {

            this.traceSource.Info("TestPlanInputFileName: " + (DataContext as TestGenModel).TestPlanInputFileName);


            TestPlan testPlan = (DataContext as TestGenModel).GenerateTestPlan();
            SetTestPlan(testPlan);

        }



        private void SetTestPlan(TestPlan testPlan)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    this.uiContext.Plan = testPlan;
                }
                catch (Exception e)
                {
                    File.WriteAllLines(@"C:\TEMP\crashLog.txt", new string[] { e.ToString() });
                }
            });
        }

        private void BrowseInputFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".csv";
            dlg.Filter = "Comma Separated|*.csv";

            if (dlg.ShowDialog() == true)
            {
                (DataContext as TestGenModel).TestPlanInputFileName = dlg.FileName;
            }

        }

        private void BrowseOutputFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ".TapPlan";
            dlg.Filter = "Test Plan|*.TapPlan";

            if (dlg.ShowDialog() == true)
            {
                (DataContext as TestGenModel).TestPlanOutputFileName = dlg.FileName;
            }
        }
    }
}
