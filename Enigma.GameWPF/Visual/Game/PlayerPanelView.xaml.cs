using System.Windows;
using System.Windows.Controls;
using DependencyPropertyGenerator;

namespace Enigma.GameWPF.Visual.Game {
    [DependencyProperty<Visibility>("ShopViewVisibility", DefaultValue = Visibility.Visible)]
    [DependencyProperty<Visibility>("EquipmentViewVisibility", DefaultValue = Visibility.Collapsed)]
    [DependencyProperty<ShopAbilityItemView>("SelectedShopItemView")]
    [DependencyProperty<AbilityItemView>("SelectedInventoryItemView")]
    [DependencyProperty<EquipmentAbilityItemView>("SelectedEquipmentItemView")]
    public partial class PlayerPanelView : View<PlayerPanelViewModel> {
        public PlayerPanelView() {
            InitializeComponent();
        }

        private void LostFocus_ListView(object sender, RoutedEventArgs e) {
            ListView listView = (ListView)sender;
            listView.SelectedIndex = -1;
        }

        private void Click_Button_Equipment(object sender, RoutedEventArgs e) {
            EquipmentViewVisibility = Visibility.Visible;
            ShopViewVisibility = Visibility.Collapsed;
        }
        private void Click_Button_Shop(object sender, RoutedEventArgs e) {
            ShopViewVisibility = Visibility.Visible;
            EquipmentViewVisibility = Visibility.Collapsed;
        }
        private void MouseLeftButtonDown_ShopAbilityItemView(object sender, RoutedEventArgs e) {
            ShopAbilityItemView control = (ShopAbilityItemView)sender;
        }
        private void MouseLeftButtonDown_Control_InventoryItem(object sender, RoutedEventArgs e) {
            ViewModel.UpdateDataFromModel();
        }
        private void MouseRightButtonDown_Control_InventoryItem(object sender, RoutedEventArgs e) {
            ViewModel.UpdateDataFromModel();
        }

    }
}
