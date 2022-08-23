using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Enigma.GameWPF.Visual.Settings.Graphics {
    public class ScreenRatiosViewModel : ViewModel {
        public ScreenRatiosViewModel() { }
        public IReadOnlyList<GameWPF.Settings.ScreenRatio> ScreenRatios => GameWPF.Settings.ScreenRatio.SupportedScreenRatios;
        public GameWPF.Settings.ScreenRatio SelectedScreenRatio {
            get => GameWPF.Settings.Graphics.ScreenRatio;
            set {
                GameWPF.Settings.Graphics.ScreenRatio = value;
                NotifyPropertyChanged();
            }
        }

        public void Apply() {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.ApplyScreenRatio(SelectedScreenRatio);
        }
    }
}