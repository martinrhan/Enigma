using System;
using Enigma.Common.Math;
using Enigma.Spacial;

namespace Enigma.PhysicsEngine {
    internal static class CollisionResolver {
        internal static void AdjustPosition(PhysicsBody bodyA, PhysicsBody bodyB, Spacial.Spacial spacial) {
            switch (bodyA) {
                case CirclePhysicsBody circleBodyA:
                    AdjustPosition(circleBodyA, bodyB, spacial);
                    return;
                default:
                    throw new ArgumentException("The argument's type" + bodyA.GetType().ToString() + "is not supported.");
            }
        }

        internal static void AdjustPosition(CirclePhysicsBody circleBodyA, PhysicsBody bodyB, Spacial.Spacial spacial) {
            switch (bodyB) {
                case CirclePhysicsBody circleBodyB:
                    AdjustPosition(circleBodyA, circleBodyB, spacial);
                    return;
                default:
                    throw new ArgumentException("The argument's type" + bodyB.GetType().ToString() + "is not supported.");
            }
        }

        internal static void AdjustPosition(CirclePhysicsBody circlebodyA, CirclePhysicsBody circleBodyB, Spacial.Spacial spacial) {
            Vector2 centerDifference = spacial.GetShortestDisplacement(circlebodyA.Center, circleBodyB.Center);
            double currentDistance = centerDifference.Length;
            double suppossedDistance = circlebodyA.Radius + circleBodyB.Radius;
            double needToMoveDistance = suppossedDistance - currentDistance;
            double bodyASpeed = circlebodyA.Velocity.Length, bodyBSpeed = circleBodyB.Velocity.Length;
            double totalSpeed = bodyASpeed + bodyBSpeed;
            double bodyASpeedProportion = bodyASpeed / totalSpeed;
            double bodyBSpeedProportion = bodyBSpeed / totalSpeed;
            Vector2 bodyADisplacement = centerDifference.ChangeLength(needToMoveDistance * bodyASpeedProportion);
            Vector2 bodyBDisplacement = centerDifference.ChangeLength(-needToMoveDistance * bodyBSpeedProportion);
            circlebodyA.Translate(bodyADisplacement);
            circleBodyB.Translate(bodyBDisplacement);
        }
    }
}
