using Enigma.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Spacial.TestWPF.Models {
    public class TestSpace {
        public Wrapper<IShapedObject> ShapedObjectAWrapper { get; } = new Wrapper<IShapedObject>();
        public Wrapper<IShapedObject> ShapedObjectBWrapper { get; } = new Wrapper<IShapedObject>();
    }
}
