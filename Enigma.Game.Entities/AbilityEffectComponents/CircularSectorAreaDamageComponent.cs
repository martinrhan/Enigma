using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Game.Entities.AbilityEffectComponents {
    public class CircularSectorAreaDamageComponent : AbilityEffectComponent {
        public CircularSectorAreaDamageComponent(AbilityEffectComponentFactoryArguments arguments) {
        }

        public void Invoke(AbilityEffectAssembly.UpdateInterface updateInterface, in InvokeArguments arguments) {
            updateInterface.DealCircularSectorAreaDamage(arguments.Radius, arguments.Theta, arguments.Strength);
        }
        public struct InvokeArguments {
            public double Radius { get; set; }
            public double Theta { get; set; }
            public float Strength { get; init; }
        }
    }
}
