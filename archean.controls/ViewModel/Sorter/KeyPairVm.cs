
using System.Windows.Media;

namespace archean.controls.ViewModel.Sorter
{
    public class KeyPairVm
    {
        public KeyPairVm(Brush brush, int orderInStage, int hiKey, int lowKey)
        {
            Brush = brush;
            StageSection = orderInStage;
            HiKey = hiKey;
            LowKey = lowKey;
        }

        public Brush Brush { get; }
        public int StageSection { get; }
        public int HiKey { get; }
        public int LowKey { get; }
    }
}
