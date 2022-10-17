using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using Enigma.Common.Math;

namespace Enigma.Game {
    public class AbilityMechanism : IFactoryProduct {
        public static Expression FactoryLoadAddition(Type toLoadSubtype, ParameterExpression productExpression) {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            List<Expression> arguments = new List<Expression>();
            foreach (MethodInfo methodInfo in toLoadSubtype.GetMethods().Where(mi => mi.Name.StartsWith("Update"))) {
                string remainingName = methodInfo.Name[6..];
                if (!remainingName.StartsWith("_Phase")) throw new Exception("The method name " + methodInfo.Name + " has incorrect format.");
                remainingName = remainingName[6..];
                int nextUnderscoreIndex = remainingName.IndexOf('_');
                int phaseIndex = int.Parse(remainingName[..nextUnderscoreIndex]);
                remainingName = remainingName[nextUnderscoreIndex..];
                if (!remainingName.StartsWith("_Stage")) throw new Exception("The method name " + methodInfo.Name + " has incorrect format.");
                remainingName = remainingName[6..];
                int stageIndex = int.Parse(remainingName);
                ParameterExpression updateInterfaceExpression = Expression.Parameter(typeof(UpdateInterface));
                var updateMethodExpression = Expression.Lambda<Action<UpdateInterface>>(Expression.Call(productExpression, methodInfo, updateInterfaceExpression), updateInterfaceExpression);
                ConstructorInfo constructorInfo = typeof(PhaseStageUpdateMethodInfo).GetConstructors(bindingFlags)[0];
                NewExpression newMethodInfoExpression = Expression.New(constructorInfo, Expression.Constant(phaseIndex), Expression.Constant(stageIndex), updateMethodExpression);
                arguments.Add(newMethodInfoExpression);
            }
            NewArrayExpression arrayExpression = Expression.NewArrayInit(typeof(PhaseStageUpdateMethodInfo),arguments);
            MethodCallExpression methodCallExpression = Expression.Call(productExpression, typeof(AbilityMechanism).GetMethod(nameof(SetPhaseStageUpdateMethods), bindingFlags), arrayExpression);
            return methodCallExpression;
        }

        private Action<UpdateInterface>[][] phaseStageUpdateMethods;
        internal readonly struct PhaseStageUpdateMethodInfo {
            internal PhaseStageUpdateMethodInfo(int phaseIndex, int stageIndex, Action<UpdateInterface> updateMethod) {
                PhaseIndex = phaseIndex;
                StageIndex = stageIndex;
                UpdateMethod = updateMethod;
            }
            internal int PhaseIndex { get; }
            internal int StageIndex { get; }
            internal Action<UpdateInterface> UpdateMethod { get; }
        }
        internal void SetPhaseStageUpdateMethods(PhaseStageUpdateMethodInfo[] infos) {
            int MaxPhaseIndex = infos.Max(info => info.PhaseIndex);
            Action<UpdateInterface>[][] array = new Action<UpdateInterface>[MaxPhaseIndex + 1][];
            for (int i = 0; i < array.Length; i++) {
                array[i] = new Action<UpdateInterface>[1];
            }
            foreach (var info in infos) {
                if (array[info.PhaseIndex].Length <= info.StageIndex) {
                    Array.Resize(ref array[info.PhaseIndex], info.StageIndex + 1);
                }
                array[info.PhaseIndex][info.StageIndex] = info.UpdateMethod;
            }
            phaseStageUpdateMethods = array;
            phaseStagesElapsedTime = array.Select(a => new double[a.Length]).ToArray();
        }

        public int PhaseCount => phaseStageUpdateMethods.Length;
        public int CurrentPhaseIndex { get; private set; }
        public int CurrentPhaseStageCount => phaseStageUpdateMethods[CurrentPhaseIndex].Length;
        public int CurrentStageIndex { get; private set; }

        private double[][] phaseStagesElapsedTime;
        public IReadOnlyList<IReadOnlyList<double>> PhaseStagesElapsedTime => phaseStagesElapsedTime;

