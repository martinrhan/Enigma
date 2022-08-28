using Enigma.Common.Math;
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

namespace Enigma.Spacial.TestWPF.Visual {
    /// <summary>
    /// Interaction logic for Vector2InputBox.xaml
    /// </summary>
    public partial class Vector2InputBox : UserControl {
        public Vector2InputBox() {
            InitializeComponent();
        }

        public Vector2 Vector2 {
            get { return (Vector2)GetValue(Vector2Property); }
            set { SetValue(Vector2Property, value); }
        }
        public static readonly DependencyProperty Vector2Property =
            DependencyProperty.Register("Vector2", typeof(Vector2), typeof(Vector2InputBox), new FrameworkPropertyMetadata(Vector2.Zero, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Vector2PropertyChanged));
        private static void Vector2PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            Vector2InputBox inputBox = (Vector2InputBox)sender;
            Vector2 vector2 = (Vector2)e.NewValue;
            inputBox.X = vector2.X;
            inputBox.Y = vector2.Y;
        }

        public double X {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(Vector2InputBox), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, XPropertyChanged));
        private static void XPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            Vector2InputBox inputBox = (Vector2InputBox)sender;
            double x = (double)e.NewValue;
            inputBox.Vector2 = new Vector2(x, inputBox.Vector2.Y);
        }

        public double Y {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(Vector2InputBox), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, YPropertyChanged));
        private static void YPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            Vector2InputBox inputBox = (Vector2InputBox)sender;
            double y = (double)e.NewValue;
            inputBox.Vector2 = new Vector2(inputBox.Vector2.X, y);
        }
    }
}
