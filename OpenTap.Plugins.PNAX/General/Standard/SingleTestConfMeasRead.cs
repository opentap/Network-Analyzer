using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using OpenTap;

namespace OpenTap.Plugins.PNAX
{
    public class ConfigMeas
    {
        [Display("Measurement", Order: 20)]
        public StandardTraceEnum StandardTrace { get; set; }

        [Display("Start", Order: 21)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public double Start { get; set; }

        [Display("Stop", Order: 22)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000000")]
        public double Stop { get; set; }

        [Display("Power", Order: 23)]
        [Unit("dBm", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public double Power { get; set; }

        [Display("Points", Order: 24)]
        public int Points { get; set; }

        [Display("IF Bandwidth", Order: 25)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000")]
        public double IFBandwidth { get; set; }

        public ConfigMeas()
        {
            StandardTrace = StandardTraceEnum.S11;
            Start = 10e6;
            Stop = 10e9;
            Power = -10;
            Points = 201;
            IFBandwidth = 10e3;
        }
    }

    public class MeasureReturn
    {
        [Output]
        [Display("Frequency", Order: 100)]
        [Unit("Hz", UseEngineeringPrefix: true, StringFormat: "0.000000")]
        public List<double> Freq { get; set; } = new List<double>();

        [Output]
        [Display("Trace", Order: 101)]
        [Unit("dB", UseEngineeringPrefix: true, StringFormat: "0.00")]
        public List<double> Trace { get; set; } = new List<double>();

        public MeasureReturn()
        {
            Freq = new List<double>();
            Trace = new List<double>();
        }
    }

    [Display(
        "Config-Meas",
        Groups: new[] { "Network Analyzer", "General", "Standard" },
        Description: "This test step is an example on how to setup the instrument, trigger it and get the results in a single test step"
    )]
    public class SingleTestConfMeasRead : TestStep
    {
        #region Settings
        public virtual PNAX PNAX { get; set; }

        [Display("Channel", Order: 9)]
        public int Channel { get; set; }

        [Display("", Group: "Configuration Parameters", Order: 10)]
        [EmbedProperties(PrefixPropertyName = false)]
        public ConfigMeas configMeas { get; set; } = new ConfigMeas();

        [Display("Sweep Mode", Order: 20)]
        public SweepModeEnumType sweepMode { get; set; }

        [Display("", Group: "Results", Order: 100)]
        [EmbedProperties(PrefixPropertyName = false)]
        public MeasureReturn MeasureReturn { get; set; } = new MeasureReturn();
        #endregion

        public SingleTestConfMeasRead()
        {
            Channel = 1;
            sweepMode = SweepModeEnumType.SING;
        }

        public override void Run()
        {
            PNAX.Preset();

            Initialize();
            Trigger();
            Measure();
        }

        public void Initialize()
        {
            int Channel = 1;
            int _tnum = 0;
            int _mnum = 0;
            string _MeasName = "";
            int Window = 1;
            StandardTraceEnum standardTrace = configMeas.StandardTrace;
            string standardTraceName = $"CH{Channel}_{standardTrace}";

            PNAX.AddNewTrace(
                Channel,
                Window,
                standardTraceName,
                "Standard",
                configMeas.StandardTrace.ToString(),
                ref _tnum,
                ref _mnum,
                ref _MeasName
            );
            PNAX.SetStandardSweepType(Channel, ScalerMixerSweepType.LinearFrequency);
            PNAX.SetStart(Channel, configMeas.Start);
            PNAX.SetStop(Channel, configMeas.Stop);
            PNAX.SetPower(Channel, configMeas.Power);
            PNAX.SetPoints(Channel, configMeas.Points);
            PNAX.SetIFBandwidth(Channel, configMeas.IFBandwidth);
        }

        public void Trigger()
        {
            PNAX.SetSweepMode(Channel, sweepMode);
        }

        public void Measure()
        {
            List<Double> FrequencyOutput = new List<double>();
            List<Double> TraceOutput = new List<double>();
            int Channel = 1;
            int mnum = 1;

            List<List<string>> results = PNAX.StoreTraceData(Channel, mnum);
            var xResult = results.Where((item, index) => index % 2 == 0).ToList();
            var yResult = results.Where((item, index) => index % 2 != 0).ToList();

            FrequencyOutput = xResult[0]
                .Select(double.Parse)
                .Select(z => Math.Round(z, 2))
                .ToList();
            TraceOutput = yResult[0].Select(double.Parse).Select(z => Math.Round(z, 2)).ToList();

            MeasureReturn.Freq = FrequencyOutput;
            MeasureReturn.Trace = TraceOutput;
        }
    }
}
