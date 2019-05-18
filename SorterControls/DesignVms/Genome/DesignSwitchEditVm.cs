using SorterControls.ViewModel.Genome;

namespace SorterControls.DesignVms.Genome
{
    public class DesignSwitchEditVm : SwitchEditorVm
    {
        public DesignSwitchEditVm() : base(16)
        {
            SorterPosition = 1004;
            LowKey = 5;
            HiKey = 11;
        }
    }
}
