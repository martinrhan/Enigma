using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.GameWPF.Visual.Settings {
    public class InterfaceViewModel : Enigma.GameWPF.Visual.ViewModel {
        public InterfaceViewModel() {
            LanguagesViewModel = new Interface.LanguagesViewModel();
        }
        public Interface.LanguagesViewModel LanguagesViewModel { get; }
        public void Apply() {
            LanguagesViewModel.Apply();
        }
    }
}
