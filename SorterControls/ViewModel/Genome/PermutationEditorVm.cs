using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using FirstFloor.ModernUI.Presentation;
using Sorting.KeyPairs;

namespace SorterControls.ViewModel.Genome
{
    public class PermutationEditorVm : NotifyPropertyChanged, ISorterGenomeEditorVm
    {
        public PermutationEditorVm(int keyCount)
        {
            _keyCount = keyCount;
        }

        public GenomeEditorType GenomeEditorType
        {
            get { return GenomeEditorType.Permutation; }
        }

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        private readonly Subject<ISorterGenomeEditorVm> _onGenomeChanged
            = new Subject<ISorterGenomeEditorVm>();

        public IObservable<ISorterGenomeEditorVm> OnGenomeChanged
        {
            get { return _onGenomeChanged; }
        }

        public IReadOnlyList<IKeyPair> KeyPairs
        {
            get { return null; }
        }

        public string Serialized
        {
            get { return String.Empty; }
        }
    }
}
