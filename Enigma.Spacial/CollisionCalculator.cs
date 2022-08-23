using Enigma.Common.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Spacial {
    internal static class CollisionCalculator {
        internal static bool CheckAABBIntersection(in AABB a, in AABB b, Spacial spaical) {
            if (AABB.HaveIntersection(a, b)) return true;
            else
            if (a.UpperBound.X > spaical.Width) {
                if (AABB.HaveIntersection(a.Translate(new(-spaical.Width, 0)), b)) return true;
                if (a.UpperBound.Y > spaical.Height) {
                    if (AABB.HaveIntersection(a.Translate(new(0, -spaical.Height)), b)) return true;
                    if (AABB.HaveIntersection(a.Translate(new(-spaical.Width, -spaical.Height)), b)) return true;
                }
            } else {
                if (a.UpperBound.Y > spaical.Height) {
                    if (AABB.HaveIntersection(a.Translate(new(0, -spaical.Height)), b)) return true;
                }
            }
            if (b.UpperBound.X > spaical.Width) {
                if (AABB.HaveIntersection(b.Translate(new(-spaical.Width, 0)), a)) return true;
                if (b.UpperBound.Y > spaical.Height) {
                    if (AABB.HaveIntersection(b.Translate(new(0, -spaical.Height)), a)) return true;
                    if (AABB.HaveIntersection(b.Translate(new(-spaical.Width, -spaical.Height)), a)) return true;
                }
            } else {
                if (a.UpperBound.Y > spaical.Height) {
                    if (AABB.HaveIntersection(b.Translate(new(0, -spaical.Height)), a)) return true;
                }
            }
            return false;
        }

        internal static bool CheckCollision(IShapedObject shapeA, IShapedObject shapeB, Spacial spacial) {
            switch (shapeA) {
                case ICircleShapedObject circleA:
                    return CheckCollision(circleA, shapeB, spacial);
                case IRectangleShapedObject rectangleA:
                    return CheckCollision(rectangleA, shapeB, spacial);
                default:
                    throw new ArgumentException("The argument's type" + shapeA.GetType().ToString() + "is not supported.");
            }
        }
        internal static bool CheckCollision(ICircleShapedObject circleA, IShapedObject shapeB, Spacial spacial) {
            switch (shapeB) {
                case ICircleShapedObject circleB:
                    return CalculateCollision(circleA.Shape, circleB.Shape, spacial);
                case IRectangleShapedObject rectangleB:
                    return CalculateCollision(circleA.Shape, rectangleB.Shape, spacial);
                default:
                    throw new ArgumentException("The argument's type" + shapeB.GetType().ToString() + "is not supported.");
            }
        }
        internal static bool CheckCollision(IRectangleShapedObject rectangleA, IShapedObject shapeB, Spacial spacial) {
            switch (shapeB) {
                case ICircleShapedObject circleB:
                    return CalculateCollision(circleB.Shape, rectangleA.Shape, spacial);
                //case IRectangleShaped rectangleB:
                    //return CalculateCollision(rectangleA, rectangleB, spacial);
                default:
                    throw new ArgumentException("The argument's type" + shapeB.GetType().ToString() + "is not supported.");
            }
        }
        internal static bool CalculateCollision(in Circle circleA, in Circle circleB, Spacial spacial) {
            return spacial.GetShortestDisplacement(circleA.Center, circleB.Center).Length < (circleA.Radius + circleB.Radius);
        }
        internal static bool CalculateCollision(in Circle circleA, in Rectangle rectangleB, Spacial spacial) {
            static double GetTravelDistanceFromPointToLine(in Vector2 point, in Vector2 lineStart, in Vector2 lineEnd) {
                Vector2 displacement = lineEnd - lineStart;
                Vector2 leftwardPerpendicular = displacement.Rotate90Degree();
                Ray ray_pointToLine = new Ray(point, leftwardPerpendicular);
                Ray ray_line = new Ray(lineStart, displacement);
                return Ray.GetTravelDistance(ray_pointToLine, ray_line);
            }
            static bool CheckCollision(in Circle circle, in Rectangle rectangle) {
                //If the circle intersects any line, return true.
                double travelDistance01 = GetTravelDistanceFromPointToLine(circle.Center, rectangle.Points[0], rectangle.Points[1]);
                if (-circle.Radius < travelDistance01 && travelDistance01 < circle.Radius) return true;
                double travelDistance12 = GetTravelDistanceFromPointToLine(circle.Center, rectangle.Points[1], rectangle.Points[2]);
                if (-circle.Radius < travelDistance12 && travelDistance12 < circle.Radius) return true;
                double travelDistance23 = GetTravelDistanceFromPointToLine(circle.Center, rectangle.Points[2], rectangle.Points[3]);
                if (-circle.Radius < travelDistance23 && travelDistance23 < circle.Radius) return true;
                double travelDistance30 = GetTravelDistanceFromPointToLine(circle.Center, rectangle.Points[3], rectangle.Points[0]);
                if (-circle.Radius < travelDistance30 && travelDistance30 < circle.Radius) return true;
                //If center of the circle is at right side for each line (going clockwise), return true.
                if (0 < travelDistance01 && 0 < travelDistance12 && 0 < travelDistance23 && 0 < travelDistance30) return true;
                return false;
            }
            bool circleAABBUpperBoundXGreaterThanSpacialWidth = circleA.AABB.UpperBound.X > spacial.Width;
            bool rectangleAABBUpperBoundXGreaterThanSpacialWidth = rectangleB.AABB.UpperBound.X > spacial.Width;
            bool circleAABBUpperBoundYGreaterThanSpacialHeight = circleA.AABB.UpperBound.Y > spacial.Height;
            bool rectangleAABBUpperBoundYGreaterThanSpacialHeight = rectangleB.AABB.UpperBound.Y > spacial.Height;
            if (CheckCollision(circleA, rectangleB)) return true;
            if (circleAABBUpperBoundYGreaterThanSpacialHeight != rectangleAABBUpperBoundYGreaterThanSpacialHeight) {
                if (circleAABBUpperBoundYGreaterThanSpacialHeight) {
                    if (CheckCollision(circleA.Translate(new Vector2(0, -spacial.Height)), rectangleB)) return true;
                } else {
                    if (CheckCollision(circleA, rectangleB.Translate(new Vector2(0, -spacial.Height)))) return true;
                }
            }
            if (circleAABBUpperBoundXGreaterThanSpacialWidth != rectangleAABBUpperBoundXGreaterThanSpacialWidth) {
                if (circleAABBUpperBoundXGreaterThanSpacialWidth) {
                    if (CheckCollision(circleA.Translate(new Vector2(-spacial.Width, 0)), rectangleB)) return true;
                } else {
                    if (CheckCollision(circleA, rectangleB.Translate(new Vector2(-spacial.Width, 0)))) return true;
                }
            }
            if (circleAABBUpperBoundYGreaterThanSpacialHeight != rectangleAABBUpperBoundYGreaterThanSpacialHeight &&
                circleAABBUpperBoundXGreaterThanSpacialWidth != rectangleAABBUpperBoundXGreaterThanSpacialWidth) {
                Circle circle = circleA;
                Rectangle rectangle = rectangleB;
                if (circleAABBUpperBoundYGreaterThanSpacialHeight) {
                    circle = circle.Translate(new Vector2(0, -spacial.Height));
                } else {
                    rectangle = rectangle.Translate(new Vector2(0, -spacial.Height));
                }
                if (circleAABBUpperBoundXGreaterThanSpacialWidth) {
                    circle = circle.Translate(new Vector2(-spacial.Width, 0));
                } else {
                    rectangle = rectangle.Translate(new Vector2(-spacial.Width, 0));
                }
                if (CheckCollision(circle, rectangle)) return true;
            }
            return false;
        }
    }
}
