using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Enigma.Common;

namespace Enigma.GameWPF.Input {
    public class InputAction {
        public string Id { get; init; }
        public Action<Controller, Point> DownAction { get; init; }
        public Action<Controller, Point> HoldAction { get; init; }
        public Action<Controller, Point> UpAction { get; init; }
    }
}
