using CommunityToolkit.Mvvm.Messaging;
using Enigma.Spacial.TestWPF.Models;

namespace Enigma.Spacial.TestWPF.Visual {

    public class CollisionCalculatorResultViewModel : ViewModel, IRecipient<TestSpaceChangedMessage> {

        public CollisionCalculatorResultViewModel() {
            WeakReferenceMessenger.Default.Register<CollisionCalculatorResultViewModel, TestSpaceChangedMessage>(this, (r, m) => r.Receive(m));
        }

        private bool aABBIntersection;

        public bool AABBIntersection {
            get { return aABBIntersection; }
            set {
                aABBIntersection = value;
                NotifyPropertyChanged();
            }
        }

        private bool shapeCollision;

        public bool ShapeCollision {
            get { return shapeCollision; }
            set {
                shapeCollision = value;
                NotifyPropertyChanged();
            }
        }

        public void Receive(TestSpaceChangedMessage message) {
            UpdateDataFromModel(message.TestSpace);
        }

        public void UpdateDataFromModel(TestSpace model) {
            if (model.ShapedObjectA == null || model.ShapedObjectB == null) return;
            AABBIntersection = CollisionCalculator.CheckAABBIntersection(model.ShapedObjectA.AABB, model.ShapedObjectB.AABB, model);
            ShapeCollision = CollisionCalculator.CheckCollision(model.ShapedObjectA, model.ShapedObjectB, model);
        }
    }
}