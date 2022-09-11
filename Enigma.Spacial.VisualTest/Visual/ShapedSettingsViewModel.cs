using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enigma.Spacial.TestWPF.Models;
using ExtendedWPF;

namespace Enigma.Spacial.TestWPF.Visual {
    public class ShapedSettingsViewModel : ViewModel {
        public ShapedSettingsViewModel(int messengerToken) {
            CircleShapedSettingsViewModel = new CircleShapedSettingsViewModel(messengerToken);
            RectangleShapedSettingsViewModel = new RectangleShapedSettingsViewModel(messengerToken);
        }

        public CircleShapedSettingsViewModel CircleShapedSettingsViewModel { get; } 
        public RectangleShapedSettingsViewModel RectangleShapedSettingsViewModel { get; }

        public IShapedObject Model { get; private set; }
        public void AssignModel(IShapedObject model, TestSpace testSpace) {
            Model = model;
            if (model is CircleShapedObject cso) {
                CircleShapedSettingsViewModel.AssignModel(cso, testSpace);
            } else if (model is RectangleShapedObject rso) {
                RectangleShapedSettingsViewModel.AssignModel(rso, testSpace);
            }
        }
    }
}
