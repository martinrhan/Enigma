using System;
using Enigma.Game;

namespace Enigma.Game.Entities.AIGameBodyBehaviourFactories.Projectile {
    public class StandardProjectileBehaviour : AIGameBodyBehaviour {
        public StandardProjectileBehaviour(AIGameBodyBehaviourFactoryArguments arguments) {
            movementAction = new GameBodyMovementAction.BuiltIn.MoveToDirectionLimited(arguments.ProjectileArguments.MovementDirection, arguments.ProjectileArguments.MovementDistance);
        }
        private GameBodyMovementAction.BuiltIn.MoveToDirectionLimited movementAction;
        protected override void Start(UpdateInterface updateInterface) {
            updateInterface.ToSetMovementAction = new(movementAction);
        }
        protected override void Update(UpdateInterface updateInterface) {
            if (movementAction.IsCompleted) {
                updateInterface.ToStartCastingAbilityIndexes = new int[] { 0 };
            }
        }
    }

}
