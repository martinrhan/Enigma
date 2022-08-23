using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enigma.Common.Math;
using Enigma.Game;

namespace Enigma.Game.Entities.EnemyGroups {
    public class GeneralCrowdBehaviour : EnemyWaveBehaviour {
        public GeneralCrowdBehaviour(EnemyWaveBehaviourFactoryArguments arguments) {
            this.arguments = arguments;
        }
        private readonly EnemyWaveBehaviourFactoryArguments arguments;

        protected override bool Start(UpdateInterface updateInterface) {
            AIGameBodyTemplate template = arguments.RolledAIGameBodyTemplates[0][0];
            Vector2 playerPosition = updateInterface.GameWorld.PlayerGameBodyList[0].Center;
            GameBody gameBody = new GameBody(template, new AIGameBodyBehaviourFactoryArguments(), playerPosition + new Vector2(100, 100));
            updateInterface.LaunchGameBody(gameBody);
            return false;
        }

        protected override bool Update(UpdateInterface updateInterface) {

            return false;
        }
    }


}
