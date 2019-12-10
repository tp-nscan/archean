using archean.controls.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace archean.ViewModel.Pages
{
    public class GoldbachPageVm : BindableBase
    {

        #region StepCommand

        RelayCommand _stepCommand;

        public ICommand StepCommand => _stepCommand ?? (_stepCommand = new RelayCommand(
            DoStep,
            CanStep
            ));

        private void DoStep()
        {


        }

        bool CanStep()
        {
            return true;
        }

        #endregion // StepCommand


    }
}
