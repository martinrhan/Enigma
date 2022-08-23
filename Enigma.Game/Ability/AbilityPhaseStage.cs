using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Game {
    public class AbilityPhaseStage {
        internal AbilityPhaseStage(AbilityPhaseStageTemplate template) {
            Template = template;
            startComponents = template.StartComponentMetadataList.Select(data => data.Factory.New(new())).ToArray();
            updateComponents = template.UpdateComponentMetadataList.Select(data => data.Factory.New(new())).ToArray();
            cancelComponents = template.CancelComponentMetadataList.Select(data => data.Factory.New(new())).ToArray();
            completeComponents = template.CompleteComponentMetadataList.Select(data => data.Factory.New(new())).ToArray();
        }
        public float MaxTime { get; init; }

        public AbilityPhaseStageTemplate Template { get; }

        internal readonly AbilityEffectComponent[] startComponents;
        internal readonly AbilityEffectComponent[] updateComponents;
        internal readonly AbilityEffectComponent[] cancelComponents;
        internal readonly AbilityEffectComponent[] completeComponents;

        internal void Start_Internal(AbilityEffectAssembly.UpdateInterface updateInterface) {
            Template.Start(this, updateInterface);
        }
        internal void Update_Internal(AbilityEffectAssembly.UpdateInterface updateInterface) {
            Template.Update(this, updateInterface);
        }
        internal void Cancel_Internal(AbilityEffectAssembly.UpdateInterface updateInterface) {
            Template.Cancel(this, updateInterface);
        }
        internal void Complete_Internal(AbilityEffectAssembly.UpdateInterface updateInterface) {
            Template.Complete(this, updateInterface);
        }
    }
}
