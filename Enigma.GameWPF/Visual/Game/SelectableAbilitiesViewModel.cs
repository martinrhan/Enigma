using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enigma.Game;
using Enigma.GameWPF.Input;

namespace Enigma.GameWPF.Visual.Game {
    public class SelectableAbilitiesViewModel : CollectionViewModel<AbilityViewModel, int> {
        public SelectableAbilitiesViewModel() {
        }

        public int SelectedAbilityIndex { get; set; }

        public void UpdateDataFromModel(AbilityCollection abilityCollection, InputBindingManager inputBindingManager) {
            UpdateDataFromModel_Protected(Enumerable.Range(0, inputBindingManager.SelectAbilityInputActions.Count), (viewModel, i) => {
                var model = i >= abilityCollection.Count ? null : abilityCollection[i];
                KeyOrMouseButton? input = inputBindingManager.ActionToInputDictionary[inputBindingManager.SelectAbilityInputActions[i]];
                viewModel.UpdateDataFromModel(model, input, i == SelectedAbilityIndex);
            });
        }


    }
}
