using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Enigma.Game;

namespace Enigma.GameWPF.Visual.Game {
    public class EnemyWaveManagerViewModel : ViewModel {
        public SelectableEnemyWaveTemplatesViewModel SelectableEnemyWaveTemplatesViewModel { get; } = new SelectableEnemyWaveTemplatesViewModel();

        private EnemyWaveManager model;

        public bool LaunchWaveAvailable => model == null? false : model.CurrentEnemyWave == null;

        public void UpdateDataFromModel(EnemyWaveManager model) {
            this.model = model;
            SelectableEnemyWaveTemplatesViewModel.UpdateDataFromModel(model.SelectableEnemyWaveTemplateList);
            NotifyPropertyChanged(nameof(SelectableEnemyWaveTemplatesViewModel));
            NotifyPropertyChanged(nameof(LaunchWaveAvailable));
        }

        public void LaunchWave() {
            model.LaunchNewEnemyWave(SelectableEnemyWaveTemplatesViewModel.SelectedIndex);
            NotifyPropertyChanged(nameof(LaunchWaveAvailable));
        }
    }
}
