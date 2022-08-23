using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Game {
    public abstract class EnemyWaveBehaviour {
        private bool isStarted = false;
        internal void Update_Internal(GameWorld gameWorld, double deltaTime) {
            UpdateInterface updateInterface = new UpdateInterface(gameWorld);
            if (!isStarted) {
                Start(updateInterface);
                isStarted = true;
            }
            Update(updateInterface);
        }
        protected abstract bool Start(UpdateInterface updateInterface);
        protected abstract bool Update(UpdateInterface updateInterface);

        public class UpdateInterface {
            internal UpdateInterface(GameWorld gameWorld) {
                this.GameWorld = gameWorld;
            }
            public GameWorld GameWorld { get; }
            public void ReportEliminatedStrength(int eliminatedStength) {
                
            }
            public void LaunchGameBody(GameBody gameBody) {
                GameWorld.OperationManager.PendAddGameBody(gameBody);
            }
        }

    }

    public class EnemyWaveBehaviourFactoryArguments {
        public IReadOnlyList<IReadOnlyList<AIGameBodyTemplate>> RolledAIGameBodyTemplates { get; init; }
    }
}
