using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Enigma.GameWPF.Input;

namespace Enigma.GameWPF.Visual.Settings.Control {
    public class InputBindingsViewModel {
        public InputBindingsViewModel() {
            var jsonObject = GameWPF.Settings.InputBindingJsonObject.Read();
            BuiltInInputActionViewModels =
                jsonObject.BuiltInInputActionBindings.Select(p => new InputActionViewModel(p.Key, p.Value)).ToArray();
            SelectAbilityInputActionViewModels = new ObservableCollection<InputActionViewModel>(
                jsonObject.SelectAbilityInputActionBindings.Select((str_key, i) => new InputActionViewModel("SelectAbility" + i.ToString(), str_key)));
        }
        public InputActionViewModel[] BuiltInInputActionViewModels { get; }
        public ObservableCollection<InputActionViewModel> SelectAbilityInputActionViewModels { get; }

        public void AddSelectAbilityInputAction() {
            SelectAbilityInputActionViewModels.Add(new InputActionViewModel("SelectAbility" + SelectAbilityInputActionViewModels.Count.ToString(), null));
        }
        public void RemoveSelectAbilityInputAction(int index) {
            int i = index;
            while (index + 1 < SelectAbilityInputActionViewModels.Count) {
                SelectAbilityInputActionViewModels[i].BindedInput = SelectAbilityInputActionViewModels[i + 1].BindedInput;
            }
            SelectAbilityInputActionViewModels.RemoveAt(SelectAbilityInputActionViewModels.Count - 1);
        }

        public void Write() {
            var jsonObject = new GameWPF.Settings.InputBindingJsonObject();
            jsonObject.BuiltInInputActionBindings = BuiltInInputActionViewModels.ToDictionary(vm => vm.Name, vm => vm.BindedInput.ToString());
            jsonObject.SelectAbilityInputActionBindings = SelectAbilityInputActionViewModels.Select(vm => vm.BindedInput.ToString()).ToArray();
            jsonObject.Write();
        }
    }
}
