using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enigma.Common.Math;

namespace Enigma.Spacial {
    public static class AABBFactory {
        public static AABB FromCircle(in Circle circle) {
            return FromCircle(circle.Center, circle.Radius);
        }
        public static AABB FromCircle(Vector2 center, double radius) {
            (double x, double y) = center;
            return new AABB(new Vector2(x - radius, y - radius), new Vector2(x + radius, y + radius));
        }
        public static AABB FromRectangle(in Rectangle rectangle) {
            return FromPolygon(rectangle.Points);
        }
        public static AABB FromPolygon(IReadOnlyList<Vector2> points) {
            double lowerX = points[0].X, lowerY = points[0].Y, upperX = points[0].X, upperY = points[0].Y;
            for (int i = 1; i < points.Count; i++) {
                if (points[i].X < lowerX) lowerX = points[i].X;
                if (points[i].Y < lowerY) lowerY = points[i].Y;
                if (points[i].X > upperX) upperX = points[i].X;
                if (points[i].Y > upperY) upperY = points[i].Y;
            }
            return new AABB(new Vector2(lowerX, lowerY), new Vector2(upperX, upperY));
        }
        public static AABB FromCircularSector(in CircularSector circularSector) {
            Span<double> checkPoints = stackalloc double[4];
            Span<bool> checkPointReached = stackalloc bool[4];
            double firstCheckPointIndex_double = Math.Ceiling(circularSector.AngleStart / Constants.HalfPI);
            double firstCheckPoint = firstCheckPointIndex_double * Constants.HalfPI;
            double toFirstCheckPoint = firstCheckPoint - circularSector.AngleStart;
            int checkPointReachedCount = 0;
            if (circularSector.Theta > toFirstCheckPoint) {
                checkPointReachedCount++;
                double remainingTheta = circularSector.Theta - toFirstCheckPoint;
                checkPointReachedCount += (int)(remainingTheta / Constants.HalfPI);
            }
            int checkPointIndex = (int)firstCheckPointIndex_double;
            if (checkPointIndex == 4) checkPointIndex = 0;
            for (int i = 0; i < checkPointReachedCount; i++) {
                checkPointReached[checkPointIndex] = true;
                checkPointIndex++;
            }
            Vector2 arcPointA = circularSector.Center + Vector2.FromPolar(circularSector.AngleStart, circularSector.Radius);
            Vector2 arcPointB = circularSector.Center + Vector2.FromPolar(circularSector.AngleStart + circularSector.Theta, circularSector.Radius);
            double upperBoundX;
            if (checkPointReached[0]) {
                upperBoundX = circularSector.Center.X + circularSector.Radius;
            } else {
                upperBoundX = Math.Max(circularSector.Center.X, Math.Max(arcPointA.X, arcPointB.X));
            }
            double upperBoundY;
            if (checkPointReached[1]) {
                upperBoundY = circularSector.Center.Y + circularSector.Radius;
            } else {
                upperBoundY = Math.Max(circularSector.Center.Y, Math.Max(arcPointA.Y, arcPointB.Y));
            }
            double lowerBoundX;
            if (checkPointReached[2]) {
                lowerBoundX = circularSector.Center.X - circularSector.Radius;
            } else {
                lowerBoundX = Math.Min(circularSector.Center.X, Math.Min(arcPointA.X, arcPointB.X));
            }
            double lowerBoundY;
            if (checkPointReached[3]) {
                lowerBoundY = circularSector.Center.Y - circularSector.Radius;
            } else {
                lowerBoundY = Math.Min(circularSector.Center.Y, Math.Min(arcPointA.Y, arcPointB.Y));
            }
            return new AABB(lowerBoundX, lowerBoundY, upperBoundX, upperBoundY);
        }
    }
}
