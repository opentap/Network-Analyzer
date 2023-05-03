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
        public int AddNewTrace(int Channel, int Window, String Trace, String MeasClass, String Meas)
        {
            int traceid = GetNewWindowTraceID(Window);

            ScpiCommand($"CALCulate{Channel.ToString()}:CUST:DEFine \'{Trace}\',\'{MeasClass}\',\'{Meas.ToString()}\'");

            // Create a window if it doesn't exist already
            ScpiCommand($"DISPlay:WINDow{Window.ToString()}:STATe ON");

            // Display the measurement
            ScpiCommand($"DISPlay:WINDow{Window.ToString()}:TRACe{traceid.ToString()}:FEED \'{Trace}\'");

            // 
            ScpiCommand($"CALCulate{Channel.ToString()}:PARameter:SELect \'{Trace}\'");

            // Get Trace number
            int tnum = ScpiQuery<int>($"CALCulate{Channel.ToString()}:PARameter:TNUMber?");

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
    }
}
