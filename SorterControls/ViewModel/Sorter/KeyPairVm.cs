using System.Windows.Media;
using Sorting.KeyPairs;

namespace SorterControls.ViewModel.Sorter
{
    public class KeyPairVm
    {
        public KeyPairVm(IKeyPair keyPair, Brush switchBrush, int position)
        {
            _switchBrush = switchBrush;
            _position = position;
            _keyPair = keyPair;
        }

        private readonly IKeyPair _keyPair;
        public IKeyPair KeyPair
        {
            get { return _keyPair; }
        }


        private readonly Brush _switchBrush;
        public Brush SwitchBrush
        {
            get { return _switchBrush; }
        }

        private readonly int _position;
        public int Position
        {
            get { return _position; }
        }
    }
}
