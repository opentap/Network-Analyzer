using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

//Note this template assumes that you have a SCPI based instrument, and accordingly
//extends the ScpiInstrument base class.

//If you do NOT have a SCPI based instrument, you should modify this instance to extend
//the (less powerful) Instrument base class.

namespace OpenTap.Plugins.PNAX
{
    public partial class PNAX : ScpiInstrument
    {
        /// <summary>
        /// Finds all active channels
        /// </summary>
        public List<int> GetActiveChannels()
        {
            var channels = ScpiQuery(":SYST:CHAN:CAT?");

            // Clean channel string
            char[] charsToTrim = { '"', '\n' };
            var activeChannels = channels.Trim(charsToTrim).Split(',').Select(int.Parse).ToList();
            return activeChannels;
        }

        public List<int> GetChannelMeasurements(int Channel)
        {
            var measurements = ScpiQuery($"SYSTem:MEASure:CATalog? {Channel}");

            // Clean channel string
            char[] charsToTrim = { '"', '\n' };
            var activeMeasurements = measurements.Trim(charsToTrim).Split(',').Select(int.Parse).ToList();
            return activeMeasurements;
        }

        /// <summary>
        /// Checks test step paramater against list of active channels
        /// </summary>
        public List<int> ChannelListCheck(List<int> channelsList, List<int> activeChannels)
        {
            if (channelsList.Count == 0)
                channelsList = activeChannels;
            else
            {
                foreach (var channel in channelsList)
                {
                    if (activeChannels.IndexOf(channel) != -1)
                        continue;
                    else
                        throw new IndexOutOfRangeException();
                }
            }

            return channelsList;
        }

        /// <summary>
        /// Returns array of trace names
        /// </summary>
        public string[] GetTraceNames(int channel)
        {
            // Grab trace names
            char[] charsToTrim = { '"', '\n' };
            var traces = ScpiQuery($"CALC{channel}:PAR:CAT:EXT?");
            traces = traces.Replace("B,", "B_");
            var tracesList = traces.Trim(charsToTrim).Split(',');
            return tracesList;
        }
        /// <summary>
                 /// Load state file into Network Analyzer
                 /// </summary>
        public void LoadState(string filename, bool overwrite)
        {
            string FileName = filename;

            ScpiCommand(":SYST:PRES");

            // If running VNA on same computer as TAP
            if (VisaAddress.Contains("localhost"))
            {
                // use the file name since TAP and VNA are running on same computer
                FileName = filename;
            }
            else
            {
                // VNA is being accessed remotely
                FileName = Path.GetFileName(filename);
                string FilePath = Path.GetDirectoryName(filename);

                // first find if the file is already in the VNA
                var mmemCatalogStr = ScpiQuery("MMEM:CATalog?");
                List<string> mmemCat = mmemCatalogStr.Split(',').ToList();
                bool found = false;
                foreach (string mmem in mmemCat)
                {
                    if (mmem.Contains(FileName))
                    {
                        // found it
                        found = true;
                        break;
                    }
                }

                if (!found || overwrite)
                {
                    byte[] filedata = File.ReadAllBytes(filename);
                    // copy it
                    ScpiIEEEBlockCommand($"MMEM:TRAN \"{FileName}\", ", filedata);

                    // validate it was copied
                    var mmemCatalogStr2 = ScpiQuery("MMEM:CATalog?");
                    List<string> mmemCat2 = mmemCatalogStr2.Split(',').ToList();
                    bool found2 = false;
                    foreach (string mmem2 in mmemCat2)
                    {
                        if (mmem2.Contains(FileName))
                        {
                            // found it
                            found2 = true;
                            break;
                        }
                    }
                    if (!found2)
                    {
                        throw new Exception("Error while copying file to instrument!");
                    }
                }

            }
            ScpiCommand($":MMEM:LOAD:FILE \"{FileName}\"");
            TapThread.Sleep(750);
            var errorList = QueryErrors();

            if (errorList.Count > 0)
                throw new FileNotFoundException();

        }

