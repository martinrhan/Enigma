using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Common {
    public static class MyMethods {
        public static void SwapValue<T>(ref T a, ref T b) {
            T temp = a;
            a = b;
            b = temp;
        }
        public static IEnumerable<string> SplitPascalCase(string str) {
            int lastCapitalIndex = 0;
            int i;
            for (i = 1; i < str.Length; i++) {
                if (char.IsUpper(str[i])) {
                    yield return str[lastCapitalIndex..i];
                    lastCapitalIndex = i;
                }
            }
            yield return str[lastCapitalIndex..i];
        }
    }
}
