using Enigma.Spacial;
using Enigma.Common.Math;

namespace Enigma.PhysicsEngine {
    public abstract class PhysicsBody : IShapedObject {
        public Vector2 Velocity { get; protected internal set;}
        public bool AllowPositionAdjustment { get; protected internal set; }
        public abstract AABB AABB { get; }
        internal abstract void SetAABBLowerBound(in Vector2 position);
        internal abstract void Translate(in Vector2 displacement);
    }
}
