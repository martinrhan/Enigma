using Enigma.Common.Math;
using System.Windows;
using System.Windows.Media;

namespace Enigma.Spacial.TestWPF.Visual {

    public class RectangleViewModel : ViewModel {

        public RectangleViewModel() {
        }

        private PointCollection pointCollection;

        public PointCollection PointCollection {
            get => pointCollection;
            set {
                pointCollection = value;
                NotifyPropertyChanged();
            }
        }

        public void UpdateDataFromModel(RectangleShapedObject model) {
            PointCollection = new PointCollection() {
                ConvertToPoint(model.Shape.Points[0] - model.AABB.LowerBound, model.AABB.Height),
                ConvertToPoint(model.Shape.Points[1] - model.AABB.LowerBound, model.AABB.Height),
                ConvertToPoint(model.Shape.Points[2] - model.AABB.LowerBound, model.AABB.Height),
                ConvertToPoint(model.Shape.Points[3] - model.AABB.LowerBound, model.AABB.Height)
            };
        }

        private Point ConvertToPoint(in Vector2 vector2, double aABBHeight) {
            return new Point(vector2.X, aABBHeight - vector2.Y);
        }
    }
}