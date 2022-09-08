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
            public Vector2 Intersection { get; init; }
            public double RayATraveledDistance { get; init; }
            public double RayBTraveledDistance { get; init; }
        }

        public double GetTravelDistance(in Ray rayTo) {
            Matrix22 matrix22 = new Matrix22(
                this.DirectionalVector.X, -rayTo.DirectionalVector.X,
                this.DirectionalVector.Y, -rayTo.DirectionalVector.Y
                );
            Matrix21 matrix21 = new Matrix21(
                rayTo.Origin.X - this.Origin.X,
                rayTo.Origin.Y - this.Origin.Y
                );
            Matrix21 result = matrix22.Inverse() * matrix21;
            return result[0];
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
                Intersection = new Vector2(rayA.Origin.X + result[0] * rayA.DirectionalVector.X, rayA.Origin.Y + result[0] * rayA.DirectionalVector.Y),
                RayATraveledDistance = result[0],
                RayBTraveledDistance = result[1]
            };
        }
    }
}
