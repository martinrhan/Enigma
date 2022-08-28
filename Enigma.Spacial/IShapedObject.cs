using Enigma.Spacial;
using Enigma.Common.Math;
using System.Collections.Generic;
using System;

namespace Enigma.Spacial {
    public interface IShapedObject {
        public AABB AABB { get; }
    }

    public interface IAABBShapedObject : IShapedObject {
        public AABB Shape { get; }
    }

    public interface ICircleShapedObject : IShapedObject {
        public Circle Shape { get; }
    }

    public interface IRectangleShapedObject : IShapedObject {
        public Rectangle Shape { get; set; }
    }

    public interface ICircularSectorShapedObject : IShapedObject {
        public CircularSector Shape { get; set; }
    }
}
