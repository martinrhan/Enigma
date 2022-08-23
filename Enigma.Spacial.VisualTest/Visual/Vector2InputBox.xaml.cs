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
            DependencyProperty.Register("Vector2", typeof(Vector2), typeof(Vector2InputBox), new PropertyMetadata());

        private void textBoxX_TextChanged(object sender, TextChangedEventArgs e) {
            Vector2 += new Vector2(double.Parse(textBoxX.Text),0);
        }
        private void textBoxY_TextChanged(object sender, TextChangedEventArgs e) {
            Vector2 += new Vector2(0, double.Parse(textBoxY.Text));
        }
    }
}