        /// <summary>
        /// Trigger sweep on channels
        /// </summary>
        public void MeasureState(List<int> channelsList)
        {
            List<int> activeChannels = GetActiveChannels();
            channelsList = ChannelListCheck(channelsList, activeChannels);
            int deftimeout = IoTimeout;
            IoTimeout = 20000;
            // Trigger every channel
            foreach (var channel in channelsList)
            {
                ScpiCommand($":SENS{channel}:SWE:MODE SING");
            }
            IoTimeout = deftimeout;
        }

        /// <summary>
        /// Store trace info from each channel in the state
        /// </summary>
        public List<List<string>> StoreTraceData(List<int> channelsList)
        {
            ScpiCommand(":FORM:DATA ASCii, 0");

            List<int> activeChannels = GetActiveChannels();
            channelsList = ChannelListCheck(channelsList, activeChannels);

            var resultsList = new List<List<string>>();
            var allTraceNumList = new List<string>();

            var traceTitles = new List<string>();
            var FullTraceName = new List<string>();
            var TraceTitleSet = new List<string>();
            foreach (var channel in channelsList)
            {
                string[] tracesList = GetTraceNames(channel);
                //Log.Debug("TraceList: " + tracesList.ToString());
                // Create list of available traces
                var traceNumList = new List<string>();
                for (var i = 0; i < tracesList.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        var traceNum = tracesList[i].Split('_').Last();
                        traceNumList.Add(traceNum);
                        allTraceNumList.Add(traceNum);
                        FullTraceName.Add(tracesList[i]);
                    }
                    else
                    {
                        traceTitles.Add(tracesList[i]);
                    }
                }

                // Return trace information
                foreach (var trace in traceNumList)
                {
                    // trace is MNUM in help file
                    SelectMeasurement(channel, trace);
                    var xString = ScpiQuery($"CALC{channel}:X:VAL?");
                    var xList = xString.Split(',').ToList();
                    int XCount = xList.Count;

                    var yString = IsModelA ? ScpiQuery($"CALC{channel}:DATA? FDATA") : ScpiQuery($"CALC{channel}:MEAS{trace}:DATA:FDATA?");
                    var yList = yString.Split(',').ToList();

                    resultsList.Add(xList);
                    resultsList.Add(yList);

                    // now query PF
                    string strPF = IsModelA? ScpiQuery($"CALC{channel}:LIMit:FAIL?") : ScpiQuery($"CALC{channel}:MEAS{trace}:LIMit:FAIL?");
                    List<string> GlobalPF = new List<string>();
                    // Create a list of n values all equal to strPF
                    string verdict = strPF.Equals("0") ? Verdict.Pass.ToString() : Verdict.Fail.ToString();
                    for (int pfIndex = 0; pfIndex < XCount; pfIndex++)
                    {
                        GlobalPF.Add(verdict);
                    }

                    // query results
                    // TODO: Find alternative, currently LIM:REP:ALL? not supported on A models
                    var limitReportAllStr = IsModelA ? "" : ScpiQuery($"CALC{channel}:Meas{trace}:LIMit:REPort:ALL?");
                    var limitReportAll = limitReportAllStr.Split(',').ToList();
                    //List<double> XAxisValues = new List<double>();
                    //List<Verdict> PassFail = new List<Verdict>();
                    //List<double> UpperLimit = new List<double>();
                    //List<double> LowerLimit = new List<double>();


                    var x1 = limitReportAll.Where((item, index) => ((index == 0) || ((index >= 4) && (index % 4 == 0)))).ToList();
                    var x2 = limitReportAll.Where((item, index) => ((index == 1) || ((index >= 5) && (index % 4 == 1)))).ToList();
                    var x3 = limitReportAll.Where((item, index) => ((index == 2) || ((index >= 6) && (index % 4 == 2)))).ToList();
                    var x4 = limitReportAll.Where((item, index) => ((index == 3) || ((index >= 7) && (index % 4 == 3)))).ToList();

                    //for (int limitIndex = 0; limitIndex < ((limitReportAll.Count())/4); limitIndex++)
                    //{
                    //    XAxisValues.Add(limitReportAll[(4*limitIndex)]);
                    //    Verdict PF = nPF == limitReportAll[(4 * limitIndex) + 1] ? Verdict.Fail : Verdict.Pass;
                    //    PassFail.Add(PF);
                    //    UpperLimit.Add(limitReportAll[(4 * limitIndex) + 2]);
                    //    LowerLimit.Add(limitReportAll[(4 * limitIndex) + 3]);
                    //}

                    // Append columns
                    resultsList.Add(GlobalPF);
                    resultsList.Add(x1);
                    resultsList.Add(x2);
                    resultsList.Add(x3);
                    resultsList.Add(x4);
                }
            }

