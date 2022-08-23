using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Enigma.Spacial;
using Enigma.Common.Math;

namespace Enigma.PhysicsEngine {
    public class PhysicsWorld<T> : ICollection<T> where T : PhysicsBody {
        public double Width => spacialCollection.Width;
        public double Height => spacialCollection.Height;

        public PhysicsWorld(SpacialCollection<T> spacialCollection) {
            this.spacialCollection = spacialCollection;
        }
        public void Update(Func<T,T,bool> collidabilityPredicate) {
            spacialCollection.ParallelForEach(t => {
                t.SetAABBLowerBound(GetClampedPosition(t.AABB.LowerBound));
            });
            spacialCollection.Update();
            spacialCollection.PrepareCollisionData(collidabilityPredicate);
            AdjustPositions();
            spacialCollection.ParallelForEach(t => {
                t.SetAABBLowerBound(GetClampedPosition(t.AABB.LowerBound));
            });
            spacialCollection.Update();
        }
        public Vector2 GetClampedPosition(in Vector2 position) {
            (double x, double y) = position;
            if (x < 0) x += Width;
            else if (Width < x) x -= Width;
            if (y < 0) y += Height;
            else if (Height < y) y -= Height;
            return new Vector2(x, y);
        }

        public IEnumerable<T> AABBHitTest(in AABB aABB, Predicate<T> hittablityPredicate) {
            return spacialCollection.AABBHitTest(aABB, hittablityPredicate);
        }
        public IEnumerable<T> CircleHitTest(in Vector2 center, double radius, Predicate<T> hittablityPredicate) {
            CircleShapedObject circle = new(center, radius);
            return spacialCollection.HitTest(circle, hittablityPredicate);
        }
        public IEnumerable<T> RectangleHitTest(IRectangleShapedObject rectangleShaped, Predicate<T> hittablityPredicate) {
            return spacialCollection.HitTest(rectangleShaped, hittablityPredicate);
        }

        public void ParallelForEach(Action<T> action) => spacialCollection.ParallelForEach(action);

        private void AdjustPositions() {
            foreach (var pair in spacialCollection.CollisionPairs) {
                if (pair.Item1.AllowPositionAdjustment && pair.Item2.AllowPositionAdjustment) {
                    CollisionResolver.AdjustPosition(pair.Item1, pair.Item2, spacialCollection);
                }
            }
        }

        public IEnumerable<ValueTuple<T, T>> CollisionPairs => spacialCollection.CollisionPairs;
        public IEnumerable<T> QueryCollisionDictionary(T item) => spacialCollection.QueryCollisionDictionary(item);
        public Vector2 GetShortestDisplacement(Vector2 from, Vector2 to) => spacialCollection.GetShortestDisplacement(from, to);

        #region ICollection
        private readonly SpacialCollection<T> spacialCollection;
        public int Count => spacialCollection.Count;
        public bool IsReadOnly => spacialCollection.IsReadOnly;
        public void Add(T item) => spacialCollection.Add(item);
        public void Clear() => spacialCollection.Clear();
        public bool Contains(T item) => spacialCollection.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) {
            spacialCollection.CopyTo(array, arrayIndex);
        }
        public IEnumerator<T> GetEnumerator() => spacialCollection.GetEnumerator();
        public bool Remove(T item) => spacialCollection.Remove(item);
        IEnumerator IEnumerable.GetEnumerator() => spacialCollection.GetEnumerator();
        #endregion
    }
}
