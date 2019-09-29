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
            SortableVm = sortableVms;
            SectionCount = _keyPairVms.Max(vm => vm.StageSection) + 1;
        }

        readonly List<KeyPairVm> _keyPairVms;
        public IEnumerable<KeyPairVm> KeyPairVms { get { return _keyPairVms; } }
        public int Order { get; }
        public int SectionCount { get; }
        public int StageIndex { get; }
        public SortableVm SortableVm { get; }
        public StageVmStyle StageVmStyle { get; }
    }


    public static class StageVmProcs
    {

        public static double WidthToHeight(this StageVm stageVm)
        {
            return stageVm.StageVmStyle.SortableVmStyle
                          .WidthToHeight(stageVm.SectionCount, stageVm.Order);
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
                    stageVmStyle: stageVmStyle.ChangeSectionCount(switchBlocks.Length),
                    order: order,
                    keyPairVms: kpVms,
                    sortableVms: sortableVms.ChangeSectionCount(switchBlocks.Length)
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

        public static Tuple<List<StageVm>, SortableVm> Step(this IEnumerable<StageVm> stageVms, SortableVm sortableVm)
        {
            var curSortableVm = sortableVm;
            var lst = new List<StageVm>();
            foreach(var stageVm in stageVms)
            {
                var res = stageVm.ToNextStep(curSortableVm);
                lst.Add(res.Item1);
                curSortableVm = res.Item2;
            }
            return new Tuple<List<StageVm>, SortableVm>(lst, curSortableVm);
        }

        public static Tuple<StageVm, SortableVm> ToNextStep(this StageVm stageVm, SortableVm sortableVms = null)
        {
            if(stageVm.SortableVm == null)
            {
                return new Tuple<StageVm, SortableVm>(
                              item1: new StageVm(
                                          stageIndex: stageVm.StageIndex,
                                          stageVmStyle: stageVm.StageVmStyle,
                                          order: stageVm.Order,
                                          keyPairVms: stageVm.KeyPairVms,
                                          sortableVms: sortableVms.ChangeSectionCount(stageVm.StageVmStyle.SortableVmStyle.SectionCount)),
                              item2: null);
            }


            switch (stageVm.SortableVm.StageVmStep)
            {
                case StageVmStep.Left:

                    return new Tuple<StageVm, SortableVm>(
                                 item1: new StageVm(
                                              stageIndex: stageVm.StageIndex,
                                              stageVmStyle: stageVm.StageVmStyle,
                                              order: stageVm.Order,
                                              keyPairVms: stageVm.KeyPairVms,
                                              sortableVms: stageVm.SortableVm.ToPreSortStep(stageVm.KeyPairVms.ToArray())),
                                  item2: null);

                case StageVmStep.Presort:

                    var newKeypairVms = stageVm.KeyPairVms.UpdateKeyPairVms(stageVm.SortableVm.CurrentSortableItemVms);
                    var newSortableVms = new SortableVm(
                        order: stageVm.Order,
                        sortableVmStyle: stageVm.SortableVm.SortableVmStyle,
                        currentSortableItemVms: stageVm.SortableVm.CurrentSortableItemVms.UpdateSortableVms(stageVm.KeyPairVms).ToArray(),
                        nextSortableItemVms: null,
                        stageVmStep: stageVm.SortableVm.StageVmStep,
                        animationPct: 0);

                    return new Tuple<StageVm, SortableVm>(
                                  item1: new StageVm(
                                              stageIndex: stageVm.StageIndex,
                                              stageVmStyle: stageVm.StageVmStyle,
                                              order: stageVm.Order,
                                              keyPairVms: newKeypairVms,
                                              sortableVms: newSortableVms.ToPostSortStep(stageVm.KeyPairVms.ToArray())),
                                  item2: null);

                case StageVmStep.PostSort:

                    return new Tuple<StageVm, SortableVm>(
                                  item1: new StageVm(
                                              stageIndex: stageVm.StageIndex,
                                              stageVmStyle: stageVm.StageVmStyle,
                                              order: stageVm.Order,
                                              keyPairVms: stageVm.KeyPairVms,
                                              sortableVms: stageVm.SortableVm.ToRightStep()),
                                  item2: null);

                case StageVmStep.Right:

                    return new Tuple<StageVm, SortableVm>(
                                  item1: new StageVm(
                                              stageIndex: stageVm.StageIndex,
                                              stageVmStyle: stageVm.StageVmStyle,
                                              order: stageVm.Order,
                                              keyPairVms: stageVm.KeyPairVms,
                                              sortableVms: null),
                                  item2: stageVm.SortableVm.ToLeftStep());

                case StageVmStep.None:

                    return new Tuple<StageVm, SortableVm>(
                                  item1: new StageVm(
                                              stageIndex: stageVm.StageIndex,
                                              stageVmStyle: stageVm.StageVmStyle,
                                              order: stageVm.Order,
                                              keyPairVms: stageVm.KeyPairVms,
                                              sortableVms: sortableVms),
                                  item2: null);
                default:
                    throw new Exception($"{stageVm.SortableVm.StageVmStep} not handled");
            }

        }


        public static StageVm ToNextStepOld(this StageVm stageVm, SortableVm sortableVms = null)
        {
            switch (stageVm.SortableVm.StageVmStep)
            {
                case StageVmStep.Left:
                    return new StageVm(
                            stageIndex: stageVm.StageIndex,
                            stageVmStyle: stageVm.StageVmStyle,
                            order: stageVm.Order,
                            keyPairVms: stageVm.KeyPairVms,
                            sortableVms: stageVm.SortableVm.ToPreSortStep(
                                stageVm.KeyPairVms.ToArray())
                         );

                case StageVmStep.Presort:

                    var newKeypairVms = stageVm.KeyPairVms.UpdateKeyPairVms(stageVm.SortableVm.CurrentSortableItemVms);
                    var newSortableVms = new SortableVm(
                        order: stageVm.Order,
                        sortableVmStyle: stageVm.SortableVm.SortableVmStyle,
                        currentSortableItemVms: stageVm.SortableVm.CurrentSortableItemVms.UpdateSortableVms(stageVm.KeyPairVms).ToArray(),
                        nextSortableItemVms: null,
                        stageVmStep: stageVm.SortableVm.StageVmStep,
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
                            sortableVms: stageVm.SortableVm.ToRightStep()
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
                    throw new Exception($"{stageVm.SortableVm.StageVmStep} not handled");
            }

        }


        public static StageVm ClearSwitchUses(this StageVm stageVm)
        {
            return new StageVm(
                    stageIndex: stageVm.StageIndex,
                    stageVmStyle: stageVm.StageVmStyle,
                    order: stageVm.Order,
                    keyPairVms: stageVm.KeyPairVms.Select(kpvm => kpvm.ResetUseHistory()),
                    sortableVms: stageVm.SortableVm
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
                    sortableVms: stageVm.SortableVm
                 );
        }

    }
}
