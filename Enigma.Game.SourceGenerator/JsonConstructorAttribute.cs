using System;
using System.Collections.Generic;
using System.Text;

namespace Enigma.Game.SourceGenerator {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class JsonConstructorAttribute : Attribute {
    }
}
