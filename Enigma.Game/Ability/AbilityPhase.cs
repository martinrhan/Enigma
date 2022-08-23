using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Game {
    public class AbilityPhase {
        internal AbilityPhase(AbilityPhaseTemplate template) {
            Template = template;
            stageList = template.StageTemplateList.Select(template => template.New()).ToArray();
        }
        public AbilityPhaseTemplate Template { get; }

        internal readonly AbilityPhaseStage[] stageList;
        public IReadOnlyList<AbilityPhaseStage> StageList => StageList;
        /// <summary>
        /// -1 means idling, the ability is not being cast.
        /// </summary>
        public int CurrentStageIndex { get; private set; } = -1;
        public AbilityPhaseStage CurrentStage => CurrentStageIndex == -1 ? null : stageList[CurrentStageIndex];
        public bool IsIdling => CurrentStageIndex == -1;

        internal void StartCasting(AbilityEffectAssembly.UpdateInterface updateInterface) {
            if (CurrentStageIndex != -1) return;
            CurrentStageIndex = 0;
            CurrentStage.Start_Internal(updateInterface);
        }
        public double CurrentStageElapsedTime { get; private set; } = 0;
        internal void CancelCasting(AbilityEffectAssembly.UpdateInterface updateInterface) {
            if (CurrentStageIndex == -1) return;
            CurrentStageIndex = -1;
            CurrentStage.Cancel_Internal(updateInterface);
            CurrentStageElapsedTime = 0;
        }

        internal ReturnedValue_Update_Internal Update_Internal(AbilityEffectAssembly.UpdateInterface updateInterface) {
            bool gotoNextPhase = false;
            if (CurrentStageIndex == -1) {

            } else {
                CurrentStage.Update_Internal(updateInterface);
                if (CurrentStageElapsedTime >= CurrentStage.MaxTime) {
                    CurrentStage.Complete_Internal(updateInterface);
                    CurrentStageIndex += 1;
                    if (CurrentStageIndex == stageList.Length) {
                        CurrentStageIndex = -1;
                        CurrentStageElapsedTime = 0;
                        gotoNextPhase = true;
                    } else {
                        CurrentStageElapsedTime -= CurrentStage.MaxTime;
                    }
                }
                CurrentStageElapsedTime += updateInterface.DeltaTime;
            }
            return new ReturnedValue_Update_Internal() {
                GotoNextPhase = gotoNextPhase,
            };
        }
        internal struct ReturnedValue_Update_Internal {
            internal bool GotoNextPhase { get; init; }
        }
    }
}
