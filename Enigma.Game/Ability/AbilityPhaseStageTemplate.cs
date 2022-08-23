using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Enigma.Game {
    public partial class AbilityPhaseStageTemplate {
        public IReadOnlyList<AbilityEffectComponentMetadata> StartComponentMetadataList { get; }
        public IReadOnlyList<AbilityEffectComponentMetadata> UpdateComponentMetadataList { get; }
        public IReadOnlyList<AbilityEffectComponentMetadata> CancelComponentMetadataList { get; }
        public IReadOnlyList<AbilityEffectComponentMetadata> CompleteComponentMetadataList { get; }

        internal Action<AbilityPhaseStage, AbilityEffectAssembly.UpdateInterface> Start;
        internal Action<AbilityPhaseStage, AbilityEffectAssembly.UpdateInterface> Update;
        internal Action<AbilityPhaseStage, AbilityEffectAssembly.UpdateInterface> Cancel;
        internal Action<AbilityPhaseStage, AbilityEffectAssembly.UpdateInterface> Complete;

        internal bool IsInitialized { get; private set; }
        private static readonly Func<string>[] componentsFieldNames = new Func<string>[]{
            () => nameof(AbilityPhaseStage.startComponents),
            () => nameof(AbilityPhaseStage.updateComponents),
            () => nameof(AbilityPhaseStage.cancelComponents),
            () => nameof(AbilityPhaseStage.completeComponents),
        };
        private static readonly Func<AbilityPhaseStageTemplate, IReadOnlyList<AbilityEffectComponentMetadata>>[] componentMetadataLists = new Func<AbilityPhaseStageTemplate, IReadOnlyList<AbilityEffectComponentMetadata>>[]{
            template => template.StartComponentMetadataList,
            template => template.UpdateComponentMetadataList,
            template => template.CancelComponentMetadataList,
            template => template.CompleteComponentMetadataList,
        };
        internal void Initialize(AbilityEffectAssemblyTemplate effectAssemblyTemplate) {
            Start = GetExpression(effectAssemblyTemplate, 0).Compile();
            Update = GetExpression(effectAssemblyTemplate, 1).Compile();
            Cancel = GetExpression(effectAssemblyTemplate, 2).Compile();
            Complete = GetExpression(effectAssemblyTemplate, 3).Compile();
        }
        private Expression<Action<AbilityPhaseStage, AbilityEffectAssembly.UpdateInterface>> GetExpression(AbilityEffectAssemblyTemplate effectAssemblyTemplate, int positionIndex) {
            List<BlockExpression> blockExpressions = new List<BlockExpression>();
            ParameterExpression updateInterfaceExpression = Expression.Parameter(typeof(AbilityEffectAssembly.UpdateInterface));
            ParameterExpression abilityPhaseStageExpression = Expression.Parameter(typeof(AbilityPhaseStage));
            IReadOnlyList<AbilityEffectComponentMetadata> componentMetadataList = componentMetadataLists[positionIndex](this);
            for (int componentIndex = 0; componentIndex < componentMetadataList.Count; componentIndex++) {
                AbilityEffectComponentMetadata componentMetadata = componentMetadataList[componentIndex];
                List<Expression> localVariableExpresssions = new();
                List<Expression> assignLocalVariableExpressions = new();
                foreach (var sourceInfo in componentMetadata.InvokeArgumentDataSourceInfoList) {
                    var result = effectAssemblyTemplate.FindComponentData(sourceInfo.SourceComponentId);
                    MemberExpression abilityExpression = Expression.Property(updateInterfaceExpression, nameof(AbilityEffectAssembly.UpdateInterface.Ability));
                    MemberExpression effectAssemblyExpression = Expression.Property(abilityExpression, nameof(Ability.EffectAssembly));
                    MemberExpression phaseListExpression = Expression.Field(effectAssemblyExpression, nameof(AbilityEffectAssembly.phaselist));
                    BinaryExpression phaseExpression_source = Expression.ArrayIndex(phaseListExpression, Expression.Constant(result.PhaseTemplateIndex));
                    MemberExpression stageListExpression_source = Expression.Field(phaseExpression_source, nameof(AbilityPhase.stageList));
                    BinaryExpression stageExpression = Expression.ArrayIndex(stageListExpression_source, Expression.Constant(result.StageTemplateIndex));
                    MemberExpression startComponentsExpression_source = Expression.Field(stageExpression, componentsFieldNames[result.Position]());
                    BinaryExpression componentExpression_source = Expression.ArrayIndex(startComponentsExpression_source, Expression.Constant(result.InPositionIndex));
                    AbilityEffectComponentMetadata sourceComponentMetadata = componentMetadataLists[result.Position](effectAssemblyTemplate.PhaseTemplateList[result.PhaseTemplateIndex].StageTemplateList[result.StageTemplateIndex])[result.InPositionIndex];
                    UnaryExpression convertedComponentExpression_source = Expression.Convert(componentExpression_source, sourceComponentMetadata.Factory.ProductType);
                    MemberExpression sourcePropertyExpression = Expression.Property(convertedComponentExpression_source, sourceInfo.SourcePropertyId);
                    ParameterExpression localVariableExpresssion = Expression.Variable(sourceComponentMetadata.Factory.ProductType, sourceInfo.InExpressionId);
                    localVariableExpresssions.Add(localVariableExpresssion);
                    BinaryExpression assignLocalVariableExpression = Expression.Assign(localVariableExpresssion, convertedComponentExpression_source);
                    assignLocalVariableExpressions.Add(assignLocalVariableExpression);
                }
                MethodInfo invokeMethodInfo = componentMetadata.Factory.ProductType.GetMethod("Invoke");
                Type argumentsType = invokeMethodInfo.GetParameters()[1].GetType();
                NewExpression newArgumentsExpression = Expression.New(argumentsType.GetConstructor(new Type[0]));
                List<MemberBinding> memberBindings = new List<MemberBinding>();
                foreach (AbilityEffectComponentMetadata.InvokeArgumentMetadata argumentMetadata in componentMetadata.InvokeArgumentMetadataList) {
                    Expression convertExpression = CSharpScript.EvaluateAsync<Expression>(argumentMetadata.ConvertExpression, ScriptOptions.Default).Result;
                    memberBindings.Add(Expression.Bind(argumentsType.GetProperty(argumentMetadata.ArgumentId), convertExpression));
                }
                MemberInitExpression argumentsMemberInitExpression = Expression.MemberInit(newArgumentsExpression, memberBindings);
                ParameterExpression argumentsExpression = Expression.Parameter(argumentsType);
                BinaryExpression assignArgumentsExpression = Expression.Assign(argumentsExpression, argumentsMemberInitExpression);
                MemberExpression componentsExpression = Expression.Field(abilityPhaseStageExpression, componentsFieldNames[positionIndex]());
                BinaryExpression componentExpression = Expression.ArrayIndex(componentsExpression, Expression.Constant(componentIndex));
                UnaryExpression convertedComponentExpression = Expression.Convert(componentExpression, componentMetadata.Factory.ProductType);
                MethodCallExpression invokeExpression = Expression.Call(convertedComponentExpression, invokeMethodInfo, updateInterfaceExpression, argumentsExpression);
                BlockExpression blockExpression = Expression.Block(
                    localVariableExpresssions.Concat(assignLocalVariableExpressions).Concat(new Expression[] {
                        argumentsExpression,
                        assignArgumentsExpression,
                        invokeExpression
                    })
                );
                blockExpressions.Add(blockExpression);
            }
            return Expression.Lambda<Action<AbilityPhaseStage, AbilityEffectAssembly.UpdateInterface>>(
                Expression.Block(
                    blockExpressions
                ),
                abilityPhaseStageExpression,
                updateInterfaceExpression
            );
        }

        internal AbilityPhaseStage New() {
            return new AbilityPhaseStage(this);
        }
    }
}
