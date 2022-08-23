using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.GameWPF.Visual.Settings.Interface {
    public class LanguagesViewModel : Enigma.GameWPF.Visual.ViewModel {
        public LanguagesViewModel() { }

        public IReadOnlyList<CultureInfo> SupportedCultures => GameWPF.Settings.UICulture.SupportedCultures;

        public bool UseSystemUICulture {
            get => GameWPF.Settings.Interface.UICulture.UseSystemUICulture;
            set {
                GameWPF.Settings.Interface.UICulture.UseSystemUICulture = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(CanSelectCulture));
            }
        }
        public bool CanSelectCulture => !UseSystemUICulture;
        public CultureInfo SelectedCulture {
            get => GameWPF.Settings.Interface.UICulture.SelectedCulture;
            set {
                GameWPF.Settings.Interface.UICulture.SelectedCulture = value;
                NotifyPropertyChanged();
            }
        }

        public void Apply() {
            if (UseSystemUICulture) return;
            CultureInfo.CurrentUICulture = SelectedCulture;
        }
    }
}
