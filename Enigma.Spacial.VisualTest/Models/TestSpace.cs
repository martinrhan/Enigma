using Enigma.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
