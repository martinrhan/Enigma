﻿using System;
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
    /// Interaction logic for NewGame.xaml
    /// </summary>
    public partial class NewGamePage : Page {
        public NewGamePage() {
            InitializeComponent();
        }
        public static readonly Uri uri = new Uri("Visual/NewGamePage.xaml", UriKind.Relative);

        private void Button_Back_Click(object sender, RoutedEventArgs e) {
            NavigationService.GoBack();
        }

        private void Button_Go_Click(object sender, RoutedEventArgs e) {
            NavigationService.Navigate(GamePage.uri);
        }
    }
}
