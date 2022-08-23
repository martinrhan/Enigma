using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Enigma.Common;
using Enigma.Common.Math;

namespace Enigma.Spacial {
    public readonly struct AABB : IEquatable<AABB>, IFormattable {
        public AABB(double lowerBoundX, double lowerBoundY, double upperBoundX, double upperBoundY)
            : this(new Vector2(lowerBoundX, lowerBoundY), new Vector2(upperBoundX, upperBoundY)) { }
        public AABB(in Vector2 lowerBound, in Vector2 upperBound) {
            if (CheckInvalid(lowerBound, upperBound)) {
                throw new ArgumentException();
            }
            LowerBound = lowerBound;
            UpperBound = upperBound;
        }
        public Vector2 LowerBound { get; init; }
        public Vector2 UpperBound { get; init; }

        public AABB Translate(in Vector2 displacement) =>
            new AABB(this.LowerBound + displacement, this.UpperBound + displacement);

        public static AABB Combine(in AABB a, in AABB b) => new AABB(
                new Vector2(Math.Min(a.LowerBound.X, b.LowerBound.X), Math.Min(a.LowerBound.Y, b.LowerBound.Y)),
                new Vector2(Math.Max(a.UpperBound.X, b.UpperBound.X), Math.Max(a.UpperBound.Y, b.UpperBound.Y))
                );

        public static AABB Intersect(in AABB a, in AABB b) {
            double lowerX = Math.Max(a.LowerBound.X, b.LowerBound.X);
            double lowerY = Math.Max(a.LowerBound.Y, b.LowerBound.Y);
            double upperX = Math.Min(a.UpperBound.X, b.UpperBound.X);
            double upperY = Math.Min(a.UpperBound.Y, b.UpperBound.Y);
            if (upperX > lowerX && upperY > lowerY) {
                return new AABB(new Vector2(lowerX, lowerY), new Vector2(upperX, upperY));
            } else {
                return default;
            }
        }
        public static double IntersectArea(in AABB a, in AABB b) {
            double lowerX = Math.Max(a.LowerBound.X, b.LowerBound.X);
            double lowerY = Math.Max(a.LowerBound.Y, b.LowerBound.Y);
            double upperX = Math.Min(a.UpperBound.X, b.UpperBound.X);
            double upperY = Math.Min(a.UpperBound.Y, b.UpperBound.Y);
            if (upperX > lowerX && upperY > lowerY) {
                return (upperX - lowerX) * (upperY - lowerY);
            } else {
                return 0;
            }
        }
        public static bool HaveIntersection(in AABB a, in AABB b) {
            double lowerX = Math.Max(a.LowerBound.X, b.LowerBound.X);
            double lowerY = Math.Max(a.LowerBound.Y, b.LowerBound.Y);
            double upperX = Math.Min(a.UpperBound.X, b.UpperBound.X);
            double upperY = Math.Min(a.UpperBound.Y, b.UpperBound.Y);
            return upperX > lowerX && upperY > lowerY;
        }
        public double Width => UpperBound.X - LowerBound.X;
        public double Height => UpperBound.Y - LowerBound.Y;
        public double Area => Width * Height;

        public static AABB CreateRandom(double multiplier) {
            Random random = new Random();
            double getNext() {
                return random.NextDouble() * multiplier;
            }
            double lowerBoundX = getNext(), upperBoundX = getNext(), lowerBoundY = getNext(), upperBoundY = getNext();
            if (upperBoundX < lowerBoundX) MyMethods.SwapValue(ref lowerBoundX, ref upperBoundX);
            if (upperBoundY < lowerBoundY) MyMethods.SwapValue(ref lowerBoundY, ref upperBoundY);
            return new AABB(new Vector2(lowerBoundX, lowerBoundY), new Vector2(upperBoundX, upperBoundY));
        }

        private bool IsInvalid => CheckInvalid(LowerBound, UpperBound);
        private static bool CheckInvalid(in Vector2 lowerBound, in Vector2 upperBound) =>
            lowerBound.X > upperBound.X || lowerBound.Y > upperBound.Y;

        public override int GetHashCode() => HashCode.Combine(LowerBound.GetHashCode(), UpperBound.GetHashCode());

        public bool Equals(AABB other) => LowerBound == other.LowerBound & UpperBound == other.UpperBound;

        public override string ToString() => $"{LowerBound},{UpperBound}";
        public string ToString(string format) => $"{LowerBound.ToString(format)},{UpperBound.ToString(format)}";
        public string ToString(IFormatProvider formatProvider) => $"{LowerBound.ToString(formatProvider)},{UpperBound.ToString(formatProvider)}";
        public string ToString(string format, IFormatProvider formatProvider) =>
            $"{LowerBound.ToString(format, formatProvider)},{UpperBound.ToString(format, formatProvider)}";
    }

}
