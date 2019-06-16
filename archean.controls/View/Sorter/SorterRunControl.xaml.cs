using archean.controls.Utils;
using archean.controls.ViewModel;
using archean.controls.ViewModel.Sorter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace archean.controls.View.Sorter
{
    public partial class SorterRunControl
    {
        public SorterRunControl()
        {
            InitializeComponent();
        }

        #region SorterDisplayVm

        //[Category("Custom Properties")]
        public SorterDisplayVm SorterDisplayVm
        {
            get { return (SorterDisplayVm)GetValue(SorterDisplayVmProperty); }
            set { SetValue(SorterDisplayVmProperty, value); }
        }

        public static readonly DependencyProperty SorterDisplayVmProperty =
            DependencyProperty.Register("SorterDisplayVm", typeof(SorterDisplayVm), typeof(SorterRunControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnSorterVmPropertyChanged));

        private static void OnSorterVmPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sorterControlControl = (SorterRunControl)d;
            var sorterDisplayVm = e.NewValue as SorterDisplayVm;
            if (sorterDisplayVm != null)
            {
                //sorterControlControl.StageLayout = sorterDisplayVm.StageLayout;
                //sorterControlControl.AnimationSpeed = sorterDisplayVm.AnimationSpeed;
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
            SorterDisplayVm.KeepGoing = false;
            SorterDisplayVm.MakeStep();
            if (SorterDisplayVm.CurrentStageVm.StageVmStep == StageVmStep.Right)
            {
                SorterDisplayVm.MakeStep();
            }
        }

        bool CanStep()
        {
            if (SorterDisplayVm == null) return false;

            return (SorterDisplayVm.CurrentStageVm != null) &&
                   (
                    (SorterDisplayVm.CurrentStageVm.StageVmStep != StageVmStep.Right) 
                    ||
                    (SorterDisplayVm.CurrentStageVm.IndexInSorter < SorterDisplayVm.StageVms.Count - 1)
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
            SorterDisplayVm.KeepGoing = true;
            SorterDisplayVm.MakeStep();
        }

        bool CanRun()
        {
            if (SorterDisplayVm == null) return false;

            return (SorterDisplayVm.CurrentStageVm != null) &&
                   (
                    (SorterDisplayVm.CurrentStageVm.StageVmStep != StageVmStep.Right)
                    ||
                    (SorterDisplayVm.CurrentStageVm.IndexInSorter < SorterDisplayVm.StageVms.Count - 1)
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
            var currentIndex = SorterDisplayVm.CurrentStageVm.IndexInSorter;
            SorterDisplayVm.StageVms = new ObservableCollection<StageVm>(
                    SorterDisplayVm.StageVms.Select(stvm => stvm.ClearSwitchUses())
                    );
            SorterDisplayVm.CurrentStageVm = SorterDisplayVm.StageVms[currentIndex];
        }

        bool CanClear()
        {
            if (SorterDisplayVm == null) return false;

            return (SorterDisplayVm.CurrentStageVm != null) &&
                   (SorterDisplayVm.CurrentStageVm.IndexInSorter < SorterDisplayVm.StageVms.Count);
        }

        #endregion // ClearCommand


        #region ResetCommand

        RelayCommand _resetCommand;

        public ICommand ResetCommand => _resetCommand ?? (_resetCommand = new RelayCommand(
                DoReset,
                CanReset
            ));

        private void DoReset()
        {
            SorterDisplayVm.SortableItemVms = StageVmProcs.ScrambledSortableVms(
                SorterDisplayVm.StagedSorterDef.sorterDef.order, DateTime.Now.Millisecond, true);
            SorterDisplayVm.StageVms = new ObservableCollection<StageVm>(
                     SorterDisplayVm.StageVms.ResetSortables(SorterDisplayVm.SortableItemVms));
            SorterDisplayVm.CurrentStageVm = SorterDisplayVm.StageVms[0];
        }

        bool CanReset()
        {
            if (SorterDisplayVm == null) return false;

            return (SorterDisplayVm.CurrentStageVm != null) &&
                   (SorterDisplayVm.CurrentStageVm.IndexInSorter < SorterDisplayVm.StageVms.Count);
        }

        #endregion // ResetCommand


        #region AnimationSpeed

        public IEnumerable<AnimationSpeed> AnimationSpeeds
        {
            get
            {
                return Enum.GetValues(typeof(AnimationSpeed)).Cast<AnimationSpeed>();
            }
        }

        public AnimationSpeed AnimationSpeed
        {
            get { return (AnimationSpeed)GetValue(AnimationSpeedProperty); }
            set { SetValue(AnimationSpeedProperty, value); }
        }

        public static readonly DependencyProperty AnimationSpeedProperty =
            DependencyProperty.Register("AnimationSpeed", typeof(AnimationSpeed), typeof(SorterRunControl),
            new FrameworkPropertyMetadata(AnimationSpeed.None, FrameworkPropertyMetadataOptions.None, OnAnimationSpeedPropertyChanged));

        private static void OnAnimationSpeedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var sorterRunControl = (SorterRunControl)d;
            //var animationSpeed = (AnimationSpeed)e.NewValue;
            //if (sorterRunControl.SorterDisplayVm != null)
            //{
            //    sorterRunControl.SorterDisplayVm.AnimationSpeed = animationSpeed;
            //    sorterRunControl.DoReset();
            //}
        }

        #endregion


        #region StageLayout

        public IEnumerable<StageLayout> StageLayouts
        {
            get
            {
                return Enum.GetValues(typeof(StageLayout)).Cast<StageLayout>();
            }
        }

        public StageLayout StageLayout
        {
            get { return (StageLayout)GetValue(StageLayoutProperty); }
            set { SetValue(StageLayoutProperty, value); }
        }

        public static readonly DependencyProperty StageLayoutProperty =
            DependencyProperty.Register("StageLayout", typeof(StageLayout), typeof(SorterRunControl),
            new FrameworkPropertyMetadata(StageLayout.Single, FrameworkPropertyMetadataOptions.None, OnStageLayoutPropertyChanged));

        private static void OnStageLayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var sorterRunControl = (SorterRunControl)d;
            //var stageLayout = (StageLayout)e.NewValue;
            //if (sorterRunControl.SorterDisplayVm != null)
            //{
            //    sorterRunControl.SorterDisplayVm.StageLayout = stageLayout;
            //    sorterRunControl.DoReset();
            //}
        }

        #endregion

    }
}
