using Enigma.GameWPF.Input;

namespace Enigma.GameWPF.Visual.Settings.Control {
    public class InputActionViewModel : ViewModel {
        public InputActionViewModel(string name, string bindedInput) {
            Name = name;
            this.bindedInput = bindedInput == null? null: KeyOrMouseButton.Parse(bindedInput);
        }

        public string Name { get; }

        private KeyOrMouseButton? bindedInput;
        public KeyOrMouseButton? BindedInput {
            get => bindedInput; 
            set {
                bindedInput = value;
                NotifyPropertyChanged();
            }
        }
    }
}
