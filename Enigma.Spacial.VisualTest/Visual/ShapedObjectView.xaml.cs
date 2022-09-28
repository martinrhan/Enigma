using System.Windows;

namespace Enigma.Spacial.TestWPF.Visual {

    public partial class ShapedObjectView : View<ShapedObjectViewModel> {

        public ShapedObjectView() {
            InitializeComponent();
        }

        public int SelectedShapedTypeIndex {
            get { return (int)GetValue(SelectedShapedTypeIndexProperty); }
            set { SetValue(SelectedShapedTypeIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedShapedTypeIndexProperty =
            DependencyProperty.Register("SelectedShapedTypeIndex", typeof(int), typeof(ShapedObjectView), new PropertyMetadata(0));
    }
}