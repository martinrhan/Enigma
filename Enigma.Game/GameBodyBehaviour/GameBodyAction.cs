using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Game {
    public abstract class GameBodyAction {
        internal GameBodyAction() {
        }
        public bool IsStarted { get; private protected set; } = false;
        public bool IsCompleted { get; private protected set; } = false;
    }
}
