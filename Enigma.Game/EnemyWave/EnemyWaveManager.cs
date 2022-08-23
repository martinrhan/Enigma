using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Game {
    public class EnemyWaveManager : RarityObjectRoller<EnemyWaveTemplate> {
        internal EnemyWaveManager() {
            RollItems();
        }
        private readonly EnemyWaveTemplate[] selectableEnemyWaveTemplates = new EnemyWaveTemplate[3];
        public IReadOnlyList<EnemyWaveTemplate> SelectableEnemyWaveTemplateList => selectableEnemyWaveTemplates;
        public void RollItems() {
            for (int i = 0; i < selectableEnemyWaveTemplates.Length; i++) {
                selectableEnemyWaveTemplates[i] = RollItem_Protected();
            }
        }
        public void LaunchNewEnemyWave(int index) {
            if (CurrentEnemyWave != null) throw new InvalidOperationException("Cannot make a new EnemyWave when there is already an ongoing EnemyWave");
            CurrentEnemyWave = selectableEnemyWaveTemplates[index].NewEnemyWave(new EnemyWaveBehaviourFactoryArguments() {
                RolledAIGameBodyTemplates = selectableEnemyWaveTemplates[index].AIGameBodyTemplatePoolList.Select(pool => pool.RollAIGameBodyTemplates().ToArray()).ToArray()
            });
        }

        public EnemyWave CurrentEnemyWave { get; private set; }
        public int CurrentWaveCount { get; private set; }
    }
}
