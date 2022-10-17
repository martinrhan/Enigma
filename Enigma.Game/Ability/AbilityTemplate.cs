using System;
using System.Collections.Generic;
using System.Linq;

namespace Enigma.Game {
    /// <summary>
    /// Read-only objects created by reading json files and stored in a static immutable Dictionary. Any Ability or AbilityItem is created with a reference to an AbilityTemplate.
    /// </summary>
    /// 
    public class AbilityTemplate : IRarityObject {
        internal static readonly Dictionary<string, AbilityTemplate> dictionary = new Dictionary<string, AbilityTemplate>();
        public static readonly IReadOnlyDictionary<string, AbilityTemplate> Dictionary = dictionary;

        private string id;
        public string Id {
            get => id;
            init {
                id = value;
                abilityMechanismFactory = AbilityMechanismFactory.Dictionary[value];
            }
        }
        public int Price { get; init; }
        public int Size { get; init; }
        public Rarity Rarity { get; init; }

        private AbilityMechanismFactory abilityMechanismFactory;
        public AbilityMechanismFactory AbilityMechanismFactory => abilityMechanismFactory;

        public Ability New() {
            return new Ability(this);
        }
    }
}