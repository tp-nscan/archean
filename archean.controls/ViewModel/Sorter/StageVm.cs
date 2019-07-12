using archean.controls.ViewModel.Common;
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
            int stageIndex,
            StageVmStep stageVmStep,
            StageVmStyle stageVmStyle,
            int order,
            IEnumerable<KeyPairVm> keyPairVms,
            SortableItemVm[] sortableItemVms,
            SortableItemVm[] sortableItemVmsOld
            )
        {
            StageIndex = stageIndex;
            StageVmStep = stageVmStep;
            StageVmStyle = stageVmStyle;
            Order = order;
            _keyPairVms = keyPairVms.ToList();
            SortableItemVms = sortableItemVms;
            SortableItemVmsOld = sortableItemVmsOld;
            SectionCount = _keyPairVms.Max(vm => vm.StageSection) + 1;
            VmWidth = StageVmStyle.StageRightMargin + 
                      StageVmStyle.SwitchHSpacing * SectionCount;
            VmHeight = 2 * StageVmStyle.VPadding + 
                      StageVmStyle.KeyLineHeight * Order + 
                      StageVmStyle.KeyLineHeight;
        }

        public void RaiseAnimationFinished()
        {
            _animationFinished.OnNext(this);
        }

        public int StageIndex { get; }
        public StageVmStep StageVmStep { get; }
        public int Order { get; }
        public SortableItemVm[] SortableItemVms { get; }
        public SortableItemVm[] SortableItemVmsOld { get; }
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
                                                  int stageIndex,
                                                  StageVmStyle stageVmStyle, 
                                                  int keyCount)
        {
            var swLayout = core.Sorting.StageLayout.LayoutSwitchesLoose(keyCount, stage.switches);
            var kpVms = swLayout.ToKeyPairVms(stageIndex, stageVmStyle);
            return new StageVm(
                    stageVmStep: StageVmStep.None,
                    stageIndex: stageIndex,
                    stageVmStyle: stageVmStyle,
                    order: keyCount,
                    keyPairVms: kpVms,
                    sortableItemVms: null,
                    sortableItemVmsOld: null
                 );
        }


        public static StageVm SwitchBlocksToStageVm(
                            this core.Sorting.ISwitch[][] switchBlocks,
                            int stageIndex,
                            StageVmStyle stageVmStyle, 
                            int order,
                            SortableItemVm[] sortableVms,
                            StageVmStep stageVmStep)
        {
            var kpVms = switchBlocks.ToKeyPairVms(stageIndex, stageVmStyle);
            return new StageVm(
                    stageVmStep: stageVmStep,
                    stageIndex: stageIndex,
                    stageVmStyle: stageVmStyle,
                    order: order,
                    keyPairVms: kpVms,
                    sortableItemVms: sortableVms,
                    sortableItemVmsOld: null
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
                            stageIndex: stageVm.StageIndex,
                            stageVmStyle: stageVm.StageVmStyle,
                            order: stageVm.Order,
                            keyPairVms: stageVm.KeyPairVms,
                            sortableItemVms: stageVm.SortableItemVms.ToPreSortStep(stageVm.KeyPairVms.ToArray()),
                            sortableItemVmsOld: stageVm.SortableItemVms
                         );

                case StageVmStep.Presort:

                    var newKeypairVms = stageVm.KeyPairVms.UpdateKeyPairVms(stageVm.SortableItemVms);
                    var newSortableItemVms = stageVm.SortableItemVms.UpdateSortableVms(stageVm.KeyPairVms);

                    return new StageVm(
                            stageVmStep: StageVmStep.PostSort,
                            stageIndex: stageVm.StageIndex,
                            stageVmStyle: stageVm.StageVmStyle,
                            order: stageVm.Order,
                            keyPairVms: newKeypairVms,
                            sortableItemVms: newSortableItemVms.ToPostSortStep(stageVm.KeyPairVms.ToArray()),
                            sortableItemVmsOld: stageVm.SortableItemVms
                         );


                case StageVmStep.PostSort:

                    return new StageVm(
                            stageVmStep: StageVmStep.Right,
                            stageIndex: stageVm.StageIndex,
                            stageVmStyle: stageVm.StageVmStyle,
                            order: stageVm.Order,
                            keyPairVms: stageVm.KeyPairVms.Select(kvm=>kvm.ToInactive()),
                            sortableItemVms: stageVm.SortableItemVms.ToRightStep(),
                            sortableItemVmsOld: stageVm.SortableItemVms
                         );

                case StageVmStep.Right:
                    return new StageVm(
                            stageVmStep: StageVmStep.None,
                            stageIndex: stageVm.StageIndex,
                            stageVmStyle: stageVm.StageVmStyle,
                            order: stageVm.Order,
                            keyPairVms: stageVm.KeyPairVms,
                            sortableItemVms: null,
                            sortableItemVmsOld: null
                         );

                case StageVmStep.None:
                    return new StageVm(
                            stageVmStep: StageVmStep.Left,
                            stageIndex: stageVm.StageIndex,
                            stageVmStyle: stageVm.StageVmStyle,
                            order: stageVm.Order,
                            keyPairVms: stageVm.KeyPairVms,
                            sortableItemVms: sortableItemVms,
                            sortableItemVmsOld: null
                         );
                default:
                    throw new Exception($"{stageVm.StageVmStep} not handled");
            }

        }


        public static StageVm ClearSwitchUses(this StageVm stageVm)
        {
            return new StageVm(
                    stageVmStep: stageVm.StageVmStep,
                    stageIndex: stageVm.StageIndex,
                    stageVmStyle: stageVm.StageVmStyle,
                    order: stageVm.Order,
                    keyPairVms: stageVm.KeyPairVms.Select(kpvm => kpvm.ResetUseHistory()),
                    sortableItemVms: stageVm.SortableItemVms,
                    sortableItemVmsOld: stageVm.SortableItemVmsOld
                 );
        }

        public static StageVm SetStageVmStyle(this StageVm stageVm, StageVmStyle stageVmStyle)
        {
            return new StageVm(
                    stageVmStep: stageVm.StageVmStep,
                    stageIndex: stageVm.StageIndex,
                    stageVmStyle: stageVmStyle,
                    order: stageVm.Order,
                    keyPairVms: stageVm.KeyPairVms,
                    sortableItemVms: stageVm.SortableItemVms,
                    sortableItemVmsOld: stageVm.SortableItemVmsOld
                 );
        }

    }
}
