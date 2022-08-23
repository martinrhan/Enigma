using Enigma.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Enigma.GameWPF.Visual.Game {
    public class MinimapViewModel : CollectionViewModel<MinimapItemViewModel, MinimapItem> {
        public MinimapViewModel() {

        }
        public MinimapView View { get; set; }
        public double ViewWidth => View.ActualWidth;
        public double ViewHeight => View.ActualHeight;
        public double OccupiedViewWidth { get; private set; }
        public double OccupiedViewHeight { get; private set; }
        public double CameraIndicatorLowerBoundX { get; private set; }
        public double CameraIndicatorLowerBoundY { get; private set; }
        public double CameraIndicatorWidth { get; private set;}
        public double CameraIndicatorHeight { get; private set; }

        public void UpdateDataFromModel(GameWorld gameWorld, Camera camera) {
            double viewObjectRatioX = ViewWidth / gameWorld.Width;
            double viewObjectRatioY = ViewHeight / gameWorld.Height;
            double lowerRatio = Math.Max(viewObjectRatioX, viewObjectRatioY);
            OccupiedViewWidth = gameWorld.Width * lowerRatio;
            OccupiedViewHeight = gameWorld.Height * lowerRatio;
            CameraIndicatorLowerBoundX = camera.LowerBound.X * lowerRatio;
            CameraIndicatorLowerBoundY = camera.LowerBound.Y * lowerRatio;
            CameraIndicatorWidth = camera.Width * lowerRatio;
            CameraIndicatorHeight = camera.Height * lowerRatio;
            UpdateDataFromModel_Protected(from g in gameWorld.GameBodies select new MinimapItem(g), (viewModel, model) => { 
                viewModel.UpdateDataFromModel(model, lowerRatio);
            });
        }

        public override void NotifyChanged() {
            base.NotifyChanged();
            NotifyPropertyChanged(nameof(OccupiedViewWidth));
            NotifyPropertyChanged(nameof(OccupiedViewHeight));
            NotifyPropertyChanged(nameof(CameraIndicatorLowerBoundX));
            NotifyPropertyChanged(nameof(CameraIndicatorLowerBoundY));
            NotifyPropertyChanged(nameof(CameraIndicatorWidth));
            NotifyPropertyChanged(nameof(CameraIndicatorHeight));
        }
    }
}
