using System.Windows;
using System.Windows.Controls;

namespace Enigma.GameWPF.Visual.Game {
    public partial class PlayerPanelView : View<PlayerPanelViewModel> {
        public PlayerPanelView() {
            InitializeComponent();
        }

        private void LostFocus_ListView(object sender, RoutedEventArgs e) {
            ListView listView = (ListView)sender;
            listView.SelectedIndex = -1;
        }

        private void Click_Button_Equipment(object sender, RoutedEventArgs e) {
            EquipmentVisibility = Visibility.Visible;
            ShopVisibility = Visibility.Collapsed;
        }
        private void Click_Button_Shop(object sender, RoutedEventArgs e) {
            ShopVisibility = Visibility.Visible;
            EquipmentVisibility = Visibility.Collapsed;
        }
        private void Click_Button_Buy(object sender, RoutedEventArgs e) {
            ViewModel.Inventory.BuyAbilityItem(listView_Shop.SelectedIndex, ViewModel.Shop);
            ViewModel.UpdateDataFromModel();
        }
        private void Click_Button_Sell(object sender, RoutedEventArgs e) {
            ViewModel.Inventory.SellAbilityItem(listView_Inventory.SelectedIndex);
            ViewModel.UpdateDataFromModel();
        }
        private void Click_Button_Move(object sender, RoutedEventArgs e) {
            ViewModel.Inventory.SellAbilityItem(listView_Inventory.SelectedIndex);
            ViewModel.UpdateDataFromModel();
        }
        private void Click_Button_Equip(object sender, RoutedEventArgs e) {

            ViewModel.UpdateDataFromModel();
        }
        private void ExchangeInventoryEquipment(object sender, RoutedEventArgs e) {
            ViewModel.Inventory.ExchangeWithAbilityCollection(ViewModel.Player.PlayerGameBody.AbilityCollection, listView_Inventory.SelectedIndex, listView_Equipment.SelectedIndex);
            ViewModel.UpdateDataFromModel();
        }

        public Visibility ShopVisibility {
            get { return (Visibility)GetValue(ShopVisibilityProperty); }
            set { SetValue(ShopVisibilityProperty, value); }
        }
        public static readonly DependencyProperty ShopVisibilityProperty =
            DependencyProperty.Register("ShopVisibility", typeof(Visibility), typeof(PlayerPanelView), new PropertyMetadata(Visibility.Collapsed));

        public Visibility EquipmentVisibility {
            get { return (Visibility)GetValue(EquipmentVisibilityProperty); }
            set { SetValue(EquipmentVisibilityProperty, value); }
        }
        public static readonly DependencyProperty EquipmentVisibilityProperty =
            DependencyProperty.Register("EquipmentVisibility", typeof(Visibility), typeof(PlayerPanelView), new PropertyMetadata(Visibility.Visible));



    }
}
