using Enigma.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedWPF;
using CommunityToolkit.Mvvm.Messaging;

namespace Enigma.Spacial.TestWPF.Visual {
    public class ShapedObjectViewModel : ViewModel, IRecipient<ShapedObjectModelChangedMessage> {
        public ShapedObjectViewModel(int messengerToken) {
            WeakReferenceMessenger.Default.Register<ShapedObjectViewModel, ShapedObjectModelChangedMessage, int>(this, messengerToken, (r, m) => r.Receive(m));
        }

        public double LowerBoundX { get; private set; }
        public double LowerBoundY { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }

        public CircleViewModel CircleViewModel { get; } = new CircleViewModel();
        public RectangleViewModel RectangleViewModel { get; } = new RectangleViewModel();

        public void Receive(ShapedObjectModelChangedMessage message) {
            UpdateDataFromModel(message.ShapedObject);
        }

        public void UpdateDataFromModel(IShapedObject model) {
            LowerBoundX = model.AABB.LowerBound.X;
            LowerBoundY = model.AABB.LowerBound.Y;
            Width = model.AABB.Width;
            Height = model.AABB.Height;
            NotifyPropertyChanged(nameof(LowerBoundX));
            NotifyPropertyChanged(nameof(LowerBoundY));
            NotifyPropertyChanged(nameof(Width));
            NotifyPropertyChanged(nameof(Height));
            if (model is CircleShapedObject cso) {
                CircleViewModel.UpdateDataFromModel(cso);
            } else if (model is RectangleShapedObject rso) {
                RectangleViewModel.UpdateDataFromModel(rso);
            }
        }

    }
}
