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
            AbilityMechanism = template.AbilityMechanismFactory.New(new());
        }
        public AbilityTemplate Template { get; }
        public string GivenName { get; set; }
        public int Size => Template.Size;
        public AbilityMechanism AbilityMechanism { get; }

        public AbilityItem ConvertToAbilityItem() => new AbilityItem(Template);

        internal void StartCasting() {
            AbilityMechanism.StartCasting();
        }

        internal void CancelCasting() {
            AbilityMechanism.CancelCasting();
        }

        internal void Update_Internal(GameBody gameBody, AbilityCastInputData inputData, double deltaTime) {
            AbilityMechanism.Update_Internal(gameBody, inputData, this, deltaTime);
        }
    }
}