global using AbilityEffectAssemblyFactory = Enigma.Game.Factory<Enigma.Game.AbilityEffectComponentFactoryArguments, Enigma.Game.AbilityEffectComponent>;
global using AIGameBodyBehaviourFactory = Enigma.Game.Factory<Enigma.Game.AIGameBodyBehaviourFactoryArguments, Enigma.Game.AIGameBodyBehaviour>;
global using EnemyWaveBehaviourFactory = Enigma.Game.Factory<Enigma.Game.EnemyWaveBehaviourFactoryArguments, Enigma.Game.EnemyWaveBehaviour>;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Enigma.Game {
    public class Factory<TArgument, T> {
        private static readonly Dictionary<string, Factory<TArgument, T>> dictionary = new Dictionary<string, Factory<TArgument, T>>();
        internal static void Register(Factory<TArgument, T> factory) => dictionary.Add(factory.Id, factory);
        public static IReadOnlyDictionary<string, Factory<TArgument, T>> Dictionary => dictionary;
        public string Id { get; private init; }
        public Func<TArgument, T> New { get; private init; }
        internal Type ProductType { get; private init; }

        public static Factory<TArgument, T> LoadType(Type type, string requiredNamePostfix) {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TArgument));
            ConstructorInfo constructorInfo = type.GetConstructor(new Type[] { typeof(TArgument) });
            Expression[] parameterExpressions;
            if (constructorInfo == null) {
                constructorInfo = type.GetConstructor(new Type[] { });
                if (constructorInfo == null) {
                    return null;
                } else {
                    parameterExpressions = new Expression[] { };
                }
            } else {
                parameterExpressions = new Expression[] { parameterExpression };
            }
            Factory<TArgument, T> factory = new Factory<TArgument, T>() {
                Id = type.Name.EndsWith(requiredNamePostfix) ? type.Name.Substring(0, type.Name.Length - requiredNamePostfix.Length) :
                    throw new InvalidOperationException("The type name " + type.Name + " does not end with the requied postfix " + requiredNamePostfix),
                New = Expression.Lambda<Func<TArgument, T>>(
                    Expression.New(constructorInfo, parameterExpressions),
                    new ParameterExpression[] { parameterExpression }
                ).Compile(),
                ProductType = type
            };
            return factory;
        }

    }
}
