using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.GameWPF.Settings {
    public class ScreenRatio {
        private ScreenRatio(int width, int height) {
            Width = width;
            Height = height;
        }
        public int Width { get; }
        public int Height { get; }
        public override string ToString() {
            return Width.ToString() + ":" + Height.ToString();
        }
        public static ScreenRatio Parse(string str) {
            string[] strs = str.Split(':');
            int width = int.Parse(strs[0]);
            int height = int.Parse(strs[1]);
            foreach (ScreenRatio ratio in supportedScreenRatios) {
                if (ratio.Width == width && ratio.Height == height) return ratio;
            }
            return supportedScreenRatios[0];
        }
        private static ScreenRatio[] supportedScreenRatios { get; } = new ScreenRatio[] {
            new ScreenRatio(16, 9),
            new ScreenRatio(16, 10)
        };
        public static IReadOnlyList<ScreenRatio> SupportedScreenRatios => supportedScreenRatios;
    }
}
