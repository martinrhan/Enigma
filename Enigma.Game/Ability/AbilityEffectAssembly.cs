using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Enigma.Game.Extensions.GameWorldExtensions;

namespace Enigma.Game {
    public class AbilityEffectAssembly {
        internal AbilityEffectAssembly(AbilityEffectAssemblyTemplate template) {
            Template = template;
            phaselist = template.PhaseTemplateList.Select(template => template.New()).ToArray();
        }
        public AbilityEffectAssemblyTemplate Template { get; }

        internal readonly AbilityPhase[] phaselist;
        public IReadOnlyList<AbilityPhase> Phaselist => phaselist;

        public int CurrentPhaseIndex { get; private set; } = 0;
        public AbilityPhase CurrentPhase => phaselist[CurrentPhaseIndex];

        internal void StartCasting(GameBody gameBody, AbilityCastInputData inputData, Ability ability, double deltaTime) {
            CurrentPhase.StartCasting(new UpdateInterface(gameBody, inputData, ability, deltaTime));
        }

        internal void CancelCasting(GameBody gameBody, AbilityCastInputData inputData, Ability ability, double deltaTime) {
            CurrentPhase.CancelCasting(new UpdateInterface(gameBody, inputData, ability, deltaTime));
        }

        internal void Update_Internal(GameBody gameBody, AbilityCastInputData inputData, Ability ability, double deltaTime) {
            var returnedValue = CurrentPhase.Update_Internal(new UpdateInterface(gameBody, inputData, ability, deltaTime));
            if (returnedValue.GotoNextPhase) {
                CurrentPhaseIndex += 1;
                if (CurrentPhaseIndex == phaselist.Length) {
                    CurrentPhaseIndex -= phaselist.Length;
                }
            }
        }


        public class UpdateInterface {
            internal UpdateInterface(GameBody caster, AbilityCastInputData inputData, Ability ability, double deltaTime) {
                Ability = ability;
                InputData = inputData;
                Caster = caster;
                DeltaTime = deltaTime;
            }
            public GameBody Caster { get; init; }
            public AbilityCastInputData InputData { get; init; }
            public Ability Ability { get; init; }
            public double DeltaTime { get; init; }
            public void CreateCircleAreaEffect() {

            }
            public void CreateRayEffect() {

            }
            public void LaunchGameBody(GameBody gameBody) {
                Caster.GameWorld.OperationManager.PendAddGameBody(gameBody);
            }
            public void DestroyGameBody(GameBody gameBody) {
                Caster.GameWorld.OperationManager.PendDestroyGameBody(gameBody);
            }
            public void DealCircularSectorAreaDamage(double radius, double theta, double damage) {
                //Caster.GameWorld.CircleHitTest

            }
            public void DealRectangleAreaDamage() {

            }
        }
    }
}
