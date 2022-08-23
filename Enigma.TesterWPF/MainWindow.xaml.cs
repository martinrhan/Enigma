using System;
using System.Windows;
using Enigma.Spacial;

namespace Enigma.TesterWPF {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            BVHTreeViewModel = new ViewModels.BVHTreeViewModel(new BVHTree<AABBObject>());
        }

        public ViewModels.BVHTreeViewModel BVHTreeViewModel {
            get { return (ViewModels.BVHTreeViewModel)GetValue(BVHTreeViewModelProperty); }
            set { SetValue(BVHTreeViewModelProperty, value); }
        }
        public static readonly DependencyProperty BVHTreeViewModelProperty =
            DependencyProperty.Register(nameof(BVHTreeViewModel), typeof(ViewModels.BVHTreeViewModel), typeof(MainWindow), new PropertyMetadata(null));

        private void button_Add_Click(object sender, RoutedEventArgs e) {
            BVHTreeViewModel.Add(new AABBObject() { AABB = AABB.CreateRandom(500d) });
        }

        private void button_Foreach_Click(object sender, RoutedEventArgs e) {
            BVHTreeViewModel.ForEachAABBIntersection(p => { });
        }
    }
}
