namespace Enigma.GameWPF.Visual.Game {
    public partial class EnemyWaveManagerView : View<EnemyWaveManagerViewModel> {
        public EnemyWaveManagerView() {
            InitializeComponent();
        }

        private void Button_Launch_Click(object sender, System.Windows.RoutedEventArgs e) {
            ViewModel.LaunchWave();
        }
    }
}
