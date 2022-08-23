using Enigma.Spacial;
using Enigma.Common.Math;

namespace Enigma.PhysicsEngine {
    public abstract class CirclePhysicsBody : PhysicsBody, ICircleShapedObject{
        public Vector2 Center { get; protected set; }
        public abstract double Radius { get;}

        public Circle Shape => new Circle(Center, Radius);
        public override AABB AABB => AABBFactory.FromCircle(Shape);

        internal override void SetAABBLowerBound(in Vector2 position) {
            Center = position + new Vector2(Radius, Radius);
        }
        internal override void Translate(in Vector2 displacement) {
            Center += displacement;
        }
    }
}
