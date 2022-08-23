using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Enigma.GameWPF.Visual {
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenuPage : Page {
        public MainMenuPage() {
            InitializeComponent();
        }
        public static readonly Uri uri = new Uri("Visual/MainMenuPage.xaml", UriKind.Relative);

        private void Button_ContinueGame_Click(object sender, RoutedEventArgs e) {

        }

        private void Button_NewGame_Click(object sender, RoutedEventArgs e) {
            NavigationService.Navigate(NewGamePage.uri);
        }

        private void Button_Setting_Click(object sender, RoutedEventArgs e) {
            NavigationService.Navigate(SettingsPage.uri);
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }
    }
}
