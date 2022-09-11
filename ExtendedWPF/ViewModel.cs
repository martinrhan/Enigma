﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ExtendedWPF {
    public abstract class ViewModel : INotifyPropertyChanged{
        public event PropertyChangedEventHandler PropertyChanged;
        protected internal void NotifyPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool IsEmpty { get; set; }

    }
}
