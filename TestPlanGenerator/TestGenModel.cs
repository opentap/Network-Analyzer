using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using OpenTap;
using OpenTap.Plugins.BasicSteps;
using OpenTap.Plugins.PNAX;
using static OpenTap.Plugins.PNAX.PNAX;

namespace TestPlanGenerator
{
    public class MeasurementChannel
    {
        public int Channel { get; set; }
        public String MClass { get; set; }
    }

    public class StandardSweep : MeasurementChannel
    {
        public int Points { get; set; }
        public double IFBW { get; set; }
    }

    public class StandardLinearSweep : StandardSweep
    {
        public double StartFreq { get; set; }
        public double StopFreq { get; set; }
        public double Power { get; set; }
    }

    public class StandardPowerSweep : StandardSweep
    {
        public double StartPower { get; set; }
        public double StopPower { get; set; }
        public double CW { get; set; }
    }

    public class StandardCWSweep : StandardSweep
    {
        public double Power { get; set; }
        public double CW { get; set; }
    }

    public class ExcelDialogStep
    {
        public String StepType { get; set; }
        public String MessageID { get; set; }
        public String Message { get; set; }
        public InputButtons DialogButtons { get; set; }
        public bool ShowPicture { get; set; }
        public String URL { get; set; }
        public Verdict PositiveVerdict { get; set; }
        public Verdict NegativeVerdict { get; set; }
    }

    public class StandardPhaseSweep : StandardSweep
    {
        public double StartPhase { get; set; }
        public double StopPhase { get; set; }
        public double CW { get; set; }
    }

    class TestGenModel : INotifyPropertyChanged
    {
        List<object> ListOfchannels = new List<object>();
        List<object> ListOfTraces = new List<object>();
        protected OpenTap.TraceSource traceSource;

