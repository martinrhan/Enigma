using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enigma.GameWPF.Input;

namespace Enigma.GameWPF.Visual.Settings {
    public class ControlViewModel : ViewModel {
        public ControlViewModel() {
            InputBindingsViewModel = new Control.InputBindingsViewModel();
        }
        public Control.InputBindingsViewModel InputBindingsViewModel { get; }
        public void Write() {
            InputBindingsViewModel.Write();
        }
    }
}
