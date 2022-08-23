using System.Windows;

namespace Enigma.GameWPF.Visual.Game {
    public partial class PausePanelsView : View<PausePanelsViewModel> {
        public PausePanelsView() {
            InitializeComponent();
        }
        public double InventoryViewLeftMargin {
            get { return (double)GetValue(InventoryViewLeftMarginProperty); }
            set { SetValue(InventoryViewLeftMarginProperty, value); }
        }
        public static readonly DependencyProperty InventoryViewLeftMarginProperty =
            DependencyProperty.Register("InventoryViewLeftMargin", typeof(double), typeof(PausePanelsView), new PropertyMetadata());
    }
}
