namespace Enigma.GameWPF.Visual.Settings.Interface {
    /// <summary>
    /// Interaction logic for LanguagesView.xaml
    /// </summary>
    public partial class LanguagesView :View<LanguagesViewModel> {
        public LanguagesView() {
            ViewModel = new LanguagesViewModel();
            InitializeComponent();
        }
    }
}
