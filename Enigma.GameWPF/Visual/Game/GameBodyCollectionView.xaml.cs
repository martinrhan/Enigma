
namespace Enigma.GameWPF.Visual.Game {
    public partial class GameBodyCollectionView : View<GameBodyCollectionViewModel> {
        public GameBodyCollectionView() {
            InitializeComponent();
        }

        private void view_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            if (ViewModel != null) {
                ViewModel.View = this;
            }
        }
    }
}
