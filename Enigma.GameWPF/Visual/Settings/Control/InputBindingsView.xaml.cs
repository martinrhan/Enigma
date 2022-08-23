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

namespace Enigma.GameWPF.Visual.Settings.Control {
    /// <summary>
    /// Interaction logic for KeyBindingView.xaml
    /// </summary>
    public partial class InputBindingsView : View<InputBindingsViewModel> {
        public InputBindingsView() {
            InitializeComponent();
        }

        private void Button_RemoveInputAction_Click(object sender, RoutedEventArgs e) {
            InputActionViewModel inputActionViewModel = (sender as Button).DataContext as InputActionViewModel;
            ViewModel.SelectAbilityInputActionViewModels.Remove(inputActionViewModel);
        }

        private void Button_AddInputAction_Click(object sender, RoutedEventArgs e) {
            InputActionViewModel inputActionViewModel = new InputActionViewModel("SelectAbility" + ViewModel.SelectAbilityInputActionViewModels.Count.ToString(), null);
            ViewModel.SelectAbilityInputActionViewModels.Add(inputActionViewModel);
        }
    }
}
