using ExtendedWPF;
using System.Windows;
using System.Windows.Controls;

namespace Enigma.Spacial.TestWPF.Visual {
    public partial class ShapedSettingsView : ContentControl {
        public ShapedSettingsView() {
            InitializeComponent();
        }

        public int SelectedShapedTypeIndex {
            get { return (int)GetValue(SelectedShapedTypeIndexProperty); }
            set { SetValue(SelectedShapedTypeIndexProperty, value); }
        }
        public static readonly DependencyProperty SelectedShapedTypeIndexProperty =
            DependencyProperty.Register("SelectedShapedTypeIndex", typeof(int), typeof(ShapedSettingsView), new PropertyMetadata(0));
    }
}
