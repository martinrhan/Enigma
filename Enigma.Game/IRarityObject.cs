using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Game {
    public interface IRarityObject {
        public Rarity Rarity { get; }
    }

    public enum Rarity { NotForPlayer, Common, Rare, Mythic }

    public abstract class RarityObjectRoller<T> where T : IRarityObject {
        static RarityObjectRoller() {
            foreach (Rarity r in Enum.GetValues(typeof(Game.Rarity))) {
                RarityListDictionary.Add(r, new List<T>());
            }
        }
        internal RarityObjectRoller() {

        }
        protected Dictionary<Rarity, int> RarityValueDictionary = new Dictionary<Rarity, int>() {
                {Rarity.Mythic, 1 },
                {Rarity.Rare, 5 },
                {Rarity.Common, 25 }
            };

        private static readonly Dictionary<Rarity, List<T>> RarityListDictionary = new Dictionary<Rarity, List<T>>();
        private readonly Random random = new Random();
        protected T RollItem_Protected() {
            start:
            int rand = random.Next(0, RarityValueDictionary.Values.Sum());
            int accumulated = 0;
            Rarity rolledRarity = default;
            foreach (Rarity rarity in RarityValueDictionary.Keys) {
                accumulated += RarityValueDictionary[rarity];
                if (rand < accumulated) {
                    rolledRarity = rarity;
                    IReadOnlyList<T> list = RarityListDictionary[rolledRarity];
                    if (list.Count == 0) goto start;
                    return list[random.Next(0, list.Count - 1)];
                }
            }
            throw new Exception();
        }

        internal static void Register(T item) {
            RarityListDictionary[item.Rarity].Add(item);
        }
    }
}
