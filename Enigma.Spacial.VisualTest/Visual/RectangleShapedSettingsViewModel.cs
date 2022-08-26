using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedWPF;

namespace Enigma.Spacial.TestWPF.Visual {
    public class RectangleShapedSettingsViewModel : ViewModel {
        public RectangleShapedSettingsViewModel() {

        }

        public RectangleShapedObject Model { get; private set; }
        public void AssignModel(RectangleShapedObject model) {
            Model = model;
        }

    }
}
