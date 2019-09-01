using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System;

namespace archean.controls.ViewModel.Sorter2
{
    public class StageVm
    {
        private readonly Subject<StageVm> _animationFinished = new Subject<StageVm>();
        public IObservable<StageVm> OnAnimationFinished => _animationFinished;

        public StageVm(
            int stageIndex,
            StageVmStyle stageVmStyle,
            int order,
            IEnumerable<KeyPairVm> keyPairVms,
            SortableVm sortableVms
        )
        {
            StageIndex = stageIndex;
            StageVmStyle = stageVmStyle;
            Order = order;
            _keyPairVms = keyPairVms.ToList();
            SortableVms = sortableVms;
            SectionCount = _keyPairVms.Max(vm => vm.StageSection) + 1;
            VmWidth = StageVmStyle.StageRightMargin + 
                      StageVmStyle.SwitchHSpacing * SectionCount;
            VmHeight = 2 * StageVmStyle.VPadding + 
                      StageVmStyle.KeyLineHeight * Order + 
                      StageVmStyle.KeyLineHeight;
        }

        public IEnumerable<KeyPairVm> KeyPairVms { get { return _keyPairVms; } }
        public int Order { get; }
        public int SectionCount { get; }
        public int StageIndex { get; }
        public SortableVm SortableVms { get; }
        public StageVmStyle StageVmStyle { get; }
        readonly List<KeyPairVm> _keyPairVms;
        public double WidthToHeight { get { return (VmHeight > 0) ? VmWidth / VmHeight : 0.0;  } }
        public double VmHeight { get; }
        public double VmWidth { get; }
    }


    public static class StageVmProcs
    {
        public static StageVm ToStageVmNoSortables(this core.Sorting.Stage stage,
                                                  int stageIndex,
                                                  StageVmStyle stageVmStyle, 
                                                  int keyCount)
        {
            var swLayout = core.Sorting.StageLayout.LayoutSwitchesLoose(keyCount, stage.switches);
            var kpVms = swLayout.ToKeyPairVms(stageIndex, stageVmStyle);
            return new StageVm(
                    stageIndex: stageIndex,
                    stageVmStyle: stageVmStyle,
                    order: keyCount,
                    keyPairVms: kpVms,
                    sortableVms: null
                 );
        }


        public static StageVm SwitchBlocksToStageVm(
                            this core.Sorting.ISwitch[][] switchBlocks,
                            int stageIndex,
                            StageVmStyle stageVmStyle, 
                            int order,
                            SortableVm sortableVms)
        {
            var kpVms = switchBlocks.ToKeyPairVms(stageIndex, stageVmStyle);
            return new StageVm(
                    stageIndex: stageIndex,
                    stageVmStyle: stageVmStyle,
                    order: order,
                    keyPairVms: kpVms,
                    sortableVms: sortableVms
                 );
        }


        public static SortableItemVm[] RandomPermutationSortableItemVms(int keyCount, int seed, bool showLabels)
        {
            return
                RandomPermutation(keyCount, seed).ToRedBlueSortableVms(keyCount, showLabels);
        }

        public static SortableItemVm[] Random_0_1_SortableItemVms(int keyCount, int seed, bool showLabels)
        {
            return
                Random_0_1_Array(keyCount, seed).ToBlackOrWhiteSortableVms(keyCount, showLabels);
        }

        public static int[] RandomPermutation(int order, int seed)
        {
            return
                    core.Combinatorics.FisherYatesShuffle(
                    new Random(seed),
                    Enumerable.Range(0, order).ToArray()
                ).ToArray();
        }

        public static int[] Random_0_1_Array(int order, int seed)
        {
            return
                    core.Combinatorics.Random_0_1(
                    rnd:new Random(seed),
                    len:order, 
                    pctOnes: 0.5
                ).ToArray();
        }

        public static StageVm ToNextStep(this StageVm stageVm, SortableVm sortableVms = null)
        {
            switch (stageVm.SortableVms.StageVmStep)
            {
                case StageVmStep.Left:
                    return new StageVm(
                            stageIndex: stageVm.StageIndex,
                            stageVmStyle: stageVm.StageVmStyle,
                            order: stageVm.Order,
                            keyPairVms: stageVm.KeyPairVms,
                            sortableVms: stageVm.SortableVms.ToPreSortStep(
                                stageVm.KeyPairVms.ToArray())
                         );

                case StageVmStep.Presort:

                    var newKeypairVms = stageVm.KeyPairVms.UpdateKeyPairVms(stageVm.SortableVms.CurrentSortableItemVms);
                    var newSortableVms = new SortableVm(
                        currentSortableItemVms: stageVm.SortableVms.CurrentSortableItemVms.UpdateSortableVms(stageVm.KeyPairVms).ToArray(),
                        nextSortableItemVms: null,
                        stageVmStep: stageVm.SortableVms.StageVmStep,
                        animationPct: 0);

                    return new StageVm(
                            stageIndex: stageVm.StageIndex,
                            stageVmStyle: stageVm.StageVmStyle,
                            order: stageVm.Order,
                            keyPairVms: newKeypairVms,
                            sortableVms: newSortableVms.ToPostSortStep(stageVm.KeyPairVms.ToArray())
                         );


                case StageVmStep.PostSort:

                    return new StageVm(
                            stageIndex: stageVm.StageIndex,
                            stageVmStyle: stageVm.StageVmStyle,
                            order: stageVm.Order,
                            keyPairVms: stageVm.KeyPairVms.Select(kvm=>kvm.ToInactive()),
                            sortableVms: stageVm.SortableVms.ToRightStep()
                         );

                case StageVmStep.Right:
                    return new StageVm(
                            stageIndex: stageVm.StageIndex,
                            stageVmStyle: stageVm.StageVmStyle,
                            order: stageVm.Order,
                            keyPairVms: stageVm.KeyPairVms,
                            sortableVms: null
                         );

                case StageVmStep.None:
                    return new StageVm(
                            stageIndex: stageVm.StageIndex,
                            stageVmStyle: stageVm.StageVmStyle,
                            order: stageVm.Order,
                            keyPairVms: stageVm.KeyPairVms,
                            sortableVms: sortableVms
                         );
                default:
                    throw new Exception($"{stageVm.SortableVms.StageVmStep} not handled");
            }

        }


        public static StageVm ClearSwitchUses(this StageVm stageVm)
        {
            return new StageVm(
                    stageIndex: stageVm.StageIndex,
                    stageVmStyle: stageVm.StageVmStyle,
                    order: stageVm.Order,
                    keyPairVms: stageVm.KeyPairVms.Select(kpvm => kpvm.ResetUseHistory()),
                    sortableVms: stageVm.SortableVms
                 );
        }

        public static StageVm ClearAll(this StageVm stageVm)
        {
            return new StageVm(
                    stageIndex: stageVm.StageIndex,
                    stageVmStyle: stageVm.StageVmStyle,
                    order: stageVm.Order,
                    keyPairVms: stageVm.KeyPairVms.Select(kpvm => kpvm.ResetUseHistory()),
                    sortableVms: null
                 );
        }

        public static StageVm SetStageVmStyle(this StageVm stageVm, StageVmStyle stageVmStyle)
        {
            return new StageVm(
                    stageIndex: stageVm.StageIndex,
                    stageVmStyle: stageVmStyle,
                    order: stageVm.Order,
                    keyPairVms: stageVm.KeyPairVms,
                    sortableVms: stageVm.SortableVms
                 );
        }

    }
}
