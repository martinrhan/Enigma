using System;

namespace Enigma.Common.Math {
    public readonly struct Vector2 : IEquatable<Vector2>, IFormattable {
        public Vector2(double x, double y) {
            X = x; Y = y;
        }
        public double X { get; init; }
        public double Y { get; init; }

        public static Vector2 Zero => default;

        public static Vector2 FromPolar(double direction, double length) {
            return new Vector2(System.Math.Cos(direction) * length, System.Math.Sin(direction) * length);
        }

        public static Vector2 operator +(in Vector2 vectorA, in Vector2 vectorB) {
            return new Vector2(vectorA.X + vectorB.X, vectorA.Y + vectorB.Y);
        }

        public static Vector2 operator -(in Vector2 vectorA, in Vector2 vectorB) {
            return new Vector2(vectorA.X - vectorB.X, vectorA.Y - vectorB.Y);
        }

        public static Vector2 operator -(in Vector2 vector) {
            return new Vector2(-vector.X, -vector.Y);
        }

        public static Vector2 operator *(in Vector2 vector, in double real) {
            return new Vector2(vector.X * real, vector.Y * real);
        }

        public static Vector2 operator /(in Vector2 vector, in double real) {
            return new Vector2(vector.X / real, vector.Y / real);
        }

        public bool Equals(Vector2 other) {
            return X == other.X && Y == other.Y;
        }

        public static bool operator ==(in Vector2 vectorA, in Vector2 vectorB) {
            return vectorA.X == vectorB.X && vectorA.Y == vectorB.Y;
        }

        public static bool operator !=(in Vector2 vectorA, in Vector2 vectorB) {
            return vectorA.X != vectorB.X || vectorA.Y != vectorB.Y;
        }

        public override bool Equals(object obj) {
            return obj is Vector2 v && this == v;
        }

        public double Length => System.Math.Sqrt(X * X + Y * Y);

        public Vector2 ChangeLength(double length) => this * (length / Length);

        public double Direction => System.Math.Atan2(Y, X);

        public Vector2 Rotate(double theta) => new Vector2(
                X * System.Math.Cos(theta) - Y * System.Math.Sin(theta),
                X * System.Math.Sin(theta) + Y * System.Math.Cos(theta)
                );
        public Vector2 Rotate(double theta, in Vector2 pivot) {
            Vector2 difference = this - pivot;
            Vector2 rotatedDifference = difference.Rotate(theta);
            return pivot + rotatedDifference;
        }
        public Vector2 Rotate90Degree() {
            return new Vector2(-Y, X);
        }
        public Vector2 RotateNegative90Degree() {
            return new Vector2(Y, -X);
        }

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public void Deconstruct(out double x, out double y) {
            x = X; y = Y;
        }

        public override string ToString() => $"({X},{Y})";
        public string ToString(string format) => $"({X.ToString(format)},{Y.ToString(format)})";
        public string ToString(IFormatProvider formatProvider) => $"({X.ToString(formatProvider)},{Y.ToString(formatProvider)})";
        public string ToString(string format, IFormatProvider formatProvider) =>
            $"({X.ToString(format, formatProvider)},{Y.ToString(format, formatProvider)})";

    }

}
