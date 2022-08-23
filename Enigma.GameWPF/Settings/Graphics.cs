using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Enigma.GameWPF.Settings {
    public static class Graphics {
        public static void ReadJson() {
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Settings\Graphics.json";
            string text = File.ReadAllText(filePath);
            var jsonObject = JsonSerializer.Deserialize<JsonObject>(text);
            ScreenRatio = ScreenRatio.Parse(jsonObject.ScreenRatio);
            FullScreen = jsonObject.FullScreen;
        }

        public static ScreenRatio ScreenRatio { get; set; }
        public static bool FullScreen { get; set; }

        public static void WriteJson() {
            JsonObject jsonObject = new JsonObject();
            jsonObject.ScreenRatio = ScreenRatio.ToString();
            jsonObject.FullScreen = FullScreen;
            string text = JsonSerializer.Serialize(jsonObject);
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Settings\Interface.json";
            File.WriteAllText(filePath, text);
        }

        private class JsonObject {
            public string ScreenRatio { get; set; }
            public bool FullScreen { get; set; }
        }
    }
}
