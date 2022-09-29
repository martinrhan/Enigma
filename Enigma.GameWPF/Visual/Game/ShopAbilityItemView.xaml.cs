using System.Windows;

namespace Enigma.GameWPF.Visual.Game {
    public partial class ShopAbilityItemView : View<ShopAbilityItemViewModel> {
        public ShopAbilityItemView() {
            InitializeComponent();
        }

        public bool IsSelected {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(ShopAbilityItemView), new PropertyMetadata());


    }
}
