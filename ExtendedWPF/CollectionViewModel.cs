using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ExtendedWPF {
    public abstract class CollectionViewModel<TViewModel, TModel> : ViewModel, IReadOnlyList<TViewModel>, INotifyCollectionChanged where TViewModel : ViewModel, new() {
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
        private int lastNotifyNonEmptyCount = 0;
        protected void UpdateDataFromModel_Protected(IEnumerable<TModel> models, Action<TViewModel, TModel> action) {
            int i = 0;
            addRecord = new List<TViewModel>();
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
            foreach (TViewModel viewModel in addRecord) {
                NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, viewModel));
            }
            int greaterInt = Math.Max(nonEmptyCount, lastNotifyNonEmptyCount);
            for (i = 0; i < greaterInt; i++) {
                viewModelList[i].NotifyPropertyChanged(null);
            }
            lastNotifyNonEmptyCount = nonEmptyCount;
        }

        private List<TViewModel> addRecord = new();
    }
}
