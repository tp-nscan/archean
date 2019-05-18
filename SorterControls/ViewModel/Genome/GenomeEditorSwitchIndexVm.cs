using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;
using Sorting.KeyPairs;

namespace SorterControls.ViewModel.Genome
{
    public class GenomeEditorSwitchIndexVm : NotifyPropertyChanged, ISorterGenomeEditorVm
    {
        public GenomeEditorSwitchIndexVm(int keyCount, IReadOnlyList<IKeyPair> keyPairs)
        {
            _keyCount = keyCount;

            for (var i = 0; i < keyPairs.Count; i++)
            {
                var switchEditVm = new SwitchEditorVm(KeyCount)
                {
                    SorterPosition = i,
                    LowKey = keyPairs[i].LowKey,
                    HiKey = keyPairs[i].HiKey,
                    NotifyEnabled = true
                };

                _updateSuscriptions[switchEditVm]
                        = switchEditVm.OnSwitchEditVmChanged.Subscribe(vm => Serialize());

                SwitchEditVms.Add(switchEditVm);
            }

            Serialize();
        }

        void Serialize()
        {
            Serialized = SwitchEditVms.Select(  
                vm=> KeyPairRepository.KeyPairFromKeys(vm.LowKey.Value, vm.HiKey.Value))
                .ToSerialized();
        }

        public void AddSwitchEditVm(SwitchEditorVm switchEditVm, int sorterPosition)
        {
            _updateSuscriptions[switchEditVm] 
                = switchEditVm.OnSwitchEditVmChanged.Subscribe(vm => Serialize());

            SwitchEditVms.Insert(sorterPosition, switchEditVm);
            SetSwitchUpdates(false);
            ReIndex();
            SetSwitchUpdates(true);
            //Serialize();
        }

        public void RemoveSwitchEditVm(SwitchEditorVm switchEditVm)
        {
            _updateSuscriptions[switchEditVm].Dispose();
            _updateSuscriptions.Remove(switchEditVm);
 
            SwitchEditVms.Remove(switchEditVm);
            SetSwitchUpdates(false);
            ReIndex();
            SetSwitchUpdates(true);
            Serialize();
        }

        void SetSwitchUpdates(bool isEnabled)
        {
            foreach (var switchEditVm in SwitchEditVms)
            {
                switchEditVm.NotifyEnabled = isEnabled;
            }
        }

        readonly Dictionary<SwitchEditorVm, IDisposable> _updateSuscriptions 
            = new Dictionary<SwitchEditorVm, IDisposable>(); 

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        private ObservableCollection<SwitchEditorVm> _switchEditVms 
            = new ObservableCollection<SwitchEditorVm>();

        public ObservableCollection<SwitchEditorVm> SwitchEditVms
        {
            get { return _switchEditVms; }
            set { _switchEditVms = value; }
        }

        private string _serialized;

        public IReadOnlyList<IKeyPair> KeyPairs
        {
            get { 
                return SwitchEditVms.Select(
                vm=>KeyPairRepository.KeyPairFromKeys(vm.LowKey.Value, vm.HiKey.Value))
                                     .ToList(); 
            }
        }

        public string Serialized
        {
            get { return _serialized; }
            set
            {
                _serialized = value;
                OnPropertyChanged("Serialized");
                _onGenomeChanged.OnNext(this);
            }
        }

        bool Deserialize(string serialized)
        {
            return true;
        }


        #region InsertCommand

        RelayCommand _insertCommand;
        public ICommand InsertCommand
        {
            get
            {
                return _insertCommand ?? ( _insertCommand
                    = new RelayCommand
                        (
                            OnInsertCommand,
                            CanInsertCommand
                        ));
            }
        }

        protected void OnInsertCommand(object param)
        {
            var vm = param as SwitchEditorVm;

            if (vm == null) { return;}

            AddSwitchEditVm( new SwitchEditorVm(KeyCount), vm.SorterPosition);
        }

        void ReIndex()
        {
            var index = 0;
            foreach (var switchEditVm in SwitchEditVms)
            {
                switchEditVm.SorterPosition = index++;
            }
        }

        bool CanInsertCommand(object param)
        {
            return (param != null);
        }

        #endregion // ResetCommand

        #region DeleteCommand

        RelayCommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand
                    = new RelayCommand
                        (
                            OnDeleteCommand,
                            CanDeleteCommand
                        ));
            }
        }

        protected void OnDeleteCommand(object param)
        {
            var vm = param as SwitchEditorVm;

            if (vm == null) { return; }

            RemoveSwitchEditVm(vm);
        }

        bool CanDeleteCommand(object param)
        {
            return (param != null);
        }

        #endregion // DeleteCommand

        public GenomeEditorType GenomeEditorType
        {
            get { return GenomeEditorType.SwitchIndex; }
        }

        private readonly Subject<ISorterGenomeEditorVm> _onGenomeChanged
            = new Subject<ISorterGenomeEditorVm>();

        public IObservable<ISorterGenomeEditorVm> OnGenomeChanged
        {
            get { return _onGenomeChanged; }
        }
    }
}
