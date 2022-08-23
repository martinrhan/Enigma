using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Enigma.Common.WPF {
    /// <summary>
    /// Interaction logic for FormatedView.xaml
    /// </summary>
    public partial class FormatedView : ContentControl {
        public FormatedView() {
            InitializeComponent();
        }

        public string Format {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }
        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.Register("Format", typeof(string), typeof(FormatedView), new PropertyMetadata(null));
    }

    [ValueConversion(typeof(IFormattable), typeof(string))]
    public class FormatConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is IFormattable formattable) {
                return formattable.ToString(parameter as string, null);
            } else {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is IFormattable formattable) {
                return formattable.ToString(parameter as string, null);
            } else {
                return null;
            }
        }
    }
}
