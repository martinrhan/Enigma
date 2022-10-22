using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Enigma.PhysicsEngine;

namespace Enigma.Game {
    /// <summary>
    /// Each PlayerGameBodyBehaviour is assigned to a Player and its PlayerGameBody. Fields in this class are to be set by UI application then get by the GameBody. 
    /// </summary>
    public sealed class PlayerGameBodyBehaviour : GameBodyBehaviour {
        internal PlayerGameBodyBehaviour() {
        }
        internal PlayerBehaviourUpdateData UpdateData = new PlayerBehaviourUpdateData();

        private protected override ReturnedValue_Update_Protected Update_Protected(GameBody gameBody) {
            ReturnedValue_Update_Protected result = new(UpdateData.FocusedAbilityIndex, UpdateData.ToStartCastingAbilityIndexes, UpdateData.ToCancelCastingAbilityIndexes, UpdateData.ToSetInputDatas, UpdateData.ToSetMovementAction);
            return result;
        }

    }

    public class PlayerBehaviourUpdateData {
        public int FocusedAbilityIndex { get; init; } = -1;
        public int[] ToStartCastingAbilityIndexes { get; init; } = new int[0];
        public int[] ToCancelCastingAbilityIndexes { get; init; } = new int[0];
        public ValueTuple<int, AbilityCastInputData>[] ToSetInputDatas { get; init; } = new ValueTuple<int, AbilityCastInputData>[0];
        public Tuple<GameBodyMovementAction> ToSetMovementAction { get; init; } = null;
    }
}
