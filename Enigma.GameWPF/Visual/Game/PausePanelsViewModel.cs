using Enigma.Game;
using Enigma.GameWPF.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Enigma.GameWPF.Visual.Game {
    public class PausePanelsViewModel : ViewModel {
        public PausePanelsViewModel() {
        }

        public EnemyWaveManagerViewModel EnemyWaveManagerViewModel { get; } = new EnemyWaveManagerViewModel();
        public PlayerPanelViewModel PlayerPanelViewModel { get; } = new PlayerPanelViewModel();

        public const int MenuTabIndex = 0;
        public const int InventoryTabIndex = 1;
        public const int EnemyWaveManagerTabIndex = 2;

        private int selectedIndex = InventoryTabIndex;
        public int SelectedIndex {
            get => selectedIndex;
            private set {
                selectedIndex = value;
                NotifyPropertyChanged();
            }
        }

        private Visibility visibility = Visibility.Collapsed;
        public Visibility Visibility {
            get => visibility;
            private set {
                visibility = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(IsPaused));
            }
        }

        public bool IsPaused => Visibility == Visibility.Visible;

        public void SelectMenu() {
            if (IsPaused) {
                if (selectedIndex != MenuTabIndex) {
                    SelectedIndex = MenuTabIndex;
                }
            } else {
                SelectedIndex = MenuTabIndex;
                Visibility = Visibility.Visible;
            }
        }
        public void SelectInventory() {
            if (IsPaused) {
                if (selectedIndex != MenuTabIndex) {
                    SelectedIndex = InventoryTabIndex;
                }
            } else {
                SelectedIndex = InventoryTabIndex;
                Visibility = Visibility.Visible;
            }
        }
        public void SelectEnemyWaveManager() {
            if (IsPaused) {
                if (selectedIndex != MenuTabIndex) {
                    SelectedIndex = EnemyWaveManagerTabIndex;
                }
            } else {
                SelectedIndex = EnemyWaveManagerTabIndex;
                Visibility = Visibility.Visible;
            }
        }
        public void Close() {
            Visibility = Visibility.Collapsed;
        }

        public void UpdateDataFromModel(EnemyWaveManager enemyWaveManager, Player player, InputBindingManager inputBindingManager) {
            EnemyWaveManagerViewModel.UpdateDataFromModel(enemyWaveManager);
            PlayerPanelViewModel.UpdateDataFromModel(player, inputBindingManager);
        }
        public void NotifyChanged() {
        }
    }
}
