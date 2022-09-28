using CommunityToolkit.Mvvm.Messaging;
using Enigma.Spacial.TestWPF.Models;

namespace Enigma.Spacial.TestWPF.Visual {

    public class ShapedObjectViewModel : ViewModel, IRecipient<ShapedObjectModelChangedMessage> {

        public ShapedObjectViewModel(int messengerToken) {
            WeakReferenceMessenger.Default.Register<ShapedObjectViewModel, ShapedObjectModelChangedMessage, int>(this, messengerToken, (r, m) => r.Receive(m));
        }

        public double LowerBoundX { get; private set; }
        public double LowerBoundY { get; private set; }

        public double Width { get; private set; }
        public double Height { get; private set; }

        public double ShapedObjectAABBWidth { get; private set; }
        public double ShapedObjectAABBHeight { get; private set; }

        public CircleViewModel CircleViewModel { get; } = new CircleViewModel();
        public RectangleViewModel RectangleViewModel { get; } = new RectangleViewModel();

        public void Receive(ShapedObjectModelChangedMessage message) {
            UpdateDataFromModel(message.ShapedObject, message.TestSpace);
        }

        public void UpdateDataFromModel(IShapedObject model, TestSpace testSpace) {
            ShapedObjectAABBWidth = model.AABB.Width;
            ShapedObjectAABBHeight = model.AABB.Height;
            Width = testSpace.Width + ShapedObjectAABBWidth;
            Height = testSpace.Height + ShapedObjectAABBHeight;
            LowerBoundX = model.AABB.LowerBound.X;
            if (model.AABB.UpperBound.X > testSpace.Width) {
                LowerBoundX -= testSpace.Width;
            }
            LowerBoundY = model.AABB.LowerBound.Y;
            if (model.AABB.UpperBound.Y > testSpace.Height) {
                LowerBoundY -= testSpace.Height;
            }
            NotifyPropertyChanged(null);
            if (model is CircleShapedObject cso) {
                CircleViewModel.UpdateDataFromModel(cso);
            } else if (model is RectangleShapedObject rso) {
                RectangleViewModel.UpdateDataFromModel(rso);
            }
        }
    }
}