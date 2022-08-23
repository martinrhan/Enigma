using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Game.Entities.AbilityEffectComponents {
    public class ChargeComponent: AbilityEffectComponent {
        public ChargeComponent(AbilityEffectComponentFactoryArguments arguments) {
        }

        public double ChargedTime { get; private set; }


        public void Invoke(AbilityEffectAssembly.UpdateInterface updateInterface, InvokeArguments arguments) {
            ChargedTime += updateInterface.DeltaTime;
        }
        public struct InvokeArguments {
        }
    }
}
