using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Game {
    public class EnemyWave {
        internal EnemyWave(EnemyWaveBehaviour behaviour) {
            EnemyWaveBehaviour = behaviour;
        }

        public int WaveCount { get; init; }
        public int TotalStrength { get; init; }
        public int CurrentStrength { get; internal set; }
        public EnemyWaveBehaviour EnemyWaveBehaviour { get; }

        internal void Update(GameWorld gameWorld, double deltaTime) {
            EnemyWaveBehaviour.Update_Internal(gameWorld, deltaTime);
        }
    }
}
