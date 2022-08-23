using Enigma.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Enigma.Game {
    /// <summary>
    /// Equiped ability. Game-time data such as remaininig cooldown are recorded here.
    /// </summary>
    public class Ability {
        internal Ability(AbilityTemplate template) {
            this.Template = template;
            EffectAssembly = template.AbilityEffectAssemblyTemplate.New();
        }
        public AbilityTemplate Template { get; }
        public string GivenName { get; set; }
        public int Size => Template.Size;
        public AbilityEffectAssembly EffectAssembly { get; }

        public AbilityItem ConvertToAbilityItem() => new AbilityItem(Template);

        internal void StartCasting(GameBody gameBody, AbilityCastInputData inputData, double deltaTime) {
            EffectAssembly.StartCasting(gameBody, inputData, this, deltaTime);
        }

        internal void CancelCasting(GameBody gameBody, AbilityCastInputData inputData, double deltaTime) {
            EffectAssembly.CancelCasting(gameBody, inputData, this, deltaTime);
        }

        internal void Update_Internal(GameBody gameBody, AbilityCastInputData inputData, double deltaTime) {
            EffectAssembly.Update_Internal(gameBody, inputData, this, deltaTime);
        }
    }
}