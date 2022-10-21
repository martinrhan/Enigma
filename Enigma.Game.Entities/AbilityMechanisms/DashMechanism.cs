using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enigma.Game.Extensions;

namespace Enigma.Game.Entities.AbilityMechanisms {
    public class DashMechanism : AbilityMechanism {
        /// <summary>
        /// The idling stage of phase0
        /// </summary>
        public void Update_Phase0_Stage0(UpdateInterface updateInterface) {
        }
        /// <summary>
        /// 
        /// </summary>
        public void Update_Phase0_Stage1(UpdateInterface updateInterface) {
            updateInterface.Translate(updateInterface.Caster,
                updateInterface.Caster.GameWorld.GetShortestDisplacement(updateInterface.Caster.Center, updateInterface.InputData.TargetPoint).ChangeLength(1) * (updateInterface.DeltaTime * 600)
            );
            updateInterface.ReportStageProgress(PhaseStagesElapsedTime[0][1] / 0.5);
        }
    }
}
