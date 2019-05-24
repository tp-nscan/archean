using archean.controls.DesignVms.Sorter;
using archean.controls.ViewModel.Sorter;
using System.Windows.Input;
using archean.controls.Utils;

namespace archean.ViewModel.Pages
{
    public class StagePageVm : BindableBase
    {
        public StagePageVm()
        {
            StageVm = new StageVmD();
        }

        StageVm _stageVm;
        public StageVm StageVm
        {
            get => _stageVm;
            set => SetProperty(ref _stageVm, value);
        }


        #region StepCommand

        RelayCommand _stepCommand;

        public ICommand StepCommand => _stepCommand ?? (_stepCommand = new RelayCommand(
            DoStep,
            CanStep
            ));

        private void DoStep()
        {
            StageVm = StageVm.ToNextStep();
        }

        bool CanStep()
        {
            return true;
        }

        #endregion // StepCommand
    }
}
