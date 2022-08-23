using Enigma.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.GameWPF.Visual.Game {
    public class AIGameBodyTemplatePoolsViewModel : CollectionViewModel<AIGameBodyTemplatePoolViewModel, AIGameBodyTemplatePool> {
        public AIGameBodyTemplatePoolsViewModel() {

        }

        public void UpdateDataFromModel(IEnumerable<AIGameBodyTemplatePool> models) {
            UpdateDataFromModel_Protected(models, (vm, m) => vm.UpdateDataFromModel(m));
        }
    }
}
