using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;

namespace Enigma.Game {
    public abstract class AIGameBodyBehaviour : GameBodyBehaviour, IFactoryProduct {
        public static Expression FactoryLoadAddition(Type toLoadSubtype, ParameterExpression productExpression) {
            return Expression.Block();
        }

        public AIGameBodyBehaviourMode Mode { get; internal set; } = AIGameBodyBehaviourMode.Autonomous;

        public void PuppetUpdate(Action<UpdateInterface> action) {
            this.action = action;
        }
        private Action<UpdateInterface> action;

        private bool isStarted = false;
        private protected override ReturnedValue_Update_Protected Update_Protected(GameBody gameBody) {
            UpdateInterface updateInterface = new UpdateInterface(gameBody);
            if (!isStarted) {
                if (Mode != AIGameBodyBehaviourMode.Puppet) Start(updateInterface);
                isStarted = true;
            }
            if (Mode == AIGameBodyBehaviourMode.Puppet) {
                if (action != null) {
                    action(updateInterface);
                    action = null;
                }
            } else Update(updateInterface);
            return new(updateInterface.FocusedAbilityIndex,updateInterface.ToStartCastingAbilityIndexes, updateInterface.ToCancelCastingAbilityIndexes, updateInterface.ToSetInputDatas, updateInterface.ToSetMovementAction);
        }
        protected abstract void Start(UpdateInterface updateInterface);
        protected abstract void Update(UpdateInterface updateInterface);

        public class UpdateInterface {
            internal UpdateInterface(GameBody gameBody) {
                this.GameBody = gameBody;
            }
            public GameBody GameBody { get; }
            public int FocusedAbilityIndex = -1;
            public int[] ToStartCastingAbilityIndexes { get; set; } = new int[0];
            public int[] ToCancelCastingAbilityIndexes { get; set; } = new int[0];
            public ValueTuple<int, AbilityCastInputData>[] ToSetInputDatas { get; set; } = new ValueTuple<int, AbilityCastInputData>[0];
            public Tuple<GameBodyMovementAction> ToSetMovementAction { get; set; }
        }
    }

    public class AIGameBodyBehaviourFactoryArguments {
        public ProjectileAIGameBodyBehaviourFactoryArguments ProjectileArguments { get; init; }
    }

    public class ProjectileAIGameBodyBehaviourFactoryArguments {
        public ProjectileAIGameBodyBehaviourFactoryArguments(double movementDirection, double movementDistance) {
            MovementDistance = movementDistance;
            MovementDirection = movementDirection;
        }
        public double MovementDirection { get; }
        public double MovementDistance { get; }
    }

    public enum AIGameBodyBehaviourMode { Autonomous, Hybrid, Puppet }
}

