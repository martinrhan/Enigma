using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Enigma.GameWPF.Settings {
    public static class Interface {
        public static void ReadJson() {
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Settings\Interface.json";
            string text = File.ReadAllText(filePath);
            var jsonObject = JsonSerializer.Deserialize<JsonObject>(text);
            UICulture = UICulture.Parse(jsonObject.UICulture);
        }

        public static UICulture UICulture { get; set; }

        public static void WriteJson() {
            JsonObject jsonObject = new JsonObject();
            jsonObject.UICulture = UICulture.ToString();
            string text = JsonSerializer.Serialize(jsonObject);
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Settings\Interface.json";
            File.WriteAllText(filePath, text);
        }

        private class JsonObject {
            public string UICulture { get; set; }
        }
    }
}
