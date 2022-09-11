using Enigma.Common.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedWPF;
using CommunityToolkit.Mvvm.Messaging;
using Enigma.Spacial.TestWPF.Models;

namespace Enigma.Spacial.TestWPF.Visual {
    public class CircleShapedSettingsViewModel : ViewModel {
        public CircleShapedSettingsViewModel(int messengerToken) {
            this.messengerToken = messengerToken;
        }
        private readonly int messengerToken;
        private void SendShapedObjectModelChangedMessage() {
            WeakReferenceMessenger.Default.Send(new ShapedObjectModelChangedMessage() { TestSpace = TestSpace, ShapedObject = Model }, messengerToken);
        }

        public double Radius {
            get => Model == null ? 0 : Model.Shape.Radius;
            set {
                if (Model == null) return;
                Model.Shape = Model.Shape.ChangeRadius(value);
                NotifyPropertyChanged();
                SendShapedObjectModelChangedMessage();
            }
        }

        public Vector2 Center {
            get => Model == null ? Vector2.Zero : Model.Shape.Center;
            set {
                if (Model == null) return;
                Model.Shape = Model.Shape.ChangeCenter(value);
                NotifyPropertyChanged();
                SendShapedObjectModelChangedMessage();
            }
        }

        public CircleShapedObject Model { get; private set; }
        public TestSpace TestSpace { get; private set; }
        public void AssignModel(CircleShapedObject model, TestSpace testSpace) {
            Model = model;
            TestSpace = testSpace;
            NotifyPropertyChanged(null);
            SendShapedObjectModelChangedMessage();
        }
    }
}
