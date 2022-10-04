using System.Windows;
using System.Windows.Controls;
using DependencyPropertyGenerator;
using System.Linq;
using System;
using System.Windows.Media;
using System.Xml.Linq;

namespace Enigma.GameWPF.Visual.Game {
    [DependencyProperty<ItemViewType>("SelectedItemViewType", DefaultValue = ItemViewType.None, IsReadOnly = true)]
    [DependencyProperty<UIElement>("SelectedItemView", IsReadOnly = true)]
    [AttachedDependencyProperty<bool>("IsSelected", IsReadOnly = true)]
    public partial class PlayerPanelView : View<PlayerPanelViewModel> {
        public PlayerPanelView() {
            InitializeComponent();
        }

        partial void OnSelectedItemViewChanged(UIElement oldValue, UIElement newValue) {
            if (oldValue != null) SetIsSelected(oldValue, false);
            if (newValue != null) SetIsSelected(newValue, true);
        }

        private void Click_ShopItemView(object sender, RoutedEventArgs e) {
            UIElement itemView = (UIElement)sender;
            if (GetIsSelected(itemView)) {
                SelectedItemView = null;//Unselect selected item
                SelectedItemViewType = ItemViewType.None;
            } else {
                SelectedItemView = itemView; //Select the Clicked item, from any another selection or nothing selected.
                SelectedItemViewType = ItemViewType.Shop;
            }
        }
        private void Click_InventoryItemView(object sender, RoutedEventArgs e) {
            UIElement itemView = (UIElement)sender;
            if (GetIsSelected(itemView)) {
                SelectedItemView = null;//Unselect selected item
                SelectedItemViewType = ItemViewType.None;
            } else {
                switch (SelectedItemViewType) {
                    case ItemViewType.None://Select item when nothing was selected.
                        SelectedItemView = itemView;
                        SelectedItemViewType = ItemViewType.Inventory;
                        goto finishSwitch;
                    case ItemViewType.Shop:
                        ViewModel.Player.BuyAbilityItem(FindIndex(itemsControl_Shop, SelectedItemView), FindIndex(itemsControl_Inventory, itemView));
                        SelectedItemView = null;
                        break;
                    case ItemViewType.Inventory:
                        ViewModel.Player.InventoryInternalExchange(FindIndex(itemsControl_Inventory, SelectedItemView), FindIndex(itemsControl_Inventory, itemView));
                        SelectedItemView = null;
                        break;
                    case ItemViewType.Equipment:
                        ViewModel.Player.Exchange_Inventory_AbilityCollection(FindIndex(itemsControl_Inventory, itemView), FindIndex(itemsControl_Equipment, SelectedItemView));
                        SelectedItemView = null;
                        break;
                    default: throw new Exception();
                }
                SelectedItemViewType = ItemViewType.None;
                ViewModel.UpdateDataFromModel();
            finishSwitch:;
            }
        }
        private void MouseRightButtonUp_InventoryItemView(object sender, RoutedEventArgs e) {
            UIElement itemView = (UIElement)sender;
            ViewModel.Player.SellAbilityItem(FindIndex(itemsControl_Inventory, itemView));
            ViewModel.UpdateDataFromModel();
        }
        private void Click_EquipmentItemView(object sender, RoutedEventArgs e) {
            UIElement itemView = (UIElement)sender;
            if (GetIsSelected(itemView)) {
                SelectedItemView = null;
                SelectedItemViewType = ItemViewType.None;
            } else {
                switch (SelectedItemViewType) {
                    case ItemViewType.None:
                        SelectedItemView = itemView;
                        SelectedItemViewType = ItemViewType.Equipment;
                        goto finishSwitch;
                    case ItemViewType.Shop:
                        ViewModel.Player.BuyAbilityItemToAbilityCollection(
                            FindIndex(itemsControl_Shop, SelectedItemView),
                            FindIndex(itemsControl_Equipment, itemView)
                        );
                        SelectedItemView = null;
                        break;
                    case ItemViewType.Inventory:
                        ViewModel.Player.Exchange_Inventory_AbilityCollection(FindIndex(itemsControl_Inventory, SelectedItemView), FindIndex(itemsControl_Equipment, itemView));
                        SelectedItemView = null;
                        break;
                    case ItemViewType.Equipment:
                        ViewModel.Player.AbilityCollectionInternalExchange(FindIndex(itemsControl_Equipment, SelectedItemView), FindIndex(itemsControl_Equipment, itemView));
                        SelectedItemView = null;
                        break;
                    default: throw new Exception();
                }
                SelectedItemViewType = ItemViewType.None;
                ViewModel.UpdateDataFromModel();
            finishSwitch:;
            }
        }
        private void MouseRightButtonUp_EquipmentItemView(object sender, RoutedEventArgs e) {
            UIElement itemView = (UIElement)sender;
            ViewModel.Player.SellAbilityItemFromAbilityCollection(FindIndex(itemsControl_Inventory, itemView));
            ViewModel.UpdateDataFromModel();
        }

        private static int FindIndex(ItemsControl itemsControl, UIElement uIElement) {
            return itemsControl.ItemContainerGenerator.IndexFromContainer(VisualTreeHelper.GetParent(uIElement));
        }

        public enum ItemViewType { None, Equipment, Inventory, Shop }
        public static ItemViewType ItemViewType_Shop = ItemViewType.Shop;
    }

}
