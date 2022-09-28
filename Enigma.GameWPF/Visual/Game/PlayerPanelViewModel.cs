using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Enigma.Game;
using Enigma.GameWPF.Input;

namespace Enigma.GameWPF.Visual.Game {
    public class PlayerPanelViewModel : ViewModel {
        public PlayerPanelViewModel() {

        }

        public bool IsSelectingItemPlacement { get; private set; }

        public GeneralCollectionViewModel<EquipmentAbilityItemViewModel, int> EquipmentItemsViewModel { get; } = new();
        public GeneralCollectionViewModel<ShopAbilityItemViewModel, AbilityItem> ShopItemsViewModel { get; } = new();
        public GeneralCollectionViewModel<AbilityItemViewModel, AbilityItem> InventoryItemsViewModel { get; } = new();

        public Player Player { get; private set; }
        public InputBindingManager InputBindingManager { get; private set; }
        public Inventory Inventory => Player.Inventory;
        public Shop Shop => Player.Shop;

        public void UpdateDataFromModel() {
            UpdateDataFromModel(Player, InputBindingManager);
        }
        public void UpdateDataFromModel(Player model, InputBindingManager inputBindingManager) {
            Player = model;
            InputBindingManager = inputBindingManager;
            AbilityCollection abilityCollection = model.PlayerGameBody.AbilityCollection;
            EquipmentItemsViewModel.UpdateDataFromModel(
                Enumerable.Range(0, inputBindingManager.SelectAbilityInputActions.Count), 
                (viewModel, i) => {
                    Ability model = i >= abilityCollection.Count ? null : abilityCollection[i];
                    KeyOrMouseButton? input = inputBindingManager.ActionToInputDictionary[inputBindingManager.SelectAbilityInputActions[i]];
                    viewModel.UpdateDataFromModel(model, input);
                }
            );
            InventoryItemsViewModel.UpdateDataFromModel(
                model.Inventory,
                (vm, m) => vm.UpdateDataFromModel(m)
            );
            ShopItemsViewModel.UpdateDataFromModel(
                model.Shop.ItemList,
                (vm, m) => vm.UpdateDataFromModel(m)
            );
            NotifyPropertyChanged(null);
            InventoryItemsViewModel.NotifyChanged();
            ShopItemsViewModel.NotifyChanged();
        }
    }
}
