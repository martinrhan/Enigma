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

        private const int InventoryTabIndex = 0;
        private const int EnemyWaveManagerTabIndex = 1;
        private const int MenuTabIndex = 2;

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

        public void SelectInventory() {
            if (visibility == Visibility.Visible) {
                if (selectedIndex != MenuTabIndex) {
                    if (selectedIndex == InventoryTabIndex) {
                        Visibility = Visibility.Collapsed;
                    } else {
                        SelectedIndex = InventoryTabIndex;
                    }
                }
            } else {
                SelectedIndex = InventoryTabIndex;
                Visibility = Visibility.Visible;
            }
        }

        public void SelectEnemyWaveManager() {
            if (visibility == Visibility.Visible) {
                if (selectedIndex != MenuTabIndex) {
                    if (selectedIndex == EnemyWaveManagerTabIndex) {
                        Visibility = Visibility.Collapsed;
                    } else {
                        SelectedIndex = EnemyWaveManagerTabIndex;
                    }
                }
            } else {
                SelectedIndex = EnemyWaveManagerTabIndex;
                Visibility = Visibility.Visible;
            }
        }

        public void UpdateDataFromModel(EnemyWaveManager enemyWaveManager, Player player, InputBindingManager inputBindingManager) {
            EnemyWaveManagerViewModel.UpdateDataFromModel(enemyWaveManager);
            PlayerPanelViewModel.UpdateDataFromModel(player, inputBindingManager);
        }
        public void NotifyChanged() {
        }
    }
}
