using Enigma.GameWPF.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Enigma.GameWPF.Input {
    public partial class InputBindingManager {
        public InputBindingManager() {
            InputBindingJsonObject inputBindingJsonObject = InputBindingJsonObject.Read();
            foreach (InputAction inputAction in builtInInputActions) {
                string input_str = inputBindingJsonObject.BuiltInInputActionBindings[inputAction.Id];
                KeyOrMouseButton? input = KeyOrMouseButton.Parse(input_str);
                actionToInputDictionary.Add(inputAction, input);
                if (input != null) inputToActionDictionary.Add(input.Value, inputAction);
            }
            for (int i = 0; i < inputBindingJsonObject.SelectAbilityInputActionBindings.Length; i++) {
                string str = inputBindingJsonObject.SelectAbilityInputActionBindings[i];
                KeyOrMouseButton? input = KeyOrMouseButton.Parse(str);
                InputAction inputAction = NewSelectAbilityInputAction(i);
                selectAbilityInputActions.Add(inputAction);
                actionToInputDictionary.Add(inputAction, input);
                if (input != null) inputToActionDictionary.Add(input.Value, inputAction);
            }
        }
        private readonly Dictionary<InputAction, KeyOrMouseButton?> actionToInputDictionary = new Dictionary<InputAction, KeyOrMouseButton?>();
        public IReadOnlyDictionary<InputAction, KeyOrMouseButton?> ActionToInputDictionary => actionToInputDictionary;
        private readonly Dictionary<KeyOrMouseButton, InputAction> inputToActionDictionary = new Dictionary<KeyOrMouseButton, InputAction>();
        public IReadOnlyDictionary<KeyOrMouseButton, InputAction> InputToActionDictionary => inputToActionDictionary;

        public static IReadOnlyList<InputAction> BuiltInInputActions => builtInInputActions;

        private readonly List<InputAction> selectAbilityInputActions = new List<InputAction>();
        public IReadOnlyList<InputAction> SelectAbilityInputActions => selectAbilityInputActions;

        private InputAction NewSelectAbilityInputAction(int index) {
            return new InputAction() {
                Id = "SelectAbility" + index.ToString(),
                DownAction = (c, m) => c.AbilityButtonDown(index, m),
                UpAction = (c, m) => c.AbilityButtonUp(index, m)
            };
        }

        public void InvokeInputDown(KeyOrMouseButton input, Controller controller, Point mousePosition = default) {
            InputAction action;
            if (InputToActionDictionary.TryGetValue(input, out action)) {
                action.DownAction(controller, mousePosition);
            }
        }
        public void InvokeInputHold(KeyOrMouseButton input, Controller controller, Point mousePosition = default) {
            InputAction action;
            if (InputToActionDictionary.TryGetValue(input, out action)) {
                action.HoldAction(controller, mousePosition);
            }
        }
        public void InvokeInputUp(KeyOrMouseButton input, Controller controller, Point mousePosition = default) {
            InputAction action;
            if (InputToActionDictionary.TryGetValue(input, out action)) {
                action.UpAction(controller, mousePosition);
            }
        }
    }
}
