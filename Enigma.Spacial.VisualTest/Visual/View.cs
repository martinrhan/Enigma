using System.Windows;
using System.Windows.Controls;

namespace Enigma.Spacial.TestWPF.Visual {

    public class View<T> : ContentControl, IView<T> {

        public T ViewModel {
            get { return (T)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(T), typeof(View<T>), new PropertyMetadata());
    }

    public interface IView<out T> {
        public T ViewModel { get; }
    }
}