using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Enigma.GameWPF.Input {
    public readonly struct KeyOrMouseButton {
        public KeyOrMouseButton(Key key) {
            this.key = key;
            this.mouseButton = MouseButton.Left;
        }

        public KeyOrMouseButton(MouseButton mouseButton) {
            this.mouseButton = mouseButton;
            this.key = Key.None;
        }

        public readonly Key key;
        public Key Key => key;

        public readonly MouseButton mouseButton;
        public MouseButton MouseButton => mouseButton;

        public bool IsMouseButton => key == Key.None;

        public override string ToString() {
            if (IsMouseButton) {
                return "Mouse" + mouseButton.ToString();
            } else {
                return key.ToString();
            }
        }

        public string LocalizedString {
            get {
                return ToString();
                //return (string)Properties.Settings.Default[ToString()];
            }
        }

        public override int GetHashCode() {
            return HashCode.Combine(key, mouseButton);
        }

        public static KeyOrMouseButton? Parse(string str) {
            if (str == null || str == "") {
                return null;
            } else if (str.StartsWith("Mouse")) {
                str = str.Substring(5);
                return new KeyOrMouseButton(Enum.Parse<MouseButton>(str));
            } else {
                return new KeyOrMouseButton(Enum.Parse<Key>(str));
            }
        }
    }
}
