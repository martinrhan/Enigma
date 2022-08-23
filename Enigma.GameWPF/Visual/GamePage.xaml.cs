using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Enigma.GameWPF.Input;
using System.Collections.Generic;
using System;
using Enigma.Game;
using Enigma.GameWPF.Visual.Game;
using System.Windows.Media;
using System.Diagnostics;

namespace Enigma.GameWPF.Visual {
    public partial class GamePage : Page {
        public GamePage() {
            (GameWorld gameWorld, Player player) = GameWorld.New();
            controller = new Controller(player, this);
            inputBindingManager = new InputBindingManager();
            GameWorldViewModel = new GameWorldViewModel(gameWorld, player, inputBindingManager);
            InitializeComponent();
        }
        public static readonly Uri uri = new Uri("Visual/GamePage.xaml", UriKind.Relative);

        public GameWorldViewModel GameWorldViewModel { get; }

        private readonly Controller controller;
        private readonly InputBindingManager inputBindingManager;

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            eventHandler = new EventHandler(CheckMouseInput);
            CompositionTarget.Rendering += eventHandler;
            Focus();
        }
        private void Page_KeyDown(object sender, KeyEventArgs e) {//In WPF, pressing key cause keydown event called repeatedly
            //Debug.WriteLine(e.Key.ToString());
            if (e.IsRepeat) {
                inputBindingManager.InvokeInputHold(new KeyOrMouseButton(e.Key), controller);
            } else {
                inputBindingManager.InvokeInputDown(new KeyOrMouseButton(e.Key), controller);
            }
        }
        private void Page_KeyUp(object sender, KeyEventArgs e) {
            inputBindingManager.InvokeInputUp(new KeyOrMouseButton(e.Key), controller);
        }
        private void Page_MouseDown(object sender, MouseButtonEventArgs e) {//MouseDown event is not called repeatedly even the button is held.
            inputBindingManager.InvokeInputDown(new KeyOrMouseButton(e.ChangedButton), controller, e.GetPosition(this));
        }
        private void Page_MouseUp(object sender, MouseButtonEventArgs e) {
            inputBindingManager.InvokeInputUp(new KeyOrMouseButton(e.ChangedButton), controller, e.GetPosition(this));
        }
        private EventHandler eventHandler;
        private void CheckMouseInput(object sender, EventArgs e) {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                inputBindingManager.InvokeInputHold(new KeyOrMouseButton(MouseButton.Left), controller, Mouse.GetPosition(this));
            if (Mouse.RightButton == MouseButtonState.Pressed)
                inputBindingManager.InvokeInputHold(new KeyOrMouseButton(MouseButton.Right), controller, Mouse.GetPosition(this));
            if (Mouse.MiddleButton == MouseButtonState.Pressed)
                inputBindingManager.InvokeInputHold(new KeyOrMouseButton(MouseButton.Middle), controller, Mouse.GetPosition(this));
            if (Mouse.XButton1 == MouseButtonState.Pressed)
                inputBindingManager.InvokeInputHold(new KeyOrMouseButton(MouseButton.XButton1), controller, Mouse.GetPosition(this));
            if (Mouse.XButton2 == MouseButtonState.Pressed)
                inputBindingManager.InvokeInputHold(new KeyOrMouseButton(MouseButton.XButton2), controller, Mouse.GetPosition(this));
        }
        private void Page_PreviewMouseWheel(object sender, MouseWheelEventArgs e) {
            GameWorldViewModel.Camera.ZoomIn((double)e.Delta / 1000d);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            GameWorldViewModel.Dispose();
            CompositionTarget.Rendering -= eventHandler;
        }
    }
}