        public void SetTraceSource(TraceSource value)
        {
            traceSource = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private String _TestPlanInputFileName;
        public String TestPlanInputFileName
        {
            get { return _TestPlanInputFileName; }
            set
            {
                _TestPlanInputFileName = value;
                RaisePropertyChanged("TestPlanInputFileName");
            }
        }

        private String _TestPlanDialogInputFileName;
        public String TestPlanDialogInputFileName
        {
            get { return _TestPlanDialogInputFileName; }
            set
            {
                _TestPlanDialogInputFileName = value;
                RaisePropertyChanged("TestPlanInputFileName");
            }
        }

        private String _TestPlanOutputFileName;
        public String TestPlanOutputFileName
        {
            get { return _TestPlanOutputFileName; }
            set
            {
                _TestPlanOutputFileName = value;
                RaisePropertyChanged("TestPlanOutputFileName");
            }
        }

        public TestPlan GenerateTestPlan()
        {
            this.traceSource.Info("Starting Test Sequence Generation");

            this.traceSource.Info("TestPlanInputFileName: " + TestPlanInputFileName.ToString());
            this.traceSource.Info("TestPlanOutputFileName: " + TestPlanOutputFileName.ToString());

            TestPlan testPlan = new TestPlan();
            testPlan.ChildTestSteps.Clear();
            ListOfchannels = new List<object>();
            ListOfTraces = new List<object>();
            List<StandardTrace> standardTraces = new List<StandardTrace>();

            // Open and Parse CSV file
            ParseExcelFile(TestPlanInputFileName);

            // foreach Channel in csv file
            // add channel to childsteps
            foreach (object o in ListOfchannels)
            {
                // Create a list of all Traces associated to this Channel number
                // make sure the channel id and channel type are the same on both lists before adding to the trace list
                MeasurementChannel MeasChannel = o as MeasurementChannel;

                // reset list of traces
                standardTraces = new List<StandardTrace>();
                foreach (object tr in ListOfTraces)
                {
                    if (tr is StandardTrace)
                    {
                        StandardTrace str = tr as StandardTrace;
                        if (str.Channel == MeasChannel.Channel)
                        {
                            standardTraces.Add(str);
                        }
                    }
                }

                if (MeasChannel.MClass.Equals("Standard"))
                {
                    if (o is StandardLinearSweep)
                    {
                        StandardLinearSweep sls = o as StandardLinearSweep;

                        // now generate the standard channel measurement class
                        StandardChannel standard = new StandardChannel(
                            sls.Channel,
                            StandardSweepTypeEnum.LinearFrequency,
                            sls.StartFreq,
                            sls.StopFreq,
                            sls.Power,
                            double.NaN,
                            sls.Points,
                            sls.IFBW,
                            standardTraces
                        );
                        testPlan.ChildTestSteps.Add(standard);
                    }
                    else if (o is StandardPowerSweep)
                    {
                        StandardPowerSweep sls = o as StandardPowerSweep;

                        // now generate the standard channel measurement class
                        StandardChannel standard = new StandardChannel(
                            sls.Channel,
                            StandardSweepTypeEnum.PowerSweep,
                            sls.StartPower,
                            sls.StopPower,
                            double.NaN,
                            sls.CW,
                            sls.Points,
                            sls.IFBW,
                            standardTraces
                        );
                        testPlan.ChildTestSteps.Add(standard);
                    }
                    else if (o is StandardCWSweep)
                    {
                        StandardCWSweep sls = o as StandardCWSweep;

                        // now generate the standard channel measurement class
                        StandardChannel standard = new StandardChannel(
                            sls.Channel,
                            StandardSweepTypeEnum.CWTime,
                            double.NaN,
                            double.NaN,
                            sls.Power,
                            sls.CW,
                            sls.Points,
                            sls.IFBW,
                            standardTraces
                        );
                        testPlan.ChildTestSteps.Add(standard);
                    }
                    else if (o is StandardPhaseSweep)
                    {
                        StandardPhaseSweep sls = o as StandardPhaseSweep;

                        // now generate the standard channel measurement class
                        StandardChannel standard = new StandardChannel(
                            sls.Channel,
                            StandardSweepTypeEnum.PhaseSweep,
                            sls.StartPhase,
                            sls.StopPhase,
                            double.NaN,
                            sls.CW,
                            sls.Points,
                            sls.IFBW,
                            standardTraces
                        );
                        testPlan.ChildTestSteps.Add(standard);
                    }
                }
            }

            // save output file

            return testPlan;
        }

        public void ParseExcelFile(String filename)
        {
            this.traceSource.Info("Importing file: " + filename);
            Workbook wb = null;
            Application app = null;

            app = new Application();
            wb = app.Workbooks.Open(filename);

            // Parse Channels
            int sheetIndex = 1;
            Worksheet sheet = (Worksheet)wb.Sheets.Item[sheetIndex];
            Range excelRange = sheet.UsedRange;
            foreach (Range row in excelRange.Rows)
            {
                int rowNumber = row.Row;

                if (rowNumber == 1)
                {
                    continue;
                }

                int col = excelRange.Columns.Count;
                string columnLetter = GetExcelColumnName(col);
                string[] values = GetRange(
                    "A" + rowNumber + ":" + columnLetter + rowNumber + "",
                    sheet
                );

                if (values[1].Equals("Standard"))
                {
                    if (values[2].Equals("LinearFrequency") || values[2].Equals("LogFrequency"))
                    {
                        StandardLinearSweep linearSweep = new StandardLinearSweep();
                        linearSweep.Channel = int.Parse(values[0]);
                        linearSweep.MClass = "Standard";
                        linearSweep.StartFreq = double.Parse(values[3]);
                        linearSweep.StopFreq = double.Parse(values[4]);
                        linearSweep.Power = double.Parse(values[5]);
                        linearSweep.Points = int.Parse(values[6]);
                        linearSweep.IFBW = double.Parse(values[7]);
                        ListOfchannels.Add(linearSweep);
                    }
                    if (values[2].Equals("PowerSweep"))
                    {
                        StandardPowerSweep powerSweep = new StandardPowerSweep();
                        powerSweep.Channel = int.Parse(values[0]);
                        powerSweep.MClass = "Standard";
                        powerSweep.StartPower = double.Parse(values[8]);
                        powerSweep.StopPower = double.Parse(values[9]);
                        powerSweep.CW = double.Parse(values[10]);
                        powerSweep.Points = int.Parse(values[6]);
                        powerSweep.IFBW = double.Parse(values[7]);
                        ListOfchannels.Add(powerSweep);
                    }
                    if (values[2].Equals("CWTime"))
                    {
                        StandardCWSweep csSweep = new StandardCWSweep();
                        csSweep.Channel = int.Parse(values[0]);
                        csSweep.MClass = "Standard";
                        csSweep.CW = double.Parse(values[10]);
                        csSweep.Points = int.Parse(values[6]);
                        csSweep.IFBW = double.Parse(values[7]);
                        ListOfchannels.Add(csSweep);
                    }
                    if (values[2].Equals("PhaseSweep"))
                    {
                        StandardPhaseSweep phaseSweep = new StandardPhaseSweep();
                        phaseSweep.Channel = int.Parse(values[0]);
                        phaseSweep.MClass = "Standard";
                        phaseSweep.StartPhase = double.Parse(values[11]);
                        phaseSweep.StopPhase = double.Parse(values[12]);
                        phaseSweep.CW = double.Parse(values[10]);
                        phaseSweep.Points = int.Parse(values[6]);
                        phaseSweep.IFBW = double.Parse(values[7]);
                        ListOfchannels.Add(phaseSweep);
                    }
                }
            }

            // Parse Traces
            sheetIndex = 2;
            sheet = (Worksheet)wb.Sheets.Item[sheetIndex];
            excelRange = sheet.UsedRange;
            foreach (Range row in excelRange.Rows)
            {
                int rowNumber = row.Row;

                if (rowNumber == 1)
                {
                    continue;
                }

                int col = excelRange.Columns.Count;
                string columnLetter = GetExcelColumnName(col);
                string[] values = GetRange(
                    "A" + rowNumber + ":" + columnLetter + rowNumber + "",
                    sheet
                );

                if (values[1].Equals("Standard"))
                {
                    StandardTrace trace = new StandardTrace();
                    trace.Channel = int.Parse(values[0]);
                    StandardTraceEnum standardtrace;
                    Enum.TryParse(values[2], out standardtrace);
                    trace.Meas = standardtrace;
                    trace.Window = int.Parse(values[3]);
                    trace.Sheet = int.Parse(values[4]);
                    MeasurementFormatEnum measurementFormat;
                    Enum.TryParse(values[5], out measurementFormat);
                    trace.MeasurementFormat = measurementFormat;
                    ListOfTraces.Add(trace);
                    trace.Title = values[6];
                }
            }

            wb.Close();
        }

        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        public string[] GetRange(string range, Worksheet excelWorksheet)
        {
            Microsoft.Office.Interop.Excel.Range workingRangeCells = excelWorksheet.get_Range(
                range,
                Type.Missing
            );
            //workingRangeCells.Select();

            System.Array array = (System.Array)workingRangeCells.Cells.Value2;
            string[] arrayS = this.ConvertToStringArray(array);

            return arrayS;
        }

        string[] ConvertToStringArray(System.Array values)
        {
            // create a new string array
            string[] theArray = new string[values.Length];

            // loop through the 2-D System.Array and populate the 1-D String Array
            for (int i = 1; i <= values.Length; i++)
            {
                if (values.GetValue(1, i) == null)
                    theArray[i - 1] = "";
                else
                    theArray[i - 1] = (string)values.GetValue(1, i).ToString();
            }

            return theArray;
        }

        public TestPlan GenerateDialogTestPlanCSV()
        {
            TestPlan testPlan = new TestPlan();
            testPlan.ChildTestSteps.Clear();

            this.traceSource.Info("Starting Test Sequence Generation from CSV");

            this.traceSource.Info(
                "TestPlanInputFileName: " + TestPlanDialogInputFileName.ToString()
            );

            return testPlan;
        }

        public TestPlan GenerateDialogTestPlan()
        {
            this.traceSource.Info("Starting Test Sequence Generation");

            this.traceSource.Info(
                "TestPlanInputFileName: " + TestPlanDialogInputFileName.ToString()
            );
            this.traceSource.Info("TestPlanOutputFileName: " + TestPlanOutputFileName.ToString());

            TestPlan testPlan = new TestPlan();
            testPlan.ChildTestSteps.Clear();
            ListOfchannels = new List<object>();
            ListOfTraces = new List<object>();
            List<StandardTrace> standardTraces = new List<StandardTrace>();

            if (TestPlanDialogInputFileName.EndsWith("xlsx"))
            {
                ParseDialogExcelFile(TestPlanDialogInputFileName);
            }
            else if (TestPlanDialogInputFileName.EndsWith("csv"))
            {
                // Open and Parse CSV file
                ParseDialogCSV(TestPlanDialogInputFileName);
            }
            else
            {
                throw new Exception("Unsupported input file!");
            }

            // foreach Channel in csv file
            // add channel to childsteps
            foreach (object o in ListOfchannels)
            {
                ExcelDialogStep excelDialogStep = o as ExcelDialogStep;
                if (excelDialogStep.StepType.Equals("Dialog"))
                {
                    OpenTap.Plugins.BasicSteps.DialogStep dialogStep = new DialogStep()
                    {
                        Title = excelDialogStep.MessageID,
                        Message = excelDialogStep.Message,
                        Buttons = excelDialogStep.DialogButtons,
                        ShowPicture = excelDialogStep.ShowPicture,
                        PictureSource = excelDialogStep.URL,
                        Name = excelDialogStep.MessageID,
                        PositiveAnswer = excelDialogStep.PositiveVerdict,
                        NegativeAnswer = excelDialogStep.NegativeVerdict,
                    };
                    testPlan.ChildTestSteps.Add(dialogStep);
                }
            }

            return testPlan;
        }

        public void ParseDialogCSV(String filename)
        {
            int NumberOfHeaders = 0;
            using (var reader = new StreamReader(TestPlanDialogInputFileName))
            {
                string line;
                int rowNumber = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    String[] values = line.Split(',');

                    rowNumber++;
                    if (rowNumber == 1)
                    {
                        // headers
                        NumberOfHeaders = values.Count();
                        continue;
                    }

                    if (values[0].Equals("Dialog"))
                    {
                        if (values.Count() != NumberOfHeaders)
                        {
                            throw new Exception(
                                $"Row {rowNumber}: {line} \nhas an incorrect character, check the message does not contain any commas"
                            );
                        }

                        ExcelDialogStep dialogStep = new ExcelDialogStep();
                        dialogStep.StepType = values[0];
                        dialogStep.MessageID = values[1];
                        dialogStep.Message = values[2];
                        if (values[3].Equals("OK/Cancel"))
                        {
                            dialogStep.DialogButtons = InputButtons.OkCancel;
                        }
                        else
                        {
                            dialogStep.DialogButtons = InputButtons.YesNo;
                        }
                        dialogStep.ShowPicture = bool.Parse(values[4]);
                        dialogStep.URL = values[5];
                        if (values[6].Equals("Pass"))
                        {
                            dialogStep.PositiveVerdict = Verdict.Pass;
                        }
                        else if (values[6].Equals("Fail"))
                        {
                            dialogStep.PositiveVerdict = Verdict.Fail;
                        }
                        else
                        {
                            dialogStep.PositiveVerdict = Verdict.NotSet;
                        }

                        if (values[7].Equals("Pass"))
                        {
                            dialogStep.NegativeVerdict = Verdict.Pass;
                        }
                        else if (values[7].Equals("Fail"))
                        {
                            dialogStep.NegativeVerdict = Verdict.Fail;
                        }
                        else
                        {
                            dialogStep.NegativeVerdict = Verdict.NotSet;
                        }
                        ListOfchannels.Add(dialogStep);
                    }
                }
            }
        }

