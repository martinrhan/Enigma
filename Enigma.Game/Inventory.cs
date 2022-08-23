using Enigma.Common.Collections;

namespace Enigma.Game {
    public sealed class Inventory : SlottedCollection<AbilityItem> {
        internal Inventory(AbilityCollection abilityCollection) : base(10) {
            this.abilityCollection = abilityCollection;
        }
        private readonly AbilityCollection abilityCollection;

        public int Gold { get; internal set; }

        public new void InternalExchange(int indexFrom, int indexTo) {
            base.InternalExchange(indexFrom, indexTo);
        }
        public void ExchangeWithAbilityCollection(int indexFrom, int indexTo) {
            ExternalExchange(indexFrom, indexTo, abilityCollection, abilityItem => abilityItem.ConvertToAblity(), ability => ability.ConvertToAbilityItem());
        }
        internal new void PlaceAtFirstEmptySlot(AbilityItem item) {
            base.PlaceAtFirstEmptySlot(item);
        }
    }
}
