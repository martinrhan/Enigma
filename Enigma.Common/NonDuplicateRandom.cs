using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Common {
    public class NonDuplicateRandom {
        public NonDuplicateRandom(int maxExclusive) {
            list = new List<int>(Enumerable.Range(0, maxExclusive));
        }

        private readonly List<int> list;
        private readonly Random random = new Random();
        public int Next() {
            int rolledIndex = random.Next(list.Count);
            int rolledValue = list[rolledIndex];
            try {
                list.RemoveAt(rolledIndex);
            } catch (ArgumentOutOfRangeException) {
                throw new InvalidOperationException();
            }
            return rolledValue;
        }
        public IEnumerable<int> GetRandomIntegers(int count) {
            for (int i = 0; i < count; i++) {
                yield return Next();
            }
        }
    }


}
