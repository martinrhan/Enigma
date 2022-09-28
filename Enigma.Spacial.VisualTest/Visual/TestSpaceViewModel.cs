using CommunityToolkit.Mvvm.Messaging;
using Enigma.Common.Math;
using Enigma.Spacial.TestWPF.Models;
using System;

namespace Enigma.Spacial.TestWPF.Visual {

    public class TestSpaceViewModel : ViewModel, IRecipient<ShapedObjectModelChangedMessage> {

        public TestSpaceViewModel() {
            WeakReferenceMessenger.Default.Register<TestSpaceViewModel, ShapedObjectModelChangedMessage, int>(this, 0, (r, m) => r.Receive(m));
            WeakReferenceMessenger.Default.Register<TestSpaceViewModel, ShapedObjectModelChangedMessage, int>(this, 1, (r, m) => r.Receive(m));
        }

        public double Width => Model == null ? 0 : Model.Width;
        public double Height => Model == null ? 0 : Model.Height;

        public ShapedSettingsViewModel ShapedSettingsAViewModel { get; } = new ShapedSettingsViewModel(0);
        public ShapedSettingsViewModel ShapedSettingsBViewModel { get; } = new ShapedSettingsViewModel(1);
        public ShapedObjectViewModel ShapedObjectAViewModel { get; } = new ShapedObjectViewModel(0);
        public ShapedObjectViewModel ShapedObjectBViewModel { get; } = new ShapedObjectViewModel(1);
        public CollisionCalculatorResultViewModel CollisionCalculatorResultViewModel { get; } = new CollisionCalculatorResultViewModel();

        private static Func<IShapedObject>[] shapedObjectFactories = new Func<IShapedObject>[] {
            ()=> new CircleShapedObject(new Circle(Vector2.Zero, 50).Translate(new(100,100))),
            ()=> new RectangleShapedObject(new Rectangle(100,100).Translate(new(200,200)))
        };

        private int aSelectedShapedTypeIndex = 0;

        public int ASelectedShapedTypeIndex {
            get => aSelectedShapedTypeIndex;
            set {
                aSelectedShapedTypeIndex = value;
                NotifyPropertyChanged();
                Model.ShapedObjectA = shapedObjectFactories[value]();
                ShapedSettingsAViewModel.AssignModel(Model.ShapedObjectA, Model);
                SendTestSpaceChangedMessage();
            }
        }

        private int bSelectedShapedTypeIndex = 0;

        public int BSelectedShapedTypeIndex {
            get => bSelectedShapedTypeIndex;
            set {
                bSelectedShapedTypeIndex = value;
                NotifyPropertyChanged();
                Model.ShapedObjectB = shapedObjectFactories[value]();
                ShapedSettingsBViewModel.AssignModel(Model.ShapedObjectB, Model);
                SendTestSpaceChangedMessage();
            }
        }

        public TestSpace Model { get; private set; }

        public void AssignModel(TestSpace model) {
            Model = model;
            NotifyPropertyChanged(nameof(Width));
            NotifyPropertyChanged(nameof(Height));
            Model.ShapedObjectA = shapedObjectFactories[0]();
            ShapedSettingsAViewModel.AssignModel(Model.ShapedObjectA, model);
            Model.ShapedObjectB = shapedObjectFactories[0]();
            ShapedSettingsBViewModel.AssignModel(Model.ShapedObjectB, model);
            SendTestSpaceChangedMessage();
        }

        private void SendTestSpaceChangedMessage() {
            WeakReferenceMessenger.Default.Send(new TestSpaceChangedMessage() { TestSpace = Model });
        }

        public void Receive(ShapedObjectModelChangedMessage message) {
            SendTestSpaceChangedMessage();
        }
    }
}