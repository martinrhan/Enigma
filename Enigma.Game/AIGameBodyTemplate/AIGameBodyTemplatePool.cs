using Enigma.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Game {
    public class AIGameBodyTemplatePool {
        public AIGameBodyTemplateSetExpression SetExpression { get; init; }
        private AIGameBodyTemplate[] set;
        public IReadOnlyList<AIGameBodyTemplate> Set {
            get {
                if (set == null) {
                    set = GetSet(SetExpression).ToArray();
                }
                return set;
            }
        }
        public int RollCount { get; init; }

        private IReadOnlySet<AIGameBodyTemplate> GetSet(AIGameBodyTemplateSetExpression setExpression) {
            if (setExpression.Set != null) {
                return setExpression.Set.Content;
            } else if (setExpression.SubExpressions != null) {
                if (setExpression.SubExpressions.Length % 2 == 0) {
                    throw new ArgumentException("There are incorrect amount of expressions, it should be odd number.");
                }
                HashSet<AIGameBodyTemplate> set = new HashSet<AIGameBodyTemplate>(GetSet(setExpression.SubExpressions[0]));
                for (int i = 1; i < setExpression.SubExpressions.Length; i++) {
                    switch (setExpression.SubExpressions[i].SetOperation) {
                        case SetOperation.None:
                            throw new ArgumentException("The AIGameBodyTemplateSetExpression at index " + i + " is not a set operation.");
                        case SetOperation.Union:
                            set.UnionWith(GetSet(setExpression.SubExpressions[i + 1]));
                            return set;
                        case SetOperation.Intersection:
                            set.IntersectWith(GetSet(setExpression.SubExpressions[i + 1]));
                            return set;
                        case SetOperation.Difference:
                            set.ExceptWith(GetSet(setExpression.SubExpressions[i + 1]));
                            return set;
                    }
                }
            }
            throw new ArgumentException();
        }
        public IEnumerable<AIGameBodyTemplate> RollAIGameBodyTemplates() {
            if (Set.Count < RollCount) throw new ArgumentOutOfRangeException(nameof(RollCount), $"There are only {Set.Count} AIGameBodyTemplate in the set but {RollCount} is requested");
            NonDuplicateRandom random = new NonDuplicateRandom(Set.Count);
            foreach (int i in random.GetRandomIntegers(RollCount)) {
                yield return Set[i];
            }
        }
    }
}
