using System;
using System.ComponentModel;
using System.Reactive.Subjects;
using FirstFloor.ModernUI.Presentation;

namespace SorterControls.ViewModel.Genome
{
    public class SwitchEditorVm : NotifyPropertyChanged, 
            IDataErrorInfo
    {
        public SwitchEditorVm(int keyCount)
        {
            _keyCount = keyCount;
        }

        private readonly Subject<SwitchEditorVm> _onSwitchEditVmChanged 
            = new Subject<SwitchEditorVm>();
        public IObservable<SwitchEditorVm> OnSwitchEditVmChanged
        {
            get { return _onSwitchEditVmChanged; }
        }

        public bool NotifyEnabled { get; set; }

        void SendUpdates()
        {
            if (
                    NotifyEnabled && 
                    LowKey.IsAValidLowKey(HiKey, KeyCount) && 
                    HiKey.IsAValidHiKey(LowKey, KeyCount)
               )
            {
                _onSwitchEditVmChanged.OnNext(this);
            }
        }

        private int _sorterPosition;
        public int SorterPosition
        {
            get { return _sorterPosition; }
            set
            {
                _sorterPosition = value;
                OnPropertyChanged("SorterPosition");
                SendUpdates();
            }
        }

        private int? _lowKey;
        public int? LowKey
        {
            get { return _lowKey; }
            set
            {
                _lowKey = value;
                OnPropertyChanged("LowKey");
                SendUpdates();
            }
        }

        private int? _hiKey;
        public int? HiKey
        {
            get { return _hiKey; }
            set
            {
                _hiKey = value;
                OnPropertyChanged("HiKey");
                SendUpdates();
            }
        }

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "LowKey")
                {
                    return (LowKey.IsAValidLowKey(HiKey, KeyCount)) ? null : " ";
                }
                if (columnName == "HiKey")
                {
                    return (HiKey.IsAValidHiKey(LowKey, KeyCount)) ? null : " ";
                }
                return null;
            }
        }

        public string Error
        {
            get { return null; }
        }
    }
}
