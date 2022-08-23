using System;
using System.Collections.Generic;
using System.Linq;
using Enigma.Common;
using Enigma.Common.Math;
using Enigma.PhysicsEngine;

namespace Enigma.Game {
    public class GameBody : CirclePhysicsBody {
        public GameBody(AIGameBodyTemplate template, AIGameBodyBehaviourFactoryArguments arguments, in Vector2 center) {
            AbilityCollection = template.AbilityCollection.DeepCopy();
            Behaviour = template.AIGameBodyBehaviourFactory.New(arguments);
            Center = center;
            PropertyBasicValues = template.Properties;
            for (int i = 0; i < modifierCollectionArray.Length; i++) {
                modifierCollectionArray[i] = new FloatModifierCollection();
            }
        }
        internal GameBody(AbilityCollection abilityCollection, GameBodyBehaviour behaviour, in Vector2 center, in PropertyBuffer propertyBasicValues) {
            AbilityCollection = abilityCollection;
            Behaviour = behaviour;
            Center = center;
            PropertyBasicValues = propertyBasicValues;
            for (int i = 0; i < modifierCollectionArray.Length; i++) {
                modifierCollectionArray[i] = new FloatModifierCollection();
            }
        }
        public override double Radius => this[Property.Radius];

        public GameWorld GameWorld { get; internal set; }
        public CollisionGroup CollisionGroup { get; internal set; }
        public AbilityCollection AbilityCollection { get; }
        public int CurrentHP { get; internal set; }
        public int CurrentShield { get; internal set; }

        public PropertyBuffer PropertyBasicValues { get; }
        private readonly FloatModifierCollection[] modifierCollectionArray = new FloatModifierCollection[5];

        public double this[Property property] => modifierCollectionArray[(int)property].GetModified(PropertyBasicValues[property]);

        public Team BelongedTeam { get; set; }

        internal void SetMovement(double direction) {
            Velocity = new Vector2(this[Property.Speed] * Math.Cos(direction), this[Property.Speed] * Math.Sin(direction));
        }
        internal void SetMovement(double direction, double speedProportion) {
            Velocity = new Vector2(this[Property.Speed] * Math.Cos(direction) * speedProportion, this[Property.Speed] * Math.Sin(direction) * speedProportion);
        }
        internal void SetMovementToZero() {
            Velocity = Vector2.Zero;
        }
        internal void StepMovement(double deltaTime) {
            Vector2 displacement = Velocity * deltaTime;
            Center += displacement;
        }
        internal void Translate(Vector2 displacement) {
            Center += displacement;
        }

        public GameBodyBehaviour Behaviour { get; }
        internal void Update_Internal(double deltaTime) {
            Behaviour.Update_Internal(this, deltaTime);
        }

        public enum Property : int { MaxHP, MaxShield, Armour, Speed, Radius}
        public unsafe struct PropertyBuffer {
            static PropertyBuffer() {
                dictionary = Enum.GetValues<Property>().ToDictionary(property => property.ToString());
            }
            [SourceGenerator.JsonConstructor]
            public static PropertyBuffer FromDictionary(IDictionary<string, double> dictionary) {
                PropertyBuffer buffer = new PropertyBuffer();
                foreach (string key in dictionary.Keys) {
                    buffer.buffer[(int)PropertyBuffer.dictionary[key]] = dictionary[key];
                }
                return buffer;
            }
            private static readonly IReadOnlyDictionary<string, Property> dictionary;
            public double this[Property property] {
                get => buffer[(int)property];
                init {
                    if (value < 0) throw new ArgumentOutOfRangeException("Property values cannot be negative");
                    buffer[(int)property] = value;
                }
            }
            private fixed double buffer[5];
        }
    }

    public enum Team { Player, NonPlayer }
}