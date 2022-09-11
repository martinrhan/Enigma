using Enigma.Spacial.TestWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Spacial.TestWPF.Visual {
    public class ShapedObjectModelChangedMessage {
        public TestSpace TestSpace { get; init; }
        public IShapedObject ShapedObject { get; init; }
    }
}
