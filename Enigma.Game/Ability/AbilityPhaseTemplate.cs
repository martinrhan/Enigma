using System.Collections.Generic;
using System.Linq;

namespace Enigma.Game {
    public class AbilityPhaseTemplate {
        [SourceGenerator.DontUseId]
        public IReadOnlyList<AbilityPhaseStageTemplate> StageTemplateList { get; init; }

        internal void Initialize(AbilityEffectAssemblyTemplate effectAssemblyTemplate) {
            foreach (AbilityPhaseStageTemplate stageTemplate in StageTemplateList) {
                stageTemplate.Initialize(effectAssemblyTemplate);
            }
        }

        internal AbilityPhase New() {
            return new AbilityPhase(this);
        }
    }
}
