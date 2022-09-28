namespace Enigma.GameWPF.Visual.Game {
    public partial class GameWorldView : View<GameWorldViewModel> {
        public GameWorldView() {
            InitializeComponent();
        }

        private void view_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            Focus();
        }
    }
}
