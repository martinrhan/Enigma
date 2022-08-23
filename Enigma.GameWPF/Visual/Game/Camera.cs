using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enigma.Common.Math;
using Enigma.Game;
using Enigma.GameWPF.Visual.Settings.Graphics;
using Enigma.Spacial;
using static Enigma.Game.Extensions.GameWorldExtensions;

namespace Enigma.GameWPF.Visual.Game {
    public class Camera {
        public Camera(GameWorld gameWorld) {
            this.gameWorld = gameWorld;
            LowerBound = Vector2.Zero;
            Width = GameWPF.Settings.Graphics.ScreenRatio.Width * 100;
            Height = GameWPF.Settings.Graphics.ScreenRatio.Height * 100;
            UpperBound = new Vector2(LowerBound.X + Width, LowerBound.Y + Height);
        }
        private readonly GameWorld gameWorld;
        public Vector2 LowerBound { get; private set; }
        public Vector2 UpperBound { get; private set; }

        public AABB AABB => new AABB(LowerBound, UpperBound);

        public double Width { get; private set; }
        public double Height { get; private set; }

        public GameBody NeedMoveTo { get; set; } 

        public Vector2 Center {
            get => new Vector2(LowerBound.X + Width / 2, LowerBound.Y + Height / 2);
            set {
                LowerBound = new Vector2(value.X - Width / 2, value.Y - Height / 2);
                LowerBound = gameWorld.GetClampedPosition(LowerBound);
                UpperBound = new Vector2(LowerBound.X + Width, LowerBound.Y + Height);
            }
        }

        public double WidthLowerLimit { get; set; } = 100;
        private double WidthUpperLimit => gameWorld.Width;

        private double movementSpeed = 50;
        public void LoadSetting() {

        }
        public void Translate(double speedMultiplierX, double speedMultiplierY) {
            double translationX = movementSpeed * speedMultiplierX;
            double translationY = movementSpeed * speedMultiplierY;
            Vector2 translation = new Vector2(translationX, translationY);
            LowerBound = gameWorld.GetClampedPosition(LowerBound + translation);
            UpperBound = gameWorld.GetClampedPosition(UpperBound + translation);
        }
        public void ZoomIn(double delta) {
            double newWidth = Width - delta * Width;
            if (newWidth < WidthLowerLimit) {
                newWidth = WidthLowerLimit;
                delta = (Width - newWidth) / Width;
            } else if (WidthUpperLimit < newWidth) {
                newWidth = WidthUpperLimit;
                delta = (Width - newWidth) / Width;
            }

            Vector2 lowerBoundToUpperBound = UpperBound - LowerBound;
            Vector2 lowerBoundMovement = lowerBoundToUpperBound * delta / 2;
            Vector2 upperBoundMovement = -lowerBoundMovement;
            Vector2 newLowerBound = LowerBound + lowerBoundMovement;
            Vector2 newUpperBound = UpperBound + upperBoundMovement;
            LowerBound = newLowerBound;
            UpperBound = newUpperBound;
            Width = newWidth;
            Height = UpperBound.Y - LowerBound.Y;
        }
    }
}