        private void GotoNextStage() {
            CurrentStageIndex++;
            if (CurrentStageIndex == CurrentPhaseStageCount) {
                CurrentStageIndex = 0;
                CurrentPhaseIndex++;
                if (CurrentPhaseIndex == PhaseCount) {
                    CurrentPhaseIndex = 0;
                    for (int i = 0; i < phaseStagesElapsedTime.Length; i++) {
                        Array.Fill(phaseStagesElapsedTime[i], 0);
                    }
                }
            }
            currentStageStarting = true;
        }
        private void GotoStage0() {
            CurrentStageIndex = 0;
            Array.Fill(phaseStagesElapsedTime[CurrentPhaseIndex], 0);
            currentStageStarting = true;
        }
        private bool isStartCasting = false;
        public void StartCasting() {
            if (CurrentStageIndex != 0) {
                return;
            }
            isStartCasting = true;
        }
        private bool isCancelCasting = false;
        public void CancelCasting() {
            if (CurrentStageIndex == 0) {
                return;
            }
            isCancelCasting = true;
        }
        private bool currentStageStarting = true;
        internal void Update_Internal(GameBody caster, AbilityCastInputData inputData, Ability ability, double deltaTime) {
            if (isStartCasting || isCancelCasting) {
                UpdateInterface updateInterface_beforeChangeStage = new UpdateInterface() {
                    Ability = ability,
                    InputData = inputData,
                    Caster = caster,
                    DeltaTime = deltaTime,
                    IsStarting = currentStageStarting,
                    IsCancelling = true
                };
                var method_beforeChangeStage = phaseStageUpdateMethods[CurrentPhaseIndex][CurrentStageIndex];
                if (method_beforeChangeStage != null) method_beforeChangeStage(updateInterface_beforeChangeStage);
                if (isStartCasting) {
                    GotoNextStage();
                } else {
                    GotoStage0();
                }
            }
            UpdateInterface updateInterface = new UpdateInterface() {
                Ability = ability,
                InputData = inputData,
                Caster = caster,
                DeltaTime = deltaTime,
                IsStarting = currentStageStarting,
                IsCancelling = false
            };
            var method = phaseStageUpdateMethods[CurrentPhaseIndex][CurrentStageIndex];
            if (method != null) method(updateInterface);
            currentStageStarting = false;
            phaseStagesElapsedTime[CurrentPhaseIndex][CurrentStageIndex] += deltaTime;
        }

        public class UpdateInterface {
            internal UpdateInterface() {
            }
            public GameBody Caster { get; init; }
            public AbilityCastInputData InputData { get; init; }
            public Ability Ability { get; init; }
            public double DeltaTime { get; init; }

            public bool IsStarting { get; init; }
            public bool IsCancelling { get; init; }

            internal double ReportedStageProgress { get; private set; }
            /// <summary>
            /// Report the progress of current stage so the Mechanism determine whether the stage is completed. Furthermore, the displayed progress bar also depend on this.
            /// </summary>
            /// <param name="progress">Ranges from 0 to 1, if greater than 1, current stage is completed</param>
            /// <exception cref="ArgumentOutOfRangeException">When progress is less than 0.</exception>
            public void ReportStageProgress(double progress) {
                if (progress < 0) throw new ArgumentOutOfRangeException(nameof(progress), "Progress cannot be less than 0");
                ReportedStageProgress = progress;
            }

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
            public void Translate(GameBody gameBody, Vector2 displacement) {
                Caster.GameWorld.OperationManager.PendAdditionalDisplacement(gameBody, displacement);
            }
            public void DealCircularSectorAreaDamage(double radius, double theta, double damage) {
                //Caster.GameWorld.CircleHitTest

            }
            public void DealRectangleAreaDamage() {

            }
            public void TranslateGameBody() {

            }
        }
    }

    public struct AbilityMechanismFactoryArguments {

    }
}
