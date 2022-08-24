using Enigma.Spacial.TestWPF.Models;
using ExtendedWPF;
using System.Windows.Controls;

namespace Enigma.Spacial.TestWPF.Visual {
    public partial class TestSpaceView : ContentControl {
        public TestSpaceView() {
            InitializeComponent();
        }

        public TestSpace Model { get; private set; }
        public void AssignModel(TestSpace model) {
            Model = model;
            shapedSettingsViewA.AssignModel(model.ShapedObjectAWrapper);
            shapedSettingsViewB.AssignModel(model.ShapedObjectBWrapper);
        }
    }
}
