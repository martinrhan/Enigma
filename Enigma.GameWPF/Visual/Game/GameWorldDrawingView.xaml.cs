using System.Windows.Media;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System;
using System.Windows.Media.Media3D;
using DependencyPropertyGenerator;

namespace Enigma.GameWPF.Visual.Game {
    [DependencyProperty<GameWorldDrawingViewModel>("ViewModel")]
    public partial class GameWorldDrawingView : FrameworkElement {
        public GameWorldDrawingView() {
            InitializeComponent();
            visuals = new VisualCollection(this);
        }
        private VisualCollection visuals;

        protected override int VisualChildrenCount => visuals.Count;
        protected override System.Windows.Media.Visual GetVisualChild(int index) {
            return visuals[index];
        }

        private void view_Loaded(object sender, RoutedEventArgs e) {
            if (ViewModel == null) return;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            GameWorldDrawingViewModel viewModel = (GameWorldDrawingViewModel)sender;
            int i = 0;
            double scaleX = this.ActualWidth / viewModel.CameraWidth;
            double scaleY = this.ActualHeight / viewModel.CameraHeight;
            foreach (var gbvm in viewModel.GameBodyViewModels) {
                if (visuals.Count == i) {
                    visuals.Add(new DrawingVisual());
                }
                DrawingContext drawingContext = ((DrawingVisual)visuals[i]).RenderOpen();
                drawingContext.PushTransform(new ScaleTransform(scaleX, scaleY));
                drawingContext.DrawEllipse(Brushes.LightBlue, null, new(gbvm.CenterX, viewModel.CameraHeight - gbvm.CenterY), gbvm.Radius, gbvm.Radius);
                drawingContext.Close();
                i++;
            }
            while (visuals.Count > i) {
                visuals.RemoveAt(visuals.Count - 1);
            }
        }
    }
}
