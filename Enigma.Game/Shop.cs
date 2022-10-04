using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Game {
    public class Shop : RarityObjectRoller<AbilityTemplate> {
        internal Shop() {
            ItemCount = 3;
            RollItems();
        }
        public int ItemCount { get; set; } 
        internal readonly List<AbilityItem> itemList = new List<AbilityItem>();
        public IReadOnlyList<AbilityItem> ItemList => itemList;
        public void RollItems() {
            itemList.Clear();
            for (int i = 0; i < ItemCount; i++) {
                AbilityItem rolledItem;
                do {
                    rolledItem = new(RollItem_Protected());
                } while (itemList.Contains(rolledItem));
                itemList.Add(rolledItem);
            }
        }
    }

    /// <summary>
    /// Yet not equiped ability, sold in shop or stored in inventory.
    /// </summary>
    public class AbilityItem {
        internal AbilityItem(AbilityTemplate template) {
            Template = template;
        }
        public int Price => Template.Price;
        public AbilityTemplate Template { get; }
        public Ability ConvertToAblity() => new Ability(Template);
    }

}
