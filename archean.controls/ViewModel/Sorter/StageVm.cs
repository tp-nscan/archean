using System.Collections.Generic;
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
            VmWidth = StageVmStyle.StageRightMargin + 
                      StageVmStyle.SwitchHSpacing * SectionCount;
            VmHeight = 2 * StageVmStyle.VPadding + 
                      StageVmStyle.KeyLineHeight * KeyCount + 
                      StageVmStyle.KeyLineHeight;
        }

        public void RaiseAnimationFinished()
        {
            _animationFinished.OnNext(this);
        }

        public int IndexInSorter { get; }
        public StageVmStep StageVmStep { get; }
        public int KeyCount { get; }
        public SortableItemVm[] SortableItemVms { get; }
        public StageVmStyle StageVmStyle { get; }
        readonly List<KeyPairVm> _keyPairVms;
        public IEnumerable<KeyPairVm> KeyPairVms { get { return _keyPairVms; } }

        public int SectionCount { get; }
        public double VmHeight { get; }
        public double VmWidth { get; }
        public double WidthToHeight { get { return (VmHeight > 0) ? VmWidth / VmHeight : 0.0;  } }
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


        public static StageVm ToNextStep(this StageVm stageVm, SortableItemVm[] sortableItemVms = null)
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
                            sortableItemVms: sortableItemVms
                         );
                default:
                    throw new System.Exception($"{stageVm.StageVmStep} not handled");
            }

        }


        public static StageVm ClearSwitchUses(this StageVm stageVm)
        {
            return new StageVm(
                    stageVmStep: stageVm.StageVmStep,
                    indexInSorter: stageVm.IndexInSorter,
                    stageVmStyle: stageVm.StageVmStyle,
                    keyCount: stageVm.KeyCount,
                    keyPairVms: stageVm.KeyPairVms.Select(
                        stvm => { stvm.KeyPairUse = KeyPairUse.NotUsed;
                                  return stvm; }),
                    sortableItemVms: stageVm.SortableItemVms
                 );
        }

        public static StageVm SetStageVmStyle(this StageVm stageVm, StageVmStyle stageVmStyle)
        {
            return new StageVm(
                    stageVmStep: stageVm.StageVmStep,
                    indexInSorter: stageVm.IndexInSorter,
                    stageVmStyle: stageVmStyle,
                    keyCount: stageVm.KeyCount,
                    keyPairVms: stageVm.KeyPairVms,
                    sortableItemVms: stageVm.SortableItemVms
                 );
        }

    }
}
