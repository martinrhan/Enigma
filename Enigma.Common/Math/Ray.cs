using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Common.Math {
    public struct Ray {
        public Ray(in Vector2 origin, in Vector2 directionalVector) {
            Origin = origin;
            DirectionalVector = directionalVector.ChangeLength(1);
        }
        public Vector2 Origin { get; }
        public Vector2 DirectionalVector { get; }

        public readonly struct GetIntersectionResult {
            public double X { get; init; }
            public double Y { get; init; }
            public double RayATravelDistance { get; init; }
            public double RayBTravelDistance { get; init; }
        }

        public static GetIntersectionResult GetIntersection(in Ray rayA, in Ray rayB) {
            Matrix22 matrix22 = new Matrix22(
                rayA.DirectionalVector.X, -rayB.DirectionalVector.X,
                rayA.DirectionalVector.Y, -rayB.DirectionalVector.Y
                );
            Matrix21 matrix21 = new Matrix21(
                rayB.Origin.X - rayA.Origin.X,
                rayB.Origin.Y - rayA.Origin.Y
                );
            Matrix21 result = matrix22.Inverse() * matrix21;
            return new GetIntersectionResult() {
                X = rayA.Origin.X + result[0] * rayA.DirectionalVector.X,
                Y = rayA.Origin.Y + result[0] * rayA.DirectionalVector.Y,
                RayATravelDistance = result[0],
                RayBTravelDistance = result[1]
            };
        }
        public static double GetTravelDistance(in Ray rayTraveller, in Ray rayTo) {
            Matrix22 matrix22 = new Matrix22(
                rayTraveller.DirectionalVector.X, -rayTo.DirectionalVector.X,
                rayTraveller.DirectionalVector.Y, -rayTo.DirectionalVector.Y
                );
            Matrix21 matrix21 = new Matrix21(
                rayTo.Origin.X - rayTraveller.Origin.X,
                rayTo.Origin.Y - rayTraveller.Origin.Y
                );
            Matrix21 result = matrix22.Inverse() * matrix21;
            return result[0];
        }
    }
}
