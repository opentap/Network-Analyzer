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

        public int AddNewTrace(int Channel, int Window, String Trace, String MeasClass, String Meas, ref int tnum, ref int mnum)
        {
            int traceid = GetNewWindowTraceID(Window);
            mnum = GetUniqueTraceId();

            // MeasName = Trace + mnum
            // i.e. for Trace = CH1_S11 
            //          mnum = 1
            //          then we get: CH1_S11_1
            // This is the format that the PNA uses
            String MeasName = Trace + "_" + mnum.ToString();

            ScpiCommand($"CALCulate{Channel.ToString()}:CUST:DEFine \'{MeasName}\',\'{MeasClass}\',\'{Meas.ToString()}\'");

            // Create a window if it doesn't exist already
            ScpiCommand($"DISPlay:WINDow{Window.ToString()}:STATe ON");

            // Display the measurement
            ScpiCommand($"DISPlay:WINDow{Window.ToString()}:TRACe{traceid.ToString()}:FEED \'{MeasName}\'");

            // 
            ScpiCommand($"CALCulate{Channel.ToString()}:PARameter:SELect \'{MeasName}\'");

            // Get Trace number
            tnum = ScpiQuery<int>($"CALCulate{Channel.ToString()}:PARameter:TNUMber?");

            return tnum;
        }

        public void SetTraceTitle(int Window, int tnum, String TraceTitle)
        {
            if (TraceTitle != "")
            {
                ScpiCommand($"DISPlay:WINDow{Window.ToString()}:TRACe{tnum.ToString()}:TITLe:DATA '{TraceTitle}'");

                ScpiCommand($"DISPlay:WINDow{Window.ToString()}:TRACe{tnum.ToString()}:TITLe ON");
            }
        }

        public void SetTraceFormat(int Channel, int mnum, MeasurementFormatEnum meas)
        {
            ScpiCommand($"CALCulate{Channel.ToString()}:MEASure{mnum.ToString()}:FORMat {meas.ToString()}");
        }
    }
}
