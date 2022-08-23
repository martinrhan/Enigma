using Enigma.Common.Math;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Enigma.Spacial {
    public abstract class SpacialCollection<T> :  Spacial, ICollection<T> where T : IShapedObject {
        public abstract void Update();
        public abstract void ParallelForEach(Action<T> action);

        public abstract void PrepareCollisionData(Func<T, T, bool> collidabilityPredicate);

        public abstract IEnumerable<T> AABBHitTest(AABB aABB, Predicate<T> hittablityPredicate);
        public abstract IEnumerable<T> HitTest(IShapedObject testerShaped, Predicate<T> hittablityPredicate);

        public int Count { get; protected set; }
        public bool IsReadOnly { get; }

        private ConcurrentBag<(T, T)> collisionPairBag { get; } = new ConcurrentBag<(T, T)>();
        private ConcurrentDictionary<T, ConcurrentBag<T>> collisionDictionary { get; } = new ConcurrentDictionary<T, ConcurrentBag<T>>();
        protected void ClearCollisionData() {
            collisionPairBag.Clear();
            collisionDictionary.Clear();
        }
        protected void AddCollisionData(T itemA, T itemB) {
            collisionPairBag.Add((itemA, itemB));
            if (!collisionDictionary.TryAdd(itemA, new ConcurrentBag<T> { itemB })) {
                collisionDictionary[itemA].Add(itemB);
            }
            if (!collisionDictionary.TryAdd(itemB, new ConcurrentBag<T> { itemA })) {
                collisionDictionary[itemB].Add(itemA);
            }
        }
        public IEnumerable<ValueTuple<T, T>> CollisionPairs => collisionPairBag;
        public IEnumerable<T> QueryCollisionDictionary(T item) {
            ConcurrentBag<T> result;
            if (collisionDictionary.TryGetValue(item, out result)) {
                return result;
            } else {
                return Enumerable.Empty<T>();
            }
        }

        public abstract void Add(T item);
        public abstract void Clear();
        public abstract bool Contains(T item);
        public abstract void CopyTo(T[] array, int arrayIndex);
        public abstract bool Remove(T item);
        public abstract IEnumerator<T> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public abstract class Spacial {
        public double Width { get; protected init; }
        public double Height { get; protected init; }
        public Vector2 GetShortestDisplacement(in Vector2 from, in Vector2 to) {
            double halfWidth = Width / 2;
            double differenceX = to.X - from.X;
            if (differenceX > halfWidth) differenceX -= Width;
            else if (differenceX < -halfWidth) differenceX += Width;
            double halfHeight = Height / 2;
            double differenceY = to.Y - from.Y;
            if (differenceY > halfHeight) differenceY -= Height;
            else if (differenceY < -halfHeight) differenceY += Height;
            return new Vector2(differenceX, differenceY);
        }
    }
}
