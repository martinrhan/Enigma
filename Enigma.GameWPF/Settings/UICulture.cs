using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.GameWPF.Settings {
    public class UICulture {
        private UICulture() { }
        public static UICulture Parse(string str) { 
            if (str == "System") {
                return new UICulture() {
                    UseSystemUICulture = true
                };
            } else {
                return new UICulture() {
                    UseSystemUICulture = false,
                    SelectedCulture = ParseCultureInfo(str)
                };
            }
        }
        private static CultureInfo ParseCultureInfo(string str) {
            for (int i = 0; i < SupportedCultures.Count; i++) {
                if (SupportedCultures[i].Name == str) return SupportedCultures[i];
            }
            return SupportedCultures[0];
        }

        public bool UseSystemUICulture { get; set; }
        public CultureInfo SelectedCulture { get; set; }

        public override string ToString() {
            if (UseSystemUICulture) return "System";
            return SelectedCulture.Name;
        }

        public static IReadOnlyList<CultureInfo> SupportedCultures { get; } = new CultureInfo[] {
            new CultureInfo("en-GB"),
            new CultureInfo("zh-Hans")
        };
    }
}
