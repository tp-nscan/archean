using archean.controls.Utils;
using archean.controls.ViewModel;
using archean.controls.ViewModel.Sorter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace archean.controls.View.Sorter
{
    public partial class SorterControlControl
    {
        public SorterControlControl()
        {
            InitializeComponent();
        }

        #region SorterVm

        [Category("Custom Properties")]
        public SorterVm SorterVm
        {
            get { return (SorterVm)GetValue(SorterVmProperty); }
            set { SetValue(SorterVmProperty, value); }
        }

        public static readonly DependencyProperty SorterVmProperty =
            DependencyProperty.Register("SorterVm", typeof(SorterVm), typeof(SorterControlControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnSorterVmPropertyChanged));

        private static void OnSorterVmPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sorterControlControl = (SorterControlControl)d;
            var sorterVm = e.NewValue as SorterVm;
            if (sorterVm != null)
            {
                sorterControlControl.StageLayout = sorterVm.StageLayout;
                sorterControlControl.AnimationSpeed = sorterVm.AnimationSpeed;
            }
        }

        #endregion


        #region StepCommand

        RelayCommand _stepCommand;

        public RelayCommand StepCommand => _stepCommand ?? (_stepCommand = new RelayCommand(
                DoStep,
                CanStep
            ));

        private void DoStep()
        {
            SorterVm.KeepGoing = false;
            SorterVm.MakeStep();
            if (SorterVm.CurrentStageVm.StageVmStep == StageVmStep.Right)
            {
                SorterVm.MakeStep();
            }
          //  StepCommand.RaiseCanExecuteChanged();
        }

        bool CanStep()
        {
            if (SorterVm == null) return false;

            return (SorterVm.CurrentStageVm != null) &&
                   (
                    (SorterVm.CurrentStageVm.StageVmStep != StageVmStep.Right) 
                    ||
                    (SorterVm.CurrentStageVm.IndexInSorter < SorterVm.StageVms.Count - 1)
                   );
        }

        #endregion // StepCommand


        #region RunCommand

        RelayCommand _runCommand;

        public ICommand RunCommand => _runCommand ?? (_runCommand = new RelayCommand(
                DoRun,
                CanRun
            ));

        private void DoRun()
        {
            SorterVm.KeepGoing = true;
            SorterVm.MakeStep();
        }

        bool CanRun()
        {
            if (SorterVm == null) return false;

            return (SorterVm.CurrentStageVm != null) &&
                   (
                    (SorterVm.CurrentStageVm.StageVmStep != StageVmStep.Right)
                    ||
                    (SorterVm.CurrentStageVm.IndexInSorter < SorterVm.StageVms.Count - 1)
                   );
        }

        #endregion // RunCommand


        #region ClearCommand

        RelayCommand _clearCommand;

        public ICommand ClearCommand => _clearCommand ?? (_clearCommand = new RelayCommand(
                DoClear,
                CanClear
            ));

        private void DoClear()
        {
            SorterVm.StageVms = new ObservableCollection<StageVm>(
                    SorterVm.StageVms.Select(stvm => stvm.ClearSwitchUses())
                    );
        }

        bool CanClear()
        {
            if (SorterVm == null) return false;

            return (SorterVm.CurrentStageVm != null) &&
                   (SorterVm.CurrentStageVm.IndexInSorter < SorterVm.StageVms.Count);
        }

        #endregion // ClearCommand


        #region PackCommand

        RelayCommand<string> _packCommand;

        public ICommand PackCommand => _packCommand ?? (_packCommand = new RelayCommand<string>(
                DoPack,
                CanPack
            ));

        private void DoPack(string param)
        {
            if (param == "Tight")
            {
                SorterVm.StageLayout = StageLayout.Tight;
            }
            else if (param == "Loose")
            {
                SorterVm.StageLayout = StageLayout.Loose;
            }
            else
            {
                SorterVm.StageLayout = StageLayout.Single;
            }
        }

        bool CanPack(string param)
        {
            if (SorterVm == null) return false;

            return (SorterVm.CurrentStageVm != null) &&
                   (SorterVm.CurrentStageVm.IndexInSorter < SorterVm.StageVms.Count);
        }

        #endregion // PackCommand


        #region ResetCommand

        RelayCommand<string> _resetCommand;

        public ICommand ResetCommand => _resetCommand ?? (_resetCommand = new RelayCommand<string>(
                DoReset,
                CanReset
            ));

        private void DoReset(string param)
        {
            SorterVm.SortableItemVms = StageVmProcs.ScrambledSortableVms(
                SorterVm.StagedSorterDef.sorterDef.order, DateTime.Now.Millisecond, true);
            SorterVm.StageVms = new ObservableCollection<StageVm>(
                     SorterVm.StageVms.ResetSortables(SorterVm.SortableItemVms));
            SorterVm.CurrentStageVm = SorterVm.StageVms[0];
        }

        bool CanReset(string param)
        {
            if (SorterVm == null) return false;

            return (SorterVm.CurrentStageVm != null) &&
                   (SorterVm.CurrentStageVm.IndexInSorter < SorterVm.StageVms.Count);
        }

        #endregion // ResetCommand


        public IEnumerable<AnimationSpeed> AnimationSpeeds
        {
            get
            {
                return Enum.GetValues(typeof(AnimationSpeed)).Cast<AnimationSpeed>();
            }
        }

        public AnimationSpeed AnimationSpeed
        {
            get {
                return (SorterVm == null) ? 
                    AnimationSpeed.None : 
                    SorterVm.AnimationSpeed; }
            set
            {
                SorterVm.AnimationSpeed = value;
            }
        }


        public IEnumerable<StageLayout> StageLayouts
        {
            get
            {
                return Enum.GetValues(typeof(StageLayout)).Cast<StageLayout>();
            }
        }

        private StageLayout _stageLayout = StageLayout.Single;
        public StageLayout StageLayout
        {
            get { return _stageLayout; }
            set
            {
                _stageLayout = value;
                SorterVm.StageLayout = value;
            }
        }

    }
}
