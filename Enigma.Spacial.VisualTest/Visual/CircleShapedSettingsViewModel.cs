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
            get { return Model.Shape.Radius; }
            set {
                Model.Shape = Model.Shape.ChangeRadius(value);
                NotifyPropertyChanged();
                WeakReferenceMessenger.Default.Send(new ShapedObjectModelChangedMessage() { ShapedObject = Model }, messengerToken);
            }
        }

        public Vector2 Center {
            get { return Model.Shape.Center; }
            set {
                Model.Shape = Model.Shape.ChangeCenter(value);
                NotifyPropertyChanged();
                WeakReferenceMessenger.Default.Send(new ShapedObjectModelChangedMessage() { ShapedObject = Model }, messengerToken);
            }
        }

        public CircleShapedObject Model { get; private set; }
        public void AssignModel(CircleShapedObject model) {
            Model = model;
            NotifyPropertyChanged(nameof(Radius));
            NotifyPropertyChanged(nameof(Center));
            WeakReferenceMessenger.Default.Send(new ShapedObjectModelChangedMessage() { ShapedObject = Model }, messengerToken);
        }
    }
}
