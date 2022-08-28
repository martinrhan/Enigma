using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enigma.Common.Math;
using ExtendedWPF;

namespace Enigma.Spacial.TestWPF.Visual {
    public class RectangleShapedSettingsViewModel : ViewModel {
        public RectangleShapedSettingsViewModel(int messengerToken) {
            this.messengerToken = messengerToken;
        }
        private int messengerToken;

        public Vector2 P0 {
            get { return Model.Shape.Points[0]; }
            set {
                Model.Shape = Model.Shape.Translate(value - P0);
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(P1));
                NotifyPropertyChanged(nameof(P2));
                NotifyPropertyChanged(nameof(P3));
            }
        }
        public Vector2 P1 {
            get { return Model.Shape.Points[1]; }
        }
        public Vector2 P2 {
            get { return Model.Shape.Points[2]; }
        }
        public Vector2 P3 {
            get { return Model.Shape.Points[3]; }
        }
        public RectangleShapedObject Model { get; private set; }
        public void AssignModel(RectangleShapedObject model) {
            Model = model;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(P1));
            NotifyPropertyChanged(nameof(P2));
            NotifyPropertyChanged(nameof(P3));
        }
    }
}
