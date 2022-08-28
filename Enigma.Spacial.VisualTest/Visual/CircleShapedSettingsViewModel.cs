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
        public CircleShapedSettingsViewModel(int messengerToken) {
            this.messengerToken = messengerToken;
        }
        private readonly int messengerToken;

        public double Radius {
            get => Model == null ? 0 : Model.Shape.Radius;
            set {
                if (Model == null) return;
                Model.Shape = Model.Shape.ChangeRadius(value);
                NotifyPropertyChanged();
                WeakReferenceMessenger.Default.Send(new ShapedObjectModelChangedMessage() { ShapedObject = Model }, messengerToken);
            }
        }

        public Vector2 Center {
            get => Model == null ? Vector2.Zero : Model.Shape.Center; 
            set {
                if (Model == null) return;
                Model.Shape = Model.Shape.ChangeCenter(value);
                NotifyPropertyChanged();
                WeakReferenceMessenger.Default.Send(new ShapedObjectModelChangedMessage() { ShapedObject = Model }, messengerToken);
            }
        }

        public CircleShapedObject Model { get; private set; }
        public void AssignModel(CircleShapedObject model) {
            Model = model;
            NotifyPropertyChanged(null);
            WeakReferenceMessenger.Default.Send(new ShapedObjectModelChangedMessage() { ShapedObject = Model }, messengerToken);
        }
    }
}
