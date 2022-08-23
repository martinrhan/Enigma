using Enigma.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.GameWPF.Visual.Game {
    public class SelectableEnemyWaveTemplatesViewModel : CollectionViewModel<EnemyWaveTemplateViewModel, EnemyWaveTemplate> {
        public SelectableEnemyWaveTemplatesViewModel() {

        }

        public int SelectedIndex { get; set; } = 0;

        public void UpdateDataFromModel(IEnumerable<EnemyWaveTemplate> templates) {
            UpdateDataFromModel_Protected(templates, (viewModel, Model) => {
                viewModel.UpdateDataFromModel(Model);
            });
            NotifyChanged();
        }
    }
}
