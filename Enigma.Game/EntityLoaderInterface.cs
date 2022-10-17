using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Enigma.Common;
using Enigma.Game.Json;

namespace Enigma.Game {
    /// <summary>
    /// Provides interface methods for UI application to add entities such as AIGameBodyBehaviourFactory, etc.
    /// </summary>
    public static class EntityLoaderInterface {
        public static void RegisterAIGameBodyBehaviourFactory(AIGameBodyBehaviourFactory factory) {
            AIGameBodyBehaviourFactory.Register(factory);
        }
        public static void RegisterAbilityMechanismFactory(AbilityMechanismFactory factory) {
            AbilityMechanismFactory.Register(factory);
        }
        public static void RegisterEnemyWaveBehaviourFactory(EnemyWaveBehaviourFactory factory) {
            EnemyWaveBehaviourFactory.Register(factory);
        }

        public static AbilityTemplate DeserializeAbilityTemplate(string jsonString) {
            return JsonSerializer.Deserialize<AbilityTemplateJsonObject>(jsonString).Convert();
        }
        public static void RegisterAbilityTemplate(AbilityTemplate template) {
            AbilityTemplate.dictionary.Add(template.Id, template);
            RarityObjectRoller<AbilityTemplate>.Register(template);
        }

        public static AIGameBodyTemplate DeserializeAIGameBodyTemplate(string jsonString) {
            return JsonSerializer.Deserialize<AIGameBodyTemplateJsonObject>(jsonString).Convert();
        }
        public static void RegisterAIGameBodyTemplate(AIGameBodyTemplate template) {
            AIGameBodyTemplate.dictionary.Add(template.Id, template);
            foreach (AIGameBodyTemplateSet set in template.ParentSetList) {
                set.content.Add(template);
            }
        }

        public static EnemyWaveTemplate DeserializeEnemyWaveTemplate(string jsonString) {
            return JsonSerializer.Deserialize<EnemyWaveTemplateJsonObject>(jsonString).Convert();
        }
        public static void RegisterEnemyWaveTemplate(EnemyWaveTemplate template) {
            EnemyWaveTemplate.dictionary.Add(template.Id, template);
            RarityObjectRoller<EnemyWaveTemplate>.Register(template);
        }
    }
}
