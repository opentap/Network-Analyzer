using OpenTap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public enum MeasurementFormatEnum
        {
            [Display("Lin Mag")]
            MLINear,
            [Display("Log Mag")]
            MLOGarithmic,
            [Display("Phase")]
            PHASe,
            [Display("Unwrapped Phase")]
            UPHase,
            [Display("Imaginary")]
            IMAGinary,
            [Display("Real")]
            REAL,
            [Display("Polar")]
            POLar,
            [Display("Smith")]
            SMITh,
            [Display("Inverted Smith")]
            SADMittance,
            [Display("SWR")]
            SWR,
            [Display("Group Delay")]
            GDELay,
            [Display("Kelvin")]
            KELVin,
            [Display("Fahrenheit")]
            FAHRenheit,
            [Display("Celsius")]
            CELSius,
            [Display("Positive Phase")]
            PPHase,
            [Display("Complex")]
            COMPlex
        }

        public int AddNewTrace(int Channel, int Window, string Trace, string MeasClass, string Meas, ref int tnum, ref int mnum, ref string MeasName)
        {
            int traceid = GetNewWindowTraceID(Window);
            mnum = GetUniqueTraceId();

            // MeasName = Trace + mnum
            // i.e. for Trace = CH1_S11 
            //          mnum = 1
            //          then we get: CH1_S11_1
            // This is the format that the PNA uses
            MeasName = Trace + "_" + mnum.ToString();

            ScpiCommand($"CALCulate{Channel}:CUST:DEFine \'{MeasName}\',\'{MeasClass}\',\'{Meas}\'");

            // Create a window if it doesn't exist already
            ScpiCommand($"DISPlay:WINDow{Window}:STATe ON");

            // Display the measurement
            ScpiCommand($"DISPlay:WINDow{Window}:TRACe{traceid}:FEED \'{MeasName}\'");

            // 
            ScpiCommand($"CALCulate{Channel}:PARameter:SELect \'{MeasName}\'");

            // Get Trace number
            tnum = ScpiQuery<int>($"CALCulate{Channel}:PARameter:TNUMber?");

            return tnum;
        }

        public void SetTraceTitle(int Window, int tnum, string TraceTitle)
        {
            if (TraceTitle != "")
            {
                ScpiCommand($"DISPlay:WINDow{Window}:TRACe{tnum}:TITLe:DATA '{TraceTitle}'");
                ScpiCommand($"DISPlay:WINDow{Window}:TRACe{tnum}:TITLe ON");
            }
        }

        public bool GetTraceTitleState(int Window, int tnum)
        {
            return ScpiQuery<bool>($"DISPlay:WINDow{Window}:TRACe{tnum}:TITLe?");
        }

        public String GetTraceTitle(int Window, int tnum)
        {
            String titleData = ScpiQuery($"DISPlay:WINDow{Window}:TRACe{tnum}:TITLe:DATA?");
            titleData = titleData.Replace("\"", "");
            return titleData;
        }

        public void SetTraceFormat(int Channel, int mnum, MeasurementFormatEnum meas)
        {
            ScpiCommand($"CALCulate{Channel}:MEASure{mnum}:FORMat {meas}");
        }

        public void SelectMeasurement(int Channel, int mnum)
        {
            ScpiCommand($"CALC{Channel}:PAR:MNUM:SEL {mnum}");
        }

        public void SelectMeasurement(int Channel, String mnum)
        {
            ScpiCommand($"CALC{Channel}:PAR:MNUM:SEL {mnum}");
        }

        public int GetSelectedTraceNumber(int Channel)
        {
            return ScpiQuery<int>($"CALC{Channel}:PAR:TNUMber?");
        }

        public int GetSelectedWindowNumber(int Channel)
        {
            return ScpiQuery<int>($"CALC{Channel}:PAR:WNUMber?");
        }

        public string GetTraceTitle(int Channel, int mnum, string MeasName)
        {
            // Set Active Measurement
            SelectMeasurement(Channel, mnum);

            // Get TNUM
            int tnum = GetSelectedTraceNumber(Channel);

            // Get WNUM
            int wnum = GetSelectedWindowNumber(Channel);

            // Get Trace Title using TNUM and WNUM
            bool traceState = GetTraceTitleState(wnum, tnum);

            if (traceState)
            {
                // Replace MeasName with TraceTitle
                String traceTitle = GetTraceTitle(wnum, tnum);
                MeasName = traceTitle;
            }
            else
            {
                // Title has not been set, 
                // continue using MeasName
            }

            return MeasName;
        }
    }
}
