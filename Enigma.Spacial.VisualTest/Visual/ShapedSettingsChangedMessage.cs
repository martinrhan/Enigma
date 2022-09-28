using Enigma.Spacial.TestWPF.Models;

namespace Enigma.Spacial.TestWPF.Visual {

    public class ShapedObjectModelChangedMessage {
        public TestSpace TestSpace { get; init; }
        public IShapedObject ShapedObject { get; init; }
    }
}