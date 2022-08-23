using Enigma.Game;
using Enigma.GameWPF.Visual.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Enigma.GameWPF.Visual {
    public abstract class ViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool IsEmpty { get; set; }
    }

    public abstract class ManualNotifyChangedViewModel : ViewModel {
        public abstract void NotifyChanged();
    }

    public abstract class CollectionViewModel<TViewModel, TModel> : ManualNotifyChangedViewModel, IReadOnlyList<TViewModel>, INotifyCollectionChanged where TViewModel : ManualNotifyChangedViewModel, new() {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        protected void NotifyCollectionChanged(NotifyCollectionChangedEventArgs e) {
            CollectionChanged?.Invoke(this, e);
        }

        public IEnumerator<TViewModel> GetEnumerator() => viewModelList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => viewModelList.Count;
        public TViewModel this[int index] => viewModelList[index];

        private readonly List<TViewModel> viewModelList = new();
        private int nonEmptyCount = 0;
        protected void UpdateDataFromModel_Protected(IEnumerable<TModel> models, Action<TViewModel, TModel> action) {
            int i = 0;
            foreach (TModel model in models) {
                if (viewModelList.Count == i) {
                    TViewModel viewModel = new TViewModel();
                    viewModelList.Add(viewModel);
                    addRecord.Add(viewModel);
                }
                viewModelList[i].IsEmpty = false;
                action(viewModelList[i], model);
                i++;
            }
            for (int j = i; j < nonEmptyCount; j++) {
                viewModelList[j].IsEmpty = true;
            }
            nonEmptyCount = i;
        }

        private List<TViewModel> addRecord = new();
        private int lastNotifyNonEmptyCount = 0;

        public override void NotifyChanged() {
            foreach (TViewModel viewModel in addRecord) {
                NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, viewModel));
            }
            addRecord.Clear();
            int greaterInt = Math.Max(nonEmptyCount, lastNotifyNonEmptyCount);
            for (int i = 0; i < greaterInt; i++) {
                viewModelList[i].NotifyChanged();
            }
            lastNotifyNonEmptyCount = nonEmptyCount;
        }

    }

}
