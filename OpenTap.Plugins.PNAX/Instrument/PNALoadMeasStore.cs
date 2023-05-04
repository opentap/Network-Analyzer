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
        private List<int> GetActiveChannels()
        {
            var channels = ScpiQuery(":SYST:CHAN:CAT?");

            // Clean channel string
            char[] charsToTrim = { '"', '\n' };
            var activeChannels = channels.Trim(charsToTrim).Split(',').Select(int.Parse).ToList();
            return activeChannels;
        }

        /// <summary>
        /// Checks test step paramater against list of active channels
        /// </summary>
        private List<int> ChannelListCheck(List<int> channelsList, List<int> activeChannels)
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
        private string[] GetTraceNames(int channel)
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
        public void LoadState(string filename)
        {
            ScpiCommand(":SYST:PRES");
            ScpiCommand($":MMEM:LOAD:FILE \"{filename}\"");
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

            // Trigger every channel
            foreach (var channel in channelsList)
            {
                ScpiCommand($":SENS{channel}:SWE:MODE SING");
            }

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
                        allTraceNumList.Add(traceNum);
                    }
                    else
                    {
                        traceTitles.Add(tracesList[i]);
                    }
                }

                // Return trace information
                foreach (var trace in traceNumList)
                {
                    ScpiCommand($"CALC{channel}:PAR:MNUM:SEL {trace}");
                    var xString = ScpiQuery($"CALC{channel}:X:VAL?");
                    var xList = xString.Split(',').ToList();

                    var yString = ScpiQuery($"CALC{channel}:DATA? FDATA");
                    var yList = yString.Split(',').ToList();

                    resultsList.Add(xList);
                    resultsList.Add(yList);
                }
            }

            resultsList.Insert(0, allTraceNumList);
            resultsList.Insert(1, traceTitles);
            return resultsList;
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
                    ScpiCommand($"CALC{channel}:PAR:MNUM:SEL {trace}");

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
    }
}
