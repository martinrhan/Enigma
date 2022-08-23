using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Common {
    public interface IModifierSource {
        public string Name { get; }
    }

    public readonly struct IntModifier {
        public IntModifier(int value, IModifierSource source) {
            Value = value;
            Source = source;
        }
        public readonly int Value;
        public readonly IModifierSource Source;
    }
    public readonly struct FloatModifier {
        public FloatModifier(float value,IModifierSource source) {
            Value = value;
            Source = source;
        }
        public readonly float Value;
        public readonly IModifierSource Source;
    }

    public class IntModifierCollection {
        private readonly HashSet<IntModifier> plusModifierHashSet = new HashSet<IntModifier>();
        public int totalPlus { get; private set; }
        public IEnumerable<IntModifier> GetPlusModifiers => plusModifierHashSet;
        public void AddPlusModifier(IntModifier modifier) {
            plusModifierHashSet.Add(modifier);
            totalPlus += modifier.Value;
        }
        public bool RemovePlusModifier(IntModifier modifier) {
            if (plusModifierHashSet.Remove(modifier)) {
                totalPlus -= modifier.Value;
                return true;
            } else return false;
        }

        private readonly HashSet<IntModifier> multiplyModifierHashSet = new HashSet<IntModifier>();
        public float totalMultiply { get; private set; } = 1f;
        public IEnumerable<IntModifier> GetMultiplyModifiers => multiplyModifierHashSet;
        public void AddMultiplyModifiers(IntModifier modifier) {
            multiplyModifierHashSet.Add(modifier);
            totalMultiply += modifier.Value;
        }
        public bool RemoveMultiplyModifiers(IntModifier modifier) {
            if (multiplyModifierHashSet.Remove(modifier)) {
                totalMultiply -= modifier.Value;
                return true;
            } else return false;
        }
        public int GetModified(int original) {
            return (int)((original + totalPlus) * totalMultiply);
        }
    }

    public class FloatModifierCollection {
        private readonly HashSet<FloatModifier> plusModifierHashSet = new HashSet<FloatModifier>();
        private float totalPlus;
        public IEnumerable<FloatModifier> GetPlusModifiers => plusModifierHashSet;
        public void AddPlusModifier(FloatModifier modifier) {
            plusModifierHashSet.Add(modifier);
            totalPlus += modifier.Value;
        }
        public bool RemovePlusModifier(FloatModifier modifier) {
            if (plusModifierHashSet.Remove(modifier)) {
                totalPlus -= modifier.Value;
                return true;
            } else return false;
        }

        private readonly List<FloatModifier> multiplyModifierHashSet = new List<FloatModifier>();
        private float totalMultiply = 1f;
        public IEnumerable<FloatModifier> GetMultiplyModifiers => multiplyModifierHashSet;
        public void AddMultiplyModifier(FloatModifier modifier) {
            multiplyModifierHashSet.Add(modifier);
            totalMultiply += modifier.Value;
        }
        public bool RemoveMultiplyModifier(FloatModifier modifier) {
            if (multiplyModifierHashSet.Remove(modifier)) {
                totalMultiply -= modifier.Value;
                return true;
            } else return false;
        }

        public float GetModified(float original) {
            return (original + totalPlus) * totalMultiply;
        }
        public double GetModified(double original) {
            return (original + totalPlus) * totalMultiply;
        }

        public IEnumerable<IModifierSource> GetPlusModifierSources => from FloatModifier modifier in plusModifierHashSet select modifier.Source; 
        public IEnumerable<IModifierSource> GetMultiplyModifierSources => from FloatModifier modifier in multiplyModifierHashSet select modifier.Source; 
    }


}
