using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.GameWPF.Visual.Game {
    public class MinimapItemViewModel : ManualNotifyChangedViewModel {
        public double LowerBoundX { get; private set; }
        public double LowerBoundY { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }

        public void UpdateDataFromModel(MinimapItem model, double ratio) {
            LowerBoundX = model.GameBody.AABB.LowerBound.X * ratio;
            LowerBoundY = model.GameBody.AABB.LowerBound.Y * ratio;
            Width = model.GameBody.AABB.Width * ratio;
            Height = model.GameBody.AABB.Height * ratio;
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
