using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Enigma.Game;

namespace Enigma.GameWPF.Visual.Game {
    public static class AbilityIconView {
        private static readonly Dictionary<string, Func<ContentControl>> dictionary = new Dictionary<string, Func<ContentControl>>();

        public static ContentControl New(string Id) => dictionary[Id]();

        public static void Add(Type t) {
            if (!t.IsAssignableTo(typeof(IView<AbilityIconViewModel>))) {
                throw new ArgumentException("The parameter's type is not assignable to IView<AbilityIconViewModel>");
            }
            string name = t.Name;
            if (!name.EndsWith("IconView")) {
                throw new ArgumentException("The parameter's type is not ending with IconView");
            }
            dictionary.Add(name.Substring(0, name.Length - 8), ()=> Activator.CreateInstance(t) as ContentControl);
        }
    }
}
