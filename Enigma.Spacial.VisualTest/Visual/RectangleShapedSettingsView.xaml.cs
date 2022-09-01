using ExtendedWPF;
using System;

namespace Enigma.Spacial.TestWPF.Visual {
    public partial class RectangleShapedSettingsView : View<RectangleShapedSettingsViewModel> {
        public RectangleShapedSettingsView() {
            InitializeComponent();
        }
        private void Button_InvokeRotation_Click(object sender, System.Windows.RoutedEventArgs e) {
            ViewModel.InvokeRotation(double.Parse(textBox_RotateBy.Text) * Math.PI);
        }
    }
}
