using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using archean.controls.Utils;

namespace archean.ViewModel.Pages
{
    public class BenchmarkPageVm : BindableBase
    {
        public BenchmarkPageVm()
        {
            _stopwatch = new Stopwatch();
        }

        Stopwatch _stopwatch;

        #region StepCommand

        RelayCommand _stepCommand;

        public ICommand StepCommand => _stepCommand ?? (
                _stepCommand = new RelayCommand
                                    (
                                        DoStep,
                                        CanStep
                                    ));

        private void DoStep()
        {
            var task = Task.Factory.StartNew(() => {
                RunBatches();
            }).ContinueWith(t => {
              Application.Current.Dispatcher.Invoke(() => Message = "Done");
            });
        }

        bool CanStep()
        {
            return true;
        }

        #endregion // StepCommand

        private void RunBatches()
        {
            try
            {
                _stopwatch.Reset();
                _stopwatch.Start();
                Time = String.Empty;
                Result = String.Empty;
                Message = "Running";
                int successCount = 0;
                if(Parallel)
                {
                    successCount = core.Benchmarks.SorterBenchB(123, 16, 80, 300);
                }
                else
                {
                    successCount = core.Benchmarks.SorterBenchA(123, 16, 80, 300);
                }

                Result = successCount.ToString();
                _stopwatch.Stop();
                Time = _stopwatch.Elapsed.ToString();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private string _result;
        public string Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private string _time;
        public string Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }

        private bool _parallel;
        public bool Parallel
        {
            get => _parallel;
            set => SetProperty(ref _parallel, value);
        }
    }
}
