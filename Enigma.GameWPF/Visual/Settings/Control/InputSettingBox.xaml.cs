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
using Enigma.GameWPF.Input;

namespace Enigma.GameWPF.Visual.Settings.Control {
    /// <summary>
    /// Interaction logic for KeyView.xaml
    /// </summary>
    public partial class InputSettingBox : UserControl {
        public InputSettingBox() {
            InitializeComponent();
        }

        public KeyOrMouseButton? KeyOrMouseButton {
            get { return (KeyOrMouseButton?)GetValue(KeyOrMouseButtonProperty); }
            set { SetValue(KeyOrMouseButtonProperty, value); }
        }
        public static readonly DependencyProperty KeyOrMouseButtonProperty =
            DependencyProperty.Register("KeyOrMouseButton", typeof(KeyOrMouseButton?), typeof(InputSettingBox), new PropertyMetadata());

        public string ListeningHint {
            get { return (string)GetValue(ListeningHintProperty); }
            set { SetValue(ListeningHintProperty, value); }
        }
        public static readonly DependencyProperty ListeningHintProperty =
            DependencyProperty.Register("ListeningHint", typeof(string), typeof(InputSettingBox), new PropertyMetadata());

        public bool IsListeningInput {
            get { return (bool)GetValue(IsListeningInputProperty); }
            set { SetValue(IsListeningInputProperty, value); }
        }
        public static readonly DependencyProperty IsListeningInputProperty =
            DependencyProperty.Register("IsListeningInput", typeof(bool), typeof(InputSettingBox), new PropertyMetadata(false));

        private void Button_Click(object sender, RoutedEventArgs e) {
            IsListeningInput = true;
            CaptureMouse();
        }

        private void userControl_KeyUp(object sender, KeyEventArgs e) {
            if (IsListeningInput) {
                if (e.Key != Key.Escape) {
                    KeyOrMouseButton = new KeyOrMouseButton(e.Key);
                }
                IsListeningInput = false;
                ReleaseMouseCapture();
            }
        }

        private void userControl_MouseUp(object sender, MouseButtonEventArgs e) {
            if (IsListeningInput) {
                KeyOrMouseButton = new KeyOrMouseButton(e.ChangedButton);
                IsListeningInput = false;
                ReleaseMouseCapture();
            }
        }
    }
}
