global using AbilityMechanismFactory = Enigma.Game.Factory<Enigma.Game.AbilityMechanismFactoryArguments, Enigma.Game.AbilityMechanism>;
global using AIGameBodyBehaviourFactory = Enigma.Game.Factory<Enigma.Game.AIGameBodyBehaviourFactoryArguments, Enigma.Game.AIGameBodyBehaviour>;
global using EnemyWaveBehaviourFactory = Enigma.Game.Factory<Enigma.Game.EnemyWaveBehaviourFactoryArguments, Enigma.Game.EnemyWaveBehaviour>;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Enigma.Game {
    public class Factory<TArgument, TProduct> where TProduct : IFactoryProduct {
        private static readonly Dictionary<string, Factory<TArgument, TProduct>> dictionary = new Dictionary<string, Factory<TArgument, TProduct>>();
        internal static void Register(Factory<TArgument, TProduct> factory) => dictionary.Add(factory.Id, factory);
        public static IReadOnlyDictionary<string, Factory<TArgument, TProduct>> Dictionary => dictionary;
        public string Id { get; private init; }
        public Func<TArgument, TProduct> New { get; private init; }
        internal Type ProductType { get; private init; }

        /// <param name="type">The actual type of product, must be derived from TProduct.</param>
        /// <param name="requiredNamePostfix">The name of type should end with this, otherwise an InvalidOperationException will be thrown.</param>
        /// <returns>A factory which has it New function calling constructor of type.
        /// The type should has a public constuctor which has exactly one parameter of TArgument type, or no parameter. Otherwise, null will be returned.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static Factory<TArgument, TProduct> LoadType(Type type, string requiredNamePostfix) {
            string id = type.Name.EndsWith(requiredNamePostfix) ? type.Name.Substring(0, type.Name.Length - requiredNamePostfix.Length) :
                    throw new InvalidOperationException("The type name " + type.Name + " does not end with the requied postfix " + requiredNamePostfix);
            ParameterExpression factoryArgumentExpression = Expression.Parameter(typeof(TArgument));
            ConstructorInfo constructorInfo = type.GetConstructor(new Type[] { typeof(TArgument) });
            Expression[] constructorParameterExpressions;
            if (constructorInfo == null) {
                constructorInfo = type.GetConstructor(new Type[] { });
                if (constructorInfo == null) {
                    return null;
                } else {
                    constructorParameterExpressions = new Expression[] { };
                }
            } else {
                constructorParameterExpressions = new Expression[] { factoryArgumentExpression };
            }
            ParameterExpression productExpression = Expression.Variable(type, "product");
            NewExpression constructorCallExpression = Expression.New(constructorInfo, constructorParameterExpressions);
            BinaryExpression assignProductExpression = Expression.Assign(productExpression, constructorCallExpression);
            Func<TArgument, TProduct> newFunc = Expression.Lambda<Func<TArgument, TProduct>>(
                Expression.Block(
                    new ParameterExpression[] { productExpression },
                    assignProductExpression,
                    TProduct.FactoryLoadAddition(type, productExpression),
                    productExpression
                ),
                new ParameterExpression[] { factoryArgumentExpression }
            ).Compile();
            Factory<TArgument, TProduct> factory = new Factory<TArgument, TProduct>() {
                Id = id,
                New = newFunc,
                ProductType = type
            };
            return factory;
        }

    }

    public interface IFactoryProduct {
        /// <summary>
        /// After the constructor is called by the Factory's New function, the BlockExpression return by this will be inserted before the Factory's New function return its product.
        /// </summary>
        /// <param name="toLoadSubtype"></param>
        /// <param name="productExpression"></param>
        /// <returns></returns>
        public abstract static Expression FactoryLoadAddition(Type toLoadSubtype, ParameterExpression productExpression);
    }
}
