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
            SortableVm sortableVm
        )
        {
            StageIndex = stageIndex;
            StageVmStyle = stageVmStyle;
            Order = order;
            _keyPairVms = keyPairVms.ToList();
            SortableVm = sortableVm;
            SectionCount = _keyPairVms.MaxOr(vm => vm.StageSection, 0) + 1;
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

        public static V MaxOr<T,V>(this IEnumerable<T> vals, Func<T,V> keySelector, V defVal)
        {
            var lstRes = vals.ToList();
            if (lstRes.Count == 0) return defVal;
            return vals.Max(keySelector);
        }

        public static double WidthToHeight(this StageVm stageVm)
        {
            return stageVm.StageVmStyle.SortableVmStyle.StageWidthToHeight();
        }

        public static StageVm SwitchBlocksToStageVm(
                            this core.Sorting.ISwitch[][] switchBlocks,
                            int stageIndex,
                            StageVmStyle stageVmStyle, 
                            int order,
                            SortableVm sortableVm)
        {
            var kpVms = switchBlocks.ToKeyPairVms(stageIndex, stageVmStyle);
            return new StageVm(
                    stageIndex: stageIndex,
                    stageVmStyle: stageVmStyle.ChangeSectionCount(switchBlocks.Length),
                    order: order,
                    keyPairVms: kpVms,
                    sortableVm: sortableVm.ChangeSectionCount(switchBlocks.Length)
                 );
        }

        public static List<StageVm> Step(this IEnumerable<StageVm> stageVms, SortableVm sortableVm)
        {
            var curSortableVm = sortableVm;
            var lst = new List<StageVm>();
            foreach (var stageVm in stageVms)
            {
                var res = stageVm.ToNextStep(curSortableVm);
                lst.Add(res.Item1);
                curSortableVm = res.Item2;
            }
            return lst;
        }

        public static Tuple<StageVm, SortableVm> ToNextStep(this StageVm stageVm, SortableVm sortableVm = null)
        {
            if(stageVm.SortableVm == null)
            {
                if(sortableVm == null)
                { 
                    return new Tuple<StageVm, SortableVm>(
                       item1: stageVm,
                       item2: null);
                }
                return new Tuple<StageVm, SortableVm>(
                    item1: new StageVm(
                                stageIndex: stageVm.StageIndex,
                                stageVmStyle: stageVm.StageVmStyle,
                                order: stageVm.Order,
                                keyPairVms: stageVm.KeyPairVms,
                                sortableVm: sortableVm.ChangeSectionCount(stageVm.StageVmStyle.SortableVmStyle.SectionCount)),
                    item2: null);
            }

            switch (stageVm.SortableVm.StageVmStep)
            {
                case StageVmStep.Init:

                    return new Tuple<StageVm, SortableVm>(
                                 item1: new StageVm(
                                              stageIndex: stageVm.StageIndex,
                                              stageVmStyle: stageVm.StageVmStyle,
                                              order: stageVm.Order,
                                              keyPairVms: stageVm.KeyPairVms,
                                              sortableVm: stageVm.SortableVm.ToLeftStep(stageVm.KeyPairVms.ToArray())),
                                  item2: null);

                case StageVmStep.Left:

                    return new Tuple<StageVm, SortableVm>(
                                 item1: new StageVm(
                                              stageIndex: stageVm.StageIndex,
                                              stageVmStyle: stageVm.StageVmStyle,
                                              order: stageVm.Order,
                                              keyPairVms: stageVm.KeyPairVms,
                                              sortableVm: stageVm.SortableVm.ToPreSortStep(stageVm.KeyPairVms.ToArray())),
                                  item2: null);


                case StageVmStep.Presort:

                    return new Tuple<StageVm, SortableVm>(
                                  item1: new StageVm(
                                              stageIndex: stageVm.StageIndex,
                                              stageVmStyle: stageVm.StageVmStyle,
                                              order: stageVm.Order,
                                              keyPairVms: stageVm.KeyPairVms.UpdateKeyPairVms(stageVm.SortableVm.PastSortableItemVms),
                                              sortableVm: stageVm.SortableVm.ToPostSortStep()),
                                  item2: stageVm.SortableVm.ToInitStep());


                case StageVmStep.PostSort:

                    return new Tuple<StageVm, SortableVm>(
                                  item1: new StageVm(
                                              stageIndex: stageVm.StageIndex,
                                              stageVmStyle: stageVm.StageVmStyle,
                                              order: stageVm.Order,
                                              keyPairVms: stageVm.KeyPairVms,
                                              sortableVm: null),
                                  item2: null);

                default:
                    throw new Exception($"{stageVm.SortableVm.StageVmStep} not handled");
            }

        }

        public static List<StageVm> Tic(
            this IEnumerable<StageVm> stageVms,
            SortableVm sortableVm, 
            double animationPct)
        {
            var curSortableVm = sortableVm;
            var lst = new List<StageVm>();

            //foreach (var stageVm in stageVms)
            //{
            //    var res = stageVm.ToNextStep(curSortableVm);
            //    lst.Add(res.Item1);
            //    curSortableVm = null;
            //}
            //return lst;
            return stageVms.Select(vm => vm.ToNextTic(animationPct))
                           .ToList();
        }

        public static StageVm ToNextTic(this StageVm stageVm,
                        double animationPct)
        {
            if (stageVm.SortableVm == null)
            {
                return stageVm;
            }
            else
            {
                return new StageVm(
                              stageIndex: stageVm.StageIndex,
                              stageVmStyle: stageVm.StageVmStyle,
                              order: stageVm.Order,
                              keyPairVms: stageVm.KeyPairVms,
                              sortableVm: stageVm.SortableVm.ChangeAnimationPct(animationPct));
            }
        }


        public static Tuple<StageVm, SortableVm> ToNextTicOld(this StageVm stageVm, 
                                double animationPct, SortableVm sortableVm = null)
        {
            if (stageVm.SortableVm == null)
            {
                if (sortableVm == null)
                {
                    return new Tuple<StageVm, SortableVm>(
                       item1: stageVm,
                       item2: null);
                }
                return new Tuple<StageVm, SortableVm>(
                    item1: new StageVm(
                                stageIndex: stageVm.StageIndex,
                                stageVmStyle: stageVm.StageVmStyle,
                                order: stageVm.Order,
                                keyPairVms: stageVm.KeyPairVms,
                                sortableVm: stageVm.SortableVm.ChangeSectionCount(stageVm.StageVmStyle.SortableVmStyle.SectionCount)
                                                      .ChangeAnimationPct(animationPct)),
                    item2: null);
            }
            else
            {
                return new Tuple<StageVm, SortableVm>(
                  item1: new StageVm(
                              stageIndex: stageVm.StageIndex,
                              stageVmStyle: stageVm.StageVmStyle,
                              order: stageVm.Order,
                              keyPairVms: stageVm.KeyPairVms,
                              sortableVm: stageVm.SortableVm.ChangeAnimationPct(animationPct)),
                  item2: null);
            }

        }


        public static StageVm ChangeAnimationPct(this StageVm stageVm, double animationPct)
        {
            if (stageVm == null) return null;

            return new StageVm
                (
                    stageIndex: stageVm.StageIndex,
                    stageVmStyle: stageVm.StageVmStyle,
                    order: stageVm.Order,
                    keyPairVms: stageVm.KeyPairVms.Select(kpvm => kpvm.ResetUseHistory()),
                    sortableVm: stageVm.SortableVm.ChangeAnimationPct(animationPct)
                );
        }

        public static StageVm ClearSwitchUses(this StageVm stageVm)
        {
            return new StageVm(
                    stageIndex: stageVm.StageIndex,
                    stageVmStyle: stageVm.StageVmStyle,
                    order: stageVm.Order,
                    keyPairVms: stageVm.KeyPairVms.Select(kpvm => kpvm.ResetUseHistory()),
                    sortableVm: stageVm.SortableVm
                 );
        }

        public static StageVm ClearAll(this StageVm stageVm)
        {
            return new StageVm(
                    stageIndex: stageVm.StageIndex,
                    stageVmStyle: stageVm.StageVmStyle,
                    order: stageVm.Order,
                    keyPairVms: stageVm.KeyPairVms.Select(kpvm => kpvm.ResetUseHistory()),
                    sortableVm: null
                 );
        }

        public static StageVm SetStageVmStyle(this StageVm stageVm, StageVmStyle stageVmStyle)
        {
            return new StageVm(
                    stageIndex: stageVm.StageIndex,
                    stageVmStyle: stageVmStyle,
                    order: stageVm.Order,
                    keyPairVms: stageVm.KeyPairVms,
                    sortableVm: stageVm.SortableVm
                 );
        }

    }
}
