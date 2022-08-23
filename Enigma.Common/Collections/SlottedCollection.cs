using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Enigma.Common.Collections {
    public abstract class SlottedCollection<T> : IReadOnlyList<T> where T : class {
        protected SlottedCollection(int count) { 
            array = new T[count];
        }
        protected SlottedCollection(IEnumerable<T> items) {
            array = items.ToArray();
        }
        private T[] array;
        public T this[int index] {
            get => array[index];
            private set => array[index] = value;
        }
        public int Count => array.Length;

        internal void IncreaseCount(int amount) {
            Array.Resize(ref array, array.Length + amount);
        }

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)array).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected void Overwrite(int index, T item) {
            this[index] = item;
        }
        protected void InternalExchange(int indexFrom, int indexTo) {
            var temp = array[indexTo];
            array[indexTo] = this[indexFrom];
            this[indexFrom] = temp;
        }
        protected void ExternalExchange(int indexFrom, int indexTo, SlottedCollection<T> targetCollection) {
            var temp = targetCollection.array[indexTo];
            targetCollection.array[indexTo] = this[indexFrom];
            this[indexFrom] = temp;
        }
        protected void ExternalExchange<TTarget>(int indexFrom, int indexTo, SlottedCollection<TTarget> targetCollection, Func<T, TTarget> conversionFunction_From, Func<TTarget, T> conversionFunction_To) where TTarget : class {
            var temp = targetCollection.array[indexTo];
            targetCollection.array[indexTo] = this[indexFrom] == null ? null : conversionFunction_From(this[indexFrom]);
            this[indexFrom] = temp == null ? null : conversionFunction_To(temp);
        }
        protected void PlaceAtFirstEmptySlot(T item) {
            for (int i = 0; i < array.Length; i++) {
                if (array[i] == null) {
                    array[i] = item;
                }
            }
            throw new InvalidOperationException("There is no any empty slot.");
        }

    }
}
