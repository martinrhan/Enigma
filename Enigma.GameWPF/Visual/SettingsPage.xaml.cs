using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Enigma.GameWPF.Visual {
    /// <summary>
    /// Interaction logic for Setting.xaml
    /// </summary>
    public partial class SettingsPage : Page {
        public SettingsPage() {
            ControlViewModel = new Settings.ControlViewModel();
            InterfaceViewModel = new Settings.InterfaceViewModel();
            GraphicsViewModel = new Settings.GraphicsViewModel();
            InitializeComponent();
        }
        public static readonly Uri uri = new Uri("Visual/SettingsPage.xaml", UriKind.Relative);

        private void Button_Back_Click(object sender, RoutedEventArgs e) {
            NavigationService.GoBack();
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e) {
            ControlViewModel.Write();
            InterfaceViewModel.Apply();
            GraphicsViewModel.Apply();
            GameWPF.Settings.Interface.WriteJson();
            GameWPF.Settings.Graphics.WriteJson();
            NavigationService.Refresh();
        }

        public Settings.ControlViewModel ControlViewModel { get; }
        public Settings.GraphicsViewModel GraphicsViewModel { get;}
        public Settings.InterfaceViewModel InterfaceViewModel { get; }
    }
}
