using System;
using System.Reflection;

namespace Enigma.Game {
    public abstract class GameBodyBehaviour {

        internal void Update_Internal(GameBody gameBody, double deltaTime) {
            Update_Private(Update_Protected(gameBody), gameBody, deltaTime);
        }
        protected readonly struct ReturnedValue_Update_Protected {
            internal ReturnedValue_Update_Protected(int[] toStartCastingAbilityIndexes, int[] toCancelCastingAbilityIndexes, ValueTuple<int, AbilityCastInputData>[] toSetInputDatas, Tuple<GameBodyMovementAction> toSetMovementAction) {
                ToStartCastingAbilityIndexes = toStartCastingAbilityIndexes;
                ToCancelCastingAbilityIndexes = toCancelCastingAbilityIndexes;
                ToSetInputDatas = toSetInputDatas;
                ToSetMovementAction = toSetMovementAction;
            }
            internal readonly int[] ToStartCastingAbilityIndexes;
            internal readonly int[] ToCancelCastingAbilityIndexes;
            internal readonly ValueTuple<int, AbilityCastInputData>[] ToSetInputDatas;
            internal readonly Tuple<GameBodyMovementAction> ToSetMovementAction;
        }
        private protected abstract ReturnedValue_Update_Protected Update_Protected(GameBody gameBody);

        public readonly AbilityCastInputDataCollection AbilityCastInputDataCollection = new AbilityCastInputDataCollection();
        public GameBodyMovementAction CurrentMovementAction { get; private set; }

        private void Update_Private(in ReturnedValue_Update_Protected returnedValue_Update, GameBody gameBody, double deltaTime) {
            AbilityCastInputDataCollection.ResizeIfSmaller(gameBody.AbilityCollection.Count);
            AbilityCastInputDataCollection.Update(returnedValue_Update.ToSetInputDatas);
            foreach (int index in returnedValue_Update.ToStartCastingAbilityIndexes) {
                gameBody.AbilityCollection[index]?.StartCasting(gameBody, AbilityCastInputDataCollection[index], deltaTime);
            }
            foreach (int index in returnedValue_Update.ToCancelCastingAbilityIndexes) {
                gameBody.AbilityCollection[index]?.CancelCasting(gameBody, AbilityCastInputDataCollection[index], deltaTime);
            }
            for (int index = 0; index < gameBody.AbilityCollection.Count; index++) {
                gameBody.AbilityCollection[index]?.Update_Internal(gameBody, AbilityCastInputDataCollection[index], deltaTime);
            }
            var newMovementAction = returnedValue_Update.ToSetMovementAction;
            if (newMovementAction != null) {
                CurrentMovementAction?.Cancel_Internal(gameBody, deltaTime);
                CurrentMovementAction = newMovementAction.Item1;
            }
            if (CurrentMovementAction != null) {
                if (CurrentMovementAction.Update_Internal(gameBody, deltaTime)) {
                    CurrentMovementAction = null;
                }
            }
        }
    }
}