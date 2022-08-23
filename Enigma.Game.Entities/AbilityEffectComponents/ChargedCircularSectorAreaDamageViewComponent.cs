using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Game.Entities.AbilityEffectComponents {
    public class ChargedCircularSectorAreaDamageViewComponent: AbilityEffectComponent {
        public ChargedCircularSectorAreaDamageViewComponent(AbilityEffectComponentFactoryArguments arguments) {

        }

        public void Invoke(AbilityEffectAssembly.UpdateInterface updateInterface, in InvokeArguments arguments) {
        }

        public struct InvokeArguments {
            public double Radius { get; init; }
            public double Theta { get; init; }
            public double ChargedProportion { get; init; }
        }
    }
}
