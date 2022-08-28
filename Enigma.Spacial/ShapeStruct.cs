using Enigma.Common.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Spacial {
    public readonly struct Circle {
        public Circle(in Vector2 center, double radius) {
            Center = center;
            if (radius < 0) throw new ArgumentException("radius cannot be less than 0", nameof(radius));
            Radius = radius;
            AABB = AABBFactory.FromCircle(center, radius);
        }
        public Vector2 Center { get; }
        public double Radius { get; }
        public AABB AABB { get; }
        public Circle Translate(in Vector2 displacement) => new Circle(Center + displacement, Radius);
        public Circle ChangeCenter(in Vector2 center) => new Circle(center, Radius);
        public Circle ChangeRadius(double radius) => new Circle(Center, radius);
    }

    public readonly struct Rectangle {
        public Rectangle(double width, double height) : this(
            new Vector2(0, 0),
            new Vector2(width, 0),
            new Vector2(width, height),
            new Vector2(0, height),
            width,
            height
        ) { }
        private Rectangle(in Vector2 point0, in Vector2 point1, in Vector2 point2, in Vector2 point3, double width, double height) {
            points[0] = point0;
            points[1] = point1;
            points[2] = point2;
            points[3] = point3;
            Width = width;
            Height = height;
            AABB = AABBFactory.FromPolygon(points);
        }
        public Rectangle Translate(in Vector2 displacement) => new Rectangle(
            points[0] + displacement,
            points[1] + displacement,
            points[2] + displacement,
            points[3] + displacement,
            Width,
            Height
        );
        public Rectangle Rotate(double theta, in Vector2 pivot) => new Rectangle(
            points[0].Rotate(theta, pivot),
            points[1].Rotate(theta, pivot),
            points[2].Rotate(theta, pivot),
            points[3].Rotate(theta, pivot),
            Width,
            Height
        );
        public Rectangle ChangeWidth(double width) {
            Vector2 newWidthVector = (points[1] - points[0]).ChangeLength(width);
            return new Rectangle(
                points[0],
                points[0] + newWidthVector,
                points[3] + newWidthVector,
                points[3],
                width,
                Height
            );
        }
        public Rectangle ChangeHeight(double height) {
            Vector2 newHeightVector = (points[3] - points[0]).ChangeLength(height);
            return new Rectangle(
                points[0],
                points[1],
                points[1] + newHeightVector,
                points[0] + newHeightVector,
                Width,
                height
            );
        }

        private readonly Vector2[] points = new Vector2[4];
        public IReadOnlyList<Vector2> Points => points;
        public double Width { get; }
        public double Height { get; }
        public AABB AABB { get; }
    }

    public readonly struct CircularSector {
        public CircularSector(in Vector2 center, double radius, double angleStart, double theta) {
            Center = center;
            if (radius < 0) throw new ArgumentException("radius cannot be less than 0", nameof(radius));
            Radius = radius;
            if (angleStart < 0 || Constants.TwoPI < angleStart) throw new ArgumentException("the starting position of the angle should be between 0 and two pi", nameof(angleStart));
            AngleStart = angleStart;
            if (Constants.TwoPI < theta) throw new ArgumentException("theta cannot be greater than two pi");
            Theta = theta;
        }
        public Vector2 Center { get; private init; }
        public double Radius { get; private init; }
        public double AngleStart { get; private init; }
        public double Theta { get; private init; }
        public AABB AABB => AABBFactory.FromCircularSector(this);
        public CircularSector Translate(in Vector2 displacement) => new CircularSector() {
            Center = this.Center + displacement,
            Radius = this.Radius,
            AngleStart = this.AngleStart,
            Theta = this.Theta,
        };
    }
}
