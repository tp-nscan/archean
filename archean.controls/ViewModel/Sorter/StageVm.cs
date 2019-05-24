using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;

namespace archean.controls.ViewModel.Sorter
{
    public class StageVm
    {
        public StageVm(
            StageVmStep stageVmStep,
            StageVmStyle stageVmStyle,
            int keyCount,
            IEnumerable<KeyPairVm> keyPairVms,
            SortableVm[] sortableVms)
        {
            StageVmStep = stageVmStep;
            StageVmStyle = stageVmStyle;
            KeyLineBrush = StageVmStyle.KeyLineBrush;
            KeyCount = keyCount;
            SwitchLineWidth = StageVmStyle.SwitchLineWidth;
            SwitchSpacing = StageVmStyle.SwitchSpacing;
            KeyLineThickness = StageVmStyle.KeyLineThickness;
            KeyLineSpacing = StageVmStyle.KeyLineSpacing;
            HPadding = StageVmStyle.HPadding;
            VPadding = StageVmStyle.VPadding;
            BackgroundBrush = StageVmStyle.BackgroundBrush;
            _keyPairVms = keyPairVms.ToList();
            SortableVms = sortableVms;
            SectionCount = _keyPairVms.Max(vm => vm.StageSection) + 1;
            VmWidth = 2 * HPadding + (SwitchLineWidth + SwitchSpacing) * SectionCount;
            VmHeight = 2 * VPadding + (KeyLineThickness + KeyLineSpacing) * KeyCount + KeyLineSpacing;
        }

        public StageVmStep StageVmStep { get; }
        public Brush KeyLineBrush { get; }
        public int KeyCount { get; }
        public double SwitchLineWidth { get; }
        public double SwitchSpacing { get; }
        public double KeyLineThickness { get; }
        public double KeyLineSpacing { get; }
        public double HPadding { get; }
        public double VPadding { get; }
        public Brush BackgroundBrush { get; }
        public SortableVm[] SortableVms { get; }
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
        public double SwitchSpacing { get; set; }
        public double KeyLineThickness { get; set; }
        public double KeyLineSpacing { get; set; }
        public double HPadding { get; set; }
        public double VPadding { get; set; }
        public Brush BackgroundBrush { get; set; }

        static StageVmStyle _standardE;
        static StageVmStyle _standard0;
        public static StageVmStyle StandardE
        {
            get
            {
                return _standardE ?? (_standardE = new StageVmStyle
                {
                    KeyLineBrush = Brushes.Blue,
                    SwitchBrushNotUsed = Brushes.Black,
                    SwitchBrushInUse = Brushes.LightBlue,
                    SwitchBrushWasUsed = Brushes.DarkBlue,
                    SwitchLineWidth = 1.0,
                    SwitchSpacing = 2.0,
                    KeyLineThickness = 1.0,
                    KeyLineSpacing = 3.0,
                    HPadding = 3.0,
                    VPadding = 1.0,
                    BackgroundBrush = Brushes.White
                });
            }
        }

        public static StageVmStyle Standard0
        {
            get
            {
                return _standard0 ?? (_standard0 = new StageVmStyle
                {
                    KeyLineBrush = Brushes.Blue,
                    SwitchBrushNotUsed = Brushes.Black,
                    SwitchBrushInUse = Brushes.LightBlue,
                    SwitchBrushWasUsed = Brushes.DarkBlue,
                    SwitchLineWidth = 1.0,
                    SwitchSpacing = 2.0,
                    KeyLineThickness = 1.0,
                    KeyLineSpacing = 3.0,
                    HPadding = 3.0,
                    VPadding = 1.0,
                    BackgroundBrush = Brushes.Lavender
                });
            }
        }

    }


    public static class StageVmProcs
    {
        public static StageVm StageToStageVm(this core.Sorting.Stage stage,
                                                  StageVmStyle stageVmStyle, 
                                                  int order)
        {
            var swLayout = core.Sorting.StageLayout.LayoutSwitches(order, stage.switches);
            var kpVms = swLayout.ToKeyPairVms(stageVmStyle);
            return new StageVm(
                    stageVmStep: StageVmStep.Left,
                    stageVmStyle: stageVmStyle,
                    keyCount: order,
                    keyPairVms: kpVms,
                    sortableVms: new SortableVm[0]
                 );
        }


        public static StageVm SwitchBlocksToStageVm(
                            this core.Sorting.Switch[][] switchBlocks,
                            StageVmStyle stageVmStyle, 
                            int order,
                            SortableVm[] sortableVms)
        {
            var kpVms = switchBlocks.ToKeyPairVms(stageVmStyle);
            return new StageVm(
                    stageVmStep: StageVmStep.Left,
                    stageVmStyle: stageVmStyle,
                    keyCount: order,
                    keyPairVms: kpVms,
                    sortableVms: sortableVms
                 );
        }


        public static SortableVm[] ScrambledSortableVms(int keyCount)
        {
            return
                SortableVmExt.StartingPositionInts(keyCount, ScramblePos(keyCount));
        }


        public static int[] ScramblePos(int order)
        {
            return
                    core.Combinatorics.FisherYatesShuffle(
                    new System.Random(),
                    Enumerable.Range(0, order).ToArray()
                ).ToArray();
        }

        public static StageVm ToNextStep(this StageVm stageVm)
        {
            switch (stageVm.StageVmStep)
            {
                case StageVmStep.Left:
                    return new StageVm(
                            stageVmStep: StageVmStep.Presort,
                            stageVmStyle: StageVmStyle.StandardE,
                            keyCount: stageVm.KeyCount,
                            keyPairVms: stageVm.KeyPairVms,
                            sortableVms: stageVm.SortableVms.ToPreSortStep(stageVm.KeyPairVms.ToArray())
                         );
                case StageVmStep.Presort:

                    SortableVmExt.SortTheSortableVms(
                        stageVm.SortableVms.ToArray(), 
                        stageVm.KeyPairVms.ToArray());

                    return new StageVm(
                            stageVmStep: StageVmStep.PostSort,
                            stageVmStyle: StageVmStyle.StandardE,
                            keyCount: stageVm.KeyCount,
                            keyPairVms: stageVm.KeyPairVms,
                            sortableVms: stageVm.SortableVms.ToPostSortStep(stageVm.KeyPairVms.ToArray())
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
                            stageVmStyle: StageVmStyle.StandardE,
                            keyCount: stageVm.KeyCount,
                            keyPairVms: stageVm.KeyPairVms,
                            sortableVms: stageVm.SortableVms.ToRightStep()
                         );
                case StageVmStep.Right:
                    return new StageVm(
                            stageVmStep: StageVmStep.Right,
                            stageVmStyle: StageVmStyle.StandardE,
                            keyCount: stageVm.KeyCount,
                            keyPairVms: stageVm.KeyPairVms,
                            sortableVms: stageVm.SortableVms.ToMissingStep()
                         );
                default:
                    throw new System.Exception($"{stageVm.StageVmStep} not handled");
            }

        }



    }
}
