using System.Collections.Generic;

namespace Enigma.Game {
    public class AIGameBodyTemplateSet {
        private AIGameBodyTemplateSet(string id) {
            Id = id;
        }

        public string Id { get; }
        internal readonly HashSet<AIGameBodyTemplate> content = new HashSet<AIGameBodyTemplate>();
        public IReadOnlySet<AIGameBodyTemplate> Content => content;

        [SourceGenerator.JsonConstructor]
        public static AIGameBodyTemplateSet GetExistingOrNew(string id) {
            AIGameBodyTemplateSet set;
            if (dictionary.TryGetValue(id, out set)) {
                return set;
            } else {
                set = new AIGameBodyTemplateSet(id);
                dictionary.Add(id, set);
                return set;
            }
        }
        private static Dictionary<string, AIGameBodyTemplateSet> dictionary = new Dictionary<string, AIGameBodyTemplateSet>();
    }
}

