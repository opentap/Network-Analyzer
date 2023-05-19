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
    public enum CalModeEnum
    {
        SYNC,
        ASYN
    }

    public partial class PNAX : ScpiInstrument
    {
        public List<String> GetAllMeasClasses()
        {
            List<String> AllMeasClasses = new List<string>();
            List<int> Channels = GetActiveChannels();

            foreach(int ch in Channels)
            {
                ScpiCommand($"SYST:ACT:CHAN {ch}");
                WaitForOperationComplete();
                String mclass = ScpiQuery("SYSTem:ACTive:MCLass?");
                AllMeasClasses.Add(mclass);
            }
            return AllMeasClasses;
        }

        public void CalAllReset()
        {
            ScpiCommand("SYST:CAL:ALL:RESet");
        }

        public List<int> CalAllSelectChannels()
        {
            // Get active channels using GetActiveChannels()
            List<int> Channels = GetActiveChannels();
            CalAllSelectChannels(Channels);
            return Channels;
        }

        public void CalAllSelectChannels(List<int> Channels)
        {
            String AllChannelsString = String.Join(",", Channels.ToArray());
            CalAllSelectChannels(AllChannelsString);
        }

        public void CalAllSelectChannels(String Channels)
        {
            ScpiCommand($"SYSTem:CALibrate:ALL:SELect {Channels}");
        }

        public void CalAllSelectPorts(List<int> Ports)
        {
            List<int> Channels = GetActiveChannels();
            foreach(int ch in Channels)
            {
                String AllPortsString = String.Join(",", Ports.ToArray());
                CalAllSelectPorts(ch, AllPortsString);
            }
        }

        public void CalAllSelectPorts(List<int> Channels, List<int> Ports)
        {
            foreach (int ch in Channels)
            {
                String AllPortsString = String.Join(",", Ports.ToArray());
                CalAllSelectPorts(ch, AllPortsString);
            }
        }

        public void CalAllSelectPorts(int Channel, List<int> Ports)
        {
            String AllPortsString = String.Join(",", Ports.ToArray());
            CalAllSelectPorts(Channel, AllPortsString);
        }

        public void CalAllSelectPorts(int Channel, String Ports)
        {
            ScpiCommand($"SYSTem:CALibrate:ALL:CHANnel{Channel}:PORTs:SELect {Ports}");
        }

        public void CalAllSetProperty(String prop, String value)
        {
            ScpiCommand($"SYSTem:CALibrate:ALL:MCLass:PROPerty:VALue \"{prop}\",\"{value}\"");
        }

        public void CalAllSelectDutConnectorType(int Channel, int Port, String ConnectorType)
        {
            ScpiCommand($"SENSe{Channel}:CORRection:COLLect:GUIDed:CONNector:PORT{Port}:SELect \"{ConnectorType}\"");
        }

        public void CalAllSelectCalKit(int Channel, int Port, String CalKit)
        {
            ScpiCommand($"SENSe{Channel}:CORRection:COLLect:GUIDed:CKIT:PORT{Port}:SELect \"{CalKit}\"");
        }

        public int CalAllGuidedChannelNumber()
        {
            return ScpiQuery<int>($"SYSTem:CALibrate:ALL:GUIDed:CHANnel:VALue?");
        }

        public void CalAllInit(int CalChannel, String CalSetName = "", bool MatchCalSettings = false, CalModeEnum calMode = CalModeEnum.ASYN)
        {
            int MatchCalSettingsInt = 0;
            if (MatchCalSettings)
            {
                MatchCalSettingsInt = 1;
            }
            ScpiCommand($"SENSe{CalChannel}:CORRection:COLLect:GUIDed:INITiate:IMMediate \"{CalSetName}\", {MatchCalSettingsInt}, {calMode}");
            //ScpiCommand($"SENSe{CalChannel}:CORRection:COLLect:GUIDed:INITiate:IMMediate");
        }

        public int CalAllNumberOfSteps(int CalChannel)
        {
            return ScpiQuery<int>($"SENSe{CalChannel}:CORRection:COLLect:GUIDed:STEPs?");
        }

        public String CalAllStepDescription(int CalChannel, int CalStep)
        {
            return ScpiQuery($"SENSe{CalChannel}:CORRection:COLLect:GUIDed:DESCription? {CalStep}");
        }

        public void CalAllStep(int CalChannel, int CalStep, CalModeEnum calMode = CalModeEnum.ASYN)
        {
            ScpiCommand($"SENSe{CalChannel}:CORRection:COLLect:GUIDed:ACQuire STAN{CalStep},{calMode}");
        }

        public void CalAllSave(int CalChannel)
        {
            ScpiCommand($"SENSe{CalChannel}:CORRection:COLLect:GUIDed:SAVE:IMMediate");
        }

        public int SimulatorMode()
        {
            int mode = ScpiQuery<int>("SYST:ACT:SIMulator?");
            return mode;
        }

    }
}
