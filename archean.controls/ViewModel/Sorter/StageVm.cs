using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Reactive.Subjects;
using System;

namespace archean.controls.ViewModel.Sorter
{
    public class StageVm
    {
        private readonly Subject<StageVm> _animationFinished = new Subject<StageVm>();
        public IObservable<StageVm> OnAnimationFinished => _animationFinished;

        public StageVm(
            int indexInSorter,
            StageVmStep stageVmStep,
            StageVmStyle stageVmStyle,
            int keyCount,
            IEnumerable<KeyPairVm> keyPairVms,
            SortableItemVm[] sortableItemVms)
        {
            IndexInSorter = indexInSorter;
            StageVmStep = stageVmStep;
            StageVmStyle = stageVmStyle;
            KeyCount = keyCount;
            _keyPairVms = keyPairVms.ToList();
            SortableItemVms = sortableItemVms;
            SectionCount = _keyPairVms.Max(vm => vm.StageSection) + 1;
            VmWidth = 2 * HPadding + (SwitchSpacing) * SectionCount;
            VmHeight = 2 * VPadding + (KeyLineThickness + KeyLineSpacing) * KeyCount + KeyLineSpacing;
        }

        public void RaiseAnimationFinished()
        {
            _animationFinished.OnNext(this);
        }

        public int IndexInSorter { get; }
        public StageVmStep StageVmStep { get; }
        public Brush KeyLineBrush => StageVmStyle.KeyLineBrush;
        public int KeyCount { get; }
        public double SwitchLineWidth => StageVmStyle.SwitchLineWidth;
        public double SwitchSpacing => StageVmStyle.SwitchHSpacing;
        public double KeyLineThickness => StageVmStyle.KeyLineThickness;
        public double KeyLineSpacing => StageVmStyle.KeyLineSpacing;
        public double HPadding => StageVmStyle.HPadding;
        public double VPadding => StageVmStyle.VPadding;
        public Brush BackgroundBrush => StageVmStyle.BackgroundBrush;
        public SortableItemVm[] SortableItemVms { get; }
        public StageVmStyle StageVmStyle { get; }
        readonly List<KeyPairVm> _keyPairVms;
        public IEnumerable<KeyPairVm> KeyPairVms { get { return _keyPairVms; } }

        public int SectionCount { get; }
        public double VmHeight { get; }
        public double VmWidth { get; }
        public double WidthToHeight { get { return (VmHeight > 0) ? VmWidth / VmHeight : 0.0;  } }
    }


    public class StageVmStyle
    {
        public Brush KeyLineBrush { get; set; }
        public Brush SwitchBrushNotUsed { get; set; }
        public Brush SwitchBrushInUse { get; set; }
        public Brush SwitchBrushWasUsed { get; set; }
        public double SwitchLineWidth { get; set; }
        public double SwitchHSpacing { get; set; }
        public double KeyLineThickness { get; set; }
        public double KeyLineSpacing { get; set; }
        public double HPadding { get; set; }
        public double VPadding { get; set; }
        public Brush BackgroundBrush { get; set; }

        public static StageVmStyle Standard(bool oddStep)
        {
            return new StageVmStyle
            {
                KeyLineBrush = Brushes.Blue,
                SwitchBrushNotUsed = Brushes.Black,
                SwitchBrushInUse = Brushes.Yellow,
                SwitchBrushWasUsed = Brushes.GreenYellow,
                SwitchLineWidth = 1.0,
                SwitchHSpacing = 3.25,
                KeyLineThickness = 1.0,
                KeyLineSpacing = 3.0,
                HPadding = 1.0,
                VPadding = 1.0,
                BackgroundBrush = oddStep ? Brushes.Lavender : Brushes.White
            };
        }

    }


    public static class StageVmProcs
    {
        public static StageVm StageToStageVm(this core.Sorting.Stage stage,
                                                  int indexInSorter,
                                                  StageVmStyle stageVmStyle, 
                                                  int keyCount)
        {
            var swLayout = core.Sorting.StageLayout.LayoutSwitchesLoose(keyCount, stage.switches);
            var kpVms = swLayout.ToKeyPairVms(stageVmStyle);
            return new StageVm(
                    stageVmStep: StageVmStep.None,
                    indexInSorter: indexInSorter,
                    stageVmStyle: stageVmStyle,
                    keyCount: keyCount,
                    keyPairVms: kpVms,
                    sortableItemVms: new SortableItemVm[0]
                 );
        }


