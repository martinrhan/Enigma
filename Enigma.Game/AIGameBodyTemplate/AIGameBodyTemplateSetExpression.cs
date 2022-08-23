using System;
using System.Collections.Generic;
using System.Linq;

namespace Enigma.Game {
    public class AIGameBodyTemplateSetExpression{
        private AIGameBodyTemplateSetExpression() {
        }
        [SourceGenerator.JsonConstructor]
        public static AIGameBodyTemplateSetExpression Parse(string expressionString) {
            if (expressionString.Length == 0) throw new ArgumentException("The string is empty.");
            string[] strings = GetPartialStrings(expressionString).ToArray();
            if (strings.Length == 1) {
                switch (strings[0]) {
                    case "u":
                        return new AIGameBodyTemplateSetExpression() {
                            SetOperation = SetOperation.Union
                        };
                    case "n":
                        return new AIGameBodyTemplateSetExpression() {
                            SetOperation = SetOperation.Intersection
                        };
                    case "d":
                        return new AIGameBodyTemplateSetExpression() {
                            SetOperation = SetOperation.Difference
                        };
                    default:
                        return new AIGameBodyTemplateSetExpression() {
                            Set = AIGameBodyTemplateSet.GetExistingOrNew(strings[0])
                        };
                }
            } else {
                return new AIGameBodyTemplateSetExpression() {
                    SubExpressions = strings.Select(str => Parse(str)).ToArray()
                };
            }
        }
        private static IEnumerable<string> GetPartialStrings(string str) {
            int i = 0;
            do {
                i = FindFirst(str, i, c => c != ' ');
                if (i == -1) break;
                if (str[i] == '(') {
                    int bracketCloseIndex = FindBracketClose(str, i);
                    yield return str[(i + 1)..bracketCloseIndex];
                    i = bracketCloseIndex + 1;
                } else {
                    int setIdEndIndex = FindFirst(str, i, c => c == ' ' || c == '(');
                    if (setIdEndIndex == -1) {
                        yield return str[i..];
                        break;
                    } else {
                        yield return str[i..setIdEndIndex];
                        i = setIdEndIndex;
                    }
                }
            } while (i < str.Length);
        }
        private static int FindFirst(string str, int startIndex, Predicate<char> predicate) {
            for (int i = startIndex; i < str.Length; i++) {
                if (predicate(str[i])) {
                    return i;
                }
            }
            return -1;
        }
        private static int FindBracketClose(string str, int startIndex) {
            if (str[startIndex] != '(') {
                throw new ArgumentException("The string \"" + str + "\" does not start with '('");
            }
            int bracketCount = 0;
            for (int i = startIndex + 1; i < str.Length; i++) {
                switch (str[i]) {
                    case '(':
                        bracketCount += 1;
                        break;
                    case ')':
                        if (bracketCount == 0) {
                            return i;
                        } else {
                            bracketCount -= 1;
                        }
                        break;
                }
            }
            throw new ArgumentException("Bracket is not closed in string \"" + str + "\"");
        }

        public AIGameBodyTemplateSet Set { get; init; }
        public SetOperation SetOperation { get; init; }
        public AIGameBodyTemplateSetExpression[] SubExpressions { get; init; }

        public override string ToString() {
            if (Set != null) {
                return Set.Id;
            }
            if (SetOperation != SetOperation.None) {
                return SetOperation.ToString();
            }
            string result = "";
            foreach (AIGameBodyTemplateSetExpression expr in SubExpressions) {
                string str = expr.ToString();
                if (expr.SubExpressions != null) {
                    str = "(" + str + ")";
                }
                result = result + " " + expr.ToString();
            }
            return result;
        }
    }
    public enum SetOperation { None, Union, Intersection, Difference }
}
