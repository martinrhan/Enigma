using Enigma.Common;
using ExtendedWPF;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Enigma.Spacial.TestWPF.Visual {
    public partial class ShapedSettingsView : ContentControl {
        public ShapedSettingsView() {
            InitializeComponent();
        }

        public int SelectedShapedTypeIndex {
            get { return (int)GetValue(SelectedShapedTypeIndexProperty); }
            set { SetValue(SelectedShapedTypeIndexProperty, value); }
        }
        public static readonly DependencyProperty SelectedShapedTypeIndexProperty =
            DependencyProperty.Register("SelectedShapedTypeIndex", typeof(int), typeof(ShapedSettingsView), new PropertyMetadata(0, SelectedShapedTypeIndexChanged));
        private static Func<IShapedObject>[] funcs = new Func<IShapedObject>[] {
            () => new CircleShapedObject(default, 10),
            () => new RectangleShapedObject(10,10)
        };
        private static void SelectedShapedTypeIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((ShapedSettingsView)d).Model.Object = funcs[(int)e.NewValue]();
        }

        public Wrapper<IShapedObject> Model { get; private set; }
        public void AssignModel(Wrapper<IShapedObject> model) {
            Model = model;
        }
    }
}
