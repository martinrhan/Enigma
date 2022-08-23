using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Enigma.GameWPF.Visual.Game {
    /// <summary>
    /// Interaction logic for SimpleProgressBar.xaml
    /// </summary>
    public partial class SimpleProgressBar : UserControl {
        public SimpleProgressBar() {
            InitializeComponent();
            eventHandler = new EventHandler(OnProportionChanged);
        }

        public double Proportion {
            get { return (double)GetValue(ProportionProperty); }
            set { SetValue(ProportionProperty, value); }
        }
        public static readonly DependencyProperty ProportionProperty =
            DependencyProperty.Register("Proportion", typeof(double), typeof(SimpleProgressBar), new PropertyMetadata());

        public Thickness RectangleMargin {
            get { return (Thickness)GetValue(RectangleMarginProperty); }
            private set { SetValue(RectangleMarginProperty, value); }
        }
        public static readonly DependencyProperty RectangleMarginProperty =
            DependencyProperty.Register("RectangleMargin", typeof(Thickness), typeof(SimpleProgressBar), new PropertyMetadata());

        private readonly EventHandler eventHandler;
        private void OnProportionChanged(object sender, EventArgs e) {
            RectangleMargin = new Thickness(
                Proportion * ActualWidth
                ,0,0,0);
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            var pd = DependencyPropertyDescriptor.FromProperty(ProportionProperty, typeof(TextBox));
            pd.AddValueChanged(this, eventHandler);
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e) {
            var pd = DependencyPropertyDescriptor.FromProperty(ProportionProperty, typeof(TextBox));
            pd.RemoveValueChanged(this, eventHandler);
        }
    }
}
