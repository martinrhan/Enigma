using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Game.Entities.AbilityMechanisms {
    public class ChargeSlashMechanism : AbilityMechanism {
        public double ChargedTime { get; private set; }

        /// <summary>
        /// The idling stage of phase0
        /// </summary>
        public void Update_Phase0_Stage0(UpdateInterface updateInterface) {
        }
        /// <summary>
        /// Charge up an attack and strike into a rectangular area upon time up or when manually end earlier
        /// </summary>
        public void Update_Phase0_Stage1(UpdateInterface updateInterface) {
            ChargedTime += updateInterface.DeltaTime;
            if (updateInterface.IsCancelling) {
                updateInterface.DealRectangleAreaDamage();
            }

        }
    }
}
