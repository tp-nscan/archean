using System;
using System.Collections.Generic;
using Sorting.KeyPairs;

namespace SorterControls.ViewModel.Genome
{
    public interface ISorterGenomeEditorVm
    {
        GenomeEditorType GenomeEditorType { get; }
        int KeyCount { get; }
        IObservable<ISorterGenomeEditorVm> OnGenomeChanged { get; }
        IReadOnlyList<IKeyPair> KeyPairs { get; }
        string Serialized { get; }
    }
}