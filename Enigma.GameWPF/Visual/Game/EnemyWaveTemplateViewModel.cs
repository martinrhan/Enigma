using Enigma.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.GameWPF.Visual.Game {
    public class EnemyWaveTemplateViewModel : ManualNotifyChangedViewModel {
        public EnemyWaveTemplateViewModel() {

        }
        public string Id { get; private set; }
        public AIGameBodyTemplatePoolsViewModel AIGameBodyTemplatePoolsViewModel { get; } = new AIGameBodyTemplatePoolsViewModel();

        public void UpdateDataFromModel(EnemyWaveTemplate model) {
            Id = model.Id;
            AIGameBodyTemplatePoolsViewModel.UpdateDataFromModel(model.AIGameBodyTemplatePoolList);
        }

        public override void NotifyChanged() {
            NotifyPropertyChanged(nameof(Id));
        }
    }
}