        public static StageVm SwitchBlocksToStageVm(
                            this core.Sorting.Switch[][] switchBlocks,
                            int indexInSorter,
                            StageVmStyle stageVmStyle, 
                            int keyCount,
                            SortableItemVm[] sortableVms,
                            StageVmStep stageVmStep)
        {
            var kpVms = switchBlocks.ToKeyPairVms(stageVmStyle);
            return new StageVm(
                    stageVmStep: stageVmStep,
                    indexInSorter: indexInSorter,
                    stageVmStyle: stageVmStyle,
                    keyCount: keyCount,
                    keyPairVms: kpVms,
                    sortableItemVms: sortableVms
                 );
        }


        public static SortableItemVm[] ScrambledSortableVms(int keyCount, int seed, bool showLabels)
        {
            return
                ScramblePos(keyCount, seed).ToRedBlueSortableVms(keyCount, showLabels);
        }


        public static int[] ScramblePos(int order, int seed)
        {
            return
                    core.Combinatorics.FisherYatesShuffle(
                    new Random(seed),
                    Enumerable.Range(0, order).ToArray()
                ).ToArray();
        }


        public static StageVm ToNextStep(this StageVm stageVm, SortableItemVm[] sortableVms = null)
        {
            switch (stageVm.StageVmStep)
            {
                case StageVmStep.Left:
                    return new StageVm(
                            stageVmStep: StageVmStep.Presort,
                            indexInSorter: stageVm.IndexInSorter,
                            stageVmStyle: stageVm.StageVmStyle,
                            keyCount: stageVm.KeyCount,
                            keyPairVms: stageVm.KeyPairVms,
                            sortableItemVms: stageVm.SortableItemVms.ToPreSortStep(stageVm.KeyPairVms.ToArray())
                         );

                case StageVmStep.Presort:

                    var newSortableItemVms = stageVm.SortableItemVms.Select(
                        vm => vm.Copy()).ToArray();

                    SortableItemVmExt.SortTheSortableVms(
                        newSortableItemVms, 
                        stageVm.KeyPairVms.ToArray());

                    return new StageVm(
                            stageVmStep: StageVmStep.PostSort,
                            indexInSorter: stageVm.IndexInSorter,
                            stageVmStyle: stageVm.StageVmStyle,
                            keyCount: stageVm.KeyCount,
                            keyPairVms: stageVm.KeyPairVms,
                            sortableItemVms: newSortableItemVms.ToPostSortStep(stageVm.KeyPairVms.ToArray())
                         );
                case StageVmStep.PostSort:

                    foreach(var kpvm in stageVm.KeyPairVms)
                    {
                        if (kpvm.KeyPairUse == KeyPairUse.InUse)
                        {
                            kpvm.KeyPairUse = KeyPairUse.WasUsed;
                        }
                    }

                    return new StageVm(
                            stageVmStep: StageVmStep.Right,
                            indexInSorter: stageVm.IndexInSorter,
                            stageVmStyle: stageVm.StageVmStyle,
                            keyCount: stageVm.KeyCount,
                            keyPairVms: stageVm.KeyPairVms,
                            sortableItemVms: stageVm.SortableItemVms.ToRightStep()
                         );

                case StageVmStep.Right:
                    return new StageVm(
                            stageVmStep: StageVmStep.None,
                            indexInSorter: stageVm.IndexInSorter,
                            stageVmStyle: stageVm.StageVmStyle,
                            keyCount: stageVm.KeyCount,
                            keyPairVms: stageVm.KeyPairVms,
                            sortableItemVms: null
                         );

                case StageVmStep.None:
                    return new StageVm(
                            stageVmStep: StageVmStep.Left,
                            indexInSorter: stageVm.IndexInSorter,
                            stageVmStyle: stageVm.StageVmStyle,
                            keyCount: stageVm.KeyCount,
                            keyPairVms: stageVm.KeyPairVms,
                            sortableItemVms: sortableVms
                         );
                default:
                    throw new System.Exception($"{stageVm.StageVmStep} not handled");
            }

        }



    }
}
