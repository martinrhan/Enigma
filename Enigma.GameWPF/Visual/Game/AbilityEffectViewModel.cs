using Enigma.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.GameWPF.Visual.Game {
    public abstract class AbilityEffectViewModel : ManualNotifyChangedViewModel {
        protected internal abstract void UpdateDataFromModel(GameBody caster, AbilityCastInputData inputData, Ability ability, double deltaTime);
    }
}
