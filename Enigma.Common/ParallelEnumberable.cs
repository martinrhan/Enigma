using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Enigma.Common {
    public class PrallelForeachCollector<T> : IEnumerable<T> {
        public PrallelForeachCollector(Action<Action<T>> parallellyForeachAction) {
            Task.Run(() => {
                parallellyForeachAction(item => {
                    enumerator.Put(item);
                });
                enumerator.Finish();
            });
        }
        private PrallelForeachCollectorEnumerator<T> enumerator = new();

        public IEnumerator<T> GetEnumerator() => enumerator;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class PrallelForeachCollectorEnumerator<T> : IEnumerator<T> {
        internal void Put(T item) {
            bag.Add(item);
            autoResetEvent.Set();
        }
        internal void Finish() {
            isFinished = true;
            autoResetEvent.Set();
            //Console.WriteLine("count =" + bag.Count);
        }
        private readonly ConcurrentBag<T> bag = new ConcurrentBag<T>();
        private readonly AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        private bool isFinished = false;

        public T Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose() { }

        public bool MoveNext() {
            T t;
            bool confirmingFinished = false;
            start:
            if (bag.TryTake(out t)) {
                Current = t;
                return true;
            } else if (isFinished) {
                if (confirmingFinished) {
                    return false;
                } else {
                    confirmingFinished = true;
                    goto start;
                }
            } else {
                autoResetEvent.WaitOne();
                goto start;
            }
        }

        public void Reset() {
        }
    }
}
