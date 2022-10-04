using Enigma.Common.Math;
using Enigma.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Game {
    public class Player {
        internal Player(PlayerGameBodyBehaviour playerGameBodyBehaviour, GameBody playerGameBody) {
            PlayerGameBodyBehaviour = playerGameBodyBehaviour;
            PlayerGameBody = playerGameBody;
        }
        public PlayerGameBodyBehaviour PlayerGameBodyBehaviour { get; } = new PlayerGameBodyBehaviour();
        public Inventory Inventory { get; } = new Inventory();
        public Shop Shop { get; } = new Shop();
        public GameBody PlayerGameBody { get; }

        public void InventoryInternalExchange(int indexFrom, int indexTo) {
            Inventory.InternalExchange(indexFrom, indexTo);
        }
        public void AbilityCollectionInternalExchange(int indexFrom, int indexTo) {
            PlayerGameBody.AbilityCollection.InternalExchange(indexFrom, indexTo);
        }
        public void Exchange_Inventory_AbilityCollection(int index_inventory, int index_abilityCollection) {
            Inventory.ExchangeWithAbilityCollection(PlayerGameBody.AbilityCollection, index_inventory, index_abilityCollection);
        }
        public void SellAbilityItem(int itemIndex) {
            Inventory.Gold += Inventory[itemIndex].Price;
            Inventory.RemoveAt(itemIndex);
        }
        public void SellAbilityItemFromAbilityCollection(int itemIndex) {
            Inventory.Gold += PlayerGameBody.AbilityCollection[itemIndex].Template.Price;
            PlayerGameBody.AbilityCollection.RemoveAt(itemIndex);
        }
        public void BuyAbilityItem(int shopItemIndex, int inventorySlotIndex) {
            Inventory.Gold -= Shop.ItemList[shopItemIndex].Price;
            if (Inventory[inventorySlotIndex] != null) throw new InvalidOperationException();
            Inventory.PlaceAt(inventorySlotIndex, Shop.itemList[shopItemIndex]);
            Shop.itemList[shopItemIndex] = null;
        }
        public void BuyAbilityItemToAbilityCollection(int shopItemIndex, int abilityCollectionSlotIndex) {
            Inventory.Gold -= Shop.ItemList[shopItemIndex].Price;
            if (PlayerGameBody.AbilityCollection[abilityCollectionSlotIndex] != null) throw new InvalidOperationException();
            PlayerGameBody.AbilityCollection.PlaceAt(abilityCollectionSlotIndex, Shop.itemList[shopItemIndex].ConvertToAblity());
            Shop.itemList[shopItemIndex] = null;
        }

        public void ResizeAbilityCollection(int targetCount) {
            if (targetCount > PlayerGameBody.AbilityCollection.Count) {
                PlayerGameBody.AbilityCollection.IncreaseCount(targetCount - PlayerGameBody.AbilityCollection.Count);
            }
        }
    }
}
