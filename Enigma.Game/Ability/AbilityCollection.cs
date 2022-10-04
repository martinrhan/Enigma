using Enigma.Common.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Enigma.Game {
    public class AbilityCollection : SlottedCollection<Ability> {
        private AbilityCollection(IEnumerable<Ability> abilities) : base(abilities) { }

        [SourceGenerator.JsonConstructor]
        public static AbilityCollection New(IEnumerable<AbilityCollectionConstructorElement> elements) {
            return new AbilityCollection(elements.Select(element => AbilityTemplate.Dictionary[element.AbilityTemplateId].New()));
        }

        public AbilityCollection DeepCopy() {
            return new AbilityCollection(this.Select(ability => ability.Template.New()));
        }

        internal new void IncreaseCount(int amount) => base.IncreaseCount(amount);
        internal void PlaceAt(int index, Ability ability) => this[index] = ability;
        internal void RemoveAt(int index) => base[index] = null;
        internal new void InternalExchange(int indexA, int indexB) {
            base.InternalExchange(indexA, indexB);
        }
    }

    public class AbilityCollectionConstructorElement {
        public string AbilityTemplateId { get; init; }
    }
}
