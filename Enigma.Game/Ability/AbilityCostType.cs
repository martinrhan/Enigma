using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Game {
    public abstract class AbilityCostType {

        public readonly struct ReturnedValue_ExecuteCost {
            public ReturnedValue_ExecuteCost(bool isSuccessful) {
                IsSuccessful = isSuccessful;
            }
            public readonly bool IsSuccessful;
        }

        public abstract ReturnedValue_ExecuteCost ExecuteCost();

        private static readonly Dictionary<string, AbilityCostType> dictionary = new Dictionary<string, AbilityCostType>() {
            {"LocalMana", new AbilityCostType_LocalMana()}
        };
        public static IReadOnlyDictionary<string, AbilityCostType> Dictionary => dictionary;
    }

    public struct AbilityCost {
        public AbilityCostType Type { get; internal set; }
        public float Value { get; internal set; }
    }

    public class AbilityCostType_LocalMana : AbilityCostType {
        public override ReturnedValue_ExecuteCost ExecuteCost() {
            return default;
        }
    }
}
