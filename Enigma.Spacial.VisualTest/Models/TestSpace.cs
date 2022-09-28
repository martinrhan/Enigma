namespace Enigma.Spacial.TestWPF.Models {

    public class TestSpace : Spacial {

        public TestSpace() {
            Width = 400;
            Height = 300;
        }

        public IShapedObject ShapedObjectA { get; set; }
        public IShapedObject ShapedObjectB { get; set; }
    }
}