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
        public TestSpaceViewModel(TestSpace testSpace) { }

        public ShapedSettingsViewModel ShapedSettingsAViewModel { get; } = new ShapedSettingsViewModel();
        public ShapedSettingsViewModel ShapedSettingsBViewModel { get; } = new ShapedSettingsViewModel();
        public ShapedObjectViewModel ShapedObjectAViewModel { get; } = new ShapedObjectViewModel();
        public ShapedObjectViewModel ShapedObjectBViewModel { get; } = new ShapedObjectViewModel();

        private static Func<IShapedObject>[] shapedObjectFactories = new Func<IShapedObject>[] {
            ()=> new CircleShapedObject(Vector2.Zero, 10),
            ()=> new RectangleShapedObject(10, 10)
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
                ShapedSettingsAViewModel.AssignModel(Model.ShapedObjectB);
            }
        }

        public TestSpace Model { get; private set; }
        public void AssignModel(TestSpace model) {
            Model = model;
        }
    }
}
