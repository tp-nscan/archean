using System.Collections.Generic;
using System.Windows.Media;
using FirstFloor.ModernUI.Presentation;
using Sorting.KeyPairs;

namespace SorterControls.ViewModel.SorterOld
{
    public class SwitchGraphicVm : NotifyPropertyChanged
    {
        public SwitchGraphicVm
        (
            IKeyPair keyPair, 
            int keyCount, 
            List<Brush> lineBrushes, 
            int width
        )
        {
            _keyPair = keyPair;
            _keyCount = keyCount;
            _lineBrushes = lineBrushes;
            _width = width;
        }

        private readonly IKeyPair _keyPair;
        public IKeyPair KeyPair
        {
            get { return _keyPair; }
        }

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        private Brush _switchBrush;
        public Brush SwitchBrush
        {
            get { return _switchBrush; }
            set
            {
                _switchBrush = value;
                OnPropertyChanged("SwitchBrush");
            }
        }

        private readonly List<Brush> _lineBrushes;
        public List<Brush> LineBrushes
        {
            get { return _lineBrushes; }
        }

        private readonly int _width;
        public int Width
        {
            get { return _width; }
        }
    }
}
