using Enigma.Common.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedWPF;

namespace Enigma.Spacial.TestWPF.Visual {
    public class CircleShapedSettingsViewModel : ViewModel {
        private int radius;
        public int Radius {
            get { return radius; }
            set {
                radius = value;
                NotifyPropertyChanged();
                model.Shape = model.Shape.ChangeRadius(radius);
            }
        }

        private Vector2 center;
        public Vector2 Center {
            get { return center; }
            set {
                center = value;
                NotifyPropertyChanged();
                model.Shape = model.Shape.ChangeCenter(center);
            }
        }

        private CircleShapedObject model;
        public void AssignModel(CircleShapedObject circleShaped) {
            this.model = circleShaped;
        }
    }
}
