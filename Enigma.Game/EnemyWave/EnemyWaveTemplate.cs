using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enigma.Common;
using Enigma.Common.Math;

namespace Enigma.Game {
    public class EnemyWaveTemplate : IRarityObject {
        internal static Dictionary<string, EnemyWaveTemplate> dictionary = new Dictionary<string, EnemyWaveTemplate>();
        public static IReadOnlyDictionary<string, EnemyWaveTemplate> Dictionary => dictionary;

        public string Id { get; init; }
        public Rarity Rarity { get; init; }
        public EnemyWaveBehaviourFactory EnemyWaveBehaviourFactory { get; init; }
        public IReadOnlyList<AIGameBodyTemplatePool> AIGameBodyTemplatePoolList { get; init; }

        public EnemyWave NewEnemyWave(EnemyWaveBehaviourFactoryArguments arguments) {
            return new EnemyWave(EnemyWaveBehaviourFactory.New(arguments));
        }
    }

}
