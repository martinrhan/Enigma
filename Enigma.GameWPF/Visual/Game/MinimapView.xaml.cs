namespace Enigma.GameWPF.Visual.Game {
    public partial class MinimapView : View<MinimapViewModel> {
        public MinimapView() {
            InitializeComponent();
        }

        private void view_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            if (ViewModel != null) {
                ViewModel.View = this;
            }
        }
    }
}
