using Enigma.Common.Collections;
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
    }

    public class AbilityCollectionConstructorElement {
        public string AbilityTemplateId { get; init; }
    }
}
