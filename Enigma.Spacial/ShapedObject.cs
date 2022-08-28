using Enigma.Common.Math;

namespace Enigma.Spacial {
    public class AABBShapedObject: IAABBShapedObject {
        public AABBShapedObject(in AABB shape) {
            Shape = shape;
        }
        public AABB Shape { get; set; }
        public AABB AABB => Shape;
    }
    public class CircleShapedObject : ICircleShapedObject {
        public CircleShapedObject(in Vector2 center, double radius) {
            Shape = new Circle(center, radius);
        }
        public CircleShapedObject(in Circle shape) {
            Shape = shape;
        }
        public Circle Shape { get; set; }
        public AABB AABB => Shape.AABB;
    }
    public class RectangleShapedObject : IRectangleShapedObject {
        public RectangleShapedObject(double width, double height) {
            Shape = new Rectangle(width, height);
        }
        public RectangleShapedObject(Rectangle shape) {
            Shape = shape;
        }
        public Rectangle Shape { get; set; }
        public AABB AABB => Shape.AABB;
    }

}
