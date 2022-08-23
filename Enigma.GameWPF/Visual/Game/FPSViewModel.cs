using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Enigma.GameWPF.Visual.Game {
    public class FPSViewModel : ViewModel {
        public FPSViewModel() {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
        public int FPS { get; private set; }
        public void PlusOneFrameCount() {
            FPS += 1;
        }
        private readonly DispatcherTimer dispatcherTimer;
        private void dispatcherTimer_Tick(object sender, EventArgs e) {
            NotifyPropertyChanged(nameof(FPS));
            FPS = 0;
        }
    }
}
