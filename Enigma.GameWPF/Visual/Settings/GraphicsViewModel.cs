using Enigma.GameWPF.Visual.Settings.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.GameWPF.Visual.Settings {
    public class GraphicsViewModel : ViewModel {
        public GraphicsViewModel() {
            ScreenRatiosViewModel = new ScreenRatiosViewModel();
            FullScreenViewModel = new FullScreenViewModel();
        }
        public ScreenRatiosViewModel ScreenRatiosViewModel { get; }
        public FullScreenViewModel FullScreenViewModel { get; }
        public void Apply() {
            ScreenRatiosViewModel.Apply();
            FullScreenViewModel.Apply();
        }
    }
}
