using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enigma.Spacial {
    public sealed class Quadtree<T> : SpacialCollection<T> where T : IShapedObject {
        public Quadtree(int levelCount, double width, double height) {
            if (levelCount == 0 || levelCount > 30) throw new ArgumentException();
            levelArray = new QuadtreeLevel<T>[levelCount];
            List<QuadtreeCell<T>> allCellList = new List<QuadtreeCell<T>>();
            overLevelCell = new QuadtreeCell<T>(-1, 0, 0, allCellList);
            for (int i = 0; i < levelCount; i++) {
                levelArray[i] = new QuadtreeLevel<T>(i, width, height, allCellList);
            }
            Width = width;
            Height = height;
            allCellArray = allCellList.ToArray();
        }

        internal readonly QuadtreeCell<T>[] allCellArray;
        private readonly QuadtreeLevel<T>[] levelArray;
        private readonly QuadtreeCell<T> overLevelCell;

        private readonly record struct FindPlaceResult(int level, int cellIndexX, int cellIndexY);
        private readonly record struct IntegerAABB(int lowerBoundX, int lowerBoundY, int upperBoundX, int upperBoundY);
        private QuadtreeCell<T> GetCell(in AABB aABB) {
            for (int level = levelArray.Length - 1; level >= 0; level--) {
                int lowerBoundCellIndexX = (int)(aABB.LowerBound.X / levelArray[level].cellWidth);
                int lowerBoundCellIndexY = (int)(aABB.LowerBound.Y / levelArray[level].cellHeight);
                int upperBoundCellIndexX = (int)(aABB.UpperBound.X / levelArray[level].cellWidth);
                int upperBoundCellIndexY = (int)(aABB.UpperBound.Y / levelArray[level].cellHeight);
                if (lowerBoundCellIndexX == upperBoundCellIndexX && lowerBoundCellIndexY == upperBoundCellIndexY) {
                    return levelArray[level].cellArray[lowerBoundCellIndexX, lowerBoundCellIndexY];
                }
            }
            return overLevelCell;
        }

        public override void Add(T item) {
            GetCell(item.AABB).list.Add(item);
            Count++;
        }
        public override bool Remove(T item) {
            if (GetCell(item.AABB).list.Remove(item)) {
                Count--;
                return true;
            } else return false;
        }
        public override void Clear() {
            overLevelCell.list.Clear();
            foreach (QuadtreeLevel<T> level in levelArray) level.Clear();
            Count = 0;
        }
        public override bool Contains(T item) {
            return GetCell(item.AABB).list.Contains(item);
        }
        public override void CopyTo(T[] array, int arrayIndex) {
            foreach (T t in this) {
                array[arrayIndex] = t;
                arrayIndex++;
            }
        }
        public override IEnumerator<T> GetEnumerator() {
            IEnumerable<T> result = Enumerable.Empty<T>();
            foreach (QuadtreeCell<T> levelCell in allCellArray) {
                result = result.Concat(levelCell.list);
            }
            return result.GetEnumerator();
        }
        public override void Update() {
            Parallel.For(0, allCellArray.Length, i => {
                QuadtreeCell<T> cell = allCellArray[i];
                for (int itemIndex = cell.list.Count - 1; itemIndex >= 0; itemIndex--) {
                    T item = cell.list[itemIndex];
                    QuadtreeCell<T> cell_new = GetCell(item.AABB);
                    if (cell_new != cell) {
                        cell.list.RemoveAt(itemIndex);
                        cell_new.recieverBag.Add(item);
                    }
                }
            });
            Parallel.For(0, allCellArray.Length, i => {
                QuadtreeCell<T> cell = allCellArray[i];
                cell.list.AddRange(cell.recieverBag);
                cell.recieverBag.Clear();
            });
        }

        private IEnumerable<QuadtreeCell<T>> GetHitCells(AABB aABB, int levelIndex) {
            QuadtreeLevel<T> level = levelArray[levelIndex];
            int cellIndex_lowerX = (int)(aABB.LowerBound.X / level.cellWidth);
            int cellIndex_lowerY = (int)(aABB.LowerBound.Y / level.cellHeight);
            int cellIndex_upperX = (int)(aABB.UpperBound.X / level.cellWidth);
            int cellIndex_upperY = (int)(aABB.UpperBound.Y / level.cellHeight);
            if (cellIndex_upperX - cellIndex_lowerX >= level.cellArraySideLength) {
                cellIndex_upperX = cellIndex_lowerX + level.cellArraySideLength - 1;
            }
            if (cellIndex_upperY - cellIndex_lowerY >= level.cellArraySideLength) {
                cellIndex_upperY = cellIndex_lowerY + level.cellArraySideLength - 1;
            }
            for (int i = cellIndex_lowerX, i_actual = i; i <= cellIndex_upperX; i++, i_actual++) {
                if (i_actual == level.cellArraySideLength) i_actual = 0;
                for (int j = cellIndex_lowerY, j_actual = j; j <= cellIndex_upperY; j++, j_actual++) {
                    if (j_actual == level.cellArraySideLength) j_actual = 0;
                    yield return level.cellArray[i_actual, j_actual];
                }
            }
        }

        public override IEnumerable<T> AABBHitTest(AABB testerAABB, Predicate<T> hittablityPredicate) {
            foreach (T t in overLevelCell.list) {
                if (hittablityPredicate(t) && CollisionCalculator.CheckAABBIntersection(testerAABB, t.AABB, this)) yield return t;
            }
            for (int levelIndex = 0; levelIndex < levelArray.Length; levelIndex++) {
                foreach (QuadtreeCell<T> cell in GetHitCells(testerAABB, levelIndex)) {
                    foreach (T t in cell.list) {
                        if (hittablityPredicate(t) && CollisionCalculator.CheckAABBIntersection(testerAABB, t.AABB, this)) yield return t;
                    }
                }
            }
        }
        public override IEnumerable<T> HitTest(IShapedObject testerShaped, Predicate<T> hittablityPredicate) {
            foreach (T t in overLevelCell.list) {
                if (hittablityPredicate(t) && CollisionCalculator.CheckCollision(testerShaped, t, this)) yield return t;
            }
            for (int levelIndex = 0; levelIndex < levelArray.Length; levelIndex++) {
                foreach (QuadtreeCell<T> cell in GetHitCells(testerShaped.AABB, levelIndex)) {
                    foreach (T t in cell.list) {
                        if (hittablityPredicate(t) && CollisionCalculator.CheckCollision(testerShaped, t, this)) yield return t;
                    }
                }
            }
        }

        public override void ParallelForEach(Action<T> action) {
            Parallel.For(0, allCellArray.Length, i => {
                QuadtreeCell<T> cell = allCellArray[i];
                foreach (T t in cell.list) {
                    action(t);
                }
            });
        }

        private void CellInternalCheck(QuadtreeCell<T> cell, Func<T, T, bool> collidabilityPredicate) {
            for (int itemIndex_current = 0; itemIndex_current < cell.list.Count; itemIndex_current++) {
                T item_current = cell.list[itemIndex_current];
                for (int itemIndex_check = itemIndex_current + 1; itemIndex_check < cell.list.Count; itemIndex_check++) {
                    T item_check = cell.list[itemIndex_check];
                    if (collidabilityPredicate(item_current, item_check) && CollisionCalculator.CheckCollision(item_current, item_check, this)) {
                        AddCollisionData(item_current, item_check);
                    }
                }
            }
        }
        private void LowerLevelCheck(QuadtreeCell<T> cell_current, Func<T, T, bool> collidabilityPredicate) {
            foreach (T item_current in cell_current.list) {
                for (int levelIndex_check = cell_current.level + 1; levelIndex_check < levelArray.Length; levelIndex_check++) {
                    var cells = GetHitCells(item_current.AABB, levelIndex_check);
                    foreach (QuadtreeCell<T> cell_check in cells) {
                        foreach (T item_check in cell_check.list) {
                            if (collidabilityPredicate(item_current, item_check) && CollisionCalculator.CheckCollision(item_current, item_check, this)) {
                                AddCollisionData(item_current, item_check);
                            }
                        }
                    }
                }
            }
        }
        public override void PrepareCollisionData(Func<T, T, bool> collidabilityPredicate) {
            ClearCollisionData();
            Parallel.For(0, allCellArray.Length, i => {
                QuadtreeCell<T> cell = allCellArray[i];
                CellInternalCheck(cell, collidabilityPredicate);
                LowerLevelCheck(cell, collidabilityPredicate);
            });
        }
    }

    internal class QuadtreeLevel<T> {
        internal QuadtreeLevel(int level, double width, double height, IList list) {
            this.level = level;
            cellArraySideLength = 1 << level;//two to the power of level
            cellWidth = width / cellArraySideLength;
            cellHeight = height / cellArraySideLength;
            cellArray = new QuadtreeCell<T>[cellArraySideLength, cellArraySideLength];
            for (int i = 0; i < cellArraySideLength; i++) {
                for (int j = 0; j < cellArraySideLength; j++) {
                    cellArray[i, j] = new QuadtreeCell<T>(level, i, j, list);
                }
            }
        }
        private readonly int level;
        internal readonly int cellArraySideLength;
        internal readonly double cellWidth, cellHeight;
        internal readonly QuadtreeCell<T>[,] cellArray;
        internal void Clear() {
            for (int i = 0; i < cellArraySideLength; i++) {
                for (int j = 0; j < cellArraySideLength; j++) {
                    cellArray[i, j].list.Clear();
                }
            }
        }
    }

    internal class QuadtreeCell<T> {
        internal QuadtreeCell(int level, int indexX, int indexY, IList list) {
            this.level = level;
            this.indexX = indexX;
            this.indexY = indexY;
            list.Add(this);
        }
        internal readonly List<T> list = new List<T>();
        internal readonly ConcurrentBag<T> recieverBag = new ConcurrentBag<T>();

        internal readonly int level;
        internal readonly int indexX;
        internal readonly int indexY;
    }
}
