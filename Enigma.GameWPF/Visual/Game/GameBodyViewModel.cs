using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enigma.Common.Math;
using Enigma.Game;

namespace Enigma.GameWPF.Visual.Game {
    public class GameBodyViewModel {
        public double CenterX { get; private set; }
        public double CenterY { get; private set; }
        public double Radius { get; private set; }

        public void UpdateDataFromModel(GameBody model, Camera camera) {
            (double objectRelativeCenterX, double objectRelativeCenterY) = model.Center - camera.LowerBound;
            if (objectRelativeCenterX + model.AABB.Width < 0) {
                objectRelativeCenterX += model.GameWorld.Width;
            }
            if (objectRelativeCenterY + model.AABB.Height < 0) {
                objectRelativeCenterY += model.GameWorld.Height;
            }
            CenterX = objectRelativeCenterX;
            CenterY = objectRelativeCenterY;
            Radius = model.Radius;
        }
    }
}
