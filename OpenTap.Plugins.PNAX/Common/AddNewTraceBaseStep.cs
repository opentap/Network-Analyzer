using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTap.Plugins.PNAX
{
    [Browsable(false)]
    public class AddNewTraceBaseStep : SingleTraceBaseStep
    {
        #region Settings

        private ConverterStagesEnum _ConverterStagesEnum;
        [EnabledIf("IsConverter", true, HideIfDisabled = false)]
        [Display("Converter Stages", Order: 10)]
        public override ConverterStagesEnum ConverterStages
        {
            get
            {
                return _ConverterStagesEnum;
            }
            set
            {
                _ConverterStagesEnum = value;
                foreach (var step in this.ChildTestSteps)
                {
                    if (step.GetType().IsSubclassOf(typeof(SingleTraceBaseStep)))
                    {
                        (step as SingleTraceBaseStep).ConverterStages = value;
                    }
                }
            }
        }

        [Browsable(false)]
        public bool EnableButton { get; set; } = true;

        [Browsable(true)]
        [EnabledIf("EnableButton", true, HideIfDisabled = false)]
        [Display("Add New Trace", Groups: new[] { "Trace" }, Order: 20)]
        [Layout(LayoutMode.FullRow)]
        public virtual void AddNewTraceButton()
        {
            AddNewTrace();
        }
        #endregion

        public override void Run()
        {
            // Delete dummy trace defined during channel setup
            // DISPlay:MEASure<mnum>:DELete?
            // CALCulate<cnum>:PARameter:DELete[:NAME] <Mname>
            DeleteDummyTrace();

            RunChildSteps(); //If the step supports child steps.

            // If no verdict is used, the verdict will default to NotSet.
            // You can change the verdict using UpgradeVerdict() as shown below.
            UpgradeVerdict(Verdict.Pass);
        }

        protected virtual void AddNewTrace()
        {
        }

        protected virtual void DeleteDummyTrace()
        {
            PNAX.ScpiCommand($"CALCulate{Channel}:PARameter:DELete \'CH{Channel}_DUMMY_{measEnumName}_1\'");
        }
    }
}
