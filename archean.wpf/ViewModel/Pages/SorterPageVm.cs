using archean.controls.DesignVms.Sorter;
using archean.controls.ViewModel.Sorter;
using System.Windows.Input;
using archean.controls.Utils;

namespace archean.ViewModel.Pages
{
    public class SorterPageVm : BindableBase
    {

        public SorterPageVm()
        {
            SorterVm = new SorterVmD();
        }

        SorterVm _sorterVm;
        public SorterVm SorterVm
        {
            get => _sorterVm;
            set => SetProperty(ref _sorterVm, value);
        }
    }
}
