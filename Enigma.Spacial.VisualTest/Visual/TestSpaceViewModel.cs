using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enigma.Spacial.TestWPF.Models;
using Enigma.Common.Math;
using ExtendedWPF;

namespace Enigma.Spacial.TestWPF.Visual {
    public class TestSpaceViewModel : ViewModel {
        public TestSpaceViewModel() { }

        public ShapedSettingsViewModel ShapedSettingsAViewModel { get; } = new ShapedSettingsViewModel(0);
        public ShapedSettingsViewModel ShapedSettingsBViewModel { get; } = new ShapedSettingsViewModel(1);
        public ShapedObjectViewModel ShapedObjectAViewModel { get; } = new ShapedObjectViewModel(0);
        public ShapedObjectViewModel ShapedObjectBViewModel { get; } = new ShapedObjectViewModel(1);

        private static Func<IShapedObject>[] shapedObjectFactories = new Func<IShapedObject>[] {
            ()=> new CircleShapedObject(new Circle(Vector2.Zero, 100).Translate(new(200,200))),
            ()=> new RectangleShapedObject(new Rectangle(100,100).Translate(new(200,200))) 
        };

        private int aSelectedShapedTypeIndex = 0;
        public int ASelectedShapedTypeIndex {
            get { return aSelectedShapedTypeIndex; }
            set {
                aSelectedShapedTypeIndex = value;
                NotifyPropertyChanged();
                Model.ShapedObjectA = shapedObjectFactories[value]();
                ShapedSettingsAViewModel.AssignModel(Model.ShapedObjectA);
            }
        }

        private int bSelectedShapedTypeIndex = 0;
        public int BSelectedShapedTypeIndex {
            get { return bSelectedShapedTypeIndex; }
            set {
                bSelectedShapedTypeIndex = value;
                NotifyPropertyChanged();
                Model.ShapedObjectB = shapedObjectFactories[value]();
                ShapedSettingsBViewModel.AssignModel(Model.ShapedObjectB);
            }
        }

        public TestSpace Model { get; private set; }
        public void AssignModel(TestSpace model) {
            Model = model;
            Model.ShapedObjectA = shapedObjectFactories[0]();
            ShapedSettingsAViewModel.AssignModel(Model.ShapedObjectA);
            Model.ShapedObjectB = shapedObjectFactories[0]();
            ShapedSettingsBViewModel.AssignModel(Model.ShapedObjectB);
        }
    }
}
