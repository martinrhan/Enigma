using CommunityToolkit.Mvvm.Messaging;
using Enigma.Common.Math;
using Enigma.Spacial.TestWPF.Models;

namespace Enigma.Spacial.TestWPF.Visual {

    public class RectangleShapedSettingsViewModel : ViewModel {

        public RectangleShapedSettingsViewModel(int messengerToken) {
            this.messengerToken = messengerToken;
        }

        private int messengerToken;

        private void SendShapedObjectModelChangedMessage() {
            WeakReferenceMessenger.Default.Send(new ShapedObjectModelChangedMessage() { TestSpace = TestSpace, ShapedObject = Model }, messengerToken);
        }

        public Vector2 P0 {
            get => Model == null ? Vector2.Zero : Model.Shape.Points[0];
            set {
                if (Model == null) return;
                Model.Shape = Model.Shape.Translate(value - P0);
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(P1));
                NotifyPropertyChanged(nameof(P2));
                NotifyPropertyChanged(nameof(P3));
                SendShapedObjectModelChangedMessage();
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
                SendShapedObjectModelChangedMessage();
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
                SendShapedObjectModelChangedMessage();
            }
        }

        public void InvokeRotation(double theta) {
            if (Model == null) return;
            Model.Shape = Model.Shape.Rotate(theta, Model.Shape.Points[0]);
            NotifyPropertyChanged(nameof(P1));
            NotifyPropertyChanged(nameof(P2));
            NotifyPropertyChanged(nameof(P3));
            SendShapedObjectModelChangedMessage();
        }

        public RectangleShapedObject Model { get; private set; }
        public TestSpace TestSpace { get; private set; }

        public void AssignModel(RectangleShapedObject model, TestSpace testSpace) {
            Model = model;
            TestSpace = testSpace;
            NotifyPropertyChanged(null);
            SendShapedObjectModelChangedMessage();
        }
    }
}