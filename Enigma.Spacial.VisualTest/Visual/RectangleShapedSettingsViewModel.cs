using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Enigma.Common.Math;
using ExtendedWPF;

namespace Enigma.Spacial.TestWPF.Visual {
    public class RectangleShapedSettingsViewModel : ViewModel {
        public RectangleShapedSettingsViewModel(int messengerToken) {
            this.messengerToken = messengerToken;
        }
        private int messengerToken;

        public Vector2 P0 {
            get => Model == null ? Vector2.Zero : Model.Shape.Points[0]; 
            set {
                if (Model == null) return;
                Model.Shape = Model.Shape.Translate(value - P0);
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(P1));
                NotifyPropertyChanged(nameof(P2));
                NotifyPropertyChanged(nameof(P3));
                WeakReferenceMessenger.Default.Send(new ShapedObjectModelChangedMessage() { ShapedObject = Model }, messengerToken);
            }
        }
        public Vector2 P1 => Model == null ? Vector2.Zero : Model.Shape.Points[1]; 
        public Vector2 P2 => Model == null ? Vector2.Zero : Model.Shape.Points[2];
        public Vector2 P3 => Model == null ? Vector2.Zero : Model.Shape.Points[3]; 

        public double Width {
            get => Model == null ? 0 : Model.Shape.Width;
            set {
                if (Model == null) return;
                Model.Shape = Model.Shape.ChangeWidth(value);
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(P1));
                NotifyPropertyChanged(nameof(P2));
                WeakReferenceMessenger.Default.Send(new ShapedObjectModelChangedMessage() { ShapedObject = Model }, messengerToken);
            }
        }
        public double Height {
            get => Model == null ? 0 : Model.Shape.Height;
            set {
                if (Model == null) return;
                Model.Shape = Model.Shape.ChangeHeight(value);
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(P3));
                NotifyPropertyChanged(nameof(P2));
                WeakReferenceMessenger.Default.Send(new ShapedObjectModelChangedMessage() { ShapedObject = Model }, messengerToken);
            }
        }

        public void InvokeRotation(double theta) {
            if (Model == null) return;
            Model.Shape = Model.Shape.Rotate(theta, Model.Shape.Points[0]);
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(P1));
            NotifyPropertyChanged(nameof(P2));
            NotifyPropertyChanged(nameof(P3));
            WeakReferenceMessenger.Default.Send(new ShapedObjectModelChangedMessage() { ShapedObject = Model }, messengerToken);
        }

        public RectangleShapedObject Model { get; private set; }
        public void AssignModel(RectangleShapedObject model) {
            Model = model;
            NotifyPropertyChanged(null);
            WeakReferenceMessenger.Default.Send(new ShapedObjectModelChangedMessage() { ShapedObject = Model }, messengerToken);
        }
    }
}