            resultsList.Insert(0, allTraceNumList);
            resultsList.Insert(1, traceTitles);
            resultsList.Insert(2, FullTraceName);
            return resultsList;
        }

        public List<List<string>> StoreTraceData(int Channel, int mnum)
        {
            ScpiCommand(":FORM:DATA ASCii, 0");

            var resultsList = new List<List<string>>();

            // Return trace information
            // trace is MNUM in help file
            SelectMeasurement(Channel, mnum);
            var xString = ScpiQuery($"CALC{Channel}:X:VAL?");
            var xList = xString.Split(',').ToList();

            var yString = IsModelA ? ScpiQuery($"CALC{Channel}:DATA? FDATA") : ScpiQuery($"CALC{Channel}:MEAS{mnum}:DATA:FDATA?");
            var yList = yString.Split(',').ToList();

            resultsList.Add(xList);
            resultsList.Add(yList);

            return resultsList;
        }

        public string GetTraceName(int Channel, int mnum)
        {
            string retVal = "undefined trace";

            string[] tracesList = GetTraceNames(Channel);
            for (var i = 0; i < tracesList.Length; i++)
            {
                if (i % 2 == 0)
                {
                    var traceinfo = tracesList[i].Split('_');
                    if (int.Parse(traceinfo[(traceinfo.Length-1)]) == mnum) // custom traces such as CH1_PowerSupply_VM1_2 have more than 3 elements once they are split by '_', using the last item as the mnum
                    {
                        retVal = traceinfo[1];
                        return retVal;
                    }
                }
                //else
                //{
                //    traceTitles.Add(tracesList[i]);
                //}
            }


            return retVal;
        }

        /// <summary>
        /// Return min and max value from all traces in the state
        /// </summary>
        public List<string> GetMinMax(List<int> channelsList)
        {
            List<int> activeChannels = GetActiveChannels();
            channelsList = ChannelListCheck(channelsList, activeChannels);

            var minMaxList = new List<string>();
            foreach (var channel in channelsList)
            {
                string[] tracesList = GetTraceNames(channel);

                // Create list of available traces
                var traceNumList = new List<string>();
                for (var i = 0; i < tracesList.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        var traceNum = tracesList[i].Split('_').Last();
                        traceNumList.Add(traceNum);
                    }
                }

                foreach (var trace in traceNumList)
                {
                    // Select trace
                    SelectMeasurement(channel, trace);

                    // Turn marker on
                    ScpiCommand($"CALC{channel}:MARK1:STAT 1");

                    // Move marker to Max
                    ScpiCommand($":CALC{channel}:MARK:FUNC:EXEC MAX");
                    var max = ScpiQuery($":CALC{channel}:MARK:Y?").Split(',')[0].Trim();
                    Log.Info($"Trace {trace} Max Value:" + max);
                    minMaxList.Add(max);

                    // Move marker to Min
                    ScpiCommand($":CALC{channel}:MARK:FUNC:EXEC MIN");
                    var min = ScpiQuery($":CALC{channel}:MARK:Y?").Split(',')[0].Trim();
                    Log.Info($"Trace {trace} Min Value:" + min);
                    minMaxList.Add(min);

                }
            }

