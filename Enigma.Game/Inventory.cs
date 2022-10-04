using Enigma.Common.Collections;
using System;

namespace Enigma.Game {
    public sealed class Inventory : SlottedCollection<AbilityItem> {
        internal Inventory() : base(10) { }

        public int Gold { get; internal set; } = 100;

        internal new void InternalExchange(int indexA, int indexB) => base.InternalExchange(indexA, indexB);
        internal void ExchangeWithAbilityCollection(AbilityCollection abilityCollection, int indexFrom, int indexTo) {
            ExternalExchange(indexFrom, indexTo, abilityCollection, abilityItem => abilityItem.ConvertToAblity(), ability => ability.ConvertToAbilityItem());
        }
        internal void PlaceAt(int index, AbilityItem abilityItem) => base[index] = abilityItem;
        internal void RemoveAt(int index) => base[index] = null;
    }
}
