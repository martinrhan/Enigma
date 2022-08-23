using Enigma.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.GameWPF.Input {
    public class UserInputCollection {
        public HashSet<int> ToStartCastingIndexHashSet { get; } = new HashSet<int>();
        public HashSet<int> ToCancelCastingIndexHashSet { get; } = new HashSet<int>();
        public Dictionary<int, AbilityCastInputData> ToSetInputDataDictionary { get; } = new Dictionary<int, AbilityCastInputData>();
        public Tuple<GameBodyMovementAction> ToSetMovementActionTuple { get; set; } = null;

        public void Clear() {
            ToStartCastingIndexHashSet.Clear();
            ToCancelCastingIndexHashSet.Clear();
            ToSetInputDataDictionary.Clear();
            ToSetMovementActionTuple = null;
        }
    }
}
