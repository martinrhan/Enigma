using Enigma.Common.Collections;

namespace Enigma.Game {
    public sealed class Inventory : SlottedCollection<AbilityItem> {
        internal Inventory() : base(10) { }

        public int Gold { get; internal set; } = 100;

        public new void InternalExchange(int indexFrom, int indexTo) {
            base.InternalExchange(indexFrom, indexTo);
        }
        public void ExchangeWithAbilityCollection(AbilityCollection abilityCollection,int indexFrom, int indexTo) {
            ExternalExchange(indexFrom, indexTo, abilityCollection, abilityItem => abilityItem.ConvertToAblity(), ability => ability.ConvertToAbilityItem());
        }
        internal new void PlaceAtFirstEmptySlot(AbilityItem item) {
            base.PlaceAtFirstEmptySlot(item);
        }
        public void SellAbilityItem(int itemIndex) {

        }
        public void BuyAbilityItem(int itemIndex, Shop shop) {
            shop.SellAbilityItem(itemIndex, this);
        }
    }
}
