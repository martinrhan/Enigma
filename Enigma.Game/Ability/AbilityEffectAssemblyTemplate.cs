using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Game {
    public class AbilityEffectAssemblyTemplate {
        [SourceGenerator.DontUseId]
        public IReadOnlyList<AbilityPhaseTemplate> PhaseTemplateList { get; init; }

        internal void Initialize() {
            foreach (AbilityPhaseTemplate template in PhaseTemplateList) {
                template.Initialize(this);
            }
        }
        internal ReturnedValue_FindComponentData FindComponentData(string Id) {
            for (int phaseTemplateIndex = 0; phaseTemplateIndex < PhaseTemplateList.Count; phaseTemplateIndex++) {
                AbilityPhaseTemplate phaseTemplate = PhaseTemplateList[phaseTemplateIndex];
                for (int stageTemplateIndex = 0; stageTemplateIndex < phaseTemplate.StageTemplateList.Count; stageTemplateIndex++) {
                    AbilityPhaseStageTemplate stageTemplate = phaseTemplate.StageTemplateList[stageTemplateIndex];
                    int inPositionIndex;
                    for (inPositionIndex = 0; inPositionIndex < stageTemplate.StartComponentMetadataList.Count; inPositionIndex++) {
                        if (stageTemplate.StartComponentMetadataList[inPositionIndex].Id == Id) {
                            return new() {
                                PhaseTemplateIndex = phaseTemplateIndex,
                                StageTemplateIndex = stageTemplateIndex,
                                Position = 0,
                                InPositionIndex = inPositionIndex
                            };
                        }
                    }
                    for (inPositionIndex = 0; inPositionIndex < stageTemplate.UpdateComponentMetadataList.Count; inPositionIndex++) {
                        if (stageTemplate.UpdateComponentMetadataList[inPositionIndex].Id == Id) {
                            return new() {
                                PhaseTemplateIndex = phaseTemplateIndex,
                                StageTemplateIndex = stageTemplateIndex,
                                Position = 1,
                                InPositionIndex = inPositionIndex
                            };
                        }
                    }
                    for (inPositionIndex = 0; inPositionIndex < stageTemplate.CancelComponentMetadataList.Count; inPositionIndex++) {
                        if (stageTemplate.CancelComponentMetadataList[inPositionIndex].Id == Id) {
                            return new() {
                                PhaseTemplateIndex = phaseTemplateIndex,
                                StageTemplateIndex = stageTemplateIndex,
                                Position = 2,
                                InPositionIndex = inPositionIndex
                            };
                        }
                    }
                    for (inPositionIndex = 0; inPositionIndex < stageTemplate.CompleteComponentMetadataList.Count; inPositionIndex++) {
                        if (stageTemplate.CompleteComponentMetadataList[inPositionIndex].Id == Id) {
                            return new() {
                                PhaseTemplateIndex = phaseTemplateIndex,
                                StageTemplateIndex = stageTemplateIndex,
                                Position = 3,
                                InPositionIndex = inPositionIndex
                            };
                        }
                    }
                }
            }
            throw new ArgumentException("Cannot find a ComponentData with given Id.");
        }

        internal struct ReturnedValue_FindComponentData {
            internal int PhaseTemplateIndex { get; init; }
            internal int StageTemplateIndex { get; init; }
            internal int Position { get; init; }
            internal int InPositionIndex { get; init; }
        }

        public AbilityEffectAssembly New() {
            return new AbilityEffectAssembly(this);
        }
    }
}
