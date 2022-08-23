using Enigma.Common.Math;
using Enigma.Game;
using static Enigma.Game.Extensions.GameWorldExtensions;

namespace Enigma.Game {
    partial class GameBodyMovementAction {
        public static class BuiltIn {
            public class MoveToDirection : GameBodyMovementAction {
                public MoveToDirection(double targetDirection) {
                    this.targetDirection = targetDirection;
                }
                private readonly double targetDirection;

                protected override bool Start(UpdateInterface updateInterface) {
                    updateInterface.SetMovement(targetDirection);
                    return false;
                }

                protected override bool Update(UpdateInterface updateInterface) {
                    return false;
                }

                protected override void Cancel(UpdateInterface updateInterface) {
                    updateInterface.StopMovement();
                }
            }
            public class MoveToLocation : GameBodyMovementAction {
                public MoveToLocation(in Vector2 targetPosition) {
                    this.targetPosition = targetPosition;
                }
                private readonly Vector2 targetPosition;
                private double remainingDistance;

                protected override bool Start(UpdateInterface updateInterface) {
                    Vector2 displacement = updateInterface.GameBody.GameWorld.GetShortestDisplacement(updateInterface.GameBody.Center, targetPosition);
                    double direction = displacement.Direction;
                    remainingDistance = displacement.Length;
                    updateInterface.SetMovement(direction);
                    return false;
                }

                protected override bool Update(UpdateInterface updateInterface) {
                    remainingDistance -= updateInterface.GameBody[GameBody.Property.Speed] * updateInterface.DeltaTime;
                    if (remainingDistance < 0) {
                        updateInterface.StopMovement();
                        return true;
                    }
                    return false;
                }

                protected override void Cancel(UpdateInterface updateInterface) {
                    updateInterface.StopMovement();
                }
            }
            public class MoveToDirectionLimited : GameBodyMovementAction {
                public MoveToDirectionLimited(double direction, double distance) {
                    this.direction = direction;
                    this.remainingDistance = distance;
                }
                private double direction;
                private double remainingDistance;

                protected override bool Start(UpdateInterface updateInterface) {
                    updateInterface.SetMovement(direction);
                    return false;
                }
                protected override bool Update(UpdateInterface updateInterface) {
                    remainingDistance -= updateInterface.GameBody[GameBody.Property.Speed] * updateInterface.DeltaTime;
                    if (remainingDistance < 0) {
                        updateInterface.StopMovement();
                        return true;
                    }
                    return false;
                }
                protected override void Cancel(UpdateInterface updateInterface) {
                    updateInterface.StopMovement();
                }
            }
        }
    }
}
