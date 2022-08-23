using Enigma.GameWPF.Visual.Settings.Graphics;
using System;
using System.Globalization;
using System.Windows;
using Enigma.GameWPF;

namespace Enigma.GameWPF {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            Settings.Interface.ReadJson();
            if (!Settings.Interface.UICulture.UseSystemUICulture) {
                CultureInfo.CurrentUICulture = Settings.Interface.UICulture.SelectedCulture;
            }
            EntityLoader.Load();
            InitializeComponent();
            Settings.Graphics.ReadJson();
            ApplyScreenRatio(Settings.Graphics.ScreenRatio);
            ApplyFullScreen(Settings.Graphics.FullScreen);
        }

        public Uri MainMenuPageUri => Visual.MainMenuPage.uri;

        public void ApplyScreenRatio(Settings.ScreenRatio screenRatio) {
            frame.Height = screenRatio.Height * 50;
            frame.Width = screenRatio.Width * 50;
        }
        public void ApplyFullScreen(bool isFullScreen) {
            if (isFullScreen) {
                WindowState = WindowState.Maximized;
                WindowStyle = WindowStyle.None;
            } else {
                WindowStyle = WindowStyle.SingleBorderWindow;
            }
        }
    }
}
