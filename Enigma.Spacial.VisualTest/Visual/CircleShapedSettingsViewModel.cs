using Enigma.Common.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedWPF;
using CommunityToolkit.Mvvm.Messaging;

namespace Enigma.Spacial.TestWPF.Visual {
    public class CircleShapedSettingsViewModel : ViewModel {
        private double radius;
        public double Radius {
            get { return radius; }
            set {
                radius = value;
                NotifyPropertyChanged();
                Model.Shape = Model.Shape.ChangeRadius(radius);
            }
        }

        private Vector2 center;
        public Vector2 Center {
            get { return center; }
            set {
                center = value;
                NotifyPropertyChanged();
                Model.Shape = Model.Shape.ChangeCenter(center);
            }
        }

        public CircleShapedObject Model { get; private set; }
        public void AssignModel(CircleShapedObject model) {
            Model = model;
            radius = Model.Shape.Radius;
            NotifyPropertyChanged(nameof(Radius));
            center = model.Shape.Center;
            NotifyPropertyChanged(nameof(Center));
        }
    }
}
