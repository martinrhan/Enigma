using System;
using System.Collections.Generic;
using System.Text;

namespace Enigma.Game.SourceGenerator {
    /// <summary>
    /// This attribute is to be attached to template classes that are part of other template and don't have Id nor static Dictionary.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DontUseIdAttribute : Attribute {
    }
}
