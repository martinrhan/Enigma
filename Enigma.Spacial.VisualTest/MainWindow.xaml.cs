using Enigma.Spacial.TestWPF.Models;
using Enigma.Spacial.TestWPF.Visual;
using System.Windows;

namespace Enigma.Spacial.TestWPF {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            TestSpaceViewModel = new TestSpaceViewModel();
            InitializeComponent();
            TestSpaceViewModel.AssignModel(new TestSpace());
        }

        public TestSpaceViewModel TestSpaceViewModel { get; }
    }
}