        public void ParseDialogExcelFile(String filename)
        {
            this.traceSource.Info("Importing file: " + filename);
            Workbook wb = null;
            Application app = null;

            app = new Application();
            wb = app.Workbooks.Open(filename);

            // Parse Channels
            int sheetIndex = 1;
            Worksheet sheet = (Worksheet)wb.Sheets.Item[sheetIndex];
            Range excelRange = sheet.UsedRange;
            foreach (Range row in excelRange.Rows)
            {
                int rowNumber = row.Row;

                if (rowNumber == 1)
                {
                    continue;
                }

                int col = excelRange.Columns.Count;
                string columnLetter = GetExcelColumnName(col);
                string[] values = GetRange(
                    "A" + rowNumber + ":" + columnLetter + rowNumber + "",
                    sheet
                );

                if (values[0].Equals("Dialog"))
                {
                    ExcelDialogStep dialogStep = new ExcelDialogStep();
                    dialogStep.StepType = values[0];
                    dialogStep.MessageID = values[1];
                    dialogStep.Message = values[2];
                    if (values[3].Equals("OK/Cancel"))
                    {
                        dialogStep.DialogButtons = InputButtons.OkCancel;
                    }
                    else
                    {
                        dialogStep.DialogButtons = InputButtons.YesNo;
                    }
                    dialogStep.ShowPicture = bool.Parse(values[4]);
                    dialogStep.URL = values[5];
                    if (values[6].Equals("Pass"))
                    {
                        dialogStep.PositiveVerdict = Verdict.Pass;
                    }
                    else if (values[6].Equals("Fail"))
                    {
                        dialogStep.PositiveVerdict = Verdict.Fail;
                    }
                    else
                    {
                        dialogStep.PositiveVerdict = Verdict.NotSet;
                    }

                    if (values[7].Equals("Pass"))
                    {
                        dialogStep.NegativeVerdict = Verdict.Pass;
                    }
                    else if (values[7].Equals("Fail"))
                    {
                        dialogStep.NegativeVerdict = Verdict.Fail;
                    }
                    else
                    {
                        dialogStep.NegativeVerdict = Verdict.NotSet;
                    }
                    ListOfchannels.Add(dialogStep);
                }
            }
            wb.Close();
        }
    }
}
