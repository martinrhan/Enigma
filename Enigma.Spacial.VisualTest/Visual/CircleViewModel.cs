using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using ExtendedWPF;

namespace Enigma.Spacial.TestWPF.Visual {
    public class CircleViewModel : ViewModel, IRecipient<ShapedObjectModelChangedMessage> {
        public CircleViewModel() {
            WeakReferenceMessenger.Default.Register<CircleViewModel, ShapedObjectModelChangedMessage>(this, (r, m) => r.Receive(m));
        }

        public double LowerBoundX { get; private set; }
        public double LowerBoundY { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }

        public void Receive(ShapedObjectModelChangedMessage message) {
            UpdateDataFromModel((CircleShapedObject)message.ShapedObject);
        }

        public void UpdateDataFromModel(CircleShapedObject model) {
            LowerBoundX = model.AABB.LowerBound.X;
            LowerBoundY = model.AABB.LowerBound.Y;
            Width = model.AABB.Width;
            Height = model.AABB.Height;
            NotifyPropertyChanged(nameof(LowerBoundX));
            NotifyPropertyChanged(nameof(LowerBoundY));
            NotifyPropertyChanged(nameof(Width));
            NotifyPropertyChanged(nameof(Height));
        }
    }
}
