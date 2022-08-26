using Enigma.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedWPF;

namespace Enigma.Spacial.TestWPF.Visual {
    public class ShapedObjectViewModel : ViewModel {
        public ShapedObjectViewModel() {

        }

        public CircleViewModel CircleViewModel { get; } = new CircleViewModel();
        public RectangleViewModel RectangleViewModel { get; } = new RectangleViewModel();
                
        public void UpdateDataFromModel(IShapedObject model) {

        }

    }
}
