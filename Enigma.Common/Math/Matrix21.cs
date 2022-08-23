using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Common.Math {
    public unsafe struct Matrix21 {
        public Matrix21(double a, double b) {
            buffer[0] = a;
            buffer[1] = b;
        }
        internal fixed double buffer[2];
        public double this[byte index] => 1 < index ? throw new ArgumentOutOfRangeException() : buffer[index];
        public double this[byte indexX, byte indexY] {
            get {
                if (1 < indexX || indexY != 0) {
                    throw new ArgumentOutOfRangeException();
                }
                return buffer[indexX];
            }
        }
    }
}
