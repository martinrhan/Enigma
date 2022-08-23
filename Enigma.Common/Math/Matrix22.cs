using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Common.Math {
    public unsafe struct Matrix22 {
        public Matrix22(double a, double b, double c, double d) {
            buffer[0] = a;
            buffer[1] = b;
            buffer[2] = c;
            buffer[3] = d;
        }
        internal fixed double buffer[4];
        public double this[byte index] => 4 < index ? throw new ArgumentOutOfRangeException() : buffer[index];
        public double this[byte indexX, byte indexY] {
            get {
                if (2 <= indexX || 2 <= indexY) {
                    throw new ArgumentOutOfRangeException();
                }
                return buffer[indexX * 2 + indexY];
            }
        }

        public static Matrix22 operator *(in Matrix22 matrix22, double scalar) => new Matrix22(
                matrix22.buffer[0] * scalar, matrix22.buffer[1] * scalar,
                matrix22.buffer[2] * scalar, matrix22.buffer[3] * scalar
                );

        public static Matrix22 operator /(in Matrix22 matrix22, double scalar) => new Matrix22(
                matrix22.buffer[0] * scalar, matrix22.buffer[1] * scalar,
                matrix22.buffer[2] * scalar, matrix22.buffer[3] * scalar
                );

        public static Matrix21 operator *(in Matrix22 matrix22, in Matrix21 matrix21) => new Matrix21(
                matrix22.buffer[0] * matrix21.buffer[0] + matrix22.buffer[1] * matrix21.buffer[1],
                matrix22.buffer[2] * matrix21.buffer[0] + matrix22.buffer[3] * matrix21.buffer[1]
                );

        public Matrix22 Inverse() {
            double determinant = buffer[0] * buffer[3] - buffer[1] * buffer[2];
            return new Matrix22(
                buffer[3] / determinant, -buffer[1] / determinant,
                -buffer[2] / determinant, buffer[0] / determinant
                );
        }
    }
}
