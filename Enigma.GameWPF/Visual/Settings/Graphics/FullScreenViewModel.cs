using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Enigma.GameWPF.Visual.Settings.Graphics {
    public class FullScreenViewModel : ViewModel {
        public FullScreenViewModel() {
        }
        public bool IsFullScreen {
            get => GameWPF.Settings.Graphics.FullScreen;
            set {
                GameWPF.Settings.Graphics.FullScreen = value;
                NotifyPropertyChanged();
            }
        }

        public void Apply() {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.ApplyFullScreen(IsFullScreen);
        }
    }
}
