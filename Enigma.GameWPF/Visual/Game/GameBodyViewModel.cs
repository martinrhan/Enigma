using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enigma.Common.Math;
using Enigma.Game;

namespace Enigma.GameWPF.Visual.Game {
    public class GameBodyViewModel : ManualNotifyChangedViewModel {
        public double LowerBoundX { get; private set; }
        public double LowerBoundY { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }

        public void UpdateDataFromModel(GameBody model, GameBodyCollectionViewModel collectionViewModel, Camera camera) {
            (double objectRelativeLowerBoundX, double objectRelativeLowerBoundY) = model.AABB.LowerBound - camera.LowerBound;
            if (objectRelativeLowerBoundX + model.AABB.Width < 0) {
                objectRelativeLowerBoundX += model.GameWorld.Width;
            }
            if (objectRelativeLowerBoundY + model.AABB.Height < 0) {
                objectRelativeLowerBoundY += model.GameWorld.Height;
            }
            double viewObjectRatioX = collectionViewModel.ViewWidth / camera.Width;
            double viewObjectRatioY = collectionViewModel.ViewHeight / camera.Height;
            LowerBoundX = objectRelativeLowerBoundX * viewObjectRatioX;
            LowerBoundY = objectRelativeLowerBoundY * viewObjectRatioY;
            Width = model.AABB.Width * viewObjectRatioX;
            Height = model.AABB.Height * viewObjectRatioY;
        }
        public override void NotifyChanged() {
            NotifyPropertyChanged(nameof(IsEmpty));
            NotifyPropertyChanged(nameof(LowerBoundX));
            NotifyPropertyChanged(nameof(LowerBoundY));
            NotifyPropertyChanged(nameof(Width));
            NotifyPropertyChanged(nameof(Height));
        }
    }
}
