using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enigma.Game;
using Enigma.GameWPF.Input;

namespace Enigma.GameWPF.Visual.Game {
    public class PlayerViewModel : ManualNotifyChangedViewModel {
        public SelectableAbilitiesViewModel SelectableAbilitiesViewModel { get;} = new SelectableAbilitiesViewModel();


        public void UpdateDataFromModel(Player model, InputBindingManager inputBindingManager) {
            SelectableAbilitiesViewModel.UpdateDataFromModel(model.PlayerGameBody.AbilityCollection, inputBindingManager);
        }

        public override void NotifyChanged() {
            SelectableAbilitiesViewModel.NotifyChanged();
        }
    }
}
