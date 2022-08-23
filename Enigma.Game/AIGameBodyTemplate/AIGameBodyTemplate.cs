using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Enigma.Common;
using Enigma.Common.Math;
using Enigma.Game;

namespace Enigma.Game {
    public class AIGameBodyTemplate {
        internal readonly static Dictionary<string, AIGameBodyTemplate> dictionary = new Dictionary<string, AIGameBodyTemplate>();
        public static IReadOnlyDictionary<string, AIGameBodyTemplate> Dictionary => dictionary;

        public string Id { get; init; }
        public IReadOnlyList<AIGameBodyTemplateSet> ParentSetList { get; init; }
        public AbilityCollection AbilityCollection { get; init; }
        public AIGameBodyBehaviourFactory AIGameBodyBehaviourFactory { get; init; }
        public GameBody.PropertyBuffer Properties { get; init; }
    }
}

