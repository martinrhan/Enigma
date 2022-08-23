using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enigma.Game;

namespace Enigma.GameWPF.Visual.Game {
    public class AbilityItemViewModel : ViewModel {
        public AbilityItemViewModel(AbilityItem abilityItem) {
            this.abilityItem = abilityItem;
        }
        private AbilityItem abilityItem;

    }
}
