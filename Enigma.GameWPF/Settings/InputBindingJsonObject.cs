using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Windows.Input;

namespace Enigma.GameWPF.Settings {
    public class InputBindingJsonObject {
        private InputBindingJsonObject Rectify() {
            HashSet<string> usedKey = new HashSet<string>();
            foreach (string key in BuiltInInputActionBindings.Keys) {
                if (usedKey.Contains(BuiltInInputActionBindings[key])) {
                    BuiltInInputActionBindings[key] = null;
                } else {
                    usedKey.Add(key);
                }
            }
            for (int i = 0; i < SelectAbilityInputActionBindings.Length; i++) {
                if (usedKey.Contains(SelectAbilityInputActionBindings[i])) {
                    SelectAbilityInputActionBindings[i] = null;
                } else {
                    usedKey.Add(SelectAbilityInputActionBindings[i]);
                }
            }
            return this;
        }
        private static string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Settings\InputBinding.json";

        public Dictionary<string, string> BuiltInInputActionBindings { get; set; }
        public string[] SelectAbilityInputActionBindings { get; set; }

        public static InputBindingJsonObject Read() {
            string text = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<InputBindingJsonObject>(text).Rectify();
        }
        public void Write() {
            string text = JsonSerializer.Serialize(this.Rectify());
            File.WriteAllText(filePath, text);
        }
    }
}
