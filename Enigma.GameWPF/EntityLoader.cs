using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Controls;
using Enigma.Game;

namespace Enigma.GameWPF {
    public static partial class EntityLoader {
        public static void Load() {
            LoadAssembly();
            LoadJson();
            LoadWPFEntities();
        }

        private static readonly string EntityDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private static void LoadAssembly() {
            Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(EntityDirectory + @"\Enigma.Game.Entities.dll");
            foreach (Type t in assembly.GetTypes()) {
                if (t.IsAssignableTo(typeof(AIGameBodyBehaviour))) {
                    var factory = Factory<AIGameBodyBehaviourFactoryArguments, AIGameBodyBehaviour>.LoadType(t, "Behaviour");
                    if (factory != null) EntityLoaderInterface.RegisterAIGameBodyBehaviourFactory(factory);
                } else if (t.IsAssignableTo(typeof(EnemyWaveBehaviour))) {
                    var factory = Factory<EnemyWaveBehaviourFactoryArguments, EnemyWaveBehaviour>.LoadType(t, "Behaviour");
                    if (factory != null) EntityLoaderInterface.RegisterEnemyWaveBehaviourFactory(factory);
                }else if (t.IsAssignableTo(typeof(AbilityMechanism))) {
                    var factory = Factory<AbilityMechanismFactoryArguments, AbilityMechanism>.LoadType(t, "Mechanism");
                    if (factory != null) EntityLoaderInterface.RegisterAbilityMechanismFactory(factory);
                }
            }
        }

        private static void LoadJson() {
            string text;
            JsonSerializerOptions options = new JsonSerializerOptions() {
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals
            };
            foreach (string filePath in Directory.GetFiles(EntityDirectory + @"\Data\AbilityTemplates", "*.json", SearchOption.AllDirectories)) {
                text = File.ReadAllText(filePath);
                JsonArray array = JsonSerializer.Deserialize<JsonArray>(text, options);
                foreach (JsonNode node in array) {
                    var template = EntityLoaderInterface.DeserializeAbilityTemplate(node.ToString());
                    EntityLoaderInterface.RegisterAbilityTemplate(template);
                }
            }
            foreach (string filePath in Directory.GetFiles(EntityDirectory + @"\Data\AIGameBodyTemplates", "*.json", SearchOption.AllDirectories)) {
                text = File.ReadAllText(filePath);
                JsonArray array = JsonSerializer.Deserialize<JsonArray>(text, options);
                foreach (JsonNode node in array) {
                    var template = EntityLoaderInterface.DeserializeAIGameBodyTemplate(node.ToString());
                    EntityLoaderInterface.RegisterAIGameBodyTemplate(template);
                }
            }
            foreach (string filePath in Directory.GetFiles(EntityDirectory + @"\Data\EnemyWaveTemplates", "*.json", SearchOption.AllDirectories)) {
                text = File.ReadAllText(filePath);
                JsonArray array = JsonSerializer.Deserialize<JsonArray>(text, options);
                foreach (JsonNode node in array) {
                    var template = EntityLoaderInterface.DeserializeEnemyWaveTemplate(node.ToString());
                    EntityLoaderInterface.RegisterEnemyWaveTemplate(template);
                }
            }
        }

        private static void LoadWPFEntities() {
            //Assembly assembly = null;
            Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(EntityDirectory + @"\Enigma.GameWPF.Visual.Entities.dll");
            foreach (Type t in assembly.GetTypes()) {
                if (t.IsAssignableTo(typeof(Visual.IView<Visual.Game.AbilityIconViewModel>)) && t.IsAssignableTo(typeof(ContentControl))) {
                    Visual.Game.AbilityIconView.Add(t);
                }
            }
        }
    }
}
