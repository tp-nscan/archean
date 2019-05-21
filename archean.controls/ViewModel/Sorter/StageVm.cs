using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;

namespace archean.controls.ViewModel.Sorter
{
    public class StageVm
    {
        public StageVm(
            StageVmStep stageVmStep,
            Brush keyLineBrush, 
            int keyCount,
            double switchLineWidth,
            double switchSpacing,
            double lineThickness,
            double keyLineSpacing,
            double hPadding,
            double vPadding,
            Brush backgroundBrush, 
            IEnumerable<KeyPairVm> keyPairVms,
            SortableVm[] sortableVms)
        {
            StageVmStep = stageVmStep;
            KeyLineBrush = keyLineBrush;
            KeyCount = keyCount;
            SwitchLineWidth = switchLineWidth;
            SwitchSpacing = switchSpacing;
            KeyLineThickness = lineThickness;
            KeyLineSpacing = keyLineSpacing;
            HPadding = hPadding;
            VPadding = vPadding;
            BackgroundBrush = backgroundBrush;
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

        readonly List<KeyPairVm> _keyPairVms;
        public IEnumerable<KeyPairVm> KeyPairVms { get { return _keyPairVms; } }

        public int SectionCount { get; }
        public double VmHeight { get; }
        public double VmWidth { get; }
    }


    public class StageVmStyles
    {
        public Brush LineBrush { get; set; }
        public Brush SwitchBrush { get; set; }
        public double SwitchWidth { get; set; }
        public double SwitchSpacing { get; set; }
        public double LineThickness { get; set; }
        public double LineSpacing { get; set; }
        public double HPadding { get; set; }
        public double VPadding { get; set; }
        public Brush BackgroundBrush { get; set; }

        static StageVmStyles _standard;
        public static StageVmStyles Standard
        {
            get
            {
                return _standard ?? (_standard = new StageVmStyles
                {
                    LineBrush = Brushes.Blue,
                    SwitchBrush = Brushes.Black,
                    SwitchWidth = 1.0,
                    SwitchSpacing = 2.0,
                    LineThickness = 1.0,
                    LineSpacing = 3.0,
                    HPadding = 3.0,
                    VPadding = 1.0,
                    BackgroundBrush = Brushes.Black
                });
            }
        }
    }


    public static class StageVmProcs
    {
        public static StageVm Make(StageVmStyles stageVmStyles, core.Sorting.Stage stage)
        {
            return null;
        }

        public static StageVm ToNextStep(this StageVm stageVm)
        {
            switch (stageVm.StageVmStep)
            {
                case StageVmStep.Left:
                    return new StageVm(
                            stageVmStep: StageVmStep.Presort,
                            keyLineBrush: stageVm.KeyLineBrush,
                            keyCount: stageVm.KeyCount,
                            switchLineWidth: stageVm.SwitchLineWidth,
                            switchSpacing: stageVm.SwitchSpacing,
                            lineThickness: stageVm.KeyLineThickness,
                            keyLineSpacing: stageVm.KeyLineSpacing,
                            hPadding: stageVm.HPadding,
                            vPadding: stageVm.VPadding,
                            backgroundBrush: stageVm.BackgroundBrush,
                            keyPairVms: stageVm.KeyPairVms,
                            sortableVms: stageVm.SortableVms.ToPreSortStep(stageVm.KeyPairVms.ToArray())
                         );
                case StageVmStep.Presort:

                    SortableVmExt.SortTheSortableVms(
                        stageVm.SortableVms.ToArray(), 
                        stageVm.KeyPairVms.ToArray());

                    return new StageVm(
                            stageVmStep: StageVmStep.PostSort,
                            keyLineBrush: stageVm.KeyLineBrush,
                            keyCount: stageVm.KeyCount,
                            switchLineWidth: stageVm.SwitchLineWidth,
                            switchSpacing: stageVm.SwitchSpacing,
                            lineThickness: stageVm.KeyLineThickness,
                            keyLineSpacing: stageVm.KeyLineSpacing,
                            hPadding: stageVm.HPadding,
                            vPadding: stageVm.VPadding,
                            backgroundBrush: stageVm.BackgroundBrush,
                            keyPairVms: stageVm.KeyPairVms,
                            sortableVms: stageVm.SortableVms.ToPostSortStep(stageVm.KeyPairVms.ToArray())
                         );
                case StageVmStep.PostSort:
                    return new StageVm(
                            stageVmStep: StageVmStep.Right,
                            keyLineBrush: stageVm.KeyLineBrush,
                            keyCount: stageVm.KeyCount,
                            switchLineWidth: stageVm.SwitchLineWidth,
                            switchSpacing: stageVm.SwitchSpacing,
                            lineThickness: stageVm.KeyLineThickness,
                            keyLineSpacing: stageVm.KeyLineSpacing,
                            hPadding: stageVm.HPadding,
                            vPadding: stageVm.VPadding,
                            backgroundBrush: stageVm.BackgroundBrush,
                            keyPairVms: stageVm.KeyPairVms,
                            sortableVms: stageVm.SortableVms.ToRightStep()
                         );
                case StageVmStep.Right:
                    return new StageVm(
                            stageVmStep: StageVmStep.Right,
                            keyLineBrush: stageVm.KeyLineBrush,
                            keyCount: stageVm.KeyCount,
                            switchLineWidth: stageVm.SwitchLineWidth,
                            switchSpacing: stageVm.SwitchSpacing,
                            lineThickness: stageVm.KeyLineThickness,
                            keyLineSpacing: stageVm.KeyLineSpacing,
                            hPadding: stageVm.HPadding,
                            vPadding: stageVm.VPadding,
                            backgroundBrush: stageVm.BackgroundBrush,
                            keyPairVms: stageVm.KeyPairVms,
                            sortableVms: stageVm.SortableVms.ToRightStep()
                         );
                default:
                    throw new System.Exception($"{stageVm.StageVmStep} not handled");
            }

        }



    }
}
