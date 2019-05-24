using archean.Common;
using archean.controls.DesignVms.Sorter;
using archean.controls.ViewModel.Sorter;
using System.Windows.Input;

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


        #region StepCommand

        RelayCommand _stepCommand;

        public ICommand StepCommand => _stepCommand ?? (_stepCommand = new RelayCommand(
            DoStep,
            CanStep
            ));

        private void DoStep()
        {
           // StageVm = StageVm.ToNextStep();
        }

        bool CanStep()
        {
            return true;
        }

        #endregion // StepCommand
    }
}