            return minMaxList;
        }

        public void SaveState(string filename)
        {
            ScpiCommand($":MMEM:STOR:CSAR \"{filename}\"");

            TapThread.Sleep(1000);
            var errorList = QueryErrors();

            if (errorList.Count > 0)
                throw new FileNotFoundException();
        }

        public DateTime CalibrationCheck()
        {
            List<int> activeChannels = GetActiveChannels();

            var dummy = new DateTime(1000, 1, 1);
            var oldest = dummy;

            foreach (var channel in activeChannels)
            {
                var calset = ScpiQuery($":SENS{channel}:CORR:CSET:ACT? NAME");
                var dateStr = ScpiQuery($":CSET:DATE? {calset}");

                var dateInts = dateStr.Split(',');
                if (dateInts.Count() != 3)
                    break;

                int year = int.Parse(dateInts[0]);
                int month = int.Parse(dateInts[1]);
                int day = int.Parse(dateInts[2]);
                var calDate = new DateTime(year, month, day);

                if (calDate < oldest || oldest == dummy)
                    oldest = calDate;
            }
            return oldest;
        }

        /// <summary>
        /// Returns measurement units for each parameter
        /// </summary>
        public string GetUnits(string param)
        {
            switch (param)
            {
                case "S21":
                case "S12":
                case "CompOut21":
                case "NF":
                case "IM3Lo":
                case "IM3Hi":
                    return "dB";
                case "S11":
                case "S22":
                    return "VSWR";
                case "Output":
                case "PwrMain":
                    return "dBm";
                default:
                    return null;
            }
        }

        public void SaveScreen(string FullFileName)
        {
            string FileName = Path.GetFileName(FullFileName);
            string FilePath = Path.GetDirectoryName(FullFileName);
            string InstrumentFileName = "";
            string InstrumentFolderName = "";

            InstrumentFileName = @"C:\Users\Public\Documents\Network Analyzer\" + FileName;
            InstrumentFolderName = @"C:\Users\Public\Documents\Network Analyzer\";

            ChangeFolder(InstrumentFolderName);

            // Save screenshot to local folder on instrument: <documents>\<mode>\screen
            ScpiCommand(":MMEM:STOR:SSCR '" + InstrumentFileName + "'");

            // Make sure folder exists on local PC
            bool exists = System.IO.Directory.Exists(FilePath);
            if (!exists)
                System.IO.Directory.CreateDirectory(FilePath);

            // Copy from instrument to local PC
            byte[] filedata = ScpiQueryBlock(string.Format(":MMEM:TRAN? \"{0}\"", InstrumentFileName));

            // Write file at local PC
            File.WriteAllBytes(FullFileName, filedata);

            // Delete File from instrument
            ScpiCommand(string.Format("MMEM:DEL \"{0}\"", InstrumentFileName));

            QueryErrors();
        }

        public void SaveSnP(int Channel, int mnum, List<int> ports, string FullFileName)
        {
            string strPorts = string.Join(",", ports);
            string FileName = Path.GetFileName(FullFileName);
            string FilePath = Path.GetDirectoryName(FullFileName);
            string InstrumentFileName = "";
            string InstrumentFolderName = "";

            InstrumentFileName = @"C:\Users\Public\Documents\Network Analyzer\" + FileName;
            InstrumentFolderName = @"C:\Users\Public\Documents\Network Analyzer\";

            ChangeFolder(InstrumentFolderName);

            // Find Class Name
            String measName = GetChannelType(Channel);

            // if Standard
            if (measName.Equals("Standard"))
            {
                if (IsModelA)
                {
                    ScpiCommand($"CALCulate{Channel}:DATA:SNP:PORTs:SAVE '{strPorts}','" + InstrumentFileName + "'");
                }
                else
                {
                    ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:DATA:SNP:PORTs:SAVE '{strPorts}','" + InstrumentFileName + "'");
                }
            }
            else if (StoreSnpBlockList.Any(s => s.Equals(measName)))
            {
                // if meas name is in BlockList
                Log.Info($"Can't store SNP for Channel '{Channel}' with Measurement Class '{measName}'");
                return;
            }
            else
            {
                // use MMEM:STOR
                ScpiCommand($"MMEM:STORe '{InstrumentFileName}'");
            }


            // Make sure folder exists on local PC
            bool exists = System.IO.Directory.Exists(FilePath);
            if (!exists)
                System.IO.Directory.CreateDirectory(FilePath);

            // Copy from instrument to local PC
            byte[] filedata = ScpiQueryBlock(string.Format("MMEM:TRAN? \"{0}\"", InstrumentFileName));

            // Write file at local PC
            File.WriteAllBytes(FullFileName, filedata);

            // Delete File from instrument
            ScpiCommand(string.Format("MMEM:DEL \"{0}\"", InstrumentFileName));

            QueryErrors();
        }

        public void SaveDispState(string FullFileName)
        {
            string FileName = Path.GetFileName(FullFileName);
            string FilePath = Path.GetDirectoryName(FullFileName);
            string InstrumentFileName = "";
            string InstrumentFolderName = "";

            InstrumentFileName = @"C:\Users\Public\Documents\Network Analyzer\" + FileName;
            InstrumentFolderName = @"C:\Users\Public\Documents\Network Analyzer\";

            ChangeFolder(InstrumentFolderName);

            // Save csv data to local folder on instrument: <documents>\<mode>\screen
            ScpiCommand($":MMEM:STOR:DATA '{InstrumentFileName}','CSV Formatted Data','Displayed','Displayed',-1");

            // Make sure folder exists on local PC
            bool exists = System.IO.Directory.Exists(FilePath);
            if (!exists)
                System.IO.Directory.CreateDirectory(FilePath);

            // Copy from instrument to local PC
            byte[] filedata = ScpiQueryBlock(string.Format(":MMEM:TRAN? \"{0}\"", InstrumentFileName));

            // Write file at local PC
            File.WriteAllBytes(FullFileName, filedata);

            // Delete File from instrument
            ScpiCommand(string.Format("MMEM:DEL \"{0}\"", InstrumentFileName));

            QueryErrors();
        }


        private void ChangeFolder(string folder)
        {
            // Clear System Errors
            ScpiCommand("*CLS");

            try
            {
                // Lets Make sure folder exists
                // Change directory to D:\NVARB
                ScpiCommand(":MMEM:CDIR \"" + folder + "\"");

                // Lets see if current directory is D:\NVARB
                string strCDIR = ScpiQuery(":MMEM:CDIR?");

                // Lets see if there is an error
                List<ScpiInstrument.ScpiError> ListErrors = QueryErrors();

                // If we get errors, most likely the directory does not exist
                if (ListErrors.Count > 0)
                {
                    // Clear System Errors
                    ScpiCommand("*CLS");

                    // Make folder
                    ScpiCommand(":MMEM:MDIR \"" + folder + "\"");
                    WaitForOperationComplete();
                }

                // Change directory to D:\NVARB
                ScpiCommand(":MMEM:CDIR \"" + folder + "\"");

                // Verify we can change to the directory
                strCDIR = ScpiQuery(":MMEM:CDIR?");

                // Validate the current directory is now D:\NVARB
                if (!strCDIR.Contains(folder))
                {
                    // Lets see if there is an error
                    ListErrors.Clear();
                    ListErrors = QueryErrors();

                    // Throw excpetion if can't change to folder
                    if (ListErrors.Count > 0)
                    {
                        throw new Exception("Error while creating folder " + folder + " " + ListErrors[0].Message);
                    }
                    else
                    {
                        throw new Exception("Error while creating folder " + folder);
                    }
                }

                // We now have switched to the folder
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
