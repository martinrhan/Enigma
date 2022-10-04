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
            protected set => array[index] = value;
        }
        public int Count => array.Length;

        protected void IncreaseCount(int amount) {
            if (amount < 0) throw new ArgumentOutOfRangeException();
            Array.Resize(ref array, array.Length + amount);
        }

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)array).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected void InternalExchange(int indexA, int indexB) {
            var temp = array[indexB];
            array[indexB] = this[indexA];
            this[indexA] = temp;
        }
        protected void ExternalExchange(int indexFrom, int indexTo, SlottedCollection<T> targetCollection) {
            var temp = targetCollection.array[indexTo];
            targetCollection.array[indexTo] = this[indexFrom];
            this[indexFrom] = temp;
        }
        protected void ExternalExchange<TTarget>(int indexFrom, int indexTo, SlottedCollection<TTarget> targetCollection, Func<T, TTarget> conversionFunction_Out, Func<TTarget, T> conversionFunction_In) where TTarget : class {
            var temp = targetCollection.array[indexTo];
            targetCollection.array[indexTo] = this[indexFrom] == null ? null : conversionFunction_Out(this[indexFrom]);
            this[indexFrom] = temp == null ? null : conversionFunction_In(temp);
        }
        protected void PlaceAtFirstEmptySlot(T item) {
            for (int i = 0; i < array.Length; i++) {
                if (array[i] == null) {
                    array[i] = item;
                    return;
                }
            }
            throw new InvalidOperationException("There is no any empty slot.");
        }
        protected void Clear() {
            for (int i = 0; i<array.Length; i++) {
                array[i] = null;
            }
        }
    }
}
